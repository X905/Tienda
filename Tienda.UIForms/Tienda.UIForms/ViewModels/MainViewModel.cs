namespace Tienda.UIForms.ViewModels
{
    public class MainViewModel
    {
        public LoginViewModel login { get; set; }

        public MainViewModel()
        {
            this.login = new LoginViewModel();
        }
    }
}
