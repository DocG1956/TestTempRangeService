namespace FileExist
{
    public class FileExists
    {
        private string sFilePath = string.Empty;
        
        public FileExists(string FilePath)
        {
            sFilePath = FilePath;
        }

        public bool FileExist()
        {
            bool fileExist = File.Exists(sFilePath);

            if (!fileExist)
            {
                return false;
            }

            return fileExist;
        }
    }
}