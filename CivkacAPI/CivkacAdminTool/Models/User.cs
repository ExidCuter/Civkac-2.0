using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CivkacAdminTool.Models
{
    public class User : IDynamicable
    {
        public int Id { get; set;}

        public string Username { get; set; }

        public string Handle { get; set; }

        public string Email { get; set; }

        public string Image { get; set; }

        public List<User> Follows { get; set; }

        private User() { }
        public User(string username, string handle, string email)
        {
            this.Username = username;
            this.Handle = handle;
            this.Email = email;
            this.Image = "img/defaultProfile.png";
            this.Id = -1;
            this.Follows = new List<User>();
        }

        public User(int id, string username, string handle, string image)
        {
            this.Username = username;
            this.Handle = handle;
            this.Email = null;
            this.Image = image;
            this.Id = id;
            this.Follows = null;
        }

        public User(int id, string username, string handle, string email, string image)
        {
            this.Id = id;
            this.Username = username;
            this.Handle = handle;
            this.Email = email;
            this.Image = image;
            this.Follows = new List<User>();
        }


        public bool validate()
        {
            return Username != null && Handle != null && Email != null && Image != null;
        }

      


        public dynamic getDynamic()
        {
            dynamic item = new ExpandoObject();
            if (Id > 0)
            {
                item.id = Id;
            }
            item.username = Username;
            item.handle = Handle;
            item.image = Image;
            if (!string.IsNullOrEmpty(Email))
            {
                item.email = Email;
            }

            if (Follows != null && Follows.Count > 0)
            {
                item.follows = Follows;
            }

            return item;
        }

        public override bool Equals(object obj) {
            if (obj is User u) {
                return u.Email == this.Email && u.Username == this.Username;
            }
            return false;
        }

        public static User fromDynamic(dynamic u)
        {
            try
            {
                return new User(Convert.ToInt32(u["id"]), u["username"].ToString(), u["handle"].ToString(), u["image"].ToString());
            }
            catch (Exception e)
            {
                return null;
                throw;
            }
        }

        public override string ToString()
        {
            return Handle;
        }
    }
}
