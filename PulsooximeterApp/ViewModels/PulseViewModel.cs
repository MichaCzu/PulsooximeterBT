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

        protected PulseModel Model;

        public int HeartRate { get => Model.Heartrate; protected set => Model.Heartrate=value; }
        public int SpO2 { get => Model.SpO2; protected set => Model.SpO2 = value; }

        public PulseViewModel()
        {
            Model = new PulseModel() { Heartrate = 0, SpO2 = 0 };
        }

        public void UpdateModel( PulseModel model )
        {
            Model = model;
        }

        protected void OnPropertyChanges(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
