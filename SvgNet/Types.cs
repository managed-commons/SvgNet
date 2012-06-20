/*
	Copyright c 2003 by RiskCare Ltd.  All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions
	are met:
	1. Redistributions of source code must retain the above copyright
	notice, this list of conditions and the following disclaimer.
	2. Redistributions in binary form must reproduce the above copyright
	notice, this list of conditions and the following disclaimer in the
	documentation and/or other materials provided with the distribution.

	THIS SOFTWARE IS PROVIDED BY THE AUTHOR AND CONTRIBUTORS ``AS IS'' AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
	IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
	ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE
	FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
	DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
	OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
	HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
	LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
	OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
	SUCH DAMAGE.
*/


using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;


namespace SvgNet.SvgTypes
{
	/// <summary>
	/// The various units in which an SvgLength can be specified.
	/// </summary>
	public enum SvgLengthType
	{
		SVG_LENGTHTYPE_UNKNOWN = 0,
		SVG_LENGTHTYPE_NUMBER = 1,
		SVG_LENGTHTYPE_PERCENTAGE = 2,
		SVG_LENGTHTYPE_EMS = 3,
		SVG_LENGTHTYPE_EXS = 4,
		SVG_LENGTHTYPE_PX = 5,
		SVG_LENGTHTYPE_CM = 6,
		SVG_LENGTHTYPE_MM = 7,
		SVG_LENGTHTYPE_IN = 8,
		SVG_LENGTHTYPE_PT = 9,
		SVG_LENGTHTYPE_PC = 10,
	}

	/// <summary>
	/// The units in which an SvgAngle can be specified
	/// </summary>
	public enum SvgAngleType
	{
		SVG_ANGLETYPE_UNKNOWN = 0,
		SVG_ANGLETYPE_UNSPECIFIED = 1,
		SVG_ANGLETYPE_DEG = 2,
		SVG_ANGLETYPE_RAD = 3,
		SVG_ANGLETYPE_GRAD = 4,
	}

	/// <summary>
	/// The various different types of segment that make up an SVG path, as listed in the SVG Path grammar.
	/// </summary>
	public enum SvgPathSegType
	{
		SVG_SEGTYPE_UNKNOWN = 0,
		SVG_SEGTYPE_MOVETO,
		SVG_SEGTYPE_CLOSEPATH,
		SVG_SEGTYPE_LINETO,
		SVG_SEGTYPE_HLINETO,
		SVG_SEGTYPE_VLINETO,
		SVG_SEGTYPE_CURVETO,
		SVG_SEGTYPE_SMOOTHCURVETO,
		SVG_SEGTYPE_BEZIERTO,
		SVG_SEGTYPE_SMOOTHBEZIERTO,
		SVG_SEGTYPE_ARCTO
	}


	/// <summary>
	/// A number, as specified in the SVG standard.  It is stored as a float.
	/// </summary>
	public class SvgNumber : ICloneable
	{
		float _num;

		public SvgNumber(string s)
		{
			FromString(s);
		}

		public SvgNumber(int n)
		{
			_num = n;
		}

		public SvgNumber(float n)
		{
			_num = n;
		}

		public object Clone()
		{
			return new SvgNumber(_num);
		}

		/// <summary>
		/// float.Parse is used to parse the string.  float.Parse does not follow the exact rules of the SVG spec.
		/// </summary>
		/// <param name="s"></param>
		public void FromString(string s)
		{
			try {
				_num = float.Parse(s);
			} catch {
				throw new SvgException("Invalid SvgNumber", s);
			}
		}

		/// <summary>
		/// float.ToString is used to output a string.  This is true for all numbers in SvgNet.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return _num.ToString("F", System.Globalization.CultureInfo.InvariantCulture);
		}

		public static implicit operator SvgNumber(string s)
		{
			return new SvgNumber(s);
		}

		public static implicit operator SvgNumber(int n)
		{
			return new SvgNumber(n);
		}

