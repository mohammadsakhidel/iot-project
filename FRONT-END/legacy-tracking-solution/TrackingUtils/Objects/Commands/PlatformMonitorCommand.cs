﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingUtils.Objects.Commands
{
    public class PlatformMonitorCommand : Command
    {
        public PlatformMonitorCommand(string manufacturer, string deviceId, int contentLength, string commandId, string commandData) : base(manufacturer, deviceId, contentLength, commandId, commandData)
        {
        }
    }
}
