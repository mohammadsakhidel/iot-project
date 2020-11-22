using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TrackLib.Constants;
using TrackLib.Utils;
using Xunit;
using Xunit.Abstractions;

namespace TrackLib.Tests.Utils {
    public class SocketUtilTests {

        ITestOutputHelper _output;
        public SocketUtilTests(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void FindPublicIP_Should_Return_Public_IP_Or_Empty() {
            var publicIP = SocketUtil.FindPublicIPAddressAsync().Result;
            var regex = new Regex(Patterns.IP_V4);

            if (!string.IsNullOrEmpty(publicIP)) {
                _output.WriteLine($"Public IP Address: {publicIP}");
                Assert.Matches(regex, publicIP);
            } else
                Assert.Equal("", publicIP);
        }

    }
}
