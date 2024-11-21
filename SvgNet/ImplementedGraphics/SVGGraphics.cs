/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using SvgNet.Elements;
using SvgNet.MetafileTools;

namespace SvgNet;

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
/// are meaningless in SVG, usually because there is no physical display device to refer to.  When such a method is called an <see cref="SvgGdiNotImplementedException"/> exception is thrown.
/// </para>
/// <para>
/// Many parameters used by GDI have no SVG equivalent -- for instance, GDI allows some fine control over how font hints are used.  This detailed information is
/// thrown away.
/// </para>
/// <para>
/// Some aspects of GDI that can be implemented in SVG are not.  The most important omission is that only solid brushes are supported.
/// </para>
/// </summary>
public sealed partial class SvgGraphics : IGraphics {
    public SvgGraphics() : this(Color.FromName("Control")) {
    }

    public SvgGraphics(Color backgroundColor) {
        _root = new SvgSvgElement { Id = "SvgGdi_output" };

        _bg = new SvgRectElement(0, 0, "100%", "100%") { Id = "background" };
        _bg.Style.Set("fill", new SvgColor(backgroundColor));
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
    public string WriteSVGString() => _root.WriteSVGString(true);

    /// <summary>
    /// Get a string containing an SVG document.  The very heart of SvgGdi.  It calls <c>WriteSVGString</c> on the <see cref="SvgElement"/>
    /// at the root of this <c>SvgGraphics</c> and returns the resulting string.
    /// </summary>
    /// <param name="bounds">Width/Height values to add as attributes to the svg element</param>
    public string WriteSVGString(SizeF bounds) => _root.WriteSVGString(true, bounds);

    /// <summary>
    /// Get a string containing an SVG document.  The very heart of SvgGdi.  It calls <c>WriteSVGString</c> on the <see cref="SvgElement"/>
    /// at the root of this <c>SvgGraphics</c> and returns the resulting string.
    /// </summary>
    /// <param name="width">Width value to add as attribute to the svg element</param>
    /// <param name="height">Height value to add as attribute to the svg element</param>
    public string WriteSVGString(float width, float height) => _root.WriteSVGString(true, new SizeF(width, height));

    /// <summary>
    /// Not implemented.
    /// </summary>
    public Region Clip { get => throw new SvgGdiNotImplementedException("Clip"); set => throw new SvgGdiNotImplementedException("Clip"); }

    /// <summary>
    /// Not implemented.
    /// </summary>
    public RectangleF ClipBounds => throw new SvgGdiNotImplementedException("ClipBounds");

    /// <summary>
    /// Get is not implemented (throws an exception).  Set does nothing.
    /// </summary>
    public CompositingMode CompositingMode { get => throw new SvgGdiNotImplementedException("get_CompositingMode"); set { } }

    /// <summary>
    /// Get is not implemented (throws an exception).  Set does nothing.
    /// </summary>
    public CompositingQuality CompositingQuality { get => throw new SvgGdiNotImplementedException("get_CompositingQuality"); set { } }

    /// <summary>
    /// Not implemented.
    /// </summary>
    public float DpiX => throw new SvgGdiNotImplementedException("DpiX");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public float DpiY => throw new SvgGdiNotImplementedException("DpiY");

    /// <summary>
    /// Get is not implemented (throws an exception).  Set does nothing.
    /// </summary>
    public InterpolationMode InterpolationMode { get => throw new SvgGdiNotImplementedException("get_InterpolationMode"); set { } }

    /// <summary>
    /// Not implemented.
    /// </summary>
    public bool IsClipEmpty => throw new SvgGdiNotImplementedException("IsClipEmpty");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public bool IsVisibleClipEmpty => throw new SvgGdiNotImplementedException("IsVisibleClipEmpty");

    /// <summary>
    /// Get is not implemented (throws an exception).  Set does nothing.
    /// </summary>
    public float PageScale { get => throw new SvgGdiNotImplementedException("PageScale"); set { } }

    /// <summary>
    /// Get is not implemented (throws an exception).  Set does nothing.
    /// </summary>
    public GraphicsUnit PageUnit { get => throw new SvgGdiNotImplementedException("PageUnit"); set { } }

    /// <summary>
    /// Get is not implemented (throws an exception).  Set does nothing.
    /// </summary>
    public PixelOffsetMode PixelOffsetMode { get => throw new SvgGdiNotImplementedException("get_PixelOffsetMode"); set { } }

    /// <summary>
    /// Get is not implemented (throws an exception).  Set does nothing.
    /// </summary>
    public Point RenderingOrigin { get => throw new SvgGdiNotImplementedException("get_RenderingOrigin"); set { } }

    public SmoothingMode SmoothingMode {
        get;
        set {
            switch (value) {
                case SmoothingMode.Invalid:
                    break;

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
            field = value;
        }
    } = SmoothingMode.Invalid;

    /// <summary>
    /// Get is not implemented (throws an exception).
    /// </summary>
    public int TextContrast { get => throw new SvgGdiNotImplementedException("get_TextContrast"); set { } }

    /// <summary>
    /// Get is not implemented (throws an exception).
    /// </summary>
    public TextRenderingHint TextRenderingHint {
        get => throw new SvgGdiNotImplementedException("get_TextRenderingHint");
        set {
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

    public Matrix Transform {
        get => _transforms.Result.Clone();
        set => _transforms.SetTop(value);
    }

    /// <summary>
    /// Not implemented.
    /// </summary>
    public RectangleF VisibleClipBounds => throw new SvgGdiNotImplementedException("VisibleClipBounds");

    /// <summary>
    /// Does nothing.  Should perhaps insert a comment into the SVG XML output, but is this really analogous
    /// to a metafile comment.
    /// </summary>
    public void AddMetafileComment(byte[] data) {
        //probably should add xml comment
    }

    /// <summary>
    /// Not implemented.
    /// </summary>
    public GraphicsContainer BeginContainer(RectangleF dstrect, RectangleF srcrect, GraphicsUnit unit) => throw new SvgGdiNotImplementedException("BeginContainer (RectangleF dstrect, RectangleF srcrect, GraphicsUnit unit)");

    /// <summary>
    /// Implemented, but returns null as SVG has a proper scenegraph, unlike GDI+.  The effect of calling <c>BeginContainer</c> is to create a new SVG group
    /// and apply transformations etc to produce the effect that a GDI+ container would produce.
    /// </summary>
    public GraphicsContainer BeginContainer() {
        var gr = new SvgGroupElement();
        _cur.AddChild(gr);
        _cur = gr;
        _cur.Id += "_BeginContainer";
        _transforms.PushIdentity();
        return null;
    }

    /// <summary>
    /// Not implemented.
    /// </summary>
    public GraphicsContainer BeginContainer(Rectangle dstrect, Rectangle srcrect, GraphicsUnit unit) => throw new SvgGdiNotImplementedException("BeginContainer (Rectangle dstrect, Rectangle srcrect, GraphicsUnit unit)");

    /// <summary>
    /// Implemented
    /// </summary>
    public void Clear(Color color) {
        _cur.Children.Clear();
        _bg.Style.Set("fill", new SvgColor(color));
    }

    public void Dispose() => _transforms.Dispose();

    /// <summary>
    /// Implemented.  <c>DrawArc</c> functions work correctly and thus produce different output from GDI+ if the ellipse is not circular.
    /// </summary>
    public void DrawArc(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle) {
        string s = GDIArc2SVGPath(x, y, width, height, startAngle, sweepAngle, false);

        var arc = new SvgPathElement {
            D = s,
            Style = new SvgStyle(pen)
        };
        if (!_transforms.Result.IsIdentity)
            arc.Transform = new SvgTransformList(_transforms.Result.Clone());
        _cur.AddChild(arc);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle) => DrawArc(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawArc(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle) => DrawArc(pen, x, y, width, height, startAngle, (float)sweepAngle);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle) => DrawArc(pen, rect.X, rect.X, rect.Width, rect.Height, startAngle, sweepAngle);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawBezier(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) {
        var bez = new SvgPathElement {
            D = "M " + x1.ToString("F", CultureInfo.InvariantCulture) + " " + y1.ToString("F", CultureInfo.InvariantCulture) + " C " +
                x2.ToString("F", CultureInfo.InvariantCulture) + " " + y2.ToString("F", CultureInfo.InvariantCulture) + " " +
                x3.ToString("F", CultureInfo.InvariantCulture) + " " + y3.ToString("F", CultureInfo.InvariantCulture) + " " +
                x4.ToString("F", CultureInfo.InvariantCulture) + " " + y4.ToString("F", CultureInfo.InvariantCulture),
            Style = new SvgStyle(pen)
        };
        if (!_transforms.Result.IsIdentity)
            bez.Transform = new SvgTransformList(_transforms.Result.Clone());
        _cur.AddChild(bez);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawBezier(Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4) => DrawBezier(pen, pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawBezier(Pen pen, Point pt1, Point pt2, Point pt3, Point pt4) => DrawBezier(pen, pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawBeziers(Pen pen, PointF[] points) {
        var bez = new SvgPathElement();

        string s = "M " + points[0].X.ToString("F", CultureInfo.InvariantCulture) + " " + points[0].Y.ToString("F", CultureInfo.InvariantCulture) + " C ";

        for (int i = 1; i < points.Length; ++i) s += points[i].X.ToString("F", CultureInfo.InvariantCulture) + " " + points[i].Y.ToString("F", CultureInfo.InvariantCulture) + " ";

        bez.D = s;

        bez.Style = new SvgStyle(pen);
        if (!_transforms.Result.IsIdentity)
            bez.Transform = new SvgTransformList(_transforms.Result.Clone());
        _cur.AddChild(bez);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawBeziers(Pen pen, Point[] points) {
        PointF[] pts = Point2PointF(points);
        DrawBeziers(pen, pts);
    }

    /// <summary>
    /// Implemented.  The <c>DrawClosedCurve</c> functions emulate GDI behavior by drawing a coaligned cubic bezier.  This seems to produce
    /// a very good approximation so probably GDI+ does the same thing -- a
    /// </summary>
    public void DrawClosedCurve(Pen pen, PointF[] points) {
        PointF[] pts = Spline2Bez(points, 0, points.Length - 1, true, .5f);
        DrawBeziers(pen, pts);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawClosedCurve(Pen pen, PointF[] points, float tension, FillMode fillMode) {
        PointF[] pts = Spline2Bez(points, 0, points.Length - 1, true, tension);
        DrawBeziers(pen, pts);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawClosedCurve(Pen pen, Point[] points) {
        PointF[] pts = Spline2Bez(Point2PointF(points), 0, points.Length - 1, true, .5f);
        DrawBeziers(pen, pts);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawClosedCurve(Pen pen, Point[] points, float tension, FillMode fillMode) {
        PointF[] pts = Spline2Bez(Point2PointF(points), 0, points.Length - 1, true, tension);
        DrawBeziers(pen, pts);
    }

    /// <summary>
    /// Implemented.  The <c>DrawCurve</c> functions emulate GDI behavior by drawing a coaligned cubic bezier.  This seems to produce
    /// a very good approximation so probably GDI+ does the same.
    /// </summary>
    public void DrawCurve(Pen pen, PointF[] points) {
        PointF[] pts = Spline2Bez(points, 0, points.Length - 1, false, .5f);
        DrawBeziers(pen, pts);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawCurve(Pen pen, PointF[] points, float tension) {
        PointF[] pts = Spline2Bez(points, 0, points.Length - 1, false, tension);
        DrawBeziers(pen, pts);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments) {
        PointF[] pts = Spline2Bez(points, offset, numberOfSegments, false, .5f);
        DrawBeziers(pen, pts);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments, float tension) {
        PointF[] pts = Spline2Bez(points, offset, numberOfSegments, false, tension);
        DrawBeziers(pen, pts);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawCurve(Pen pen, Point[] points) {
        PointF[] pts = Spline2Bez(Point2PointF(points), 0, points.Length - 1, false, .5f);
        DrawBeziers(pen, pts);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawCurve(Pen pen, Point[] points, float tension) {
        PointF[] pts = Spline2Bez(Point2PointF(points), 0, points.Length - 1, false, tension);
        DrawBeziers(pen, pts);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawCurve(Pen pen, Point[] points, int offset, int numberOfSegments, float tension) {
        PointF[] pts = Spline2Bez(Point2PointF(points), offset, numberOfSegments, false, tension);
        DrawBeziers(pen, pts);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawEllipse(Pen pen, RectangleF rect) => DrawEllipse(pen, rect.X, rect.Y, rect.Width, rect.Height);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawEllipse(Pen pen, float x, float y, float width, float height) {
        var el = new SvgEllipseElement(x + (width / 2), y + (height / 2), width / 2, height / 2) {
            Style = new SvgStyle(pen)
        };
        if (!_transforms.Result.IsIdentity)
            el.Transform = new SvgTransformList(_transforms.Result.Clone());
        _cur.AddChild(el);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawEllipse(Pen pen, Rectangle rect) => DrawEllipse(pen, rect.X, rect.Y, rect.Width, (float)rect.Height);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawEllipse(Pen pen, int x, int y, int width, int height) => DrawEllipse(pen, x, y, width, (float)height);

    /// <summary>
    /// Implemented.  The <c>DrawIcon</c> group of functions emulate drawing a bitmap by creating many SVG <c>rect</c> elements.  This is quite effective but
    /// can lead to a very big SVG file.  Alpha and stretching are handled correctly.  No antialiasing is done.
    /// </summary>
    public void DrawIcon(Icon icon, int x, int y) {
        var bmp = icon.ToBitmap();
        DrawBitmapData(bmp, x, y, icon.Width, icon.Height, false);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawIcon(Icon icon, Rectangle targetRect) {
        var bmp = icon.ToBitmap();
        DrawBitmapData(bmp, targetRect.X, targetRect.Y, targetRect.Width, targetRect.Height, true);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawIconUnstretched(Icon icon, Rectangle targetRect) {
        var bmp = icon.ToBitmap();
        DrawBitmapData(bmp, targetRect.X, targetRect.Y, targetRect.Width, targetRect.Height, false);
    }

    /// <summary>
    /// Implemented.  The <c>DrawImage</c> group of functions emulate drawing a bitmap by creating many SVG <c>rect</c> elements.  This is quite effective but
    /// can lead to a very big SVG file.  Alpha and stretching are handled correctly.  No antialiasing is done.
    /// <para>
    /// The GDI+ documentation suggests that the 'Unscaled' functions should truncate the image.  GDI+ does not actually do this, but <c>SvgGraphics</c> does.
    /// </para>
    /// </summary>
    public void DrawImage(Image image, PointF point) => DrawBitmapImageUnscaled(image, point.X, point.Y);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawImage(Image image, float x, float y) => DrawBitmapImageUnscaled(image, x, y);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawImage(Image image, RectangleF rect) => DrawBitmapImage(image, rect.X, rect.Y, rect.Width, rect.Height);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawImage(Image image, float x, float y, float width, float height) => DrawBitmapImage(image, x, y, width, height);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawImage(Image image, Point point) => DrawBitmapImageUnscaled(image, point.X, point.Y);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawImage(Image image, int x, int y) => DrawBitmapImageUnscaled(image, x, y);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawImage(Image image, Rectangle rect) => DrawBitmapImage(image, rect.X, rect.Y, rect.Width, rect.Height);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawImage(Image image, int x, int y, int width, int height) => DrawBitmapImage(image, x, y, width, height);

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void DrawImage(Image image, PointF[] destPoints) => throw new SvgGdiNotImplementedException("DrawImage (Image image, PointF[] destPoints)");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void DrawImage(Image image, Point[] destPoints) => throw new SvgGdiNotImplementedException("DrawImage (Image image, Point[] destPoints)");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void DrawImage(Image image, float x, float y, RectangleF srcRect, GraphicsUnit srcUnit) => throw new SvgGdiNotImplementedException("DrawImage (Image image, Single x, Single y, RectangleF srcRect, GraphicsUnit srcUnit)");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void DrawImage(Image image, int x, int y, Rectangle srcRect, GraphicsUnit srcUnit) => throw new SvgGdiNotImplementedException("DrawImage (Image image, Int32 x, Int32 y, Rectangle srcRect, GraphicsUnit srcUnit)");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit) => throw new SvgGdiNotImplementedException("DrawImage (Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit)");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void DrawImage(Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit) => throw new SvgGdiNotImplementedException("DrawImage (Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit)");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit) => throw new SvgGdiNotImplementedException("DrawImage (Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit)");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr) => throw new SvgGdiNotImplementedException("DrawImage (Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback) => throw new SvgGdiNotImplementedException("DrawImage (Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback)");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit) => throw new SvgGdiNotImplementedException("DrawImage (Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit)");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr) => throw new SvgGdiNotImplementedException("DrawImage (Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit) => throw new SvgGdiNotImplementedException("DrawImage (Image image, Rectangle destRect, Single srcX, Single srcY, Single srcWidth, Single srcHeight, GraphicsUnit srcUnit)");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs) => throw new SvgGdiNotImplementedException("DrawImage (Image image, Rectangle destRect, Single srcX, Single srcY, Single srcWidth, Single srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs)");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit) => throw new SvgGdiNotImplementedException("DrawImage (Image image, Rectangle destRect, Int32 srcX, Int32 srcY, Int32 srcWidth, Int32 srcHeight, GraphicsUnit srcUnit)");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr) => throw new SvgGdiNotImplementedException("DrawImage (Image image, Rectangle destRect, Int32 srcX, Int32 srcY, Int32 srcWidth, Int32 srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr)");

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawImageUnscaled(Image image, Point point) => DrawImage(image, point.X, (float)point.Y);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawImageUnscaled(Image image, int x, int y) => DrawImage(image, x, (float)y);

    /// <summary>
    /// Implemented.  There seems to be a GDI bug in that the image is *not* clipped to the rectangle.  We do clip it.
    /// </summary>
    public void DrawImageUnscaled(Image image, Rectangle rect) => DrawImageUnscaled(image, rect.X, rect.Y, rect.Width, rect.Height);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawImageUnscaled(Image image, int x, int y, int width, int height) {
        if (image is Bitmap bmp)
            DrawBitmapData(bmp, x, y, width, height, false);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawLine(Pen pen, float x1, float y1, float x2, float y2) {
        if (PenUsesCustomLineCap(pen))
            DrawLines(pen, new PointF[] { new(x1, y1), new(x2, y2) });
        else {
            // This code works, but not for CustomLineCup style
            var lin = new SvgLineElement(x1, y1, x2, y2) {
                Style = new SvgStyle(pen)
            };
            if (!_transforms.Result.IsIdentity)
                lin.Transform = new SvgTransformList(_transforms.Result.Clone());
            _cur.AddChild(lin);

            DrawEndAnchors(pen, new PointF(x1, y1), new PointF(x2, y2));
        }
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawLine(Pen pen, PointF pt1, PointF pt2) => DrawLine(pen, pt1.X, pt1.Y, pt2.X, pt2.Y);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawLine(Pen pen, int x1, int y1, int x2, int y2) => DrawLine(pen, x1, y1, x2, (float)y2);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawLine(Pen pen, Point pt1, Point pt2) => DrawLine(pen, pt1.X, pt1.Y, pt2.X, (float)pt2.Y);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawLines(Pen pen, PointF[] points) {
        if (points.Length <= 1)
            return;

        if (PenUsesCustomLineCap(pen) && MetafileTrick(pen, points))
            return;

        // This code works, but not for CustomLineCap style
        var pl = new SvgPolylineElement(points) {
            Style = new SvgStyle(pen)
        };
        if (!_transforms.Result.IsIdentity)
            pl.Transform = new SvgTransformList(_transforms.Result.Clone());
        _cur.AddChild(pl);

        DrawEndAnchors(pen, points[0], points[points.Length - 1]);
    }

    private static bool PenUsesCustomLineCap(Pen pen) => pen.StartCap == LineCap.Custom || pen.EndCap == LineCap.Custom;

    private bool MetafileTrick(Pen pen, PointF[] originalPoints) {
        // GraphicsPaths used in the constructor of CustomLineCap
        // are private to the native GDI+ and for example the shape of AdjustableArrowCap
        // is completely private to the native GDI+
        //
        // So in order to render the possibly any-shaped custom line caps we'll draw the line as GDI metafile and then reverse
        // engineer the GDI metafile drawing and convert it to corresponding SVG commands

        // Calculate the bounding rectangle
        var points = new PointF[originalPoints.Length];
        Array.Copy(originalPoints, points, originalPoints.Length);
        float minX = points[0].X;
        float maxX = points[0].X;
        float minY = points[0].Y;
        float maxY = points[0].Y;
        for (int i = 1; i < points.Length; i++) {
            PointF point = points[i];
            minX = Math.Min(minX, point.X);
            maxX = Math.Max(maxX, point.X);
            minY = Math.Min(minY, point.Y);
            maxY = Math.Max(maxY, point.Y);
        }
        var bounds = new RectangleF(minX, minY, maxX - minX + 1, maxY - minY + 1);

        // Make the rectangle 0-based where "zero" represents the original shift
        PointF zero = bounds.Location;
        bounds.Offset(-zero.X, -zero.Y);

        // Make the original point-path "zero"-based
        for (int i = 0; i < points.Length; i++) {
            points[i].X -= zero.X;
            points[i].Y -= zero.Y;
        }

        bool metafileIsEmpty = true;
        var metafileBuffer = new MemoryStream();
        Metafile metafile = null;

        try {
            /* For discussion of tricky metafile details see:
             * - http://nicholas.piasecki.name/blog/2009/06/drawing-o-an-in-memory-metafile-in-c-sharp/
             * - http://stackoverflow.com/a/1533053/2626313
             */

            using (var temporaryBitmap = new Bitmap(1, 1)) {
                using var temporaryCanvas = Graphics.FromImage(temporaryBitmap);
                IntPtr hdc = temporaryCanvas.GetHdc();
                metafile = new Metafile(
                    metafileBuffer,
                    hdc,
                    bounds,
                    MetafileFrameUnit.GdiCompatible,
                    EmfType.EmfOnly);

                temporaryCanvas.ReleaseHdc();
            }

            using var metafileCanvas = Graphics.FromImage(metafile);
            metafileCanvas.DrawLines(pen, points);
        } finally {
            metafile?.Dispose();
        }

        metafileBuffer.Position = 0;
        var parser = new MetafileTools.MetafileParser();
        parser.EnumerateMetafile(metafileBuffer, pen.Width, zero, (PointF[] linePoints) => {
            metafileIsEmpty = false;

            var pl = new SvgPolylineElement(linePoints) {
                Style = new SvgStyle(pen)
            };

            // Make it pretty
            pl.Style.Set("stroke-linecap", "round");

            if (!_transforms.Result.IsIdentity)
                pl.Transform = new SvgTransformList(_transforms.Result.Clone());
            _cur.AddChild(pl);
        }, (PointF[] linePoints, Brush fillBrush) => {
            metafileIsEmpty = false;
            FillPolygon(fillBrush, linePoints);
        });
        // TODO: metafile recording on OpenSUSE Linux with Mono 3.8.0 does not seem to work at all
        // as the supposed implementation in https://github.com/mono/libgdiplus/blob/master/src/graphics-metafile.c is
        // full of "TODO". In this case we should take a graceful signal to use the fallback approach
        return !metafileIsEmpty;
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawLines(Pen pen, Point[] points) {
        PointF[] pts = Point2PointF(points);
        DrawLines(pen, pts);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    /// <remarks>
    /// Mainly based on the libgdi+ implementation: https://github.com/mono/libgdiplus/blob/master/src/graphics-cairo.c
    /// and this SO question reply: https://stackoverflow.com/questions/1790862/how-to-determine-endpoints-of-arcs-in-graphicspath-pathpoints-and-pathtypes-arra
    /// from SiliconMind.
    /// </remarks>
    public void DrawPath(Pen pen, GraphicsPath path) {
        SvgPath data = HandleGraphicsPath(path);
        var pathElement = new SvgPathElement {
            Style = new SvgStyle(pen),
            D = data
        };
        if (!_transforms.Result.IsIdentity)
            pathElement.Transform = new SvgTransformList(_transforms.Result.Clone());
        _cur.AddChild(pathElement);
    }

    /// <summary>
    /// Implemented.  <c>DrawPie</c> functions work correctly and thus produce different output from GDI+ if the ellipse is not circular.
    /// </summary>
    public void DrawPie(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle) {
        string s = GDIArc2SVGPath(x, y, width, height, startAngle, sweepAngle, true);

        var pie = new SvgPathElement {
            D = s,
            Style = new SvgStyle(pen)
        };
        if (!_transforms.Result.IsIdentity)
            pie.Transform = new SvgTransformList(_transforms.Result.Clone());

        _cur.AddChild(pie);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawPie(Pen pen, RectangleF rect, float startAngle, float sweepAngle) => DrawPie(pen, rect.X, rect.X, rect.Width, rect.Height, startAngle, sweepAngle);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawPie(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle) => DrawPie(pen, x, y, width, height, startAngle, (float)sweepAngle);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawPie(Pen pen, Rectangle rect, float startAngle, float sweepAngle) => DrawPie(pen, rect.X, rect.X, rect.Width, rect.Height, startAngle, sweepAngle);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawPolygon(Pen pen, PointF[] points) {
        var pl = new SvgPolygonElement(points) {
            Style = new SvgStyle(pen)
        };
        if (!_transforms.Result.IsIdentity)
            pl.Transform = new SvgTransformList(_transforms.Result.Clone());
        _cur.AddChild(pl);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawPolygon(Pen pen, Point[] points) {
        PointF[] pts = Point2PointF(points);
        DrawPolygon(pen, pts);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawRectangle(Pen pen, Rectangle rect) => DrawRectangle(pen, rect.Left, rect.Top, rect.Width, (float)rect.Height);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawRectangle(Pen pen, float x, float y, float width, float height) {
        var rc = new SvgRectElement(x, y, width, height) {
            Style = new SvgStyle(pen)
        };
        if (!_transforms.Result.IsIdentity)
            rc.Transform = new SvgTransformList(_transforms.Result.Clone());
        _cur.AddChild(rc);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawRectangle(Pen pen, int x, int y, int width, int height) => DrawRectangle(pen, x, y, width, (float)height);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawRectangles(Pen pen, RectangleF[] rects) {
        foreach (RectangleF rc in rects) DrawRectangle(pen, rc.Left, rc.Top, rc.Width, rc.Height);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawRectangles(Pen pen, Rectangle[] rects) {
        foreach (Rectangle rc in rects) DrawRectangle(pen, rc.Left, rc.Top, rc.Width, (float)rc.Height);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawString(string s, Font font, Brush brush, float x, float y) => DrawText(s, font, brush, new RectangleF(x, y, 0, 0), StringFormat.GenericDefault, true);

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
    public void DrawString(string s, Font font, Brush brush, PointF point) => DrawText(s, font, brush, new RectangleF(point.X, point.Y, 0, 0), StringFormat.GenericDefault, true);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawString(string s, Font font, Brush brush, float x, float y, StringFormat format) => DrawText(s, font, brush, new RectangleF(x, y, 0, 0), format, true);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawString(string s, Font font, Brush brush, PointF point, StringFormat format) => DrawText(s, font, brush, new RectangleF(point.X, point.Y, 0, 0), format, true);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle) => DrawText(s, font, brush, layoutRectangle, StringFormat.GenericDefault, false);

    /// <summary>
    /// Implemented
    /// </summary>
    public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format) => DrawText(s, font, brush, layoutRectangle, format, false);

    /// <summary>
    /// The effect of calling this method is to pop out of the closest SVG group.  This simulates restoring GDI+ state from a <c>GraphicsContainer</c>
    /// </summary>
    public void EndContainer(GraphicsContainer container) {
        if (_cur == _topgroup)
            return;

        _cur = (SvgStyledTransformedElement)_cur.Parent;

        _transforms.Pop();
    }

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
    /// </summary>
    public void ExcludeClip(Rectangle rect) => throw new SvgGdiNotImplementedException("ExcludeClip (Rectangle rect)");

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
    /// </summary>
    public void ExcludeClip(Region region) => throw new SvgGdiNotImplementedException("ExcludeClip (Region region)");

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillClosedCurve(Brush brush, PointF[] points) {
        PointF[] pts = Spline2Bez(points, 0, points.Length - 1, true, .5f);
        FillBeziers(brush, pts, FillMode.Alternate);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillMode) {
        PointF[] pts = Spline2Bez(points, 0, points.Length - 1, true, .5f);
        FillBeziers(brush, pts, fillMode);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillMode, float tension) {
        PointF[] pts = Spline2Bez(points, 0, points.Length - 1, true, tension);
        FillBeziers(brush, pts, fillMode);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillClosedCurve(Brush brush, Point[] points) {
        PointF[] pts = Spline2Bez(Point2PointF(points), 0, points.Length - 1, true, .5f);
        FillBeziers(brush, pts, FillMode.Alternate);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillClosedCurve(Brush brush, Point[] points, FillMode fillMode) {
        PointF[] pts = Spline2Bez(Point2PointF(points), 0, points.Length - 1, true, .5f);
        FillBeziers(brush, pts, fillMode);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillClosedCurve(Brush brush, Point[] points, FillMode fillMode, float tension) {
        PointF[] pts = Spline2Bez(Point2PointF(points), 0, points.Length - 1, true, tension);
        FillBeziers(brush, pts, fillMode);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillEllipse(Brush brush, RectangleF rect) => FillEllipse(brush, rect.X, rect.Y, rect.Width, rect.Height);

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillEllipse(Brush brush, float x, float y, float width, float height) {
        var el = new SvgEllipseElement(x + (width / 2), y + (height / 2), width / 2, height / 2) {
            Style = HandleBrush(brush)
        };
        if (!_transforms.Result.IsIdentity)
            el.Transform = new SvgTransformList(_transforms.Result.Clone());
        _cur.AddChild(el);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillEllipse(Brush brush, Rectangle rect) => FillEllipse(brush, rect.X, rect.Y, rect.Width, (float)rect.Height);

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillEllipse(Brush brush, int x, int y, int width, int height) => FillEllipse(brush, x, y, width, (float)height);

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillPath(Brush brush, GraphicsPath path) {
        SvgPath data = HandleGraphicsPath(path);
        var pathElement = new SvgPathElement {
            Style = HandleBrush(brush),
            D = data
        };
        if (path.FillMode == FillMode.Alternate) pathElement.Style.Set("fill-rule", "evenodd");
        else pathElement.Style.Set("fill-rule", "nonzero");

        if (!_transforms.Result.IsIdentity)
            pathElement.Transform = new SvgTransformList(_transforms.Result.Clone());
        _cur.AddChild(pathElement);
    }

    /// <summary>
    /// Implemented <c>FillPie</c> functions work correctly and thus produce different output from GDI+ if the ellipse is not circular.
    /// </summary>
    public void FillPie(Brush brush, Rectangle rect, float startAngle, float sweepAngle) => FillPie(brush, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillPie(Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle) {
        string s = GDIArc2SVGPath(x, y, width, height, startAngle, sweepAngle, true);

        var pie = new SvgPathElement {
            D = s,
            Style = HandleBrush(brush)
        };
        if (!_transforms.Result.IsIdentity)
            pie.Transform = new SvgTransformList(_transforms.Result.Clone());

        _cur.AddChild(pie);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillPie(Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle) => FillPie(brush, x, y, width, (float)height, startAngle, sweepAngle);

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillPolygon(Brush brush, PointF[] points) =>
        // TODO: received shapes may not have the vertex list "normalized" correctly
        FillPolygon(brush, points, FillMode.Alternate);

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillPolygon(Brush brush, Point[] points) {
        PointF[] pts = Point2PointF(points);
        FillPolygon(brush, pts, FillMode.Alternate);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillPolygon(Brush brush, PointF[] points, FillMode fillMode) {
        var pl = new SvgPolygonElement(points) {
            Style = HandleBrush(brush)
        };
        if (fillMode == FillMode.Alternate)
            pl.Style.Set("fill-rule", "evenodd");
        else
            pl.Style.Set("fill-rule", "nonzero");

        if (!_transforms.Result.IsIdentity)
            pl.Transform = new SvgTransformList(_transforms.Result.Clone());
        _cur.AddChild(pl);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillPolygon(Brush brush, Point[] points, FillMode fillMode) {
        PointF[] pts = Point2PointF(points);
        FillPolygon(brush, pts, fillMode);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillRectangle(Brush brush, RectangleF rect) => FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillRectangle(Brush brush, float x, float y, float width, float height) {
        var rc = new SvgRectElement(x, y, width, height) {
            Style = HandleBrush(brush)
        };
        if (!_transforms.Result.IsIdentity)
            rc.Transform = _transforms.Result.Clone();
        _cur.AddChild(rc);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillRectangle(Brush brush, Rectangle rect) => FillRectangle(brush, rect.X, rect.Y, rect.Width, (float)rect.Height);

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillRectangle(Brush brush, int x, int y, int width, int height) => FillRectangle(brush, x, y, width, (float)height);

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillRectangles(Brush brush, RectangleF[] rects) {
        foreach (RectangleF rc in rects) FillRectangle(brush, rc);
    }

    /// <summary>
    /// Implemented
    /// </summary>
    public void FillRectangles(Brush brush, Rectangle[] rects) {
        foreach (Rectangle rc in rects) FillRectangle(brush, rc);
    }

    /// <summary>
    /// Not implemented, because GDI+ regions/paths are not emulated.
    /// </summary>
    public void FillRegion(Brush brush, Region region) => throw new SvgGdiNotImplementedException("FillRegion (Brush brush, Region region)");

    /// <summary>
    /// Does nothing
    /// </summary>
    public void Flush() {
        //nothing to do
    }

    /// <summary>
    /// Does nothing
    /// </summary>
    /// <param name="intention"></param>
    public void Flush(FlushIntention intention) {
        //nothing to do
    }

    /// <summary>
    /// Not meaningful when there is no actual display device.
    /// </summary>
    public Color GetNearestColor(Color color) => throw new SvgGdiNotImplementedException("GetNearestColor (Color color)");

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
    /// </summary>
    public void IntersectClip(Rectangle rect) => throw new SvgGdiNotImplementedException("IntersectClip (Rectangle rect)");

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
    /// </summary>
    public void IntersectClip(RectangleF rect) => throw new SvgGdiNotImplementedException("IntersectClip (RectangleF rect)");

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
    /// </summary>
    public void IntersectClip(Region region) => throw new SvgGdiNotImplementedException("IntersectClip (Region region)");

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
    /// </summary>
    public bool IsVisible(int x, int y) => throw new SvgGdiNotImplementedException("IsVisible (Int32 x, Int32 y)");

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
    /// </summary>
    public bool IsVisible(Point point) => throw new SvgGdiNotImplementedException("IsVisible (Point point)");

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
    /// </summary>
    public bool IsVisible(float x, float y) => throw new SvgGdiNotImplementedException("IsVisible (Single x, Single y)");

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
    /// </summary>
    public bool IsVisible(PointF point) => throw new SvgGdiNotImplementedException("IsVisible (PointF point)");

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
    /// </summary>
    public bool IsVisible(int x, int y, int width, int height) => throw new SvgGdiNotImplementedException("IsVisible (Int32 x, Int32 y, Int32 width, Int32 height)");

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
    /// </summary>
    public bool IsVisible(Rectangle rect) => throw new SvgGdiNotImplementedException("IsVisible (Rectangle rect)");

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
    /// </summary>
    public bool IsVisible(float x, float y, float width, float height) => throw new SvgGdiNotImplementedException("IsVisible (Single x, Single y, Single width, Single height)");

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
    /// </summary>
    public bool IsVisible(RectangleF rect) => throw new SvgGdiNotImplementedException("IsVisible (RectangleF rect)");

    /// <summary>
    ///  This method is implemented and produces a result which is often correct, but it is impossible to guarantee because 'MeasureString' is a fundamentally inapplicable to device independent output like SVG.
    /// </summary>
    public Region[] MeasureCharacterRanges(string text, Font font, RectangleF layoutRect, StringFormat stringFormat) => GetDefaultGraphics().MeasureCharacterRanges(text, font, layoutRect, stringFormat);

    /// <summary>
    /// This method is implemented and produces a result which is often correct, but it is impossible to guarantee because 'MeasureString' is a fundamentally inapplicable to device independent output like SVG.
    /// </summary>
    public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat, out int charactersFitted, out int linesFilled) => GetDefaultGraphics().MeasureString(text, font, layoutArea, stringFormat, out charactersFitted, out linesFilled);

    /// <summary>
    /// This method is implemented and produces a result which is often correct, but it is impossible to guarantee because 'MeasureString' is a fundamentally inapplicable to device independent output like SVG.
    /// </summary>
    public SizeF MeasureString(string text, Font font, PointF origin, StringFormat stringFormat) => GetDefaultGraphics().MeasureString(text, font, origin, stringFormat);

    /// <summary>
    /// This method is implemented and produces a result which is often correct, but it is impossible to guarantee because 'MeasureString' is a fundamentally inapplicable to device independent output like SVG.
    /// </summary>
    public SizeF MeasureString(string text, Font font, SizeF layoutArea) => GetDefaultGraphics().MeasureString(text, font, layoutArea);

    /// <summary>
    /// This method is implemented and produces a result which is often correct, but it is impossible to guarantee because 'MeasureString' is a fundamentally inapplicable to device independent output like SVG.
    /// </summary>
    public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat) => GetDefaultGraphics().MeasureString(text, font, layoutArea, stringFormat);

    /// <summary>
    ///  This method is implemented and produces a result which is often correct, but it is impossible to guarantee because 'MeasureString' is a fundamentally inapplicable to device independent output like SVG.
    /// </summary>
    public SizeF MeasureString(string text, Font font) => GetDefaultGraphics().MeasureString(text, font);

    /// <summary>
    ///  This method is implemented and produces a result which is often correct, but it is impossible to guarantee because 'MeasureString' is a fundamentally inapplicable to device independent output like SVG.
    /// </summary>
    public SizeF MeasureString(string text, Font font, int width) => GetDefaultGraphics().MeasureString(text, font, width);

    /// <summary>
    /// This method is implemented and produces a result which is often correct, but it is impossible to guarantee because 'MeasureString' is a fundamentally inapplicable to device independent output like SVG.
    /// </summary>
    public SizeF MeasureString(string text, Font font, int width, StringFormat format) => GetDefaultGraphics().MeasureString(text, font, width, format);

    /// <summary>
    /// Implemented
    /// </summary>
    public void MultiplyTransform(Matrix matrix) => _transforms.Top.Multiply(matrix);

    /// <summary>
    /// Implemented, but ignores <c>order</c>
    /// </summary>
    public void MultiplyTransform(Matrix matrix, MatrixOrder order) => _transforms.Top.Multiply(matrix, order);

    /// <summary>
    /// Implemented.
    /// </summary>
    public void ResetClip() => _cur.Style.Set("clip-path", null);

    /// <summary>
    /// Implemented
    /// </summary>
    public void ResetTransform() => _transforms.ResetTransform();

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
    /// </summary>
    public void Restore(GraphicsState gstate) => throw new SvgGdiNotImplementedException("Restore (GraphicsState gstate)");

    /// <summary>
    /// Implemented
    /// </summary>
    public void RotateTransform(float angle) => _transforms.Top.Rotate(angle);

    /// <summary>
    /// Implemented, but ignores <c>order</c>
    /// </summary>
    public void RotateTransform(float angle, MatrixOrder order) => _transforms.Top.Rotate(angle, order);

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
    /// </summary>
    public GraphicsState Save() => throw new SvgGdiNotImplementedException("Save ()");

    /// <summary>
    /// Implemented
    /// </summary>
    public void ScaleTransform(float sx, float sy) => _transforms.Top.Scale(sx, sy);

    /// <summary>
    /// Implemented, but ignores <c>order</c>
    /// </summary>
    public void ScaleTransform(float sx, float sy, MatrixOrder order) => _transforms.Top.Scale(sx, sy, order);

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void SetClip(Graphics g) => throw new SvgGdiNotImplementedException("SetClip (Graphics g)");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void SetClip(Graphics g, CombineMode combineMode) => throw new SvgGdiNotImplementedException("SetClip (Graphics g, CombineMode combineMode)");

    /// <summary>
    /// Implemented.
    /// </summary>
    public void SetClip(Rectangle rect) => SetClip(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height));

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void SetClip(Rectangle rect, CombineMode combineMode) => throw new SvgGdiNotImplementedException("SetClip (Rectangle rect, CombineMode combineMode)");

    /// <summary>
    /// Implemented.
    /// </summary>
    public void SetClip(RectangleF rect) {
        var clipper = new SvgClipPathElement();
        clipper.Id += "_SetClip";
        var rc = new SvgRectElement(rect.X, rect.Y, rect.Width, rect.Height);
        clipper.AddChild(rc);
        _defs.AddChild(clipper);

        _cur.Style.Set("clip-path", new SvgUriReference(clipper));
    }

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
    /// </summary>
    public void SetClip(RectangleF rect, CombineMode combineMode) => throw new SvgGdiNotImplementedException("SetClip (RectangleF rect, CombineMode combineMode)");

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
    /// </summary>
    public void SetClip(GraphicsPath path) => throw new SvgGdiNotImplementedException("SetClip (GraphicsPath path)");

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
    /// </summary>
    public void SetClip(GraphicsPath path, CombineMode combineMode) => throw new SvgGdiNotImplementedException("SetClip (GraphicsPath path, CombineMode combineMode)");

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.  Only rectangular clip regions work.
    /// </summary>
    public void SetClip(Region region, CombineMode combineMode) => throw new SvgGdiNotImplementedException("SetClip (Region region, CombineMode combineMode)");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, PointF[] pts) => throw new SvgGdiNotImplementedException("TransformPoints (CoordinateSpace destSpace, CoordinateSpace srcSpace, PointF[] pts)");

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, Point[] pts) => throw new SvgGdiNotImplementedException("TransformPoints (CoordinateSpace destSpace, CoordinateSpace srcSpace, Point[] pts)");

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
    /// </summary>
    public void TranslateClip(float dx, float dy) => throw new SvgGdiNotImplementedException("TranslateClip (Single dx, Single dy)");

    /// <summary>
    /// Not implemented, because SvgGdi does not try and emulate GDI+ regions.
    /// </summary>
    public void TranslateClip(int dx, int dy) => throw new SvgGdiNotImplementedException("TranslateClip (Int32 dx, Int32 dy)");

    /// <summary>
    /// Implemented
    /// </summary>
    public void TranslateTransform(float dx, float dy) => _transforms.Top.Translate(dx, dy);

    /// <summary>
    /// Implemented, but ignores <c>order</c>
    /// </summary>
    public void TranslateTransform(float dx, float dy, MatrixOrder order) => _transforms.Top.Translate(dx, dy, order);

    //a default graphics so that we can make a guess as to functions like MeasureString
    private static Graphics _g;
    private readonly SvgRectElement _bg;
    private readonly SvgDefsElement _defs;
    private readonly SvgSvgElement _root;
    private readonly SvgGroupElement _topgroup;
    private readonly MatrixStack _transforms;
    private SvgStyledTransformedElement _cur;

    private static void AddHatchBrushDetails(SvgPatternElement patty, SvgColor col, HatchStyle hs) {
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
                l3 = new SvgLineElement(7, 9, 9, 7);
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

    private static PointF ControlPoint(PointF l, PointF pt, float t) {
        var v = new PointF(l.X - pt.X, l.Y - pt.Y);

        float vlen = (float)Math.Sqrt((v.X * v.X) + (v.Y * v.Y));
        v.X /= (float)Math.Sqrt(vlen / (10 * t * t));
        v.Y /= (float)Math.Sqrt(vlen / (10 * t * t));

        return new PointF(pt.X + v.X, pt.Y + v.Y);
    }

    private static PointF[] ControlPoints(PointF l, PointF r, PointF pt, float t) {
        //points to vectors
        var lv = new PointF(l.X - pt.X, l.Y - pt.Y);
        var rv = new PointF(r.X - pt.X, r.Y - pt.Y);

        var nlv = new PointF(lv.X - rv.X, lv.Y - rv.Y);
        var nrv = new PointF(rv.X - lv.X, rv.Y - lv.Y);

        float nlvlen = (float)Math.Sqrt((nlv.X * nlv.X) + (nlv.Y * nlv.Y));
        nlv.X /= (float)Math.Sqrt(nlvlen / (10 * t * t));
        nlv.Y /= (float)Math.Sqrt(nlvlen / (10 * t * t));

        float nrvlen = (float)Math.Sqrt((nrv.X * nrv.X) + (nrv.Y * nrv.Y));
        nrv.X /= (float)Math.Sqrt(nrvlen / (10 * t * t));
        nrv.Y /= (float)Math.Sqrt(nrvlen / (10 * t * t));

        var ret = new PointF[2];

        ret[0] = new PointF(pt.X + nlv.X, pt.Y + nlv.Y);
        ret[1] = new PointF(pt.X + nrv.X, pt.Y + nrv.Y);

        return ret;
    }

    private static void DrawImagePixel(SvgElement container, Color c, float x, float y, float w, float h) {
        if (c.A == 0)
            return;

        var rc = new SvgRectElement(x, y, w, h) {
            Id = ""
        };
        rc.Style.Set("fill", "rgb(" + c.R + "," + c.G + "," + c.B + ")");
        if (c.A < 255)
            rc.Style.Set("opacity", c.A / 255f);

        container.AddChild(rc);
    }

    private static string GDIArc2SVGPath(float x, float y, float width, float height, float startAngle, float sweepAngle, bool pie) {
        int longArc = 0;

        var start = new PointF();
        var end = new PointF();
        var center = new PointF(x + (width / 2f), y + (height / 2f));

        startAngle = startAngle / 360f * 2f * (float)Math.PI;
        sweepAngle = sweepAngle / 360f * 2f * (float)Math.PI;

        sweepAngle += startAngle;

#pragma warning disable IDE0180
        if (sweepAngle > startAngle) {
            float temp = startAngle;
            startAngle = sweepAngle;
            sweepAngle = temp;
        }
#pragma warning restore IDE0180

        if (sweepAngle - startAngle > Math.PI || startAngle - sweepAngle > Math.PI) longArc = 1;

        start.X = ((float)Math.Cos(startAngle) * (width / 2f)) + center.X;
        start.Y = ((float)Math.Sin(startAngle) * (height / 2f)) + center.Y;

        end.X = ((float)Math.Cos(sweepAngle) * (width / 2f)) + center.X;
        end.Y = ((float)Math.Sin(sweepAngle) * (height / 2f)) + center.Y;

        string s = "M " + start.X.ToString("F", CultureInfo.InvariantCulture) + "," + start.Y.ToString("F", CultureInfo.InvariantCulture) +
            " A " + (width / 2f).ToString("F", CultureInfo.InvariantCulture) + " " + (height / 2f).ToString("F", CultureInfo.InvariantCulture) + " " +
            "0 " + longArc.ToString() + " 0 " + end.X.ToString("F", CultureInfo.InvariantCulture) + " " + end.Y.ToString("F", CultureInfo.InvariantCulture);

        if (pie) {
            s += " L " + center.X.ToString("F", CultureInfo.InvariantCulture) + "," + center.Y.ToString("F", CultureInfo.InvariantCulture);
            s += " L " + start.X.ToString("F", CultureInfo.InvariantCulture) + "," + start.Y.ToString("F", CultureInfo.InvariantCulture);
        }

        return s;
    }

    private static Graphics GetDefaultGraphics() {
        if (_g == null) {
            var b = new Bitmap(1, 1);
            _g = Graphics.FromImage(b);
        }

        return _g;
    }

    private static float GetFontDescentPercentage(Font font) => (float)font.FontFamily.GetCellDescent(font.Style) / font.FontFamily.GetEmHeight(font.Style);

    private static PointF[] Point2PointF(Point[] p) {
        var pf = new PointF[p.Length];
        for (int i = 0; i < p.Length; ++i) pf[i] = new PointF(p[i].X, p[i].Y);

        return pf;
    }

    //This seems to be a very good approximation.  GDI must be using a similar simplistic method for some odd reason.
    //If a curve is closed, it uses all points, and ignores start and num.
    private static PointF[] Spline2Bez(PointF[] points, int start, int num, bool closed, float tension) {
        var res = new ArrayList();

        int l = points.Length - 1;

        _ = res.Add(points[0]);
        _ = res.Add(ControlPoint(points[1], points[0], tension));

        for (int i = 1; i < l; ++i) {
            PointF[] pts = ControlPoints(points[i - 1], points[i + 1], points[i], tension);
            _ = res.Add(pts[0]);
            _ = res.Add(points[i]);
            _ = res.Add(pts[1]);
        }

        _ = res.Add(ControlPoint(points[l - 1], points[l], tension));
        _ = res.Add(points[l]);

        if (closed) {
            //adjust rh cp of point 0
            PointF[] pts = ControlPoints(points[l], points[1], points[0], tension);
            res[1] = pts[1];

            //adjust lh cp of point l and add rh cp
            pts = ControlPoints(points[l - 1], points[0], points[l], tension);
            res[res.Count - 2] = pts[0];
            _ = res.Add(pts[1]);

            //add new end point and its lh cp
            pts = ControlPoints(points[l], points[1], points[0], tension);
            _ = res.Add(pts[0]);
            _ = res.Add(points[0]);

            return (PointF[])res.ToArray(typeof(PointF));
        } else {
            var subset = new ArrayList();

            for (int i = start * 3; i < (start + num) * 3; ++i) _ = subset.Add(res[i]);

            _ = subset.Add(res[(start + num) * 3]);

            return (PointF[])subset.ToArray(typeof(PointF));
        }
    }

    private void DrawBitmapData(Bitmap b, float x, float y, float w, float h, bool scale) {
        SvgGroupElement groupElement = new BitmapDrawer(
            new SvgGroupElement("bitmap_at_" + x.ToString("F", CultureInfo.InvariantCulture) + "_" + y.ToString("F", CultureInfo.InvariantCulture)),
            x,
            y,
            scaleX: scale ? w / b.Width : 1,
            scaleY: scale ? h / b.Height : 1)
            .DrawBitmapData(b);
        if (!_transforms.Result.IsIdentity)
            groupElement.Transform = _transforms.Result.Clone();
        _cur.AddChild(groupElement);
    }

    private void DrawBitmapImage(Image image, float x, float y, float width, float height) {
        if (image is Bitmap bmp)
            DrawBitmapData(bmp, x, y, width, height, true);
    }

    private void DrawBitmapImageUnscaled(Image image, float x, float y) {
        if (image is Bitmap bmp)
            DrawBitmapData(bmp, x, y, image.Width, image.Height, false);
    }

    private void DrawEndAnchor(LineCap lc, Color col, float w, PointF pt, float angle) {
        SvgStyledTransformedElement anchor;

        switch (lc) {
            case LineCap.ArrowAnchor:
                anchor = new SvgPolygonElement(new PointF(0, -w / 2f), new PointF(-w, w), new PointF(w, w));
                break;

            case LineCap.DiamondAnchor:
                anchor = new SvgPolygonElement(new PointF(0, -w), new PointF(w, 0), new PointF(0, w), new PointF(-w, 0));
                break;

            case LineCap.RoundAnchor:
                anchor = new SvgEllipseElement(0, 0, w, w);
                break;

            case LineCap.SquareAnchor:
                float ww = w / 3 * 2;
                anchor = new SvgRectElement(0 - ww, 0 - ww, ww * 2, ww * 2);
                break;

            case LineCap.NoAnchor:
            case LineCap.Flat:
            case LineCap.Custom:
            default:
                return;
        }

        if (anchor == null)
            return;

        anchor.Id += "_line_anchor";
        anchor.Style.Set("fill", new SvgColor(col));
        anchor.Style.Set("stroke", "none");

        var rotation = new Matrix();
        rotation.Rotate(angle / (float)Math.PI * 180);
        var translation = new Matrix();
        translation.Translate(pt.X, pt.Y);

        anchor.Transform = new SvgTransformList(_transforms.Result.Clone());
        anchor.Transform.Add(translation);
        anchor.Transform.Add(rotation);
        _cur.AddChild(anchor);
    }

    private void DrawEndAnchors(Pen pen, PointF start, PointF end) {
        float startAngle = (float)Math.Atan((start.X - end.X) / (start.Y - end.Y)) * -1;
        float endAngle = (float)Math.Atan((end.X - start.X) / (end.Y - start.Y)) * -1;

        DrawEndAnchor(pen.StartCap, pen.Color, pen.Width, start, startAngle);
        DrawEndAnchor(pen.EndCap, pen.Color, pen.Width, end, endAngle);
    }

    private void DrawText(string s, Font font, Brush brush, RectangleF rect, StringFormat fmt, bool ignoreRect) {
        if (s?.Contains('\n') == true)
            throw new SvgGdiNotImplementedException("DrawText multiline text");

        var txt = new SvgTextElement(s, new SvgLength(rect.X, SvgLengthType.SVG_LENGTHTYPE_PX), new SvgLength(rect.Y, SvgLengthType.SVG_LENGTHTYPE_PX)) {
            //GDI takes x and y as the upper left corner; svg takes them as the lower left.
            //We must therefore move the text one line down, but SVG does not understand about lines,
            //so we do as best we can, applying a downward translation before the current GDI translation.

            Transform = new SvgTransformList(_transforms.Result.Clone()),

            Style = HandleBrush(brush)
        };
        txt.Style += new SvgStyle(font);

        switch (fmt.Alignment) {
            case StringAlignment.Near:
                break;

            case StringAlignment.Center: {
                    if (ignoreRect)
                        throw new SvgGdiNotImplementedException("DrawText automatic rect");

                    txt.Style.Set("text-anchor", "middle");
                    txt.X = rect.X + (rect.Width / 2);
                }
                break;

            case StringAlignment.Far: {
                    if (ignoreRect)
                        throw new SvgGdiNotImplementedException("DrawText automatic rect");

                    txt.Style.Set("text-anchor", "end");
                    txt.X = rect.Right;
                }
                break;

            default:
                throw new SvgGdiNotImplementedException("DrawText horizontal alignment");
        }

        if (!ignoreRect && (fmt.FormatFlags & StringFormatFlags.NoClip) != StringFormatFlags.NoClip) {
            var clipper = new SvgClipPathElement();
            clipper.Id += "_text_clipper";
            var rc = new SvgRectElement(rect.X, rect.Y, rect.Width, rect.Height);
            clipper.AddChild(rc);
            _defs.AddChild(clipper);

            txt.Style.Set("clip-path", new SvgUriReference(clipper));
        }

        switch (fmt.LineAlignment) {
            case StringAlignment.Near: {
                    // TODO: ??
                    // txt.Style.Set("baseline-shift", "-86%");//a guess.
                    var span = new SvgTspanElement(s) {
                        DY = new SvgLength(txt.Style.Get("font-size").ToString())
                    };
                    txt.Text = null;
                    txt.AddChild(span);
                }
                break;

            case StringAlignment.Center: {
                    if (ignoreRect)
                        throw new SvgGdiNotImplementedException("DrawText automatic rect");

                    txt.Y.Value += rect.Height / 2;
                    var span = new SvgTspanElement(s) {
                        DY = new SvgLength(txt.Style.Get("font-size").ToString())
                    };
                    span.DY.Value *= 1 - GetFontDescentPercentage(font) - 0.5f;
                    txt.Text = null;
                    txt.AddChild(span);
                }
                break;

            case StringAlignment.Far: {
                    if (ignoreRect)
                        throw new SvgGdiNotImplementedException("DrawText automatic rect");

                    txt.Y.Value += rect.Height;
                    // This would solve the alignment as well, but it's not supported by Internet Explorer
                    //
                    // txt.Attributes["dominant-baseline"] = "text-after-edge";
                    var span = new SvgTspanElement(s) {
                        DY = new SvgLength(txt.Style.Get("font-size").ToString())
                    };
                    span.DY.Value *= 1 - GetFontDescentPercentage(font) - 1;
                    txt.Text = null;
                    txt.AddChild(span);
                }
                break;

            default:
                throw new SvgGdiNotImplementedException("DrawText vertical alignment");
        }

        _cur.AddChild(txt);
    }

    private void FillBeziers(Brush brush, PointF[] points, FillMode fillMode) {
        var bez = new SvgPathElement();

        string s = "M " + points[0].X.ToString("F", CultureInfo.InvariantCulture) + " " + points[0].Y.ToString("F", CultureInfo.InvariantCulture) + " C ";

        for (int i = 1; i < points.Length; ++i) s += points[i].X.ToString("F", CultureInfo.InvariantCulture) + " " + points[i].Y.ToString("F", CultureInfo.InvariantCulture) + " ";

        s += "Z";

        bez.D = s;

        bez.Style = HandleBrush(brush);
        bez.Transform = new SvgTransformList(_transforms.Result.Clone());
        if (fillMode == FillMode.Alternate) bez.Style.Set("fill-rule", "evenodd");
        else bez.Style.Set("fill-rule", "nonzero");
        _cur.AddChild(bez);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
    private SvgStyle HandleBrush(Brush br) {
        if (br is SolidBrush sbr) return new SvgStyle(sbr);

        if (br is LinearGradientBrush grbr) {
            RectangleF rc = grbr.Rectangle;
            var grad = new SvgLinearGradientElement(rc.Left, rc.Top, rc.Right, rc.Bottom);
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

            if (cb != null) for (int i = 0; i < grbr.InterpolationColors.Colors.Length; ++i) grad.AddChild(new SvgStopElement(grbr.InterpolationColors.Positions[i], grbr.InterpolationColors.Colors[i]));
            else {
                grad.AddChild(new SvgStopElement("0%", grbr.LinearColors[0]));
                grad.AddChild(new SvgStopElement("100%", grbr.LinearColors[1]));
            }

            grad.Id += "_LinearGradientBrush";

            _defs.AddChild(grad);

            var s = new SvgStyle();
            s.Set("fill", new SvgUriReference(grad));
            return s;
        }

        if (br is HatchBrush habr) {
            var patty = new SvgPatternElement(0, 0, 8, 8, null);
            patty.Style.Set("shape-rendering", "crispEdges");
            patty.Style.Set("stroke-linecap", "butt");

            var rc = new SvgRectElement(0, 0, 8, 8);
            rc.Style.Set("fill", new SvgColor(habr.BackgroundColor));
            patty.AddChild(rc);

            AddHatchBrushDetails(patty, new SvgColor(habr.ForegroundColor), habr.HatchStyle);

            patty.Id += "_HatchBrush";
            patty.PatternUnits = "userSpaceOnUse";
            patty.PatternContentUnits = "userSpaceOnUse";
            _defs.AddChild(patty);

            var s = new SvgStyle();
            s.Set("fill", new SvgUriReference(patty));
            return s;
        }

        //most types of brush we can't emulate, but luckily they are quite unusual
        return new SvgStyle(new SolidBrush(Color.Salmon));
    }

    private static SvgPath HandleGraphicsPath(GraphicsPath path) {
        var pathBuilder = new StringBuilder();
        using var subpaths = new GraphicsPathIterator(path);
        using var subpath = new GraphicsPath(path.FillMode);
        subpaths.Rewind();

        //Iterate through all the subpaths in the path. Each subpath will contain either
        //lines or Bezier curves
        for (int s = 0; s < subpaths.SubpathCount; s++) {
            if (subpaths.NextSubpath(subpath, out bool isClosed) == 0) continue; //go to next subpath if this one has zero points.

            PathPointType lastType = PathPointType.Start;
            for (int i = 0; i < subpath.PathPoints.Length; i++) {
                /* Each subpath point has a corresponding path point type which can be:
                 *The point starts the subpath
                 *The point is a line point
                 *The point is Bezier curve point
                 */
                PointF point = subpath.PathPoints[i];
                PathPointType pathType = (PathPointType)subpath.PathTypes[i] & PathPointType.PathTypeMask;
                switch (pathType) //Mask off non path-type types
                {
                    case PathPointType.Start:
                        //Move to start point
                        if (pathBuilder.Length > 0) _ = pathBuilder.Append(' ');
                        _ = pathBuilder.AppendFormat(CultureInfo.InvariantCulture, "M {0},{1}", point.X, point.Y);
                        break;

                    case PathPointType.Line:
                        // Draw line to current point
                        if (lastType != PathPointType.Line) _ = pathBuilder.Append(" L");
                        _ = pathBuilder.AppendFormat(CultureInfo.InvariantCulture, " {0},{1}", point.X, point.Y);
                        break;

                    case PathPointType.Bezier3:
                        // Draw curve to current point
                        if (lastType != PathPointType.Bezier3) _ = pathBuilder.Append(" C");
                        _ = pathBuilder.AppendFormat(CultureInfo.InvariantCulture, " {0},{1}", point.X, point.Y);
                        break;

                    default:
                        continue;
                }

                lastType = pathType;
            }

            if (isClosed)                     // Close path
                _ = pathBuilder.Append(" Z");
        }

        return new SvgPath(pathBuilder.ToString());
    }

    /// <summary>
    /// This class is needed because GDI+ does not maintain a proper scene graph; rather it maintains a single transformation matrix
    /// which is applied to each new object.  The matrix is saved and reloaded when 'begincontainer' and 'endcontainer' are called.  SvgGraphics has to
    /// emulate this behaviour.
    /// <para>
    /// This matrix stack caches it's 'result' (ie. the current transformation, the product of all matrices).  The result is
    /// recalculated when necessary.
    /// </para>
    /// </summary>
    private sealed class MatrixStack : IDisposable {
        public MatrixStack() {
            _mx = [];

            //we need 2 identity matrices on the stack.  This is because we do a resettransform()
            //by pop dup (to set current xform to xform of enclosing group).
            PushIdentity();
            PushIdentity();
        }

        public Matrix Result => _result ??= MatrixExtensions.MultiplyAll(_mx);

        public Matrix Top {
            get {
                //because we cannot return a const, we have to reset result
                //even though the caller might not even want to change the matrix.  This a typical
                //problem with weaker languages that don't have const.
                _result = null;
                return _mx[_mx.Count - 1];
            }
        }

        internal void SetTop(Matrix value) {
            _mx[_mx.Count - 1] = value;
            _result = null;
        }
        public void ResetTransform() {
            Pop();
            _mx.Insert(_mx.Count, Top.Clone());
        }

        public void Pop() {
            if (_mx.Count <= 1)
                return;

            _mx.RemoveAt(_mx.Count - 1);
            _result = null;
        }

        public void PushIdentity() => _mx.Add(new Matrix());
        public void Dispose() => _result?.Dispose();

        private readonly List<Matrix> _mx;
        private Matrix _result;
    }
}

