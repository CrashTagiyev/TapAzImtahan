using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TapAzImtahan.Model;
using TapAzImtahan.View;
using TapAzImtahan.ViewModel;

namespace TapAzImtahan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TapAzAccount? LoggedAccount { get; set; }



        public MainWindow()
        {
            try
            {

                InitializeComponent();


                LoginPageViewModel loginViewModel = new LoginPageViewModel();
                LoggedAccount = loginViewModel.LoggedAccountLoginVM;
                loginViewModel.MainFrame = LogFrame;
                LoginPage loginPage = new LoginPage();
                loginPage.DataContext = loginViewModel;

                LogFrame.NavigationService.Navigate(loginPage);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


    }
}
