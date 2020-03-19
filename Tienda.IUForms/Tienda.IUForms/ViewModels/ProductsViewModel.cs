
namespace Tienda.IUForms.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Tienda.Common.Models;
    using Tienda.Common.Services;
    using Xamarin.Forms;
    public class ProductsViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }

        public ProductsViewModel()
        {

            this.apiService = new ApiService();
            this.LoadProducts();
        }


        private async void LoadProducts()
        {
            var response = await this.apiService.GetListAsync<Product>(
                "https://tiendita.azurewebsites.net",
                "/api",
                "/Products");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept"
                    );
                return;
            }

            var myProducts = (List<Product>)response.Result;
            this.Products = new ObservableCollection<Product>(myProducts);
        }

    }
}
