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
using System.Collections;
using System.Xml;
using System.Text;
using System.IO;
using SvgNet.SvgTypes;

namespace SvgNet
{
	/// <summary>
	/// This is an SvgElement that can have a CSS style and an SVG transformation list.  It contains special properties to make reading and setting the style
	/// and the transformation easier.  All SVG elements that actually represent visual entities or groups of entities are <c>SvgStyledTransformedElements</c>.
	/// </summary>
	public class SvgStyledTransformedElement : SvgElement
	{

		public SvgStyledTransformedElement()
		{
		}

		public SvgStyledTransformedElement(string id) : base(id)
		{
		}


		/// <summary>
		/// Given a document and a current node, read this element from the node.
		/// </summary>
		/// <param name="doc"></param>
		/// <param name="el"></param>
		public override void ReadXmlElement(XmlDocument doc, XmlElement el)
		{
			foreach (XmlAttribute att in el.Attributes)
			{
				if (att.Name == "style")
				{
					Style = new SvgStyle(att.Value);	
				}
				else if (att.Name == "transform")
				{
					Transform = new SvgTransformList(att.Value);
				}
				else
				{
					this[att.Name] = att.Value;
				}
			}
		}

		/// <summary>
		/// Overridden in this class to provide special handling for the style and transform attributes,
		/// which are often long and complicated.  For instance, it may be desirable for styles to be written as entities or as separate
		/// attributes.
		/// </summary>
		/// <param name="doc"></param>
		/// <param name="parent"></param>
		public override void WriteXmlElements(XmlDocument doc, XmlElement parent)
		{
			XmlElement me = doc.CreateElement("", Name, doc.NamespaceURI);
			foreach(string s in _atts.Keys)
			{
				
				if (s == "style")
				{
					WriteStyle(doc, me, _atts[s]);
				}
				else if (s == "transform")
				{
					WriteTransform(doc, me, _atts[s]);
				}
				else
				{
					me.SetAttribute(s, doc.NamespaceURI, _atts[s].ToString());
				}
			}

			foreach(SvgElement el in _children)
			{
				el.WriteXmlElements(doc, me);
			}

			if(parent == null)
				doc.AppendChild(me);
			else
				parent.AppendChild(me);
		}

		/// <summary>
		/// Provides an easy way to get the attribute called "style" as an <c>SvgStyle</c> object.  If no such attribute has been set, one is created when
		/// this property is read.
		/// </summary>
		public SvgStyle Style
		{
			get
			{
				object o = _atts["style"];
				SvgStyle st = null;

				if (o == null)
				{
					st = new SvgStyle();
					_atts["style"] = st;
					return st;
				}

				if (o.GetType() == typeof(SvgStyle))
					return (SvgStyle)o;
				else
				{
					//in case the property was set as a string, make a real object and save it.
					st = new SvgStyle(o.ToString());
					_atts["style"] = st;
					return st;
				}
					
			}

			set{_atts["style"] = value;}
		}

		/// <summary>
		/// Provides an easy way to get the attribute called "transform" as an <c>SvgTransformList</c> object.  If no such attribute has been set, one is created when
		/// this property is read.
		/// </summary>
		public SvgTransformList Transform
		{
			get
			{
				object o = _atts["transform"];
				SvgTransformList tl = null;
				
				if (o==null)
				{
					tl = new SvgTransformList();
					_atts["transform"] = tl;
					return tl;
				}
				
				if (o.GetType() == typeof(SvgTransformList))
					return (SvgTransformList)o;
				else
				{
					//in case the property was set as a string, make a real object and save it.
					tl = new SvgTransformList(o.ToString());
					_atts["transform"] = tl;
					return tl;
				}
	
			}

			set{_atts["transform"] = value;}
		}




		private void WriteStyle(XmlDocument doc, XmlElement me, object o)
		{
			if (o.GetType() != typeof(SvgStyle))
			{
				me.SetAttribute("style", doc.NamespaceURI, o.ToString());
				return;
			}

			SvgStyle style = (SvgStyle)o;

			/*
			foreach(string s in style.Keys)
			{
				me.SetAttribute(s, doc.NamespaceURI, style.Get(s).ToString());
			}
			*/

			me.SetAttribute("style", doc.NamespaceURI, style.ToString());

			doc.CreateEntityReference("pingu");
		}

		private void WriteTransform(XmlDocument doc, XmlElement me, object o)
		{
			//if (o.GetType() != typeof(SvgTransformList))
			//{
				me.SetAttribute("transform", doc.NamespaceURI, o.ToString());
				return;
			//}

		}
	}
}
