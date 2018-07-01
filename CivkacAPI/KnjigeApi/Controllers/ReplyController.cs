using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Civkac.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Civkac.Controllers {
    [Produces("application/json")]
    [Route("api/Reply")]
    public class ReplyController : Controller {
        // GET: api/Reply/5
        [HttpGet("{id}", Name = "GetReplies")]
        public IEnumerable<dynamic> GetReplies(int id) {
            return Outputter.getDynamicList(Database.getInstance().getAllReplies(id));
        }

        [Route("get/{id}")]
        [HttpGet("{id}", Name = "GetReplies")]
        public dynamic GetOneReply(int id) {
            return Database.getInstance().getReply(id).getDynamic();
        }

        // POST: api/Reply
        [HttpPost]
        public IActionResult Post([FromBody] dynamic value) {
            if (value != null) {
                try {
                    int postId = value["postId"];
                    int id = value["id"];
                    User u = Database.getInstance().getUser(id);
                    if (u.checkPassword(value["password"].ToString())) {
                        string text = value["text"].ToString();
                        if (text.Length > 0) {
                            if (Database.getInstance().InsertIntoReply(new Reply(text, u), postId)) {
                                return Ok(new Reply(text, u).getDynamic());
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
        [Route("post/{identifikator}")]
        [HttpPost("{identifikator}", Name = "PostReply")]
        public IActionResult PostReply(int identifikator,[FromBody] dynamic value)
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
                            if (Database.getInstance().InsertIntoReply(new Reply(text, u), identifikator))
                            {
                                return Ok(new Reply(text, u).getDynamic());
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
        // PUT: api/Reply/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] dynamic value) {
            Reply org = Database.getInstance().getReply(id);

            if (value != null && org != null) {
                try {
                    int idU = value["id"];
                    User u = Database.getInstance().getUser(idU);
                    if (u.checkPassword(value["password"].ToString()) && u.Id == org.User.Id) {
                        string text = value["text"].ToString();
                        if (text.Length > 0) {
                            if (Database.getInstance().updateReplyByID(org, new Reply(text, u))) {
                                return Ok(new Reply(text, u).getDynamic());
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
            //TODO: REPLY
            Reply repy = Database.getInstance().getReply(id);
            if (repy != null) {
                if (Database.getInstance().deleteReplyById(id)) {
                    return Ok("deleted");
                }
            }

            return BadRequest("Element ne obstaja!");
        }
    }
}