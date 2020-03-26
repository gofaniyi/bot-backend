using AutoMapper;
using GloEpidBot.Model.Domain;
using GloEpidBot.Resources;

namespace Gloepid_AdminAPI.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Report, ReportResource>();
        }
    }
}