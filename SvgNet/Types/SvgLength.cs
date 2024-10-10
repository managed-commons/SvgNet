/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Types;
/// <summary>
/// A length or coordinate component (in SVG 1.1 the specification says they are the same)
/// </summary>
public class SvgLength : ICloneable {
    public SvgLength(string s) => FromString(s);

    public SvgLength(float f) {
        Value = f;
        Type = SvgLengthType.SVG_LENGTHTYPE_UNKNOWN;
    }

    public SvgLength(float f, SvgLengthType type) {
        Value = f;
        Type = type;
    }

    public SvgLengthType Type { get; set; }

    public float Value { get; set; }

    public static implicit operator SvgLength(string s) => new(s);

    public static implicit operator SvgLength(float s) => new(s);

    public object Clone() => new SvgLength(Value, Type);

    public void FromString(string s) {
        if (s.TrySplitNumberAndSuffix(out string number, out string suffix)) {
            Value = float.Parse(number, CultureInfo.InvariantCulture);

            Type = suffix switch {
                "%" => SvgLengthType.SVG_LENGTHTYPE_PERCENTAGE,
                "em" => SvgLengthType.SVG_LENGTHTYPE_EMS,
                "ex" => SvgLengthType.SVG_LENGTHTYPE_EXS,
                "px" => SvgLengthType.SVG_LENGTHTYPE_PX,
                "cm" => SvgLengthType.SVG_LENGTHTYPE_CM,
                "mm" => SvgLengthType.SVG_LENGTHTYPE_MM,
                "in" => SvgLengthType.SVG_LENGTHTYPE_IN,
                "pt" => SvgLengthType.SVG_LENGTHTYPE_PT,
                "pc" => SvgLengthType.SVG_LENGTHTYPE_PC,
                "" => SvgLengthType.SVG_LENGTHTYPE_UNKNOWN,
                _ => throw new SvgException("Invalid SvgLength", s),
            };
        }
    }

    public override string ToString() {
        string s = Value.ToString("F", CultureInfo.InvariantCulture);
        switch (Type) {
            case SvgLengthType.SVG_LENGTHTYPE_PERCENTAGE:
                s += "%";
                break;

            case SvgLengthType.SVG_LENGTHTYPE_EMS:
                s += "em";
                break;

            case SvgLengthType.SVG_LENGTHTYPE_EXS:
                s += "ex";
                break;

            case SvgLengthType.SVG_LENGTHTYPE_PX:
                s += "px";
                break;

            case SvgLengthType.SVG_LENGTHTYPE_CM:
                s += "cm";
                break;

            case SvgLengthType.SVG_LENGTHTYPE_MM:
                s += "mm";
                break;

            case SvgLengthType.SVG_LENGTHTYPE_IN:
                s += "in";
                break;

            case SvgLengthType.SVG_LENGTHTYPE_PT:
                s += "pt";
                break;

            case SvgLengthType.SVG_LENGTHTYPE_PC:
                s += "pc";
                break;
        }
        return s;
    }
}
