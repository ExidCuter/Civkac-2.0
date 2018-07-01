using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Civkac.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MySql.Data.MySqlClient;

namespace Civkac {
    public class Database {
        private MySqlConnection conn;
        private static Database instance;

        private Database() {
            conn = new MySqlConnection("server=localhost;port=3306;database=civkac;user=civkac;password=civkac;");
        }

        public static Database getInstance() {
            if (instance == null) {
                instance = new Database();
            }

            return instance;
        }

        public List<User> getAllUsers() {
            List<User> users = new List<User>();
            try {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from user", conn);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        users.Add(new User(Convert.ToInt32(reader["id"]), reader["username"].ToString(),
                            reader["handle"].ToString(), reader["email"].ToString(), reader["image"].ToString(),
                            reader["password"].ToString()));
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
            finally {
                conn.Close();
            }

            return users;
        }

        public User getUser(int id) {
            User user = null;
            try {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from user where id = " + id, conn);
                using (var reader = cmd.ExecuteReader()) {
                    reader.Read();
                    user = new User(Convert.ToInt32(reader["id"]), reader["username"].ToString(),
                        reader["handle"].ToString(), reader["email"].ToString(), reader["image"].ToString(),
                        reader["password"].ToString());
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
            finally {
                conn.Close();
            }

            return user;
        }

        public User getUserByUsername(String username) {
            User user = null;
            try {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from user where username = '" + username + "'", conn);
                using (var reader = cmd.ExecuteReader()) {
                    reader.Read();
                    user = new User(Convert.ToInt32(reader["id"]), reader["username"].ToString(),
                        reader["handle"].ToString(), reader["email"].ToString(), reader["image"].ToString(),
                        reader["password"].ToString());
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
            finally {
                conn.Close();
            }

            return user;
        }

        public List<Post> getAllPosts() {
            List<Post> posts = new List<Post>();
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "SELECT u.id as user_id, u.username, u.handle, u.image, p.id as post_id, p.text, p.time FROM user u INNER JOIN post p ON u.id = p.user_id",
                        conn);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        posts.Add(new Post(Convert.ToInt32(reader["post_id"]), reader["text"].ToString(), new User(
                            Convert.ToInt32(reader["user_id"]), reader["username"].ToString(),
                            reader["handle"].ToString(), reader["image"].ToString())));
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            finally {
                conn.Close();
            }

            foreach (Post post in posts) {
                post.Ratings = getRatingOnPost(post.Id);
                post.Replies = getAllReplies(post.Id);
            }

            return posts;
        }

        public Post getPost(int id) {
            Post post = null;
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "SELECT u.id as user_id, u.username, u.handle, u.image, p.id as post_id, p.text, p.time FROM user u INNER JOIN post p ON u.id = p.user_id where p.id = " +
                        id,
                        conn);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        post = new Post(Convert.ToInt32(reader["post_id"]), reader["text"].ToString(), new User(
                            Convert.ToInt32(reader["user_id"]), reader["username"].ToString(),
                            reader["handle"].ToString(), reader["image"].ToString()));
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            finally {
                conn.Close();
            }

            if (post != null) {
                post.Ratings = getRatingOnPost(post.Id);
                post.Replies = getAllReplies(post.Id);
            }

            return post;
        }

        public Reply getReply(int id) {
            Reply reply = null;
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "SELECT u.id as user_id, u.username, u.handle, u.image, p.id as post_id, p.text, p.time FROM user u INNER JOIN reply p ON u.id = p.user_id where p.id = " +
                        id,
                        conn);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        reply = new Reply(Convert.ToInt32(reader["post_id"]), reader["text"].ToString(), new User(
                            Convert.ToInt32(reader["user_id"]), reader["username"].ToString(),
                            reader["handle"].ToString(), reader["image"].ToString()));
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            finally {
                conn.Close();
            }

            if (reply != null) {
                reply.Ratings = getRatingOnReply(reply.Id);
            }

            return reply;
        }


        public List<Post> getFeed(int userId) {
            User user = getUser(userId);
            List<Post> posts = new List<Post>();
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "SELECT u.id as user_id, u.username, u.handle, u.image, p.id as post_id, p.text, p.time FROM user u INNER JOIN post p ON u.id = p.user_id WHERE p.user_id = " +
                        userId + " OR p.user_id IN (SELECT folows FROM follows WHERE user = " + userId +
                        ") OR p.text LIKE '%@" + user.Handle + "%' ORDER BY p.time DESC",
                        conn);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        posts.Add(new Post(Convert.ToInt32(reader["post_id"]), reader["text"].ToString(), new User(
                            Convert.ToInt32(reader["user_id"]), reader["username"].ToString(),
                            reader["handle"].ToString(), reader["image"].ToString())));
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            finally {
                conn.Close();
            }

            foreach (Post post in posts) {
                post.Ratings = getRatingOnPost(post.Id);
                post.Replies = getAllReplies(post.Id);
            }

            return posts;
        }

