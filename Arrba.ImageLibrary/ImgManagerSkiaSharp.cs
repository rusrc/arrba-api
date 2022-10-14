using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Arrba.Exceptions;
using Arrba.ImageLibrary.Json;
using Arrba.ImageLibrary.ModelViews;
using Arrba.Services.Logger;
using SkiaSharp;
using static Arrba.Constants.CONSTANT;


namespace Arrba.ImageLibrary
{
    public class ImgManagerSkiaSharp : IImgManager
    {
        private const int Quality = 80;

        private readonly (int Width, int Height, bool Main)[] _sizes = {
            (Width: 1920, Height: 1080, Main: true),
            (Width: 400, Height: 300, Main: false) // MIDDLE_FILE_NAME_PREFIX
        };

        private readonly string _wwwRootPath;
        private readonly ILogService _logService;

        private readonly string _logoPath;
        private readonly string _adImagesFolder;
        private readonly string _backgroundImagePath;

        public ImgManagerSkiaSharp(
            string wwwRootPath,
            ILogService logService)
        {
            _wwwRootPath = wwwRootPath;
            _logService = logService;

            _adImagesFolder = ImgFolderManager.GetRootImageFolderPath(_wwwRootPath);
            _logoPath = ImgFolderManager.GetLogoPath(_wwwRootPath);
            _backgroundImagePath = ImgFolderManager.GetBackgroundImagePath(_wwwRootPath);
        }

        public async Task<ImgJson> SaveImagesAsync(string folderCategoryPath, string uniqueImgFolderName)
        {
            var imgJson = await Task.Run(() => SaveImages(folderCategoryPath, uniqueImgFolderName));

            // post images into bitbuket

            return imgJson;
        }

        public static IEnumerable<AdImgDto> GetImages(string imgJsonString, string folderName, string categoryId)
        {
            if (string.IsNullOrEmpty(imgJsonString))
            {
                throw new BusinessLogicException("Hey! JsonList must be not null");
            }

            var imgJson = ImgJson.Parse(imgJsonString);
            var adImgList = new List<AdImgDto>();

            imgJson.ImgJsonOrderList.ForEach(e =>
            {
                var indexName = e.IndexName.ToString();
                adImgList.Add(new AdImgDto
                {
                    FullFileName = $"${ROOT_HOST_HTTPS}/Uploads/AdvImages/{categoryId}/{folderName}/{indexName}-{BIG_FILE_NAME_PREFIX}.jpg",
                    MiddleFileName = $"{ROOT_HOST_HTTPS}/Uploads/AdvImages/{categoryId}/{folderName}/{indexName}-{MIDDLE_FILE_NAME_PREFIX}.jpg",
                    SmallFileName = $"{ROOT_HOST_HTTPS}/Uploads/AdvImages/{categoryId}/{folderName}/{indexName}-{MIDDLE_FILE_NAME_PREFIX}.jpg",
                    SuperSmallFileName = $"{ROOT_HOST_HTTPS}/Uploads/AdvImages/{categoryId}/{folderName}/{indexName}-{MIDDLE_FILE_NAME_PREFIX}.jpg",
                    ImageStatus = e.Order == 0 ? AdImageStatus.Main : AdImageStatus.NotMain
                });
            });

            return adImgList;
        }

