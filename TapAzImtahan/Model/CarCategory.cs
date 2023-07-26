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

        public CarCategory(string? make, string? model, float? motor, string? color, string? salon, string? description, string? imageUri) : base(description, imageUri)
        {
            Make = make;
            Model = model;
            Motor = motor;
            Color = color;
            Salon = salon;
            Description += $"\nMarka:{Make}\n\nModel:{Model}\nMotor:{Motor}\nColor:{Color}\nSalon:{Salon}\n\n{Description}";
        }
        public string? Make { get => make; set { make = value; OnPropertyChanged(nameof(Make)); } }
        public string? Model { get; set; }
        public float? Motor { get; set; }
        public string? Color { get; set; }
        public string? Salon { get; set; }
    }
}
