using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using PulsooximeterApp.Models;
using Xamarin.Forms;
using PulsooximeterApp;
using PulsooximeterApp.Views;

namespace PulsooximeterApp.ViewModels
{
    class DevicesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> ListOfDevices { get; set; } = new List<string>();
        string selectedBthDevice;
        public string SelectedBthDevice { get => selectedBthDevice;
            set
            {
                selectedBthDevice = value;
                OnPropertyChanged(nameof(SelectedBthDevice));
                Connect();

                var page = (Page)Activator.CreateInstance(typeof(PulsePage));
                page.Title = "Pomiary";
            }
        }
        public string Message { get; set; }

        public ICommand SearchDevices { protected set; get; }
        public ICommand ConnectDevice { protected set; get; }

        public DevicesViewModel()
        {
            SelectedBthDevice = DependencyService.Get<IBth>().

            SearchDevices = new Command(() =>
            {
                GetPairedDevices();
                OnPropertyChanged(nameof(ListOfDevices));
            });
        }

        public void GetPairedDevices()
        {
            try
            {
                ListOfDevices = DependencyService.Get<IBth>().PairedDevices();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Attention", ex.Message, "Ok");
            }

        }

        public void Connect()
        {
            try
            {
                DependencyService.Get<IBth>().Connect(SelectedBthDevice);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Attention", ex.Message, "Ok");
            }
        }

        public void Disconnect()
        {
            try
            {
                DependencyService.Get<IBth>().Disconnect();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Attention", ex.Message, "Ok");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
