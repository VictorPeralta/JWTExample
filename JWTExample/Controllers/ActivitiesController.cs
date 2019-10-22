using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JWTExample.Models;
using Microsoft.AspNetCore.Identity;

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

        [HttpGet]
        public IEnumerable<ActivityLog> Get()
        {
            return db.ActivityLog.ToList();
        }

        [HttpPost]
        public ActionResult<ActivityLog> Post([FromBody] ActivityLog activity)
        {
            db.Add(activity);
            db.SaveChanges();
            return CreatedAtAction(nameof(Get), new {id = activity.Id}, activity);
        }


    }
}
