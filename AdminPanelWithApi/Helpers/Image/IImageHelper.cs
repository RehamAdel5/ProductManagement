
using System.Drawing.Imaging;

namespace AdminPanelWithApi.Helpers.Image
{
    public interface IImageHelper
    {
        void CompressImageFile(Stream srcImgStream, string targetPath);
        Task DeleteFile(string targetPath);
        ImageCodecInfo GetEncoder(ImageFormat format);
        Task<string> ProcessFileUpload(IFormFile? imageFile, string targetPath);
        Task<Tuple<bool, string>> ProcessImageUpload(IFormFile? imageFile, string targetPath);
    }
}
