using IP_Checker.Commands;
using IP_Checker.Model;
using IP_Checker.ViewModels.Base;
using MapControl;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Net.WebRequestMethods;

namespace IP_Checker.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private string _title = "IP Checker";
        private string _ipAddress = "";
        private string _sortedData = "";
        private Location _location;
        public IpChecker IpChecker { get; }
        public ICommand OnCheckButtonClick { get; }
        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        public string IpAddress
        {
            get { return _ipAddress; }
            set { Set(ref _ipAddress, value); }
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

        public MainWindowViewModel()
        {
            OnCheckButtonClick = new RelayCommand
                (OnCheckButtonClickExecuted, CanCheckButtonClickExecute);
            IpChecker = new IpChecker();
            IpChecker.GetData();
            _location = new Location(IpChecker.Latitude, IpChecker.Longitude);
        }

        public bool CanCheckButtonClickExecute(object parameter) => true;
        public void OnCheckButtonClickExecuted(object parameter)
        {
            IpChecker.IpAddress = IpAddress;
            IpChecker.GetData();
            Location = new Location(IpChecker.Latitude, IpChecker.Longitude);
            SortedData = IpChecker.SortedData;
        }
    }


}
