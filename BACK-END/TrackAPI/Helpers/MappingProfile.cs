
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
                .ForMember(dest => dest.CreationTime, opt =>
                    opt.MapFrom(src => src.CreationTime.ToString(Values.DATETIME_FORMAT)));

            // Report <--> TrackerReportModel
            CreateMap<TrackerReportModel, Report>();
            CreateMap<Report, TrackerReportModel>()
                .ForMember(dest => dest.CreationTime, opt =>
                    opt.MapFrom(src => src.CreationTime.ToString(Values.DATETIME_FORMAT)));

            // CommandLog <--> CommandLogModel
            CreateMap<CommandLogModel, CommandLog>();
            CreateMap<CommandLog, CommandLogModel>()
                .ForMember(dest => dest.CreationTime, opt =>
                    opt.MapFrom(src => src.CreationTime.ToString(Values.DATETIME_FORMAT)));

            // Image <--> ImageModel
            CreateMap<ImageModel, Image>();
            CreateMap<Image, ImageModel>();
        }
    }
}
