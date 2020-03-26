using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloEpidBot.Model.Domain;
using GloEpidBot.Model.Parameters;
using GloEpidBot.Model.Repositories;
using GloEpidBot.Model.Services.Communication;
using GloEpidBot.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace GloEpidBot.Persistence.Repositories
{
    public class ReportRepository : BaseRepository, IReportRepository
    {
        public ReportRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Report>> ListAsync(ReportParameters reportParameters)
        {
            var query = _context.Reports.AsQueryable();
            var searchBy = reportParameters.SearchBy.Trim().ToLowerInvariant();  
             query.Where(r => r.Location.ToLowerInvariant().Contains(searchBy) 
                            || r.Age >= reportParameters.MinAge && r.Age <= reportParameters.MaxAge
                        /* || r.ReporterName.ToLowerInvariant().Contains(searchBy)  
                        || r.RiskStatus.ToLowerInvariant().Contains(searchBy)
                        || r.Symptoms.ToLowerInvariant().Contains(searchBy)
                        || r.DateReported.ToShortDateString().Equals(searchBy) */)
                        .OrderBy(r => r.DateReported);  
        
            return PagedList<Report>.ToPagedList(query,
		        reportParameters.PageNumber,
		        reportParameters.PageSize).ToList();
        }

        public async Task<SaveReportResponse> FindReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            
           return new SaveReportResponse(report);
        }
    }
}