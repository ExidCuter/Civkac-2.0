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
    /// Interaction logic for PostList.xaml
    /// </summary>
    public partial class PostList : UserControl {
        private MainWindow main;
        public PostList()
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

        private void Posts_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (Posts.SelectedItem is Post p) {
                main.showPostsReplies(p);
            }
        }

        private void EditClicked(object sender, RoutedEventArgs e) {
            if (Posts.SelectedItem is Post p)
            {
                main.editPost(p);
            }
        }

        private void DeleteClicked(object sender, RoutedEventArgs e)
        {
            if (Posts.SelectedItem is Post p)
            {
                main.deletePost(p);
            }
        }
    }
}
