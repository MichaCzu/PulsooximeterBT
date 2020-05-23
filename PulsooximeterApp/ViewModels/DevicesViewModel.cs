using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using PulsooximeterApp.Models;
using Xamarin.Forms;

namespace PulsooximeterApp.ViewModels
{
    class DevicesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        List<BlueDeviceModel> BlueDevices;

        public ICommand SearchDevices { protected set; get; }
        public ICommand ConnectDevice { protected set; get; }

        public DevicesViewModel()
        {
            SearchDevices = new Command(() =>
            {
                // wyszukaj nowe urządzenia i wklej do listy BlueDevices, zaktualizuj widok
            });

            ConnectDevice = new Command<string>((device) =>
            {
                // Połącz z urządzeniem, przełącz z PulsePage(?)
            });
        }

        int Count { get => BlueDevices.Count; }



        protected void OnPropertyChanges(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
