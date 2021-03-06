using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrackLib.Constants;
using TrackLib.Utils;
using TrackWorker.Helpers;
using TrackWorker.Models;
using TrackWorker.Processors.Pipelines;
using TrackWorker.ServerEvents;
using TrackWorker.Services;
using TrackWorker.Shared;

namespace TrackWorker.Processors.Middlewares.Messages {
    public class GpsWatchLocationMiddleware : Middleware, IGpsWatchLocationMiddleware {

        private const string REPORT_TYPE = "location";

        public override bool OperateOnMessage(PipelineContext context) {

            var trackerService = context.Services.GetService(typeof(ITrackerService)) as ITrackerService;
            var reportService = context.Services.GetService(typeof(IMessageService)) as IMessageService;

            #region VALIDATION:
            var isValid = MessageHelper.Validate(context, trackerService);
            if (!isValid)
                return false;
            #endregion

            #region PROCESSING:
            _ = GpsWatchMessage.TryParse(context.Message.Base64Text, out var message);
            var tracker = trackerService.GetWithIncludeAsync(message.UniqueID).Result;
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

            // Send location updated server event to all listening users:
            tracker.Users.ForEach(user => {
                if (UserConnections.Contains(user.UserId)) {
                    var clients = UserConnections.Get(user.UserId).Select(c => c.Client).ToList();
                    var @event = new LocationUpdatedServerEvent(tracker.Id) {
                        Latitude = reportData.Latitude,
                        Longitude = reportData.Longitude,
                        Altitude = reportData.Altitude,
                        Speed = reportData.Speed,
                        Direction = reportData.Direction,
                        Battery = reportData.Power
                    };

                    ServerEvent.SendToAll(@event, clients);
                }
            });
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
