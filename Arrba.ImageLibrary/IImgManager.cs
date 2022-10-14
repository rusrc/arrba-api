using System.Threading.Tasks;
using Arrba.ImageLibrary.Json;

namespace Arrba.ImageLibrary
{
    public interface IImgManager
    {
        Task<ImgJson> SaveImagesAsync(string folderCategoryPath, string uniqueImgFolderName);
    }
}
