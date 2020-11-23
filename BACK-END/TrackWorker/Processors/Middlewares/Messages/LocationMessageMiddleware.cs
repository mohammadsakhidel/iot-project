using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TrackDataAccess.Models;
using TrackDataAccess.Repositories;
using TrackLib.Constants;
using TrackLib.Utils;
using TrackWorker.Models;
using TrackWorker.Processors.Pipelines;
using TrackWorker.Utils;

namespace TrackWorker.Processors.Middlewares.Messages {
    public class LocationMessageMiddleware : Middleware, ILocationMessageMiddleware {

        private readonly ITrackerRepository _trackerRepository;
        private readonly ILocationReportRepository _locationRepository;
        public LocationMessageMiddleware(ITrackerRepository trackerRepository,
            ILocationReportRepository locationRepository) {
            _trackerRepository = trackerRepository;
            _locationRepository = locationRepository;
        }

        public override bool OperateOnMessage(PipelineContext context) {
            #region VALIDATION:
            // Null Inputs check:
            if (context == null || context.Message == null)
                return false;

            // Parse Message:
            var messageParsed = ThreeGElecMessage.TryParse(context.Message.Base64Text, out var message);
            if (!messageParsed)
                return false;

            // Check database for tracker existence:
            var tracker = _trackerRepository.Get(message.UniqueID);
            if (tracker == null)
                return false;
            #endregion

            #region PROCESSING:
            var loc = ThreeGElecLocMessageData.FromArray(message.ContentItems.ToArray());
            if (loc.IsValid) {
                var locReport = new LocationReport {
                    TrackerId = tracker.Id,
                    ReportTime = loc.ReportTime,
                    Latitude = loc.Latitude,
                    LatitudeMark = loc.LatitudeMark.ToString(),
                    Longitude = loc.Longitude,
                    LongitudeMark = loc.LongitudeMark.ToString(),
                    Speed = loc.Speed,
                    Direction = loc.Direction,
                    Altitude = loc.Altitude,
                    Battery = loc.Power,
                    SignalStrength = loc.SignalStrength,
                    TrackerState = loc.TrackerStateBinary
                };
                _locationRepository.Add(locReport);
                _locationRepository.SaveAsync().Wait();
            }
            return true;
            #endregion
        }

        public override bool ValidateMessage(Message message) {
            if (message == null || string.IsNullOrEmpty(message.Base64Text)
                || !TextUtil.IsBase64String(message.Base64Text))
                return false;

            var text = Encoding.ASCII.GetString(Convert.FromBase64String(message.Base64Text));
            var regex = new Regex(Patterns.MESSAGE_LOCATION);
            return regex.IsMatch(text);
        }
    }
}
