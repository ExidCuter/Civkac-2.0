using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Civkac.Models
{
    public class Rating
    {
        private int id;
        private User author;
        private bool positive;

        public int Id => id;

        public User Author => author;

        public bool Positive => positive;

        public Rating(User author, bool positive)
        {
            this.id = -1;
            this.author = author;
            this.positive = positive;
        }

    }

}
