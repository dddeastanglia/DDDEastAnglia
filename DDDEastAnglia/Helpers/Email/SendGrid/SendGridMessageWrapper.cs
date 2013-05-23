using System;
using System.Net.Mail;

namespace DDDEastAnglia.Helpers.Email.SendGrid
{
    public class SendGridMessageWrapper : IMailMessage
    {
        public SendGridMail.SendGrid SendGrid{get {return sendGrid;}}
        private readonly SendGridMail.SendGrid sendGrid;

        public MailAddress From{get {return sendGrid.From;}}
        public MailAddress[] To{get {return sendGrid.To;}}
        public string Subject{get {return sendGrid.Subject;}}
        public string Html{get {return sendGrid.Html;}}
        public string Text{get {return sendGrid.Text;}}

        public SendGridMessageWrapper(SendGridMail.SendGrid sendGrid)
        {
            if (sendGrid == null)
            {
                throw new ArgumentNullException("sendGrid");
            }
            
            this.sendGrid = sendGrid;
        }
    }
}