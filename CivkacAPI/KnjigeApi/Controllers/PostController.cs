using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Civkac.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Civkac.Controllers {
    [Produces("application/json")]
    [Route("api/Post")]
    public class PostController : Controller {
        // GET: api/Post
        [HttpGet]
        public IEnumerable<dynamic> Get() {
            return Outputter.getDynamicList(Database.getInstance().getAllPosts());
        }


        // GET: api/Post/5
        [HttpGet("{id}", Name = "GetPost")]
        public IActionResult GetPost(int id) {
            Post post = Database.getInstance().getPost(id);
            if (post != null) {
                return Ok(post.getDynamic());
            }

            return NotFound("Element ne obstaja");
        }

        //// POST: api/Post
        [HttpPost]
        public IActionResult Post([FromBody] dynamic value) {
            if (value != null) {
                try {
                    int id = value["id"];
                    User u = Database.getInstance().getUser(id);
                    if (u.checkPassword(value["password"].ToString())) {
                        string text = value["text"].ToString();
                        if (text.Length > 0) {
                            if (Database.getInstance().InsertIntoPost(new Post(text, u))) {
                                return Ok(new Post(text, u).getDynamic());
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

        [Route("tweet")]
        [HttpPost(Name = "PostTweet")]
        public IActionResult PostTweet([FromBody] dynamic value)
        {
            if (value != null)
            {
                try
                {
                    String id = value["username"];
                    User u = Database.getInstance().getUserByUsername(id);
                    if (u.checkPassword(value["password"].ToString()))
                    {
                        string text = value["text"].ToString();
                        if (text.Length > 0)
                        {
                            if (Database.getInstance().InsertIntoPost(new Post(text, u)))
                            {
                                return Ok(new Post(text, u).getDynamic());
                            }
                        }

                        return BadRequest("No text in the post!");
                    }

                    return NotFound("User not found!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return BadRequest("Ni podatkov!");
        }

        // PUT: api/Post/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] dynamic value) {
            Post org = Database.getInstance().getPost(id);

            if (value != null && org != null) {
                try {
                    int idU = value["id"];
                    User u = Database.getInstance().getUser(idU);
                    if (u.checkPassword(value["password"].ToString()) && u.Id == org.Author.Id) {
                        string text = value["text"].ToString();
                        if (text.Length > 0) {
                            if (Database.getInstance().updatePostByID(org, new Post(text, u))) {
                                return Ok(new Post(text, u).getDynamic());
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
            Post post = Database.getInstance().getPost(id);
            if (post != null) {
                if (Database.getInstance().deletePostById(id)) {
                    return Ok("deleted");
                }
            }

            return BadRequest("Element ne obstaja!");
        }
    }
}