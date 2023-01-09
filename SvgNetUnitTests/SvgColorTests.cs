/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2023 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using NUnit.Framework;

using SvgNet.Exceptions;
using SvgNet.Types;

namespace SvgNet;

[TestFixture]
public class SvgColorTests {

    [TestCase("black", 255, 0, 0, 0)]
    [TestCase("Black", 255, 0, 0, 0)]
    [TestCase("BLACK", 255, 0, 0, 0)]
    [TestCase("white", 255, 255, 255, 255)]
    [TestCase("red", 255, 255, 0, 0)]
    [TestCase("#F00", 255, 255, 0, 0)]
    [TestCase("#C0C0C0", 255, 192, 192, 192)]
    [TestCase("#c0c0c0", 255, 192, 192, 192)]
    [TestCase("rgb(192,192,192)", 255, 192, 192, 192)]
    [TestCase("RGB (192,192,192)", 255, 192, 192, 192)]
    [TestCase("RGB (75%,75%,75%)", 255, 191, 191, 191)]
    public void TestSvgColor_FromString(string colorAsString, int a, int r, int g, int b) {
        var color = new SvgColor(colorAsString);
        Assert.NotNull(color);
        Assert.AreEqual(colorAsString, color.ToString());
        Assert.AreEqual(color.Color.A, a, "alpha");
        Assert.AreEqual(color.Color.R, r, "red");
        Assert.AreEqual(color.Color.G, g, "green");
        Assert.AreEqual(color.Color.B, b, "blue");
    }

    [TestCase("blackPearl")]
    [TestCase("whitish")]
    [TestCase("")]
    [TestCase("Transparent")]
    [TestCase("#F0")]
    [TestCase("#F00A")]
    [TestCase("#C0C0C")]
    [TestCase("#C0C0C0C")]
    [TestCase("#C0C0C080")]
    [TestCase("#Z0c0c0")]
    [TestCase("rgb(1920,192,192)")]
    [TestCase("rgb(192,192,-192)")]
    [TestCase("RGB (175%,75%,75%)")]
    public void TestSvgColor_FromStringException(string colorAsString) {
        SvgException ex = Assert.Throws<SvgException>(() => new SvgColor(colorAsString));
        Assert.AreEqual("Invalid SvgColor", ex.Message);
    }
}