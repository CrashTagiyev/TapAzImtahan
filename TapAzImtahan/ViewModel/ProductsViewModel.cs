using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TapAzImtahan.Command;
using TapAzImtahan.Helper;
using TapAzImtahan.Model;

namespace TapAzImtahan.ViewModel
{
    public class ProductsViewModel : INotifyPropertyChanged
    {
        public  void message()
        {
        }
        public ProductsViewModel()
        {
            CurrentProducts = new ObservableCollection<Product>() { };
            Products = new ObservableCollection<Product>() {new CarCategory("Mercedes" ,"S500" ,1500 ,"black" ,"Deri" ,"Ela masindi" , "C:\\Users\\Lenova\\Source\\Repos\\TapAzImtahannn\\TapAzImtahan\\Car Pistures\\indir (7).jpeg") };
            TempProducts = new ObservableCollection<Product>() { };
            carMarkas = new ObservableCollection<CarMarkaClass>();
            selectedCarMarka = new CarMarkaClass();
            OtherNewProduct = new Product();
            FillCardMarkaModels();
            FillColors();
            CurrentProducts = Products;
            ShowSelectedProductCommand = new RelayCommand(ShowSelectedProduct);
            AddCommand = new RelayCommand(AddProduct, CanAddProduct);
            OnlyCarsCommand = new RelayCommand(ShowOnlyCars);
            EveryProducCommand = new RelayCommand(ShowAllProducts);
            ImageFileDialogCommand = new RelayCommand(ImageFileDialogExecute);
            AddBewCommand = new RelayCommand(OpenCreationPanel);
            BackToMainMenuCommand = new RelayCommand(BackTOMainMenu);
        }
        public ICommand? ShowSelectedProductCommand { get; set; }
        public ICommand? BackToMainMenuCommand { get; set; }
        public ICommand? AddCommand { get; set; }
        public ICommand? EveryProducCommand { get; set; }
        public ICommand? OnlyCarsCommand { get; set; }
        public ICommand? OnlyElectronicsCommand { get; set; }
        public ICommand? OnlyPetsCommand { get; set; }
        public ICommand? ImageFileDialogCommand { get; set; }
        public ICommand? RegisterProductCommand { get; set; }
        public ICommand? AddBewCommand { get; set; }
        public ICommand ComboBoxSelectionChangedCommand { get; set; }

        public static ObservableCollection<Product> Products { get; set; }
        public static ObservableCollection<Product> CurrentProducts { get; set; }
        public static ObservableCollection<Product> TempProducts { get; set; }
        public ObservableCollection<string> ProductCategories { get; set; } = new ObservableCollection<string>() { "Car", "Other" };
        public ObservableCollection<string> CarSalonCategory { get; set; } = new ObservableCollection<string>() { "Koja", "Vlyur" };
        public ObservableCollection<CarMarkaClass> CarMarkas { get => carMarkas; set { carMarkas = value; OnPropertyChanged(nameof(CarMarkas)); } }
        public CarMarkaClass SelectedCarMarka { get => selectedCarMarka; set { selectedCarMarka = value; newCarProduct.Make = selectedCarMarka.name; OnPropertyChanged(nameof(SelectedCarMarka)); } }

        public ObservableCollection<string> NewProductCategories { get; set; } = new ObservableCollection<string>() { "Pet", "Electronics", "Home", "Work" };

        private int selectedCarModelIndex = -1;

        public int SelectedCarModelIndex
        {
            get { return selectedCarModelIndex; }
            set { selectedCarModelIndex = value; newCarProduct.Model = SelectedCarMarka?.models?[SelectedCarModelIndex].name; OnPropertyChanged(nameof(SelectedCarModelIndex)); }

        }


        public ObservableCollection<ColorsJson> Colors { get => colors; set { colors = value; OnPropertyChanged(nameof(Colors)); } }

        private ColorsJson selectedColor;

