/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Types;
/// <summary>
/// The various different types of segment that make up an SVG path, as listed in the SVG Path grammar.
/// </summary>
public enum SvgPathSegType {
    SVG_SEGTYPE_UNKNOWN = 0,
    SVG_SEGTYPE_MOVETO,
    SVG_SEGTYPE_CLOSEPATH,
    SVG_SEGTYPE_LINETO,
    SVG_SEGTYPE_HLINETO,
    SVG_SEGTYPE_VLINETO,
    SVG_SEGTYPE_CURVETO,
    SVG_SEGTYPE_SMOOTHCURVETO,
    SVG_SEGTYPE_BEZIERTO,
    SVG_SEGTYPE_SMOOTHBEZIERTO,
    SVG_SEGTYPE_ARCTO
}
