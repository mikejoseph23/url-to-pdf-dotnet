using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using UrlToPdf.Models;

namespace UrlToPdf
{
    public static class UrlToPdf
    {
        public static UrlToPdfResult Convert(string url, string savePath)
        {
            var retVal = new UrlToPdfResult {StartTime = DateTime.UtcNow};
            var nodeJsScriptPath = ConfigurationManager.AppSettings["UrlToPdf.NodeJsFilePath"];
            var workingDirectory = Path.GetDirectoryName(nodeJsScriptPath);
            Debug.Assert(workingDirectory != null, nameof(workingDirectory) + " != null");
            InstallNodeModules(workingDirectory, retVal);

            var conversionStartTime = DateTime.UtcNow;
            ConvertUrlToPdfUsingNode(url, savePath, workingDirectory);

            retVal.ConversionTimeTakenSeconds = (DateTime.UtcNow - conversionStartTime).TotalSeconds;
            retVal.OutputFilePath = savePath;
            retVal.EndTime = DateTime.UtcNow;
            retVal.TotalSeconds = (retVal.EndTime - retVal.StartTime).TotalSeconds;
            return retVal;
        }

        private static void ConvertUrlToPdfUsingNode(string url, string savePath, string workingDirectory)
        {
            var exePath = ConfigurationManager.AppSettings["UrlToPdf.NodeJsExecutablePath"];
            var process = new Process
            {
                StartInfo =
                {
                    FileName = exePath,
                    Arguments = $"urlToPdf.js {url} {savePath}",
                    WorkingDirectory = workingDirectory
                }
            };

            process.Start();
            process.WaitForExit();
        }

        private static void InstallNodeModules(string workingDirectory, UrlToPdfResult retVal)
        {
            if (Directory.Exists(Path.Combine(workingDirectory, "node_modules"))) return;

            var installProcess = new Process
            {
                StartInfo =
                {
                    FileName = ConfigurationManager.AppSettings["UrlToPdf.NpmOrYarnPath"],
                    WorkingDirectory = workingDirectory
                },
            };

            installProcess.Start();
            installProcess.WaitForExit();
            retVal.InstallerTimeTakenSeconds = (DateTime.UtcNow - retVal.StartTime).TotalSeconds;
        }
    }
}
