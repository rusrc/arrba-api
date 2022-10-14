using Arrba.Constants;
using Arrba.ImageLibrary.Exceptions;
using Arrba.ImageLibrary.Json;
using Arrba.ImageLibrary.ModelViews;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Arrba.Exceptions;
using ImgJson = Arrba.ImageLibrary.Json.ImgJson;

//NOTE that on Ubuntu (and other Linuxes) you may need to install some native dependencies
//https://www.hanselman.com/blog/HowDoYouUseSystemDrawingInNETCore.aspx
namespace Arrba.ImageLibrary
{
    public partial class ImgManager
    {
        public ImgManager(string rootPath)
        {
            RootPath = rootPath;
            AdImagesFolder = ImgFolderManager.GetRootImageFolderPath(rootPath);
            LogoPath = ImgFolderManager.GetLogoPath(rootPath);
            BackgroundPath = ImgFolderManager.GetBackgroundImagePath(rootPath);
        }

        public string RootPath { get; set; }
        /// <summary>
        /// Uploads/AdImages
        /// </summary>
        public string AdImagesFolder { get; set; }
        /// <summary>
        /// App_Data/no_image.jpg
        /// </summary>
        public string LogoPath { get; set; }
        /// <summary>
        /// App_Data/backgroud.jpg
        /// </summary>
        public string BackgroundPath { get; set; }

        public ImgJson SaveImages(string uniqueImgFolderName, long categFolderName, ImgJson addToList = null, List<ImgSizeSetting> imgSizeList = null)
        {
            if (string.IsNullOrEmpty(uniqueImgFolderName))
            {
                return new ImgJson
                {
                    CountHistory = 0,
                    ImgJsonOrderList = new List<ImgJsonOrder>()
                };
            }

            // App_Data/tempImages/guid-guid-guid-guid	
            string sourcePath = ImgFolderManager.GetTempUniqueItemFolder(uniqueImgFolderName, this.RootPath);

            // Uploads/AdvImages/1/guid-guid-guid-guid
            string targetPath = Path.Combine(this.AdImagesFolder, categFolderName.ToString(), uniqueImgFolderName);

            string[] files = Directory.GetFiles(sourcePath);
            return SaveImages(files, targetPath, categFolderName.ToString(), addToList, imgSizeList);
        }

        private ImgJson SaveImages(string[] files, string uniqueImgFolderName, string categFolderName, ImgJson addToList = null, List<ImgSizeSetting> imgSizeList = null)
        {
            return this._saveImages(files, uniqueImgFolderName, categFolderName, addToList, imgSizeList);
        }

