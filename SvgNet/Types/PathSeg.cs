/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Types;
/// <summary>
/// A segment in an Svg path.  This is not a real SVG type; it is not in the SVG spec.  It is provided for making paths
/// easier to specify and parse.
/// </summary>
public class PathSeg(SvgPathSegType type, bool abs, float[] data) : ICloneable {
    public bool Abs { get; } = abs;

    public string Char => type switch {
        SvgPathSegType.SVG_SEGTYPE_MOVETO => Abs ? "M" : "m",
        SvgPathSegType.SVG_SEGTYPE_CLOSEPATH => "z",
        SvgPathSegType.SVG_SEGTYPE_LINETO => Abs ? "L" : "l",
        SvgPathSegType.SVG_SEGTYPE_HLINETO => Abs ? "H" : "h",
        SvgPathSegType.SVG_SEGTYPE_VLINETO => Abs ? "V" : "v",
        SvgPathSegType.SVG_SEGTYPE_CURVETO => Abs ? "C" : "c",
        SvgPathSegType.SVG_SEGTYPE_SMOOTHCURVETO => Abs ? "S" : "s",
        SvgPathSegType.SVG_SEGTYPE_BEZIERTO => Abs ? "Q" : "q",
        SvgPathSegType.SVG_SEGTYPE_SMOOTHBEZIERTO => Abs ? "T" : "t",
        SvgPathSegType.SVG_SEGTYPE_ARCTO => Abs ? "A" : "a",

        _ => throw new SvgException("Invalid PathSeg type", type.ToString()),
    };

    public float[] Data => data;

    public SvgPathSegType Type => type;

    public object Clone() => new PathSeg(type, Abs, (float[])data.Clone());
};
