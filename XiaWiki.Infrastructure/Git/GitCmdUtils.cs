using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XiaWiki.Infrastructure.Options;

namespace XiaWiki.Infrastructure.Git;

internal class GitCmdUtils(IOptionsMonitor<WikiOption> wikiOptionDelegate, ILogger<GitCmdUtils> logger)
{
    public async Task<bool> GitCheck()
    {
        try
        {
            var (successed, _) = await GitVersion();

            return successed;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "git check error");
            return false;
        }
    }

    public async Task<(bool, string)> GitVersion()
    {
        return await ExecGitCommamd("--version");
    }

    public async Task<(bool, string)> GitCloneDocs()
    {
        var option = wikiOptionDelegate.CurrentValue;

        return await ExecGitCommamd($"clone {option.GitRepository} {option.PagesFolderName}", option.Workspace);
    }

    public async Task<(bool, string)> GitPullDocs()
    {
        var option = wikiOptionDelegate.CurrentValue;

        return await ExecGitCommamd($"pull", option.PagesDir);
    }

    private async Task<(bool, string)> ExecGitCommamd(string args, string? workingDir = null)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = args,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        if (workingDir != null)
            startInfo.WorkingDirectory = workingDir;

        logger.LogInformation($"{nameof(ExecGitCommamd)}:WorkingDirectory={startInfo.WorkingDirectory},FileName={startInfo.FileName},Arguments={startInfo.Arguments}");

        using var process = Process.Start(startInfo);
        if (process is null)
            return (false, string.Empty);

        await process.WaitForExitAsync();

        using var outputReader = process.StandardOutput;
        using var errorReader = process.StandardError;
        var output = await outputReader.ReadToEndAsync();
        var error = await errorReader.ReadToEndAsync();

        logger.LogInformation($"{nameof(ExecGitCommamd)}:Output={output},Error={error}");

        return string.IsNullOrEmpty(error) ? (true, output) : (false, error);
    }
}
