using System;
using System.Web;
using System.Web.SessionState;

namespace MiniMVC {
    public class HttpAsyncHandlerWithSession : HttpAsyncHandler, IRequiresSessionState {
        public HttpAsyncHandlerWithSession(Func<HttpContextBase, AsyncCallback, IAsyncResult> begin, Action<IAsyncResult> end) : base(begin, end) {}
    }
}