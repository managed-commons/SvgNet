/*
	Copyright © 2003 RiskCare Ltd. All rights reserved.
	Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
	Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

	Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Elements;
/// <summary>
/// Represents an SVG stop element, which specifies one color in a gradient.
/// </summary>
public class SvgStopElement : SvgStyledTransformedElement {
    public SvgStopElement() {
    }

    public SvgStopElement(SvgLength num, SvgColor col) {
        Offset = num;

        Style.Set("stop-color", col);
    }

    public override string Name => "stop";

    public SvgLength Offset {
        get => (SvgLength)_atts["offset"];
        set => _atts["offset"] = value;
    }
}
