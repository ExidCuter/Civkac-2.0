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
using CivkacAdminTool.REST;
using CivkacAdminTool.Tests;
using CivkacAdminTool.Views;
using Flurl.Http;
using Newtonsoft.Json;

namespace CivkacAdminTool {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private FakeData FakeData = new FakeData();

        public static String username = "";
        public static String password = "";

        public MainWindow() {
            InitializeComponent();
            this.Visibility = Visibility.Hidden;
            new LoginForm().ShowDialog();
            this.Visibility = Visibility.Visible;


            usersList.Users.ItemsSource = RealData.getUsers();
        }


        public void showUsersPosts(User u) {
            postList.Posts.ItemsSource = RealData.getPostsFromUser(u);
        }

        public void showPostsReplies(Post p) {
            new ReplyView(RealData.getRepliesFromPost(p)).ShowDialog();
        }

        public void editUser(User u) {
            EditView e = new EditView(u, RealData.getPostsFromUser(u).ToList());
            e.ShowDialog();
            if (e.save) {
                if (e.userEditor.validate())
                {
                    u.Username = e.userEditor.username.Text;
                    u.Image = e.userEditor.image.Text;
                    u.Handle = e.userEditor.handle.Text;
                    if (RealData.editUser(u)) {
                        MessageBox.Show("saved");
                    }
                }
            }
            
        }

        public void deleteUser(User u) {
            FakeData.deleteUser(u);
            MessageBox.Show("It's gone!");
        }

        public void editPost(Post p) {
            //TODO: UI
            EditView e = new EditView(p, RealData.getRepliesFromPost(p).ToList());
            e.ShowDialog();
            if (e.save)
            {
                if (e.postEditor.validate())
                {
                    p.Text = e.postEditor.Text.Text;
                    if (RealData.editPost(p))
                    {
                        MessageBox.Show("saved");
                    }
                }
            }
            FakeData.editPost(p);
        }

        public void deletePost(Post p) {
            RealData.deletePost(p);
            MessageBox.Show("It's gone!");
        }

        public void editReply(Reply r) {
            EditView e = new EditView(r);
            e.ShowDialog();
            if (e.save)
            {
                if (e.replyEditor.validate())
                {
                    r.Text = e.replyEditor.Text.Text;
                    if (RealData.editReply(r))
                    {
                        MessageBox.Show("saved");
                    }
                }
            }
            FakeData.editReply(r);
        }

        public void deleteReply(Reply r) {
            RealData.deleteReply(r);
            MessageBox.Show("It's gone!");
        }

        private void NewUser_OnClick(object sender, RoutedEventArgs e)
        {
            new NewData(NewData.DataType.user).ShowDialog();
        }

        private void NewPost_OnClick(object sender, RoutedEventArgs e)
        {
            new NewData(NewData.DataType.post).ShowDialog();
        }

        private void NewReply_OnClick(object sender, RoutedEventArgs e)
        {
            new NewData(NewData.DataType.reply).ShowDialog();
        }

        private void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            usersList.Users.ItemsSource = RealData.getUsers();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void About_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Made by Domen Jesenovec for ORA", "About", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        private void GetReports_OnClick(object sender, RoutedEventArgs e)
        {
            new ReportsView().ShowDialog();
        }

        private async void Test_OnClick(object sender, RoutedEventArgs e)
        {
            var responseString = await "http://localhost:63771/api/user/IsAdmin"
                .PostJsonAsync(new { username = "admin", password = "pass" }).ReceiveString();
            if (responseString == "\"true\"")
            {
                MessageBox.Show("\"true\"");
            }
            else MessageBox.Show("false");
        }
    }
}