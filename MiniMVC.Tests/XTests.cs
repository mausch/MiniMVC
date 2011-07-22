using System;
using NUnit.Framework;

namespace MiniMVC.Tests {
    [TestFixture]
    public class XTests {
        [Test]
        public void FixEmptyScripts() {
            var script = X.E("script", X.A("src", "http://ajax.googleapis.com/ajax/libs/jquery/1.4.3/jquery.min.js"));
            var n = X.FixEmptyScripts(script);
            Console.WriteLine(n.ToString());
            StringAssert.DoesNotContain("/>", n.ToString());
            StringAssert.Contains("/>", script.ToString());
        }
    }
}