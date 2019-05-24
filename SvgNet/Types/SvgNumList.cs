/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2019 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System;
using System.Collections;
using System.Globalization;

namespace SvgNet.SvgTypes {

    /// <summary>
    /// A number list, as used in the SVG spec for e.g. the value of a viewBox attribute.  Basically a list of numbers in
    /// any format separated by whitespace and commas.
    /// </summary>
    public class SvgNumList : ICloneable {

        public SvgNumList(string s) => FromString(s);

        public SvgNumList(float[] pts) {
            foreach (float p in pts) {
                _pts.Add(p);
            }
        }

        public int Count => _pts.Count;

        public float this[int idx] {
            get => (float)_pts[idx];
            set => _pts[idx] = value;
        }

        public static implicit operator SvgNumList(string s) {
            return new SvgNumList(s);
        }

        public static implicit operator SvgNumList(float[] f) {
            return new SvgNumList(f);
        }

        public static float[] String2Floats(string s) {
            try {
                var sa = s.Split(new char[] { ',', ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var arr = new ArrayList();
                foreach (string str in sa) {
                    if (!string.IsNullOrWhiteSpace(str)) {
                        arr.Add(float.Parse(str.Trim(), CultureInfo.InvariantCulture));
                    }
                }
                return (float[])arr.ToArray(typeof(float));
            } catch (Exception) {
                throw new SvgException("Invalid number list", s);
            }
        }

        public object Clone() => new SvgNumList((float[])_pts.ToArray(typeof(float)));

        public void FromString(string s) {
            try {
                var fa = String2Floats(s);

                foreach (float f in fa) {
                    _pts.Add(f);
                }
            } catch (Exception) {
                throw new SvgException("Invalid SvgNumList", s);
            }
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
