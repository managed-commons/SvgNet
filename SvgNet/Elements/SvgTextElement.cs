/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Elements;
/// <summary>
/// Represents a <c>text</c> element.  The SVG text element is unusual in that it expects actual XML text nodes below
/// it, rather than consisting only of attributes and child elements  (other elements like this are title, desc, and tspan).
/// <c>SvgTextElement</c> therefore has to be serialized
/// to XML slightly differently.
/// </summary>
public class SvgTextElement : SvgBaseTextElement, IElementWithText {

    public SvgTextElement() {
    }

    public SvgTextElement(string s) {
        var tn = new TextNode(s);
        AddChild(tn);
    }

    [Obsolete("Must pass SvgLength, for x and y, which specifies a unit. Use the new constructor override")]
    public SvgTextElement(string s, float x, float y) {
        var tn = new TextNode(s);
        AddChild(tn);
        X = x;
        Y = y;
    }

    public SvgTextElement(string s, SvgLength x, SvgLength y) {
        var tn = new TextNode(s);
        AddChild(tn);
        X = x;
        Y = y;
    }

    public override string Name => "text";

    public string Text {
        get => ((TextNode)FirstChild).Text;
        set => ((TextNode)FirstChild).Text = value;
    }

    public SvgLength TextLength {
        get => (SvgLength)_atts["textLength"];
        set => _atts["textLength"] = value;
    }
}