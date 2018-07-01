using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivkacAdminTool.Models;

namespace CivkacAdminTool.Tests {
    class FakeData {
        private static Random random = new Random();
        public static List<User> users { get; set; } = new List<User>();
        public static List<Post> posts { get; set; } = new List<Post>();
        public static List<Reply> replies { get; set; } = new List<Reply>();

        public FakeData() {
            generateUsers();
            generatePosts();
            generateReplies();
        }


        public static string randomString(bool toUpper = false) {
            int length = random.Next(3, 10);

            const string chars = "abcdefghijklmnopqrstuvwxyz";
            string str = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            if (toUpper) {
                return firstCharToUpper(str);
            }

            return str;
        }

        public static string firstCharToUpper(string input) {
            switch (input) {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        private static void generateUsers() {
            for (int i = 0; i < 10; i++) {
                users.Add(new User(i, randomString(true), randomString(), randomString() + "@mail.com",
                    "img/default.jpg"));
            }
        }

        private static void generatePosts() {
            for (int i = 0; i < 100; i++) {
                posts.Add(new Post(i, randomString(true), users[random.Next(0, users.Count)]));
            }
        }

        private static void generateReplies() {
            for (int i = 0; i < 1000; i++) {
                replies.Add(new Reply(i, randomString(true), users[random.Next(0, users.Count)],
                    posts[random.Next(0, posts.Count)]));
            }
        }

        public static IEnumerable<Post> getPostsFromUser(User u) {
            foreach (Post post in posts) {
                if (post.Author.Equals(u)) {
                    yield return post;
                }
            }
        }

        public static IEnumerable<Reply> getRepliesFromPost(Post p) {
            foreach (Reply reply in replies) {
                if (reply.Post.Id == p.Id) {
                    yield return reply;
                }
            }
        }

        public static IEnumerable<Report> getRepots()
        {
            List<Report> r = new List<Report>();
            for (int i = 0; i < 10; i++)
            {
                r.Add(new Report(randomString(true), users[random.Next(0, users.Count)],
                    users[random.Next(0, users.Count)]));

            }
            return r;
        }

        public static bool addUser(User u, String password)
        {
            users.Add(new User(users.Count, u.Username,u.Handle, u.Email, u.Image));
            return true;
        }

        public static bool editUser(User u) {
            return true;
        }

        public static bool deleteUser(User u) {
            users.Remove(u);
            return true;
        }

        public static bool addPost(Post p)
        {   
            posts.Add(new Post(posts.Count, p.Text, p.Author));
            return true;
        }

        public static bool editPost(Post p) {
            return true;
        }

        public static bool deletePost(Post p) {
            posts.Remove(p);
            return true;
        }

        public static bool addReply(Reply r)
        {
            replies.Add(new Reply(replies.Count, r.Text, r.User, r.Post));
            return true;
        }

        public static bool editReply(Reply r) {
            return true;
        }

        public static bool deleteReply(Reply r) {
            replies.Remove(r);
            return true;
        }
    }
}