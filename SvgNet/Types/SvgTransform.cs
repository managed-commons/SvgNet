/*
	Copyright © 2003 RiskCare Ltd. All rights reserved.
	Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
	Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

	Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Types;
/// <summary>
/// Represents a single element in an SVG transformation list.  The transformation is represented internally as a
/// GDI+ Matrix object.
/// </summary>
public class SvgTransform : ICloneable {
    public SvgTransform() => Matrix = new Matrix();

    public SvgTransform(string s) => FromString(s);

    public SvgTransform(Matrix m) => Matrix = m;

    public Matrix Matrix { get; set; }

    public object Clone() => new SvgTransform(Matrix.Clone());

    /// <summary>
    /// Parse a transformation according to the SVG standard.  This is complex enough that it makes
    /// me wish it was worth using a real parser, but antlr is so unwieldy.
    /// </summary>
    public void FromString(string s) {
        Matrix = new Matrix();

        if (s.TryParseTransformation(out string name, out float[] points))
            switch (name) {
                case "matrix":
                    if (points.Length == 6) {
                        Matrix = new Matrix(points[0], points[1], points[2], points[3], points[4], points[5]);
                        return;
                    }
                    break;
                case "translate":
                    if (points.Length == 1) {
                        Matrix.Translate(points[0], 0);
                        return;
                    }
                    if (points.Length == 2) {
                        Matrix.Translate(points[0], points[1]);
                        return;
                    }
                    break;
                case "scale":
                    if (points.Length == 1) {
                        Matrix.Scale(points[0], 0);
                        return;
                    }
                    if (points.Length == 2) {
                        Matrix.Scale(points[0], points[1]);
                        return;
                    }
                    break;
                case "rotate":
                    if (points.Length == 1) {
                        Matrix.Rotate(points[0]);
                        return;
                    }
                    if (points.Length == 3) {
                        Matrix.Translate(points[1], points[2]);
                        Matrix.Rotate(points[0]);
                        Matrix.Translate(points[1] * -1, points[2] * -1);
                        return;
                    }
                    break;
                case "skewX":
                    if (points.Length == 1) {
                        Matrix.Shear(points[0], 0);
                        return;
                    }
                    break;
                case "skewY":
                    if (points.Length == 1) {
                        Matrix.Shear(0, points[0]);
                        return;
                    }
                    break;
                default:
                    throw new SvgException($"Invalid SvgTransformation: unrecognized name '{name}'", s);

            }
        throw new SvgException("Invalid SvgTransformation", s);
    }

    /// <summary>
    /// Currently, we always output as matrix() no matter how the transform was specified.
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
        string result = "matrix(";

        foreach (float f in Matrix.Elements) {
            result += f.ToString("F", CultureInfo.InvariantCulture);
            result += " ";
        }

        result += ")";

        return result;
    }
}
