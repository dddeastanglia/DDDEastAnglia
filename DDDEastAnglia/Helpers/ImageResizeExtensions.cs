using System;
using System.Drawing;

namespace DDDEastAnglia.Helpers
{
    public static class ImageResizeExtensions
    {
        public static System.Drawing.Image ConstrainToWidthOf(this System.Drawing.Image image, int width)
        {
            if (image.Width <= width)
            {
                return image;
            }

            double scale = ((double) width) / image.Width;
            int newHeight = (int) Math.Floor(image.Height * scale);
            return ScaleImage(image, width, newHeight);
        }

        public static System.Drawing.Image ConstrainToHeightOf(this System.Drawing.Image image, int height)
        {
            if (image.Height <= height)
            {
                return image;
            }

            double scale = ((double) height) / image.Height;
            int newWidth = (int) Math.Floor(image.Width * scale);
            return ScaleImage(image, newWidth, height);
        }

        private static System.Drawing.Image ScaleImage(System.Drawing.Image image, int width, int height)
        {
            var scaledImage = new Bitmap(image, width, height);
            image.Dispose();
            return scaledImage;
        }
    }
}
