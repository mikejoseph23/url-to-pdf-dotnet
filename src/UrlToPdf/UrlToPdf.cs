﻿using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using UrlToPdf.Core.Models;

namespace UrlToPdf.Core
{
    public static class UrlToPdf
    {
        public static UrlToPdfResult Convert(string url, string savePath)
        {
            var retVal = new UrlToPdfResult {StartTime = DateTime.UtcNow};
            var conversionStartTime = bool.Parse(ConfigurationManager.AppSettings["UrlToPdf.ConvertUsingNode"])
                ? ConvertUsingNodeJs(url, savePath, retVal)
                : ConvertUsingPuppeteerSharp(url, savePath, retVal);
            retVal.ConversionTimeTakenSeconds = (DateTime.UtcNow - conversionStartTime).TotalSeconds;
            retVal.OutputFilePath = savePath;
            retVal.EndTime = DateTime.UtcNow;
            retVal.TotalSeconds = (retVal.EndTime - retVal.StartTime).TotalSeconds;
            return retVal;
        }

        private static DateTime ConvertUsingPuppeteerSharp(string url, string savePath, UrlToPdfResult retVal)
        {
            var conversionStartTime = DateTime.UtcNow;


            return conversionStartTime;
        }

        #region ConvertUsingNodeJs

        private static DateTime ConvertUsingNodeJs(string url, string savePath, UrlToPdfResult retVal)
        {
            var nodeJsScriptPath = ConfigurationManager.AppSettings["UrlToPdf.NodeJsFilePath"];
            var workingDirectory = Path.GetDirectoryName(nodeJsScriptPath);
            var nodeJsScriptFile = Path.GetFileName(nodeJsScriptPath);
            Debug.Assert(workingDirectory != null, nameof(workingDirectory) + " != null");
            InstallNodeModules(workingDirectory, retVal);

            var conversionStartTime = DateTime.UtcNow;
            if (File.Exists(savePath)) File.Delete(savePath);

            var exePath = ConfigurationManager.AppSettings["UrlToPdf.NodeJsExecutablePath"];
            var process = new Process
            {
                StartInfo =
                {
                    FileName = exePath,
                    Arguments = $"{nodeJsScriptFile} {url} {savePath}",
                    WorkingDirectory = workingDirectory,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };

            process.Start();
            process.WaitForExit();
            return conversionStartTime;
        }

        private static void InstallNodeModules(string workingDirectory, UrlToPdfResult retVal)
        {
            if (Directory.Exists(Path.Combine(workingDirectory, "node_modules"))) return;

            var installProcess = new Process
            {
                StartInfo =
                {
                    FileName = ConfigurationManager.AppSettings["UrlToPdf.NpmOrYarnPath"],
                    WorkingDirectory = workingDirectory,
                    WindowStyle = ProcessWindowStyle.Hidden
                },
            };

            installProcess.Start();
            installProcess.WaitForExit();
            retVal.InstallerTimeTakenSeconds = (DateTime.UtcNow - retVal.StartTime).TotalSeconds;
        }

        #endregion
    }
}
