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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace SvgNet.SvgGdi
{
	/// <summary>
	/// An IGraphics implementation that simply passes every call through to a GDI+ <c>Graphics</c> object.
	/// </summary>
	public class GdiGraphics : IGraphics
	{
		private Graphics _g;

		public GdiGraphics(Graphics g)
		{
			_g = g;
		}

		public void Flush (){_g.Flush();}
		public void Flush (FlushIntention intention){_g.Flush(intention);}
		public void ResetTransform (){_g.ResetTransform() ;}
		public void MultiplyTransform (Matrix matrix){_g.MultiplyTransform(matrix) ;}
		public void MultiplyTransform (Matrix matrix, MatrixOrder order){_g.MultiplyTransform(matrix, order) ;}
		public void TranslateTransform (Single dx, Single dy){_g.TranslateTransform(dx, dy) ;}
		public void TranslateTransform (Single dx, Single dy, MatrixOrder order){_g.TranslateTransform(dx,dy, order) ;}
		public void ScaleTransform (Single sx, Single sy){_g.ScaleTransform(sx,sy) ;}
		public void ScaleTransform (Single sx, Single sy, MatrixOrder order){_g.ScaleTransform(sx,sy) ;}
		public void RotateTransform (Single angle){_g.RotateTransform(angle) ;}
		public void RotateTransform (Single angle, MatrixOrder order){_g.RotateTransform(angle, order) ;}
		public void TransformPoints (CoordinateSpace destSpace, CoordinateSpace srcSpace, PointF[] pts){_g.TransformPoints(destSpace, srcSpace, pts) ;}
		public void TransformPoints (CoordinateSpace destSpace, CoordinateSpace srcSpace, Point[] pts){_g.TransformPoints(destSpace, srcSpace, pts) ;}
		public System.Drawing.Color GetNearestColor (Color color){return _g.GetNearestColor(color) ;}
		public void DrawLine (Pen pen, Single x1, Single y1, Single x2, Single y2){_g.DrawLine(pen,x1,y1,x2,y2) ;}
		public void DrawLine (Pen pen, PointF pt1, PointF pt2){_g.DrawLine(pen, pt1, pt2) ;}
		public void DrawLines (Pen pen, PointF[] points){_g.DrawLines(pen, points) ;}
		public void DrawLine (Pen pen, Int32 x1, Int32 y1, Int32 x2, Int32 y2){_g.DrawLine(pen, x1, y1, x2, y2) ;}
		public void DrawLine (Pen pen, Point pt1, Point pt2){_g.DrawLine(pen, pt1, pt2) ;}
		public void DrawLines (Pen pen, Point[] points){_g.DrawLines(pen, points) ;}
		public void DrawArc (Pen pen, Single x, Single y, Single width, Single height, Single startAngle, Single sweepAngle){_g.DrawArc(pen, x,y,width,height,startAngle, sweepAngle) ;}
		public void DrawArc (Pen pen, RectangleF rect, Single startAngle, Single sweepAngle){_g.DrawArc(pen, rect, startAngle, sweepAngle) ;}
		public void DrawArc (Pen pen, Int32 x, Int32 y, Int32 width, Int32 height, Int32 startAngle, Int32 sweepAngle){_g.DrawArc(pen, x,y,width,height,startAngle,sweepAngle) ;}
		public void DrawArc (Pen pen, Rectangle rect, Single startAngle, Single sweepAngle){_g.DrawArc(pen, rect, startAngle, sweepAngle) ;}
		public void DrawBezier (Pen pen, Single x1, Single y1, Single x2, Single y2, Single x3, Single y3, Single x4, Single y4){_g.DrawBezier(pen,x1,y1,x2,y2,x3,y3,x4,y4) ;}
		public void DrawBezier (Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4){_g.DrawBezier(pen, pt1,pt2,pt3,pt4) ;}
		public void DrawBeziers (Pen pen, PointF[] points){_g.DrawBeziers(pen, points) ;}
		public void DrawBezier (Pen pen, Point pt1, Point pt2, Point pt3, Point pt4){_g.DrawBezier(pen, pt1,pt2,pt3,pt4) ;}
		public void DrawBeziers (Pen pen, Point[] points){_g.DrawBeziers(pen, points) ;}
		public void DrawRectangle (Pen pen, Rectangle rect){_g.DrawRectangle(pen, rect) ;}
		public void DrawRectangle (Pen pen, Single x, Single y, Single width, Single height){_g.DrawRectangle(pen, x,y,width,height) ;}
		public void DrawRectangles (Pen pen, RectangleF[] rects){_g.DrawRectangles(pen,rects) ;}
		public void DrawRectangle (Pen pen, Int32 x, Int32 y, Int32 width, Int32 height){_g.DrawRectangle(pen,x,y,width,height) ;}
		public void DrawRectangles (Pen pen, Rectangle[] rects){_g.DrawRectangles(pen, rects) ;}
		public void DrawEllipse (Pen pen, RectangleF rect){_g.DrawEllipse(pen, rect) ;}
		public void DrawEllipse (Pen pen, Single x, Single y, Single width, Single height){_g.DrawEllipse(pen,x,y,width,height) ;}
		public void DrawEllipse (Pen pen, Rectangle rect){_g.DrawEllipse(pen,rect) ;}
		public void DrawEllipse (Pen pen, Int32 x, Int32 y, Int32 width, Int32 height){_g.DrawEllipse(pen,x,y,width,height) ;}
		public void DrawPie (Pen pen, RectangleF rect, Single startAngle, Single sweepAngle){_g.DrawPie(pen,rect,startAngle,sweepAngle) ;}
		public void DrawPie (Pen pen, Single x, Single y, Single width, Single height, Single startAngle, Single sweepAngle){_g.DrawPie(pen,x,y,width,height,startAngle,sweepAngle) ;}
		public void DrawPie (Pen pen, Rectangle rect, Single startAngle, Single sweepAngle){_g.DrawPie(pen, rect, startAngle, sweepAngle) ;}
		public void DrawPie (Pen pen, Int32 x, Int32 y, Int32 width, Int32 height, Int32 startAngle, Int32 sweepAngle){_g.DrawPie(pen,x,y,width,height,startAngle,sweepAngle) ;}
		public void DrawPolygon (Pen pen, PointF[] points){_g.DrawPolygon(pen,points) ;}
		public void DrawPolygon (Pen pen, Point[] points){_g.DrawPolygon(pen,points) ;}
		public void DrawPath (Pen pen, GraphicsPath path){_g.DrawPath(pen, path) ;}
		public void DrawCurve (Pen pen, PointF[] points){_g.DrawCurve(pen, points) ;}
		public void DrawCurve (Pen pen, PointF[] points, Single tension){_g.DrawCurve(pen,points,tension) ;}
		public void DrawCurve (Pen pen, PointF[] points, Int32 offset, Int32 numberOfSegments){_g.DrawCurve(pen, points,offset,numberOfSegments) ;}
		public void DrawCurve (Pen pen, PointF[] points, Int32 offset, Int32 numberOfSegments, Single tension){_g.DrawCurve(pen, points,offset,numberOfSegments,tension) ;}
		public void DrawCurve (Pen pen, Point[] points){_g.DrawCurve(pen, points) ;}
		public void DrawCurve (Pen pen, Point[] points, Single tension){_g.DrawCurve(pen, points, tension) ;}
		public void DrawCurve (Pen pen, Point[] points, Int32 offset, Int32 numberOfSegments, Single tension){_g.DrawCurve(pen, points, offset, numberOfSegments, tension) ;}
		public void DrawClosedCurve (Pen pen, PointF[] points){_g.DrawClosedCurve(pen, points) ;}
		public void DrawClosedCurve (Pen pen, PointF[] points, Single tension, FillMode fillmode){_g.DrawClosedCurve(pen, points, tension, fillmode) ;}
		public void DrawClosedCurve (Pen pen, Point[] points){_g.DrawClosedCurve(pen, points) ;}
		public void DrawClosedCurve (Pen pen, Point[] points, Single tension, FillMode fillmode){_g.DrawClosedCurve(pen, points, tension, fillmode) ;}
		public void Clear (Color color){_g.Clear(color) ;}
		public void FillRectangle (Brush brush, RectangleF rect){_g.FillRectangle(brush,rect) ;}
		public void FillRectangle (Brush brush, Single x, Single y, Single width, Single height){_g.FillRectangle(brush,x,y,width,height) ;}
		public void FillRectangles (Brush brush, RectangleF[] rects){_g.FillRectangles(brush,rects) ;}
		public void FillRectangle (Brush brush, Rectangle rect){_g.FillRectangle(brush,rect) ;}
		public void FillRectangle (Brush brush, Int32 x, Int32 y, Int32 width, Int32 height){_g.FillRectangle(brush,x,y,width,height) ;}
		public void FillRectangles (Brush brush, Rectangle[] rects){_g.FillRectangles(brush,rects) ;}
		public void FillPolygon (Brush brush, PointF[] points){_g.FillPolygon(brush,points) ;}
		public void FillPolygon (Brush brush, PointF[] points, FillMode fillMode){_g.FillPolygon(brush,points,fillMode) ;}
		public void FillPolygon (Brush brush, Point[] points){_g.FillPolygon(brush,points) ;}
		public void FillPolygon (Brush brush, Point[] points, FillMode fillMode){_g.FillPolygon(brush,points,fillMode) ;}
		public void FillEllipse (Brush brush, RectangleF rect){_g.FillEllipse(brush,rect) ;}
		public void FillEllipse (Brush brush, Single x, Single y, Single width, Single height){_g.FillEllipse(brush,x,y,width,height) ;}
		public void FillEllipse (Brush brush, Rectangle rect){_g.FillEllipse(brush,rect) ;}
		public void FillEllipse (Brush brush, Int32 x, Int32 y, Int32 width, Int32 height){_g.FillEllipse(brush,x,y,width,height) ;}
		public void FillPie (Brush brush, Rectangle rect, Single startAngle, Single sweepAngle){_g.FillPie(brush,rect,startAngle,sweepAngle) ;}
		public void FillPie (Brush brush, Single x, Single y, Single width, Single height, Single startAngle, Single sweepAngle){_g.FillPie(brush,x,y,width,height,startAngle,sweepAngle) ;}
		public void FillPie (Brush brush, Int32 x, Int32 y, Int32 width, Int32 height, Int32 startAngle, Int32 sweepAngle){_g.FillPie(brush,x,y,width,height,startAngle,sweepAngle) ;}
		public void FillPath (Brush brush, GraphicsPath path){_g.FillPath(brush,path) ;}
		public void FillClosedCurve (Brush brush, PointF[] points){_g.FillClosedCurve(brush,points) ;}
		public void FillClosedCurve (Brush brush, PointF[] points, FillMode fillmode){_g.FillClosedCurve(brush,points,fillmode) ;}
		public void FillClosedCurve (Brush brush, PointF[] points, FillMode fillmode, Single tension){_g.FillClosedCurve(brush,points,fillmode,tension) ;}
		public void FillClosedCurve (Brush brush, Point[] points){_g.FillClosedCurve(brush,points) ;}
		public void FillClosedCurve (Brush brush, Point[] points, FillMode fillmode){_g.FillClosedCurve(brush,points,fillmode) ;}
		public void FillClosedCurve (Brush brush, Point[] points, FillMode fillmode, Single tension){_g.FillClosedCurve(brush,points,fillmode,tension) ;}
		public void FillRegion (Brush brush, Region region){_g.FillRegion(brush,region) ;}
		public void DrawString (String s, Font font, Brush brush, Single x, Single y){_g.DrawString(s,font,brush,x,y) ;}
		public void DrawString (String s, Font font, Brush brush, PointF point){_g.DrawString(s,font,brush,point) ;}
		public void DrawString (String s, Font font, Brush brush, Single x, Single y, StringFormat format){_g.DrawString(s,font,brush,x,y,format) ;}
		public void DrawString (String s, Font font, Brush brush, PointF point, StringFormat format){_g.DrawString(s,font,brush,point,format) ;}
		public void DrawString (String s, Font font, Brush brush, RectangleF layoutRectangle){_g.DrawString(s,font,brush,layoutRectangle) ;}
		public void DrawString (String s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format){_g.DrawString(s,font,brush,layoutRectangle,format) ;}
		public System.Drawing.SizeF MeasureString (String text, Font font, SizeF layoutArea, StringFormat stringFormat, out Int32 charactersFitted, out Int32 linesFilled){int a, b; SizeF siz = _g.MeasureString(text,font,layoutArea,stringFormat,out a,out b); charactersFitted = a; linesFilled = b; return siz;}
		public System.Drawing.SizeF MeasureString (String text, Font font, PointF origin, StringFormat stringFormat){return _g.MeasureString(text,font,origin,stringFormat) ;}
		public System.Drawing.SizeF MeasureString (String text, Font font, SizeF layoutArea){return _g.MeasureString(text,font,layoutArea) ;}
		public System.Drawing.SizeF MeasureString (String text, Font font, SizeF layoutArea, StringFormat stringFormat){return _g.MeasureString(text,font,layoutArea,stringFormat) ;}
		public System.Drawing.SizeF MeasureString (String text, Font font){return _g.MeasureString(text,font) ;}
		public System.Drawing.SizeF MeasureString (String text, Font font, Int32 width){return _g.MeasureString(text,font,width) ;}
		public System.Drawing.SizeF MeasureString (String text, Font font, Int32 width, StringFormat format){return _g.MeasureString(text,font,width,format) ;}
		public System.Drawing.Region[] MeasureCharacterRanges (String text, Font font, RectangleF layoutRect, StringFormat stringFormat){return _g.MeasureCharacterRanges(text,font,layoutRect,stringFormat) ;}
		public void DrawIcon (Icon icon, Int32 x, Int32 y){_g.DrawIcon(icon,x,y) ;}
		public void DrawIcon (Icon icon, Rectangle targetRect){_g.DrawIcon(icon,targetRect) ;}
		public void DrawIconUnstretched (Icon icon, Rectangle targetRect){_g.DrawIconUnstretched(icon,targetRect) ;}
		public void DrawImage (Image image, PointF point){_g.DrawImage(image,point) ;}
		public void DrawImage (Image image, Single x, Single y){_g.DrawImage(image,x,y) ;}
		public void DrawImage (Image image, RectangleF rect){_g.DrawImage(image,rect) ;}
		public void DrawImage (Image image, Single x, Single y, Single width, Single height){_g.DrawImage(image,x,y,width,height) ;}
		public void DrawImage (Image image, Point point){_g.DrawImage(image,point) ;}
		public void DrawImage (Image image, Int32 x, Int32 y){_g.DrawImage(image,x,y) ;}
		public void DrawImage (Image image, Rectangle rect){_g.DrawImage(image,rect) ;}
		public void DrawImage (Image image, Int32 x, Int32 y, Int32 width, Int32 height){_g.DrawImage(image,x,y,width,height) ;}
		public void DrawImageUnscaled (Image image, Point point){_g.DrawImageUnscaled(image,point) ;}
		public void DrawImageUnscaled (Image image, Int32 x, Int32 y){_g.DrawImageUnscaled(image,x,y) ;}
		public void DrawImageUnscaled (Image image, Rectangle rect){_g.DrawImageUnscaled(image,rect) ;}
		public void DrawImageUnscaled (Image image, Int32 x, Int32 y, Int32 width, Int32 height){_g.DrawImageUnscaled(image,x,y,width,height) ;}
		public void DrawImage (Image image, PointF[] destPoints){_g.DrawImage(image,destPoints) ;}
		public void DrawImage (Image image, Point[] destPoints){_g.DrawImage(image, destPoints) ;}
		public void DrawImage (Image image, Single x, Single y, RectangleF srcRect, GraphicsUnit srcUnit){_g.DrawImage(image,x,y,srcRect,srcUnit) ;}
		public void DrawImage (Image image, Int32 x, Int32 y, Rectangle srcRect, GraphicsUnit srcUnit){_g.DrawImage(image,x,y,srcRect,srcUnit) ;}
		public void DrawImage (Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit){_g.DrawImage(image,destRect,srcRect,srcUnit) ;}
		public void DrawImage (Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit){_g.DrawImage(image,destRect,srcRect,srcUnit) ;}
		public void DrawImage (Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit){_g.DrawImage(image,destPoints,srcRect,srcUnit) ;}
		public void DrawImage (Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr){_g.DrawImage(image,destPoints,srcRect,srcUnit) ;}
		public void DrawImage (Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback){_g.DrawImage(image,destPoints,srcRect,srcUnit) ;}
		public void DrawImage (Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit){_g.DrawImage(image, destPoints,srcRect,srcUnit) ;}
		public void DrawImage (Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr){_g.DrawImage(image,destPoints,srcRect,srcUnit) ;}
		public void DrawImage (Image image, Rectangle destRect, Single srcX, Single srcY, Single srcWidth, Single srcHeight, GraphicsUnit srcUnit){_g.DrawImage(image, destRect,srcX,srcY,srcWidth,srcHeight,srcUnit) ;}
		public void DrawImage (Image image, Rectangle destRect, Single srcX, Single srcY, Single srcWidth, Single srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs){_g.DrawImage(image,destRect,srcX,srcY,srcWidth,srcHeight,srcUnit) ;}
		public void DrawImage (Image image, Rectangle destRect, Int32 srcX, Int32 srcY, Int32 srcWidth, Int32 srcHeight, GraphicsUnit srcUnit){_g.DrawImage(image,destRect,srcX,srcY,srcWidth,srcHeight,srcUnit) ;}
		public void DrawImage (Image image, Rectangle destRect, Int32 srcX, Int32 srcY, Int32 srcWidth, Int32 srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr){_g.DrawImage(image,destRect,srcX,srcY,srcWidth,srcHeight,srcUnit,imageAttr) ;}
		public void SetClip (Graphics g){_g.SetClip(g) ;}
		public void SetClip (Graphics g, CombineMode combineMode){_g.SetClip(g,combineMode) ;}
		public void SetClip (Rectangle rect){_g.SetClip(rect) ;}
		public void SetClip (Rectangle rect, CombineMode combineMode){_g.SetClip(rect,combineMode) ;}
		public void SetClip (RectangleF rect){_g.SetClip(rect) ;}
		public void SetClip (RectangleF rect, CombineMode combineMode){_g.SetClip(rect,combineMode ) ;}
		public void SetClip (GraphicsPath path){_g.SetClip(path) ;}
		public void SetClip (GraphicsPath path, CombineMode combineMode){_g.SetClip(path,combineMode) ;}
		public void SetClip (Region region, CombineMode combineMode){_g.SetClip(region,combineMode) ;}
		public void IntersectClip (Rectangle rect){_g.IntersectClip(rect) ;}
		public void IntersectClip (RectangleF rect){_g.IntersectClip(rect) ;}
		public void IntersectClip (Region region){_g.IntersectClip(region) ;}
		public void ExcludeClip (Rectangle rect){_g.ExcludeClip(rect) ;}
		public void ExcludeClip (Region region){_g.ExcludeClip(region) ;}
		public void ResetClip (){_g.ResetClip() ;}
		public void TranslateClip (Single dx, Single dy){_g.TranslateClip(dx,dy) ;}
		public void TranslateClip (Int32 dx, Int32 dy){_g.TranslateClip(dx,dy) ;}
		public System.Boolean IsVisible (Int32 x, Int32 y){return _g.IsVisible(x,y) ;}
		public System.Boolean IsVisible (Point point){return _g.IsVisible(point) ;}
		public System.Boolean IsVisible (Single x, Single y){return _g.IsVisible(x,y) ;}
		public System.Boolean IsVisible (PointF point){return _g.IsVisible(point) ;}
		public System.Boolean IsVisible (Int32 x, Int32 y, Int32 width, Int32 height){return _g.IsVisible(x,y,width,height) ;}
		public System.Boolean IsVisible (Rectangle rect){return _g.IsVisible(rect) ;}
		public System.Boolean IsVisible (Single x, Single y, Single width, Single height){return _g.IsVisible(x,y,width,height) ;}
		public System.Boolean IsVisible (RectangleF rect){return _g.IsVisible(rect) ;}
		public System.Drawing.Drawing2D.GraphicsState Save (){return _g.Save() ;}
		public void Restore (GraphicsState gstate){_g.Restore(gstate) ;}
		public System.Drawing.Drawing2D.GraphicsContainer BeginContainer (RectangleF dstrect, RectangleF srcrect, GraphicsUnit unit){return _g.BeginContainer(dstrect,srcrect,unit) ;}
		public System.Drawing.Drawing2D.GraphicsContainer BeginContainer (){return _g.BeginContainer() ;}
		public void EndContainer (GraphicsContainer container){_g.EndContainer(container) ;}
		public System.Drawing.Drawing2D.GraphicsContainer BeginContainer (Rectangle dstrect, Rectangle srcrect, GraphicsUnit unit){return _g.BeginContainer(dstrect,srcrect,unit) ;}
		public void AddMetafileComment (Byte[] data){_g.AddMetafileComment(data) ;}
		public System.Drawing.Drawing2D.CompositingMode CompositingMode {get{return _g.CompositingMode;} set{_g.CompositingMode=value;} }
		public System.Drawing.Point RenderingOrigin {get{return _g.RenderingOrigin; } set{_g.RenderingOrigin=value;} }
		public System.Drawing.Drawing2D.CompositingQuality CompositingQuality {get{return _g.CompositingQuality; } set{_g.CompositingQuality=value;} }
		public System.Drawing.Text.TextRenderingHint TextRenderingHint {get{return _g.TextRenderingHint; } set{_g.TextRenderingHint=value;} }
		public System.Int32 TextContrast {get{return _g.TextContrast; } set{_g.TextContrast=value;} }
		public System.Drawing.Drawing2D.SmoothingMode SmoothingMode {get{return _g.SmoothingMode; } set{_g.SmoothingMode=value;} }
		public System.Drawing.Drawing2D.PixelOffsetMode PixelOffsetMode {get{return _g.PixelOffsetMode; } set{_g.PixelOffsetMode =value;} }
		public System.Drawing.Drawing2D.InterpolationMode InterpolationMode {get{return _g.InterpolationMode; } set{_g.InterpolationMode =value;} }
		public System.Drawing.Drawing2D.Matrix Transform {get{return _g.Transform; } set{_g.Transform=value;} }
		public System.Drawing.GraphicsUnit PageUnit {get{return _g.PageUnit; } set{_g.PageUnit=value;} }
		public System.Single PageScale {get{return _g.PageScale; } set{_g.PageScale=value;} }
		public System.Single DpiX {get{return _g.DpiX; } }
		public System.Single DpiY {get{return _g.DpiY; } }
		public System.Drawing.Region Clip {get{return _g.Clip; } set{_g.Clip=value;} }
		public System.Drawing.RectangleF ClipBounds {get{return _g.ClipBounds; } }
		public System.Boolean IsClipEmpty {get{return _g.IsClipEmpty; } }
		public System.Drawing.RectangleF VisibleClipBounds {get{return _g.VisibleClipBounds; } }
		public System.Boolean IsVisibleClipEmpty {get{return _g.IsVisibleClipEmpty; } }
	}
}
