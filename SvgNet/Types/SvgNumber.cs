/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Types;
/// <summary>
/// A number, as specified in the SVG standard.  It is stored as a float.
/// </summary>
public class SvgNumber : ICloneable {
    public SvgNumber(string s) => FromString(s);

    public SvgNumber(int n) => _num = n;

    public SvgNumber(float n) => _num = n;

    public static implicit operator SvgNumber(string s) => new(s);

    public static implicit operator SvgNumber(int n) => new(n);

    public static implicit operator SvgNumber(float n) => new(n);

    public object Clone() => new SvgNumber(_num);

    /// <summary>
    /// float.Parse is used to parse the string.  float.Parse does not follow the exact rules of the SVG spec.
    /// </summary>
    /// <param name="s"></param>
    public void FromString(string s) {
        try {
            _num = float.Parse(s, CultureInfo.InvariantCulture);
        } catch {
            throw new SvgException("Invalid SvgNumber", s);
        }
    }

    /// <summary>
    /// float.ToString is used to output a string.  This is true for all numbers in SvgNet.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => _num.ToString("F", CultureInfo.InvariantCulture);

    private float _num;
}
