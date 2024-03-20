/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Types;
/// <summary>
/// The units in which an SvgAngle can be specified
/// </summary>
public enum SvgAngleType {
    SVG_ANGLETYPE_UNKNOWN = 0,
    SVG_ANGLETYPE_UNSPECIFIED = 1,
    SVG_ANGLETYPE_DEG = 2,
    SVG_ANGLETYPE_RAD = 3,
    SVG_ANGLETYPE_GRAD = 4,
}
