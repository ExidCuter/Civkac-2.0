using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Civkac.Models
{
    public class Report : IDynamicable
    {
        private int id;
        private String reason;
        private User author;
        private User reportedUser;

        public int Id => id;

        public string Reason => reason;

        public User Author
        {
            get => author;
            set => author = value;
        }

        public User ReportedUser => reportedUser;

        public Report(string reason, User author, User reportedUser)
        {
            this.id = -1;
            this.reason = reason;
            this.author = author;
            this.reportedUser = reportedUser;
        }

        public Report(int id, string reason, User reportedUser)
        {
            this.id = id;
            this.reason = reason;
            this.reportedUser = reportedUser;
        }

        public dynamic getDynamic()
        {
            dynamic item = new ExpandoObject();
            if (Id > 0)
            {
                item.id = Id;
            }
            item.reason = Reason;
            item.author = Author;
            item.reportedUser = ReportedUser;
            return item;
        }
    }
}
