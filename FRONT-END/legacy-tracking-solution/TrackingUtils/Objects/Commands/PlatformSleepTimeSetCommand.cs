﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingUtils.Objects.Commands
{
    public class PlatformSleepTimeSetCommand : Command
    {
        public PlatformSleepTimeSetCommand(string manufacturer, string deviceId, int contentLength, string commandId, string commandData) : base(manufacturer, deviceId, contentLength, commandId, commandData)
        {
        }
    }
}