        internal ImgJsonOrder GetSaveImagesAndImgJsonOrder(
            string tempFilePath,
            string userFolderPath,
            int indexName)
        {
            SKBitmap bitmap = null;
            try
            {
                var sw = Stopwatch.StartNew();
                bitmap = SKBitmap.Decode(tempFilePath);

                foreach (var size in _sizes)
                {
                    var pathToSaveJpg = Path.Combine(userFolderPath, $"{indexName}-{size.Width}x{size.Height}.jpg");

                    // Compute index to get new width
                    var index = bitmap.Width / (decimal)bitmap.Height;
                    var newWidth = Convert.ToInt32(Math.Round(size.Height * index, 3, MidpointRounding.AwayFromZero));

                    bitmap = bitmap.Resize(new SKImageInfo(newWidth, size.Height), SKFilterQuality.Medium);

                    using (var image = SKImage.FromBitmap(bitmap))
                        if (size.Main)
                            using (var data = image.Encode(SKEncodedImageFormat.Jpeg, Quality))
                            using (var stream = File.OpenWrite(pathToSaveJpg))
                                data.SaveTo(stream);
                        else
                            using (var subset = image.Subset(GetCroppedRectI(image.Width, image.Height, size.Width, size.Height)))
                            using (var data = subset.Encode(SKEncodedImageFormat.Jpeg, Quality))
                            using (var stream = File.OpenWrite(pathToSaveJpg))
                                data.SaveTo(stream);
                }

                sw.Stop();
                _logService.Info(
                    $@"The file with index '{indexName}' croped on {_sizes.Length} sizes for {sw.ElapsedMilliseconds} ms. tempFilePath: {tempFilePath}");

                return new ImgJsonOrder { Order = indexName, IndexName = indexName };
            }
            catch (Exception ex)
            {
                throw new BusinessCriticalLogicException(ex.Message, ex);
            }
            finally
            {
                bitmap?.Dispose();
            }
        }

        private ImgJson SaveImages(string folderCategoryPath, string uniqueImgFolderName)
        {
            var imgJson = new ImgJson { CountHistory = 0, ImgJsonOrderList = new List<ImgJsonOrder>() };

            if (string.IsNullOrEmpty(uniqueImgFolderName))
            {
                return imgJson;
            }

            string combinePath = Path.Combine(_adImagesFolder, folderCategoryPath, uniqueImgFolderName);
            string userFolderPath = ImgManager.CheckFolderAndCreate(combinePath, true);

            // App_Data/tempImages/guid-guid-guid-guid	
            string sourcePath = ImgFolderManager.GetTempUniqueItemFolder(uniqueImgFolderName, _wwwRootPath);
            string[] tempFilePaths = Directory.GetFiles(sourcePath);

            Parallel.ForEach(tempFilePaths, (tempFilePath, pls, index) =>
            {
                var imgJsonOrder = GetSaveImagesAndImgJsonOrder(tempFilePath, userFolderPath, (int)index);
                imgJson
                    .ImgJsonOrderList
                    .Add(imgJsonOrder);
            });


            imgJson.CountHistory = imgJson.GetMaxIndexNameInList();

            return imgJson;
        }

        private SKRectI GetCroppedRectI(int originalWidth, int originalHeight, int containerWidth, int containerHeight)
        {
            var newWidth = originalWidth;
            var newHeight = originalHeight;

            double relX = newWidth / (double)containerWidth;
            double relY = newHeight / (double)containerHeight;

            int dX = 0;
            int dY = 0;

            if (relX >= relY && relY > 1.0)
            {
                newHeight = containerHeight;
                newWidth = relX > relY ? (int)(newWidth / relY) : containerWidth;
            }
            else if (relY > relX && relX > 1.0)
            {
                newWidth = containerWidth;
                newHeight = (int)(newHeight / relX);
            }

            dX = (int)((containerWidth - newWidth) / 2.0);
            dY = (int)((containerHeight - newHeight) / 2.0);

            if (dX < 0 || dY < 0)
            {
                int dX1 = 0;
                int dY1 = 0;

                if (dX < 0) dX1 = dX * -1;
                if (dY < 0) dY1 = dY * -1;

                newWidth = newWidth - dX1 * 2;
                newHeight = newHeight - dY1 * 2;

                return new SKRectI(left: dX1, top: dY1, right: newWidth, bottom: newHeight);
            }

            return new SKRectI(left: dX, top: dY, right: newWidth, bottom: newHeight);
        }
    }
}
