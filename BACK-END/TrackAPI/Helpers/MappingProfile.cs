using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Constants;
using TrackAPI.Models;
using TrackDataAccess.Models;

namespace TrackAPI.Helpers {
    public class MappingProfile : Profile {
        public MappingProfile() {

            // Tracker <--> TrackerModel
            CreateMap<TrackerModel, Tracker>();
            CreateMap<Tracker, TrackerModel>()
                .ForMember(model => model.CreationTime, opt =>
                    opt.MapFrom(src => src.CreationTime.ToString(Values.DATETIME_FORMAT)));

        }
    }
}
