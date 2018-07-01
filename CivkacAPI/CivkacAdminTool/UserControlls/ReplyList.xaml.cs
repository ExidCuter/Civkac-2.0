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
    /// Interaction logic for ReplyList.xaml
    /// </summary>
    public partial class ReplyList : UserControl {
        private MainWindow main;
        public ReplyList()
        {
            InitializeComponent();
            foreach (Window win in App.Current.Windows)
            {
                if (win is MainWindow w)
                {
                    main = w;
                    break;
                }
            }
        }

        private void EditClicked(object sender, RoutedEventArgs e) {
            if (RepliesList.SelectedItem is Reply r)
            {
                main.editReply(r);
            }
        }

        private void DeleteClicked(object sender, RoutedEventArgs e)
        {
            if (RepliesList.SelectedItem is Reply r)
            {
                main.deleteReply(r);
            }
        }
    }
}
