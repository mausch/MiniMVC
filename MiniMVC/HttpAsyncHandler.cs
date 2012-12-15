using System;
using System.Web;

namespace MiniMVC {
    public class HttpAsyncHandler : IHttpAsyncHandler {
        private readonly Func<HttpContextBase, AsyncCallback, IAsyncResult> begin;
        private readonly Action<IAsyncResult> end;

        public HttpAsyncHandler(Func<HttpContextBase, AsyncCallback, IAsyncResult> begin, Action<IAsyncResult> end) {
            this.begin = begin;
            this.end = end;
        }

        public void ProcessRequest(HttpContext context) {
            end(begin(new HttpContextWrapper(context), null));
        }

        public bool IsReusable {
            get { return true; }
        }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData) {
            return begin(new HttpContextWrapper(context), cb);
        }

        public void EndProcessRequest(IAsyncResult result) {
            end(result);
        }
    }
}