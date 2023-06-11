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
        private IpChecker _ipChecker;
        public ICommand OnCheckButtonClick { get; }
        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        public string IpAddress
        {
            get { return _ipAddress; }
            set 
            {
                if (Regex.IsMatch(value, "[0-9]"))
                {
                    if (value.Contains(','))
                    {
                        value = value.Replace(',', '.');
                    }
                }
                else
                {
                    MessageBox.Show("Typically, an address is written as four decimal numbers " +
                        "between 0 and 255 (equivalent to four eight-bit numbers) " +
                        "separated by dots, such as 192.168.0.3");
                    value = "";
                }
                _ipAddress = value;
                OnPropertyChanged(IpAddress);
            }
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
            _ipChecker = new();
            _location = new Location(IpChecker.Latitude, IpChecker.Longitude);
        }

        public bool CanCheckButtonClickExecute(object parameter) => true;
        public void OnCheckButtonClickExecuted(object parameter)
        {
            
            _ipChecker = new(IpAddress);

            _location = new Location(IpChecker.Latitude, IpChecker.Longitude);
            SortedData = IpChecker.SortedData;
        }
    }


}
