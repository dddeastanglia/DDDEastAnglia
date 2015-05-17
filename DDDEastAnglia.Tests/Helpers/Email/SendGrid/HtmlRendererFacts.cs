using DDDEastAnglia.Helpers.Email.SendGrid;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers.Email.SendGrid
{
    [TestFixture]
    public class HtmlRendererFacts
    {
        [TestFixture]
        public class Render_Should
        {
            [Test]
            public void Convert_Markdown_To_HTML()
            {
                var sut = CreateSut();

                var renderedHtml = sut.Render("*italicised text*");

                Assert.That(renderedHtml, Contains.Substring("<p><em>italicised text</em></p>"));
            }

            [Test]
            public void Wrap_The_Content_In_An_HTML_Envelope()
            {
                var sut = CreateSut();

                var renderedHtml = sut.Render("");

                Assert.That(renderedHtml, Is.Not.StringContaining("[MessageBody]"));
                Assert.That(renderedHtml, Is.StringStarting("<html><body>"));
                Assert.That(renderedHtml, Is.StringEnding("</body></html>"));
            }

            private static HtmlRenderer CreateSut()
            {
                return new HtmlRenderer("<html><body>[MessageBody]</body></html>");
            }
        }
    }
}
