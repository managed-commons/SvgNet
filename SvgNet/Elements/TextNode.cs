/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System.Xml;

namespace SvgNet.Elements;
/// <summary>
/// Represents the text contained in a title, desc, text, or tspan element.  Maps to an XmlText object in an XML document.  It is inherited from
/// </summary>
public class TextNode : SvgElement {
    public TextNode() {
    }

    public TextNode(string s) => Text = s;

    public override string Name => "a text node, not an svg element";

    public string Text { get; set; }

    /// <summary>
    /// Adds a child, and sets the child's parent to this element.
    /// </summary>
    /// <param name="ch"></param>
    public override void AddChild(SvgElement ch) => throw new SvgException("A TextNode cannot have children");

    /// <summary>
    /// Adds a variable number of children
    /// </summary>
    /// <param name="ch"></param>
    public override SvgElement AddChildren(params SvgElement[] ch) => throw new SvgException("A TextNode cannot have children");

    /// <summary>
    /// Given a document and a current node, read this element from the node.
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="el"></param>
    public override void ReadXmlElement(XmlDocument doc, XmlElement el) => throw new SvgException("TextNode::ReadXmlElement should not be called; " +
            "the value should be filled in with a string when the XML doc is being read.", "");

    /// <summary>
    /// Overridden to simply create an XML text node below the parent.
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="parent"></param>
    public override void WriteXmlElements(XmlDocument doc, XmlElement parent) {
        XmlText xt = doc.CreateTextNode(Text);
        _ = parent switch {
            null => doc.AppendChild(xt),
            _ => parent.AppendChild(xt),
        };
    }
}
