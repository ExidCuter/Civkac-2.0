using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Civkac.Models {
    public class User : IEditable, IDynamicable {
        private String password;

        public int Id { get; }

        public string Username { get; set; }

        public string Handle { get; set; }

        public string Email { get; set; }

        public string Image { get; set; }

        public List<User> Follows { get; set; }

        public User(string username, string handle, string email) {
            this.Username = username;
            this.Handle = handle;
            this.Email = email;
            this.Image = "img/defaultProfile.png";
            this.Id = -1;
            this.Follows = new List<User>();
        }

        public User(int id, string username, string handle, string image) {
            this.Username = username;
            this.Handle = handle;
            this.Email = null;
            this.Image = image;
            this.Id = id;
            this.Follows = null;
        }

        public User(string username, string handle, string email, string password) {
            this.Username = username;
            this.Handle = handle;
            this.Email = email;
            this.Image = "img/defaultProfile.png";
            this.Id = -1;
            this.Follows = null;
            this.password = password;
        }

        public User(int id, string username, string handle, string email, string image, string password) {
            this.Id = id;
            this.Username = username;
            this.Handle = handle;
            this.Email = email;
            this.Image = image;
            this.Follows = new List<User>();
            this.password = password;
        }

        public bool checkPassword(string password) {
            return this.password == password;
        }

        public bool validate() {
            return Username != null && Handle != null && Email != null && Image != null && password != null;
        }

        public void edit(string newText) {
            throw new NotImplementedException();
        }

        public void delete() {
            throw new NotImplementedException();
        }

        public dynamic getDynamic() {
            dynamic item = new ExpandoObject();
            if (Id > 0) {
                item.id = Id;
            }
            item.username = Username;
            item.handle = Handle;
            item.image = Image;
            if (!string.IsNullOrEmpty(Email)) {
                item.email = Email;
            }

            if (Follows != null && Follows.Count > 0) {
                item.follows = Follows;
            }

            return item;
        }

    }
}