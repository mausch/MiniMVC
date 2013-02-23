using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using System.Xml.Linq;

namespace MiniMVC.Tests {
    [TestFixture]
    public class XTests {
        [Test]
        public void FixEmptyScripts() {
            var script = X.E("script", X.A("src", "http://ajax.googleapis.com/ajax/libs/jquery/1.4.3/jquery.min.js"));
            var n = script.FixEmptyElements();
            Console.WriteLine(n.ToString());
            StringAssert.DoesNotContain("/>", n.ToString());
            StringAssert.Contains("/>", script.ToString());
        }

        [Test]
        public void FixVoidElement() {
            var x = X.E("input", X.A("type", "text"), "bla");
            var n = x.FixEmptyElements();
            Console.WriteLine(n.ToString());
            StringAssert.DoesNotContain("/>", x.ToString());
            StringAssert.Contains("/>", n.ToString());
        }

        [Test]
        public void ApplyNamespace() {
            var x = X.E("html",
                        X.E("head",
                            X.E("title", "Page title")),
                        X.E("body",
                            X.E("p", X.A("class", "ptext"), "hello")));
            var xhtml = x.ApplyNamespace(X.XHTML_Namespace);
            Console.WriteLine(xhtml.ToString());
            Assert.True(Regex.Matches(xhtml.ToString(), "xmlns=\"http://www.w3.org/1999/xhtml\"").Count == 1);
        }

        [Test]
        public void XRawPreservesSpaces() {
            var n = X.Raw("  <a> hello world</a>");
            var x = X.E("span", n);
            Assert.AreEqual("<span>  <a> hello world</a></span>", x.ToString());
        }

        [Test]
        public void WriteToStream() {
            var ms = new MemoryStream();
            var n = X.E("a", X.Raw("  <img src='something'/>"));
            n.WriteToStream(ms);
            ms.Position = 0;
            var s = Encoding.UTF8.GetString(ms.ToArray());
            Assert.AreEqual("<a>  <img src=\"something\" /></a>", s);
        }

        [Test]
        public void Trim() {
            var nodes = X.Raw("  <img src='something'/>  <img/> <b/>  ");
            Assert.AreEqual(7, nodes.Length);
            var trim = nodes.Trim().ToArray();
            Assert.AreEqual(5, trim.Length);
        }

        [Test]
        public void Javascript() {
            var e = X.Javascript("function() { return a && b > 5; }");
            Assert.AreEqual("<script type=\"text/javascript\">/*<![CDATA[*/function() { return a && b > 5; }/*]]>*/</script>", e.ToString());
        }

        [Test]
        public void SelectOption_one() {
            var options = new[] {
                X.E("option", X.A("value", "1"), "text1"),
                X.E("option", X.A("value", "2"), "text2"),
            };
            var options2 = X.SelectOption(options, "1").ToList();
            Assert.IsNull(options[0].Attribute("selected"));
            Assert.IsNull(options[1].Attribute("selected"));
            Assert.AreEqual("selected", options2[0].Attribute("selected").Value);
            Assert.IsNull(options2[1].Attribute("selected"));
        }

        [Test]
        public void SelectOption_one_with_empty() {
            var options = new[] {
                X.E("option", X.A("value", "1"), "text1"),
                X.E("option", "text2"),
            };
            var options2 = X.SelectOption(options, "1").ToList();
            Assert.IsNull(options[0].Attribute("selected"));
            Assert.IsNull(options[1].Attribute("selected"));
            Assert.AreEqual("selected", options2[0].Attribute("selected").Value);
            Assert.IsNull(options2[1].Attribute("selected"));
        }

        [Test]
        public void RemoveAttribute() {
            var e = X.E("option", X.A("selected", "selected"), X.A("class", "something"));
            var ee = e.RemoveAttr("selected");
            Assert.AreEqual(2, e.Attributes().Count());
            Assert.AreEqual(1, ee.Attributes().Count());
        }

        private static XAttribute Value(string value) {
            return X.A("value", value);
        }

        [Test]
        public void SelectedValue() {
            var selected = X.A("selected", "selected");
            var options = new[] {
                X.E("option", selected, Value("1"), "uno"),
                X.E("option", Value("2"), "dos"),
                X.E("option", Value("3"), "tres"),
            };
            var value = options.SelectedValue();
            Assert.AreEqual("1", value);
        }

        [Test]
        public void SelectedText() {
            var selected = X.A("selected", "selected");
            var options = new[] {
                X.E("option", selected, Value("1"), "uno"),
                X.E("option", Value("2"), "dos"),
                X.E("option", Value("3"), "tres"),
            };
            var value = options.SelectedText();
            Assert.AreEqual("uno", value);
        }
    }
}