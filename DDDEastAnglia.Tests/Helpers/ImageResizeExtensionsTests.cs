using System.Drawing;
using DDDEastAnglia.Helpers;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers
{
    [TestFixture]
    public sealed class ImageResizeExtensionsTests
    {
        [Test]
        public void ConstrainToHeight_DoesNotResizeTheSuppliedImage_WhenItIsShorterThanTheDesiredHeight()
        {
            const int originalHeight = 300;

            using (var bitmap = new Bitmap(200, originalHeight))
            {
                const int newHeight = 400;
                var resizedBitmap = bitmap.ConstrainToHeightOf(newHeight);
                Assert.That(resizedBitmap.Height, Is.EqualTo(originalHeight));
            }
        }

        [Test]
        public void ConstrainToHeight_ResizesTheSuppliedImage_WhenItIsTallerThanTheDesiredHeight()
        {
            using (var bitmap = new Bitmap(200, 300))
            {
                const int newHeight = 20;
                var resizedBitmap = bitmap.ConstrainToHeightOf(newHeight);
                Assert.That(resizedBitmap.Height, Is.EqualTo(newHeight));
            }
        }

        [Test]
        public void ConstrainToWidth_DoesNotResizeTheSuppliedImage_WhenItIsNarrowerThanTheDesiredWidth()
        {
            const int originalWidth = 200;

            using (var bitmap = new Bitmap(originalWidth, 300))
            {
                const int newWidth = 400;
                var resizedBitmap = bitmap.ConstrainToWidthOf(newWidth);
                Assert.That(resizedBitmap.Width, Is.EqualTo(originalWidth));
            }
        }

        [Test]
        public void ConstrainToWidth_ResizesTheSuppliedImage_WhenItIsWiderThanTheDesiredWidth()
        {
            using (var bitmap = new Bitmap(200, 300))
            {
                const int newWidth = 20;
                var resizedBitmap = bitmap.ConstrainToWidthOf(newWidth);
                Assert.That(resizedBitmap.Width, Is.EqualTo(newWidth));
            }
        }
    }
}
