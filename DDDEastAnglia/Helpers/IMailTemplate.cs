namespace DDDEastAnglia.Helpers
{
    public interface IMailTemplate
    {
        string RenderBody();
        string RenderSubjectLine();
    }
}