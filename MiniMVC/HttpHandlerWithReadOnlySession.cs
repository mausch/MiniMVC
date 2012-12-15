using System;
using System.Web;
using System.Web.SessionState;

namespace MiniMVC {
    public class HttpHandlerWithReadOnlySession : HttpHandler, IReadOnlySessionState {
        public HttpHandlerWithReadOnlySession(Action<HttpContextBase> action) : base(action) {}
    }
}
