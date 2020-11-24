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
using TrackWorker.Shared;

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
            var isValid = MessageHelper.Validate(context, _trackerRepository);
            if (!isValid)
                return false;
            #endregion

            #region PROCESSING:
            _ = ThreeGElecMessage.TryParse(context.Message.Base64Text, out var message);
            var tracker = _trackerRepository.Get(message.UniqueID);
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
            #endregion

            return true;
        }

        public override bool IsMatch(Message message) {
            if (message == null || string.IsNullOrEmpty(message.Base64Text)
                || !TextUtil.IsBase64String(message.Base64Text))
                return false;

            var text = Encoding.ASCII.GetString(Convert.FromBase64String(message.Base64Text));
            var regex = new Regex(Patterns.MESSAGE_LOCATION);
            return regex.IsMatch(text);
        }
    }
}
