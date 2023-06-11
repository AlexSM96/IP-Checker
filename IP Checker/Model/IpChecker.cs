using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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

            var line = client.DownloadString($"http://ipwho.is/{IpAddress}")
                .ToUpper()
                .Replace('{', ' ')
                .Replace('}', ' ')
                .Replace('"', ' ')
                .Replace("_", " ")
                .Replace("/", " ")
                .Replace(@"\", " ")
                .Trim()
                .Split(',')
                .Skip(1)
                .ToArray()
                .Where(x => x.Length > 5 && !x.Contains("FLAG") && !x.Contains("EMOJI")
                       && !x.Contains("IS") && !x.Contains("ORG"))
                .ToList();

            return line;
        }
    }
}
