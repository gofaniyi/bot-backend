using System.Collections.Generic;
using System.Threading.Tasks;
using GloEpidBot.Model.Domain;
using GloEpidBot.Model.Parameters;
using GloEpidBot.Model.Repositories;
using GloEpidBot.Model.Services;
using GloEpidBot.Model.Services.Communication;
using Microsoft.AspNetCore.Mvc;

namespace GloEpidBot.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            this._reportRepository = reportRepository;
        }

        public async Task<IEnumerable<Report>> ListAsync(ReportParameters reportParameters)
        { 
            return await _reportRepository.ListAsync(reportParameters);
        }

        public async Task<SaveReportResponse>  GetReport(int id)
        {
            var report =  await _reportRepository.FindReport(id);
             
            return report;
        }

        public async Task<IEnumerable<SelfAssesment>>  GetSelfAssessment()
        {
            var assessment =  await _reportRepository.ListSelfAssessment();
             
            return assessment;
        }

    }
}