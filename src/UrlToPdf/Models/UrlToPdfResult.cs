using System;

namespace UrlToPdf.Core.Models
{
    public class UrlToPdfResult
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public double InstallerTimeTakenSeconds { get; set; }

        public double ConversionTimeTakenSeconds { get; set; }

        public double TotalSeconds { get; set; }

        public string OutputFilePath { get; set; }
    }
}
