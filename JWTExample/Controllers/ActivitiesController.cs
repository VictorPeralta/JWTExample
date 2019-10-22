using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JWTExample.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace JWTExample.Controllers
{
    [Route("api/[controller]")]
    public class ActivitiesController : Controller
    {
        private readonly ActivityContext db;
        private readonly UserManager<IdentityUser> _userManager;

        public ActivitiesController(ActivityContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            db = context;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public IEnumerable<ActivityLog> Get()
        {
            return db.ActivityLog.ToList();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult<ActivityLog>> PostAsync([FromBody] ActivityLog activity)
        {
            var user = await _userManager.FindByIdAsync(User.Identity.Name);
            activity.User = user;
            db.Add(activity);
            db.SaveChanges();
            return CreatedAtAction(nameof(Get), new {id = activity.Id}, activity);
        }


    }
}
