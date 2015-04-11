using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;
using DDDEastAnglia.Services.Messenger.Email.Templates;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Services.Messengers.Email.Templates
{
    [TestFixture]
    public class SessionUpdatedMailTemplateFacts
    {
        [TestFixture]
        public class The_Template_Should
        {
            [Test]
            public void Substitute_The_Session_Title_In_The_Template_Body()
            {
                var template = CreateSut(title: "My awesome session");

                var renderedTemplate = template.RenderBody();

                Assert.That(renderedTemplate, Is.Not.StringContaining("[SessionTitle]"));
                Assert.That(renderedTemplate, Contains.Substring("My awesome session"));
            }

            [Test]
            public void Substitute_The_Session_Abstract_In_The_Template_Body()
            {
                var template = CreateSut(@abstract: "Vote for me!");

                var renderedTemplate = template.RenderBody();

                Assert.That(renderedTemplate, Is.Not.StringContaining("[SessionAbstract]"));
                Assert.That(renderedTemplate, Contains.Substring("Vote for me!"));
            }

            [Test]
            public void Substitute_The_Session_Title_In_The_Template_Subject_Line()
            {
                var template = CreateSut(title: "My awesome session");

                var renderedSubjectLine = template.RenderSubjectLine();

                Assert.That(renderedSubjectLine, Is.Not.StringContaining("[SessionTitle]"));
                Assert.That(renderedSubjectLine, Is.StringContaining("My awesome session"));
            }

            private static IMailTemplate CreateSut(string title = "", string @abstract = "")
            {
                var template = SessionUpdatedMailTemplate.Create(@"Services\Messengers\Email\Templates\SessionUpdated.txt",
                    new Session
                    {
                        Title = title,
                        Abstract = @abstract
                    });

                return template;
            }
        }
    }
}
