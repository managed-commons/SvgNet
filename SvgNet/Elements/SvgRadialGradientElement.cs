/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Elements;
/// <summary>
/// Represents an svg radialGradient element
/// </summary>
public class SvgRadialGradientElement : SvgStyledTransformedElement {
    public SvgRadialGradientElement() {
    }

    public SvgLength CX {
        get => (SvgLength)_atts["cx"];
        set => _atts["cx"] = value;
    }

    public SvgLength CY {
        get => (SvgLength)_atts["cy"];
        set => _atts["cy"] = value;
    }

    public SvgLength FX {
        get => (SvgLength)_atts["fx"];
        set => _atts["fx"] = value;
    }

    public SvgLength FY {
        get => (SvgLength)_atts["fy"];
        set => _atts["fy"] = value;
    }

    public SvgTransformList GradientTransform {
        get => (SvgTransformList)_atts["gradientTransform"];
        set => _atts["gradientTransform"] = value;
    }

    public string GradientUnits {
        get => (string)_atts["gradientUnits"];
        set => _atts["gradientUnits"] = value;
    }

    public override string Name => "radialGradient";

    public SvgLength R {
        get => (SvgLength)_atts["r"];
        set => _atts["r"] = value;
    }

    public string SpreadMethod {
        get => (string)_atts["spreadMethod"];
        set => _atts["spreadMethod"] = value;
    }
}
