/*
	Copyright c 2003 by RiskCare Ltd.  All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions
	are met:
	1. Redistributions of source code must retain the above copyright
	notice, this list of conditions and the following disclaimer.
	2. Redistributions in binary form must reproduce the above copyright
	notice, this list of conditions and the following disclaimer in the
	documentation and/or other materials provided with the distribution.

	THIS SOFTWARE IS PROVIDED BY THE AUTHOR AND CONTRIBUTORS ``AS IS'' AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
	IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
	ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE
	FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
	DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
	OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
	HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
	LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
	OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
	SUCH DAMAGE.
*/


using System;
using SvgNet.SvgTypes;

namespace SvgNet.SvgElements
{
	/// <summary>
	/// Represents an SVG <c>symbol</c> element.
	/// </summary>
	public class SvgSymbolElement : SvgNet.SvgElement
	{
		public SvgSymbolElement()
		{
		}

		public override string Name{get{return "symbol";}}

		public SvgNumList ViewBox
		{
			get{return (SvgNumList)_atts["viewbox"];}
			set{_atts["viewbox"] = value;}
		}

		public string PreserveAspectRatio
		{
			get{return (string)_atts["preserveAspectRatio"];}
			set{_atts["preserveAspectRatio"] = value;}
		}
	}

	/// <summary>
	/// Represents an SVG <c>use</c> element.
	/// </summary>
	public class SvgUseElement : SvgNet.SvgStyledTransformedElement, IElementWithXRef
	{
		public SvgUseElement()
		{
		}

		public SvgUseElement(SvgXRef xref)
		{
			XRef = xref;
		}

		public SvgUseElement(string href)
		{
			Href = href;
		}

		public SvgUseElement(SvgLength x, SvgLength y, SvgXRef xref)
		{
			XRef = xref;
			X = x;
			Y = y;
		}

		public SvgUseElement(SvgLength x, SvgLength y, string href)
		{
			Href = href;
			X = x;
			Y = y;
		}

		public override string Name{get{return "use";}}

		public SvgLength Width
		{
			get{return (SvgLength)_atts["width"];}
			set{_atts["width"] = value;}
		}
		public SvgLength Height
		{
			get{return (SvgLength)_atts["height"];}
			set{_atts["height"] = value;}
		}

		public SvgLength X
		{
			get{return (SvgLength)_atts["x"];}
			set{_atts["x"] = value;}
		}
		public SvgLength Y
		{
			get{return (SvgLength)_atts["y"];}
			set{_atts["y"] = value;}
		}

		public SvgXRef XRef
		{
			get{return new SvgXRef(this);}
			set{value.WriteToElement(this);}
		}

		public string Href
		{
			get{return (string)_atts["xlink:href"];}
			set{_atts["xlink:href"] = value;}
		}

	}


	
	/// <summary>
	/// Represents an SVG <c>image</c> element.
	/// </summary>
	public class SvgImageElement : SvgNet.SvgStyledTransformedElement, IElementWithXRef
	{
		public SvgImageElement()
		{
		}

		public SvgImageElement(SvgXRef xref)
		{
			XRef = xref;
		}

		public SvgImageElement(SvgLength x, SvgLength y, SvgXRef xref)
		{
			XRef = xref;
			X = x;
			Y = y;
		}

		public SvgImageElement(string href)
		{
			Href = href;
		}

		public SvgImageElement(SvgLength x, SvgLength y, string href)
		{
			Href = href;
			X = x;
			Y = y;
		}

		public override string Name{get{return "use";}}

		public SvgLength Width
		{
			get{return (SvgLength)_atts["width"];}
			set{_atts["width"] = value;}
		}
		public SvgLength Height
		{
			get{return (SvgLength)_atts["height"];}
			set{_atts["height"] = value;}
		}

		public SvgLength X
		{
			get{return (SvgLength)_atts["x"];}
			set{_atts["x"] = value;}
		}
		public SvgLength Y
		{
			get{return (SvgLength)_atts["y"];}
			set{_atts["y"] = value;}
		}

		public string PreserveAspectRatio
		{
			get{return (string)_atts["preserveAspectRatio"];}
			set{_atts["preserveAspectRatio"] = value;}
		}

		public SvgXRef XRef
		{
			get{return new SvgXRef(this);}
			set{value.WriteToElement(this);}
		}

		public string Href
		{
			get{return (string)_atts["xlink:href"];}
			set{_atts["xlink:href"] = value;}
		}

	}
}
