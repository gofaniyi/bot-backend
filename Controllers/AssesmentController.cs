using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloEpidBot.Model.Domain;
using GloEpidBot.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using static GloEpidBot.Utilities.Responses;

namespace GloEpidBot.Controllers
{
    public class AssesmentController : Controller
    {
        private readonly AppDbContext _context;

        public AssesmentController(AppDbContext context)
        {
            _context = context;
        }



        [HttpPost]
        [Route("api/v1/assesments")]
        public IActionResult assesments([FromBody] assesmentModel assesment)
        {
            if(!ModelState.IsValid)
                if (!ModelState.IsValid)
                {
                    var ErrorBox = new ErrorBoss();

                    foreach (var model in ModelState.Values)
                    {

                        foreach (var error in model.Errors)
                        {
                            ErrorBox.Errors.Add(new ErrorResponse
                            {
                                Status = "100",
                                Details = error.ErrorMessage,
                                Title = "One or more parameter(s) is invalid",


                            });
                        }
                    }
                    ErrorBox.Message = "Parameter validation failed";
                    ErrorBox.DidError = true;

                    return BadRequest(ErrorBox);
                }

            try
            {
                return Ok();
            }
            catch (System.Exception ex)
            {

                return StatusCode(500, new ErrorBoss
                {
                    DidError = true,
                    Message = "An internal error occurred",
                    Errors = new List<ErrorResponse>()
                     {
                          new ErrorResponse
                          {
                               Title = "Internal Error",
                               Status = "500",
                               Details = ex.Message
                          }
                     }
                });
            }
           
        }










    }


   
   
}
