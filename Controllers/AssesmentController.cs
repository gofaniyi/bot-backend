using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloEpidBot.Model.Domain;
using GloEpidBot.Persistence.Contexts;
using GloEpidBot.Utilities;
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


        [HttpGet]
        [Route("api/v1/assessments/id")]
        public IActionResult GetAssesment([FromRoute] string id)
        {
            try
            {
                return Ok(new SingleResponse<assesment>
                {
                    DidError = false,
                    Message = "Items retrieved successfully",
                    data = new Data<assesment>
                    {
                        attributes = _context.GetAssesments.Where(x=>x.assesmentId == id).FirstOrDefault(),
                         Id  =id,
                         type ="Assessments"
                         

                    }
                });
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




        [HttpGet]
        [Route("api/v1/assessments")]
        public IActionResult GetAssesments()
        {
            try
            {
                return Ok(new PagedResponse<assesment>
                {
                    DidError = false,
                    Message = "Items retrieved successfully",
                    data = new DataList<assesment>
                    {
                        attributes = _context.GetAssesments.ToList(),
                        type = "Assessment",

                    },
                    

                }
                );
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

        [HttpPost]
        [Route("api/v1/assessments")]
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
