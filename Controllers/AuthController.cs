using GloEpidBot.Model.Domain;
using GloEpidBot.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloEpidBot.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        public AuthController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        [Route("api/v1/auth/partner-token")]
        public IActionResult GeneratePartnerToken(PartnerTokenModel tokenModel)
        {
            return Ok();
        }
        
        [HttpPost]
        [Route("api/v1/auth/invite-user")]
        public IActionResult InviteUser()
        {
            return Ok();
        }
    }
}