        private ImgJson _saveImages(object[] files, string uniqueImgFolderName, string categFolderName, ImgJson addToList = null, List<ImgSizeSetting> imgSizeList = null)
        {
            #region ImgSizeList
            // Add more sizes if needed
            imgSizeList = imgSizeList ?? new List<ImgSizeSetting> {
                GetImgSizeSetting(CONSTANT.BIG_FILE_NAME_PREFIX),
                GetImgSizeSetting(CONSTANT.FULL_FILE_NAME_PREFIX),           // 1024x720 - full изображение с огрничением 1024x720
                GetImgSizeSetting(CONSTANT.MIDDLE_FILE_NAME_PREFIX),         // 400x300 - для карточки объявления
                GetImgSizeSetting(CONSTANT.MIDDLE_FILE_NAME_PREFIX2),
                GetImgSizeSetting(CONSTANT.SMALL_FILE_NAME_PREFIX),          // 200x150 - для горячих
                GetImgSizeSetting(CONSTANT.SUPER_SMALL_FILE_NAME_PREFIX)     // 60x45 - для превью в карточке
            };
            #endregion

            var imgJson = addToList ?? new ImgJson { CountHistory = 0, ImgJsonOrderList = new List<ImgJsonOrder>() };
            var folderCategPath = CheckFolderAndCreate(Path.Combine(AdImagesFolder, categFolderName), true);
            var userFolderPath = CheckFolderAndCreate(Path.Combine(AdImagesFolder, folderCategPath, uniqueImgFolderName), true);

            var imgLogo = Image.FromFile(LogoPath); // получаем файл логотипа
            var imgBackground = Image.FromFile(BackgroundPath); // получаем файл фонового изображения

            int maxWidth = imgSizeList.Max(x => x.Width);   // Максимальный размер в настройках
            int maxHeight = imgSizeList.Max(x => x.Height); // Минимальный размер в настройках


            var resultImage = new ResultImage();
            int indexName = 0;

            for (int i = 0; i < files.Length; i++)
            {

                Image resizedImg = null;

                try
                {
                    //if (files[i] is HttpPostedFileBase file)
                    //{
                    //    using (file.InputStream)
                    //    {
                    //        using (var fullImg = Image.FromStream(file.InputStream, useEmbeddedColorManagement: true, validateImageData: false))
                    //        {
                    //            resizedImg = ResizeImage(fullImg, ImageResizeMode.OnMinSide, maxWidth, maxHeight);
                    //        }
                    //    }
                    //}
                    if (files[i] is string filePath)
                    {
                        using (var fullImg = Image.FromFile(filePath, useEmbeddedColorManagement: true))
                        {
                            resizedImg = ResizeImage(fullImg, ImageResizeMode.OnMinSide, maxWidth, maxHeight);
                        }
                    }
                }
                catch
                {
                    resizedImg?.Dispose();
                    continue;
                }

                foreach (var e in imgSizeList)
                {
                    // предварительно уменьшаем изображение 
                    resultImage.bmp = (e.Width == maxWidth) && (e.Height == maxHeight)
                                        ? new Bitmap(ImgManager.ResizeImage(resizedImg, ImageResizeMode.OnMaxSide, e.Width, e.Height))
                                        : new Bitmap(ImgManager.ResizeImage(resizedImg, ImageResizeMode.OnMinSide, e.Width, e.Height));
                    resultImage.imgRightSideShift = resultImage.bmp.Width;
                    resultImage.imgBottomSideShift = resultImage.bmp.Height;

                    #region add container, proccessing

                    var addWaterMark = false;
                    if (e.Width >= DEFAULT_CONTAINER_WIDTH && addWaterMark)
                    {
                        int newLogoWidth;

                        if (e.Width == DEFAULT_CONTAINER_WIDTH)
                        {
                            // получаем изображение, вписанное в контейнер
                            resultImage = ImgManager.CreateImageInContainer(resultImage.bmp, ImageSizeChangeType.Inclose, imgBackground, e.Width, e.Height);
                            newLogoWidth = (int)(e.Width * 0.2);
                        }
                        else
                        {
                            newLogoWidth = (resultImage.bmp.Width < resultImage.bmp.Height)
                                         ? (int)(resultImage.bmp.Height * 0.2)
                                         : (int)(resultImage.bmp.Width * 0.2);
                        }

                        Image imgLogoWatermark = imgLogo;
                        if (imgLogo.Width > newLogoWidth)
                        {
                            double relation = (double)imgLogo.Width / newLogoWidth;
                            int newLogoHeight = (int)(imgLogo.Height / relation);
                            imgLogoWatermark = ImgManager.ResizeLogoImage(imgLogo, new Size(newLogoWidth, newLogoHeight));
                        }

                        resultImage.bmp = ImgManager.CreateWatermark(resultImage, imgLogoWatermark);
                    }
                    else
                    {
                        resultImage = ImgManager.CreateImageInContainer(resultImage.bmp, ImageSizeChangeType.Crop, null, e.Width, e.Height);
                    }
                    #endregion

                    //Получаем последний индекс в названии файла
                    indexName = imgJson.CreateIndexName(i);
                    //Путь и имя файла, чтобы там сохранить его
                    var path = Path.Combine(userFolderPath, $"{indexName}-{e.Width}x{e.Height}.jpg");

                    resultImage.bmp.Save(path, ImageFormat.Jpeg);
                    resultImage.bmp.Dispose();
                }

                resizedImg?.Dispose();
                imgJson.ImgJsonOrderList.Add(new ImgJsonOrder { Order = indexName, IndexName = indexName });
            }

            imgLogo.Dispose();
            imgBackground.Dispose();


            imgJson.CountHistory = imgJson.GetMaxIndexNameInList();

            return imgJson;
        }


