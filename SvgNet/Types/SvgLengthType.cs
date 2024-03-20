/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Types;
/// <summary>
/// The various units in which an SvgLength can be specified.
/// </summary>
public enum SvgLengthType {
    SVG_LENGTHTYPE_UNKNOWN = 0,
    SVG_LENGTHTYPE_NUMBER = 1,
    SVG_LENGTHTYPE_PERCENTAGE = 2,
    SVG_LENGTHTYPE_EMS = 3,
    SVG_LENGTHTYPE_EXS = 4,
    SVG_LENGTHTYPE_PX = 5,
    SVG_LENGTHTYPE_CM = 6,
    SVG_LENGTHTYPE_MM = 7,
    SVG_LENGTHTYPE_IN = 8,
    SVG_LENGTHTYPE_PT = 9,
    SVG_LENGTHTYPE_PC = 10,
}
