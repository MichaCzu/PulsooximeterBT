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
    class PulseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string HeartRate { get; set; } = "0";
        public string SpO2 { get; set; } = "0";

        public PulseViewModel()
        {
            Device.StartTimer(TimeSpan.FromSeconds(2), OnUpdateModel);

        }

        public bool OnUpdateModel()
        {
            try
            {
                var data = DependencyService.Get<IBth>().Read();
                HeartRate = data;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Attention", ex.Message, "Ok");
            }

            return true;
        }

        protected void OnPropertyChanges(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
