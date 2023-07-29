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
using System.Text.RegularExpressions;
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

        public ProductsViewModel()
        {
            CurrentProducts = new ObservableCollection<Product>() { };
            Products = new ObservableCollection<Product>() { new CarCategory("Mercedes", "S500", 1500, "Black", "Deri", 150000, "Ela masindi", "https://cdn.discordapp.com/attachments/1057964461455056936/1134866845854335066/bmw-m8-competition-gran-coupe-modelfinder.png.asset.1643111901255.png") };
            TempProducts = new ObservableCollection<Product>() { };
            carMarkas = new ObservableCollection<CarMarkaClass>();
            selectedCarMarka = new CarMarkaClass();
            OtherNewProduct = new Product();
            FillCardMarkaModels();
            FillColors();
            FakeProducts();
            OnlyCarsCommand = new RelayCommand(ShowOnlyCars);
            OnlyPetsCommand = new RelayCommand(ShowOnlyPet);
            OnlyElectronicsCommand = new RelayCommand(ShowOnlyElectronics);
            OnlyHomeCommand = new RelayCommand(ShowOnlyHomes);
            OnlyWorksCommand = new RelayCommand(ShowOnlyWorks);
            
            
            CurrentProducts = Products;
            ShowSelectedProductCommand = new RelayCommand(ShowSelectedProduct);
            AddCommand = new RelayCommand(AddProduct, CanAddProduct);
            EveryProducCommand = new RelayCommand(ShowAllProducts);

            ImageFileDialogCommand = new RelayCommand(ImageFileDialogExecute);

            AddBewCommand = new RelayCommand(OpenCreationPanel);
            BackToMainMenuCommand = new RelayCommand(BackTOMainMenu);
            SearchCommand = new RelayCommand(SearchExecute, canSearch);
        }
        public ICommand? ShowSelectedProductCommand { get; set; }
        public ICommand? BackToMainMenuCommand { get; set; }
        public ICommand? AddCommand { get; set; }
        public ICommand? SearchCommand { get; set; }
        public ICommand? EveryProducCommand { get; set; }
        public ICommand? OnlyCarsCommand { get; set; }
        public ICommand? OnlyPetsCommand { get; set; }
        public ICommand? OnlyElectronicsCommand { get; set; }
        public ICommand? OnlyHomeCommand { get; set; }
        public ICommand? OnlyWorksCommand { get; set; }
        public ICommand? ImageFileDialogCommand { get; set; }
        public ICommand? RegisterProductCommand { get; set; }
        public ICommand? AddBewCommand { get; set; }
        public ICommand ComboBoxSelectionChangedCommand { get; set; }

        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> CurrentProducts { get => currentProducts; set { currentProducts = value; OnPropertyChanged(nameof(CurrentProducts)); } }
        public ObservableCollection<Product> TempProducts { get; set; }
        public ObservableCollection<string> ProductCategories { get; set; } = new ObservableCollection<string>() { "Car", "Other" };
        public ObservableCollection<string> CarSalonCategory { get; set; } = new ObservableCollection<string>() { "Koja", "Vlyur" };
        public ObservableCollection<CarMarkaClass> CarMarkas { get => carMarkas; set { carMarkas = value; OnPropertyChanged(nameof(CarMarkas)); } }
        public CarMarkaClass SelectedCarMarka { get => selectedCarMarka; set { selectedCarMarka = value; newCarProduct.Make = selectedCarMarka.name; OnPropertyChanged(nameof(SelectedCarMarka)); } }
        public ObservableCollection<string> NewProductCategories { get; set; } = new ObservableCollection<string>() { "Car", "Pet", "Electronics", "Home", "Work" };

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
        private string? selectedCategory;
        public string? SelectedCategory
        {
            get => selectedCategory;
            set
            {
                if (selectedCategory != value)
                {
                    selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                    OnCategoryCHanged(value);
                }
            }
        }
        private int selectedCarModelIndex = -1;

        public int SelectedCarModelIndex
        {
            get { return selectedCarModelIndex; }
            set { selectedCarModelIndex = value; newCarProduct.Model = SelectedCarMarka?.models?[SelectedCarModelIndex].name; OnPropertyChanged(nameof(SelectedCarModelIndex)); }

        }
        public void OnCategoryCHanged(string? value)
        {
            TempProducts = new ObservableCollection<Product>();
            for (int i = 0; i < Products.Count; i++)
            {
                if (Products[i].Category == value)
                {
                    TempProducts.Add(Products[i]);
                }
            }
            CurrentProducts = TempProducts;
        }


        private void FakeProducts()
        {
            Products.Add(new CarCategory("Toyota", "Camry", 2.5f, "White", "Leather", 28000f, "A reliable family sedan.", "https://listings-prod.tcimg.net/listings/20633/07/65/4T1BZ1HK9JU506507/P3EGJM5ZSXDRC57KB37RB44K3A-cr-540.jpg"));
            Products.Add(new CarCategory("Honda", "Civic", 1.8f, "Black", "Cloth", 25000f, "Efficient and economical.", "https://bluesky.cdn.imgeng.in/cogstock-images/spincar-0f30092d6006c425afbfb24e90349416c0c715d4.jpg?imgeng=/w_400/"));
            Products.Add(new CarCategory("Ford", "Mustang", 5.0f, "Red", "Leather", 40000f, "A powerful sports car.", "https://www.svtperformance.com/attachments/73774845-cd2a-4ddd-b589-d19f93c09d9c-jpg.610122/"));
            Products.Add(new CarCategory("Nissan", "Altima", 2.0f, "Silver", "Leather", 27000f, "Comfortable and spacious.", "https://images.cdn.manheim.com/20230719142543-02423f00-b2d2-4642-895c-0d3e7614a0a1.jpg?size=w344h256"));
            Products.Add(new CarCategory("Chevrolet", "Malibu", 1.5f, "Blue", "Cloth", 26000f, "Great fuel efficiency.", "https://i.ytimg.com/vi/C7QXlnw_T5c/sddefault.jpg"));

            //Pet
            Products.Add(new Product("Pet Bed", 29.99f, "Cozy bed for your furry friend.", "Pet", "https://m.media-amazon.com/images/I/61gxx3o19RL._AC_SL1500_.jpg"));
            Products.Add(new Product("Dog Leash", 12.49f, "Durable leash for walking your dog.", "Pet", "https://ruffwear.com/cdn/shop/products/40354-Roamer-Leash-Granite-Gray.png?crop=center&height=550&v=1676490728&width=820"));
            Products.Add(new Product("Cat Food Bowl", 9.99f, "Stylish bowl for your cat's meals.", "Pet", "https://media.istockphoto.com/id/181896077/photo/cat-food-bowl.jpg?s=1024x1024&w=is&k=20&c=AWLCXsz07FC84BYVTh0f66gYf4k7HI11Pk4r_RNwuSM="));
            Products.Add(new Product("Bird Cage", 39.99f, "Spacious cage for your feathered companion.", "Pet", "https://imgprd19.hobbylobby.com/5/5f/15/55f154c59f55a00a0386035a1b18fc4b2e37e6eb/1400Wx1400H-1638592-0618.jpg"));
            Products.Add(new Product("Fish Tank", 24.99f, "Colorful aquarium for your fish.", "Pet", "https://sparkenzy.com/cdn/shop/products/fishtankcabint_603x700.png?v=1645785154"));

            //Home
            Products.Add(new Product("Scented Candle", 8.99f, "Relaxing lavender-scented candle.", "Home", "https://cdn.thewirecutter.com/wp-content/media/2021/08/scentedcandles-2048px-0S1A9970-2x1-1.jpg?auto=webp&quality=75&crop=2:1&width=1024"));
            Products.Add(new Product("Picture Frame", 14.99f, "Elegant frame for your cherished memories.", "Home", "https://cf.ltkcdn.net/antiques/images/orig/262393-1600x1030-antique-picture-frame-styles-values.jpg"));
            Products.Add(new Product("Throw Blanket", 19.99f, "Soft and warm blanket for your couch.", "Home", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSyIcJWtU-5QXWjHfJG7zFAXLnL3w4UvpLeJJxp4qP8HA&s    "));
            Products.Add(new Product("Cutlery Set", 34.99f, "Complete set of kitchen utensils.", "Home", "https://i5.walmartimages.com/asr/c83d6843-9ee1-4967-b6b8-5c3ce55b6a62.60662f421e4fdd10a7194d0e3b533e39.jpeg"));
            Products.Add(new Product("Plant Pot", 11.49f, "Decorative pot for your indoor plants.", "Home", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcROk16c5k186tNAtzxfEFv_sgrtQ60vZOuDE8Z1rf1F&s"));

           //Electronics
            Products.Add(new Product("Wireless Mouse", 19.99f, "Responsive wireless mouse for your computer.", "Electronics", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRSgMuSKmKDMwGtcIfko-LZfmOvB9l9EMBi5KPtQcPw&s"));
            Products.Add(new Product("Power Bank", 29.99f, "Portable power bank to charge your devices on the go.", "Electronics", "https://maxi.az/upload/iblock/265/tico0n87l28z5a48hoelodpysdlwi7o2/portativ_enerji_y_ma_cihaz_power_bank_ttec_powerslim_lcd_20_000_mah_powerbank_with_usb_c_input_outpu.webp"));
            Products.Add(new Product("Bluetooth Headphones", 59.99f, "High-quality headphones for immersive audio.", "Electronics", "https://m.media-amazon.com/images/I/61-g7m+90eL.jpg"));
            Products.Add(new Product("Smartphone Stand", 12.99f, "Adjustable stand for your smartphone.", "Electronics", "https://www.rollingstone.com/wp-content/uploads/2020/05/Lamicall-Adjustable-Cell-Phone-Stand-FI.jpg?w=996"));
            Products.Add(new Product("USB-C Cable", 8.49f, "Durable USB-C cable for fast charging.", "Electronics", "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/HN892?wid=4321&hei=3557&fmt=jpeg&qlt=95&.v=1562973652891"));

            //Work
            Products.Add(new Product("Pen Set", 7.99f, "Professional pen set for your office needs.", "Work", "https://m.media-amazon.com/images/I/61G7vJndSSL.jpg"));
            Products.Add(new Product("Desk Lamp", 24.99f, "Adjustable desk lamp for better lighting.", "Work", "https://tekled.co.uk/cdn/shop/products/atlas-architect-swing-arm-black-desk-lamp-with-clip-e27-main-image-926202.jpg?v=1680525122"));
            Products.Add(new Product("Notebook Organizer", 15.49f, "Organizer for your notebooks and stationery.", "Work", "https://assets-production.mochi.media/products/84402/photos/square_thumbnail_247575-myo-a5-notepad-planner-organizer-v2-bd18e8.jpg"));
            Products.Add(new Product("Document Holder", 9.99f, "Ergonomic holder for your important papers.", "Work", "https://images.kkeu.de/is/image/BEG/Office_Equipment/Add-on_desktops/Document_holder_pdplarge-mrd--000062182534_PRD_org_all.jpg"));
            Products.Add(new Product("Whiteboard Marker Set", 6.49f, "Vibrant markers for your whiteboard.", "Work", "https://cdn.modulor.de/products/lcr28/1_1920_1920_r_staedtler_lumocolor_whiteboard_marker_set_351_wp8.jpg"));

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
        private string? searchedString;
        public string? SearchedString
        {
            get => searchedString;
            set
            {
                if (searchedString != value)
                {
                    searchedString = value;
                    OnPropertyChanged(nameof(SearchedString));
                }
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

        public void SearchExecute(object? parameter)
        {
            TempProducts = new ObservableCollection<Product>();
            for (int i = 0; i < Products.Count; i++)
            {
                string pattern = Regex.Escape(SearchedString);
                string searching = Products[i].Name+Products[i].Description;
                // Use Regex.Match to find the first match in the target string
                Match match = Regex.Match(searching, pattern,RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    TempProducts.Add(Products[i]);
                }
            }
            CurrentProducts = TempProducts;
        }
        public bool canSearch(object? parameter)
        {
            return SearchedString != null;
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
        public void ShowOnlyPet(object? paarameter)
        {
            TempProducts = new ObservableCollection<Product>();
            for (int i = 0; i < Products.Count; i++)
            {
                if (Products[i].Category == "Pet")
                {
                    TempProducts.Add(Products[i]);
                }
            }
            CurrentProducts = TempProducts;
        }
        public void ShowOnlyElectronics(object? paarameter)
        {
            TempProducts = new ObservableCollection<Product>();
            for (int i = 0; i < Products.Count; i++)
            {
                if (Products[i].Category == "Electronics")
                {
                    TempProducts.Add(Products[i]);
                }
            }
            CurrentProducts = TempProducts;
        }
        public void ShowOnlyHomes(object? paarameter)
        {
            TempProducts = new ObservableCollection<Product>();
            for (int i = 0; i < Products.Count; i++)
            {
                if (Products[i].Category == "Home")
                {
                    TempProducts.Add(Products[i]);
                }
            }
            CurrentProducts = TempProducts;
        }
        public void ShowOnlyWorks(object? paarameter)
        {
            TempProducts = new ObservableCollection<Product>();
            for (int i = 0; i < Products.Count; i++)
            {
                if (Products[i].Category == "Work")
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
                otherNewProduct.ImageUri = selectedImageUri;    
            }
        }
        public void OpenCreationPanel(object? parameter)
        {
            productManagementPanelVisibility = Visibility.Hidden;
            ProductCreationPanelVisibility = Visibility.Visible;

        }
        public void BackTOMainMenu(object? parameter)
        {
            ProductCreationPanelVisibility = Visibility.Hidden;
            ProductManagementPanelVisibility = Visibility.Visible;
        }
        public void AddProduct(object? parameter)
        {

            if (selectedComboBoxItem != null && selectedComboBoxItem == "Car")
            {
                Products.Add(new CarCategory(NewCarProduct?.Make, NewCarProduct?.Model, NewCarProduct?.Motor, NewCarProduct?.Color, NewCarProduct?.Salon, NewCarProduct.Price, NewCarProduct?.Description, NewCarProduct?.ImageUri));
                MessageBox.Show("New Car Product was successfully created");
            }
            else if (selectedComboBoxItem != null && selectedComboBoxItem == "Other")
            {
                Products.Add(new Product(OtherNewProduct.Name, OtherNewProduct.Price, OtherNewProduct.Description, OtherNewProduct.Category, OtherNewProduct.ImageUri));
                MessageBox.Show($"New {OtherNewProduct.Category} Product was successfully created");
            }

        }
        public bool CanAddProduct(object? parameter)
        {
            if (selectedComboBoxItem == "Car")
            {
                if (newCarProduct.Make != null && newCarProduct.Model != null && newCarProduct.Motor != null && newCarProduct.Color != null && newCarProduct.Salon != null && newCarProduct.Price > 0 && newCarProduct.Description != null && newCarProduct.ImageUri != null)
                {
                    return true;
                }
            }
            else if (SelectedComboBoxItem == "Other")

            {
                if (OtherNewProduct.Name != null && OtherNewProduct.Price > 0 && OtherNewProduct.Description != null && OtherNewProduct.ImageUri != null)
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
        public string ReadEmbeddedImagePath(string ImageName)
        {
            string result = string.Empty;

            // Get the assembly where the embedded resource is located
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Replace 'YourNamespace' with the namespace where the JSON file is located
            string resourceName = "TapAzImtahan.Car Pistures." + ImageName;

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
