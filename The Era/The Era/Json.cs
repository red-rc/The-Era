using System.IO;

public class Json
{
    public static void SaveJson(string json, string path)
    {
        var stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
        var writer = new StreamWriter(stream);
        writer.WriteLine(json);
    }

    public static string LoadJson(string path)
    {
        StreamReader streamReader = new(path);
        return streamReader.ReadToEnd();
    }
}
