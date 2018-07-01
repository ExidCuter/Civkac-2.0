using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Civkac.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Civkac.Controllers {
    [Produces("application/json")]
    [Route("api/Report")]
    public class ReportController : Controller {
        // GET: api/Report
        [HttpGet]
        public IEnumerable<dynamic> Get() {
            return Outputter.getDynamicList(Database.getInstance().getAllReports());
        }

        // GET: api/Report/5
        [HttpGet("{id}", Name = "GetReports")]
        public IActionResult GetReports(int id) {
            Report r = Database.getInstance().getReport(id);
            if (r != null) {
                return Ok(r.getDynamic());
            }

            return NotFound("Ne obstaja");
        }

        // POST: api/Report
        [HttpPost]
        public IActionResult Post([FromBody] dynamic value) {
            //TODO: INSERT INTO DATABASE
            if (value != null) {
                try {
                    int reporterid = value["author"];
                    int reporteid = value["reportedUser"];

                    User reporter = Database.getInstance().getUser(reporterid);
                    User reporte = Database.getInstance().getUser(reporteid);

                    if (reporte != null && reporter != null) {
                        string text = value["reason"].ToString();
                        if (text.Length > 0) {
                            if (Database.getInstance().InsertIntoReport(new Report(text, reporter, reporte))) {
                                return Ok(new Report(text, reporter, reporte));
                            }
                        }

                        return BadRequest("Ni Razloga!");
                    }

                    return BadRequest("Napacni podatki!");
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return BadRequest("Ni podatkov!");
        }

        // PUT: api/Report/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] dynamic value) {
            //TODO: CHECK IF POST BELONGS TO USER THAT SENT THE REQUEST
            Report org = Database.getInstance().getReport(id);

            if (value != null && org != null) {
                try {
                    int idU = value["author"]["id"];
                    User u = Database.getInstance().getUser(idU);
                    if (u.checkPassword(value["author"]["password"].ToString()) && u.Id == org.Author.Id) {
                        string text = value["text"].ToString();
                        if (text.Length > 0) {
                            if (Database.getInstance().updateReportByID(org, text)) {
                                return Ok("Updated");
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
                //TODO: DELETE

                return Ok("deleted");
            }

            return BadRequest("Element ne obstaja!");
        }
    }
}