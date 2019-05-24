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

        public bool Abs { get; private set; }

        public string Char {
            get {
                switch (_type) {
                    case SvgPathSegType.SVG_SEGTYPE_MOVETO: return (Abs ? "M" : "m");
                    case SvgPathSegType.SVG_SEGTYPE_CLOSEPATH: return "z";
                    case SvgPathSegType.SVG_SEGTYPE_LINETO: return (Abs ? "L" : "l");
                    case SvgPathSegType.SVG_SEGTYPE_HLINETO: return (Abs ? "H" : "h");
                    case SvgPathSegType.SVG_SEGTYPE_VLINETO: return (Abs ? "V" : "v");
                    case SvgPathSegType.SVG_SEGTYPE_CURVETO: return (Abs ? "C" : "c");
                    case SvgPathSegType.SVG_SEGTYPE_SMOOTHCURVETO: return (Abs ? "S" : "s");
                    case SvgPathSegType.SVG_SEGTYPE_BEZIERTO: return (Abs ? "Q" : "q");
                    case SvgPathSegType.SVG_SEGTYPE_SMOOTHBEZIERTO: return (Abs ? "T" : "t");
                    case SvgPathSegType.SVG_SEGTYPE_ARCTO: return (Abs ? "A" : "a");
                }

                throw new SvgException("Invalid PathSeg type", _type.ToString());
            }
        }

        public float[] Data => _data;

        public SvgPathSegType Type => _type;

        public object Clone() => new PathSeg(_type, Abs, (float[])_data.Clone());
    };
}
