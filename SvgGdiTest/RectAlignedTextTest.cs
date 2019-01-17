/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2019 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using SvgNet.SvgGdi;
using System.Drawing;

public static class RectAlignedTextTest
{
    public static void RenderRectAlignedText(IGraphics ig, float width, float height, Font baseFont)
    {
        ig.Clear(Color.White);
        ig.ScaleTransform(width / CanvasSize, height / CanvasSize);
        DrawTest(ig, baseFont);
    }

    private static int RectFontSize = 20;
    private static int RectGap = 20;
    private static int RectSize = 150;
    private static int CanvasSize => 3 * RectSize + 4 * RectGap;

    private static void DrawRect(IGraphics canvas, string id, Rectangle rect, StringAlignment horizontalAlignment, StringAlignment verticalAlignment, Font baseFont)
    {
        var format = new StringFormat {
            Alignment = horizontalAlignment,
            LineAlignment = verticalAlignment,
            FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip
        };

        var pen = new Pen(new SolidBrush(Color.Black), 1);
        canvas.DrawRectangle(pen, rect);

        var font = new Font(baseFont.Name, RectFontSize, baseFont.Style, baseFont.Unit);

        {
            // Draw label
            var labelFormat = new StringFormat {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip
            };
            var labelRect = new Rectangle(rect.X, rect.Y - RectGap, RectGap, RectGap);
            var labelFont = new Font(baseFont.Name, RectFontSize * 0.8f, baseFont.Style, baseFont.Unit);
            canvas.DrawString(id, labelFont, new SolidBrush(Color.Black), labelRect, labelFormat);
        }

        canvas.DrawString("Helloy", font, new SolidBrush(Color.Blue), rect, format);
    }

    private static void DrawTest(IGraphics canvas, Font baseFont)
    {
        canvas.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, CanvasSize, CanvasSize));

        var alignments = new StringAlignment[] { StringAlignment.Near, StringAlignment.Center, StringAlignment.Far };

        int id = 1;
        foreach (var verticalAlignment in alignments) {
            foreach (var horizontalAlignment in alignments) {
                var x = RectGap + ((int)horizontalAlignment) * (RectSize + RectGap);
                var y = RectGap + ((int)verticalAlignment) * (RectSize + RectGap);
                var rect = new Rectangle(x, y, RectSize, RectSize);
                DrawRect(canvas, id.ToString(), rect, horizontalAlignment, verticalAlignment, baseFont);
                id++;
            }
        }
    }
}