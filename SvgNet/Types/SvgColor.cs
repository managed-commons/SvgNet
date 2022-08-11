/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2022 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System.Text.RegularExpressions;

namespace SvgNet.Types;
/// <summary>
/// A color, as found in CSS2 and used in SVG.  As well as a GDI Color object, SvgColor stores
/// the string it was initialized from, so that when a color specified as 'black' is written out,
/// it will be written 'black' rather than '#000000'
/// </summary>
public class SvgColor : ICloneable {
    public SvgColor(string s) => FromString(s);

    public SvgColor(Color c) => Color = c;

    public SvgColor(Color c, string s) {
        Color = c;
        _original_string = s;
    }

    public Color Color { get; set; }

    public static implicit operator SvgColor(Color c) => new(c);

    public static implicit operator SvgColor(string s) => new(s);

    public object Clone() => new SvgColor(Color, _original_string);

    private static readonly Regex rg = new("[rR][gG][bB]");

    /// <summary>
    /// As well as parsing the four types of CSS color descriptor (rgb, #xxxxxx, color name, and system color name),
    /// the FromString of this type stores the original string
    /// </summary>
    /// <param name="s"></param>
    public void FromString(string s) {
        _original_string = s;

        if (s.StartsWith("#", StringComparison.Ordinal)) {
            FromHexString(s);
            return;
        }

        if (rg.Match(s).Success) {
            FromRGBString(s);
            return;
        }

        Color = Color.FromName(s);

        if (Color.A == 0)
            throw new SvgException("Invalid SvgColor", s);
    }

    /// <summary>
    /// If the SvgColor was constructed from a string, use that string; otherwise use rgb() form
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
        if (_original_string != null)
            return _original_string;

        string s = "rgb(";
        s += Color.R.ToString();
        s += ",";
        s += Color.G.ToString();
        s += ",";
        s += Color.B.ToString();
        s += ")";

        return s;
    }

    private string _original_string;

    private void FromHexString(string s) {
        int r, g, b;
        if (s.Length == 4) {
            r = s.ParseHex(1, 1);
            g = s.ParseHex(2, 1);
            b = s.ParseHex(3, 1);
            r += r * 16;
            g += g * 16;
            b += b * 16;
            Color = Color.FromArgb(r, g, b);
        } else if (s.Length == 7) {
            r = s.ParseHex(1, 2);
            g = s.ParseHex(3, 2);
            b = s.ParseHex(5, 2);
            Color = Color.FromArgb(r, g, b);
        } else             throw new SvgException("Invalid SvgColor", s);
    }

    private void FromRGBString(string s) {
        int r, g, b;
        var rg = new Regex(@"[rgbRGB ]+\( *(?<r>\d+)[, ]+(?<g>\d+)[, ]+(?<b>\d+) *\)");
        Match m = rg.Match(s);
        if (m.Success) {
            r = int.Parse(m.Groups["r"].Captures[0].Value, CultureInfo.InvariantCulture);
            g = int.Parse(m.Groups["g"].Captures[0].Value, CultureInfo.InvariantCulture);
            b = int.Parse(m.Groups["b"].Captures[0].Value, CultureInfo.InvariantCulture);

            Color = Color.FromArgb(r, g, b);
            return;
        }

        rg = new Regex(@"[rgbRGB ]+\( *(?<r>\d+)%[, ]+(?<g>\d+)%[, ]+(?<b>\d+)% *\)");
        m = rg.Match(s);
        if (m.Success) {
            r = int.Parse(m.Groups["r"].Captures[0].Value, CultureInfo.InvariantCulture) * 255 / 100;
            g = int.Parse(m.Groups["g"].Captures[0].Value, CultureInfo.InvariantCulture) * 255 / 100;
            b = int.Parse(m.Groups["b"].Captures[0].Value, CultureInfo.InvariantCulture) * 255 / 100;

            Color = Color.FromArgb(r, g, b);
            return;
        }

        throw new SvgException("Invalid SvgColor", s);
    }
}
