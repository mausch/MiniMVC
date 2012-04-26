using System;
using System.Web;
using System.Web.SessionState;

namespace MiniMVC {
    public class HttpHandlerWithSession: IHttpHandler, IRequiresSessionState {
        private readonly Action<HttpContextBase> action;

        public HttpHandlerWithSession(Action<HttpContextBase> action) {
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
