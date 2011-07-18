using System;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace MiniMVC {
    public class XDocResult : IResult {
        private readonly XDocument doc;
        public string ContentType { get; set; }

        public XDocResult(XDocument doc) {
            if (doc == null)
                throw new ArgumentNullException("doc");
            this.doc = doc;
        }

        public void Execute(HttpContextBase context) {
            if (ContentType != null)
                context.Response.ContentType = ContentType;
            using (var xmlwriter = XmlWriter.Create(context.Response.Output, new XmlWriterSettings {Indent = true})) {
                doc.WriteTo(xmlwriter);
                xmlwriter.Flush();
            }
        }
    }
}