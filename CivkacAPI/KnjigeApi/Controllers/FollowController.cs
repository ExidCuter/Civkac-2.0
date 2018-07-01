using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Civkac.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Civkac.Controllers {
    [Produces("application/json")]
    [Route("api/Follow")]
    public class FollowController : Controller {
        // POST: api/Follow
        [HttpPost]
        public IActionResult Post([FromBody] dynamic value) {
            if (value != null) {
                try {
                    String username1 = value["username1"];
                    String username2 = value["username2"];
                    User u1 = Database.getInstance().getUserByUsername(username1);
                    User u2 = Database.getInstance().getUserByUsername(username2);
                    if (u1.checkPassword(value["password"].ToString())) {
                        if (Database.getInstance().isUserFollowingUser(u1, u2)) {
                            Database.getInstance().UserUnFollowsUser(u1, u2);
                            return Ok("following");
                        }
                        Database.getInstance().UserFollowsUser(u1, u2);
                        return Ok("Unfollowed");
                        
                    }

                    return NotFound("User not found!");
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }

            return BadRequest("Ni podatkov!");
        }
    }
}