using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TrackDataAccess.Models;
using TrackDataAccess.Repositories;
using TrackLib.Constants;
using TrackWorker.Helpers;
using TrackWorker.Processors.Middlewares;
using TrackWorker.Processors.Middlewares.Messages;
using TrackWorker.Processors.Pipelines;
using TrackWorker.Shared;
using Xunit;
using Xunit.Abstractions;

namespace TrackWorker.Tests.Middlewares {
    public class LinkMessageMiddlewareTests {

        private readonly ITestOutputHelper _output;
        public LinkMessageMiddlewareTests(ITestOutputHelper output) {
            _output = output;
        }

        [Theory]
        [MemberData(nameof(ValidateMessageData))]
        public void ValidateMessageTest(Helpers.TrackerMessage message, bool expected) {
            // Arrange:
            IOptions<AppSettings> options = Options.Create(new AppSettings { 
                ServerName = "ws1"
            });
            var middleware = new GpsWatchLinkMiddleware(options);

            // Act:
            var validated = middleware.IsMatch(message);

            // Assert:
            Assert.Equal(expected, validated);
        }

        [Theory]
        [MemberData(nameof(OperateOnMessageData))]
        public void OperateOnMessageTest(PipelineContext context, bool expected) {

            // Arrang:
            var mockTracker = new Tracker { Id = "SG-8800000015" };
            var mockRepo = new Moq.Mock<ITrackerRepository>();
            mockRepo.Setup(repo => repo.Get(It.IsAny<string>())).Returns(() => mockTracker);
            mockRepo.Setup(repo => repo.SaveAsync()).Callback(() => {
                _output.WriteLine("Mock SaveAsync called.");
            });
            IOptions<AppSettings> options = Options.Create(new AppSettings {
                ServerName = "ws1"
            });
            var middleware = new GpsWatchLinkMiddleware(options);

            // Act:
            var result = middleware.OperateOnMessage(context);

            // Assert:
            Assert.Equal(expected, result);
            if (expected) {
                var messageParsed = GpsWatchMessage.TryParse(context.Message.Base64Text, out var threeGElecMsg);
                Assert.True(messageParsed);
                Assert.NotNull(threeGElecMsg);
                Assert.NotEmpty(mockTracker.LastConnectedServer);
                Assert.Matches(Patterns.IP_V4, mockTracker.LastConnectedServer);
                Assert.True(mockTracker.LastConnection.HasValue);
                Assert.True(mockTracker.LastConnection.Value > DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(20)));
                Assert.True(TrackerConnections.Exists(threeGElecMsg.UniqueID));
            }

        }

        #region DATA MEMBERS:
        public static IEnumerable<object[]> ValidateMessageData() {

            // Empty Message Text
            yield return new object[] {
                new Helpers.TrackerMessage { Base64Text = "" }, false
            };

            // NULL Message Text
            yield return new object[] {
                new Helpers.TrackerMessage { Base64Text = null }, false
            };

            // Invalid base64 text:
            yield return new object[] {
                new Helpers.TrackerMessage { Base64Text = "this is not Base64" }, false
            };

            // Base64 but invalid message:
            yield return new object[] {
                new Helpers.TrackerMessage { Base64Text = "SGVsbG8gSG93IEFyZSBZb3U/" }, false
            };

            // Base64 valid but irrelevant middleware message:
            yield return new object[] {
                new Helpers.TrackerMessage { Base64Text = "W1NHKjg4MDAwMDAwMTUqMDAwMipVS10=" }, false
            };

            // Valid Message
            yield return new object[] {
                new Helpers.TrackerMessage { Base64Text = "W1NHKjg4MDAwMDAwMTUqMDAwMipMS10=" }, true
            };

        }
        public static IEnumerable<object[]> OperateOnMessageData() {
            // Invalid Tracker ID:
            yield return new object[] {
                new PipelineContext { Message = new Helpers.TrackerMessage { Base64Text = "W1NHKjg4MDAwMDAwKjAwMDIqTEtd" } },
                false
            };
            // Valid Tracker ID:
            var mockSocket = new Mock<ISocketWrapper>();
            mockSocket.Setup(socket => socket.Send(It.IsAny<byte[]>())).Returns(0);
            mockSocket.Setup(socket => socket.GetRealSocket()).Returns(() => null);
            yield return new object[] {
                new PipelineContext { Message = new Helpers.TrackerMessage { Base64Text = "W1NHKjg4MDAwMDAwMTUqMDAwMipMS10=", Socket = mockSocket.Object } },
                true
            };
        }
        #endregion
    }
}
