/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2019 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System;

namespace SvgNet.SvgGdi
{
    /// <summary>
    /// Exception thrown when a GDI+ operation is attempted on an IGraphics implementor that does not support the operation.
    /// For instance, <c>SvgGraphics</c> does not support any of the <c>MeasureString</c> methods.
    /// </summary>
    [Serializable]
    public class SvgGdiNotImplementedException : Exception
    {
        public SvgGdiNotImplementedException(string method) => Method = method;

        public string Method { get; }
    }
}