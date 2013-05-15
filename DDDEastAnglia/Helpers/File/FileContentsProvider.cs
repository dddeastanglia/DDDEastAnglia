using System.IO;

namespace DDDEastAnglia.Helpers.File
{
    public class FileContentsProvider : IFileContentsProvider
    {
        public string GetFileContents(string path)
        {
            using (var reader = new StreamReader(path))
            {
                string contents = reader.ReadToEnd();
                return contents;
            }
        }
    }
}