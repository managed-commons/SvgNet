/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Elements;
public abstract class SvgBaseTextElement : SvgStyledTransformedElement {
    public SvgLength DX {
        get => (SvgLength)_atts["dx"];
        set => _atts["dx"] = value;
    }

    public SvgLength DY {
        get => (SvgLength)_atts["dy"];
        set => _atts["dy"] = value;
    }

    public string LengthAdjust {
        get => (string)_atts["lengthAdjust"];
        set => _atts["lengthAdjust"] = value;
    }

    public SvgNumList Rotate {
        get => (SvgNumList)_atts["rotate"];
        set => _atts["rotate"] = value;
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