using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using AdminPanelWithApi.Helpers.Image;

namespace AdminPanelWithApi.Helpers.CompressImage
{
    public class ImageHelper:IImageHelper
    {
        public ImageHelper()
        {
        }

        public async Task<Tuple<bool,string>> ProcessImageUpload(IFormFile? imageFile, string targetPath)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0 || !IsImage(imageFile))
                {
                    return new Tuple<bool,string>(false,string.Empty);
                }
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                FormFile newFile = new FormFile(imageFile.OpenReadStream(), 0, imageFile.Length, string.Empty, fileName);
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                string filePath = Path.Combine(targetPath, newFile.FileName);
                Stream fileStream = newFile.OpenReadStream();
              
                    CompressImageFile(fileStream, filePath);
                return new Tuple<bool, string>(true, fileName);
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, string.Empty);
            }
        }
        public void CompressImageFile(Stream srcImgStream, string targetPath)
        {
            try
            {
                // Convert stream to image
                using var image = System.Drawing.Image.FromStream(srcImgStream);

                float maxHeight = 900.0f;
                float maxWidth = 900.0f;
                int newWidth;
                int newHeight;

                var originalBMP = new Bitmap(srcImgStream);
                int originalWidth = originalBMP.Width;
                int originalHeight = originalBMP.Height;

                if (originalWidth > maxWidth || originalHeight > maxHeight)
                {
                    // To preserve the aspect ratio  
                    float ratioX = (float)maxWidth / originalWidth;
                    float ratioY = (float)maxHeight / originalHeight;
                    float ratio = Math.Min(ratioX, ratioY);
                    newWidth = (int)(originalWidth * ratio);
                    newHeight = (int)(originalHeight * ratio);
                }

                else
                {
                    newWidth = originalWidth;
                    newHeight = originalHeight;
                }

                var bitmap = new Bitmap(originalBMP, newWidth, newHeight);
                var imgGraph = Graphics.FromImage(bitmap);

                imgGraph.SmoothingMode = SmoothingMode.Default;
                imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);

                var extension = Path.GetExtension(targetPath).ToLower();
                // for file extension having png and gif
                if (extension == ".png" || extension == ".gif")
                {
                    // Save image to targetPath
                    bitmap.Save(targetPath, image.RawFormat);
                }

                // for file extension having .jpg or .jpeg
                else if (extension == ".jpg" || extension == ".jpeg")
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    Encoder myEncoder = Encoder.Quality;
                    var encoderParameters = new EncoderParameters(1);
                    var parameter = new EncoderParameter(myEncoder, 50L);
                    encoderParameters.Param[0] = parameter;

                    // Save image to targetPath
                    bitmap.Save(targetPath, jpgEncoder, encoderParameters);
                }
                bitmap.Dispose();
                imgGraph.Dispose();
                originalBMP.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public async Task<string> ProcessFileUpload(IFormFile? imageFile, string targetPath)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return string.Empty; // or throw an error based on your requirements
            }
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            FormFile newFile = new FormFile(imageFile.OpenReadStream(), 0, imageFile.Length, string.Empty, fileName);
            
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            string filePath = Path.Combine(targetPath, newFile.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await newFile.CopyToAsync(fileStream);
            }
            return newFile.FileName;
        }
       
        public async Task DeleteFile(string targetPath)
        {
            if (!Directory.Exists(targetPath))
            {
                File.Delete(targetPath);
            }
        }

        private bool IsImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    byte[] bytes = memoryStream.ToArray();
                    string contentType = file.ContentType;

                    // Check if content type starts with "image/"
                    if (!string.IsNullOrEmpty(contentType) && contentType.StartsWith("image/"))
                    {
                        // Check if it's a valid image format by trying to create a Bitmap from the bytes
                        using (var imageStream = new MemoryStream(bytes))
                        {
                            using (var bitmap = new Bitmap(imageStream))
                            {
                                // If the Bitmap constructor doesn't throw an exception, it's a valid image
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Any exceptions indicate that the file is not a valid image
                return false;
            }

            return false;
        }
    }
}
