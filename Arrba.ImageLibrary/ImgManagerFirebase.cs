using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Arrba.ImageLibrary.Json;
using Arrba.ImageLibrary.ModelViews;
using Arrba.Services.Configuration;
using Arrba.Services.Logger;
using Firebase.Storage;
using static Arrba.Constants.CONSTANT;

namespace Arrba.ImageLibrary
{
    public class ImgManagerFirebase : IImgManager
    {
        private readonly ImgManager _imgManager;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly ILogService _logService;
        public ImgManagerFirebase(ImgManager imgManager, ApplicationConfiguration applicationConfiguration, ILogService logService)
        {
            this._imgManager = imgManager;
            this._applicationConfiguration = applicationConfiguration;
            this._logService = logService;
        }

        /// <summary>
        /// Save images in firebase
        /// </summary>
        /// <param name="uniqueImgFolderName"></param>
        /// <returns></returns>
        public async Task<ImgJson> SaveImagesAsync(string uniqueImgFolderName, string _)
        {
            var imgJson = new ImgJson { CountHistory = 0, ImgJsonOrderList = new List<ImgJsonOrder>() };
           
            if (string.IsNullOrEmpty(uniqueImgFolderName))
            {
                return imgJson;
            }

            // App_Data/tempImages/guid-guid-guid-guid	
            string sourcePath = ImgFolderManager.GetTempUniqueItemFolder(uniqueImgFolderName, _imgManager.RootPath);
            string[] filePaths = Directory.GetFiles(sourcePath);
            int indexName = 0;

            foreach (var filePath in filePaths)
            {
                var stream = File.Open(filePath, FileMode.Open);
                var fileName = uniqueImgFolderName;
                var fileExtension = Path.GetExtension(filePath);
                string imageUrl;

                try
                {
                    imageUrl = await new FirebaseStorage(_applicationConfiguration.FirebaseStorageEndpoint)
                                        .Child("images")
                                        .Child($"{fileName}_{indexName}{fileExtension}")
                                        .PutAsync(stream);

                    _logService.Info($"Image {fileName}_{indexName}{fileExtension} added successfully.... url {imageUrl}");
                }
                catch (Exception ex)
                {
                    _logService.Error($"Image {fileName}_{indexName}{fileExtension} failed.... " + ex.Message, ex);
                    throw;
                }
                finally
                {
                    stream?.Dispose();
                }

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    imgJson.ImgJsonOrderList.Add(new ImgJsonOrder { Order = indexName, IndexName = indexName });
                }

                indexName++;
            }

            imgJson.CountHistory = imgJson.GetMaxIndexNameInList();

            return imgJson;
        }

        public static IEnumerable<AdImgDto> GetImages(string imgJsonString, string folderName, string _)
        {
            if (string.IsNullOrEmpty(imgJsonString))
            {
                throw new NullReferenceException("Hey! JsonList must be not null");
            }

            var imgJson = ImgJson.Parse(imgJsonString);
            var adImgList = new List<AdImgDto>();
            const string ext = "jpg";

            // thumb@1024x720-02f78438329c484a9f50035d32e8a08f_15.jpg
            imgJson.ImgJsonOrderList.ForEach(e =>
            {
                var indexName = e.IndexName.ToString();
                adImgList.Add(new AdImgDto
                {
                    FullFileName = string.Format(FIREBASE_URL, $"thumb%40{FULL_FILE_NAME_PREFIX}-{folderName}_{indexName}.{ext}"), // string.Concat((object)indexName, CONSTANT.FULL_FILE_NAME_PREFIX, ".jpg"),
                    MiddleFileName = string.Format(FIREBASE_URL, $"thumb%40{MIDDLE_FILE_NAME_PREFIX2}-{folderName}_{indexName}.{ext}"),
                    SmallFileName = string.Format(FIREBASE_URL, $"thumb%40{MIDDLE_FILE_NAME_PREFIX2}-{folderName}_{indexName}.{ext}"),
                    SuperSmallFileName = string.Format(FIREBASE_URL, $"thumb%40{MIDDLE_FILE_NAME_PREFIX2}-{folderName}_{indexName}.{ext}"),
                    ImageStatus = e.Order == 0 ? AdImageStatus.Main : AdImageStatus.NotMain
                });
            });

            return adImgList;
        }
    }
}
