/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace System;

public static class StringExtensions {
    public static int ParseHex(this string s, int startIndex, int length = 1)
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        => int.Parse(s.AsSpan(startIndex, length), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
#else
        => int.Parse(s.Substring(startIndex, length), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
#endif

    public static string SkipFirst(this string s)
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        => s[1..];
#else
        => s.Substring(1);
#endif

#if NET8_0_OR_GREATER
    private static readonly Buffers.SearchValues<char> digits = Buffers.SearchValues.Create(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
#else
    private static readonly char[] digits = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
#endif
    public static bool TrySplitNumberAndSuffix(this string s, out string number, out string suffix) {
#if NET8_0_OR_GREATER
        int i = s.AsSpan().LastIndexOfAny(digits) + 1;
#else
        int i = s.LastIndexOfAny(digits) + 1;
#endif
        suffix = number = null;
        if (i == 0)
            return false;
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        number = s[..i];
        suffix = s[i..];
#else
        number = s.Substring(0, i);
        suffix = s.Substring(i);
#endif
        return true;
    }

    public static bool TryParseTransformation(this string s, out string name, out float[] points) {
        int idx = s.IndexOf('(');
        int idx2 = s.IndexOf(')');
        if (idx == -1 || idx2 <= idx) {
            name = null;
            points = null;
            return false;
        }
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        name = s[..idx];
#else
        name = s.Substring(0, idx);
#endif
        name = name.Trim(' ', ',');
        points = SvgNumList.String2Floats(s.Substring(idx + 1, idx2 - idx - 1));
        return true;
    }
}
