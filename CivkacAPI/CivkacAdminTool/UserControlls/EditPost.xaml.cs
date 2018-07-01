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
using CivkacAdminTool.Views;

namespace CivkacAdminTool.UserControlls
{
    /// <summary>
    /// Interaction logic for EditPost.xaml
    /// </summary>
    public partial class EditPost : UserControl
    {
        public EditPost()
        {
            InitializeComponent();
        }

        public bool validate()
        {
            if (author.Text != "")
            {
                if (Text.Text != "" && Text.Text.Length <= 300)
                {
                    return true;
                }
                MessageBox.Show("Text not ok");
            }
            MessageBox.Show("Author not ok");

            return false;
        }

        private void SaveClick(object sender, RoutedEventArgs e) {
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
