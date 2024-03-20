/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System;

namespace SvgDocTest {
    public static class Assert {
        public static void Equals(float a, float b) {
            if (a != b) {
                throw new Exception("Assert.Equals");
            }
        }

        public static void Equals(bool a, bool b) {
            if (a != b) {
                throw new Exception("Assert.Equals");
            }
        }
    }
}
