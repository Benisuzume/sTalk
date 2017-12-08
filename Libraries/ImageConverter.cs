using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace sTalk.Libraries
{
    public static class ImageConverter
    {
        public static byte[] ToBytes(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public static Image FromBytes(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }

        public static Image Resize(Image image, int width, int height)
        {
            var resized = new Bitmap(width, height);
            resized.SetResolution(72, 72);

            using (var g = Graphics.FromImage(resized))
            {
                var rect = new Rectangle(0, 0, width, height);
                g.DrawImage(image, rect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            }

            return resized;
        }
    }
}