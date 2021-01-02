using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Models;
using TrackWorker.Models;

namespace TrackWorker.Helpers {
    public class MappingProfile : Profile {

        public MappingProfile() {

            // AccessCode <--> AccessCodeModel
            CreateMap<AccessCodeModel, AccessCode>().ReverseMap();

            // Tracker <--> TrackerModel
            CreateMap<TrackerModel, Tracker>().ReverseMap();

            // TrackerUser <--> TrackerUserModel
            CreateMap<TrackerUserModel, TrackerUser>().ReverseMap();

            // Report <--> ReportModel
            CreateMap<ReportModel, Report>().ReverseMap();

        }

    }
}
