using System;
using Tienda.IUForms.ViewModels;
using Tienda.IUForms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tienda.IUForms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


            MainViewModel.GetInstance().Login = new LoginViewModel();
            this.MainPage = new NavigationPage(new LoginPage());
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
