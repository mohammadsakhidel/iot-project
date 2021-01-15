
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Constants;
using TrackAPI.Models;
using TrackDataAccess.Models;
using TrackLib.Constants;

namespace TrackAPI.Helpers {
    public class MappingProfile : Profile {
        public MappingProfile() {

            // Tracker <--> TrackerModel
            CreateMap<TrackerModel, Tracker>();
            CreateMap<Tracker, TrackerModel>()
                .ForMember(dest => dest.CreationTime, opt =>
                    opt.MapFrom(src => src.CreationTime.ToString(SharedValues.DATETIME_FORMAT)));

            // Report <--> TrackerReportModel
            CreateMap<Message, MessageModel>()
                .Include<GpsTrackerMessage, GpsTrackerMessageModel>()
                .ForMember(dest => dest.CreationTime, opt =>
                    opt.MapFrom(src => src.CreationTime.ToString(SharedValues.DATETIME_FORMAT)));
            CreateMap<GpsTrackerMessage, GpsTrackerMessageModel>();

            // CommandLog <--> CommandLogModel
            CreateMap<CommandLogModel, CommandLog>();
            CreateMap<CommandLog, CommandLogModel>()
                .ForMember(dest => dest.CreationTime, opt =>
                    opt.MapFrom(src => src.CreationTime.ToString(SharedValues.DATETIME_FORMAT)));

            // Image <--> ImageModel
            CreateMap<ImageModel, Image>();
            CreateMap<Image, ImageModel>();

            // AccessCode <--> AccessCodeModel
            CreateMap<AccessCodeModel, AccessCode>();
            CreateMap<AccessCode, AccessCodeModel>();

        }
    }
}
