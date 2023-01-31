using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Suplemento.Views;
using Xamarin.Essentials;
namespace Suplemento
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (Preferences.Get("starting",true))
            {
                PreferencesInit();
            }

            MainPage = new NavigationPage( new Home());
        }


        public void PreferencesInit()
        {
            Preferences.Set("starting", false);
            Preferences.Set("fontFamily", "GandhiR");
            Preferences.Set("fontSize", 20);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
