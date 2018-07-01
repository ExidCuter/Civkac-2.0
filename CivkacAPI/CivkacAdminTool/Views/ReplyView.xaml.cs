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

namespace CivkacAdminTool.Views
{
    /// <summary>
    /// Interaction logic for ReplyView.xaml
    /// </summary>
    public partial class ReplyView : Window
    {
        public ReplyView(IEnumerable<Reply> replies)
        {
            InitializeComponent();
            Replies.RepliesList.ItemsSource = replies;
        }
    }
}
