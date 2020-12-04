using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TrackDataAccess.Models;
using TrackDataAccess.Repositories;
using TrackLib.Constants;
using TrackLib.Utils;
using TrackWorker.Helpers;
using TrackWorker.Processors.Pipelines;
using TrackWorker.Shared;

namespace TrackWorker.Processors.Middlewares.Messages {
    public class LocationMessageMiddleware : Middleware, ILocationMessageMiddleware {

        private const string REPORT_TYPE = "location";

        private readonly ITrackerRepository _trackerRepository;
        private readonly IReportRepository _reportRepository;
        public LocationMessageMiddleware(ITrackerRepository trackerRepository,
            IReportRepository locationRepository) {

            _trackerRepository = trackerRepository;
            _reportRepository = locationRepository;

        }

        public override bool OperateOnMessage(PipelineContext context) {
            #region VALIDATION:
            var isValid = MessageHelper.Validate(context, _trackerRepository);
            if (!isValid)
                return false;
            #endregion

            #region PROCESSING:
            _ = ThreeGElecMessage.TryParse(context.Message.Base64Text, out var message);
            var tracker = _trackerRepository.Get(message.UniqueID);
            var reportData = ThreeGElecReportData.FromArray(message.ContentItems.ToArray());

            var report = new Report {
                ReportType = REPORT_TYPE,
                TrackerId = tracker.Id,
                ReportTime = reportData.ReportTime,
                Latitude = reportData.Latitude,
                LatitudeMark = reportData.LatitudeMark.ToString(),
                Longitude = reportData.Longitude,
                LongitudeMark = reportData.LongitudeMark.ToString(),
                IsValid = reportData.IsValid,
                Speed = reportData.Speed,
                Direction = reportData.Direction,
                Altitude = reportData.Altitude,
                Battery = reportData.Power,
                SignalStrength = reportData.SignalStrength,
                TrackerState = reportData.TrackerStateBinary,
                CreationTime = DateTime.UtcNow
            };
            _reportRepository.Add(report);
            _reportRepository.SaveAsync().Wait();
            #endregion

            return true;
        }

        public override bool IsMatch(Message message) {
            if (message == null || string.IsNullOrEmpty(message.Base64Text)
                || !TextUtil.IsBase64String(message.Base64Text))
                return false;

            var text = Encoding.ASCII.GetString(Convert.FromBase64String(message.Base64Text));
            return Regex.IsMatch(text, Patterns.MESSAGE_LOCATION);
        }
    }
}
