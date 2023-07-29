using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TapAzImtahan.Model
{
    interface ICarCategory
    {

        string? Make { get; }
        string? Model { get; }
        float? Motor { get; }
        string? Color { get; }
        string? Salon { get; }
    }
    public class CarCategory : Product, ICarCategory
    {
        private string? make;

        public CarCategory()
        {
        }

        public CarCategory(string? make, string? model, float? motor, string? color, string? salon, float price,string? description, string? imageUri) : base(price,description, imageUri)
        {
            Make = make;
            Model = model;
            Name = make + " " + model;
            Motor = motor;
            Category = "Car";
            Color = color;
            Salon = salon;
            Description += $"\nMotor:{motor}\nColor:{color}\nSalon:{salon}\n\n{description}";
        }
        public string? Make { get => make; set { make = value; OnPropertyChanged(nameof(Make)); } }
        public string? Model { get; set; }
        public float? Motor { get; set; }
        public string? Color { get; set; }
        public string? Salon { get; set; }
    }
}
