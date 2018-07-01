using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CivkacAdminTool.Models
{
    public class Reply :  IDynamicable
    {
        private int id;
        private Post post;
        private String text;
        private User user;
        private int ratings;


        public int Id
        {
            get => id;
            set => id = value;
        }

        public string Text
        {
            get => text;
            set => text = value;
        }

        public User User
        {
            get => user;
            set => user = value;
        }

        public int Ratings {
            get => ratings;
            set => ratings = value;
        }

        public Post Post
        {
            get => post;
            set => post = value;
        }

        public Reply(string text, User user, Post post)
        {
            this.id = -1;
            this.text = text;
            this.user = user;
            this.post = post;
            this.ratings = 0;
        }

        public Reply(int id, string text, User user, Post post)
        {
            this.id = id;
            this.text = text;
            this.user = user;
            this.post = post;
        }

        public dynamic getDynamic()
        {
            dynamic item = new ExpandoObject();
            if (Id > 0)
            {
                item.id = Id;
            }
            item.text = Text;
            item.author = user.getDynamic();
            item.post = post;
            item.ratings = Ratings;
            return item;
        }
    }
}
