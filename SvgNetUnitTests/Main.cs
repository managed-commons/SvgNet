/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2019 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using NUnit.Framework;
using SvgNet.SvgGdi;
using System.Drawing;

namespace SvgNet
{
    [TestFixture]
    public class Main
    {
        [TestCase("Clipping")]
        [TestCase("Transforms")]
        [TestCase("Lines")]
        [TestCase("Curves")]
        [TestCase("Transparency")]
        [TestCase("Fills")]
        [TestCase("Arcs/Pies")]
        [TestCase("Text")]
        [TestCase("Path")]
        [TestCase("Path with Polygon")]
        [TestCase("Path (Slow)")]
        public void TestCases(string key)
        {
            TestContext.WriteLine($"=== Renderer {key}");
            var value = TestShared.Renderers[key];
            var ig = new SvgGraphics(Color.WhiteSmoke);
            value(ig);
            var svgBody = ig.WriteSVGString();
            key = key.Replace("/", "."); // Arcs/Pies is not file friendly
            var dstPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, $"{TestContext.CurrentContext.Test.ID}.{key}.svg");
            System.IO.File.WriteAllText(dstPath, svgBody);
            TestContext.AddTestAttachment(dstPath, key);
        }

        [Test]
        public void TestSvgFactory_BuildElementNameDictionary()
        {
            var dict = SvgFactory.BuildElementNameDictionary();
            Assert.NotNull(dict);
            Assert.That(dict, Has.Count.GreaterThanOrEqualTo(25));
            Assert.IsTrue(dict.ContainsKey("svg"));
        }
    }
}
