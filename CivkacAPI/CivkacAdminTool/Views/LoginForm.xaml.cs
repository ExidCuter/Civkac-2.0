using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CivkacAdminTool.Models;
using CivkacAdminTool.REST;

namespace CivkacAdminTool.Views
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        private bool success = false;
        public LoginForm()
        {
            InitializeComponent();
            Closing += OnWindowClosing;
        }

        private void LogInClicked(object sender, RoutedEventArgs e)
        { 
            //database
            if (RealData.isAdmin(username.Text, password.Password))
            {
                success = true;
                MainWindow.username = username.Text;
                MainWindow.password = password.Password;
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect username or password! Try again!", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
                password.Clear();
            }
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (!success)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = false;
                
            }

        }
    }
}
