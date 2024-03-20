/*
	Copyright © 2003 RiskCare Ltd. All rights reserved.
	Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
	Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

	Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Elements;
/// <summary>
/// Represents a <c>path</c> element
/// </summary>
public class SvgPathElement : SvgStyledTransformedElement {
    public SvgPathElement() {
    }

    public SvgPath D {
        get => (SvgPath)_atts["d"];
        set => _atts["d"] = value.ToString();
    }

    public override string Name => "path";

    public SvgNumber PathLength {
        get => (SvgNumber)_atts["pathlength"];
        set => _atts["pathlength"] = value;
    }
}
