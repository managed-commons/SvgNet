/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Elements;
/// <summary>
/// Represents a <c>tref</c> element.
/// </summary>
[Obsolete("Don't use it anymore")]
public class SvgTrefElement : SvgBaseTextElement, IElementWithXRef {

    public SvgTrefElement() {
    }

    public SvgTrefElement(string href) => Href = href;

    public SvgTrefElement(string href, float x, float y) {
        Href = href;
        X = x;
        Y = y;
    }

    public string Href {
        get => (string)_atts["xlink:href"];
        set => _atts["xlink:href"] = value;
    }

    public override string Name => "tref";

    public SvgXRef XRef {
        get => new(this);
        set => value.WriteToElement(this);
    }
}