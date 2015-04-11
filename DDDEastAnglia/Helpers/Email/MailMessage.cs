using System.Net.Mail;

namespace DDDEastAnglia.Helpers.Email
{
    public class MailMessage
    {
        protected bool Equals(MailMessage other)
        {
            return Equals(From, other.From) && Equals(To, other.To) && string.Equals(Subject, other.Subject) && string.Equals(Html, other.Html) && string.Equals(Text, other.Text);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MailMessage)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (From != null ? From.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (To != null ? To.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Subject != null ? Subject.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Html != null ? Html.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Text != null ? Text.GetHashCode() : 0);
                return hashCode;
            }
        }

        public MailAddress From { get; set; }
        public MailAddress To { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
        public string Text { get; set; }
    }
}