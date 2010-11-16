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
	/// Represents a <c>g</c> element.  It has no particular properties of its own.
	/// </summary>
	public class SvgGroupElement : SvgNet.SvgStyledTransformedElement
	{
		public SvgGroupElement()
		{
		}

		public SvgGroupElement(string id) : base(id)
		{
		}

		public override string Name{get{return "g";}}
	}

	/// <summary>
	/// Represents a <c>switch</c> element.  It has no particular properties of its own.
	/// </summary>
	public class SvgSwitchElement : SvgNet.SvgStyledTransformedElement
	{
		public SvgSwitchElement()
		{
		}

		public SvgSwitchElement(string id) : base(id)
		{
		}

		public override string Name{get{return "g";}}
	}


	/// <summary>
	/// Represents a <c>clippath</c> element.  It has no particular properties of its own.
	/// </summary>
	public class SvgClipPathElement : SvgNet.SvgElement
	{
		public SvgClipPathElement()
		{
		}

		public SvgClipPathElement(string id) : base(id)
	{
	}

		public override string Name{get{return "clipPath";}}
	}

	/// <summary>
	/// Represents an <c>a</c> element.  It has an xref and a target.
	/// </summary>
	public class SvgAElement : SvgNet.SvgStyledTransformedElement, IElementWithXRef
	{
		public SvgAElement()
		{
		}

		public SvgAElement(string href)
		{
			Href = href;
		}

		public override string Name{get{return "a";}}

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

		public string Target
		{
			get{return (string)_atts["target"];}
			set{_atts["target"] = value;}
		}
	}

	/// <summary>
	/// Represents a <c>defs</c> element.  It has no particular properties of its own.
	/// </summary>
	public class SvgDefsElement : SvgNet.SvgElement
	{
		public SvgDefsElement()
		{
		}

		public SvgDefsElement(string id) : base(id)
		{
		}

		public override string Name{get{return "defs";}}
	}

	/// <summary>
	/// Represents an element that is not yet represented by a class of its own.  
	/// </summary>
	public class SvgGenericElement : SvgNet.SvgElement
	{
		string _name;

		public SvgGenericElement()
		{
			_name = "generic svg node";
		}

		public SvgGenericElement(string name)
		{
			_name = name;
		}

		public override string Name{get{return _name;}}
	}

}
