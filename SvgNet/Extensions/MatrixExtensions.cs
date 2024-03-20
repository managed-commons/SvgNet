/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace System.Drawing.Drawing2D;

public static class MatrixExtensions {

    public static Matrix MultiplyAll(this List<Matrix> stack, Matrix result = null) {
        result ??= new Matrix();
        foreach (Matrix mat in stack)
            if (!mat.IsIdentity)
                result.Multiply(mat);
        return result;
    }
}