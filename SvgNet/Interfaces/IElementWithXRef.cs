/*
	Copyright © 2003 RiskCare Ltd. All rights reserved.
	Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
	Copyright © 2015-2022 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

	Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using SvgNet.Types;

namespace SvgNet.Interfaces;
/// <summary>
/// Interface for SvgElements that xlink to another element, e.g. <c>use</c>
/// </summary>
public interface IElementWithXRef {
    string Href { get; set; }

    SvgXRef XRef { get; set; }
}
