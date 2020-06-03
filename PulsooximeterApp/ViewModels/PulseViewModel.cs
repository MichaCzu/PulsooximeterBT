using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

        public string HeartRate { get; set; } = "-";
        public string SpO2 { get; set; } = "-";
        public string ErrorString { get; set; } = "";
        bool isRunning = false;
        bool _beat = false;
        public bool Beat { get => _beat; set { _beat = value; } }

        public PulseViewModel()
        {
            Start();
        }

        public bool OnUpdateModel()
        {
            try
            {
                if (!DependencyService.Get<IBth>().IsConnected())
                {
                    ErrorString = "Nie połączono z urządzeniem..";
                    HeartRate = "-";
                    SpO2 = "-";
                    OnPropertyChanged(nameof(ErrorString));
                    OnPropertyChanged(nameof(HeartRate));
                    OnPropertyChanged(nameof(SpO2));
                    return true;
                }
                else
                {
                    ErrorString = "";
                    OnPropertyChanged(nameof(ErrorString));
                }

                var rawdata = DependencyService.Get<IBth>().Read();

                if (rawdata == "beat")
                {
                    Console.WriteLine("Beat!");
                    Beat = !Beat;
                    OnPropertyChanged(nameof(Beat));
                }

                var data = rawdata.Split(';');

                if (data.Length == 2)
                {
                    if (Double.Parse(data[0], new CultureInfo("en-US")) > 30.0)
                    {
                        HeartRate = data[0];
                        SpO2 = data[1];
                    } else {
                        ErrorString = "Przyłóż palec do czytnika";
                        OnPropertyChanged(nameof(ErrorString));

                        HeartRate = "-";
                        SpO2 = "-";
                    }

                    OnPropertyChanged(nameof(HeartRate));
                    OnPropertyChanged(nameof(SpO2));
                }
            }
            catch (Exception ex)
            {
                ErrorString = "Wystąpił problem z urządzeniem..";
                OnPropertyChanged(nameof(ErrorString));
            }

            return isRunning;
        }
        public void Start()
        {
            if(isRunning == false)
            {
                Device.StartTimer(TimeSpan.FromSeconds(1), OnUpdateModel);
                isRunning = true;
            }
        }
        public void Stop()
        {
            isRunning = false;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
