using Arrba.Constants;
using Arrba.ImageLibrary.ModelViews;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ImgJson = Arrba.ImageLibrary.Json.ImgJson;
using ImgJsonOrder = Arrba.ImageLibrary.Json.ImgJsonOrder;

namespace Arrba.ImageLibrary
{

    public class ImgFolderManager
    {
        public const string UPLOADS_FOLDER = "Uploads";
        public const string ADV_IMAGES = "AdvImages";
        public const string TEMP_IMAGE_FOLDER = "tempImages";

        /// <summary>
        /// Retrun full path of temp unique folder
        /// Example: D:/wwwroot/tempimages/ea762e884c4e4be69eec1b033fb6510c
        /// </summary>
        /// <returns></returns>
        public static DirectoryInfo GenerateTempUniqueItemFolder(string uniqueItemFolder, string wwwrootPath)
        {
            return Directory.CreateDirectory(Path.Combine(wwwrootPath, TEMP_IMAGE_FOLDER, uniqueItemFolder));
        }

        /// <summary>
        /// Retrun full path
        /// D:/App_Data/tempImages/ea762e884c4e4be69eec1b033fb6510c
        /// </summary>
        /// <returns></returns>
        public static string GetTempUniqueItemFolder(string uniqueItemFolder, string wwwrootPath)
        {
            string sourcePath = Path.Combine(wwwrootPath, TEMP_IMAGE_FOLDER, uniqueItemFolder);
            return sourcePath;
        }

        /// <summary>
        /// Retrun relative path
        /// /uploads/AdvImages/1/ea762e884c4e4be69eec1b033fb6510c
        /// </summary>
        /// <returns></returns>
        public static string GetRelativeItemFolderPath(string categoryName, string uniqueItemFolder)
        {
            string rootFolder = Path.Combine(UPLOADS_FOLDER, ADV_IMAGES, categoryName, uniqueItemFolder);
            return rootFolder;
        }

        /// <summary>
        /// Retrun full path
        /// D:/uploads/AdvImages/
        /// </summary>
        /// <returns></returns>
        public static string GetRootImageFolderPath(string wwwrootPath)
        {
            var path = Path.Combine(wwwrootPath, UPLOADS_FOLDER, ADV_IMAGES);
            return path;
        }

        /// <summary>
        /// Retrun relative path
        /// /uploads/no_image.jpg
        /// </summary>
        /// <returns></returns>
        public static string GetRelativeNoImagePath()
        {
            return Path.Combine("content", SETTINGS.NoImageName);
        }

        /// <summary>
        /// Retrun full path
        /// D:/uploads/no_image.jpg
        /// </summary>
        /// <returns></returns>
        public static string GetNoImagePath(string wwwrootPath)
        {
            var path = Path.Combine(wwwrootPath, "content", SETTINGS.NoImageName);
            return path;
        }

        /// <summary>
        /// Retrun full path
        /// D:/uploads/background_image.jpg
        /// </summary>
        /// <returns></returns>
        public static string GetBackgroundImagePath(string wwwrootPath)
        {
            var path = Path.Combine(wwwrootPath, "content", SETTINGS.BackgroundImage);
            return path;
        }

        public static string GetLogoPath(string wwwrootPath)
        {
            var path = Path.Combine(wwwrootPath, "content",  SETTINGS.LogoWaterMarkFile);
            return path;
        }

        /// <summary>
        /// Получает номер в название файла
        /// </summary>
        /// <param name="str">Название файла</param>
        /// <returns>Число</returns>
        public static string GetImgNumber(string str)
        {
            var reg = new Regex("^\\d+", RegexOptions.IgnoreCase);

            return reg.Match(str).Value;
        }

        /// <summary>
        /// Получает название файлов из списка объектов ImgJsonOrder преобразуя в список объектов типа AdImgModelView 
        /// </summary>
        /// <param name="JsonList">Список сортер полученный из json</param>
        /// <returns>список объектов типа AdImgModelView</returns>
        public static List<AdImgDto> GetImages(List<ImgJsonOrder> JsonList)
        {
            //1024x720
            var adImgList = new List<AdImgDto>();

            JsonList.ForEach(e =>
            {
                var indexName = e.IndexName.ToString();
                adImgList.Add(new AdImgDto
                {
                    FullFileName = string.Concat((object) indexName, CONSTANT.FULL_FILE_NAME_PREFIX, ".jpg"),
                    MiddleFileName = string.Concat((object) indexName, CONSTANT.MIDDLE_FILE_NAME_PREFIX, ".jpg"),
                    SmallFileName = string.Concat((object) indexName, CONSTANT.SMALL_FILE_NAME_PREFIX, ".jpg"),
                    SuperSmallFileName = string.Concat((object) indexName, CONSTANT.SUPER_SMALL_FILE_NAME_PREFIX, ".jpg"),
                    ImageStatus = e.Order == 0 ? AdImageStatus.Main : AdImageStatus.NotMain
                });
            });

            return adImgList;
        }

