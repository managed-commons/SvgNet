using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using SvgNet.SvgGdi;

using NUnit.Framework;

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
    }
}
