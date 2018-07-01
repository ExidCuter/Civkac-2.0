using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Civkac.Models
{
    public class Reply : IEditable, IDynamicable
    {
        private int id;
        private String text;
        private User user;
        private int ratings;

        public int Id => id;

        public string Text => text;

        public User User => user;

        public int Ratings {
            get => ratings;
            set => ratings = value;
        }

        public Reply(string text, User user)
        {
            this.id = -1;
            this.text = text;
            this.user = user;
            this.ratings = 0;
        }

        public Reply(int id, string text, User user)
        {
            this.id = id;
            this.text = text;
            this.user = user;
        }

        public void edit(string newText)
        {
            throw new NotImplementedException();
        }

        public void delete()
        {
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
            item.author = user.getDynamic();
            item.ratings = Ratings;
            return item;
        }
    }
}
