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
    public class GpsWatchLocationMiddleware : Middleware, IGpsWatchLocationMiddleware {

        private const string REPORT_TYPE = "location";

        public override bool OperateOnMessage(PipelineContext context) {

            var trackerRepository = context.Services.GetService(typeof(ITrackerRepository)) as ITrackerRepository;
            var reportRepository = context.Services.GetService(typeof(IReportRepository)) as IReportRepository;

            #region VALIDATION:
            var isValid = MessageHelper.Validate(context, trackerRepository);
            if (!isValid)
                return false;
            #endregion

            #region PROCESSING:
            _ = GpsWatchMessage.TryParse(context.Message.Base64Text, out var message);
            var tracker = trackerRepository.Get(message.UniqueID);
            var reportData = GpsWatchReportData.FromArray(message.ContentItems.ToArray());

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
            reportRepository.Add(report);
            reportRepository.SaveAsync().Wait();
            #endregion

            return true;
        }

        public override bool IsMatch(TrackerMessage message) {
            if (message == null || string.IsNullOrEmpty(message.Base64Text)
                || !TextUtil.IsBase64String(message.Base64Text))
                return false;

            var text = Encoding.ASCII.GetString(Convert.FromBase64String(message.Base64Text));
            return Regex.IsMatch(text, Patterns.MESSAGE_LOCATION);
        }
    }
}
