namespace DDDEastAnglia.Services.Messenger.Email
{
    public interface IPostman
    {
        void Deliver(MailMessage message);
    }
}