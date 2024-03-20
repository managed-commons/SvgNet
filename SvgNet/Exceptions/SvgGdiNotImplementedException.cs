/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.Exceptions;
/// <summary>
/// Exception thrown when a GDI+ operation is attempted on an IGraphics implementor that does not support the operation.
/// For instance, <c>SvgGraphics</c> does not support any of the <c>MeasureString</c> methods.
/// </summary>
[Serializable]
public sealed class SvgGdiNotImplementedException : NotImplementedException {
    public SvgGdiNotImplementedException(string method) => Method = method;

    public SvgGdiNotImplementedException() => Method = "?";

    public SvgGdiNotImplementedException(string message, Exception innerException) : base(message, innerException) => Method = "?";

    public SvgGdiNotImplementedException(string method, string message, Exception innerException) : base(message, innerException) => Method = method;

    public string Method { get; }
}
