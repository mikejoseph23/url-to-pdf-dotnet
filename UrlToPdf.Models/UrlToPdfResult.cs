using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlToPdf.Models
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
