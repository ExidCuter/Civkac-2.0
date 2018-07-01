using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Civkac.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Civkac.Controllers {
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller {
        // GET: api/User
        [HttpGet]
        public IEnumerable<dynamic> GetUsers() {
            return Outputter.getDynamicList(Database.getInstance().getAllUsers());
        }

        [Route("like/{part}")]
        [HttpGet("{part}", Name = "like")]
        public IEnumerable<dynamic> GetUsersLike(String part)
        {
            return Outputter.getDynamicList(Database.getInstance().getAllUsersLike(part));
        }


        [Route("posts/{id}")]
        [HttpGet("{id}", Name = "GetPostsOfUser")]
        public IActionResult GetPostsOfUser(int id) {
            List<Post> posts = Database.getInstance().getPostsFromUser(id);
            if (posts != null && posts.Count != 0) {
                return Ok(Outputter.getDynamicList(posts));
            }

            return NotFound("Element ne obstaja");
        }

        // GET: api/User/5
        [Route("get/{id}")]
        [HttpGet("{id}", Name = "getUser")]
        public IActionResult getUser(int id) {
            User user = Database.getInstance().getUser(id);
            if (user == null)
                return NotFound("User with" + id + " does not exist");
            return Ok(user.getDynamic());
        }

        [Route("username/{username}")]
        [HttpGet]
        public IActionResult Get([FromQuery] string logged, string username) {
            User user = Database.getInstance().getUserByUsername(username);
            if (user == null)
                return NotFound("User with" + username + " does not exist");
            List<Post> posts = Database.getInstance().getPostsFromUser(user.Id);
            int numOfPosts = posts.Count;
            dynamic usercek = user.getDynamic();
            usercek.posts = Outputter.getDynamicList(posts);
            usercek.postNumber = numOfPosts;
            usercek.samePerson = false;
            usercek.follows = false;
            if (logged != "") {
                User user2 = Database.getInstance().getUserByUsername(logged);
                if (user2 != null && Database.getInstance().isUserFollowingUser(user2, user)) {
                    usercek.follows = true;
                    
                }
                if (user2 != null && user2.Id == user.Id)
                {
                    usercek.samePerson = true;
                }
            }
            return Ok(usercek);
        }


        // POST: api/User
        [HttpPost]
        public IActionResult Post([FromBody] dynamic objecto) {
            if (objecto != null) {
                try {
                    string username = objecto["username"].ToString();
                    string handle = objecto["handle"].ToString();
                    string email = objecto["email"].ToString();
                    string password = objecto["password"].ToString();
                    User user = new User(username, handle, email, password);
                    if (user.validate()) {
                        if (Database.getInstance().InsertIntoUser(user, password)) {
                            return Ok(user.getDynamic());
                        }
                    }
                }
                catch (Exception e) {
                    return BadRequest("Napacni podatki");
                }
            }

            return BadRequest("Prazen objekt");
        }

        [Route("IsAdmin")]
        [HttpPost]
        public IActionResult PostAdmin([FromBody] dynamic objecto) {
            if (objecto != null) {
                try {
                    string username = objecto["username"].ToString();
                    string password = objecto["password"].ToString();
                    User user = Database.getInstance().getUserByUsername(username);

                    if (user != null) {
                        if (user.checkPassword(password)) {
                            return Ok("true");
                        }
                    }
                }
                catch (Exception e) {
                    return BadRequest("Napacni podatki");
                }
            }

            return BadRequest("Napacni podatki");
        }

        [Route("Login")]
        [HttpPost]
        public IActionResult LoginUser([FromBody] dynamic objecto) {
            if (objecto != null) {
                try {
                    string username = objecto["username"].ToString();
                    string password = objecto["password"].ToString();
                    User user = Database.getInstance().getUserByUsername(username);

                    if (user != null) {
                        if (user.checkPassword(password)) {
                            return Ok("true");
                        }
                    }
                }
                catch (Exception e) {
                    return BadRequest("Napacni podatki");
                }
            }

            return BadRequest("Napacni podatki");
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] dynamic value) {
            User org = Database.getInstance().getUser(id);

            if (value != null && org != null) {
                try {
                    int idU = value["id"];
                    User u = Database.getInstance().getUser(idU);
                    if (u.checkPassword(value["password"].ToString()) && u.Id == org.Id) {
                        string handle = value["handle"].ToString();
                        string image = value["image"].ToString();
                        if (handle.Length > 0 && image.Length > 0) {
                            if (Database.getInstance()
                                .updateUserByID(org, new User(org.Id, org.Username, handle, image))) {
                                return Ok(new User(org.Id, org.Username, handle, image).getDynamic());
                            }
                        }

                        return BadRequest("No text in the post!");
                    }

                    return NotFound("User not found!");
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }

            return BadRequest("Ni podatkov!");
        }


        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            User user = Database.getInstance().getUser(id);
            if (user != null) {
                //TODO: DELETE
                return Ok("deleted");
            }

            return BadRequest("Element ne obstaja!");
        }
    }
}