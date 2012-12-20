using System;
using System.Web;

namespace MiniMVC {
    public class HttpHandler: IHttpHandler {
        private readonly Action<HttpContextBase> action;

        public HttpHandler(Action<HttpContextBase> action) {
            this.action = action;
        }

        public bool IsReusable {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context) {
            action(new HttpContextWrapper(context));
        }

        public static readonly IHttpHandler NotFound = new HttpHandler(ctx => ctx.Response.NotFound());
    }
}
