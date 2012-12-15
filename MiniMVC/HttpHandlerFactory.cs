using System;
using System.Web;

namespace MiniMVC {
    public abstract class HttpHandlerFactory: IHttpHandlerFactory {
        public abstract IHttpHandler GetHandler(HttpContextBase context);

        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated) {
            return GetHandler(new HttpContextWrapper(context));
        }

        public void ReleaseHandler(IHttpHandler handler) {
            var d = handler as IDisposable;
            if (d != null)
                d.Dispose();
        }
    }
}
