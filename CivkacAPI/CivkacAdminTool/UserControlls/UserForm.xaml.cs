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

namespace CivkacAdminTool.UserControlls
{
    /// <summary>
    /// Interaction logic for UserForm.xaml
    /// </summary>
    public partial class UserForm : UserControl
    {
        public UserForm()
        {
            InitializeComponent();
        }

        public bool validate()
        {
            if (username.Text != "" && username.Text.Length<=45)
            {
                if (handle.Text != "" && handle.Text.Length<=20)
                {
                    if (password.Password != "" && password.Password.Length <= 45)
                    {
                        if (image.Text != "" && image.Text.Length<=300)
                        {
                            return true;
                        }
                        MessageBox.Show("Image not ok");
                    }
                    MessageBox.Show("Password not ok");
                }
                MessageBox.Show("Handle not ok");
            }
            MessageBox.Show("username not ok");

            return false;
        }
    }

}
