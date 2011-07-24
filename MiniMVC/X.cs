using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MiniMVC {
    /// <summary>
    /// Various helper functions to deal with XML / XHTML / HTML
    /// </summary>
    public static class X {
        /// <summary>
        /// Shortcut to build a <see cref="XAttribute"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XAttribute A(string name, string value) {
            return new XAttribute(XName.Get(name), value);
        }

        /// <summary>
        /// Shortcut to build a <see cref="XElement"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static XElement E(string name, params object[] content) {
            return new XElement(XName.Get(name), content);
        }

        /// <summary>
        /// Parses raw xml
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static XNode[] Raw(string xml) {
            var x = "<x>" + xml + "</x>";
            var xdoc = XDocument.Parse(x);
            return xdoc.Document.Root.Nodes().ToArray();
        }

        public static XElement Alter(this XElement e, Func<bool> pred, Action<XElement> alter) {
            if (pred())
                alter(e);
            return e;
        }

        /// <summary>
        /// XHTML 1.0 Transitional doctype
        /// </summary>
        public static readonly XDocumentType XHTML1_0_Transitional_Doctype = new XDocumentType("html", "-//W3C//DTD XHTML 1.0 Transitional//EN", "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", null);

        /// <summary>
        /// XHTML 1.0 Strict doctype
        /// </summary>
        public static readonly XDocumentType XHTML1_0_Strict_Doctype = new XDocumentType("html", "-//W3C//DTD XHTML 1.0 Strict//EN", "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd", null);

        /// <summary>
        /// HTML 5 doctype
        /// </summary>
        public static readonly XDocumentType HTML5_Doctype = new XDocumentType("html", null, null, null);

        /// <summary>
        /// Non-breaking space
        /// </summary>
        public static readonly string nbsp = "\u00A0";

        /// <summary>
        /// Right angle quotation mark
        /// </summary>
        public static readonly string raquo = "\u00BB";

        /// <summary>
        /// Left angle quotation mark
        /// </summary>
        public static readonly string laquo = "\u00AB";

        /// <summary>
        /// Single right-pointing angle quotation mark
        /// </summary>
        public static readonly string rsaquo = "\u203A";

        /// <summary>
        /// Single left-pointing angle quotation mark
        /// </summary>
        public static readonly string lsaquo = "\u2039";

        /// <summary>
        /// Copyright character entity
        /// </summary>
        public static readonly string copy = "\u00A9";

        /// <summary>
        /// XHTML namespace (http://www.w3.org/1999/xhtml)
        /// </summary>
        public static readonly XNamespace XHTML_Namespace = XNamespace.Get("http://www.w3.org/1999/xhtml");

        private static readonly HashSet<string> emptyElems = new HashSet<string> { "area", "base", "basefont", "br", "col", "command", "frame", "hr", "img", "input", "isindex", "keygen", "link", "meta", "param", "source", "track", "wbr" };

        public static XNode FixEmptyElements(XNode n) {
            if (n is XElement) {
                var e = n as XElement;
                var isEmptyElem = emptyElems.Contains(e.Name.LocalName);
                if (isEmptyElem && !e.IsEmpty)
                    return new XElement(e.Name, e.Attributes());
                if (!isEmptyElem && e.IsEmpty)
                    return new XElement(e.Name, e.Attributes(), new XText(""), e.Nodes().Select(FixEmptyElements));
                return new XElement(e.Name, e.Attributes(), e.Nodes().Select(FixEmptyElements));
            }
            return n;
        }

        public static XNode ApplyNamespace(XNamespace ns, XNode n) {
            if (n is XElement) {
                var e = n as XElement;
                return new XElement(ns + e.Name.LocalName, e.Attributes(), e.Nodes().Select(x => ApplyNamespace(ns, x)));
            }
            return n;
        }

        public static XNode MakeHTMLCompatible(XNode n) {
            var xhtml = ApplyNamespace(XHTML_Namespace, n);
            return FixEmptyElements(xhtml);
        }

        public static XDocument MakeHTML5Doc(XElement root) {
            return new XDocument(HTML5_Doctype, MakeHTMLCompatible(root));
        }
    }
}