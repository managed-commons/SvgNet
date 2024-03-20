/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Elements;
/// <summary>
/// Represents a <c>tspan</c> element.  The tspan element is unique in that it expects actual XML text nodes below
/// it, rather than consisting only of attributes and child elements.  <c>SvgTextElement</c> therefore has to be serialized
/// to XML slightly differently.
/// </summary>
public class SvgTspanElement : SvgTextElement {

    public SvgTspanElement() {
    }

    public SvgTspanElement(string s) : base(s) {
    }

    [Obsolete("Use constructor override that receives SvgLength for x and y")]
    public SvgTspanElement(string s, float x, float y) : base(s, x, y) {
    }

    public SvgTspanElement(string s, SvgLength x, SvgLength y) : base(s, x, y) {
    }

    public override string Name => "tspan";
}