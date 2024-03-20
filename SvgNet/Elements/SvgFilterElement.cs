/*
	Copyright © 2003 RiskCare Ltd. All rights reserved.
	Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
	Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

	Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Elements;
public class SvgFilterElement : SvgElement {
    public SvgFilterElement() {
    }

    public SvgFilterElement(SvgLength x, SvgLength y, SvgLength w, SvgLength h) {
        X = x;
        Y = y;
        Width = w;
        Height = h;
    }

    public string FilterRes {
        get => (string)_atts["filterRes"];
        set => _atts["filterRes"] = value;
    }

    public string FilterUnits {
        get => (string)_atts["filterUnits"];
        set => _atts["filterUnits"] = value;
    }

    public SvgLength Height {
        get => (SvgLength)_atts["height"];
        set => _atts["height"] = value;
    }

    public override string Name => "filter";

    public string PrimitiveUnits {
        get => (string)_atts["primitiveUnits"];
        set => _atts["primitiveUnits"] = value;
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