        public ColorsJson SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; newCarProduct.Color = SelectedColor.name; OnPropertyChanged(nameof(SelectedColor)); }
        }
        private Product otherNewProduct;

        public Product OtherNewProduct
        {
            get { return otherNewProduct; }
            set { otherNewProduct = value; OnPropertyChanged(nameof(OtherNewProduct)); }
        }


        private string? selectedImageUri;
        public string? SelectedImageUri
        {
            get => selectedImageUri;
            set
            {
                if (selectedImageUri != value)
                {
                    selectedImageUri = value;
                    OnPropertyChanged(nameof(SelectedImageUri));
                }
            }
        }



        private CarCategory? newCarProduct = new CarCategory();

        public CarCategory? NewCarProduct
        {
            get => newCarProduct;
            set
            {
                newCarProduct = value; OnPropertyChanged(nameof(NewCarProduct));
            }
        }

        private string? selectedComboBoxItem;
        public string? SelectedComboBoxItem
        {
            get => selectedComboBoxItem;
            set
            {
                if (selectedComboBoxItem != value)
                {
                    selectedComboBoxItem = value;
                    OnPropertyChanged(nameof(SelectedComboBoxItem));
                    OnSelectionChanged(value);
                }
            }
        }

        public void OnSelectionChanged(string? value)
        {
            if (value == "Car")
            {

                OtherProductVisibility = Visibility.Hidden;
                CarProductCreateSelectionVisibility = Visibility.Visible;
            }

            if (value == "Other")
            {
                CarProductCreateSelectionVisibility = Visibility.Hidden;
                OtherProductVisibility = Visibility.Visible;
            }
        }


        private Product? selectedProduct;

        public Product? SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value; OnPropertyChanged(nameof(SelectedProduct));

                if (SelectedProduct != null)
                {
                    ProductLWvisibility = Visibility.Hidden;
                }
            }
        }
        private Visibility? carProductCreateSelectionVisibility = Visibility.Hidden;
        public Visibility? CarProductCreateSelectionVisibility
        {
            get { return carProductCreateSelectionVisibility; }
            set { carProductCreateSelectionVisibility = value; OnPropertyChanged(nameof(CarProductCreateSelectionVisibility)); }
        }

        private Visibility? otherProductVisibility = Visibility.Hidden;
        public Visibility? OtherProductVisibility
        {
            get { return otherProductVisibility; }
            set { otherProductVisibility = value; OnPropertyChanged(nameof(OtherProductVisibility)); }
        }

        private Visibility? productLWvisibility = Visibility.Visible;
        public Visibility? ProductLWvisibility
        {
            get { return productLWvisibility; }
            set { productLWvisibility = value; OnPropertyChanged(nameof(ProductLWvisibility)); }
        }

        private Visibility? productCreationPanelVisibility = Visibility.Hidden;
        public Visibility? ProductCreationPanelVisibility
        {
            get { return productCreationPanelVisibility; }
            set { productCreationPanelVisibility = value; OnPropertyChanged(nameof(ProductCreationPanelVisibility)); }
        }
        private Visibility? productManagementPanelVisibility = Visibility.Visible;
        public Visibility? ProductManagementPanelVisibility
        {
            get { return productManagementPanelVisibility; }
            set { productManagementPanelVisibility = value; OnPropertyChanged(nameof(ProductManagementPanelVisibility)); }
        }

        private Visibility? selectedProductLWvisibility = Visibility.Visible;
        public Visibility? SelectedProductLWvisibility
        {
            get { return selectedProductLWvisibility; }
            set { selectedProductLWvisibility = value; OnPropertyChanged(nameof(SelectedProductLWvisibility)); }
        }

        private bool isEnable = true;
        private ObservableCollection<Product> currentProducts;
        private ObservableCollection<CarMarkaClass> carMarkas;
        private CarMarkaClass selectedCarMarka;
        private ObservableCollection<ColorsJson> colors = new ObservableCollection<ColorsJson>();

        public bool IsEnable
        {
            get { return isEnable; }
            set
            {
                isEnable = value; OnPropertyChanged(nameof(IsEnable));

            }
        }

        public void ShowOnlyCars(object? paarameter)
        {
            TempProducts = new ObservableCollection<Product>();
            for (int i = 0; i < Products.Count; i++)
            {
                if (Products[i].Category == "Car")
                {
                    TempProducts.Add(Products[i]);
                }
            }
            CurrentProducts = TempProducts;
        }
        public void ShowAllProducts(object? paarameter)
        {
            TempProducts = new ObservableCollection<Product>();
            for (int i = 0; i < Products.Count; i++)
            {

                TempProducts.Add(Products[i]);
            }
            CurrentProducts = TempProducts;
        }

        public void ImageFileDialogExecute(object? image)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.png;*.jpeg;*.jpg;*.gif)|*.png;*.jpeg;*.jpg;*.gif|All files (*.*)|*.*",
                Title = "Select an Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;
                // Here, you can use the selectedImagePath as the URI for your image.
                // For example, if you are displaying the image in an Image control:
                // imageControl.Source = new BitmapImage(new Uri(selectedImagePath));
                SelectedImageUri = selectedImagePath;
                newCarProduct.ImageUri = selectedImageUri;
            }
        }
        public void OpenCreationPanel(object? parameter)
        {
            productManagementPanelVisibility= Visibility.Hidden;
            ProductCreationPanelVisibility = Visibility.Visible;

        }
        public void BackTOMainMenu(object? parameter)
        {
            ProductCreationPanelVisibility= Visibility.Hidden;
            ProductManagementPanelVisibility = Visibility.Visible;
        }
        public void AddProduct(object? parameter)
        {

            if (selectedComboBoxItem != null && selectedComboBoxItem == "Car")
            {
                Products.Add(new CarCategory(newCarProduct.Make, newCarProduct.Model, newCarProduct.Motor, newCarProduct.Color, newCarProduct.Salon, newCarProduct.Description, newCarProduct.ImageUri));
                MessageBox.Show("New Car Product was successfully created");
            }
            else if (selectedComboBoxItem != null && selectedComboBoxItem == "Car")
            {
                Products.Add(new Product(OtherNewProduct.Name, OtherNewProduct.Description, OtherNewProduct.Category, OtherNewProduct.ImageUri));
            }

        }
        public bool CanAddProduct(object? parameter)
        {
            if (selectedComboBoxItem == "Car")
            {
                if (NewCarProduct.Make != null && NewCarProduct.Model != null && NewCarProduct.Motor != null && NewCarProduct.Color != null && NewCarProduct.Salon != null && NewCarProduct.Description != null && NewCarProduct.ImageUri != null)
                {
                    return true;
                }
            }
            else if (SelectedComboBoxItem == "Other")

            {
                if (OtherNewProduct.Name != null && OtherNewProduct.Description != null && OtherNewProduct.ImageUri != null)
                {
                    return true;
                }
            }
            return false;
        }
        public string ReadEmbeddedJsonFile(string fileName)
        {
            string result = string.Empty;

            // Get the assembly where the embedded resource is located
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Replace 'YourNamespace' with the namespace where the JSON file is located
            string resourceName = "TapAzImtahan.JsonFiles." + fileName;

            // Open the embedded resource stream
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }
            return result;
        }

        public void FillCardMarkaModels()
        {
            string jsonData = ReadEmbeddedJsonFile("cars.json");
            List<CarMarkaClass> carsList = JsonSerializer.Deserialize<List<CarMarkaClass>>(jsonData);

            foreach (var car in carsList)
            {
                CarMarkas.Add(car);
            }
        }
        public void FillColors()
        {
            string jsonData = ReadEmbeddedJsonFile("colors.json");
            List<ColorsJson> colorsList = JsonSerializer.Deserialize<List<ColorsJson>>(jsonData);

            foreach (var color in colorsList)
            {
                Colors.Add(color);
            }
        }
        public void OpenPRoductRegistration(object? parameter)
        {
        }


        public void ShowSelectedProduct(object? parameter)
        {
            SelectedProduct = null;
            ProductLWvisibility = Visibility.Visible;
        }

        public event EventHandler ConditionChanged;

        protected virtual void OnConditionChanged()
        {
            ConditionChanged?.Invoke(this, EventArgs.Empty);
        }
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
