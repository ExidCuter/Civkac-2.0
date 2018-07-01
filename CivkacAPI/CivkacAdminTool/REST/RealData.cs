using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using CivkacAdminTool.Models;
using Flurl.Http;
using Newtonsoft.Json;

namespace CivkacAdminTool.REST
{
    class RealData
    {
        public static IEnumerable<User> getUsers()
        {
            Task<IEnumerable<User>> task = Task.Run(async () =>
            {
                var responseString = await "http://localhost:63771/api/user".GetStringAsync();
                IEnumerable<User> u = JsonConvert.DeserializeObject<List<User>>(responseString);
                return u;
            });
            return task.Result;
        }

        public static IEnumerable<Post> getPosts()
        {
            Task<List<Post>> task = Task.Run(async () =>
            {
                List<Post> posts = new List<Post>();
                try
                {
                    string url = "http://localhost:63771/api/post";
                    var responseString = await url.GetStringAsync();
                    dynamic dynPosts = JsonConvert.DeserializeObject<dynamic>(responseString);
                    foreach (dynamic o in dynPosts)
                    {
                        dynamic a = o["author"];
                        User user = User.fromDynamic(a);
                        posts.Add(new Post(Convert.ToInt32(o["id"]), o["text"].ToString(), user));
                    }
                    return posts;
                }
                catch (Exception e)
                {
                    return posts;
                }
            });
            return task.Result;
        }

        public static IEnumerable<Post> getPostsFromUser(User u)
        {
            Task<List<Post>> task = Task.Run(async () =>
            {
                List<Post> posts = new List<Post>();
                try
                {
                    string url = "http://localhost:63771/api/user/posts/" + u.Id;
                    var responseString = await url.GetStringAsync();
                    dynamic dynPosts = JsonConvert.DeserializeObject<dynamic>(responseString);
                    foreach (dynamic o in dynPosts)
                    {
                        dynamic a = o["author"];
                        User user = User.fromDynamic(a);
                        posts.Add(new Post(Convert.ToInt32(o["id"]), o["text"].ToString(), user));
                    }
                    return posts;
                }
                catch (Exception e)
                {
                    return posts;
                }
            });
            return task.Result;
        }

        public static IEnumerable<Reply> getRepliesFromPost(Post p)
        {
            Task<List<Reply>> task = Task.Run(async () =>
            {
                List<Reply> replies = new List<Reply>();
                try
                {
                    string url = "http://localhost:63771/api/post/" + p.Id;
                    var responseString = await url.GetStringAsync();
                    dynamic o = JsonConvert.DeserializeObject<dynamic>(responseString);

                    dynamic a = o["author"];
                    User user = User.fromDynamic(a);
                    Post post = new Post(Convert.ToInt32(o["id"]), o["text"].ToString(), user);
                    foreach (dynamic o1 in o["replies"])
                    {
                        User author = User.fromDynamic(o1["author"]);
                        replies.Add(new Reply(Convert.ToInt32(o1["id"]), o1["text"].ToString(), author, post));
                    }


                    return replies;
                }
                catch (Exception e)
                {
                    return replies;
                }
            });
            return task.Result;
        }

        
        public static IEnumerable<Report> getRepots()
        {
            Task<List<Report>> task = Task.Run(async () =>
            {
                List<Report> reports = new List<Report>();
                try
                {
                    var responseString = await "http://localhost:63771/api/report".GetStringAsync();

                    dynamic o = JsonConvert.DeserializeObject<dynamic>(responseString);
                    foreach (dynamic o1 in o)
                    {
                        User reporter = User.fromDynamic(o1["author"]);
                        User reporte = User.fromDynamic(o1["reportedUser"]);
                        reports.Add(new Report(Convert.ToInt32(o1["id"]), o1["reason"].ToString(), reporter, reporte));
                    }
                    return reports;
                }
                catch (Exception e)
                {
                    return reports;
                }
            });
            return task.Result;
        }

