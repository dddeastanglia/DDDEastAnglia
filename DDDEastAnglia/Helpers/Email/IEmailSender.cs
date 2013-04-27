namespace DDDEastAnglia.Helpers.Email
{
    public interface IEmailSender
    {
        void Send(IMailMessage message);
    }
}