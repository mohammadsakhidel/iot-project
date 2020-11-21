using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TrackDataAccess.Models;
using TrackDataAccess.Repositories;
using TrackWorker.Models;
using TrackWorker.Processors;
using TrackWorker.Processors.Middlewares;
using Xunit;
using Xunit.Abstractions;

namespace TrackWorker.Tests.Middlewares {
    public class LinkMessageMiddlewareTests {

        private readonly ITestOutputHelper _output;
        public LinkMessageMiddlewareTests(ITestOutputHelper output) {
            _output = output;
        }

        [Theory]
        [MemberData(nameof(InvokeTestData))]
        public void InvokeTest(PipelineContext context, ExpectedResult expected) {

            // Arrang:
            var mockRepo = new Moq.Mock<ITerminalRepository>();
            var mockTerminal = new Terminal { Id = "8800000015" };
            mockRepo.Setup(repo => repo.Get(It.IsAny<string>())).Returns(() => mockTerminal);
            mockRepo.Setup(repo => repo.SaveAsync()).Callback(() => {
                _output.WriteLine("Mock SaveAsync called.");
            });

            // Act:
            var middleware = new LinkMessageMiddleware(mockRepo.Object);
            middleware.Invoke(context);

            // Assert:
            Assert.Equal(expected.MessageValid, context.MessageValid);
            Assert.Equal(expected.MessageProcessed, context.MessageProcessed);
            if (expected.MessageProcessed)
                Assert.True(mockTerminal.LastConnection > DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(10)));

        }

        public static IEnumerable<object[]> InvokeTestData() {

            // Only validate a context with an empty message:
            yield return new object[] {
                new PipelineContext {
                    OnlyValidate = true,
                    Message = new Models.Message { Base64Text = "" }
                },
                new ExpectedResult { MessageValid = false, MessageProcessed = false }
            };

            // Only validate a context with a NULL message:
            yield return new object[] {
                new PipelineContext {
                    OnlyValidate = true,
                    Message = new Models.Message { Base64Text = null }
                },
                new ExpectedResult { MessageValid = false, MessageProcessed = false }
            };

            // Only validate a context with an invalid format message:
            yield return new object[] {
                new PipelineContext {
                    OnlyValidate = true,
                    Message = new Models.Message { Base64Text = "invalid format message" }
                },
                new ExpectedResult { MessageValid = false, MessageProcessed = false }
            };

            // Only validate a context with an irrelevant message:
            yield return new object[] {
                new PipelineContext {
                    OnlyValidate = true,
                    Message = new Models.Message { Base64Text = "W1NHKjg4MDAwMDAwMTUqMDAwMipVS10=" }
                },
                new ExpectedResult { MessageValid = false, MessageProcessed = false }
            };

            // Only validate a context with a valid message:
            yield return new object[] {
                new PipelineContext {
                    OnlyValidate = true,
                    Message = new Models.Message { Base64Text = "W1NHKjg4MDAwMDAwMTUqMDAwMipMS10=" }
                },
                new ExpectedResult { MessageValid = true, MessageProcessed = false }
            };

            // An empty message:
            yield return new object[] {
                new PipelineContext {
                    OnlyValidate = false,
                    Message = new Models.Message { Base64Text = "" }
                },
                new ExpectedResult { MessageValid = false, MessageProcessed = false }
            };

            // A NULL message:
            yield return new object[] {
                new PipelineContext {
                    OnlyValidate = false,
                    Message = new Models.Message { Base64Text = null }
                },
                new ExpectedResult { MessageValid = false, MessageProcessed = false }
            };

            // Invalid format message:
            yield return new object[] {
                new PipelineContext {
                    OnlyValidate = false,
                    Message = new Models.Message { Base64Text = "invalid format message" }
                },
                new ExpectedResult { MessageValid = false, MessageProcessed = false }
            };

            // Irrelevant message:
            yield return new object[] {
                new PipelineContext {
                    OnlyValidate = false,
                    Message = new Models.Message { Base64Text = "W1NHKjg4MDAwMDAwMTUqMDAwMipVS10=" }
                },
                new ExpectedResult { MessageValid = false, MessageProcessed = false }
            };

            // Valid message:
            var mockSocket = new Mock<ISocket>();
            mockSocket.Setup(socket => socket.Send(It.IsAny<byte[]>())).Returns(1);
            mockSocket.Setup(socket => socket.GetRealSocket()).Returns(() => null);
            yield return new object[] {
                new PipelineContext {
                    OnlyValidate = false,
                    Message = new Models.Message { Socket = mockSocket.Object, Base64Text = "W1NHKjg4MDAwMDAwMTUqMDAwMipMS10=" }
                },
                new ExpectedResult { MessageValid = true, MessageProcessed = true }
            };

        }
    }

    public struct ExpectedResult {
        public bool MessageValid { get; set; }
        public bool MessageProcessed { get; set; }
    }
}
