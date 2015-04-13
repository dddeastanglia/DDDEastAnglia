using System.Drawing;
using DDDEastAnglia.Helpers;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers
{
    [TestFixture]
    public sealed class ImageResizeExtensionsTests
    {
        [Test]
        public void ContrainToHeight_DoesNotResizeTheSuppliedImage_WhenItIsShorterThanTheDesiredHeight()
        {
            const int originalHeight = 300;

            using (var bitmap = new Bitmap(200, originalHeight))
            {
                const int newHeight = 400;
                var resizedBitmap = bitmap.ContrainToHeightOf(newHeight);
                Assert.That(resizedBitmap.Height, Is.EqualTo(originalHeight));
            }
        }

        [Test]
        public void ContrainToHeight_ResizesTheSuppliedImage_WhenItIsTallerThanTheDesiredHeight()
        {
            using (var bitmap = new Bitmap(200, 300))
            {
                const int newHeight = 20;
                var resizedBitmap = bitmap.ContrainToHeightOf(newHeight);
                Assert.That(resizedBitmap.Height, Is.EqualTo(newHeight));
            }
        }

        [Test]
        public void ContrainToWidth_DoesNotResizeTheSuppliedImage_WhenItIsNarrowerThanTheDesiredWidth()
        {
            const int originalWidth = 200;

            using (var bitmap = new Bitmap(originalWidth, 300))
            {
                const int newWidth = 400;
                var resizedBitmap = bitmap.ContrainToWidthOf(newWidth);
                Assert.That(resizedBitmap.Width, Is.EqualTo(originalWidth));
            }
        }

        [Test]
        public void ContrainToWidth_ResizesTheSuppliedImage_WhenItIsWiderThanTheDesiredWidth()
        {
            using (var bitmap = new Bitmap(200, 300))
            {
                const int newWidth = 20;
                var resizedBitmap = bitmap.ContrainToWidthOf(newWidth);
                Assert.That(resizedBitmap.Width, Is.EqualTo(newWidth));
            }
        }
    }
}
