using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Suplemento.Models;
using Suplemento.ViewModels;
using System.Windows.Input;

namespace Suplemento.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Anthem : ContentPage
    {
        public Anthem(Mhymns athem)
        {
            InitializeComponent();
         
            this.BindingContext = new VMathem(Navigation, athem);
        }

        
    }
}