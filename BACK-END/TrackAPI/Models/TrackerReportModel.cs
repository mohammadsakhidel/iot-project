﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Models {
    public class TrackerReportModel {
        public int Id { get; set; }
        public string ReportType { get; set; }
        public string TrackerId { get; set; }
        public double Latitude { get; set; }
        public string LatitudeMark { get; set; }
        public double Longitude { get; set; }
        public string LongitudeMark { get; set; }
        public bool IsValid { get; set; }
        public double Speed { get; set; }
        public double Direction { get; set; }
        public double? Altitude { get; set; }
        public double? SignalStrength { get; set; }
        public double? Battery { get; set; }
        public string TrackerState { get; set; }
        public string CreationTime { get; set; }
    }
}
