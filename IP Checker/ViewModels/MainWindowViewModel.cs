using IP_Checker.Commands;
using IP_Checker.Model;
using IP_Checker.ViewModels.Base;
using MapControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace IP_Checker.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private string _title = "IP Checker";
        private string _ipAddress = string.Empty;
        private string _sortedData = string.Empty;
        private Location _location;
        private IpChecker _ipChecker;
        public ICommand OnCheckButtonClick { get; }
        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }
        public string SortedData
        {
            get { return _sortedData; }
            set { Set(ref _sortedData, value); }
        }

        public Location Location
        {
            get { return _location; }
            set { Set(ref _location, value); }
        }

        public string IpAddress
        {
            get { return _ipAddress; }
            set
            {
                if (Regex.IsMatch(value, @"^[0-9]+\.[0-9]+\.[0-9]+(\.[0-9]+)$"))
                {
                    if (Regex.IsMatch(value, @"\D+"))
                    {
                        value = new Regex(@"\D+").Replace(value, ".");
                    }
                }
                else
                {
                    MessageBox.Show("Typically, an address is written as four decimal numbers " +
                        "between 0 and 255 (equivalent to four eight-bit numbers) " +
                        "separated by DOTS, such as 192.168.0.3");
                    value = "";
                }
                _ipAddress = value;
                OnPropertyChanged(IpAddress);
            }
        }


        public MainWindowViewModel()
        {
            OnCheckButtonClick = new RelayCommand
                (OnCheckButtonClickExecuted, CanCheckButtonClickExecute);
            _ipChecker = new();
            _location = new Location();
        }

        public bool CanCheckButtonClickExecute(object parameter) => true;
        public void OnCheckButtonClickExecuted(object parameter)
        {
            _ipChecker = new(IpAddress);
            var ipInfo = _ipChecker.GetData();
            _location = new Location(GetOnMapLocation(ipInfo).Item1, GetOnMapLocation(ipInfo).Item2);
            SortedData = string.Empty;
            foreach (var item in ipInfo)
            {
                SortedData += item + "\r\n";
            }
        }

        private Tuple<double, double> GetOnMapLocation(List<string> ipInfo)
        {
            var locations = ipInfo
                .Where(x => x.Contains("latitude", StringComparison.OrdinalIgnoreCase)
                       || x.Contains("longitude", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Substring(9))
                .ToList();
            double latitude = double.Parse(locations[0].Replace('.', ','));
            double longitude = double.Parse(locations[1].Remove(0, 1).Replace('.', ','));
            return Tuple.Create(latitude, longitude);
        }
    }


}
