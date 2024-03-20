/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System.Text.RegularExpressions;

namespace SvgNet.Types;

/// <summary>
/// A color, as found in CSS2 and used in SVG.  As well as a GDI Color object, SvgColor stores
/// the string it was initialized from, so that when a color specified as 'black' is written out,
/// it will be written 'black' rather than '#000000'
/// </summary>
public partial class SvgColor : ICloneable {
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

    /// <summary>
    /// If the SvgColor was constructed from a string, use that string; otherwise use rgb() form
    /// </summary>
    /// <returns></returns>
    public override string ToString() => _original_string ?? $"rgb({Color.R},{Color.G},{Color.B})";

    private string _original_string;

    /// <summary>
    /// As well as parsing the four types of CSS color descriptor (rgb, #xxxxxx, color name, and system color name),
    /// the FromString of this type stores the original string
    /// </summary>
    /// <param name="s"></param>
    public void FromString(string s) {
        _original_string = s;
        Color = TryParseHexString(s, out Color color) || TryParseRGB(s, out color) || TryParseFromName(s, out color)
            ? color
            : throw new SvgException("Invalid SvgColor", s);

        static bool TryParseHexString(string s, out Color color) {
            if (HEX_COLOR_REGEX.Match(s).Success) {
                int r, g, b;
                if (s.Length == 4) {
                    r = s.ParseHex(1, 1);
                    g = s.ParseHex(2, 1);
                    b = s.ParseHex(3, 1);
                    r += r * 16;
                    g += g * 16;
                    b += b * 16;
                    color = Color.FromArgb(r, g, b);
                    return true;
                }
                if (s.Length == 7) {
                    r = s.ParseHex(1, 2);
                    g = s.ParseHex(3, 2);
                    b = s.ParseHex(5, 2);
                    color = Color.FromArgb(r, g, b);
                    return true;
                }
            }
            color = Color.Transparent;
            return false;
        }

        static bool TryParseRGB(string s, out Color color) {
            if (RGB_PREFIX_REGEX.Match(s).Success)
                return TryParseScaling(s, RGB_INTEGERS_REGEX, 100, out color) || TryParseScaling(s, RGB_PERCENTS_REGEX, 255, out color);
            else {
                color = Color.Transparent;
                return false;
            }

            static bool TryParseScaling(string s, Regex regex, int scale, out Color color) {
                try {
                    Match m = regex.Match(s);
                    if (m.Success) {
                        int r, g, b;
                        r = Parse(m, "r") * scale / 100;
                        g = Parse(m, "g") * scale / 100;
                        b = Parse(m, "b") * scale / 100;
                        color = Color.FromArgb(r, g, b);
                        return true;

                        static int Parse(Match m, string component) =>
                            int.Parse(m.Groups[component].Captures[0].Value, CultureInfo.InvariantCulture);
                    }
                } catch {
                }
                color = Color.Transparent;
                return false;
            }
        }

        static bool TryParseFromName(string s, out Color color) {
            try {
                color = Color.FromName(s);
                return color.A != 0;
            } catch {
                color = Color.Transparent;
                return false;
            }
        }
    }

#if NET7_0_OR_GREATER
    private static readonly Regex RGB_PREFIX_REGEX = RGB_PREFIX();
    private static readonly Regex RGB_INTEGERS_REGEX = RGB_INTEGERS();
    private static readonly Regex RGB_PERCENTS_REGEX = RGB_PERCENTS();
    private static readonly Regex HEX_COLOR_REGEX = HEX_COLOR();

    [GeneratedRegex("[rR][gG][bB]")]
    private static partial Regex RGB_PREFIX();
    [GeneratedRegex("[rgbRGB ]+\\( *(?<r>\\d+)[, ]+(?<g>\\d+)[, ]+(?<b>\\d+) *\\)")]
    private static partial Regex RGB_INTEGERS();
    [GeneratedRegex("[rgbRGB ]+\\( *(?<r>\\d+)%[, ]+(?<g>\\d+)%[, ]+(?<b>\\d+)% *\\)")]
    private static partial Regex RGB_PERCENTS();
    [GeneratedRegex("^#[0-9A-Fa-f]+$")]
    private static partial Regex HEX_COLOR();
#else
    private static readonly Regex RGB_PREFIX_REGEX = new("[rR][gG][bB]");
    private static readonly Regex RGB_INTEGERS_REGEX = new("[rgbRGB ]+\\( *(?<r>\\d+)[, ]+(?<g>\\d+)[, ]+(?<b>\\d+) *\\)");
    private static readonly Regex RGB_PERCENTS_REGEX = new("[rgbRGB ]+\\( *(?<r>\\d+)%[, ]+(?<g>\\d+)%[, ]+(?<b>\\d+)% *\\)");
    private static readonly Regex HEX_COLOR_REGEX = new("^#[0-9A-Fa-f]+$");

#endif
}
