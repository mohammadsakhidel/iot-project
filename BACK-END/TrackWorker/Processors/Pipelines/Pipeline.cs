using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWorker.Extensions;
using TrackWorker.Helpers;
using TrackWorker.Processors.Middlewares;

namespace TrackWorker.Processors.Pipelines {
    public class Pipeline : IPipeline {
        private PipelineContext _context;
        private List<Middleware> _middlewares = new List<Middleware>();

        public void UseMiddleware<TMiddleware>() {

            var middleware = (Middleware)Program.Host.Services.GetService(typeof(TMiddleware));

            if (_middlewares.Any())
                _middlewares[_middlewares.Count - 1].Next = middleware;

            _middlewares.Add(middleware);

        }

        public async Task DispatchAsync(Message message, bool onlyValidate = false) {
            try {

                _context = new PipelineContext() {
                    Message = message,
                    OnlyValidate = onlyValidate
                };

                if (!_middlewares.Any())
                    throw new InvalidOperationException("No middleware defined.");

                await Task.Run(() => {
                    _middlewares[0].Invoke(_context);
                });

            } catch (Exception ex) {
                var logger = (ILogger<Worker>)Program.Host.Services.GetService(typeof(ILogger<Worker>));
                logger.LogError(ex.LogMessage(nameof(DispatchAsync)));
            }
        }

        public PipelineContext GetContext() {
            return _context;
        }
    }
}
