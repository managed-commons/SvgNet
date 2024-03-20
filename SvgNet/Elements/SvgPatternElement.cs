/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Elements;
/// <summary>
/// Represents an SVG pattern element, which defines a fill pattern by defining a viewport onto a subscene.
/// </summary>
public class SvgPatternElement : SvgStyledTransformedElement {
    public SvgPatternElement() {
    }

    public SvgPatternElement(SvgLength width, SvgLength height, SvgNumList vport) {
        Width = width;
        Height = height;
        ViewBox = vport;
    }

    public SvgPatternElement(SvgLength x, SvgLength y, SvgLength width, SvgLength height, SvgNumList vport) {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        ViewBox = vport;
    }

    public SvgLength Height {
        get => (SvgLength)_atts["height"];
        set => _atts["height"] = value;
    }

    public override string Name => "pattern";

    public string PatternContentUnits {
        get => (string)_atts["patternContentUnits"];
        set => _atts["patternContentUnits"] = value;
    }

    public SvgTransformList PatternTransform {
        get => (SvgTransformList)_atts["patternTransform"];
        set => _atts["patternTransform"] = value;
    }

    public string PatternUnits {
        get => (string)_atts["patternUnits"];
        set => _atts["patternUnits"] = value;
    }

    public string PreserveAspectRatio {
        get => (string)_atts["preserveAspectRatio"];
        set => _atts["preserveAspectRatio"] = value;
    }

    public SvgNumList ViewBox {
        get => (SvgNumList)_atts["viewBox"];
        set => _atts["viewBox"] = value;
    }

    public SvgLength Width {
        get => (SvgLength)_atts["width"];
        set => _atts["width"] = value;
    }

    public SvgLength X {
        get => (SvgLength)_atts["x"];
        set => _atts["x"] = value;
    }

    public SvgLength Y {
        get => (SvgLength)_atts["y"];
        set => _atts["y"] = value;
    }
}
