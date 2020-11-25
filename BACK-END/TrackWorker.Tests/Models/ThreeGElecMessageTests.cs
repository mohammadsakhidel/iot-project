using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Helpers;
using Xunit;

namespace TrackWorker.Tests.Models {
    public class ThreeGElecMessageTests {

        [Theory]
        [MemberData(nameof(TryParseTestData))]
        public void TryParseMethodTest(string base64Message, (bool parseResult, string manu, string tId, string cLen, string content, string[] cItems) expected) {
            // Arrange:

            // Act: 
            var result = ThreeGElecMessage.TryParse(base64Message, out var message);

            // Assert:
            Assert.Equal(expected.parseResult, result);
            if (expected.parseResult) {
                Assert.Equal(expected.manu, message.Manufacturer);
                Assert.Equal(expected.tId, message.TrackerId);
                Assert.Equal(expected.cLen, message.ContentLengthHex);
                Assert.Equal(expected.content, message.Content);
                Assert.Equal(expected.cItems.Length, message.ContentItems.Count);
                for (int i = 0; i < message.ContentItems.Count; i++) {
                    Assert.Equal(expected.cItems[i], message.ContentItems[i]);
                }
            } else {
                Assert.Null(message);
            }
        }

        public static IEnumerable<object[]> TryParseTestData() {

            // Empty Message:
            yield return new object[] { 
                "",
                (false, "", "", "", "", new string[] { })
            };

            // Nul Message:
            yield return new object[] {
                null,
                (false, "", "", "", "", new string[] { })
            };

            // Invalid message
            yield return new object[] {
                "Invalid base 64 message",
                (false, "", "", "", "", new string[] { })
            };

            // Valid message without extra content items
            yield return new object[] {
                "W1NHKjg4MDAwMDAwMTUqMDAwMipMS10=",
                (true, "SG", "8800000015", "0002", "LK", new string[] { "LK" })
            };

            // Valid message with extra content items
            yield return new object[] {
                "W1NnKjg4MDAwMDAwMTUqMDAwMipsaywgaGVsbG8gICwgIGdvb2RieWVd",
                (true, "SG", "8800000015", "0002", "LK, HELLO  ,  GOODBYE", new string[] { "LK", "HELLO", "GOODBYE" })
            };
        }

    }
}
