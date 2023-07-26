using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TapAzImtahan.Model
{

    public static class StaticID
    {
        public static int SID { get; set; } = 0;
    }
    public class Product : INotifyPropertyChanged
    {
        public Product() { }
        public Product(string? name, string? description, string? category, string? imageUri)
        {
            Name = name;
            Description = description;
            Category = category;
            ImageUri = imageUri;
        }
        public Product(string? description, string? imageUri)
        {
            Description = description;
            ImageUri = imageUri;
        }
        public Product(string? name, string? description, string? category, Image? image)
        {
            Name = name;
            Description = description;
            Category = category;
            Image = image;
        }

        public Product(string? name, string? description, string? category, params Image[]? images)
        {
            Name = name;
            Description = description;
            Category = category;
            Images = images;
        }

        public int Id { get; set; } = ++StaticID.SID;
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? ImageUri { get; set; } = null;
        public Image? Image { get; set; } = null;
        public Image[]? Images { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
