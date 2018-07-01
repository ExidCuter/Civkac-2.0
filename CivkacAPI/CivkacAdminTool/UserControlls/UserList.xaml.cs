using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CivkacAdminTool.Models;

namespace CivkacAdminTool.UserControlls {
    /// <summary>
    /// Interaction logic for UserList.xaml
    /// </summary>
    public partial class UserList : UserControl {
        private MainWindow main;

        public UserList() {
            InitializeComponent();
            foreach (Window win in App.Current.Windows)
            {
                if (win is MainWindow w) {
                    main = w;
                    break;
                }
            }
        }

        private void Users_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (Users.SelectedItem is User u) {
                main.showUsersPosts(u);
            }           
        }

        private void Edit_Click(object sender, RoutedEventArgs e) {
            if (Users.SelectedItem is User u) {
                main.editUser(u);
            }
            Users.Items.Refresh();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Users.SelectedItem is User u)
            {
                main.deleteUser(u);
            }
            Users.Items.Refresh();
        }

    }
}