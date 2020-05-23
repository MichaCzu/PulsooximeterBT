using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PulsooximeterApp.Models;
using PulsooximeterApp.ViewModels;

namespace PulsooximeterApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PulsePage : ContentPage
    {
        public PulsePage()
        {
            InitializeComponent();

            BindingContext = new PulseViewModel();
        }
    }
}