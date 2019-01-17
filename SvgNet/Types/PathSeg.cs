/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2019 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System;

namespace SvgNet.SvgTypes
{
    /// <summary>
    /// A segment in an Svg path.  This is not a real SVG type; it is not in the SVG spec.  It is provided for making paths
    /// easier to specify and parse.
    /// </summary>
    public class PathSeg : ICloneable
    {
        private bool _abs;
        public float[] _data;
        public SvgPathSegType _type;

        public PathSeg(SvgPathSegType t, bool a, float[] arr)
        {
            _type = t;
            _abs = a;
            _data = arr;
        }

        public bool Abs => _abs;

        public string Char
        {
            get {
                switch (_type) {
                    case SvgPathSegType.SVG_SEGTYPE_MOVETO: return (_abs ? "M" : "m");
                    case SvgPathSegType.SVG_SEGTYPE_CLOSEPATH: return "z";
                    case SvgPathSegType.SVG_SEGTYPE_LINETO: return (_abs ? "L" : "l");
                    case SvgPathSegType.SVG_SEGTYPE_HLINETO: return (_abs ? "H" : "h");
                    case SvgPathSegType.SVG_SEGTYPE_VLINETO: return (_abs ? "V" : "v");
                    case SvgPathSegType.SVG_SEGTYPE_CURVETO: return (_abs ? "C" : "c");
                    case SvgPathSegType.SVG_SEGTYPE_SMOOTHCURVETO: return (_abs ? "S" : "s");
                    case SvgPathSegType.SVG_SEGTYPE_BEZIERTO: return (_abs ? "Q" : "q");
                    case SvgPathSegType.SVG_SEGTYPE_SMOOTHBEZIERTO: return (_abs ? "T" : "t");
                    case SvgPathSegType.SVG_SEGTYPE_ARCTO: return (_abs ? "A" : "a");
                }

                throw new SvgException("Invalid PathSeg type", _type.ToString());
            }
        }

        public float[] Data => _data;

        public SvgPathSegType Type => _type;

        public object Clone() => new PathSeg(_type, _abs, (float[])_data.Clone());
    };
}