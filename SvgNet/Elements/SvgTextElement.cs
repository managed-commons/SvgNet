/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2019 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System;
using SvgNet.SvgTypes;

namespace SvgNet.SvgElements {
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

    /// <summary>
    /// Represents a <c>text</c> element.  The SVG text element is unusual in that it expects actual XML text nodes below
    /// it, rather than consisting only of attributes and child elements  (other elements like this are title, desc, and tspan).
    /// <c>SvgTextElement</c> therefore has to be serialized
    /// to XML slightly differently.
    /// </summary>
    public class SvgTextElement : SvgBaseTextElement, IElementWithText {
        public SvgTextElement() {
        }

        public SvgTextElement(string s) {
            var tn = new TextNode(s);
            AddChild(tn);
        }

        public SvgTextElement(string s, float x, float y) {
            var tn = new TextNode(s);
            AddChild(tn);
            X = x;
            Y = y;
        }

        public override string Name => "text";

        public string Text {
            get => ((TextNode)FirstChild).Text;
            set => ((TextNode)FirstChild).Text = value;
        }

        public SvgLength TextLength {
            get => (SvgLength)_atts["textLength"];
            set => _atts["textLength"] = value;
        }
    }

    /// <summary>
    /// Represents a <c>tref</c> element.
    /// </summary>
    [Obsolete("Don't use it anymore")]
    public class SvgTrefElement : SvgBaseTextElement, IElementWithXRef {
        public SvgTrefElement() {
        }

        public SvgTrefElement(string href) => Href = href;

        public SvgTrefElement(string href, float x, float y) {
            Href = href;
            X = x;
            Y = y;
        }

        public string Href {
            get => (string)_atts["xlink:href"];
            set => _atts["xlink:href"] = value;
        }

        public override string Name => "tref";

        public SvgXRef XRef {
            get => new(this);
            set => value.WriteToElement(this);
        }
    }

    /// <summary>
    /// Represents a <c>tspan</c> element.  The tspan element is unique in that it expects actual XML text nodes below
    /// it, rather than consisting only of attributes and child elements.  <c>SvgTextElement</c> therefore has to be serialized
    /// to XML slightly differently.
    /// </summary>
    public class SvgTspanElement : SvgTextElement {
        public SvgTspanElement() {
        }

        public SvgTspanElement(string s) : base(s) {
        }

        public SvgTspanElement(string s, float x, float y) : base(s, x, y) {
        }

        public override string Name => "tspan";
    }
}
