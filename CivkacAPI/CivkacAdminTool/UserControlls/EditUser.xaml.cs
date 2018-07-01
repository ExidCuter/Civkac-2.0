using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using CivkacAdminTool.Views;

namespace CivkacAdminTool.UserControlls
{
    /// <summary>
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditUser : UserControl
    {
        public EditUser()
        {
            InitializeComponent();
        }

        public bool validate()
        {
            if (username.Text != "" && username.Text.Length <= 45)
            {
                if (handle.Text != "" && handle.Text.Length <= 20)
                {
                    if (image.Text != "" && image.Text.Length <= 300)
                    {
                        return true;
                    }
                    MessageBox.Show("Image not ok");
                }
                MessageBox.Show("Handle not ok");
            }
            MessageBox.Show("username not ok");

            return false;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (validate())
            {
                foreach (Window win in App.Current.Windows)
                {
                    if (win is EditView w)
                    {
                        w.save = true;
                        break;
                    }
                }
                MessageBox.Show("Changes will be saved after you close the window!");
            }
        }
    }
}