        public List<Post> getFeed(string username) {
            User user = getUserByUsername(username);
            List<Post> posts = new List<Post>();
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "SELECT u.id as user_id, u.username, u.handle, u.image, p.id as post_id, p.text, p.time FROM user u INNER JOIN post p ON u.id = p.user_id WHERE p.user_id = " +
                        user.Id + " OR p.user_id IN (SELECT folows FROM follows WHERE user = " + user.Id +
                        ") OR p.text LIKE '%@" + user.Handle + "%' ORDER BY p.time DESC",
                        conn);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        posts.Add(new Post(Convert.ToInt32(reader["post_id"]), reader["text"].ToString(), new User(
                            Convert.ToInt32(reader["user_id"]), reader["username"].ToString(),
                            reader["handle"].ToString(), reader["image"].ToString())));
                    }
                }
            }
            catch (Exception e) { }
            finally {
                conn.Close();
            }

            foreach (Post post in posts) {
                post.Ratings = getRatingOnPost(post.Id);
                post.Replies = getAllReplies(post.Id);
            }

            return posts;
        }


        public List<Reply> getAllReplies(int postID) {
            List<Reply> replies = new List<Reply>();
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "SELECT r.id as reply_id, r.text,u.id as user_id, u.username, u.handle, u.image FROM reply r INNER JOIN user u ON r.user_id = u.id where r.post_id = " +
                        postID, conn);
                using (var reader2 = cmd.ExecuteReader()) {
                    while (reader2.Read()) {
                        replies.Add(new Reply(Convert.ToInt32(reader2["reply_id"]), reader2["text"].ToString(),
                            new User(Convert.ToInt32(reader2["user_id"]), reader2["username"].ToString(),
                                reader2["handle"].ToString(), reader2["image"].ToString())));
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            finally {
                conn.Close();
            }

            foreach (Reply reply in replies) {
                reply.Ratings = getRatingOnReply(reply.Id);
            }

            return replies;
        }

        private int getRatingOnPost(int id) {
            int count = 0;
            try {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT count(id) as score from postRating where post_id = " + id,
                    conn);
                using (var reader = cmd.ExecuteReader()) {
                    reader.Read();
                    count = Convert.ToInt32(reader["score"]);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            finally {
                conn.Close();
            }

            return count;
        }

        private int getRatingOnReply(int id) {
            int count = 0;
            try {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT count(id) as score from replyRating where reply_id = " + id,
                    conn);
                using (var reader = cmd.ExecuteReader()) {
                    reader.Read();
                    count = Convert.ToInt32(reader["score"]);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            finally {
                conn.Close();
            }

            return count;
        }

        public Report getReport(int id) {
            Report report = null;
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "SELECT r.id as reportID, r.reason, r.reporter, u.* from user u JOIN report r ON r.reporte = u.id where r.id = " +
                        id, conn);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        report = new Report(Convert.ToInt32(reader["reportID"]), reader["reason"].ToString(),
                            new User(Convert.ToInt32(reader["id"]), reader["username"].ToString(), "", ""));
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            finally {
                conn.Close();
            }


            if (report != null) {
                report.Author = getUser(report.Id);
            }

            return report;
        }

        public List<Report> getAllReports() {
            List<Report> reports = new List<Report>();
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "SELECT r.id as reportID, r.reason, r.reporter, u.* from user u JOIN report r ON r.reporte = u.id",
                        conn);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        reports.Add(new Report(Convert.ToInt32(reader["reportID"]), reader["reason"].ToString(),
                            new User(Convert.ToInt32(reader["id"]), reader["username"].ToString(), "", "")));
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            finally {
                conn.Close();
            }

            foreach (Report report in reports) {
                report.Author = getUser(report.Id);
            }

            return reports;
        }

        public List<Post> getPostsFromUser(int id) {
            User u = getUser(id);
            List<Post> posts = new List<Post>();
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "SELECT p.id, p.text, p.time, u.id as user_id, u.username, u.email, u.image, u.handle FROM post p JOIN user u ON u.id = p.user_id WHERE p.user_id = " +
                        u.Id + " ORDER BY p.time DESC", conn); // + " OR p.id IN (SELECT r.post_id FROM reply r WHERE r.user_id = " + u.Id + ")"
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        posts.Add(new Post(Convert.ToInt32(reader["id"]), reader["text"].ToString(), new User(
                            Convert.ToInt32(reader["user_id"]), reader["username"].ToString(),
                            reader["handle"].ToString(), reader["image"].ToString())));
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            finally {
                conn.Close();
            }

            foreach (Post post in posts) {
                post.Replies = getAllReplies(post.Id);
                post.Ratings = getRatingOnReply(post.Id);
            }

            return posts;
        }


        public bool InsertIntoPost(Post post) {
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "INSERT INTO post values(null, '" + post.Text + "', NOW(), " + post.Author.Id + ")", conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            finally {
                conn.Close();
            }

            return true;
        }

        public bool InsertIntoReply(Reply reply, int postID) {
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "INSERT INTO reply values(null, '" + reply.Text + "', NOW(), " + reply.User.Id + ", " + postID +
                        ")", conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            finally {
                conn.Close();
            }

            return true;
        }

        public bool InsertIntoReport(Report report) {
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "INSERT INTO report values(null, '" + report.Reason + "', " + report.Author.Id + ", " +
                        report.ReportedUser.Id + ")", conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            finally {
                conn.Close();
            }

            return true;
        }

        public bool InsertIntoUser(User user, string password) {
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "INSERT INTO user values(null, '" + user.Username + "', '" + user.Email +
                        "', 'img/defaultUser.png', '" + user.Handle + "', '" + password + "')", conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            finally {
                conn.Close();
            }

            return true;
        }

        public bool updatePostByID(Post org, Post toReplace) {
            int i = 0;
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "UPDATE post SET text = '" + toReplace.Text + "', time = NOW() where id = " + org.Id, conn);
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            finally {
                conn.Close();
            }

            return true;
        }

        public bool updateReplyByID(Reply org, Reply toReplace) {
            int i = 0;
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "UPDATE reply SET text = '" + toReplace.Text + "', time = NOW() where id = " + org.Id, conn);
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            finally {
                conn.Close();
            }

            return true;
        }

        public bool updateReportByID(Report org, String reason) {
            int i = 0;
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "UPDATE report SET reason = '" + reason + "' where id = " + org.Id, conn);
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            finally {
                conn.Close();
            }

            return true;
        }

        public bool updateUserByID(User org, User toReplace) {
            int i = 0;
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "UPDATE user SET username = '" + toReplace.Username + "', handle = '" + toReplace.Handle +
                        "', image = '" + toReplace.Image +
                        "' where id = " + org.Id, conn);
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            finally {
                conn.Close();
            }

            return true;
        }

        public bool deleteReplyById(int id) {
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "DELETE FROM reply WHERE id = " + id, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            finally {
                conn.Close();
            }

            return true;
        }

        public bool deletePostById(int id) {
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "DELETE FROM reply WHERE post_id = " + id, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            finally {
                conn.Close();
            }

            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "DELETE FROM post WHERE id = " + id, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            finally {
                conn.Close();
            }

            return true;
        }

        public bool UserFollowsUser(User user1, User user2) {
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "INSERT INTO follows values(null, " + user2.Id + ", " + user1.Id + ")", conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            finally {
                conn.Close();
            }

            return true;
        }

        public bool UserUnFollowsUser(User user1, User user2) {
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "DELETE FROM follows where user = " + user1.Id + " AND folows = " + user2.Id, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            finally {
                conn.Close();
            }

            return true;
        }

        public bool isUserFollowingUser(User user1, User user2) {
            int i = 0;
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "SELECT * FROM follows where user = " + user1.Id + " AND folows = " + user2.Id, conn);

                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        i++;
                    }
                }
            }
            catch (Exception e) {
                return false;
            }
            finally {
                conn.Close();
            }

            if (i > 0)
                return true;
            return false;
        }

        public List<User> getAllUsersLike(String like) {
            List<User> users = new List<User>();
            try {
                conn.Open();
                MySqlCommand cmd =
                    new MySqlCommand(
                        "select * from user where username LIKE '%" + like + "%' OR handle LIKE '%" + like + "%'", conn);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        users.Add(new User(Convert.ToInt32(reader["id"]), reader["username"].ToString(),
                            reader["handle"].ToString(), reader["email"].ToString(), reader["image"].ToString(),
                            reader["password"].ToString()));
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
            finally {
                conn.Close();
            }

            return users;
        }
    }
}