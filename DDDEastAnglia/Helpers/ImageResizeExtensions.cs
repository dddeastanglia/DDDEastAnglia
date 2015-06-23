using System;
using System.Drawing;

namespace DDDEastAnglia.Helpers
{
    public static class ImageResizeExtensions
    {
        public static Image ConstrainToWidthOf(this Image image, int width)
        {
            if (image.Width <= width)
            {
                return image;
            }

            double scale = ((double) width) / image.Width;
            int newHeight = (int) Math.Floor(image.Height * scale);
            return ScaleImage(image, width, newHeight);
        }

        public static Image ConstrainToHeightOf(this Image image, int height)
        {
            if (image.Height <= height)
            {
                return image;
            }

            double scale = ((double) height) / image.Height;
            int newWidth = (int) Math.Floor(image.Width * scale);
            return ScaleImage(image, newWidth, height);
        }

        private static Image ScaleImage(Image image, int width, int height)
        {
            var scaledImage = new Bitmap(image, width, height);
            image.Dispose();
            return scaledImage;
        }
    }
}
