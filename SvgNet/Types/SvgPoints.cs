/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2019 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System;
using System.Collections;
using System.Drawing;
using System.Globalization;

namespace SvgNet.SvgTypes {
    /// <summary>
    /// A list of points, as specified in the SVG 1.1 spec section 9.8.  Only used in polygon and polyline elements.
    /// </summary>
    public class SvgPoints : ICloneable {
        public SvgPoints(string s) => FromString(s);

        public SvgPoints(PointF[] pts) {
            foreach (PointF p in pts) {
                _pts.Add(p.X);
                _pts.Add(p.Y);
            }
        }

        /// <summary>
        /// The array must have an even length
        /// </summary>
        /// <param name="pts"></param>
        public SvgPoints(float[] pts) {
            if (pts.Length % 2 != 0)
                throw new SvgException("Invalid SvgPoints", pts.ToString());

            foreach (float p in pts) {
                _pts.Add(p);
            }
        }

        public static implicit operator SvgPoints(string s) {
            return new SvgPoints(s);
        }

        public static implicit operator SvgPoints(PointF[] pts) {
            return new SvgPoints(pts);
        }

        public object Clone() => new SvgPoints((PointF[])_pts.ToArray(typeof(PointF)));

        /// <summary>
        /// The standard boils down to a list of numbers in any format separated by any amount of wsp and commas;
        /// in other words it looks the same as a SvgNumList
        /// </summary>
        /// <param name="s"></param>
        public void FromString(string s) {
            try {
                var fa = SvgNumList.String2Floats(s);
                foreach (float f in fa) {
                    _pts.Add(f);
                }
            } catch (Exception) {
                throw new SvgException("Invalid SvgPoints", s);
            }

            if (_pts.Count % 2 != 0)
                throw new SvgException("Invalid SvgPoints", s);
        }

        public override string ToString() {
            var builder = new System.Text.StringBuilder();
            foreach (float f in _pts) {
                builder.Append(f.ToString("F", CultureInfo.InvariantCulture)).Append(" ");
            }
            return builder.ToString();
        }

        private readonly ArrayList _pts = new ArrayList();
    }
}
