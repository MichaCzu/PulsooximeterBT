using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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
        public string SpO2 { get; set; } = "- ";
        public string ErrorString { get; set; } = "";
        bool isRunning = false;
        bool _beat;
        public bool Beat { get => _beat; set { _beat = value; } }

        List<double> DataBuffer { get; set; }

        public PulseViewModel()
        {
            Start();
            DependencyService.Get<IBth>().AttachDelegate(OnReceive);
            DataBuffer = new List<double>();
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
            }
            catch (Exception ex)
            {
                ErrorString = "Wystąpił problem z urządzeniem..";
                OnPropertyChanged(nameof(ErrorString));
            }

            return isRunning;
        }

        public void OnReceive(object Sender, string receiveddata)
        {
            var rawdata = (string)receiveddata;
            var data = rawdata.Split(';');

            ErrorString = "";

            if (rawdata == "beat")
            {
                Console.WriteLine("Beat!");
                Beat = !Beat;
                OnPropertyChanged(nameof(Beat));
            }
            else if (data.Length == 2)
            {

                double hrvalue = Double.Parse(data[0], new CultureInfo("en-US"));
                if (hrvalue > 30.0)
                {
                    try
                    {
                        DataBuffer.Add(hrvalue);

                        if (DataBuffer.Count > 8)
                        {
                            DataBuffer.RemoveAt(0);
                        }
                    } 
                    catch(Exception e)
                    {

                    }
                }
                else
                {
                    if (Double.Parse(data[1], new CultureInfo("en-US")) == 0)
                    {
                        DataBuffer.Clear();
                        ErrorString = "Przyłóż palec do czytnika";
                    }
                }

                if (DataBuffer.Count > 3)
                {
                    double DataPrepare = (DataBuffer.Sum() - DataBuffer.Max() - DataBuffer.Min()) / (DataBuffer.Count - 2);
                    HeartRate = String.Format("{0:f0}", DataPrepare);
                    SpO2 = String.Format("{0:f0}", data[1]);
                }
                else
                {
                    HeartRate = "-";
                    SpO2 = "- ";
                }

                OnPropertyChanged(nameof(HeartRate));
                OnPropertyChanged(nameof(SpO2));
                OnPropertyChanged(nameof(ErrorString));
            }
        }
        public void Start()
        {
            if(isRunning == false)
            {
                Device.StartTimer(TimeSpan.FromSeconds(2), OnUpdateModel);
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
