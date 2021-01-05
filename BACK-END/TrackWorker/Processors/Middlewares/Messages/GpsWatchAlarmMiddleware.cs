using System;
using System.Text;
using System.Text.RegularExpressions;
using TrackLib.Constants;
using TrackLib.Utils;
using TrackWorker.Helpers;
using TrackWorker.Models;
using TrackWorker.Processors.Pipelines;
using TrackWorker.Services;

namespace TrackWorker.Processors.Middlewares.Messages {
    public class GpsWatchAlarmMiddleware : Middleware, IGpsWatchAlarmMiddleware {

        private const string REPORT_TYPE = "alarm";

        public override bool OperateOnMessage(PipelineContext context) {

            var trackerService = context.Services.GetService(typeof(ITrackerService)) as ITrackerService;
            var reportService = context.Services.GetService(typeof(IMessageService)) as IMessageService;

            #region VALIDATION:
            var isValid = MessageHelper.Validate(context, trackerService);
            if (!isValid)
                return false;
            #endregion

            #region SAVE IN DATABASE:
            _ = GpsWatchMessage.TryParse(context.Message.Base64Text, out var message);
            var tracker = trackerService.Get(message.UniqueID);
            var reportData = GpsWatchReportData.FromArray(message.ContentItems.ToArray());

            var report = new GpsTrackerMessageModel {
                MessageType = REPORT_TYPE,
                TrackerId = tracker.Id,
                MessageTime = reportData.ReportTime,
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
            reportService.AddAsync(report).Wait();
            #endregion

            #region REPLY TO TRACKER:
            var response = GpsWatchMessage.GetCommandText(message.Manufacturer, message.TrackerId, message.MessageType.Length.ToString("X4"), message.MessageType);
            var responseBytes = Encoding.ASCII.GetBytes(response);
            context.Message.Socket.Send(responseBytes);
            #endregion

            return true;
        }

        public override bool IsMatch(TrackerMessage message) {
            if (message == null || string.IsNullOrEmpty(message.Base64Text)
                || !TextUtil.IsBase64String(message.Base64Text))
                return false;

            var text = Encoding.ASCII.GetString(Convert.FromBase64String(message.Base64Text));
            return Regex.IsMatch(text, Patterns.MESSAGE_ALARM);
        }
    }
}
