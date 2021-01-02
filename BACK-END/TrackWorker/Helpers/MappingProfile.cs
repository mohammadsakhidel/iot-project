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
            CreateMap<AccessCodeModel, AccessCode>();
            CreateMap<AccessCode, AccessCodeModel>();

        }

    }
}
