using System;
using System.Threading.Tasks;
using System.Web;

namespace MiniMVC.TaskAdapter {
    public static class TaskAdapters {
        public static BeginEndFuncs<T> AsBeginEnd<T>(this Func<T, Task> taskf) {
            var begin = new Func<T, AsyncCallback, IAsyncResult>((x, cb) => {
                var task = taskf(x);
                if (cb != null)
                    return task.ContinueWith(t => cb(t));
                return task;
            });
            var end = new Action<IAsyncResult>(r => ((Task)r).Wait());
            return new BeginEndFuncs<T>(begin, end);
        }

        public static HttpAsyncHandler AsAsyncHandler(this Func<HttpContextBase, Task> taskf) {
            var beginEnd = taskf.AsBeginEnd();
            return new HttpAsyncHandler(beginEnd.Begin, beginEnd.End);
        }
    }
}