using Newtonsoft.Json.Linq;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;

namespace DevRant
{
    internal class Utilities
    {
        public static object GetValue(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Integer:
                    return token.ToObject<int>();
                case JTokenType.Boolean:
                    return token.ToObject<bool>();
                case JTokenType.Float:
                    return token.ToObject<float>();
                case JTokenType.String:
                    return token.ToObject<string>();
                default:
                    return token.ToString();
            }
        }

        public static Image GetImage(string url)
        {
            var request = WebRequest.Create(url);
            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                Bitmap bitmap = new Bitmap(stream);
                return bitmap;
            }
        }

        public static ImageSource GetImageSource(Image bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public static ImageSource GetImageSource(string url)
        {
            var img = GetImage(url);
            var source = GetImageSource(img);

            return source;
        }
    }
}