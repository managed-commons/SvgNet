/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using SvgNet.Elements;

namespace SvgNet.Types;
/// <summary>
/// Represents a URI reference within a style.  Local uri references are generally strings of the form
/// <c>url(#elementID)</c>.   This class should not be confused with <see cref="SvgXRef"/> which represents
/// the xlink:* properties of, for example, an <c>a</c> element.
/// </summary>
public class SvgUriReference : ICloneable {
    public SvgUriReference() {
    }

    public SvgUriReference(string href) => Href = href;

    public SvgUriReference(SvgElement target) {
        Href = "#" + target.Id;
        if (target.Id?.Length == 0) throw new SvgException("Uri Reference cannot refer to an element with no id.", target.ToString());
    }

    public string Href { get; set; }

    public object Clone() => new SvgUriReference(Href);

    public override string ToString() => "url(" + Href + ")";
}
