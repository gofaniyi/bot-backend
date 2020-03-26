using GloEpidBot.Model.Services.Communication;

using GloEpidBot.Model.Domain;

namespace GloEpidBot.Model.Services.Communication
{
    public class SaveReportResponse : BaseResponse
    {
        public Report Report { get; private set; }

        private SaveReportResponse(bool success, string message, Report report) : base(success, message)
        {
            Report = report;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="report">Saved report.</param>
        /// <returns>Response.</returns>
        public SaveReportResponse(Report report) : this(true, string.Empty, report)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public SaveReportResponse(string message) : this(false, message, null)
        { }
    }
}