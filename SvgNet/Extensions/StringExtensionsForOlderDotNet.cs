/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet;

#if !(NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER)
public static class StringExtensionsForOlderDotNet {
    public static bool Contains(this string s, char c)
        => s.IndexOf(c) >= 0;
}
#endif

