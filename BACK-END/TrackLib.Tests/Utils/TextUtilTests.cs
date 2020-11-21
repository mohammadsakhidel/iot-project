using System;
using System.Collections.Generic;
using System.Text;
using TrackLib.Utils;
using Xunit;

namespace TrackLib.Tests.Utils {
    public class TextUtilTests {
        
        [Theory]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("invalid base 64", false)]
        [InlineData("SGVsbG8gaG93IGFyZSB5b3U=", true)]
        public void IsBase64StringTest(string base64, bool expected) {
            
            var result = TextUtil.IsBase64String(base64);
            Assert.Equal(expected, result);

        }

    }
}
