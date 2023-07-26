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
using TapAzImtahan.Helper;

namespace TapAzImtahan.ViewModel
{

    public class LoginPageViewModel : INotifyPropertyChanged
    {
        private TapAzAccount? loggedAccountLoginVM;

        public TapAzAccount? LoggedAccountLoginVM
        {
            get { return loggedAccountLoginVM; }
            set { loggedAccountLoginVM = value; }
        }

        public LoginPageViewModel()
        {
            RegisterNewAccountCommand = new RelayCommand(RegisterNewAccountExecute);
            LogInCommand = new RelayCommand(LoginExecute, canLogin);
            RegisterCommand = new RelayCommand(RegisterExecute);
            GoBackToLoginCommand = new RelayCommand(GoBackToLoginExecute);
        }

        public ObservableCollection<TapAzAccount> Accounts { get; set; } = new ObservableCollection<TapAzAccount>() {
        new TapAzAccount("qwe","qwe","Emil","Surname",new DateTime(1994,6,17)),
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
        public ICommand GoBackToLoginCommand { get; set; }

        private Visibility? loginPageVisibility = Visibility.Visible;


        private MainMenuViewModel mainMenuViewModel;

        public MainMenuViewModel MainMenuViewModel
        {
            get { return mainMenuViewModel; }
            set { mainMenuViewModel = value; OnPropertyChanged(nameof(MainMenuViewModel)); }
        }
        private TapAzMainMenu tapAzMM;

        public TapAzMainMenu TapAzMM
        {
            get { return tapAzMM; }
            set { tapAzMM = value; OnPropertyChanged(nameof(TapAzMM)); }
        }

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
        private DateTime _birthday = new DateTime(2021, 1, 1);
        public DateTime BirthDay { get => _birthday; set { _birthday = value; OnPropertyChanged(nameof(BirthDay)); } }



        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void LoginExecute(object? parameter)
        {
            try
            {

                for (int i = 0; i < Accounts.Count; i++)
                {
                    if (Login == Accounts[i].Password && LoginPassword == Accounts[i].Password)
                    {
                        loggedAccountLoginVM = Accounts[i];
                        MainMenuViewModel = new MainMenuViewModel();
                        TapAzMM = new TapAzMainMenu();
                        MainMenuViewModel.LoggedTapAzAccount = loggedAccountLoginVM;
                        TapAzMM.DataContext = MainMenuViewModel;
                        MainFrame.NavigationService.Navigate(TapAzMM);
                        return;
                    }
                }
                throw new Exception("Account did not found.Login or password is wrong");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
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
                if (Email == null || Password == null || Name == null || Surname == null || BirthDay.Year == 0001) throw new Exception("Fill the every box for registration");
                else
                {
                    foreach (var item in Accounts)
                    {
                        if (Email == item.Email) throw new Exception("This Email is already taken");
                    }
                    Accounts.Add(new TapAzAccount(Email, Password, Name, Surname, BirthDay));
                    RegisterSPVisibility = Visibility.Hidden;
                    LoginStackPanelVisibility = Visibility.Visible;
                    MessageBox.Show("Account succeccfully created,you can login now!");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void GoBackToLoginExecute(object? parameter)
        {
            RegisterSPVisibility = Visibility.Hidden;
            LoginStackPanelVisibility = Visibility.Visible;
        }
    }
}
