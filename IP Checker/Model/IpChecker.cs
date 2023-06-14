using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace IP_Checker.Model
{
    internal class IpChecker
    {
      
        public string IpAddress { get; set; }

        public IpChecker(string ip = "")
        {
            IpAddress = ip;
        }

        public List<string> GetData()
        {
            using var client = new WebClient();

            var downloadString = client.DownloadString($"http://ipwho.is/{IpAddress}");
            var parsingLine = string.Empty;
            Regex.Matches(downloadString, @"[\w\,\-\:\,\.]+")
                .Select(x => x.ToString().ToUpper())
                .ToList()
                .ForEach(x => parsingLine += x);

            var listLine = parsingLine.Split(",")
                .Where(x => !x.Contains("BORDER") && !x.Contains("FLAG")
                      && !x.Contains("EMOJI") && x.Length > 5)
                .ToList();
            return listLine;
        }
    }
}
