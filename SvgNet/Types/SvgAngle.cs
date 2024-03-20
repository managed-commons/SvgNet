/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Types;
/// <summary>
/// An angle, as found here and there throughout the SVG spec
/// </summary>
public class SvgAngle : ICloneable {
    public SvgAngle(string s) => FromString(s);

    public SvgAngle(float num, SvgAngleType type) {
        Value = num;
        Type = type;
    }

    public SvgAngleType Type { get; set; }

    public float Value { get; set; }

    public static implicit operator SvgAngle(string s) => new(s);

    public object Clone() => new SvgAngle(Value, Type);

    public void FromString(string s) {
        if (s.TrySplitNumberAndSuffix(out string number, out string suffix)) {
            Value = int.Parse(number, CultureInfo.InvariantCulture);

            Type = suffix switch {
                "grad" => SvgAngleType.SVG_ANGLETYPE_GRAD,
                "rad" => SvgAngleType.SVG_ANGLETYPE_RAD,
                "deg" => SvgAngleType.SVG_ANGLETYPE_DEG,
                "" => SvgAngleType.SVG_ANGLETYPE_UNSPECIFIED,

                _ => throw new SvgException("Invalid SvgAngle", s),
            };
        }
    }

    public override string ToString() {
        string s = Value.ToString("F", CultureInfo.InvariantCulture);
        switch (Type) {
            case SvgAngleType.SVG_ANGLETYPE_DEG:
            case SvgAngleType.SVG_ANGLETYPE_UNSPECIFIED:
                s += "deg";
                break;

            case SvgAngleType.SVG_ANGLETYPE_GRAD:
                s += "grad";
                break;

            case SvgAngleType.SVG_ANGLETYPE_RAD:
                s += "rad";
                break;
        }
        return s;
    }
}
