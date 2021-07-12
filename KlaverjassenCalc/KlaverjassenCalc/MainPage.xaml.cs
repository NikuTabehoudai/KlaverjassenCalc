using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using KlaverjassenCalc.Model;
using KlaverjassenCalc.ViewModel;
using Xamarin.Essentials;

namespace KlaverjassenCalc
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new DataList();
        }
    }
}