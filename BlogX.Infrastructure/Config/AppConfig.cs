namespace BlogX.Infrastructure.Config;

public class AppConfig
{
    public string AppDataPath { get; set; } = default!;

    public string DataBasePath { get; set; } = default!;

    public string BlobPath { get; set; } = default!;

    public string BlobUri { get; set; } = default!;

    public static AppConfig BuildDefaultAppConfig()
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        var appDataPath = Path.Join(path, ".BlogX");

        var dataBasePath = Path.Join(appDataPath, "db");

        var blobPath = Path.Join(appDataPath, "blob");

        return new AppConfig
        {
            AppDataPath = appDataPath,
            DataBasePath = dataBasePath,
            BlobPath = blobPath,
            BlobUri = "/blob"
        };
    }
}
