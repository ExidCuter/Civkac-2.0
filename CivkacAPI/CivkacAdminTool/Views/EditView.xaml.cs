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

namespace CivkacAdminTool.Views {
    /// <summary>
    /// Interaction logic for EditView.xaml
    /// </summary>
    public partial class EditView : Window {
        public bool save { get; set; } = false;

        public Editing editing;

        public enum Editing {
            user, post, reply
        }

        public EditView(User u, List<Post> posts) {
            InitializeComponent();
            editing = Editing.user;
            userEditor.Visibility = Visibility.Visible;
            userEditor.id.Text = u.Id.ToString();
            userEditor.username.Text = u.Username;
            userEditor.handle.Text = u.Handle;
            userEditor.image.Text = u.Image;
            userEditor.posts.Posts.ItemsSource = posts;
        }

        public EditView(Post p, List<Reply> replies)
        {
            InitializeComponent();
            editing = Editing.post;
            postEditor.Visibility = Visibility.Visible;
            postEditor.id.Text = p.Id.ToString();
            postEditor.author.Text = p.Author.Handle;
            postEditor.Text.Text = p.Text;
            postEditor.replies.RepliesList.ItemsSource = replies;
        }

        public EditView(Reply r)
        {
            InitializeComponent();
            editing = Editing.reply;
            replyEditor.Visibility = Visibility.Visible;
            replyEditor.id.Text = r.Id.ToString();
            replyEditor.author.Text = r.User.Handle;
            replyEditor.Text.Text = r.Text;
        }
    }
}