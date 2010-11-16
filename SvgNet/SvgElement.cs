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
using System.Reflection;
using SvgNet.SvgElements;

namespace SvgNet
{
	/// <summary>
	/// The base class for SVG elements.  It represents some part of an SVG document, either an element (rect, circle etc) or a text item.  Duties include:
	/// <list type="bulleted">
	/// <item>
	/// <description>
	/// Maintains a list of child elements and a list of attributes.
	/// </description>
	/// </item>
	/// <item>
	/// <description>
	/// Writes itself and its children to an Xml document.
	/// </description>
	/// </item>
	/// <item>
	/// <description>
	/// Reads itself and its children from an Xml document.
	/// </description>
	/// </item>
	/// </list>
	/// </summary>
	public class SvgElement
	{
		protected ArrayList _children;
		protected Hashtable _atts;
		protected SvgElement _parent;

		protected static int _idcounter=0;

		public SvgElement()
		{
			Defaults();
		}

		public SvgElement(string id)
		{
			Defaults();
			Id = id;
		}

		protected void Defaults()
		{
			_children = new ArrayList();
			_atts = new Hashtable();
			Id = _idcounter.ToString();
			_idcounter++;
		}

		public string Id
		{
			get{return (string)_atts["id"];}
			set{_atts["id"] = (string)value;}
		}

		/// <summary>
		/// Given a document and a current node, read this element from the node.
		/// </summary>
		/// <param name="doc"></param>
		/// <param name="el"></param>
		public virtual void ReadXmlElement(XmlDocument doc, XmlElement el)
		{
			foreach (XmlAttribute att in el.Attributes)
			{
				this[att.Name] = att.Value;
			}
		}

		/// <summary>
		/// Given an XML document and a parent node, write out this node and its descendants as XmlElements.
		/// </summary>
		/// <param name="doc">A document</param>
		/// <param name="parent">A node, or null if this element is to be the root element</param>
		public virtual void WriteXmlElements(XmlDocument doc, XmlElement parent)
		{
			XmlElement me = doc.CreateElement("", Name, doc.NamespaceURI);
			foreach(string s in _atts.Keys)
			{
				me.SetAttribute(s, doc.NamespaceURI, _atts[s].ToString());
			}

			foreach(SvgElement el in _children)
			{
				el.WriteXmlElements(doc, me);
			}

			if(parent == null)
			{
				doc.AppendChild(me);
			}
			else
			{
				parent.AppendChild(me);
			}
		}

		/// <summary>
		/// A simple ToString() for use in debugging.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "<" + Name + " id='" + Id + "'/>";
		}

		/// <summary>
		/// Get a string that contains a complete SVG document.  XML version, DOCTYPE etc are included.
		/// </summary>
		/// <returns></returns>
		/// <param name="compressAttributes">Should usually be set true.  Causes the XML output to be optimized so that 
		/// long attributes like styles and transformations are represented with entities.</param>
		public string WriteSVGString(bool compressAttributes)
		{
			string s;
			string ents = "";

			XmlDocument doc = new XmlDocument();

			//write out our SVG tree to the new XmlDocument
			WriteXmlElements(doc, null);

			if (compressAttributes)
				ents = SvgFactory.CompressXML(doc, doc.DocumentElement);

			//This complicated business of writing to a memory stream and then reading back out to a string
			//is necessary in order to specify UTF8 -- for some reason the default is UTF16 (which makes most renderers
			//give up)

			MemoryStream ms = new MemoryStream();
			XmlTextWriter wr = new XmlTextWriter(ms, new UTF8Encoding());

			wr.Formatting = Formatting.Indented;
			wr.WriteStartDocument(true);
			wr.WriteDocType("svg", "-//W3C//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", ents);
			doc.Save(wr);

			byte[] buf = ms.ToArray();
			s = Encoding.UTF8.GetString(buf, 0, buf.Length);

			wr.Close();

			return s;
		}


		/// <summary>
		/// Adds a child, and sets the child's parent to this element.
		/// </summary>
		/// <param name="ch"></param>
		public virtual void AddChild(SvgElement ch)
		{
			if (ch.Parent != null)
			{
				throw new SvgException("Child already has a parent", ch.ToString());
			}

			_children.Add(ch);
			ch._parent = this;
		}


		/// <summary>
		/// Adds a variable number of children
		/// </summary>
		/// <param name="ch"></param>
		public virtual void AddChildren(params SvgElement[] ch)
		{
			foreach(SvgElement el in ch)
			{
				AddChild(el);
			}
		}

		/// <summary>
		/// An ArrayList containing this element's children
		/// </summary>
		public ArrayList Children
		{
			get
			{
				return _children;
			}
		}

		/// <summary>
		/// A hashtable containing this element's attributes.  Keys are strings but values can be any type; they will only be
		/// reduced to strings when this element needs to convert itself to XML.
		/// </summary>
		public Hashtable Attributes
		{
			get
			{
				return _atts;
			}
		}

		/// <summary>
		/// The element whose child this element is; can be null, because SvgElements may only be inserted into a full SVG tree
		/// long after they are created.
		/// </summary>
		public SvgElement Parent
		{
			get
			{
				return _parent;
			}
		}

		/// <summary>
		/// A quick way to get and set attributes.
		/// </summary>
		public object this[string attname]
		{
			get
			{
				return _atts[attname];
			}
			set
			{
				_atts[attname] = value;
			}
		}

		/// <summary>
		/// The name of the XML element that this SVG element represents.
		/// </summary>
		public virtual string Name{get{return "?";}}

	}
}
