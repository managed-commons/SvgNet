/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Elements;
/// <summary>
/// Represents an SVG <c>desc</c> element.  As with the SvgTextElement, the payload is in the enclosed text rather than in attributes and
/// subelements, so we need to specially add text when serializing.
/// </summary>
public class SvgDescElement : SvgElement, IElementWithText {
    public SvgDescElement() {
        var tn = new TextNode("");
        AddChild(tn);
    }

    public SvgDescElement(string s) {
        var tn = new TextNode(s);
        AddChild(tn);
    }

    public override string Name => "desc";

    public string Text {
        get => ((TextNode)FirstChild).Text;
        set => ((TextNode)FirstChild).Text = value;
    }
}

/// <summary>
/// Represents an SVG <c>desc</c> element.  As with the SvgTextElement, the payload is in the enclosed text rather than in attributes and
/// subelements, so we need to specially add text when serializing.
/// </summary>
public class SvgTitleElement : SvgElement, IElementWithText {
    public SvgTitleElement() {
        var tn = new TextNode("");
        AddChild(tn);
    }

    public SvgTitleElement(string s) {
        var tn = new TextNode(s);
        AddChild(tn);
    }

    public override string Name => "title";

    public string Text {
        get => ((TextNode)FirstChild).Text;
        set => ((TextNode)FirstChild).Text = value;
    }
}
