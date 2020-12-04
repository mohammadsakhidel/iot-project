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
    public class ReportMessageMiddleware : Middleware, IReportMessageMiddleware {

        private readonly ITrackerRepository _trackerRepository;
        private readonly IReportRepository _reportRepository;
        public ReportMessageMiddleware(ITrackerRepository trackerRepository,
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
            var report = ThreeGElecReportData.FromArray(message.ContentItems.ToArray());

            var locReport = new Report {
                ReportType = report.ReportType.ToLower() switch {
                    "ud" => "location",
                    "al" => "alarm",
                    _ => "unknown" 
                },
                TrackerId = tracker.Id,
                CreationTime = report.ReportTime,
                Latitude = report.Latitude,
                LatitudeMark = report.LatitudeMark.ToString(),
                Longitude = report.Longitude,
                LongitudeMark = report.LongitudeMark.ToString(),
                IsValid = report.IsValid,
                Speed = report.Speed,
                Direction = report.Direction,
                Altitude = report.Altitude,
                Battery = report.Power,
                SignalStrength = report.SignalStrength,
                TrackerState = report.TrackerStateBinary
            };
            _reportRepository.Add(locReport);
            _reportRepository.SaveAsync().Wait();
            #endregion

            return true;
        }

        public override bool IsMatch(Message message) {
            if (message == null || string.IsNullOrEmpty(message.Base64Text)
                || !TextUtil.IsBase64String(message.Base64Text))
                return false;

            var text = Encoding.ASCII.GetString(Convert.FromBase64String(message.Base64Text));
            var regex = new Regex(Patterns.MESSAGE_REPORT);
            return regex.IsMatch(text);
        }
    }
}
