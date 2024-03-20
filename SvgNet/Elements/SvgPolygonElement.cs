/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Elements;
/// <summary>
/// Represents a <c>polygon</c> element
/// </summary>
public class SvgPolygonElement : SvgStyledTransformedElement {
    public SvgPolygonElement(params PointF[] points) => Points = points;

    public SvgPolygonElement(SvgPoints points) => Points = points;

    public SvgPolygonElement() {
        // used by some reflection code
    }

    public override string Name => "polygon";

    public SvgPoints Points {
        get => (SvgPoints)_atts["points"];
        set => _atts["points"] = value;
    }
}
