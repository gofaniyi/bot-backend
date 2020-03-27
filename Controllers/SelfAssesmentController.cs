using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloEpidBot.Model.Domain;
using GloEpidBot.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace GloEpidBot.Controllers
{
    [Route("api/[controller]")]
    public class SelfAssesmentController : Controller
    {
        private readonly AppDbContext _context;

        public SelfAssesmentController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
       public async Task<IEnumerable<SelfAssesment>>  GetSelfAssessment() 
       {
            return _context.Assesments.ToList();
       }
    }
    
}