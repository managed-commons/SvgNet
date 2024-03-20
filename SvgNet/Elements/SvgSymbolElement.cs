/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Elements;
/// <summary>
/// Represents an SVG <c>image</c> element.
/// </summary>
public class SvgImageElement : SvgStyledTransformedElement, IElementWithXRef {
    public SvgImageElement() {
    }

    public SvgImageElement(SvgXRef xref) => XRef = xref;

    public SvgImageElement(SvgLength x, SvgLength y, SvgXRef xref) {
        XRef = xref;
        X = x;
        Y = y;
    }

    public SvgImageElement(string href) => Href = href;

    public SvgImageElement(SvgLength x, SvgLength y, string href) {
        Href = href;
        X = x;
        Y = y;
    }

    public SvgLength Height {
        get => (SvgLength)_atts["height"];
        set => _atts["height"] = value;
    }

    public string Href {
        get => (string)_atts["xlink:href"];
        set => _atts["xlink:href"] = value;
    }

    public override string Name => "image";

    public string PreserveAspectRatio {
        get => (string)_atts["preserveAspectRatio"];
        set => _atts["preserveAspectRatio"] = value;
    }

    public SvgLength Width {
        get => (SvgLength)_atts["width"];
        set => _atts["width"] = value;
    }

    public SvgLength X {
        get => (SvgLength)_atts["x"];
        set => _atts["x"] = value;
    }

    public SvgXRef XRef {
        get => new(this);
        set => value.WriteToElement(this);
    }

    public SvgLength Y {
        get => (SvgLength)_atts["y"];
        set => _atts["y"] = value;
    }
}

/// <summary>
/// Represents an SVG <c>symbol</c> element.
/// </summary>
public class SvgSymbolElement : SvgElement {
    public SvgSymbolElement() {
    }

    public override string Name => "symbol";

    public string PreserveAspectRatio {
        get => (string)_atts["preserveAspectRatio"];
        set => _atts["preserveAspectRatio"] = value;
    }

    public SvgNumList ViewBox {
        get => (SvgNumList)_atts["viewBox"];
        set => _atts["viewBox"] = value;
    }
}

/// <summary>
/// Represents an SVG <c>use</c> element.
/// </summary>
public class SvgUseElement : SvgStyledTransformedElement, IElementWithXRef {
    public SvgUseElement() {
    }

    public SvgUseElement(SvgXRef xref) => XRef = xref;

    public SvgUseElement(string href) => Href = href;

    public SvgUseElement(SvgLength x, SvgLength y, SvgXRef xref) {
        XRef = xref;
        X = x;
        Y = y;
    }

    public SvgUseElement(SvgLength x, SvgLength y, string href) {
        Href = href;
        X = x;
        Y = y;
    }

    public SvgLength Height {
        get => (SvgLength)_atts["height"];
        set => _atts["height"] = value;
    }

    public string Href {
        get => (string)_atts["xlink:href"];
        set => _atts["xlink:href"] = value;
    }

    public override string Name => "use";

    public SvgLength Width {
        get => (SvgLength)_atts["width"];
        set => _atts["width"] = value;
    }

    public SvgLength X {
        get => (SvgLength)_atts["x"];
        set => _atts["x"] = value;
    }

    public SvgXRef XRef {
        get => new(this);
        set => value.WriteToElement(this);
    }

    public SvgLength Y {
        get => (SvgLength)_atts["y"];
        set => _atts["y"] = value;
    }
}
