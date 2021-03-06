﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
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

        public static XNode T(string text) {
            return new XText(text);
        }

        /// <summary>
        /// Parses raw xml
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static XNode[] Raw(string xml) {
            var x = "<x>" + xml + "</x>";
            var xdoc = XDocument.Parse(x, LoadOptions.PreserveWhitespace);
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
        /// Ampersand
        /// </summary>
        public static readonly string amp = "\u0026";

        /// <summary>
        /// Less than
        /// </summary>
        public static readonly string lt = "\u003C";

        /// <summary>
        /// Greater than
        /// </summary>
        public static readonly string gt = "\u003E";

        /// <summary>
        /// XHTML namespace (http://www.w3.org/1999/xhtml)
        /// </summary>
        public static readonly XNamespace XHTML_Namespace = XNamespace.Get("http://www.w3.org/1999/xhtml");

        private static readonly HashSet<string> emptyElems = new HashSet<string> { "area", "base", "basefont", "br", "col", "command", "frame", "hr", "img", "input", "isindex", "keygen", "link", "meta", "param", "source", "track", "wbr" };

        public static bool IsEmptyElement(string elementName) {
            return emptyElems.Contains(elementName);
        }

        public static bool IsEmptyElement(this XElement element) {
            return IsEmptyElement(element.Name.LocalName);
        }

        public static XNode FixEmptyElements(this XNode n) {
            var e = n as XElement;
            if (e != null) {
                var isEmptyElem = e.IsEmptyElement();
                if (isEmptyElem && !e.IsEmpty)
                    return new XElement(e.Name, e.Attributes());
                var children = e.Nodes().Select(FixEmptyElements);
                if (!isEmptyElem && e.IsEmpty)
                    return new XElement(e.Name, e.Attributes(), new XText(""), children);
                return new XElement(e.Name, e.Attributes(), children);
            }
            return n;
        }

        public static XNode ApplyNamespace(this XNode n, XNamespace ns) {
            var e = n as XElement;
            if (e != null) {
                var name = ns + e.Name.LocalName;
                var children = e.Nodes().Select(x => x.ApplyNamespace(ns));
                return new XElement(name, e.Attributes(), children);
            }
            return n;
        }

        public static XNode MakeHTMLCompatible(this XNode n) {
            var xhtml = ApplyNamespace(n, XHTML_Namespace);
            return FixEmptyElements(xhtml);
        }

        public static XDocument MakeHTML5Doc(this XElement root) {
            return new XDocument(HTML5_Doctype, MakeHTMLCompatible(root));
        }

        public static XmlWriter CreateXmlWriter(Stream output) {
            var settings = new XmlWriterSettings {
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment,
                NewLineHandling = NewLineHandling.None,
                Encoding = new UTF8Encoding(false),
            };
            return XmlWriter.Create(output, settings);
        }

        public static void WriteToStream(this XNode n, Stream output) {
            if (n == null)
                return;
            using (var xmlwriter = CreateXmlWriter(output))
                n.FixEmptyElements().WriteTo(xmlwriter);
        }

        public static void WriteToResponse(this XNode n) {
            var ctx = HttpContext.Current;
            if (ctx == null)
                throw new Exception("No current HttpContext");
            n.WriteToStream(ctx.Response.OutputStream);
        }

        public static void WriteToStream(this IEnumerable<XNode> nodes, Stream output) {
            if (nodes == null)
                return;
            var root = X.E("x", nodes).FixEmptyElements() as XElement;
            using (var xmlwriter = CreateXmlWriter(output)) {
                foreach (var n in root.Nodes())
                    n.WriteTo(xmlwriter);
            }
        }

        public static string WriteToString(this IEnumerable<XNode> nodes) {
            if (nodes == null)
                return "";
            using (var ms = new MemoryStream()) {
                nodes.WriteToStream(ms);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public static void WriteToResponse(this IEnumerable<XNode> nodes) {
            var ctx = HttpContext.Current;
            if (ctx == null)
                throw new Exception("No current HttpContext");
            nodes.WriteToStream(ctx.Response.OutputStream);
        }

        public static void WriteToResponse(this IEnumerable<XElement> elements) {
            elements.Select(x => (XNode) x).WriteToResponse();
        }

        public static bool IsNullOrWhiteSpace(string value) {
            if (value != null) {
                for (int i = 0; i < value.Length; i++) {
                    if (!char.IsWhiteSpace(value[i])) {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IsWhiteSpace(this XNode n) {
            var t = n as XText;
            return t != null && IsNullOrWhiteSpace(t.Value);
        }

        public static IEnumerable<XNode> Trim(this IEnumerable<XNode> nodes) {
            return nodes.SkipWhile(IsWhiteSpace)
                .Reverse()
                .SkipWhile(IsWhiteSpace)
                .Reverse();
        }

        public static string SpacesToNbsp(string s) {
            if (s == null)
                return null;
            return s.Replace(' ', (char)0xa0);
        }

        public static XElement Javascript(string content) {
            var cdata = new XCData("*/" + content + "/*");
            var begin = new XText("/*");
            var end = new XText("*/");
            return E("script", A("type", "text/javascript"), begin, cdata, end);
        }

        public static XElement Javascript(XCData content) {
            return Javascript(content.Value);
        }

        public static IEnumerable<XElement> SelectOption(IEnumerable<XElement> options, string value) {
            return options.Select(e => {
                var valueAtt = e.Attribute("value");
                if (valueAtt == null)
                    return e;
                if (valueAtt.Value != value)
                    return e;
                var newOption = new XElement(e);
                newOption.Add(X.A("selected", "selected"));
                return newOption;
            });
        }

        public static XElement UnselectOption(XElement option) {
            return option.RemoveAttr("selected");
        }

        public static XElement RemoveChildNodes(this XElement element) {
            var e = new XElement(element);
            e.RemoveNodes();
            return e;
        }

        public static XElement RemoveAttr(this XElement element) {
            var e = new XElement(element);
            e.RemoveAttributes();
            return e;
        }

        public static XElement RemoveAttr(this XElement element, string attribute) {
            var e = element.RemoveAttr();
            var attr = element.Attributes().Where(a => a.Name.LocalName != attribute).ToArray();
            e.Add(attr);
            return e;
        }

        public static string AttributeValue(this XElement element, string attr) {
            var a = element.Attribute(attr);
            if (a == null)
                return null;
            return a.Value;
        }

        public static IEnumerable<string> SelectedValues(this IEnumerable<XElement> elements) {
            return elements.Where(e => e.Attribute("selected") != null).Select(x => x.AttributeValue("value"));
        }

        public static string SelectedValue(this IEnumerable<XElement> elements) {
            return elements.SelectedValues().FirstOrDefault();
        }

        public static string SelectedText(this IEnumerable<XElement> elements) {
            return elements.Where(e => e.Attribute("selected") != null).Select(x => x.Value).FirstOrDefault();
        }

        public static T Match<T>(this XNode node, Func<XCData, T> cdata, Func<XComment, T> comment, Func<XText, T> text, Func<XProcessingInstruction, T> instruction, Func<XElement, T> element) {
            var ncdata = node as XCData;
            if (ncdata != null)
                return cdata(ncdata);
            var ncomment = node as XComment;
            if (ncomment != null)
                return comment(ncomment);
            var ntext = node as XText;
            if (ntext != null)
                return text(ntext);
            var ninstruction = node as XProcessingInstruction;
            if (ninstruction != null)
                return instruction(ninstruction);
            var nelement = node as XElement;
            if (nelement != null)
                return element(nelement);
            throw new Exception("Unknown node type " + node.GetType());
        }

        public static readonly IEnumerable<XElement> NoElements = Enumerable.Empty<XElement>();
        public static readonly IEnumerable<XNode> NoNodes = Enumerable.Empty<XNode>();
    }
}