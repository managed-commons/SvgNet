/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System.Drawing;

using NUnit.Framework;

using SvgNet.Interfaces;

namespace SvgNet;

[TestFixture]
public class Main {
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
    public void TestCases(string key) {
        TestContext.Out.WriteLine($"=== Renderer {key}");
        System.Action<IGraphics> value = TestShared.Renderers[key];
        using var ig = new SvgGraphics(Color.WhiteSmoke);
        value(ig);
        string svgBody = ig.WriteSVGString(640, 480);
        key = key.Replace("/", "."); // Arcs/Pies is not file friendly
        string dstPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, $"{TestContext.CurrentContext.Test.ID}.{key}.svg");
        System.IO.File.WriteAllText(dstPath, svgBody);
        TestContext.AddTestAttachment(dstPath, key);
    }

    [Test]
    public void TestSvgFactory_BuildElementNameDictionary() {
        System.Collections.Hashtable dict = SvgFactory.BuildElementNameDictionary();
        Assert.That(dict, Is.Not.Null);
        Assert.That(dict, Has.Count.GreaterThanOrEqualTo(25));
        Assert.That(dict.ContainsKey("svg"));
    }
}