        /// <summary>
        /// Получает название файлов из json строки JsonList преобразуя в список объектов типа AdImgModelView 
        /// </summary>
        /// <param name="JsonList">json строка</param>
        /// <returns></returns>
        public static List<AdImgDto> GetImages(string JsonList)
        {
            if (JsonList == null)
            {
                throw new NullReferenceException("Hey! JsonList must be not null");
            }


            var jsonResult = ImgJson.Parse(JsonList);

            return GetImages(jsonResult.ImgJsonOrderList);
        }

        /// <summary>
        /// Получает название файлов из указанной папки преобразуя в список объектов типа AdImgModelView 
        /// </summary>
        /// <param name="RootFolder">json строка</param>
        /// <param name="FolderImgName">json строка</param>
        /// <returns></returns>
        public static List<AdImgDto> GetImages(string RootFolder, string FolderImgName)
        {
            var fileFolderName = Path.Combine(RootFolder, FolderImgName);

            if (Directory.Exists(fileFolderName))
            {
                var imgFiles = GetFilesName(Directory.GetFiles(fileFolderName));
                var fullImgs = imgFiles.Where(e => e.Contains(CONSTANT.FULL_FILE_NAME_PREFIX)).ToList<string>();
                var adImgList = new List<AdImgDto>();

                fullImgs.ForEach((img) =>
                {
                    var name = new Regex("^[0-9]+", RegexOptions.IgnoreCase).Match(img).Value;

                    adImgList.Add(new AdImgDto
                    {
                        FullFileName = GetFileName(imgFiles, name, CONSTANT.FULL_FILE_NAME_PREFIX), // or if full just "img"
                        MiddleFileName = GetFileName(imgFiles, name, CONSTANT.MIDDLE_FILE_NAME_PREFIX),
                        SmallFileName = GetFileName(imgFiles, name, CONSTANT.SMALL_FILE_NAME_PREFIX),
                        SuperSmallFileName = GetFileName(imgFiles, name, CONSTANT.SUPER_SMALL_FILE_NAME_PREFIX),
                        ImageStatus =
                            new Regex("^0{1}", RegexOptions.IgnoreCase).IsMatch(img)
                                ? AdImageStatus.Main
                                : AdImageStatus.NotMain,
                    });
                });

                return adImgList;
            }

            return new List<AdImgDto> { null };

        }


        /// <summary>
        /// Получает имена файлов без пути или с путями
        /// </summary>
        /// <param name="FullPaths"></param>
        /// <param name="ThatMatchWith">взять файлы соответсвующие регулярному выражению
        /// елси значение Null, то берет все файлы</param>
        /// <returns></returns>
        public static List<string> GetFilesName(string[] FullPaths, Regex ThatMatchWith = null)
        {
            List<string> result = new List<string>();

            if (ThatMatchWith != null)
            {
                FullPaths.ToList().ForEach(e =>
                {
                    if (ThatMatchWith.Match(e).Success)
                    {
                        result.Add(Path.GetFileName(e));
                    }
                });
            }
            else
            {
                FullPaths.ToList().ForEach(e => result.Add(Path.GetFileName(e)));
            }


            return result;
        }

        #region Helpers
        /// <summary>
        /// Получает имя файла из списка ImgFiles, где имя начинается с FileNameBase + AdditionalExtenstion
        /// </summary>
        /// <returns>Возврощает имя файла вида FileNameBaseAdditionalExtenstion (e.g. 0-full)</returns>
        private static string GetFileName(List<string> imgFiles, string fileNameBase, string additionalExtenstion)
        {
            if (imgFiles.Any(e => e.Contains(fileNameBase + additionalExtenstion)))
            {
                return imgFiles.SingleOrDefault(e => e.StartsWith(fileNameBase + additionalExtenstion));
            }

            return string.Empty;
        }
        #endregion
    }
}
