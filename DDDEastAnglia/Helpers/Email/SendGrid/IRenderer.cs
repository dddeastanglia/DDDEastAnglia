namespace DDDEastAnglia.Helpers.Email.SendGrid
{
    public interface IRenderer
    {
        string Render(string content);
    }
}
