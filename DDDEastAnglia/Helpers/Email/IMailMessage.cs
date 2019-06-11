using System.Net.Mail;

namespace DDDEastAnglia.Helpers.Email
{
    public interface IMailMessage
    {
        MailAddress From{get;}
        MailAddress[] To{get;}
        string Subject{get;}
        string Html{get;}
        string Text{get;}
    }
}