using System;
using System.Linq;
using System.Net;

namespace IP_Checker.Model
{
    internal class IpChecker
    {
        public string IpAddress { get; set; }
        public static string SortedData { get; private set; }
        public static double Longitude { get; private set; }
        public static double Latitude { get; private set; }

        public async void GetData()
        {
            using var client = new WebClient();

            var line = client.DownloadString($"http://ipwho.is/{IpAddress}")
            .ToUpper()
            .Replace('{', ' ')
            .Replace('}', ' ')
            .Replace('"', ' ')
            .Replace("_", " ")
            .Replace("/", " ")
            .Replace(@"\", "")
            .Trim()
            .Split(',')
            .Skip(1)
            .ToArray()
            .Where(x => x.Length > 5 && !x.Contains("FLAG") && !x.Contains("EMOJI")
            && !x.Contains("IS") && !x.Contains("ORG"))
            .ToList();

            SortedData = string.Empty;
            foreach (var item in line)
            {
                SortedData += item + "\r\n";
            }
            var locations = line
                .Where(x => x.Contains("latitude", StringComparison.OrdinalIgnoreCase)
                      || x.Contains("longitude", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Substring(11))
                .ToList();
            Latitude = double.Parse(locations[0].Replace('.', ','));
            Longitude = double.Parse(locations[1].Remove(0, 1).Replace('.', ','));

        }
    }
}
