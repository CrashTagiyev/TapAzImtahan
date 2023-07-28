using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Xsl;
using TapAzImtahan.Command;
using TapAzImtahan.Helper;
using TapAzImtahan.Model;

namespace TapAzImtahan.ViewModel
{
    public class MainMenuViewModel : INotifyPropertyChanged
    {

        private TapAzAccount loggedTapAzAccount = new TapAzAccount("sasaki", null, null, null, null);


        public TapAzAccount LoggedTapAzAccount
        {
            get { return loggedTapAzAccount; }
            set { loggedTapAzAccount = value; OnPropertyChanged(nameof(LoggedTapAzAccount)); }
        }

        public void messageExecute(object? parameter)
        {
            //ProductsViewModel.Products.Add(new Product("salam", "Qaqaw"));
            ProductsViewModel.TempProducts.Clear();
            ProductsViewModel.TempProducts.Add(new Product("blblblb","qlqlql"));
            ProductsViewModel.TempProducts.Add(new Product("blblblb","qlqlql"));
            ProductsViewModel.TempProducts.Add(new Product("blblblb","qlqlql"));
            ProductsViewModel.CurrentProducts=ProductsViewModel.TempProducts;

        }
        public RelayCommand ShowMessageCommand { get; set; }

        public ProductsViewModel? ProductVM { get => productVM; set { productVM = value; OnPropertyChanged(nameof(ProductVM)); } }
        public MainMenuViewModel()
        {
            try
            {
                ProductVM = new ProductsViewModel();
                ShowMessageCommand = new RelayCommand(messageExecute);
                ShowSelectedProductCommand = new RelayCommand(ShowButton);

                currentPage = ProductVM;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void ShowButton(object? parameter)
        {
            MessageBox.Show(LoggedTapAzAccount.Email);
        }

        public ObservableCollection<Product> MainCollection { get; set; }


        private object? currentPage;

        public object? CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; OnPropertyChanged(nameof(CurrentPage)); }
        }

        public ICommand? ShowSelectedProductCommand { get; set; }
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>() { new Product("Bmw", "teze", "Cars", "ImageUri") };

        private Product? selectedProduct;
        private ProductsViewModel? productVM;

        public Product? SelectedProduct { get => selectedProduct; set { selectedProduct = value; OnPropertyChanged(nameof(SelectedProduct)); } }

        public void ShowSelected(object? parameter) => MessageBox.Show(SelectedProduct.Id.ToString());

        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
