using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{

    public class AdminController : BaseAPIController
    {
        public UserManager<AppUser> _userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
            .Include(r => r.UserRoles)
            .ThenInclude(r => r.Role)
            .OrderBy(r => r.UserName)
            .Select(u => new
            {
                u.Id,
                Username = u.UserName,
                Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
            })
            .ToListAsync();
            return Ok(users);
        }
        [HttpPost("edit-roles/{username}")]

        public async Task<ActionResult> EditRoles(string username,[FromQuery] string roles)
        {
            var selectedRoles = roles.Split(',').ToArray();
            var user = await _userManager.FindByNameAsync(username);
            if(user==null) return NotFound("can not find user");
            var userRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            if(!result.Succeeded) return BadRequest("fail to add rols");
            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            if (!result.Succeeded) return BadRequest("fail to remove rols");
            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratorPhotoRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotosForModeration()
        {
            return Ok(" Admins or Moderators can see this");
        }
    }
}