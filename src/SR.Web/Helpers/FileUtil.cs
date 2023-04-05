namespace SR.Web.Helpers;

public class FileUtil
{
    static DirectoryInfo _outputDir = null;
    public static DirectoryInfo OutputDir
    {
        get
        {
            return _outputDir;
        }
        set
        {
            _outputDir = value;
            if (!_outputDir.Exists)
            {
                _outputDir.Create();
            }
        }
    } 
    
    public static FileInfo GetCleanFileInfo(string file)
    {
        var fi = new FileInfo(file);
        if (fi.Exists)
        { 
            fi.Delete();  // ensures we create a new workbook
        } 
        return fi; 
    }
}