﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Models.Polling {
    public class PollingInput {
        public PollingInputStatusItem[] TrackersStatus { get; set; }
    }
}
