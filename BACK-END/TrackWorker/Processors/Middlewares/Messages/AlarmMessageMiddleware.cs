using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrackDataAccess.Models;
using TrackDataAccess.Repositories;
using TrackLib.Constants;
using TrackLib.Utils;
using TrackWorker.Helpers;
using TrackWorker.Processors.Pipelines;

namespace TrackWorker.Processors.Middlewares.Messages {
    public class AlarmMessageMiddleware : Middleware, IAlarmMessageMiddleware {

        private readonly ITrackerRepository _trackerRepository;
        private readonly IAlarmReportRepository _alarmRepository;
        public AlarmMessageMiddleware(ITrackerRepository trackerRepository,
            IAlarmReportRepository alarmRepository) {
            _trackerRepository = trackerRepository;
            _alarmRepository = alarmRepository;
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

            var data = ThreeGElecLocMessageData.FromArray(message.ContentItems.ToArray());
            var alarmReport = new AlarmReport {
                TrackerId = tracker.Id,
                CreationTime = data.ReportTime,
                Latitude = data.Latitude,
                LatitudeMark = data.LatitudeMark.ToString(),
                Longitude = data.Longitude,
                LongitudeMark = data.LongitudeMark.ToString(),
                Speed = data.Speed,
                Direction = data.Direction,
                Altitude = data.Altitude,
                Battery = data.Power,
                SignalStrength = data.SignalStrength,
                TrackerState = data.TrackerStateBinary
            };
            _alarmRepository.Add(alarmReport);
            _alarmRepository.SaveAsync().Wait();
            #endregion

            return true;
        }

        public override bool IsMatch(Message message) {
            if (message == null || string.IsNullOrEmpty(message.Base64Text)
                || !TextUtil.IsBase64String(message.Base64Text))
                return false;

            var text = Encoding.ASCII.GetString(Convert.FromBase64String(message.Base64Text));
            return Regex.IsMatch(text, Patterns.MESSAGE_ALARM);
        }
    }
}
