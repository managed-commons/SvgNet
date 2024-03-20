/*
	Copyright © 2003 RiskCare Ltd. All rights reserved.
	Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
	Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

	Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Elements;
/// <summary>
/// Represents a <c>line</c> element
/// </summary>
public class SvgLineElement : SvgStyledTransformedElement {
    public SvgLineElement() {
    }

    public SvgLineElement(SvgLength x1, SvgLength y1, SvgLength x2, SvgLength y2) {
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
    }

    public override string Name => "line";

    public SvgLength X1 {
        get => (SvgLength)_atts["x1"];
        set => _atts["x1"] = value;
    }

    public SvgLength X2 {
        get => (SvgLength)_atts["x2"];
        set => _atts["x2"] = value;
    }

    public SvgLength Y1 {
        get => (SvgLength)_atts["y1"];
        set => _atts["y1"] = value;
    }

    public SvgLength Y2 {
        get => (SvgLength)_atts["y2"];
        set => _atts["y2"] = value;
    }
}
