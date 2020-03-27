using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloEpidBot.Model.Domain;
using GloEpidBot.Model.Parameters;
using GloEpidBot.Model.Repositories;
using GloEpidBot.Model.Services.Communication;
using GloEpidBot.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GloEpidBot.Persistence.Repositories
{
    public class ReportRepository : BaseRepository<Report>, IReportRepository
    {
        public ReportRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Report>> ListAsync(ReportParameters reportParameters)
        {
            
            var query = _context.Reports.AsQueryable();
            if (!string.IsNullOrEmpty(reportParameters.SearchString))
            {
                query = query.Where(r => r.RiskStatus.Contains(reportParameters.SearchString) 
                        || r.Location.Contains(reportParameters.SearchString));  
            }
            
            return PagedList<Report>.ToPagedList(query,
		        reportParameters.PageNumber,
		        reportParameters.PageSize).ToList();
        }

        public async Task<SaveReportResponse> FindReport(int id)
        {
            var report = await FindByCondition(r => r.Id.Equals(id))
            .FirstOrDefaultAsync();

             if (report == null)
		        return new SaveReportResponse("Report not found.");

           return new SaveReportResponse(report);
        }
    }
}