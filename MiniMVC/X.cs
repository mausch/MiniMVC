using System.Linq;
using System.Xml.Linq;

namespace MiniMVC {
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

        public static readonly XDocumentType XHTML1_0_Transitional = new XDocumentType("html", "-//W3C//DTD XHTML 1.0 Transitional//EN", "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", null);
        public static readonly XDocumentType XHTML1_0_Strict = new XDocumentType("html", "-//W3C//DTD XHTML 1.0 Strict//EN", "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd", null);
        public static readonly XDocumentType HTML5 = new XDocumentType("html", null, null, null);

        public static readonly string nbsp = "\u00A0";
        public static readonly string raquo = "\u00BB";
        public static readonly string laquo = "\u00AB";
        public static readonly string rsaquo = "\u203A";
        public static readonly string lsaquo = "\u2039";

        /// <summary>
        /// Copyright character entity
        /// </summary>
        public static readonly string copy = "\u00A9";
    }
}