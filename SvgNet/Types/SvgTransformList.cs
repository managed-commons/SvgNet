/*
	Copyright © 2003 RiskCare Ltd. All rights reserved.
	Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
	Copyright © 2015-2019 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

	Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System;
using System.Collections;
using System.Drawing.Drawing2D;

namespace SvgNet.SvgTypes
{

    /// <summary>
    /// Represents an SVG transform-list, as specified in section 7.6 of the SVG 1.1 standard.
    /// </summary>
    public class SvgTransformList : ICloneable
    {
        public SvgTransformList()
        {
        }

        public SvgTransformList(string s) => FromString(s);

        public SvgTransformList(Matrix m)
        {
            var tr = new SvgTransform(m);
            _t.Add(tr);
        }

        public int Count => _t.Count;

        public SvgTransform this[int idx]
        {
            get => (SvgTransform)_t[idx];
            set => _t[idx] = value;
        }

        public static implicit operator SvgTransformList(string s)
        {
            return new SvgTransformList(s);
        }

        public static implicit operator SvgTransformList(Matrix m)
        {
            return new SvgTransformList(m);
        }

        public void Add(string trans) => _t.Add(new SvgTransform(trans));

        public void Add(Matrix m) => _t.Add(new SvgTransform(m));

        public object Clone()
        {
            //use to/from string as a shortcut
            return new SvgTransformList(ToString());
        }

        /// <summary>
        /// Parse a string containing a whitespace-separated list of transformations as per the SVG
        /// standard
        /// </summary>
        public void FromString(string s)
        {
            int start = -1;

            do {
                var end = s.IndexOf(")", start + 1);

                if (end == -1) return;

                var trans = new SvgTransform(s.Substring(start + 1, end - start));

                _t.Add(trans);

                start = end;
            }
            while (true);
        }

        public override string ToString()
        {
            string result = "";

            foreach (SvgTransform tr in _t) {
                result += tr.ToString();
                result += " ";
            }

            return result;
        }

        private readonly ArrayList _t = new ArrayList();
    }
}