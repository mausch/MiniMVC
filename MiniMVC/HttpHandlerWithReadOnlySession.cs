using System;
using System.Web;
using System.Web.SessionState;

namespace MiniMVC {
    public class HttpHandlerWithReadOnlySession : IHttpHandler, IReadOnlySessionState {
        private readonly Action<HttpContextBase> action;

        public HttpHandlerWithReadOnlySession(Action<HttpContextBase> action) {
            this.action = action;
        }

        public bool IsReusable {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context) {
            action(new HttpContextWrapper(context));
        }
    }
}
