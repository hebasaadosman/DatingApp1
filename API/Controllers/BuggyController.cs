using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseAPIController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;

        }
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text ";
        }
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFount()
        {
            var thing = _context.Users.Find(-1);
            if (thing == null)
                return NotFound();
            return Ok(thing);
        }
        [HttpGet("server-error")]
        public ActionResult<string> GetSeverError()
        {

            var thing = _context.Users.Find(-1);
            var thingTpReturn = thing.ToString();
            return thingTpReturn;

        }
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequestt()
        {
            return BadRequest();
        }


    }
}