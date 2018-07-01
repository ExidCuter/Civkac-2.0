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

namespace CivkacAdminTool.UserControlls
{
    /// <summary>
    /// Interaction logic for PostForm.xaml
    /// </summary>
    public partial class PostForm : UserControl
    {
        public PostForm()
        {
            InitializeComponent();
        }

        public bool validate()
        {
            if (Author.SelectedItem is User)
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
    }
}