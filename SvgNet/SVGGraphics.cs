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
using System.Xml;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using SvgNet.SvgElements;
using SvgNet.SvgTypes;
using System.Collections;

namespace SvgNet.SvgGdi
{
	/// <summary>
	/// This is an IGraphics implementor that builds up an SVG scene.  Use it like a regular <c>Graphics</c> object, and call
	/// <c>WriteXMLString</c> to output SVG.  In this way, whatever you would normally draw becomes available as an SVG document.
	/// <para>
	/// SvgGraphics has to do quite a lot of work to convert GDI instructions to SVG equivalents.  Some things are approximated and slight differences will
	/// be noticed.  Also, in several places GDI+ does not do what it is supposed to (e.g. arcs of non-circular ellipses, truncating bitmaps).  In these cases
	/// SvgGraphics does do the right thing, so the result will be different.
	/// </para>
	/// <para>
	/// Some GDI instructions such as <c>MeasureString</c>
	/// are meaningless in SVG, usually because there is no physical display device to refer to.  When such a method is called an <see cref="SvgGdiNotImpl"/> exception is thrown.
	/// </para>
	/// <para>
	/// Many parameters used by GDI have no SVG equivalent -- for instance, GDI allows some fine control over how font hints are used.  This detailed information is
	/// thrown away. 
	/// </para>
	/// <para>
	/// Some aspects of GDI that can be implemented in SVG are not.  The most important omission is that only solid brushes are supported.
	/// </para>
	/// </summary>
	public class SvgGraphics : IGraphics
	{

		/// <summary>
		/// This class is needed because GDI+ does not maintain a proper scene graph; rather it maintains a single transformation matrix
		/// which is applied to each new object.  The matrix is saved and reloaded when 'begincontainer' and 'endcontainer' are called.  SvgGraphics has to
		/// emulate this behaviour.  
		/// <para>
		/// This matrix stack caches it's 'result' (ie. the current transformation, the product of all matrices).  The result is
		/// recalculated when necessary.
		/// </para>
		/// </summary>
		private class MatrixStack
		{
			ArrayList _mx;
			Matrix _result = null;

			public MatrixStack()
			{
				_mx = new ArrayList();

				//we need 2 identity matrices on the stack.  This is because we do a resettransform()
				//by pop dup (to set current xform to xform of enclosing group).
				Push();
				Push();
			}

			public void Dup()
			{
				_mx.Insert(_mx.Count, Top.Clone());
				_result = null;
			}

			public void Pop()
			{
				if (_mx.Count <= 1)
					return;

				_mx.RemoveAt(_mx.Count - 1);
				_result = null;
			}

			public void Push()
			{
				_mx.Add(new Matrix());
			}

			public Matrix Top
			{
				get
				{
					//because we cannot return a const, we have to reset result
					//even though the caller might not even want to change the matrix.  This a typical 
					//problem with weaker languages that don't have const.
					_result = null;
					return (Matrix)_mx[_mx.Count - 1];
				}

				set
				{
					_mx[_mx.Count - 1] = value;
					_result = null;
				}
			}

			public Matrix Result
			{
				get
				{
					if (_result != null)
						return _result;

					_result = new Matrix();

					foreach (Matrix mat in _mx) {
						if (mat.IsIdentity == false)
							_result.Multiply(mat);
					}

					return _result;
				}
			}

		}


		SvgSvgElement _root;
		SvgGroupElement _topgroup;
		SvgRectElement _bg;
		SvgDefsElement _defs;
		SvgStyledTransformedElement _cur;
		MatrixStack _transforms;

		//a default graphics so that we can make a guess as to functions like MeasureString
		static Graphics _g;

		public SvgGraphics()
		{
			_root = new SvgSvgElement();
			_root.Id = "SvgGdi_output";

			_bg = new SvgRectElement(0, 0, "100%", "100%");
			_bg.Style.Set("fill", new SvgColor(Color.FromName("Control")));
			_bg.Id = "background";
			_root.AddChild(_bg);

			_topgroup = new SvgGroupElement("root_group");
			_topgroup.Style.Set("shape-rendering", "crispEdges");
			_cur = _topgroup;
			_root.AddChild(_topgroup);

			_defs = new SvgDefsElement("clips_hatches_and_gradients");
			_root.AddChild(_defs);

			_transforms = new MatrixStack();
		}

		/// <summary>
		/// Get a string containing an SVG document.  The very heart of SvgGdi.  It calls <c>WriteSVGString</c> on the <see cref="SvgElement"/>
		/// at the root of this <c>SvgGraphics</c> and returns the resulting string.
		/// </summary>
		public string WriteSVGString()
		{
			return _root.WriteSVGString(true);
		}

		/// <summary>
		/// Does nothing
		/// </summary>
		public void Flush()
		{
			//nothing to do
		}

