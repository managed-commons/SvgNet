/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2022 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using SvgNet.Elements;

namespace SvgNet;

public sealed partial class SvgGraphics {
    private class BitmapDrawer {
        public BitmapDrawer(SvgGroupElement g, float x, float y, float scaleX, float scaleY) {
            _groupElement = g;
            _x = x;
            _y = y;
            _scaleX = scaleX;
            _scaleY = scaleY;
        }

        private readonly SvgGroupElement _groupElement;
        private readonly float _x;
        private readonly float _y;
        private readonly float _scaleX;
        private readonly float _scaleY;

        public SvgGroupElement DrawBitmapData(Bitmap b) {
            for (int line = 0; line < b.Height; ++line) {
                float scaledLine = _y + (line * _scaleY);
                // Only draws the last 'set' of pixels when a new color is encountered or it's the last pixel in the line.
                Color currentColor = GetPixelColor(b, line, 0);
                int consecutive = 1;
                for (int col = 0; col < b.Width; ++col) {
                    try {
                        if (col == b.Width - 1)
                            DrawPixel(scaledLine, col, consecutive, currentColor);
                        else {
                            // This is SO slow, but better than making the whole library 'unsafe'
                            Color nextColor = GetPixelColor(b, line, col + 1);
                            if (nextColor != currentColor) {
                                DrawPixel(scaledLine, col, consecutive, currentColor);
                                currentColor = nextColor;
                                consecutive = 1;
                            } else consecutive++;
                        }
                    } catch { }
                }
            }
            return _groupElement;
        }

        // This could be optimized in an unsafe version of the lib
        private static Color GetPixelColor(Bitmap b, int y, int x) => b.GetPixel(x, y);

        private void DrawPixel(float scaledLine, int col, int consecutive, Color color) =>
            DrawImagePixel(_groupElement,
                            color,
                            _x + ((col - consecutive - 1) * _scaleX),
                            scaledLine,
                            consecutive * _scaleX,
                            _scaleY);
    }
}

