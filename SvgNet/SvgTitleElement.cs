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
using System.Xml;

namespace SvgNet.SvgElements
{
	/// <summary>
	/// Represents an SVG <c>desc</c> element.  As with the SvgTextElement, the payload is in the enclosed text rather than in attributes and 
	/// subelements, so we need to specially add text when serializing.
	/// </summary>
	public class SvgTitleElement : SvgNet.SvgElement, IElementWithText
	{

		public SvgTitleElement()
		{
			TextNode tn = new TextNode("");
			AddChild(tn);
		}

		public SvgTitleElement(string s)
		{
			TextNode tn = new TextNode(s);
			AddChild(tn);
		}

		public override string Name{get{return "title";}}

		public string Text
		{
			get{return ((TextNode)_children[0]).Text;}
			set{((TextNode)_children[0]).Text = value;}
		}
	}

	/// <summary>
	/// Represents an SVG <c>desc</c> element.  As with the SvgTextElement, the payload is in the enclosed text rather than in attributes and 
	/// subelements, so we need to specially add text when serializing.
	/// </summary>
	public class SvgDescElement : SvgNet.SvgElement, IElementWithText
	{

		public SvgDescElement()
		{
			TextNode tn = new TextNode("");
			AddChild(tn);
		}

		public SvgDescElement(string s)
		{
			TextNode tn = new TextNode(s);
			AddChild(tn);
		}

		public override string Name{get{return "desc";}}

		public string Text
		{
			get{return ((TextNode)_children[0]).Text;}
			set{((TextNode)_children[0]).Text = value;}
		}

	}
}
