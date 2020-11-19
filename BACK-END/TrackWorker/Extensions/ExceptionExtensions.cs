using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrackWorker.Extensions {
    public static class ExceptionExtensions {
        public static string LogMessage(this Exception ex, string source) {

            string message;
            if (ex is AggregateException agg)
                message = agg.InnerExceptions.FirstOrDefault()?.Message;
            else
                message = ex.Message;

            return $"{ex.GetType().Name} error occurred in {source}. " +
                $"Error message: [{message}]\n{ex.StackTrace}";

        }
    }
}
