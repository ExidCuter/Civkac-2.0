using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Civkac.Models {
    public class Post : IEditable, IDynamicable {
        private int id = 0;
        private String text;
        private User author;
        private List<Reply> replies;
        private List<Hastag> hastags;
        private int ratings;
        private List<User> taggedUsers;

        public int Id => id;

        public string Text => text;

        public User Author => author;

        public int Ratings {
            get => ratings;
            set => ratings = value;
        }

        public List<Reply> Replies {
            get => replies;
            set => replies = value;
        }

        public List<Hastag> Hastags {
            get => hastags;
            set => hastags = value;
        }

        public List<User> TaggedUsers {
            get => taggedUsers;
            set => taggedUsers = value;
        }


        public Post(string text, User author) {
            this.id = -1;
            this.text = text;
            this.author = author;
            this.replies = new List<Reply>();
            this.hastags = new List<Hastag>();
            this.ratings = 0;
            this.taggedUsers = new List<User>();
        }

        public Post(int id, string text, User author) {
            this.id = id;
            this.text = text;
            this.author = author;
        }

        public void edit(string newText) {
            throw new NotImplementedException();
        }

        public void delete() {
            throw new NotImplementedException();
        }

        public dynamic getDynamic()
        {
            dynamic item = new ExpandoObject();
            if (Id > 0)
            {
                item.id = Id;
            }
            item.text = Text;
            item.author = Author.getDynamic();
            item.ratings = Ratings;

            if (TaggedUsers != null  && TaggedUsers.Count > 0)
            {
                item.taggedUsers = TaggedUsers;
            }

            item.replies = new List<Reply>();

            if (Replies != null && Replies.Count > 0)
            {
                item.replies = Outputter.getDynamicList(Replies);
            }

            if (Hastags != null && Hastags.Count > 0)
            {
                item.hastags = Hastags;
            }

            return item;
        }
    }
}