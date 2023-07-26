﻿using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public ProductsViewModel()
        {
            CurrentProducts = new ObservableCollection<Product>() { };
            Products = new ObservableCollection<Product>() { };
            TempProducts = new ObservableCollection<Product>() { };
            carMarkas = new ObservableCollection<CarMarkaClass>();
            selectedCarMarka = new CarMarkaClass();
            FillCardMarkaModels();
            CurrentProducts = Products;
            ShowSelectedProductCommand = new RelayCommand(ShowSelectedProduct);
            AddCommand = new RelayCommand(AddProduct, CanAddProduct);
            OnlyCarsCommand = new RelayCommand(ShowOnlyCars);
            EveryProducCommand = new RelayCommand(ShowAllProducts);
        }
        public ICommand? ShowSelectedProductCommand { get; set; }
        public ICommand? AddCommand { get; set; }
        public ICommand? EveryProducCommand { get; set; }
        public ICommand? OnlyCarsCommand { get; set; }
        public ICommand? OnlyElectronicsCommand { get; set; }
        public ICommand? OnlyPetsCommand { get; set; }
        public ICommand? RegisterProductCommand { get; set; }
        public ICommand ComboBoxSelectionChangedCommand { get; set; }

        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> CurrentProducts { get => currentProducts; set { currentProducts = value; OnPropertyChanged(nameof(currentProducts)); } }
        public ObservableCollection<Product> TempProducts { get; set; }
        public ObservableCollection<string> ProductCategories { get; set; } = new ObservableCollection<string>() { "Car", "Electronic", "Pet" };
        public ObservableCollection<CarMarkaClass> CarMarkas { get => carMarkas; set { carMarkas = value; OnPropertyChanged(nameof(CarMarkas)); } }
        public CarMarkaClass SelectedCarMarka { get => selectedCarMarka; set { selectedCarMarka = value; OnPropertyChanged(nameof(SelectedCarMarka)); } }

        public ObservableCollection<string> Colors { get; set; } = new ObservableCollection<string>();



        private void OnComboBoxSelectionChanged(string? value)
        {
            MessageBox.Show(value);
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
                    OnComboBoxSelectionChanged(selectedComboBoxItem);
                }
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


        private Visibility? productLWvisibility = Visibility.Hidden;
        public Visibility? ProductLWvisibility
        {
            get { return productLWvisibility; }
            set { productLWvisibility = value; OnPropertyChanged(nameof(ProductLWvisibility)); }
        }


        private Visibility? selectedProductLWvisibility = Visibility.Hidden;
        public Visibility? SelectedProductLWvisibility
        {
            get { return selectedProductLWvisibility; }
            set { selectedProductLWvisibility = value; OnPropertyChanged(nameof(SelectedProductLWvisibility)); }
        }

        private bool isEnable = true;
        private ObservableCollection<Product> currentProducts;
        private ObservableCollection<CarMarkaClass> carMarkas;
        private CarMarkaClass selectedCarMarka;

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

        public void AddProduct(object? parameter)
        {
            if (NewCarProduct.Make == null)
            {
                MessageBox.Show(NewCarProduct.Make);
            }
            if (selectedComboBoxItem != null && selectedComboBoxItem == "Car")
            {
                Products.Add(new CarCategory(newCarProduct.Make, newCarProduct.Model, newCarProduct.Motor, newCarProduct.Color, newCarProduct.Salon, newCarProduct.Description, newCarProduct.ImageUri));
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
            List<CarMarkaClass> carsList = JsonSerializer.Deserialize<List<CarMarkaClass>>(jsonData);

            foreach (var car in Colors)
            {
                Colors.Add(car);
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