        public void DeleteImages(string uniqueImgFolderName)
        {
            try
            {
                var userFolderPath = Path.Combine(this.AdImagesFolder, uniqueImgFolderName);
                if (Directory.Exists(userFolderPath))
                {
                    Directory.Delete(userFolderPath, true);
                }
            }
            catch
            {
                throw new ImageLibraryException($@"Произошла ошибка при удалении фалов
                                       в каталоге пользователя каталог пользователя:
                                       {uniqueImgFolderName}");
            }
        }

        /// <summary>
        /// Формирует объект ImgSizeSetting из аргумента
        /// </summary>
        /// <param name="sizePrefix">Строка должна содержать две цифры с разделителем, иначе получим исключение</param>
        /// <returns>ImgSizeSetting</returns>
        /// <exception cref="ImgManager">ImgApplicationException</exception>
        public static ImgSizeSetting GetImgSizeSetting(string sizePrefix)
        {
            MatchCollection w = new Regex(@"\d{1,4}", RegexOptions.IgnoreCase).Matches(sizePrefix);

            if (w.Count > 2)
            {
                throw new ImageLibraryException(
                    @"Упс! Получено больше 2-х значений, 
                      просто проверьте переданную строку. 
                      Убедитесь, что в строке две цифры 
                      с одним разделителем");
            }

            return new ImgSizeSetting
            {
                Width = int.Parse(w[0].Value),
                Height = int.Parse(w[1].Value)
            };
        }


        /// <summary>
        /// Проверяет есть ли папка, если нет создает её
        /// </summary>
        /// <param name="folderPath">Путь где надо искать папку</param>
        /// <returns></returns>
        public static bool CheckFolderAndCreate(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return Directory.Exists(folderPath);
        }

        /// <summary>
        /// Проверяет есть ли папка, если нет создает её и возврощает путь к этой папке
        /// </summary>
        /// <param name="folderPath">Путь где надо искать папку</param>
        /// <param name="returnPath">Метка, чтобы вернуть может быть true или false</param>
        /// <returns></returns>
        public static string CheckFolderAndCreate(string folderPath, bool returnPath)
        {
            return CheckFolderAndCreate(folderPath) ? folderPath : string.Empty;
        }

        /// <summary>
        /// User For firebase
        /// </summary>
        public static string GetImgPathFireBase(string folderName, ImgJson imgJsonObject, string imgSize = null)
        {
            var fileName = "";

            if (!string.IsNullOrEmpty(folderName) && imgJsonObject.ImgJsonOrderList.Count > 0 && imgJsonObject.ImgJsonOrderList != null)
            {
                ImgJsonOrder defaultImgJson = imgJsonObject
                                    .ImgJsonOrderList
                                    .Where(e => e != null)
                                    .OrderBy(e => e.Order)
                                    .FirstOrDefault();

                const string extension = "jpg";
                int indexName = defaultImgJson?.IndexName ?? 0;

                if (string.IsNullOrEmpty(imgSize))
                {
                    fileName = $"{folderName}_{indexName}.{extension}";
                }
                else
                {
                    // thumb@1920x1080-
                    fileName = $"thumb%40{imgSize}-{folderName}_{indexName}.{extension}";
                }
            }
            //TODO add settings in settigns.json and use injected service
            return string.Format(CONSTANT.FIREBASE_URL, fileName);
        }

        /// <summary>
        /// Form image path 
        /// Uploads/AdvImages/37/2da286c862bd4221a9ebe6d310d0e3df/0-400x300.jpg
        /// </summary>
        /// <param name="categoryName">Folder name of any ad</param>
        /// <param name="uniqueItemFolder"></param>
        /// <param name="imgJsonObject">ImgJson object with history and count</param>
        /// <param name="imgSize">Frefix name for images, use <see cref="CONSTANT"/> with param as 'MIDDLE_FILE_NAME_PREFIX'</param>
        /// <exception cref="BusinessCriticalLogicException">
        ///     ImgJsonObject with type of<see cref="ImgJson"/> can't be null
        /// </exception>
        /// <returns>string</returns>
        public static string GetImgPath(string categoryName, string uniqueItemFolder, ImgJson imgJsonObject, string imgSize = CONSTANT.MIDDLE_FILE_NAME_PREFIX)
        {
            if (imgJsonObject == null)
            {
                throw new BusinessCriticalLogicException($"Check why {nameof(imgJsonObject)} comes with null");
            }

            string imgRootPathResult;
            if (!string.IsNullOrEmpty(uniqueItemFolder) && imgJsonObject.ImgJsonOrderList.Count > 0 && imgJsonObject.ImgJsonOrderList != null)
            {
                ImgJsonOrder defaultImgJson = imgJsonObject
                                    .ImgJsonOrderList
                                    .Where(e => e != null)
                                    .OrderBy(e => e.Order)
                                    .FirstOrDefault();

                const string extension = "jpg";
                int indexMainName = defaultImgJson?.IndexName ?? 0;

                var rootImageFolder = ImgFolderManager.GetRelativeItemFolderPath(categoryName, uniqueItemFolder);
                imgRootPathResult = Path.Combine(rootImageFolder, $"{indexMainName}-{imgSize}.{extension}");
            }
            else
            {
                imgRootPathResult = ImgFolderManager.GetRelativeNoImagePath();
            }
            
            imgRootPathResult = imgRootPathResult.Trim();

            return CONSTANT.ROOT_HOST_HTTPS + "/" + imgRootPathResult;
        }

        public static string GetImgPath(string categFolderName, string folderImgName, string imgJsonString, string imgSize = CONSTANT.MIDDLE_FILE_NAME_PREFIX)
        {
            ImgJson imgJsonObject = ImgJson.Parse(imgJsonString) ?? new ImgJson { CountHistory = 0, ImgJsonOrderList = null };
            return GetImgPath(categFolderName, folderImgName, imgJsonObject, imgSize);
        }

        public static string GetImgPath(long categFolderName, string folderImgName, ImgJson imgJsonObject, string imgSize = CONSTANT.MIDDLE_FILE_NAME_PREFIX)
        {
            return GetImgPath(categFolderName.ToString(), folderImgName, imgJsonObject, imgSize);
        }

        public static Bitmap Blur(Bitmap image, Rectangle rectangle, Int32 blurSize)
        {
            Bitmap blurred = new Bitmap(image.Width, image.Height);

            // make an exact copy of the bitmap provided
            using (Graphics graphics = Graphics.FromImage(blurred))
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

            // look at every pixel in the blur rectangle
            for (Int32 xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
            {
                for (Int32 yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
                {
                    Int32 avgR = 0, avgG = 0, avgB = 0;
                    Int32 blurPixelCount = 0;

                    // average the color of the red, green and blue for each pixel in the
                    // blur size while making sure you don't go outside the image bounds
                    for (Int32 x = xx; (x < xx + blurSize && x < image.Width); x++)
                    {
                        for (Int32 y = yy; (y < yy + blurSize && y < image.Height); y++)
                        {
                            Color pixel = blurred.GetPixel(x, y);

                            avgR += pixel.R;
                            avgG += pixel.G;
                            avgB += pixel.B;

                            blurPixelCount++;
                        }
                    }

                    avgR = avgR / blurPixelCount;
                    avgG = avgG / blurPixelCount;
                    avgB = avgB / blurPixelCount;

                    // now that we know the average for the blur size, set each pixel to that color
                    for (Int32 x = xx; x < xx + blurSize && x < image.Width && x < rectangle.Width; x++)
                    {
                        for (Int32 y = yy; y < yy + blurSize && y < image.Height && y < rectangle.Height; y++)
                        {
                            blurred.SetPixel(x, y, Color.FromArgb(avgR, avgG, avgB));
                        }
                    }


                }
            }

            return blurred;
        }
    }
}
