using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Civkac.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Civkac.Controllers {
    [Produces("application/json")]
    [Route("api/Feed")]
    public class FeedController : Controller {
        // GET: api/Feed/5
        [HttpGet("{id}", Name = "GetFeed")]
        public IActionResult GetFeed(int id) {
            List<Post> posts = Database.getInstance().getFeed(id);
            if (posts != null && posts.Count != 0) {
                return Ok(Outputter.getDynamicList(posts));
            }

            return NotFound("Element ne obstaja");
        }

        [Route("user/{username}")]
        [HttpGet("{username}", Name = "GetFeedFromUsername")]
        public IActionResult GetFeed(string username)
        {
            List<Post> posts = Database.getInstance().getFeed(username);
            if (posts != null)
            {
                return Ok(Outputter.getDynamicList(posts));
            }

            return NotFound("Element ne obstaja");
        }
    }
}