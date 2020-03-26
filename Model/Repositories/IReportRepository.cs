using System.Collections.Generic;
using System.Threading.Tasks;
using GloEpidBot.Model.Domain;
using GloEpidBot.Model.Parameters;
using GloEpidBot.Model.Services.Communication;
using Microsoft.AspNetCore.Mvc;

namespace GloEpidBot.Model.Repositories
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> ListAsync(ReportParameters reportParameters);
        Task<SaveReportResponse> FindReport(int id);
        //Task<SaveReportResponse> GetReport(int id);
    }
}