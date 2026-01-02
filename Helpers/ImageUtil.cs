using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace GoIoFish.Helpers
{
    public static class ImageUtil
    {
        public static BitmapImage SafeBase64ToBitmap(string base64)
        {
            if (string.IsNullOrEmpty(base64)) return null;

            try
            {
                var bytes = Convert.FromBase64String(base64);
                using (var stream = new MemoryStream(bytes))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                    image.Freeze();
                    return image;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}