		/// <summary>
		/// Does nothing
		/// </summary>
		/// <param name="intention"></param>
		public void Flush(FlushIntention intention)
		{
			//nothing to do
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void ResetTransform()
		{
			_transforms.Pop();
			_transforms.Dup();
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void MultiplyTransform(Matrix matrix)
		{
			_transforms.Top.Multiply(matrix);
		}

		/// <summary>
		/// Implemented, but ignores <c>order</c>
		/// </summary>
		public void MultiplyTransform(Matrix matrix, MatrixOrder order)
		{
			_transforms.Top.Multiply(matrix, order);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void TranslateTransform(Single dx, Single dy)
		{
			_transforms.Top.Translate(dx, dy);
		}

		/// <summary>
		/// Implemented, but ignores <c>order</c>
		/// </summary>
		public void TranslateTransform(Single dx, Single dy, MatrixOrder order)
		{
			_transforms.Top.Translate(dx, dy, order);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void ScaleTransform(Single sx, Single sy)
		{
			_transforms.Top.Scale(sx, sy);
		}

		/// <summary>
		/// Implemented, but ignores <c>order</c>
		/// </summary>
		public void ScaleTransform(Single sx, Single sy, MatrixOrder order)
		{
			_transforms.Top.Scale(sx, sy, order);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void RotateTransform(Single angle)
		{
			_transforms.Top.Rotate(angle);
		}

		/// <summary>
		/// Implemented, but ignores <c>order</c>
		/// </summary>
		public void RotateTransform(Single angle, MatrixOrder order)
		{
			_transforms.Top.Rotate(angle, order);
		}

		/// <summary>
		/// Not implemented.
		/// </summary>
		public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, PointF[] pts) { throw new SvgGdiNotImpl("TransformPoints (CoordinateSpace destSpace, CoordinateSpace srcSpace, PointF[] pts)"); }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, Point[] pts) { throw new SvgGdiNotImpl("TransformPoints (CoordinateSpace destSpace, CoordinateSpace srcSpace, Point[] pts)"); }
		/// <summary>
		/// Not meaningful when there is no actual display device.
		/// </summary>
		public System.Drawing.Color GetNearestColor(Color color) { throw new SvgGdiNotImpl("GetNearestColor (Color color)"); }

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawLine(Pen pen, Single x1, Single y1, Single x2, Single y2)
		{
			SvgLineElement lin = new SvgLineElement(x1, y1, x2, y2);
			lin.Style = new SvgStyle(pen);
			if (!_transforms.Result.IsIdentity)
				lin.Transform = new SvgTransformList(_transforms.Result.Clone());
			_cur.AddChild(lin);

			DrawEndAnchors(pen, new PointF(x1, y1), new PointF(x2, y2));
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawLine(Pen pen, PointF pt1, PointF pt2)
		{
			DrawLine(pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawLines(Pen pen, PointF[] points)
		{
			SvgPolylineElement pl = new SvgPolylineElement(points);
			pl.Style = new SvgStyle(pen);
			if (!_transforms.Result.IsIdentity)
				pl.Transform = new SvgTransformList(_transforms.Top.Clone());
			_cur.AddChild(pl);

			DrawEndAnchors(pen, points[0], points[points.Length - 1]);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawLine(Pen pen, Int32 x1, Int32 y1, Int32 x2, Int32 y2)
		{
			DrawLine(pen, (Single)x1, (Single)y1, (Single)x2, (Single)y2);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawLine(Pen pen, Point pt1, Point pt2)
		{
			DrawLine(pen, (Single)pt1.X, (Single)pt1.Y, (Single)pt2.X, (Single)pt2.Y);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawLines(Pen pen, Point[] points)
		{
			PointF[] pts = Point2PointF(points);
			DrawLines(pen, pts);
		}

		/// <summary>
		/// Implemented.  <c>DrawArc</c> functions work correctly and thus produce different output from GDI+ if the ellipse is not circular.
		/// </summary>
		public void DrawArc(Pen pen, Single x, Single y, Single width, Single height, Single startAngle, Single sweepAngle)
		{
			string s = GDIArc2SVGPath(x, y, width, height, startAngle, sweepAngle, false);

			SvgPathElement arc = new SvgPathElement();
			arc.D = s;
			arc.Style = new SvgStyle(pen);
			if (!_transforms.Result.IsIdentity)
				arc.Transform = new SvgTransformList(_transforms.Result.Clone());
			_cur.AddChild(arc);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawArc(Pen pen, RectangleF rect, Single startAngle, Single sweepAngle)
		{
			DrawArc(pen, rect.X, rect.X, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawArc(Pen pen, Int32 x, Int32 y, Int32 width, Int32 height, Int32 startAngle, Int32 sweepAngle)
		{
			DrawArc(pen, (Single)x, (Single)y, (Single)width, (Single)height, (Single)startAngle, (Single)sweepAngle);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawArc(Pen pen, Rectangle rect, Single startAngle, Single sweepAngle)
		{
			DrawArc(pen, (Single)rect.X, (Single)rect.X, (Single)rect.Width, (Single)rect.Height, (Single)startAngle, (Single)sweepAngle);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawBezier(Pen pen, Single x1, Single y1, Single x2, Single y2, Single x3, Single y3, Single x4, Single y4)
		{
			SvgPathElement bez = new SvgPathElement();

			bez.D = "M " + x1.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " " + y1.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " C " +
				x2.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " " + y2.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " " +
				x3.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " " + y3.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " " +
				x4.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " " + y4.ToString("F", System.Globalization.CultureInfo.InvariantCulture);

			bez.Style = new SvgStyle(pen);
			if (!_transforms.Result.IsIdentity)
				bez.Transform = new SvgTransformList(_transforms.Result.Clone());
			_cur.AddChild(bez);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawBezier(Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4)
		{
			DrawBezier(pen, pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawBeziers(Pen pen, PointF[] points)
		{
			SvgPathElement bez = new SvgPathElement();

			string s = "M " + points[0].X.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " " + points[0].Y.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " C ";

			for (int i = 1; i < points.Length; ++i) {
				s += points[i].X.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " " + points[i].Y.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " ";
			}

			bez.D = s;

			bez.Style = new SvgStyle(pen);
			if (!_transforms.Result.IsIdentity)
				bez.Transform = new SvgTransformList(_transforms.Result.Clone());
			_cur.AddChild(bez);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawBezier(Pen pen, Point pt1, Point pt2, Point pt3, Point pt4)
		{
			DrawBezier(pen, (Single)pt1.X, (Single)pt1.Y, (Single)pt2.X, (Single)pt2.Y, (Single)pt3.X, (Single)pt3.Y, (Single)pt4.X, (Single)pt4.Y);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawBeziers(Pen pen, Point[] points)
		{
			PointF[] pts = Point2PointF(points);
			DrawBeziers(pen, pts);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawRectangle(Pen pen, Rectangle rect)
		{
			DrawRectangle(pen, (Single)rect.Left, (Single)rect.Top, (Single)rect.Width, (Single)rect.Height);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawRectangle(Pen pen, Single x, Single y, Single width, Single height)
		{
			SvgRectElement rc = new SvgRectElement(x, y, width, height);
			rc.Style = new SvgStyle(pen);
			if (!_transforms.Result.IsIdentity)
				rc.Transform = new SvgTransformList(_transforms.Result.Clone());
			_cur.AddChild(rc);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawRectangles(Pen pen, RectangleF[] rects)
		{
			foreach (RectangleF rc in rects) {
				DrawRectangle(pen, rc.Left, rc.Top, rc.Width, rc.Height);
			}
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawRectangle(Pen pen, Int32 x, Int32 y, Int32 width, Int32 height)
		{
			DrawRectangle(pen, (Single)x, (Single)y, (Single)width, (Single)height);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawRectangles(Pen pen, Rectangle[] rects)
		{
			foreach (Rectangle rc in rects) {
				DrawRectangle(pen, (Single)rc.Left, (Single)rc.Top, (Single)rc.Width, (Single)rc.Height);
			}
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawEllipse(Pen pen, RectangleF rect)
		{
			DrawEllipse(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawEllipse(Pen pen, Single x, Single y, Single width, Single height)
		{
			SvgEllipseElement el = new SvgEllipseElement(x + width / 2, y + height / 2, width / 2, height / 2);
			el.Style = new SvgStyle(pen);
			if (!_transforms.Result.IsIdentity)
				el.Transform = new SvgTransformList(_transforms.Result.Clone());
			_cur.AddChild(el);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawEllipse(Pen pen, Rectangle rect)
		{
			DrawEllipse(pen, (Single)rect.X, (Single)rect.Y, (Single)rect.Width, (Single)rect.Height);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawEllipse(Pen pen, Int32 x, Int32 y, Int32 width, Int32 height)
		{
			DrawEllipse(pen, (Single)x, (Single)y, (Single)width, (Single)height);
		}

		/// <summary>
		/// Implemented.  <c>DrawPie</c> functions work correctly and thus produce different output from GDI+ if the ellipse is not circular.
		/// </summary>
		public void DrawPie(Pen pen, Single x, Single y, Single width, Single height, Single startAngle, Single sweepAngle)
		{
			string s = GDIArc2SVGPath(x, y, width, height, startAngle, sweepAngle, true);

			SvgPathElement pie = new SvgPathElement();
			pie.D = s;
			pie.Style = new SvgStyle(pen);
			if (!_transforms.Result.IsIdentity)
				pie.Transform = new SvgTransformList(_transforms.Result.Clone());

			_cur.AddChild(pie);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawPie(Pen pen, RectangleF rect, Single startAngle, Single sweepAngle)
		{
			DrawPie(pen, rect.X, rect.X, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawPie(Pen pen, Int32 x, Int32 y, Int32 width, Int32 height, Int32 startAngle, Int32 sweepAngle)
		{
			DrawPie(pen, (Single)x, (Single)y, (Single)width, (Single)height, (Single)startAngle, (Single)sweepAngle);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawPie(Pen pen, Rectangle rect, Single startAngle, Single sweepAngle)
		{
			DrawPie(pen, (Single)rect.X, (Single)rect.X, (Single)rect.Width, (Single)rect.Height, (Single)startAngle, (Single)sweepAngle);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawPolygon(Pen pen, PointF[] points)
		{
			SvgPolygonElement pl = new SvgPolygonElement(points);
			pl.Style = new SvgStyle(pen);
			if (!_transforms.Result.IsIdentity)
				pl.Transform = new SvgTransformList(_transforms.Result.Clone());
			_cur.AddChild(pl);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawPolygon(Pen pen, Point[] points)
		{
			PointF[] pts = Point2PointF(points);
			DrawPolygon(pen, pts);
		}


		/// <summary>
		/// Not implemented because GDI+ regions/paths are not emulated.
		/// </summary>
		public void DrawPath(Pen pen, GraphicsPath path) { throw new SvgGdiNotImpl("DrawPath (Pen pen, GraphicsPath path)"); }


		/// <summary>
		/// Implemented.  The <c>DrawCurve</c> functions emulate GDI behavior by drawing a coaligned cubic bezier.  This seems to produce
		/// a very good approximation so probably GDI+ does the same.
		/// </summary>
		public void DrawCurve(Pen pen, PointF[] points)
		{
			PointF[] pts = Spline2Bez(points, 0, points.Length - 1, false, .5f);
			DrawBeziers(pen, pts);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawCurve(Pen pen, PointF[] points, Single tension)
		{
			PointF[] pts = Spline2Bez(points, 0, points.Length - 1, false, tension);
			DrawBeziers(pen, pts);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawCurve(Pen pen, PointF[] points, Int32 offset, Int32 numberOfSegments)
		{
			PointF[] pts = Spline2Bez(points, offset, numberOfSegments, false, .5f);
			DrawBeziers(pen, pts);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawCurve(Pen pen, PointF[] points, Int32 offset, Int32 numberOfSegments, Single tension)
		{
			PointF[] pts = Spline2Bez(points, offset, numberOfSegments, false, tension);
			DrawBeziers(pen, pts);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawCurve(Pen pen, Point[] points)
		{
			PointF[] pts = Spline2Bez(Point2PointF(points), 0, points.Length - 1, false, .5f);
			DrawBeziers(pen, pts);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawCurve(Pen pen, Point[] points, Single tension)
		{
			PointF[] pts = Spline2Bez(Point2PointF(points), 0, points.Length - 1, false, tension);
			DrawBeziers(pen, pts);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawCurve(Pen pen, Point[] points, Int32 offset, Int32 numberOfSegments, Single tension)
		{
			PointF[] pts = Spline2Bez(Point2PointF(points), offset, numberOfSegments, false, tension);
			DrawBeziers(pen, pts);
		}

		/// <summary>
		/// Implemented.  The <c>DrawClosedCurve</c> functions emulate GDI behavior by drawing a coaligned cubic bezier.  This seems to produce
		/// a very good approximation so probably GDI+ does the same thing -- a
		/// </summary>
		public void DrawClosedCurve(Pen pen, PointF[] points)
		{
			PointF[] pts = Spline2Bez(points, 0, points.Length - 1, true, .5f);
			DrawBeziers(pen, pts);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawClosedCurve(Pen pen, PointF[] points, Single tension, FillMode fillmode)
		{
			PointF[] pts = Spline2Bez(points, 0, points.Length - 1, true, tension);
			DrawBeziers(pen, pts);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawClosedCurve(Pen pen, Point[] points)
		{
			PointF[] pts = Spline2Bez(Point2PointF(points), 0, points.Length - 1, true, .5f);
			DrawBeziers(pen, pts);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawClosedCurve(Pen pen, Point[] points, Single tension, FillMode fillmode)
		{
			PointF[] pts = Spline2Bez(Point2PointF(points), 0, points.Length - 1, true, tension);
			DrawBeziers(pen, pts);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void Clear(Color color)
		{
			_cur.Children.Clear();
			_bg.Style.Set("fill", new SvgColor(color));
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillRectangle(Brush brush, RectangleF rect)
		{
			FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillRectangle(Brush brush, Single x, Single y, Single width, Single height)
		{
			SvgRectElement rc = new SvgRectElement(x, y, width, height);
			rc.Style = HandleBrush(brush);
			if (!_transforms.Result.IsIdentity)
				rc.Transform = _transforms.Result.Clone();
			_cur.AddChild(rc);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillRectangles(Brush brush, RectangleF[] rects)
		{
			foreach (RectangleF rc in rects) {
				FillRectangle(brush, rc);
			}
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillRectangle(Brush brush, Rectangle rect)
		{
			FillRectangle(brush, (Single)rect.X, (Single)rect.Y, (Single)rect.Width, (Single)rect.Height);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillRectangle(Brush brush, Int32 x, Int32 y, Int32 width, Int32 height)
		{
			FillRectangle(brush, (Single)x, (Single)y, (Single)width, (Single)height);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillRectangles(Brush brush, Rectangle[] rects)
		{
			foreach (Rectangle rc in rects) {
				FillRectangle(brush, rc);
			}
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillPolygon(Brush brush, PointF[] points)
		{
			FillPolygon(brush, points, FillMode.Alternate);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillPolygon(Brush brush, Point[] points)
		{
			PointF[] pts = Point2PointF(points);
			FillPolygon(brush, pts, FillMode.Alternate);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillPolygon(Brush brush, PointF[] points, FillMode fillmode)
		{
			SvgPolygonElement pl = new SvgPolygonElement(points);
			pl.Style = HandleBrush(brush);
			if (fillmode == FillMode.Alternate) {
				pl.Style.Set("fill-rule", "evenodd");
			} else {
				pl.Style.Set("fill-rule", "nonzero");
			}

			if (!_transforms.Result.IsIdentity)
				pl.Transform = new SvgTransformList(_transforms.Result.Clone());
			_cur.AddChild(pl);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillPolygon(Brush brush, Point[] points, FillMode fillmode)
		{
			PointF[] pts = Point2PointF(points);
			FillPolygon(brush, pts, fillmode);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillEllipse(Brush brush, RectangleF rect)
		{
			FillEllipse(brush, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillEllipse(Brush brush, Single x, Single y, Single width, Single height)
		{
			SvgEllipseElement el = new SvgEllipseElement(x + width / 2, y + height / 2, width / 2, height / 2);
			el.Style = HandleBrush(brush);
			if (!_transforms.Result.IsIdentity)
				el.Transform = new SvgTransformList(_transforms.Result.Clone());
			_cur.AddChild(el);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillEllipse(Brush brush, Rectangle rect)
		{
			FillEllipse(brush, (Single)rect.X, (Single)rect.Y, (Single)rect.Width, (Single)rect.Height);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillEllipse(Brush brush, Int32 x, Int32 y, Int32 width, Int32 height)
		{
			FillEllipse(brush, (Single)x, (Single)y, (Single)width, (Single)height);
		}

		/// <summary>
		/// Implemented <c>FillPie</c> functions work correctly and thus produce different output from GDI+ if the ellipse is not circular.
		/// </summary>
		public void FillPie(Brush brush, Rectangle rect, Single startAngle, Single sweepAngle)
		{
			FillPie(brush, (Single)rect.X, (Single)rect.Y, (Single)rect.Width, (Single)rect.Height, startAngle, sweepAngle);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillPie(Brush brush, Single x, Single y, Single width, Single height, Single startAngle, Single sweepAngle)
		{
			string s = GDIArc2SVGPath(x, y, width, height, startAngle, sweepAngle, true);

			SvgPathElement pie = new SvgPathElement();
			pie.D = s;
			pie.Style = HandleBrush(brush);
			if (!_transforms.Result.IsIdentity)
				pie.Transform = new SvgTransformList(_transforms.Result.Clone());

			_cur.AddChild(pie);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillPie(Brush brush, Int32 x, Int32 y, Int32 width, Int32 height, Int32 startAngle, Int32 sweepAngle)
		{
			FillPie(brush, (Single)x, (Single)y, (Single)width, (Single)height, startAngle, sweepAngle);
		}

		/// <summary>
		/// Not implemented, because GDI+ regions/paths are not emulated.
		/// </summary>
		public void FillPath(Brush brush, GraphicsPath path) { throw new SvgGdiNotImpl("FillPath (Brush brush, GraphicsPath path)"); }


		/// <summary>
		/// Implemented
		/// </summary>
		public void FillClosedCurve(Brush brush, PointF[] points)
		{
			PointF[] pts = Spline2Bez(points, 0, points.Length - 1, true, .5f);
			FillBeziers(brush, pts, FillMode.Alternate);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode)
		{
			PointF[] pts = Spline2Bez(points, 0, points.Length - 1, true, .5f);
			FillBeziers(brush, pts, fillmode);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode, Single tension)
		{
			PointF[] pts = Spline2Bez(points, 0, points.Length - 1, true, tension);
			FillBeziers(brush, pts, fillmode);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillClosedCurve(Brush brush, Point[] points)
		{
			PointF[] pts = Spline2Bez(Point2PointF(points), 0, points.Length - 1, true, .5f);
			FillBeziers(brush, pts, FillMode.Alternate);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode)
		{
			PointF[] pts = Spline2Bez(Point2PointF(points), 0, points.Length - 1, true, .5f);
			FillBeziers(brush, pts, fillmode);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode, Single tension)
		{
			PointF[] pts = Spline2Bez(Point2PointF(points), 0, points.Length - 1, true, tension);
			FillBeziers(brush, pts, fillmode);
		}

		/// <summary>
		/// Not implemented, because GDI+ regions/paths are not emulated.
		/// </summary>
		public void FillRegion(Brush brush, Region region) { throw new SvgGdiNotImpl("FillRegion (Brush brush, Region region)"); }

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawString(String s, Font font, Brush brush, Single x, Single y)
		{
			DrawText(s, font, brush, new RectangleF(x, y, 0, 0), StringFormat.GenericDefault, true);
		}

		/// <summary>
		/// Implemented.
		/// <para>In general, DrawString functions work, but it is impossible to guarantee that an SVG renderer will have a certain font and draw it in the 
		/// same way as GDI+.
		/// </para>
		/// <para>
		/// SVG does not do word wrapping and SvgGdi does not emulate it yet (although clipping is working).  The plan is to wait till SVG 1.2 becomes available, since 1.2 contains text
		/// wrapping/flowing attributes.
		/// </para>
		/// </summary>
		public void DrawString(String s, Font font, Brush brush, PointF point)
		{
			DrawText(s, font, brush, new RectangleF(point.X, point.Y, 0, 0), StringFormat.GenericDefault, true);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawString(String s, Font font, Brush brush, Single x, Single y, StringFormat format)
		{
			DrawText(s, font, brush, new RectangleF(x, y, 0, 0), format, true);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawString(String s, Font font, Brush brush, PointF point, StringFormat format)
		{
			DrawText(s, font, brush, new RectangleF(point.X, point.Y, 0, 0), StringFormat.GenericDefault, true);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawString(String s, Font font, Brush brush, RectangleF layoutRectangle)
		{
			DrawText(s, font, brush, layoutRectangle, StringFormat.GenericDefault, false);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawString(String s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format)
		{
			DrawText(s, font, brush, layoutRectangle, format, false);
		}

		private void DrawText(String s, Font font, Brush brush, RectangleF rect, StringFormat fmt, bool ignoreRect)
		{
			SvgTextElement txt = new SvgTextElement(s, rect.X, rect.Y);

			//GDI takes x and y as the upper left corner; svg takes them as the lower left.
			//We must therefore move the text one line down, but SVG does not understand about lines,
			//so we do as best we can, applying a downward translation before the current GDI translation.

			txt.Transform = new SvgTransformList(_transforms.Result.Clone());

			txt.Style = HandleBrush(brush);
			txt.Style += new SvgStyle(font);

			if (fmt.Alignment == StringAlignment.Center) {
				txt.Style.Set("text-anchor", "middle");
				txt.X = rect.X + rect.Width / 2;
			}

			if (fmt.Alignment == StringAlignment.Far) {
				txt.Style.Set("text-anchor", "end");
				txt.X = rect.Right;
			}

			txt.Style.Set("baseline-shift", "-86%");//a guess.

			if (!ignoreRect && (fmt.FormatFlags != StringFormatFlags.NoClip)) {
				SvgClipPathElement clipper = new SvgClipPathElement();
				clipper.Id += "_text_clipper";
				SvgRectElement rc = new SvgRectElement(rect.X, rect.Y, rect.Width, rect.Height);
				clipper.AddChild(rc);
				_defs.AddChild(clipper);

				txt.Style.Set("clip-path", new SvgUriReference(clipper));
			}


			_cur.AddChild(txt);
		}

		/// <summary>
		/// This method is implemented and produces a result which is often correct, but it is impossible to guarantee because 'MeasureString' is a fundamentally inapplicable to device independent output like SVG. 
		/// </summary>
		public System.Drawing.SizeF MeasureString(String text, Font font, SizeF layoutArea, StringFormat stringFormat, out int charactersFitted, out int linesFilled)
		{
			return GetDefaultGraphics().MeasureString(text, font, layoutArea, stringFormat, out charactersFitted, out linesFilled);
		}
		/// <summary>
		/// This method is implemented and produces a result which is often correct, but it is impossible to guarantee because 'MeasureString' is a fundamentally inapplicable to device independent output like SVG. 
		/// </summary>
		public System.Drawing.SizeF MeasureString(String text, Font font, PointF origin, StringFormat stringFormat)
		{
			return GetDefaultGraphics().MeasureString(text, font, origin, stringFormat);
		}
		/// <summary>
		/// This method is implemented and produces a result which is often correct, but it is impossible to guarantee because 'MeasureString' is a fundamentally inapplicable to device independent output like SVG. 
		/// </summary>
		public System.Drawing.SizeF MeasureString(String text, Font font, SizeF layoutArea)
		{
			return GetDefaultGraphics().MeasureString(text, font, layoutArea);
		}
		/// <summary>
		/// This method is implemented and produces a result which is often correct, but it is impossible to guarantee because 'MeasureString' is a fundamentally inapplicable to device independent output like SVG. 
		/// </summary>
		public System.Drawing.SizeF MeasureString(String text, Font font, SizeF layoutArea, StringFormat stringFormat)
		{
			return GetDefaultGraphics().MeasureString(text, font, layoutArea, stringFormat);
		}
		/// <summary>
		///  This method is implemented and produces a result which is often correct, but it is impossible to guarantee because 'MeasureString' is a fundamentally inapplicable to device independent output like SVG. 
		/// </summary>
		public System.Drawing.SizeF MeasureString(String text, Font font)
		{
			return GetDefaultGraphics().MeasureString(text, font);
		}
		/// <summary>
		///  This method is implemented and produces a result which is often correct, but it is impossible to guarantee because 'MeasureString' is a fundamentally inapplicable to device independent output like SVG. 
		/// </summary>
		public System.Drawing.SizeF MeasureString(String text, Font font, Int32 width)
		{
			return GetDefaultGraphics().MeasureString(text, font, width);
		}
		/// <summary>
		/// This method is implemented and produces a result which is often correct, but it is impossible to guarantee because 'MeasureString' is a fundamentally inapplicable to device independent output like SVG. 
		/// </summary>
		public System.Drawing.SizeF MeasureString(String text, Font font, Int32 width, StringFormat format)
		{
			return GetDefaultGraphics().MeasureString(text, font, width, format);
		}
		/// <summary>
		///  This method is implemented and produces a result which is often correct, but it is impossible to guarantee because 'MeasureString' is a fundamentally inapplicable to device independent output like SVG. 
		/// </summary>
		public System.Drawing.Region[] MeasureCharacterRanges(String text, Font font, RectangleF layoutRect, StringFormat stringFormat)
		{
			return GetDefaultGraphics().MeasureCharacterRanges(text, font, layoutRect, stringFormat);
		}


		/// <summary>
		/// Implemented.  The <c>DrawIcon</c> group of functions emulate drawing a bitmap by creating many SVG <c>rect</c> elements.  This is quite effective but
		/// can lead to a very big SVG file.  Alpha and stretching are handled correctly.  No antialiasing is done.
		/// </summary>
		public void DrawIcon(Icon icon, Int32 x, Int32 y)
		{
			Bitmap bmp = icon.ToBitmap();
			DrawBitmapData(bmp, x, y, icon.Width, icon.Height, false);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawIcon(Icon icon, Rectangle targetRect)
		{
			Bitmap bmp = icon.ToBitmap();
			DrawBitmapData(bmp, targetRect.X, targetRect.Y, targetRect.Width, targetRect.Height, true);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawIconUnstretched(Icon icon, Rectangle targetRect)
		{
			Bitmap bmp = icon.ToBitmap();
			DrawBitmapData(bmp, targetRect.X, targetRect.Y, targetRect.Width, targetRect.Height, false);
		}


		/// <summary>
		/// Implemented.  The <c>DrawImage</c> group of functions emulate drawing a bitmap by creating many SVG <c>rect</c> elements.  This is quite effective but
		/// can lead to a very big SVG file.  Alpha and stretching are handled correctly.  No antialiasing is done.
		/// <para>
		/// The GDI+ documentation suggests that the 'Unscaled' functions should truncate the image.  GDI+ does not actually do this, but <c>SvgGraphics</c> does.
		/// </para>
		/// </summary>
		public void DrawImage(Image image, PointF point)
		{
			DrawImage(image, point.X, point.Y);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawImage(Image image, Single x, Single y)
		{
			DrawImage(image, x, y, (Single)image.Width, (Single)image.Height);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawImage(Image image, RectangleF rect)
		{
			DrawImage(image, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawImage(Image image, Single x, Single y, Single width, Single height)
		{
			if (image.GetType() != typeof(Bitmap))
				return;
			DrawBitmapData((Bitmap)image, x, y, width, height, true);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawImage(Image image, Point point)
		{
			DrawImage(image, (Single)point.X, (Single)point.Y);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawImage(Image image, Int32 x, Int32 y)
		{
			DrawImage(image, (Single)x, (Single)y);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawImage(Image image, Rectangle rect)
		{
			DrawImage(image, (Single)rect.X, (Single)rect.Y, (Single)rect.Width, (Single)rect.Height);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawImage(Image image, Int32 x, Int32 y, Int32 width, Int32 height)
		{
			DrawImage(image, x, y, width, height);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawImageUnscaled(Image image, Point point)
		{
			DrawImage(image, (Single)point.X, (Single)point.Y);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawImageUnscaled(Image image, Int32 x, Int32 y)
		{
			DrawImage(image, (Single)x, (Single)y);
		}

		/// <summary>
		/// Implemented.  There seems to be a GDI bug in that the image is *not* clipped to the rectangle.  We do clip it.
		/// </summary>
		public void DrawImageUnscaled(Image image, Rectangle rect)
		{
			DrawImageUnscaled(image, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>
		/// Implemented
		/// </summary>
		public void DrawImageUnscaled(Image image, Int32 x, Int32 y, Int32 width, Int32 height)
		{
			if (image.GetType() != typeof(Bitmap))
				return;
			DrawBitmapData((Bitmap)image, x, y, width, height, false);
		}

		/// <summary>
		/// Not implemented.
		/// </summary>
		public void DrawImage(Image image, PointF[] destPoints) { throw new SvgGdiNotImpl("DrawImage (Image image, PointF[] destPoints)"); }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public void DrawImage(Image image, Point[] destPoints) { throw new SvgGdiNotImpl("DrawImage (Image image, Point[] destPoints)"); }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public void DrawImage(Image image, Single x, Single y, RectangleF srcRect, GraphicsUnit srcUnit) { throw new SvgGdiNotImpl("DrawImage (Image image, Single x, Single y, RectangleF srcRect, GraphicsUnit srcUnit)"); }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public void DrawImage(Image image, Int32 x, Int32 y, Rectangle srcRect, GraphicsUnit srcUnit) { throw new SvgGdiNotImpl("DrawImage (Image image, Int32 x, Int32 y, Rectangle srcRect, GraphicsUnit srcUnit)"); }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit) { throw new SvgGdiNotImpl("DrawImage (Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit)"); }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public void DrawImage(Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit) { throw new SvgGdiNotImpl("DrawImage (Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit)"); }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit) { throw new SvgGdiNotImpl("DrawImage (Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit)"); }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr) { throw new SvgGdiNotImpl("DrawImage (Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)"); }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback) { throw new SvgGdiNotImpl("DrawImage (Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback)"); }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit) { throw new SvgGdiNotImpl("DrawImage (Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit)"); }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr) { throw new SvgGdiNotImpl("DrawImage (Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)"); }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public void DrawImage(Image image, Rectangle destRect, Single srcX, Single srcY, Single srcWidth, Single srcHeight, GraphicsUnit srcUnit) { throw new SvgGdiNotImpl("DrawImage (Image image, Rectangle destRect, Single srcX, Single srcY, Single srcWidth, Single srcHeight, GraphicsUnit srcUnit)"); }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public void DrawImage(Image image, Rectangle destRect, Single srcX, Single srcY, Single srcWidth, Single srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs) { throw new SvgGdiNotImpl("DrawImage (Image image, Rectangle destRect, Single srcX, Single srcY, Single srcWidth, Single srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs)"); }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public void DrawImage(Image image, Rectangle destRect, Int32 srcX, Int32 srcY, Int32 srcWidth, Int32 srcHeight, GraphicsUnit srcUnit) { throw new SvgGdiNotImpl("DrawImage (Image image, Rectangle destRect, Int32 srcX, Int32 srcY, Int32 srcWidth, Int32 srcHeight, GraphicsUnit srcUnit)"); }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public void DrawImage(Image image, Rectangle destRect, Int32 srcX, Int32 srcY, Int32 srcWidth, Int32 srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr) { throw new SvgGdiNotImpl("DrawImage (Image image, Rectangle destRect, Int32 srcX, Int32 srcY, Int32 srcWidth, Int32 srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr)"); }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public void SetClip(Graphics g) { throw new SvgGdiNotImpl("SetClip (Graphics g)"); }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public void SetClip(Graphics g, CombineMode combineMode) { throw new SvgGdiNotImpl("SetClip (Graphics g, CombineMode combineMode)"); }
		/// <summary>
		/// Implemented.
		/// </summary>
		public void SetClip(Rectangle rect)
		{
			SetClip(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height));
		}

		/// <summary>
		/// Not implemented.
		/// </summary>
		public void SetClip(Rectangle rect, CombineMode combineMode) { throw new SvgGdiNotImpl("SetClip (Rectangle rect, CombineMode combineMode)"); }

		/// <summary>
		/// Implemented.
		/// </summary>
		public void SetClip(RectangleF rect)
		{
			SvgClipPathElement clipper = new SvgClipPathElement();
			clipper.Id += "_SetClip";
			SvgRectElement rc = new SvgRectElement(rect.X, rect.Y, rect.Width, rect.Height);
			clipper.AddChild(rc);
			_defs.AddChild(clipper);

			_cur.Style.Set("clip-path", new SvgUriReference(clipper));
		}

		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
		/// </summary>
		public void SetClip(RectangleF rect, CombineMode combineMode) { throw new SvgGdiNotImpl("SetClip (RectangleF rect, CombineMode combineMode)"); }
		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
		/// </summary>
		public void SetClip(GraphicsPath path) { throw new SvgGdiNotImpl("SetClip (GraphicsPath path)"); }
		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
		/// </summary>
		public void SetClip(GraphicsPath path, CombineMode combineMode) { throw new SvgGdiNotImpl("SetClip (GraphicsPath path, CombineMode combineMode)"); }
		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
		/// </summary>
		public void SetClip(Region region, CombineMode combineMode) { throw new SvgGdiNotImpl("SetClip (Region region, CombineMode combineMode)"); }

		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
		/// </summary>
		public void IntersectClip(Rectangle rect) { throw new SvgGdiNotImpl("IntersectClip (Rectangle rect)"); }
		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
		/// </summary>
		public void IntersectClip(RectangleF rect) { throw new SvgGdiNotImpl("IntersectClip (RectangleF rect)"); }
		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
		/// </summary>
		public void IntersectClip(Region region) { throw new SvgGdiNotImpl("IntersectClip (Region region)"); }
		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
		/// </summary>
		public void ExcludeClip(Rectangle rect) { throw new SvgGdiNotImpl("ExcludeClip (Rectangle rect)"); }
		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
		/// </summary>
		public void ExcludeClip(Region region) { throw new SvgGdiNotImpl("ExcludeClip (Region region)"); }

		/// <summary>
		/// Implemented.
		/// </summary>
		public void ResetClip()
		{
			_cur.Style.Set("clip-path", null);
		}

		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
		/// </summary>
		public void TranslateClip(Single dx, Single dy) { throw new SvgGdiNotImpl("TranslateClip (Single dx, Single dy)"); }
		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
		/// </summary>
		public void TranslateClip(Int32 dx, Int32 dy) { throw new SvgGdiNotImpl("TranslateClip (Int32 dx, Int32 dy)"); }

		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
		/// </summary>
		public System.Boolean IsVisible(Int32 x, Int32 y) { throw new SvgGdiNotImpl("IsVisible (Int32 x, Int32 y)"); }
		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
		/// </summary>
		public System.Boolean IsVisible(Point point) { throw new SvgGdiNotImpl("IsVisible (Point point)"); }
		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
		/// </summary>
		public System.Boolean IsVisible(Single x, Single y) { throw new SvgGdiNotImpl("IsVisible (Single x, Single y)"); }
		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
		/// </summary>
		public System.Boolean IsVisible(PointF point) { throw new SvgGdiNotImpl("IsVisible (PointF point)"); }
		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
		/// </summary>
		public System.Boolean IsVisible(Int32 x, Int32 y, Int32 width, Int32 height) { throw new SvgGdiNotImpl("IsVisible (Int32 x, Int32 y, Int32 width, Int32 height)"); }
		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
		/// </summary>
		public System.Boolean IsVisible(Rectangle rect) { throw new SvgGdiNotImpl("IsVisible (Rectangle rect)"); }
		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
		/// </summary>
		public System.Boolean IsVisible(Single x, Single y, Single width, Single height) { throw new SvgGdiNotImpl("IsVisible (Single x, Single y, Single width, Single height)"); }
		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
		/// </summary>
		public System.Boolean IsVisible(RectangleF rect) { throw new SvgGdiNotImpl("IsVisible (RectangleF rect)"); }
		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
		/// </summary>
		public System.Drawing.Drawing2D.GraphicsState Save() { throw new SvgGdiNotImpl("Save ()"); }
		/// <summary>
		/// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
		/// </summary>
		public void Restore(GraphicsState gstate) { throw new SvgGdiNotImpl("Restore (GraphicsState gstate)"); }

		/// <summary>
		/// Not implemented.
		/// </summary>
		public System.Drawing.Drawing2D.GraphicsContainer BeginContainer(RectangleF dstrect, RectangleF srcrect, GraphicsUnit unit) { throw new SvgGdiNotImpl("BeginContainer (RectangleF dstrect, RectangleF srcrect, GraphicsUnit unit)"); }

		/// <summary>
		/// Implemented, but returns null as SVG has a proper scenegraph, unlike GDI+.  The effect of calling <c>BeginContainer</c> is to create a new SVG group
		/// and apply transformations etc to produce the effect that a GDI+ container would produce.
		/// </summary>
		public System.Drawing.Drawing2D.GraphicsContainer BeginContainer()
		{
			SvgGroupElement gr = new SvgGroupElement();
			_cur.AddChild(gr);
			_cur = gr;
			_cur.Id += "_BeginContainer";
			_transforms.Push();
			return null;
		}

		/// <summary>
		/// The effect of calling this method is to pop out of the closest SVG group.  This simulates restoring GDI+ state from a <c>GraphicsContainer</c>
		/// </summary>
		public void EndContainer(GraphicsContainer container)
		{
			if (_cur == _topgroup)
				return;

			_cur = (SvgStyledTransformedElement)_cur.Parent;

			_transforms.Pop();
		}

		/// <summary>
		/// Not implemented.
		/// </summary>
		public System.Drawing.Drawing2D.GraphicsContainer BeginContainer(Rectangle dstrect, Rectangle srcrect, GraphicsUnit unit) { throw new SvgGdiNotImpl("BeginContainer (Rectangle dstrect, Rectangle srcrect, GraphicsUnit unit)"); }

		/// <summary>
		/// Does nothing.  Should perhaps insert a comment into the SVG XML output, but is this really analogous
		/// to a metafile comment.
		/// </summary>
		public void AddMetafileComment(Byte[] data)
		{
			//probably should add xml comment
		}

		/// <summary>
		/// Get is not implemented (throws an exception).  Set does nothing.
		/// </summary>
		public System.Drawing.Drawing2D.CompositingMode CompositingMode
		{ get { throw new SvgGdiNotImpl("get_CompositingMode"); } set { } }

		/// <summary>
		/// Get is not implemented (throws an exception).  Set does nothing.
		/// </summary>
		public System.Drawing.Point RenderingOrigin
		{ get { throw new SvgGdiNotImpl("get_RenderingOrigin"); } set { } }

		/// <summary>
		/// Get is not implemented (throws an exception).  Set does nothing.
		/// </summary>
		public System.Drawing.Drawing2D.CompositingQuality CompositingQuality
		{ get { throw new SvgGdiNotImpl("get_CompositingQuality"); } set { } }

		/// <summary>
		/// Get is not implemented (throws an exception).
		/// </summary>
		public System.Drawing.Text.TextRenderingHint TextRenderingHint
		{
			get { throw new SvgGdiNotImpl("get_TextRenderingHint"); }
			set
			{
				switch (value) {
					case TextRenderingHint.AntiAlias:
						_cur.Style.Set("text-rendering", "auto"); break;
					case TextRenderingHint.AntiAliasGridFit:
						_cur.Style.Set("text-rendering", "auto"); break;
					case TextRenderingHint.ClearTypeGridFit:
						_cur.Style.Set("text-rendering", "geometricPrecision"); break;

					default:
						_cur.Style.Set("text-rendering", "crispEdges"); break;
				}

			}
		}

		/// <summary>
		/// Get is not implemented (throws an exception).
		/// </summary>
		public System.Int32 TextContrast
		{ get { throw new SvgGdiNotImpl("get_TextContrast"); } set { } }

		public System.Drawing.Drawing2D.SmoothingMode SmoothingMode
		{
			get { throw new SvgGdiNotImpl("get_SmoothingMode"); }
			set
			{
				switch (value) {
					case SmoothingMode.None:
						_cur.Style.Set("shape-rendering", "crispEdges"); break;
					case SmoothingMode.Default:
						_cur.Style.Set("shape-rendering", "crispEdges"); break;
					case SmoothingMode.HighSpeed:
						_cur.Style.Set("shape-rendering", "optimizeSpeed"); break;
					case SmoothingMode.AntiAlias:
						_cur.Style.Set("shape-rendering", "auto"); break;
					case SmoothingMode.HighQuality:
						_cur.Style.Set("shape-rendering", "geometricPrecision"); break;

					default:
						_cur.Style.Set("shape-rendering", "auto"); break;
				}
			}
		}

		/// <summary>
		/// Get is not implemented (throws an exception).  Set does nothing.
		/// </summary>
		public System.Drawing.Drawing2D.PixelOffsetMode PixelOffsetMode
		{ get { throw new SvgGdiNotImpl("get_PixelOffsetMode"); } set { } }

		/// <summary>
		/// Get is not implemented (throws an exception).  Set does nothing.
		/// </summary>
		public System.Drawing.Drawing2D.InterpolationMode InterpolationMode
		{ get { throw new SvgGdiNotImpl("get_InterpolationMode"); } set { } }

		public System.Drawing.Drawing2D.Matrix Transform
		{
			get
			{
				return _transforms.Result.Clone();
			}

			set
			{
				_transforms.Top = value;
			}
		}

		/// <summary>
		/// Get is not implemented (throws an exception).  Set does nothing.
		/// </summary>
		public System.Drawing.GraphicsUnit PageUnit
		{ get { throw new SvgGdiNotImpl("PageUnit"); } set { } }

		/// <summary>
		/// Get is not implemented (throws an exception).  Set does nothing.
		/// </summary>
		public System.Single PageScale
		{ get { throw new SvgGdiNotImpl("PageScale"); } set { } }

		/// <summary>
		/// Not implemented.
		/// </summary>
		public System.Single DpiX { get { throw new SvgGdiNotImpl("DpiX"); } }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public System.Single DpiY { get { throw new SvgGdiNotImpl("DpiY"); } }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public System.Drawing.Region Clip { get { throw new SvgGdiNotImpl("Clip"); } set { throw new SvgGdiNotImpl("Clip"); } }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public System.Drawing.RectangleF ClipBounds { get { throw new SvgGdiNotImpl("ClipBounds"); } }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public System.Boolean IsClipEmpty { get { throw new SvgGdiNotImpl("IsClipEmpty"); } }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public System.Drawing.RectangleF VisibleClipBounds { get { throw new SvgGdiNotImpl("VisibleClipBounds"); } }
		/// <summary>
		/// Not implemented.
		/// </summary>
		public System.Boolean IsVisibleClipEmpty { get { throw new SvgGdiNotImpl("IsVisibleClipEmpty"); } }



		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////


		private static Graphics GetDefaultGraphics()
		{
			if (_g == null) {
				Bitmap b = new Bitmap(1, 1);
				_g = Graphics.FromImage(b);
			}

			return _g;
		}

		/// <summary>
		/// When a GDI instruction with a brush parameter is called, there can be a lot we have to do to emulate the brush.  The aim is to return a 
		/// style that represents the brush.
		/// <para>
		/// Solid brush is very easy.
		/// </para>
		/// <para>
		/// Linear grad brush:  we ignore the blend curve and the transformation (and therefore the rotation parameter if any)
		/// Hatch brush: 
		/// </para>
		/// <para>
		/// Other types of brushes are too hard to emulate and are rendered pink.
		/// </para>
		/// </summary>
		/// <param name="br"></param>
		/// <returns></returns>
		private SvgStyle HandleBrush(Brush br)
		{
			if (br.GetType() == typeof(SolidBrush)) {
				return new SvgStyle((SolidBrush)br);
			}

			if (br.GetType() == typeof(LinearGradientBrush)) {
				LinearGradientBrush grbr = (LinearGradientBrush)br;
				RectangleF rc = grbr.Rectangle;

				SvgLinearGradient grad = new SvgLinearGradient(rc.Left, rc.Top, rc.Right, rc.Bottom);


				switch (grbr.WrapMode) {
					//I have not been able to test Clamp because using a clamped gradient appears to crash the process
					//under XP (?!?!)
					case WrapMode.Clamp:
						grad.SpreadMethod = "pad"; grad.GradientUnits = "objectBoundingBox"; break;
					case WrapMode.Tile:
						grad.SpreadMethod = "repeat"; grad.GradientUnits = "userSpaceOnUse"; break;
					default:
						grad.SpreadMethod = "reflect"; grad.GradientUnits = "userSpaceOnUse"; break;
				}

				ColorBlend cb = null;

				//GDI dll tends to crash when you try and access some members of gradient brushes that haven't been specified.
				try {
					cb = grbr.InterpolationColors;
				} catch (Exception) { }

				if (cb != null) {
					for (int i = 0; i < grbr.InterpolationColors.Colors.Length; ++i) {
						grad.AddChild(new SvgStopElement(grbr.InterpolationColors.Positions[i], grbr.InterpolationColors.Colors[i]));
					}
				} else {
					grad.AddChild(new SvgStopElement("0%", grbr.LinearColors[0]));
					grad.AddChild(new SvgStopElement("100%", grbr.LinearColors[1]));
				}

				grad.Id += "_LinearGradientBrush";

				_defs.AddChild(grad);

				SvgStyle s = new SvgStyle();
				s.Set("fill", new SvgUriReference(grad));
				return s;
			}

			if (br.GetType() == typeof(HatchBrush)) {
				HatchBrush habr = (HatchBrush)br;

				SvgPatternElement patty = new SvgPatternElement(0, 0, 8, 8, new SvgNumList("4 4 12 12"));
				patty.Style.Set("shape-rendering", "crispEdges");
				patty.Style.Set("stroke-linecap", "butt");

				SvgRectElement rc = new SvgRectElement(0, 0, 8, 8);
				rc.Style.Set("fill", new SvgColor(habr.BackgroundColor));
				patty.AddChild(rc);

				AddHatchBrushDetails(patty, new SvgColor(habr.ForegroundColor), habr.HatchStyle);

				patty.Id += "_HatchBrush";
				patty.PatternUnits = "userSpaceOnUse";
				patty.PatternContentUnits = "userSpaceOnUse";
				_defs.AddChild(patty);

				SvgStyle s = new SvgStyle();
				s.Set("fill", new SvgUriReference(patty));
				return s;
			}

			//most types of brush we can't emulate, but luckily they are quite unusual
			return new SvgStyle(new SolidBrush(Color.Salmon));
		}


		private void AddHatchBrushDetails(SvgPatternElement patty, SvgColor col, HatchStyle hs)
		{
			SvgStyledTransformedElement l1 = null;
			SvgStyledTransformedElement l2 = null;
			SvgStyledTransformedElement l3 = null;
			SvgStyledTransformedElement l4 = null;

			switch (hs) {
				case HatchStyle.Cross:
					l1 = new SvgLineElement(4, 0, 4, 8);
					l2 = new SvgLineElement(0, 4, 8, 4);
					break;

				case HatchStyle.BackwardDiagonal:
					l1 = new SvgLineElement(8, 0, 0, 8);
					break;

				case HatchStyle.LightDownwardDiagonal:
				case HatchStyle.DarkDownwardDiagonal:
					l1 = new SvgLineElement(4, 0, 8, 4);
					l2 = new SvgLineElement(0, 4, 4, 8);
					l3 = new SvgLineElement(0, 0, 8, 8);
					break;
				case HatchStyle.LightHorizontal:
				case HatchStyle.DarkHorizontal:
					l1 = new SvgLineElement(0, 2, 8, 2);
					l2 = new SvgLineElement(0, 6, 8, 6);
					break;
				case HatchStyle.LightUpwardDiagonal:
				case HatchStyle.DarkUpwardDiagonal:
					l1 = new SvgLineElement(0, 4, 4, 0);
					l2 = new SvgLineElement(4, 8, 8, 4);
					l3 = new SvgLineElement(0, 8, 8, 0);
					break;
				case HatchStyle.LightVertical:
				case HatchStyle.DarkVertical:
					l1 = new SvgLineElement(2, 0, 2, 8);
					l2 = new SvgLineElement(6, 0, 6, 8);
					break;
				case HatchStyle.DashedDownwardDiagonal:
					l1 = new SvgLineElement(0, 0, 4, 4);
					l2 = new SvgLineElement(4, 0, 8, 4);
					break;
				case HatchStyle.DashedHorizontal:
					l1 = new SvgLineElement(0, 2, 4, 2);
					l2 = new SvgLineElement(4, 6, 8, 6);
					break;
				case HatchStyle.DashedUpwardDiagonal:
					l1 = new SvgLineElement(4, 0, 0, 4);
					l2 = new SvgLineElement(8, 0, 4, 4);
					break;
				case HatchStyle.DashedVertical:
					l1 = new SvgLineElement(2, 0, 2, 4);
					l2 = new SvgLineElement(6, 4, 6, 8);
					break;
				case HatchStyle.DiagonalBrick:
					l1 = new SvgLineElement(0, 8, 8, 0);
					l2 = new SvgLineElement(0, 0, 4, 4);
					break;
				case HatchStyle.DiagonalCross:
					l1 = new SvgLineElement(0, 0, 8, 8);
					l2 = new SvgLineElement(8, 0, 0, 8);
					break;
				case HatchStyle.Divot:
					l1 = new SvgLineElement(2, 2, 4, 4);
					l2 = new SvgLineElement(4, 4, 2, 6);
					break;
				case HatchStyle.DottedDiamond:
					l1 = new SvgLineElement(0, 0, 8, 8);
					l2 = new SvgLineElement(0, 8, 8, 0);
					break;
				case HatchStyle.DottedGrid:
					l1 = new SvgLineElement(4, 0, 4, 8);
					l2 = new SvgLineElement(0, 4, 8, 4);
					break;
				case HatchStyle.ForwardDiagonal:
					l1 = new SvgLineElement(0, 0, 8, 8);
					break;
				case HatchStyle.Horizontal:
					l1 = new SvgLineElement(0, 4, 8, 4);
					break;
				case HatchStyle.HorizontalBrick:
					l1 = new SvgLineElement(0, 3, 8, 3);
					l2 = new SvgLineElement(3, 0, 3, 3);
					l3 = new SvgLineElement(0, 3, 0, 7);
					l4 = new SvgLineElement(0, 7, 7, 7);
					break;
				case HatchStyle.LargeCheckerBoard:
					l1 = new SvgRectElement(0, 0, 3f, 3f);
					l2 = new SvgRectElement(4, 4, 4, 4f);
					break;
				case HatchStyle.LargeConfetti:
					l1 = new SvgRectElement(0, 0, 1, 1);
					l2 = new SvgRectElement(2, 3, 1, 1);
					l3 = new SvgRectElement(5, 2, 1, 1);
					l4 = new SvgRectElement(6, 6, 1, 1);
					break;
				case HatchStyle.NarrowHorizontal:
					l1 = new SvgLineElement(0, 1, 8, 1);
					l2 = new SvgLineElement(0, 3, 8, 3);
					l3 = new SvgLineElement(0, 5, 8, 5);
					l4 = new SvgLineElement(0, 7, 8, 7);
					break;
				case HatchStyle.NarrowVertical:
					l1 = new SvgLineElement(1, 0, 1, 8);
					l2 = new SvgLineElement(3, 0, 3, 8);
					l3 = new SvgLineElement(5, 0, 5, 8);
					l4 = new SvgLineElement(7, 0, 7, 8);
					break;
				case HatchStyle.OutlinedDiamond:
					l1 = new SvgLineElement(0, 0, 8, 8);
					l2 = new SvgLineElement(8, 0, 0, 8);
					break;
				case HatchStyle.Plaid:
					l1 = new SvgLineElement(0, 0, 8, 0);
					l2 = new SvgLineElement(0, 3, 8, 3);
					l3 = new SvgRectElement(0, 4, 3, 3);
					break;
				case HatchStyle.Shingle:
					l1 = new SvgLineElement(0, 2, 2, 0);
					l2 = new SvgLineElement(2, 0, 7, 5);
					l3 = new SvgLineElement(0, 3, 3, 7);
					break;
				case HatchStyle.SmallCheckerBoard:
					l1 = new SvgRectElement(0, 0, 1, 1);
					l2 = new SvgRectElement(4, 4, 1, 1);
					l3 = new SvgRectElement(4, 0, 1, 1);
					l4 = new SvgRectElement(0, 4, 1, 1);
					break;
				case HatchStyle.SmallConfetti:
					l1 = new SvgLineElement(0, 0, 2, 2);
					l2 = new SvgLineElement(7, 3, 5, 5);
					l3 = new SvgLineElement(2, 6, 4, 4);
					break;
				case HatchStyle.SmallGrid:
					l1 = new SvgLineElement(0, 2, 8, 2);
					l2 = new SvgLineElement(0, 6, 8, 6);
					l3 = new SvgLineElement(2, 0, 2, 8);
					l4 = new SvgLineElement(6, 0, 6, 8);
					break;
				case HatchStyle.SolidDiamond:
					l1 = new SvgPolygonElement("3 0 6 3 3 6 0 3");
					break;
				case HatchStyle.Sphere:
					l1 = new SvgEllipseElement(3, 3, 2, 2);
					break;
				case HatchStyle.Trellis:
					l1 = new SvgLineElement(0, 1, 8, 1);
					l2 = new SvgLineElement(0, 3, 8, 3);
					l3 = new SvgLineElement(0, 5, 8, 5);
					l4 = new SvgLineElement(0, 7, 8, 7);
					break;
				case HatchStyle.Vertical:
					l4 = new SvgLineElement(0, 0, 0, 8);
					break;
				case HatchStyle.Wave:
					l3 = new SvgLineElement(0, 4, 3, 2);
					l4 = new SvgLineElement(3, 2, 8, 4);
					break;
				case HatchStyle.Weave:
					l1 = new SvgLineElement(0, 4, 4, 0);
					l2 = new SvgLineElement(8, 4, 4, 8);
					l3 = new SvgLineElement(0, 0, 0, 4);
					l4 = new SvgLineElement(0, 4, 4, 8);
					break;
				case HatchStyle.WideDownwardDiagonal:
					l1 = new SvgLineElement(0, 0, 8, 8);
					l2 = new SvgLineElement(0, 1, 8, 9);
					l3 = new SvgLineElement(7, 0, 8, 1);
					break;
				case HatchStyle.WideUpwardDiagonal:
					l1 = new SvgLineElement(8, 0, 0, 8);
					l2 = new SvgLineElement(8, 1, 0, 9);
					l3 = new SvgLineElement(0, 1, -1, 0);
					break;
				case HatchStyle.ZigZag:
					l1 = new SvgLineElement(0, 4, 4, 0);
					l2 = new SvgLineElement(4, 0, 8, 4);
					break;

				case HatchStyle.Percent05:
					l1 = new SvgLineElement(0, 0, 1, 0);
					l2 = new SvgLineElement(4, 4, 5, 4);
					break;
				case HatchStyle.Percent10:
					l1 = new SvgLineElement(0, 0, 1, 0);
					l2 = new SvgLineElement(4, 2, 5, 2);
					l3 = new SvgLineElement(2, 4, 3, 4);
					l4 = new SvgLineElement(6, 6, 7, 6);
					break;
				case HatchStyle.Percent20:
					l1 = new SvgLineElement(0, 0, 2, 0);
					l2 = new SvgLineElement(4, 2, 6, 2);
					l3 = new SvgLineElement(2, 4, 4, 4);
					l4 = new SvgLineElement(5, 6, 7, 6);
					break;
				case HatchStyle.Percent25:
					l1 = new SvgLineElement(0, 0, 3, 0);
					l2 = new SvgLineElement(4, 2, 6, 2);
					l3 = new SvgLineElement(2, 4, 5, 4);
					l4 = new SvgLineElement(5, 6, 7, 6);
					break;
				case HatchStyle.Percent30:
					l1 = new SvgRectElement(0, 0, 3, 1);
					l2 = new SvgLineElement(4, 2, 6, 2);
					l3 = new SvgRectElement(2, 4, 3, 1);
					l4 = new SvgLineElement(5, 6, 7, 6);
					break;
				case HatchStyle.Percent40:
					l1 = new SvgRectElement(0, 0, 3, 1);
					l2 = new SvgRectElement(4, 2, 3, 1);
					l3 = new SvgRectElement(2, 4, 3, 1);
					l4 = new SvgRectElement(5, 6, 3, 1);
					break;
				case HatchStyle.Percent50:
					l1 = new SvgRectElement(0, 0, 3, 3);
					l2 = new SvgRectElement(4, 4, 4, 4f);
					break;
				case HatchStyle.Percent60:
					l1 = new SvgRectElement(0, 0, 4, 3);
					l2 = new SvgRectElement(4, 4, 4, 4f);
					break;
				case HatchStyle.Percent70:
					l1 = new SvgRectElement(0, 0, 4, 5);
					l2 = new SvgRectElement(4, 4, 4, 4f);
					break;
				case HatchStyle.Percent75:
					l1 = new SvgRectElement(0, 0, 7, 3);
					l2 = new SvgRectElement(0, 2, 3, 7);
					break;
				case HatchStyle.Percent80:
					l1 = new SvgRectElement(0, 0, 7, 4);
					l2 = new SvgRectElement(0, 2, 4, 7);
					break;
				case HatchStyle.Percent90:
					l1 = new SvgRectElement(0, 0, 7, 5);
					l2 = new SvgRectElement(0, 2, 5, 7);
					break;

				default:

					break;
			}

			if (l1 != null) {
				l1.Style.Set("stroke", col);
				l1.Style.Set("fill", col);
				patty.AddChild(l1);
			}
			if (l2 != null) {
				l2.Style.Set("stroke", col);
				l2.Style.Set("fill", col);
				patty.AddChild(l2);
			}
			if (l3 != null) {
				l3.Style.Set("stroke", col);
				l3.Style.Set("fill", col);
				patty.AddChild(l3);
			}
			if (l4 != null) {
				l4.Style.Set("stroke", col);
				l4.Style.Set("fill", col);
				patty.AddChild(l4);
			}
		}


		private void DrawEndAnchors(Pen pen, PointF start, PointF end)
		{
			float startAngle = (float)Math.Atan((start.X - end.X) / (start.Y - end.Y)) * -1;
			float endAngle = (float)Math.Atan((end.X - start.X) / (end.Y - start.Y)) * -1;

			CustomLineCap clcstart = null;
			CustomLineCap clcend = null;

			//GDI+ native dll throws an exception if someone forgot to specify custom cap
			try {
				clcstart = pen.CustomStartCap;
				clcend = pen.CustomEndCap;
			} catch (Exception) {
			}

			DrawEndAnchor(pen.StartCap, clcstart, pen.Color, pen.Width, start, startAngle);
			DrawEndAnchor(pen.EndCap, clcend, pen.Color, pen.Width, end, endAngle);
		}

		private void DrawEndAnchor(LineCap lc, CustomLineCap clc, Color col, float w, PointF pt, float angle)
		{
			SvgStyledTransformedElement anchor = null;
			PointF[] points = null;

			switch (lc) {
				case LineCap.ArrowAnchor:
					points = new PointF[3];
					points[0] = new PointF(0, -w / 2f);
					points[1] = new PointF(-w, w);
					points[2] = new PointF(w, w);
					anchor = new SvgPolygonElement(points);
					break;
				case LineCap.DiamondAnchor:
					points = new PointF[4];
					points[0] = new PointF(0, -w);
					points[1] = new PointF(w, 0);
					points[2] = new PointF(0, w);
					points[3] = new PointF(-w, 0);
					anchor = new SvgPolygonElement(points);
					break;
				case LineCap.RoundAnchor:
					anchor = new SvgEllipseElement(0, 0, w, w);
					break;
				case LineCap.SquareAnchor:
					float ww = (w / 3) * 2;
					anchor = new SvgRectElement(0 - ww, 0 - ww, ww * 2, ww * 2);
					break;
				case LineCap.Custom:
					//not implemented!
					return;

				default:
					return;
			}

			anchor.Id += "_line_anchor";
			anchor.Style.Set("fill", new SvgColor(col));
			anchor.Style.Set("stroke", "none");

			Matrix rotation = new Matrix();
			rotation.Rotate((angle / (float)Math.PI) * 180);
			Matrix translation = new Matrix();
			translation.Translate(pt.X, pt.Y);

			anchor.Transform = new SvgTransformList(_transforms.Result.Clone());
			anchor.Transform.Add(translation);
			anchor.Transform.Add(rotation);
			_cur.AddChild(anchor);
		}

		private void FillBeziers(Brush brush, PointF[] points, FillMode fillmode)
		{
			SvgPathElement bez = new SvgPathElement();

			string s = "M " + points[0].X.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " " + points[0].Y.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " C ";

			for (int i = 1; i < points.Length; ++i) {
				s += points[i].X.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " " + points[i].Y.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " ";
			}

			s += "Z";

			bez.D = s;

			bez.Style = HandleBrush(brush);
			bez.Transform = new SvgTransformList(_transforms.Result.Clone());
			_cur.AddChild(bez);
		}

		private void DrawBitmapData(Bitmap b, float x, float y, float w, float h, bool scale)
		{
			SvgGroupElement g = new SvgGroupElement("bitmap_at_" + x.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + "_" + y.ToString("F", System.Globalization.CultureInfo.InvariantCulture));

			float scalex = 1, scaley = 1;

			if (scale) {
				scalex = w / b.Width;
				scaley = h / b.Height;
			}

			for (int line = 0; line < b.Height; ++line) {
				for (int col = 0; col < b.Width; ++col) {
					//This is SO slow, but better than making the whole library 'unsafe'
					Color c = b.GetPixel(col, line);

					if (!scale) {
						if (col <= w && line <= h)
							DrawImagePixel(g, c, x + col, y + line, 1, 1);
					} else {
						DrawImagePixel(g, c, x + (col * scalex), y + (line * scaley), scalex, scaley);
					}
				}
			}

			if (!_transforms.Result.IsIdentity)
				g.Transform = _transforms.Result.Clone();
			_cur.AddChild(g);
		}

		private void DrawImagePixel(SvgElement container, Color c, float x, float y, float w, float h)
		{
			if (c.A == 0)
				return;

			SvgRectElement rc = new SvgRectElement(x, y, w, h);
			rc.Id = "";
			rc.Style.Set("fill", "rgb(" + c.R + "," + c.G + "," + c.B + ")");
			if (c.A < 255)
				rc.Style.Set("opacity", c.A / 255f);

			container.AddChild(rc);
		}


		private static PointF[] Point2PointF(Point[] p)
		{
			PointF[] pf = new PointF[p.Length];
			for (int i = 0; i < p.Length; ++i) {
				pf[i] = new PointF(p[i].X, p[i].Y);
			}

			return pf;
		}

		//This seems to be a very good approximation.  GDI must be using a similar simplistic method for some odd reason.
		//If a curve is closed, it uses all points, and ignores start and num.
		private static PointF[] Spline2Bez(PointF[] points, int start, int num, bool closed, float tension)
		{
			ArrayList cp = new ArrayList();
			ArrayList res = new ArrayList();

			int l = points.Length - 1;

			res.Add(points[0]);
			res.Add(ControlPoint(points[1], points[0], tension));

			for (int i = 1; i < l; ++i) {
				PointF[] pts = ControlPoints(points[i - 1], points[i + 1], points[i], tension);
				res.Add(pts[0]);
				res.Add(points[i]);
				res.Add(pts[1]);
			}

			res.Add(ControlPoint(points[l - 1], points[l], tension));
			res.Add(points[l]);


			if (closed) {
				//adjust rh cp of point 0
				PointF[] pts = ControlPoints(points[l], points[1], points[0], tension);
				res[1] = pts[1];

				//adjust lh cp of point l and add rh cp
				pts = ControlPoints(points[l - 1], points[0], points[l], tension);
				res[res.Count - 2] = pts[0];
				res.Add(pts[1]);

				//add new end point and its lh cp
				pts = ControlPoints(points[l], points[1], points[0], tension);
				res.Add(pts[0]);
				res.Add(points[0]);

				return (PointF[])res.ToArray(typeof(PointF));
			} else {
				ArrayList subset = new ArrayList();

				for (int i = start * 3; i < (start + num) * 3; ++i) {
					subset.Add(res[i]);
				}

				subset.Add(res[(start + num) * 3]);

				return (PointF[])subset.ToArray(typeof(PointF));
			}
		}

		private static PointF[] ControlPoints(PointF l, PointF r, PointF pt, float t)
		{
			//points to vectors
			PointF lv = new PointF(l.X - pt.X, l.Y - pt.Y);
			PointF rv = new PointF(r.X - pt.X, r.Y - pt.Y);

			PointF nlv = new PointF(lv.X - rv.X, lv.Y - rv.Y);
			PointF nrv = new PointF(rv.X - lv.X, rv.Y - lv.Y);

			float nlvlen = (float)Math.Sqrt(nlv.X * nlv.X + nlv.Y * nlv.Y);
			nlv.X /= (float)Math.Sqrt(nlvlen / (10 * t * t));
			nlv.Y /= (float)Math.Sqrt(nlvlen / (10 * t * t));

			float nrvlen = (float)Math.Sqrt(nrv.X * nrv.X + nrv.Y * nrv.Y);
			nrv.X /= (float)Math.Sqrt(nrvlen / (10 * t * t));
			nrv.Y /= (float)Math.Sqrt(nrvlen / (10 * t * t));

			PointF[] ret = new PointF[2];

			ret[0] = new PointF(pt.X + nlv.X, pt.Y + nlv.Y);
			ret[1] = new PointF(pt.X + nrv.X, pt.Y + nrv.Y);

			return ret;
		}

		private static PointF ControlPoint(PointF l, PointF pt, float t)
		{

			PointF v = new PointF(l.X - pt.X, l.Y - pt.Y);

			float vlen = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
			v.X /= (float)Math.Sqrt(vlen / (10 * t * t));
			v.Y /= (float)Math.Sqrt(vlen / (10 * t * t));

			return new PointF(pt.X + v.X, pt.Y + v.Y);
		}

		private static string GDIArc2SVGPath(float x, float y, float width, float height, float startAngle, float sweepAngle, bool pie)
		{
			int longArc = 0;

			PointF start = new PointF();
			PointF end = new PointF();
			PointF center = new PointF(x + width / 2f, y + height / 2f);

			startAngle = (startAngle / 360f) * 2f * (float)Math.PI;
			sweepAngle = (sweepAngle / 360f) * 2f * (float)Math.PI;

			sweepAngle += startAngle;

			if (sweepAngle > startAngle) {
				float tmp = startAngle;
				startAngle = sweepAngle;
				sweepAngle = tmp;
			}

			if (sweepAngle - startAngle > Math.PI || startAngle - sweepAngle > Math.PI) {
				longArc = 1;
			}

			start.X = (float)Math.Cos(startAngle) * (width / 2f) + center.X;
			start.Y = (float)Math.Sin(startAngle) * (height / 2f) + center.Y;

			end.X = (float)Math.Cos(sweepAngle) * (width / 2f) + center.X;
			end.Y = (float)Math.Sin(sweepAngle) * (height / 2f) + center.Y;


			string s = "M " + start.X.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + "," + start.Y.ToString("F", System.Globalization.CultureInfo.InvariantCulture) +
				" A " + (width / 2f).ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " " + (height / 2f).ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " " +
				"0 " + longArc.ToString() + " 0 " + end.X.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + " " + end.Y.ToString("F", System.Globalization.CultureInfo.InvariantCulture);

			if (pie) {
				s += " L " + center.X.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + "," + center.Y.ToString("F", System.Globalization.CultureInfo.InvariantCulture);
				s += " L " + start.X.ToString("F", System.Globalization.CultureInfo.InvariantCulture) + "," + start.Y.ToString("F", System.Globalization.CultureInfo.InvariantCulture);
			}

			return s;
		}


	}
}
