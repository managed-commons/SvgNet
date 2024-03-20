/*
	Copyright © 2003 RiskCare Ltd. All rights reserved.
	Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
	Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

	Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System.IO;
using System.Xml;

namespace SvgDocTest {
    public static class StringExtensions {
        public static string LoadText(this string svgFileName) => File.ReadAllText(svgFileName);

        public static XmlDocument LoadXml(this string svgFileName) {
            var doc = new XmlDocument();
            doc.Load(svgFileName);
            return doc;
        }
    }
}
