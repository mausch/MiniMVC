using System;
using System.Web;
using System.Web.SessionState;

namespace MiniMVC {
    public class HttpHandlerWithSession: HttpHandler, IRequiresSessionState {
        public HttpHandlerWithSession(Action<HttpContextBase> action) : base(action) {}
    }
}