        public static bool isAdmin(String username, String pass)
        {
            Task<bool> task = Task.Run(async () =>
            {
                try
                {
                    var responseString = await "http://localhost:63771/api/user/IsAdmin"
                        .PostJsonAsync(new { username = username, password = pass }).ReceiveString();
                    if (responseString.Contains("true"))
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception e)
                {
                    return false;
                }
            });
            return task.Result;
        }

        public static bool addUser(User u, String pass)
        {
            Task<bool> task = Task.Run(async () =>
            {
                var responseString = await "http://localhost:63771/api/user"
                    .PostJsonAsync(new {username = u.Username, handle = u.Handle, email = u.Email, password = pass}).ReceiveString();
                if (responseString != "Napacni podatki")
                {
                    return true;
                }
                return false;
            });
            return task.Result;
        }
        
        public static bool editUser(User u)
        {
            try
            {
                Task<bool> task = Task.Run(async () =>
                {
                    String url = "http://localhost:63771/api/user/" + u.Id;
                    var responseString = await url.PutJsonAsync(new {id = u.Id, password = MainWindow.password, handle = u.Handle, image = u.Image}).ReceiveString();


                    if (responseString != "\"User not found!\"")
                    {
                        return true;
                    }
                    return false;
                });
                return task.Result;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        /*
        public static bool deleteUser(User u)
        {
            try
            {
                Task<bool> task = Task.Run(async () =>
                {
                    var responseString = await "http://localhost:63771/api/user"
                      
                    if (responseString != "Napacni podatki")
                    {
                        return true;
                    }
                    return false;
                });
                return task.Result;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        */
        public static bool addPost(Post p, string password)
        {
            try
            {
                Task<bool> task = Task.Run(async () =>
                {
                    var responseString = await "http://localhost:63771/api/post"
                        .PostJsonAsync(new {id = p.Author.Id , password = password, text = p.Text }).ReceiveString();
                    if (responseString != "Napacni podatki")
                    {
                        return true;
                    }
                    return false;
                });
                return task.Result;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        public static bool editPost(Post p)
        {
            try
            {
                Task<bool> task = Task.Run(async () =>
                {
                    String url = "http://localhost:63771/api/post/" + p.Id;
                    var responseString = await url.PutJsonAsync(new { id = p.Author.Id, password = MainWindow.password, text = p.Text }).ReceiveString();


                    if (responseString != "\"User not found!\"")
                    {
                        return true;
                    }
                    return false;
                });
                return task.Result;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        
        public static bool deletePost(Post p)
        {
            try
            {
                Task<bool> task = Task.Run(async () =>
                {
                    String url = "http://localhost:63771/api/post/" + p.Id;
                    var responseString = await url.DeleteAsync().ReceiveString();
                    if (responseString.Contains("deleted"))
                    {
                        return true;
                    }
                    return false;
                });
                return task.Result;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        public static bool addReply(Reply r, String password)
        {
            try
            {
                Task<bool> task = Task.Run(async () =>
                {
                    var responseString = await "http://localhost:63771/api/reply"
                        .PostJsonAsync(new { postId = r.Post.Id, id = r.User.Id, password = password, text = r.Text }).ReceiveString();
                    if (responseString != "Napacni podatki")
                    {
                        return true;
                    }
                    return false;
                });
                return task.Result;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        public static bool editReply(Reply r)
        {
            try
            {
                Task<bool> task = Task.Run(async () =>
                {
                    String url = "http://localhost:63771/api/reply/" + r.Id;
                    var responseString = await url.PutJsonAsync(new { id = r.User.Id, password = MainWindow.password, text = r.Text }).ReceiveString();


                    if (responseString != "\"User not found!\"")
                    {
                        return true;
                    }
                    return false;
                });
                return task.Result;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        public static bool deleteReply(Reply r)
        {
            try
            {
                Task<bool> task = Task.Run(async () =>
                {
                    String url = "http://localhost:63771/api/reply/" + r.Id;
                    var responseString = await url.DeleteAsync().ReceiveString();
                    if (responseString.Contains("deleted"))
                    {
                        return true;
                    }
                    return false;
                });
                return task.Result;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}