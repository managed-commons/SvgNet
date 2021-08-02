/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2019 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml;

namespace SvgNet {
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
    public class SvgElement {
        public const string svgNamespaceURI = "http://www.w3.org/2000/svg";
        public const string xlinkNamespaceURI = "http://www.w3.org/1999/xlink";

        public SvgElement() => Id = GenerateNewId();

        public SvgElement(string id) => Id = id;

        /// <summary>
        /// A hashtable containing this element's attributes.  Keys are strings but values can be any type; they will only be
        /// reduced to strings when this element needs to convert itself to XML.
        /// </summary>
        public Hashtable Attributes => _atts;

        /// <summary>
        /// An ArrayList containing this element's children
        /// </summary>
        public ArrayList Children { get; protected set; } = new ArrayList();

        public string Id {
            get => (string)_atts["id"];
            set => _atts["id"] = value;
        }

        /// <summary>
        /// The name of the XML element that this SVG element represents.
        /// </summary>
        public virtual string Name => "?";

        public SvgElement Parent { get; protected set; }

        /// <summary>
        /// The element whose child this element is; can be null, because SvgElements may only be inserted into a full SVG tree
        /// long after they are created.
        /// </summary>
        /// <summary>
        /// A quick way to get and set attributes.
        /// </summary>
        public object this[string attname] {
            get => _atts[attname];
            set => _atts[attname] = value;
        }

        /// <summary>
        /// Adds a child, and sets the child's parent to this element.
        /// </summary>
        /// <param name="ch"></param>
        public virtual void AddChild(SvgElement ch) {
            if (ch.Parent != null) {
                throw new SvgException("Child already has a parent", ch.ToString());
            }

            Children.Add(ch);
            ch.Parent = this;
        }

        /// <summary>
        /// Adds a variable number of children
        /// </summary>
        /// <param name="ch"></param>
        public virtual SvgElement AddChildren(params SvgElement[] ch) {
            foreach (SvgElement el in ch) {
                AddChild(el);
            }
            return this;
        }

        /// <summary>
        /// Given a document and a current node, read this element from the node.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="el"></param>
        public virtual void ReadXmlElement(XmlDocument doc, XmlElement el) {
            foreach (XmlAttribute att in el.Attributes) {
                // TODO: after namespaced attributes are supported in the writer code (WriteXmlElements) re-enable
                // their reading.
                // For now we'll skip namespaced attributes
                if (att.Name == "xmlns" || att.Name.Contains(":"))
                    continue;

                this[att.Name] = att.Value;
            }
        }

        /// <summary>
        /// A simple ToString() for use in debugging.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "<" + Name + " id='" + Id + "'/>";

        /// <summary>
        /// Get a string that contains a complete SVG document.  XML version, DOCTYPE etc are included.
        /// </summary>
        /// <returns></returns>
        /// <param name="compressAttributes">Should usually be set true.  Causes the XML output to be optimized so that
        /// long attributes like styles and transformations are represented with entities.</param>
        public string WriteSVGString(bool compressAttributes) {
            var doc = new XmlDocument();

            var declaration = doc.CreateXmlDeclaration("1.0", null, "yes");
            doc.AppendChild(declaration);

            //write out our SVG tree to the new XmlDocument
            WriteXmlElements(doc, null);

            doc.DocumentElement.SetAttribute("xmlns", svgNamespaceURI);
            doc.DocumentElement.SetAttribute("xmlns:xlink", xlinkNamespaceURI);

            var ents = string.Empty;
            if (compressAttributes)
                ents = SvgFactory.CompressXML(doc, doc.DocumentElement);

            doc.XmlResolver = new DummyXmlResolver();
            doc.InsertAfter(
                doc.CreateDocumentType("svg", "-//W3C//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", ents),
                declaration
            );

            return ToXmlString(doc);
        }

        /// <summary>
        /// Given an XML document and a parent node, write out this node and its descendants as XmlElements.
        /// </summary>
        /// <param name="doc">A document</param>
        /// <param name="parent">A node, or null if this element is to be the root element</param>
        public virtual void WriteXmlElements(XmlDocument doc, XmlElement parent) {
            var me = doc.CreateElement("", Name, doc.NamespaceURI);
            foreach (string s in _atts.Keys) {
                if (_atts[s] is float singleValue) {
                    me.SetAttribute(s, doc.NamespaceURI, singleValue.ToString(CultureInfo.InvariantCulture));
                } else if (_atts[s] is double doubleValue) {
                    me.SetAttribute(s, doc.NamespaceURI, doubleValue.ToString(CultureInfo.InvariantCulture));
                } else {
                    me.SetAttribute(s, doc.NamespaceURI, _atts[s].ToString());
                }
            }

            foreach (SvgElement el in Children) {
                el.WriteXmlElements(doc, me);
            }

            if (parent == null) {
                doc.AppendChild(me);
            } else {
                parent.AppendChild(me);
            }
        }

        protected Hashtable _atts = new();
        protected object FirstChild => Children[0];

        protected T GetTypedAttribute<T>(string attributeName, Func<object, T> fromString) where T : new() {
            T SetNewAttributeValue(T st) {
                _atts[attributeName] = st;
                return st;
            }
            var o = _atts[attributeName];
            //in case the property was set as a string, make a real object and save it.
            return o == null ? SetNewAttributeValue(new T()) : (o is T t) ? t : SetNewAttributeValue(fromString(o));
        }

        private static int _idcounter;

        private static string ToXmlString(XmlDocument doc) => doc.OuterXml;

        private static string GenerateNewId() => _idcounter++.ToString();

        private class DummyXmlResolver : XmlResolver {
            public override System.Net.ICredentials Credentials { set { } }

            public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn) => new MemoryStream();
        }
    }
}
