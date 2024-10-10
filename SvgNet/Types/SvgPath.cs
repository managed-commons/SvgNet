/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Types;
/// <summary>
/// A path, composed of segments, as described in the SVG 1.1 spec section 8.3
/// </summary>
public class SvgPath : ICloneable {
    public SvgPath(string s) => FromString(s);

    public int Count => _path.Count;

    public PathSeg this[int idx] {
        get => (PathSeg)_path[idx];
        set => _path[idx] = value;
    }

    public static implicit operator SvgPath(string s) => new(s);

    public object Clone() =>
        //we resort to using to/from string rather than writing an efficient clone, for the moment.
        new SvgPath(ToString());

    /// <summary>
    /// The parsing of the path is not completely perfect yet.  You can only have one space between path elements.
    /// </summary>
    /// <param name="s"></param>
    public void FromString(string s) {
        string[] sa = s.Split(SvgHelpers.CommonSeparators);

        PathSeg ps;
        int datasize = 0;
        SvgPathSegType pt = SvgPathSegType.SVG_SEGTYPE_UNKNOWN;
        bool abs = false;
        int i = 0;
        char segTypeChar;
        _path = [];

        while (i < sa.Length) {
            if (sa[i]?.Length == 0) {
                i++;
                continue;
            }

            //check for a segment-type character

            if (char.IsLetter(sa[i][0])) {
                segTypeChar = sa[i][0];

                switch (segTypeChar) {
                    case 'M':
                    case 'm':
                        pt = SvgPathSegType.SVG_SEGTYPE_MOVETO;
                        abs = segTypeChar == 'M';
                        datasize = 2;
                        break;
                    case 'Z':
                    case 'z':
                        pt = SvgPathSegType.SVG_SEGTYPE_CLOSEPATH;
                        datasize = 0;
                        break;
                    case 'L':
                    case 'l':
                        pt = SvgPathSegType.SVG_SEGTYPE_LINETO;
                        abs = segTypeChar == 'L';
                        datasize = 2;
                        break;
                    case 'H':
                    case 'h':
                        pt = SvgPathSegType.SVG_SEGTYPE_HLINETO;
                        abs = segTypeChar == 'H';
                        datasize = 1;
                        break;
                    case 'V':
                    case 'v':
                        pt = SvgPathSegType.SVG_SEGTYPE_VLINETO;
                        abs = segTypeChar == 'V';
                        datasize = 1;
                        break;
                    case 'C':
                    case 'c':
                        pt = SvgPathSegType.SVG_SEGTYPE_CURVETO;
                        abs = segTypeChar == 'C';
                        datasize = 6;
                        break;
                    case 'S':
                    case 's':
                        pt = SvgPathSegType.SVG_SEGTYPE_SMOOTHCURVETO;
                        abs = segTypeChar == 'S';
                        datasize = 4;
                        break;
                    case 'Q':
                    case 'q':
                        pt = SvgPathSegType.SVG_SEGTYPE_BEZIERTO;
                        abs = segTypeChar == 'Q';
                        datasize = 4;
                        break;
                    case 'T':
                    case 't':
                        pt = SvgPathSegType.SVG_SEGTYPE_SMOOTHBEZIERTO;
                        abs = segTypeChar == 'T';
                        datasize = 2;
                        break;
                    case 'A':
                    case 'a':
                        pt = SvgPathSegType.SVG_SEGTYPE_ARCTO;
                        abs = segTypeChar == 'A';
                        datasize = 7;
                        break;
                    default:
                        throw new SvgException("Invalid SvgPath", s);
                }

                //strip off type character
                sa[i] = sa[i].SkipFirst();

                if (sa[i]?.Length == 0)
                    i++;
            } else if (pt == SvgPathSegType.SVG_SEGTYPE_MOVETO)                 // ensure implicit "lineto" commands are parsed according to SVG 1.1 spec section 8.3.2.
                pt = SvgPathSegType.SVG_SEGTYPE_LINETO;

            if (pt == SvgPathSegType.SVG_SEGTYPE_UNKNOWN)
                throw new SvgException("Invalid SvgPath", s);

            float[] arr = new float[datasize];

            for (int j = 0; j < datasize; ++j) arr[j] = float.Parse(sa[i + j], CultureInfo.InvariantCulture);

            ps = new PathSeg(pt, abs, arr);

            _ = _path.Add(ps);

            i += datasize;
        }
    }

    public override string ToString() {
        PathSeg prev = null;
        var builder = new StringBuilder();
        foreach (PathSeg seg in _path) {
            if (prev == null
                || (prev.Type != seg.Type && !(prev.Type == SvgPathSegType.SVG_SEGTYPE_MOVETO && seg.Type == SvgPathSegType.SVG_SEGTYPE_LINETO))
                || prev.Abs != seg.Abs) _ = builder.Append(seg.Char).Append(' ');
            foreach (float d in seg.Data) _ = builder.Append(d.ToString(CultureInfo.InvariantCulture)).Append(' ');
            prev = seg;
        }
        return builder.ToString();
    }

    private ArrayList _path;
}
