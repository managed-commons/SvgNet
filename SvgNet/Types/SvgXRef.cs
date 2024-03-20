/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using SvgNet.Elements;

namespace SvgNet.Types;
/// <summary>
/// Represents a URI reference.  Unlike most svg types, uri references are represented by more than one attribute
/// of an element.  This means special measures are required to get and set uri references.
/// </summary>
public class SvgXRef : ICloneable {
    public SvgXRef() {
    }

    public SvgXRef(string href) => Href = href;

    public SvgXRef(SvgStyledTransformedElement el) => ReadFromElement(el);

    public string Actuate { get; set; } = "onLoad";

    public string Arcrole { get; set; }

    public string Href { get; set; }

    public string Role { get; set; }

    public string Show { get; set; }

    public string Title { get; set; }

    public string Type { get; set; } = "simple";

    public object Clone() {
        var r = new SvgXRef {
            Href = Href,
            Type = Type,
            Role = Role,
            Arcrole = Arcrole,
            Title = Title,
            Show = Show,
            Actuate = Actuate
        };
        return r;
    }

    public void ReadFromElement(SvgStyledTransformedElement el) {
        Href = (string)el["xlink:href"];
        Role = (string)el["xlink:role"];
        Arcrole = (string)el["xlink:arcrole"];
        Title = (string)el["xlink:title"];
        Show = (string)el["xlink:show"];

        //ignore the possibility of setting type and actuate for now
    }

    public override string ToString() => Href;

    public void WriteToElement(SvgStyledTransformedElement el) {
        el["xlink:href"] = Href;
        //if (_type != "simple") el["xlink:type"] = _type;
        el["xlink:role"] = Role;
        el["xlink:arcrole"] = Arcrole;
        el["xlink:title"] = Title;
        el["xlink:show"] = Show;
        //if (_type != "onLoad") el["xlink:actuate"] = _actuate;
    }
}