		public static implicit operator SvgNumber(float n)
		{
			return new SvgNumber(n);
		}
	}

	/// <summary>
	/// A length or coordinate component (in SVG 1.1 the specification says they are the same)
	/// </summary>
	public class SvgLength : ICloneable
	{
		float _num;
		SvgLengthType _type;

		public SvgLength(string s)
		{
			FromString(s);
		}

		public SvgLength(float f)
		{
			_num = f;
			_type = SvgLengthType.SVG_LENGTHTYPE_UNKNOWN;
		}

		public SvgLength(float f, SvgLengthType type)
		{
			_num = f;
			_type = type;
		}

		public object Clone()
		{
			return new SvgLength(_num, _type);
		}

		public void FromString(string s)
		{
			int i = s.LastIndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
			if (i == -1)
				return;

			_num = float.Parse(s.Substring(0, i + 1));

			switch (s.Substring(i + 1)) {
				case "%":
					_type = SvgLengthType.SVG_LENGTHTYPE_PERCENTAGE;
					break;
				case "em":
					_type = SvgLengthType.SVG_LENGTHTYPE_EMS;
					break;
				case "ex":
					_type = SvgLengthType.SVG_LENGTHTYPE_EXS;
					break;
				case "px":
					_type = SvgLengthType.SVG_LENGTHTYPE_PX;
					break;
				case "cm":
					_type = SvgLengthType.SVG_LENGTHTYPE_CM;
					break;
				case "mm":
					_type = SvgLengthType.SVG_LENGTHTYPE_MM;
					break;
				case "in":
					_type = SvgLengthType.SVG_LENGTHTYPE_IN;
					break;
				case "pt":
					_type = SvgLengthType.SVG_LENGTHTYPE_PT;
					break;
				case "pc":
					_type = SvgLengthType.SVG_LENGTHTYPE_PC;
					break;
				case "":
					_type = SvgLengthType.SVG_LENGTHTYPE_UNKNOWN;
					break;
				default:
					throw new SvgException("Invalid SvgLength", s);
			}

		}

		public override string ToString()
		{
			string s = _num.ToString("F", System.Globalization.CultureInfo.InvariantCulture);
			switch (_type) {
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

		public static implicit operator SvgLength(string s)
		{
			return new SvgLength(s);
		}

		public static implicit operator SvgLength(float s)
		{
			return new SvgLength(s);
		}


		public float Value
		{
			get { return _num; }
			set { _num = value; }
		}
		public SvgLengthType Type
		{
			get { return _type; }
			set { _type = value; }
		}
	}

	/// <summary>
	/// An angle, as found here and there throughout the SVG spec
	/// </summary>
	public class SvgAngle : ICloneable
	{
		float _num;
		SvgAngleType _type;

		public SvgAngle(string s)
		{
			FromString(s);
		}

		public SvgAngle(float num, SvgAngleType type)
		{
			_num = num;
			_type = type;
		}

		public object Clone()
		{
			return new SvgAngle(_num, _type);
		}

		public void FromString(string s)
		{
			int i = s.LastIndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
			if (i == -1)
				return;

			_num = int.Parse(s.Substring(0, i + 1));

			switch (s.Substring(i + 1)) {
				case "grad":
					_type = SvgAngleType.SVG_ANGLETYPE_GRAD;
					break;
				case "rad":
					_type = SvgAngleType.SVG_ANGLETYPE_RAD;
					break;
				case "deg":
					_type = SvgAngleType.SVG_ANGLETYPE_DEG;
					break;
				case "":
					_type = SvgAngleType.SVG_ANGLETYPE_UNSPECIFIED;
					break;
				default:
					throw new SvgException("Invalid SvgAngle", s);
			}

		}

		public override string ToString()
		{
			string s = _num.ToString("F", System.Globalization.CultureInfo.InvariantCulture);
			switch (_type) {
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

		public static implicit operator SvgAngle(string s)
		{
			return new SvgAngle(s);
		}

		public float Value
		{
			get { return _num; }
			set { _num = value; }
		}
		public SvgAngleType Type
		{
			get { return _type; }
			set { _type = value; }
		}
	}


	/// <summary>
	/// A color, as found in CSS2 and used in SVG.  As well as a GDI Color object, SvgColor stores
	/// the string it was initialized from, so that when a color specified as 'black' is written out,
	/// it will be written 'black' rather than '#000000'
	/// </summary>
	public class SvgColor : ICloneable
	{
		Color _col;
		string _original_string;

		static Hashtable _stdcols = new Hashtable();

		public SvgColor(string s)
		{
			FromString(s);
		}

		public SvgColor(Color c)
		{
			_col = c;
		}

		public SvgColor(Color c, string s)
		{
			_col = c;
			_original_string = s;
		}

		public object Clone()
		{
			return new SvgColor(_col, _original_string);
		}

		/// <summary>
		/// As well as parsing the four types of CSS color descriptor (rgb, #xxxxxx, color name, and system color name),
		/// the FromString of this type stores the original string
		/// </summary>
		/// <param name="s"></param>
		public void FromString(string s)
		{
			_original_string = s;

			if (s.StartsWith("#")) {
				FromHexString(s);
				return;
			}

			Regex rg = new Regex(@"[rgbRGB]{3}");
			if (rg.Match(s).Success) {
				FromRGBString(s);
				return;
			}

			_col = Color.FromName(s);

			if (_col.A == 0)
				throw new SvgException("Invalid SvgColor", s);
		}

		/// <summary>
		/// If the SvgColor was constructed from a string, use that string; otherwise use rgb() form
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if (_original_string != null)
				return _original_string;

			string s = "rgb(";
			s += _col.R.ToString();
			s += ",";
			s += _col.G.ToString();
			s += ",";
			s += _col.B.ToString();
			s += ")";

			return s;
		}

		public Color Color
		{
			get { return _col; }
			set { _col = value; }
		}


		public static implicit operator SvgColor(Color c)
		{
			return new SvgColor(c);
		}

		public static implicit operator SvgColor(string s)
		{
			return new SvgColor(s);
		}

		private void FromHexString(string s)
		{
			int r, g, b;
			s = s.Substring(1);

			if (s.Length == 3) {
				r = int.Parse(s.Substring(0, 1), NumberStyles.HexNumber);
				g = int.Parse(s.Substring(1, 1), NumberStyles.HexNumber);
				b = int.Parse(s.Substring(2, 1), NumberStyles.HexNumber);
				r += r * 16;
				g += g * 16;
				b += b * 16;
				_col = Color.FromArgb(r, g, b);
			} else if (s.Length == 6) {
				r = int.Parse(s.Substring(0, 2), NumberStyles.HexNumber);
				g = int.Parse(s.Substring(2, 2), NumberStyles.HexNumber);
				b = int.Parse(s.Substring(4, 2), NumberStyles.HexNumber);
				_col = Color.FromArgb(r, g, b);
			} else {
				throw new SvgException("Invalid SvgColor", s);
			}
		}

		private void FromRGBString(string s)
		{
			int r, g, b;
			Regex rg = new Regex(@"[rgbRGB ]+\( *(?<r>\d+)[, ]+(?<g>\d+)[, ]+(?<b>\d+) *\)");
			Match m = rg.Match(s);
			if (m.Success) {
				r = int.Parse(m.Groups["r"].Captures[0].Value);
				g = int.Parse(m.Groups["g"].Captures[0].Value);
				b = int.Parse(m.Groups["b"].Captures[0].Value);

				_col = Color.FromArgb(r, g, b);
				return;
			}

			rg = new Regex(@"[rgbRGB ]+\( *(?<r>\d+)%[, ]+(?<g>\d+)%[, ]+(?<b>\d+)% *\)");
			m = rg.Match(s);
			if (m.Success) {
				r = int.Parse(m.Groups["r"].Captures[0].Value) * 255 / 100;
				g = int.Parse(m.Groups["g"].Captures[0].Value) * 255 / 100;
				b = int.Parse(m.Groups["b"].Captures[0].Value) * 255 / 100;

				_col = Color.FromArgb(r, g, b);
				return;
			}

			throw new SvgException("Invalid SvgColor", s);
		}
	}
	/*
		/// <summary>
		/// A rectangle.  The Svg spec does not define a rectangle type and this should probably be replaced with a
		/// number list
		/// </summary>
		public class SvgRect
		{
			RectangleF _rc;

			public SvgRect(string s)
			{
				FromString(s);
			}

			public SvgRect(RectangleF rc)
			{
				_rc = rc;
			}

			public void FromString(string s)
			{
				string[] toks = s.Split(new char[]{',', ' '});
				if (toks.Length != 4)
					throw new SvgException("Invalid SvgRect", s);
				try
				{
					_rc.X = float.Parse(toks[0].Trim());
					_rc.Y = float.Parse(toks[1].Trim());
					_rc.Width = float.Parse(toks[2].Trim());
					_rc.Height = float.Parse(toks[3].Trim());
				}
				catch(Exception )
				{
					throw new SvgException("Invalid SvgRect", s);
				}
			}

			public override string ToString()
			{
				string s = "";

				s += _rc.X.ToString();  s += ",";
				s += _rc.Y.ToString();  s += ",";
				s += _rc.Width.ToString();  s += ",";
				s += _rc.Height.ToString();

				return s;
			}

			public static implicit operator SvgRect(string s)
			{
				return new SvgRect(s);
			}
		}
	*/
	/// <summary>
	/// A segment in an Svg path.  This is not a real SVG type; it is not in the SVG spec.  It is provided for making paths
	/// easier to specify and parse.
	/// </summary>
	public class PathSeg : ICloneable
	{
		public SvgPathSegType _type;
		public bool _abs;
		public float[] _data;

		public PathSeg(SvgPathSegType t, bool a, float[] arr)
		{
			_type = t;
			_abs = a;
			_data = arr;
		}

		public object Clone()
		{
			return new PathSeg(_type, _abs, (float[])_data.Clone());
		}

		public float[] Data { get { return _data; } }
		public SvgPathSegType Type { get { return _type; } }
		public bool Abs { get { return _abs; } }
		public string Char
		{
			get
			{
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

	};

	/// <summary>
	/// A path, composed of segments, as described in the SVG 1.1 spec section 8.3
	/// </summary>
	public class SvgPath : ICloneable
	{
		ArrayList _path;

		public SvgPath(string s)
		{
			FromString(s);
		}

		public object Clone()
		{
			//we resort to using to/from string rather than writing an efficient clone, for the moment.
			return new SvgPath(this.ToString());
		}

		/// <summary>
		/// The parsing of the path is not completely perfect yet.  You can only have one space between path elements.
		/// </summary>
		/// <param name="s"></param>
		public void FromString(string s)
		{
			string[] sa = s.Split(new char[] { ' ', ',', '\t', '\r', '\n' });

			PathSeg ps;
			int datasize = 0;
			SvgPathSegType pt = SvgPathSegType.SVG_SEGTYPE_UNKNOWN;
			bool abs = false;
			int i = 0;
			char segTypeChar;
			_path = new ArrayList();


			while (i < sa.Length) {
				if (sa[i] == "") {
					i += 1;
					continue;
				}

				//check for a segment-type character

				if (char.IsLetter(sa[i][0])) {
					segTypeChar = sa[i][0];


					if (segTypeChar == 'M' || segTypeChar == 'm') {
						pt = SvgPathSegType.SVG_SEGTYPE_MOVETO;
						abs = (segTypeChar == 'M');
						datasize = 2;
					} else if (segTypeChar == 'Z' || segTypeChar == 'z') {
						pt = SvgPathSegType.SVG_SEGTYPE_CLOSEPATH;
						datasize = 0;
					} else if (segTypeChar == 'L' || segTypeChar == 'l') {
						pt = SvgPathSegType.SVG_SEGTYPE_LINETO;
						abs = (segTypeChar == 'L');
						datasize = 2;
					} else if (segTypeChar == 'H' || segTypeChar == 'h') {
						pt = SvgPathSegType.SVG_SEGTYPE_HLINETO;
						abs = (segTypeChar == 'H');
						datasize = 1;
					} else if (segTypeChar == 'V' || segTypeChar == 'v') {
						pt = SvgPathSegType.SVG_SEGTYPE_VLINETO;
						abs = (segTypeChar == 'V');
						datasize = 1;
					} else if (segTypeChar == 'C' || segTypeChar == 'c') {
						pt = SvgPathSegType.SVG_SEGTYPE_CURVETO;
						abs = (segTypeChar == 'C');
						datasize = 6;
					} else if (segTypeChar == 'S' || segTypeChar == 's') {
						pt = SvgPathSegType.SVG_SEGTYPE_SMOOTHCURVETO;
						abs = (segTypeChar == 'S');
						datasize = 4;
					} else if (segTypeChar == 'Q' || segTypeChar == 'q') {
						pt = SvgPathSegType.SVG_SEGTYPE_BEZIERTO;
						abs = (segTypeChar == 'Q');
						datasize = 4;
					} else if (segTypeChar == 'T' || segTypeChar == 't') {
						pt = SvgPathSegType.SVG_SEGTYPE_SMOOTHBEZIERTO;
						abs = (segTypeChar == 'T');
						datasize = 2;
					} else if (segTypeChar == 'A' || segTypeChar == 'a') {
						pt = SvgPathSegType.SVG_SEGTYPE_ARCTO;
						abs = (segTypeChar == 'A');
						datasize = 7;
					} else {
						throw new SvgException("Invalid SvgPath", s);
					}

					//strip off type character
					sa[i] = sa[i].Substring(1);

					if (sa[i] == "")
						i += 1;
				}


				if (pt == SvgPathSegType.SVG_SEGTYPE_UNKNOWN)
					throw new SvgException("Invalid SvgPath", s);

				float[] arr = new float[datasize];

				for (int j = 0; j < datasize; ++j) {
					arr[j] = float.Parse(sa[i + j]);
				}

				ps = new PathSeg(pt, abs, arr);

				_path.Add(ps);

				i += datasize;
			}
		}

		public override string ToString()
		{
			PathSeg prev = null;
			string s = "";
			foreach (PathSeg seg in _path) {
				if (prev == null || (prev.Type != seg.Type || prev.Abs != seg.Abs)) {
					s += seg.Char;
					s += " ";
				}

				foreach (float d in seg.Data) {
					s += d.ToString();
					s += " ";
				}

				prev = seg;
			}

			return s;

		}

		public PathSeg this[int idx]
		{
			get { return (PathSeg)_path[idx]; }
			set { _path[idx] = value; }
		}

		public int Count
		{
			get { return _path.Count; }
		}

		public static implicit operator SvgPath(string s)
		{
			return new SvgPath(s);
		}
	}

	/// <summary>
	/// A list of points, as specified in the SVG 1.1 spec section 9.8.  Only used in polygon and polyline elements.
	/// </summary>
	public class SvgPoints : ICloneable
	{
		ArrayList _pts = new ArrayList();

		public SvgPoints(string s)
		{
			FromString(s);
		}

		public SvgPoints(PointF[] pts)
		{
			foreach (PointF p in pts) {
				_pts.Add(p.X);
				_pts.Add(p.Y);
			}
		}

		public object Clone()
		{
			return new SvgPoints((PointF[])_pts.ToArray(typeof(PointF)));
		}

		/// <summary>
		/// The array must have an even length
		/// </summary>
		/// <param name="pts"></param>
		public SvgPoints(float[] pts)
		{
			if (pts.Length % 2 != 0)
				throw new SvgException("Invalid SvgPoints", pts.ToString());

			foreach (float p in pts) {
				_pts.Add(p);
			}
		}

		/// <summary>
		/// The standard boils down to a list of numbers in any format separated by any amount of wsp and commas;
		/// in other words it looks the same as a SvgNumList
		/// </summary>
		/// <param name="s"></param>
		public void FromString(string s)
		{
			try {
				float[] fa = SvgNumList.String2Floats(s);
				foreach (float f in fa) {
					_pts.Add(f);
				}
			} catch (Exception) {
				throw new SvgException("Invalid SvgPoints", s);
			}

			if (_pts.Count % 2 != 0)
				throw new SvgException("Invalid SvgPoints", s);
		}

		public override string ToString()
		{
			string result = "";

			foreach (float f in _pts) {
				result += f.ToString("F", System.Globalization.CultureInfo.InvariantCulture);
				result += " ";
			}

			return result;
		}

		public static implicit operator SvgPoints(string s)
		{
			return new SvgPoints(s);
		}

		public static implicit operator SvgPoints(PointF[] pts)
		{
			return new SvgPoints(pts);
		}
	}

	/// <summary>
	/// A number list, as used in the SVG spec for e.g. the value of a viewBox attribute.  Basically a list of numbers in
	/// any format separated by whitespace and commas.
	/// </summary>
	public class SvgNumList : ICloneable
	{
		ArrayList _pts = new ArrayList();

		public SvgNumList(string s)
		{
			FromString(s);
		}

		public SvgNumList(float[] pts)
		{
			foreach (float p in pts) {
				_pts.Add(p);
			}
		}

		public object Clone()
		{
			return new SvgNumList((float[])_pts.ToArray(typeof(float)));
		}

		public void FromString(string s)
		{
			try {
				float[] fa = String2Floats(s);

				foreach (float f in fa) {
					_pts.Add(f);
				}
			} catch (Exception) {
				throw new SvgException("Invalid SvgNumList", s);
			}

		}

		public override string ToString()
		{
			string result = "";

			foreach (float f in _pts) {
				result += f.ToString("F", System.Globalization.CultureInfo.InvariantCulture);
				result += " ";
			}

			return result;
		}

		public float this[int idx]
		{
			get { return (float)_pts[idx]; }
			set { _pts[idx] = value; }
		}

		public int Count
		{
			get { return _pts.Count; }
		}

		public static implicit operator SvgNumList(string s)
		{
			return new SvgNumList(s);
		}

		public static implicit operator SvgNumList(float[] f)
		{
			return new SvgNumList(f);
		}

		public static float[] String2Floats(string s)
		{
			try {
				string[] sa = s.Split(new char[] { ',', ' ', '\t', '\r', '\n' });

				ArrayList arr = new ArrayList();

				foreach (string str in sa) {
					if (str != "") {
						str.Trim();
						arr.Add(Single.Parse(str));
					}
				}

				return (float[])arr.ToArray(typeof(float));
			} catch (Exception) {
				throw new SvgException("Invalid number list", s);
			}
		}

	}

	/// <summary>
	/// Represents a URI reference within a style.  Local uri references are generally strings of the form
	/// <c>url(#elementID)</c>.   This class should not be confused with <see cref="SvgXRef"/> which represents
	/// the xlink:* properties of, for example, an <c>a</c> element.
	/// </summary>
	public class SvgUriReference : ICloneable
	{
		private string _href;

		public SvgUriReference()
		{
		}

		public SvgUriReference(string href)
		{
			_href = href;
		}

		public SvgUriReference(SvgElement target)
		{
			_href = "#" + target.Id;
			if (target.Id == "") {
				throw new SvgException("Uri Reference cannot refer to an element with no id.", target.ToString());
			}
		}

		public object Clone()
		{
			return new SvgUriReference(_href);
		}

		public override string ToString()
		{
			return "url(" + _href + ")";
		}

		public string Href
		{
			get { return _href; }
			set { _href = value; }
		}

	}
	/// <summary>
	/// Represents a URI reference.  Unlike most svg types, uri references are represented by more than one attribute
	/// of an element.  This means special measures are required to get and set uri references.
	/// </summary>
	public class SvgXRef : ICloneable
	{
		private string _href;
		private string _type = "simple";
		private string _role;
		private string _arcrole;
		private string _title;
		private string _show;
		private string _actuate = "onLoad";

		public SvgXRef()
		{
		}

		public SvgXRef(string href)
		{
			_href = href;
		}

		public SvgXRef(SvgStyledTransformedElement el)
		{
			ReadFromElement(el);
		}

		public object Clone()
		{
			SvgXRef r = new SvgXRef();
			r.Href = Href;
			r.Type = Type;
			r.Role = Role;
			r.Arcrole = Arcrole;
			r.Title = Title;
			r.Show = Show;
			r.Actuate = Actuate;
			return r;
		}

		public override string ToString()
		{
			return _href;
		}

		public void WriteToElement(SvgStyledTransformedElement el)
		{
			el["xlink:href"] = _href;
			//if (_type != "simple") el["xlink:type"] = _type;
			el["xlink:role"] = _role;
			el["xlink:arcrole"] = _arcrole;
			el["xlink:title"] = _title;
			el["xlink:show"] = _show;
			//if (_type != "onLoad") el["xlink:actuate"] = _actuate;
		}

		public void ReadFromElement(SvgStyledTransformedElement el)
		{
			_href = (string)el["xlink:href"];
			_role = (string)el["xlink:role"];
			_arcrole = (string)el["xlink:arcrole"];
			_title = (string)el["xlink:title"];
			_show = (string)el["xlink:show"];

			//ignore the possibility of setting type and actuate for now
		}

		public string Href
		{
			get { return _href; }
			set { _href = value; }
		}

		public string Type
		{
			get { return _type; }
			set { _type = value; }
		}

		public string Role
		{
			get { return _role; }
			set { _role = value; }
		}

		public string Arcrole
		{
			get { return _arcrole; }
			set { _arcrole = value; }
		}

		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		public string Show
		{
			get { return _show; }
			set { _show = value; }
		}

		public string Actuate
		{
			get { return _actuate; }
			set { _actuate = value; }
		}


	}

}
