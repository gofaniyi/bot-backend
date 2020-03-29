using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using GloEpidBot.Model.Services;
using GloEpidBot.Resources;
using GloEpidBot.Model.Domain;
using GloEpidBot.Model.Parameters;
using GloEpidBot.Model.Services.Communication;
using GloEpidBot.Persistence.Contexts;

namespace GloEpidBot.Controllers
{
    [Route("api/[controller]")]
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IMapper _mapper;
       
        
        public ReportsController(IReportService reportService, IMapper mapper)
        {
            _reportService = reportService;   
            _mapper = mapper;
          
        }

        [HttpGet]
        public async Task<IEnumerable<ReportResource>> GetAllAsync([FromQuery] ReportParameters reportParameters)
        {
            var reports = await _reportService.ListAsync(reportParameters);
            
            var resources = _mapper.Map<IEnumerable<Report>, IEnumerable<ReportResource>>(reports);
            return resources;
        }
       
        
        [HttpGet("{id}")]
        public async Task<SaveReportResponse>  GetReport(int id)
        {
            var reports = await _reportService.GetReport(id);
            
            return reports;
        }


    }
}