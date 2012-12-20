#region license
// Copyright (c) 2009 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Web;
using System.Xml.Linq;
using System.Xml;

namespace MiniMVC {
    public static class Response {
        public static void XDocument(this HttpResponseBase response, XDocument doc, string contentType) {
            if (response == null)
                throw new ArgumentNullException("response");
            if (doc == null)
                throw new ArgumentNullException("doc");
            if (contentType != null)
                response.ContentType = contentType;
            using (var xmlwriter = XmlWriter.Create(response.Output, new XmlWriterSettings { Indent = true })) {
                doc.WriteTo(xmlwriter);
                xmlwriter.Flush();
            }
        }

        public static void XElement(this HttpResponseBase response, XElement e, string contentType) {
            if (response == null)
                throw new ArgumentNullException("response");
            if (e == null)
                throw new ArgumentNullException("e");
            response.XDocument(new XDocument(e), contentType);
        }

        private const string textHtml = "text/html";

        public static void Html(this HttpResponseBase response, XDocument doc) {
            response.XDocument(doc, textHtml);
        }

        public static void Html(this HttpResponseBase response, XElement elem) {
            response.Html(elem.MakeHTML5Doc());
        }

        public static void Html(this HttpResponseBase response, string html) {
            response.ContentType = textHtml;
            response.Write(html);
        }

        public static void Empty(this HttpResponseBase context) {}

        public static void Raw(this HttpResponseBase response, object obj, string contentType) {
            if (response == null)
                throw new ArgumentNullException("response");
            if (obj == null)
                return;
            if (contentType != null)
                response.ContentType = contentType;
            if (obj is byte[]) {
                var bytes = obj as byte[];
                response.OutputStream.Write(bytes, 0, bytes.Length);
            } else {
                response.Write(obj);
            }
        }

        public static void NotFound(this HttpResponseBase response) {
            response.StatusCode = 404;
        }
    }
}
