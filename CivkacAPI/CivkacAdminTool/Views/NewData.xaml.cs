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
using System.Windows.Shapes;
using CivkacAdminTool.Models;
using CivkacAdminTool.REST;
using CivkacAdminTool.Tests;

namespace CivkacAdminTool.Views
{
    /// <summary>
    /// Interaction logic for NewData.xaml
    /// </summary>
    public partial class NewData : Window
    {
        private DataType dataType;

        public NewData(DataType dataType)
        {
            InitializeComponent();
            this.dataType = dataType;
            if (dataType == DataType.user)
            {
                UserForm.Visibility = Visibility.Visible;
            }
            if (dataType == DataType.post)
            {
                PostForm.Author.ItemsSource = RealData.getUsers();
                PostForm.Visibility = Visibility.Visible;
                
            }
            if (dataType == DataType.reply)
            {
                RepllyForm.Author.ItemsSource = RealData.getUsers();
                RepllyForm.PostID.ItemsSource = RealData.getPosts();
                RepllyForm.Visibility = Visibility.Visible;
            }
        }

        public enum DataType
        {
            user,
            post,
            reply
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
            //TODO: VALIDATION
            if (dataType == DataType.user)
            {
                if (UserForm.validate())
                {
                    if (RealData.addUser(new User(UserForm.username.Text, UserForm.handle.Text, UserForm.image.Text), UserForm.password.Password))
                    {
                        MessageBox.Show("added");
                        this.Close();
                    }
                }
                
                //validate
            }
            if (dataType == DataType.post)
            {

                if (PostForm.validate())
                {
                    if (PostForm.Author.SelectedItem is User u)
                    {
                        if (RealData.addPost(new Post(PostForm.Text.Text, u), MainWindow.password))
                        {
                            MessageBox.Show("added");
                            this.Close();
                        }
                    }
                }
            }
            if (dataType == DataType.reply)
            {
                if (RepllyForm.validate())
                {
                    if (RepllyForm.Author.SelectedItem is User u)
                    {
                        if (RepllyForm.PostID.SelectedItem is Post p)
                        {
                            if (RealData.addReply(new Reply(RepllyForm.Text.Text, u, p), MainWindow.password))
                            {
                                MessageBox.Show("added");
                                this.Close();
                            }
                        }
                    }
                }
               
                
            }
        }
    }
}