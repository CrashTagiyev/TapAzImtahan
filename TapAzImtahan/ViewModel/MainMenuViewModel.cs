using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using TapAzImtahan.Command;

namespace TapAzImtahan.ViewModel
{
    public class MainMenuViewModel: INotifyPropertyChanged
    {
       public ICommand? _command {  get; set; } 
       public MainMenuViewModel() {

            _command = new RelayCommand(showtext);
        }

        private string? txt;

        public string? Txt
        {
            get { return txt; }
            set { txt = value;OnPropertyChanged(nameof(Txt)); }
        }

        public void showtext(object? parameter)
        {
            MessageBox.Show(Txt);
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
