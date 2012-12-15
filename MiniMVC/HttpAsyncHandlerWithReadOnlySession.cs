using System;
using System.Web;
using System.Web.SessionState;

namespace MiniMVC {
    public class HttpAsyncHandlerWithReadOnlySession : HttpAsyncHandler, IReadOnlySessionState {
        public HttpAsyncHandlerWithReadOnlySession(Func<HttpContextBase, AsyncCallback, IAsyncResult> begin, Action<IAsyncResult> end) : base(begin, end) {}
    }
}