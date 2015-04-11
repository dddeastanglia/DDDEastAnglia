namespace DDDEastAnglia.Helpers.Email
{
    public interface IPostman
    {
        void Deliver(MailMessage message);
    }
}