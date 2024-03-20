/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Types;
/// <summary>
/// Represents a CSS2 style, as applied to an SVG element.
/// </summary>
public class SvgStyle : ICloneable {
    public SvgStyle() {
    }

    public SvgStyle(string s) => FromString(s);

    /// <summary>
    /// Creates a style from a GDI+ pen object.  Most properties of the pen are implemented, but GDI+ allows fine control over line-capping which
    /// has no equivalent in SVG.
    /// </summary>
    /// <param name="pen"></param>
    public SvgStyle(Pen pen) {
        var strokeCol = new SvgColor(((SolidBrush)pen.Brush).Color);
        Set("stroke", strokeCol);
        Set("stroke-width", pen.Width);
        Set("fill", "none");

        switch (pen.EndCap) {
            case LineCap.Round:
                Set("stroke-linecap", "round");
                break;

            case LineCap.Square:
                Set("stroke-linecap", "square");
                break;

            case LineCap.Flat:
                Set("stroke-linecap", "butt");
                break;
        }

        switch (pen.LineJoin) {
            case LineJoin.Bevel:
                Set("stroke-linejoin", "bevel");
                break;

            case LineJoin.Miter:
                Set("stroke-linejoin", "miter");
                break;

            case LineJoin.Round:
                Set("stroke-linejoin", "round");
                break;
        }

        //converting between adobe and ms miter limits is very hard because adobe have never explained what the value means.
        Set("stroke-miterlimit", (pen.MiterLimit / 2) + 4f);

        float[] dashes = null;

        switch (pen.DashStyle) {
            case DashStyle.Dash:
                dashes = [3, 1];
                break;

            case DashStyle.DashDot:
                dashes = [3, 1, 1, 1];
                break;

            case DashStyle.DashDotDot:
                dashes = [3, 1, 1, 1, 1];
                break;

            case DashStyle.Dot:
                dashes = [1, 1];
                break;

            case DashStyle.Custom:
                dashes = pen.DashPattern;
                break;
        }

        if (dashes != null) {
            //MS GDI changes dash pattern to match width of line; svg does not.
            for (int i = 0; i < dashes.Length; ++i) dashes[i] *= pen.Width;

            Set("stroke-dasharray", new SvgNumList(dashes));
        }

        Set("opacity", pen.Color.A / 255f);
    }

    /// <summary>
    /// Creates a style based on a GDI brush object.  Only works for solid brushes; pattern brushes are not yet emulated.
    /// </summary>
    /// <param name="brush"></param>
    public SvgStyle(SolidBrush brush) {
        var col = new SvgColor(brush.Color);
        Set("fill", col);
        Set("stroke", "none");
        Set("opacity", brush.Color.A / 255f);
    }

    /// <summary>
    /// Creates a style based on a GDI+ font object.  GDI+ allows many subtle specifications which have no SVG equivalent.
    /// </summary>
    /// <param name="font"></param>
    public SvgStyle(Font font) {
        Set("font-family", font.FontFamily.Name);

        if (font.Bold)
            Set("font-weight", "bolder");

        if (font.Italic)
            Set("font-style", "italic");

        if (font.Underline)
            Set("text-decoration", "underline");

        Set("font-size", font.SizeInPoints.ToString("F", CultureInfo.InvariantCulture) + "pt");
    }

    /// <summary>
    /// A basic way to enumerate the styles.
    /// </summary>
    public ICollection Keys => _styles.Keys;

    /// <summary>
    /// A quick way to get and set style elements.
    /// </summary>
    public object this[string attname] {
        get => _styles[attname];
        set => _styles[attname] = value;
    }

    public static implicit operator SvgStyle(string s) => new(s);

    /// <summary>
    /// Adds two SvgStyles together, resulting in a new object that contains all the attributes of both styles.
    /// Attributes are copied deeply, i.e. cloned if they are <c>ICloneable</c>.
    /// </summary>
    public static SvgStyle operator +(SvgStyle lhs, SvgStyle rhs) {
        var res = new SvgStyle();

        foreach (string key in lhs._styles.Keys) res[key] = lhs[key].CloneIfPossible();

        foreach (string key in rhs._styles.Keys) res[key] = rhs[key].CloneIfPossible();

        return res;
    }

    /// <summary>
    /// Creates a new style, but does not do a deep copy on the members in the style.  Thus if any of these are
    /// not strings, they meay be left referred to by more than one style or element.
    /// </summary>
    /// <returns></returns>
    public object Clone() => new SvgStyle() + this;

    /// <summary>
    /// Parses a CSS string representation as used in SVG.
    /// </summary>
    /// <param name="s"></param>
    public void FromString(string s) {
        try {
            string[] pairs = s.Split(';');

            foreach (string pair in pairs) {
                string[] kv = pair.Split(':');
                if (kv.Length == 2)
                    Set(kv[0].Trim(), kv[1].Trim());
            }
        } catch (Exception) {
            throw new SvgException("Invalid style string", s);
        }
    }

    /// <summary>
    /// Gets the value for a given key.
    /// </summary>
    public object Get(string key) => _styles[key];

    /// <summary>
    /// Sets a style.  The key must be a string but the value can be anything (e.g. SvgColor).  If and when the element that owns this style is written out
    /// to XML, <c>ToString</c> will be called on the value.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="val"></param>
    public void Set(string key, object val) {
        if (val == null || val.ToString()?.Length == 0) {
            _styles.Remove(key);
            return;
        }

        _styles[key] = val;
    }

    /// <summary>
    /// Outputs a CSS string representation as used in SVG.
    /// </summary>
    public override string ToString() {
        var result = new StringBuilder();
        foreach (string s in _styles.Keys) _ = result.Append(s).Append(':').Append(InvariantCultureToString(_styles[s])).Append(';');
        return result.ToString();
    }

    private readonly Hashtable _styles = [];

    private static string InvariantCultureToString(object styleValue)
        => styleValue is float styleAsFloat
            ? styleAsFloat.ToString(CultureInfo.InvariantCulture)
            : styleValue is double styleAsDouble ? styleAsDouble.ToString(CultureInfo.InvariantCulture) : styleValue.ToString();
}
