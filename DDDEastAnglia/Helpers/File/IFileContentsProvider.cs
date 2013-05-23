namespace DDDEastAnglia.Helpers.File
{
    public interface IFileContentsProvider
    {
        string GetFileContents(string path);
    }
}