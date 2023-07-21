using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using TapAzImtahan.View;
using TapAzImtahan.Model;
using TapAzImtahan.Command;
using TapAzImtahan.ViewModel;

namespace TapAzImtahan.ViewModel
{

    public class LoginPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TapAzAccount> Accounts { get; set; } = new ObservableCollection<TapAzAccount>() {
        new TapAzAccount("e@gmail.com","5458","Emil","Surname",new DateTime(1994,6,17)),
        new TapAzAccount("D@gmail.com","3333","Duman","Vezirov",new DateTime(1996,2,23)),
        new TapAzAccount("N@gmail.com","2323","Elnur","Haciyev",new DateTime(1999,9,19))
        };

        private Frame mainFrame;

        public Frame MainFrame
        {
            get { return mainFrame; }
            set
            {
                mainFrame = value;
                OnPropertyChanged(nameof(MainFrame));
            }
        }


        public ICommand? LogInCommand { get; set; }

        public ICommand RegisterCommand { get; set; }
        public ICommand RegisterNewAccountCommand { get; set; }

        private Visibility? loginPageVisibility = Visibility.Visible;

        public Visibility? LoginPageVisibility
        {
            get { return loginPageVisibility; }
            set
            {
                loginPageVisibility = value; OnPropertyChanged(nameof(LoginPageVisibility));
            }
        }
        private Visibility? loginStackPanelVisibility = Visibility.Visible;

        public Visibility? LoginStackPanelVisibility
        {
            get { return loginStackPanelVisibility; }
            set
            {
                loginStackPanelVisibility = value; OnPropertyChanged(nameof(LoginStackPanelVisibility));
            }
        }

        private Visibility? registerSPVisibility = Visibility.Hidden;

        public Visibility? RegisterSPVisibility
        {
            get { return registerSPVisibility; }
            set
            {
                registerSPVisibility = value; OnPropertyChanged(nameof(RegisterSPVisibility));
            }
        }


        private string? login;

        public string? Login
        {
            get { return login; }
            set
            {
                login = value; OnPropertyChanged(nameof(Login));
            }
        }

        private string? loginPassword;


        public string? LoginPassword
        {
            get { return loginPassword; }
            set
            {
                loginPassword = value; OnPropertyChanged(nameof(LoginPassword));
            }
        }
        private bool isPassRight;

        public bool IsPassRight
        {
            get { return isPassRight; }
            set { isPassRight = value; OnPropertyChanged(nameof(IsPassRight)); }
        }
        //
        private string? _email;
        public string? Email { get => _email; set { _email = value; OnPropertyChanged(nameof(Email)); } }
        //
        private string? _name;
        public string? Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }
        //
        private string? _surname;
        public string? Surname { get => _surname; set { _surname = value; OnPropertyChanged(nameof(Surname)); } }
        //
        private string? _password;
        public string? Password { get => _password; set { _password = value; OnPropertyChanged(nameof(Password)); } }
        //
        private DateTime _birthday=new DateTime(2021,1,1);
        public DateTime BirthDay { get => _birthday; set { _birthday = value; OnPropertyChanged(nameof(BirthDay)); } }



        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LoginPageViewModel()
        {
            RegisterNewAccountCommand = new RelayCommand(RegisterNewAccountExecute);
            LogInCommand = new RelayCommand(LoginExecute, canLogin);
            RegisterCommand = new RelayCommand(RegisterExecute);
        }

        public void LoginExecute(object? parameter)
        {
            MainFrame.NavigationService.Navigate(new TapAzMainMenu());
            //LoginPageVisibility = Visibility.Hidden;
        }

        bool canLogin(object? parameter) => true;

        public void RegisterExecute(object? parameter)
        {
            RegisterSPVisibility = Visibility.Visible;
            LoginStackPanelVisibility = Visibility.Hidden;
        }

        public void RegisterNewAccountExecute(object? parameter)
        {
            try
            {

                if (Email == "" || Password == "" || Name == "" || Surname == "" || BirthDay.Year == 0001) throw new Exception("Fill the everything for registration");
                RegisterSPVisibility = Visibility.Hidden;
                LoginStackPanelVisibility = Visibility.Visible;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
