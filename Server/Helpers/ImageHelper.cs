using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImgSend.Helpers
{
    public static class ImageHelper
    {
        public static string ImgToBase64(this Image img)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, img.RawFormat);
                    byte[] imgBytes = ms.ToArray();
                    return Convert.ToBase64String(imgBytes);
                }
            }
            catch { return null; }
        }

        public static Bitmap Base64ToImg(this string base64str)
        {
            byte[] strBytes = Convert.FromBase64String(base64str);
            try
            {
                using (MemoryStream ms = new MemoryStream(strBytes))
                {
                    ms.Position = 0;
                    Image img = Image.FromStream(ms);
                    ms.Close();
                    strBytes = null;
                    return new Bitmap(img);
                }
            }
            catch { return null; }
        }

        public static BitmapImage BitmapToImageSource(this Bitmap bm)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bm.Save(ms, ImageFormat.Jpeg);
                    ms.Position = 0;
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.StreamSource = ms;
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.EndInit();
                    return bi;
                }
            }
            catch { return null; }
        }
    }
}
