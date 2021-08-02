/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2019 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System;

namespace SvgNet.SvgTypes {
    /// <summary>
    /// A segment in an Svg path.  This is not a real SVG type; it is not in the SVG spec.  It is provided for making paths
    /// easier to specify and parse.
    /// </summary>
    public class PathSeg : ICloneable {
        public float[] _data;
        public SvgPathSegType _type;

        public PathSeg(SvgPathSegType t, bool a, float[] arr) {
            _type = t;
            Abs = a;
            _data = arr;
        }

        public bool Abs { get; }

        public string Char => _type switch {
            SvgPathSegType.SVG_SEGTYPE_MOVETO => Abs ? "M" : "m",
            SvgPathSegType.SVG_SEGTYPE_CLOSEPATH => "z",
            SvgPathSegType.SVG_SEGTYPE_LINETO => Abs ? "L" : "l",
            SvgPathSegType.SVG_SEGTYPE_HLINETO => Abs ? "H" : "h",
            SvgPathSegType.SVG_SEGTYPE_VLINETO => Abs ? "V" : "v",
            SvgPathSegType.SVG_SEGTYPE_CURVETO => Abs ? "C" : "c",
            SvgPathSegType.SVG_SEGTYPE_SMOOTHCURVETO => Abs ? "S" : "s",
            SvgPathSegType.SVG_SEGTYPE_BEZIERTO => Abs ? "Q" : "q",
            SvgPathSegType.SVG_SEGTYPE_SMOOTHBEZIERTO => Abs ? "T" : "t",
            SvgPathSegType.SVG_SEGTYPE_ARCTO => Abs ? "A" : "a",

            _ => throw new SvgException("Invalid PathSeg type", _type.ToString()),
        };

        public float[] Data => _data;

        public SvgPathSegType Type => _type;

        public object Clone() => new PathSeg(_type, Abs, (float[])_data.Clone());
    };
}
