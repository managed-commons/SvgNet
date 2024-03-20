/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Elements;
/// <summary>
/// Represents an <c>a</c> element.  It has an xref and a target.
/// </summary>
public class SvgAElement : SvgStyledTransformedElement, IElementWithXRef {
    public SvgAElement() {
    }

    public SvgAElement(string href) => Href = href;

    public string Href {
        get => (string)_atts["xlink:href"];
        set => _atts["xlink:href"] = value;
    }

    public override string Name => "a";

    public string Target {
        get => (string)_atts["target"];
        set => _atts["target"] = value;
    }

    public SvgXRef XRef {
        get => new(this);
        set => value.WriteToElement(this);
    }
}

/// <summary>
/// Represents a <c>clippath</c> element.  It has no particular properties of its own.
/// </summary>
public class SvgClipPathElement : SvgElement {
    public SvgClipPathElement() {
    }

    public SvgClipPathElement(string id) : base(id) {
    }

    public override string Name => "clipPath";
}

/// <summary>
/// Represents a <c>defs</c> element.  It has no particular properties of its own.
/// </summary>
public class SvgDefsElement : SvgElement {
    public SvgDefsElement() {
    }

    public SvgDefsElement(string id) : base(id) {
    }

    public override string Name => "defs";
}

/// <summary>
/// Represents an element that is not yet represented by a class of its own.
/// </summary>
public class SvgGenericElement : SvgElement {
    public SvgGenericElement() => _name = "generic svg node";

    public SvgGenericElement(string name) => _name = name;

    public override string Name => _name;
    private readonly string _name;
}

/// <summary>
/// Represents a <c>g</c> element.  It has no particular properties of its own.
/// </summary>
public class SvgGroupElement : SvgStyledTransformedElement {
    public SvgGroupElement() {
    }

    public SvgGroupElement(string id) : base(id) {
    }

    public override string Name => "g";
}

/// <summary>
/// Represents a <c>switch</c> element.  It has no particular properties of its own.
/// </summary>
public class SvgSwitchElement : SvgStyledTransformedElement {
    public SvgSwitchElement() {
    }

    public SvgSwitchElement(string id) : base(id) {
    }

    public override string Name => "g";
}
