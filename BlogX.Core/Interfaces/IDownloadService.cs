namespace BlogX.Core.Interfaces;

public interface IDownloadService
{
    Task<Stream> DownloadAsync(string url);
}
