using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackLib.Utils {
    public static class ImageUtil {

        public static Image CreateSquareThumbnail(Image source, int thumbSides) {

            var sourceSmallerSide = (source.Width < source.Height ? source.Width : source.Height);
            double ratio = (double)thumbSides / sourceSmallerSide;
            var destWidth = (int)(ratio * source.Width);
            var destHeight = (int)(ratio * source.Height);
            var resizedImage = ResizeImage(source, destWidth, destHeight);
            var thumb = GetPartOfImage(resizedImage, new Rectangle(0, 0, thumbSides, thumbSides));
            return thumb;

        }

        public static Bitmap ResizeImage(Image image, int width, int height) {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage)) {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes()) {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Image GetPartOfImage(Bitmap source, Rectangle rectangle) {

            PixelFormat format = source.PixelFormat;
            Bitmap destination = source.Clone(rectangle, format);
            var g = Graphics.FromImage(destination);
            g.DrawImage(destination, 0, 0);

            return destination;

        }

    }
}
