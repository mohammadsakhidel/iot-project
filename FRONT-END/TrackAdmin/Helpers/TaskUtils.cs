using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrackAdmin.Helpers {
    public class TaskUtils {
        public static CancellationTokenSource SetTimeout(Action action, int millis) {
            var cts = new CancellationTokenSource();
            var ct = cts.Token;
            _ = Task.Run(() => {
                Thread.Sleep(millis);
                if (!ct.IsCancellationRequested)
                    action();

            }, ct);
            return cts;
        }

        public static void ClearTimeout(CancellationTokenSource cts) {
            cts.Cancel();
        }
    }
}
