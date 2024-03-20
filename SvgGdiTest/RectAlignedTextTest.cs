/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System.Drawing;

using SvgNet.Interfaces;

namespace SvgGdiTest;
public static class RectAlignedTextTest {
    public static void RenderRectAlignedText(IGraphics ig, float width, float height, Font baseFont) {
        ig.Clear(Color.White);
        ig.ScaleTransform(width / _canvasSize, height / _canvasSize);
        DrawTest(ig, baseFont);
    }

    private const int _canvasSize = (3 * _rectSize) + (4 * _rectGap);
    private const int _rectFontSize = 20;
    private const int _rectGap = 20;
    private const int _rectSize = 150;

    private static void DrawRect(IGraphics canvas, string id, Rectangle rect, StringAlignment horizontalAlignment, StringAlignment verticalAlignment, Font baseFont) {
        var format = new StringFormat {
            Alignment = horizontalAlignment,
            LineAlignment = verticalAlignment,
            FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip
        };

        var pen = new Pen(new SolidBrush(Color.Black), 1);
        canvas.DrawRectangle(pen, rect);

        var font = new Font(baseFont.Name, _rectFontSize, baseFont.Style, baseFont.Unit);

        {
            // Draw label
            var labelFormat = new StringFormat {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip
            };
            var labelRect = new Rectangle(rect.X, rect.Y - _rectGap, _rectGap, _rectGap);
            var labelFont = new Font(baseFont.Name, _rectFontSize * 0.8f, baseFont.Style, baseFont.Unit);
            canvas.DrawString(id, labelFont, new SolidBrush(Color.Black), labelRect, labelFormat);
        }

        canvas.DrawString("Helloy", font, new SolidBrush(Color.Blue), rect, format);
    }

    private static void DrawTest(IGraphics canvas, Font baseFont) {
        canvas.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, _canvasSize, _canvasSize));

        var alignments = new StringAlignment[] { StringAlignment.Near, StringAlignment.Center, StringAlignment.Far };

        int id = 1;
        foreach (StringAlignment verticalAlignment in alignments)
            foreach (StringAlignment horizontalAlignment in alignments) {
                int x = _rectGap + ((int)horizontalAlignment * (_rectSize + _rectGap));
                int y = _rectGap + ((int)verticalAlignment * (_rectSize + _rectGap));
                var rect = new Rectangle(x, y, _rectSize, _rectSize);
                DrawRect(canvas, id.ToString(), rect, horizontalAlignment, verticalAlignment, baseFont);
                id++;
            }
    }
}