using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

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
    }
}