/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Exceptions;
/// <summary>
/// A general-purpose exception for problems that occur in SvgNet.
/// </summary>
[Serializable]
public class SvgException : Exception {
    public SvgException(string msg, string ctx) : base(msg) {
        Msg = msg;
        Ctx = ctx;
    }

    public SvgException(string msg) : this(msg, "") {
    }

    public SvgException() : this("", "") {
    }

    public SvgException(string msg, Exception innerException) : this(msg, "", innerException) {
    }

    public SvgException(string msg, string ctx, Exception innerException) : base(msg, innerException) {
        Msg = msg;
        Ctx = ctx;
    }

    /// <summary>
    /// A string intended to supply context information.
    /// </summary>
    public string Ctx { get; }

    /// <summary>
    /// A message describing the problem.
    /// </summary>
    public string Msg { get; }
}
