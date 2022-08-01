using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }
[HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>>  GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

      [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>>  GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}