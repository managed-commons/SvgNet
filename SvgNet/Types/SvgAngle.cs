/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2019 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System;
using System.Globalization;

namespace SvgNet.SvgTypes {

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

        public static implicit operator SvgAngle(string s) {
            return new SvgAngle(s);
        }

        public object Clone() => new SvgAngle(Value, Type);

        public void FromString(string s) {
            var i = s.LastIndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
            if (i == -1)
                return;

            Value = int.Parse(s.Substring(0, i + 1), CultureInfo.InvariantCulture);

            Type = (s.Substring(i + 1)) switch
            {
                "grad" => SvgAngleType.SVG_ANGLETYPE_GRAD,
                "rad" => SvgAngleType.SVG_ANGLETYPE_RAD,
                "deg" => SvgAngleType.SVG_ANGLETYPE_DEG,
                "" => SvgAngleType.SVG_ANGLETYPE_UNSPECIFIED,

                _ => throw new SvgException("Invalid SvgAngle", s),
            };
        }

        public override string ToString() {
            var s = Value.ToString("F", CultureInfo.InvariantCulture);
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
}
