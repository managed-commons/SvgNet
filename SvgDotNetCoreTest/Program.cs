using System;
using System.Drawing;
using SvgNet;
using SvgNet.SvgGdi;

namespace SvgDotNetCoreTest {
    public static class Program {
        public static void Main(string[] args) {
            foreach (System.Collections.Generic.KeyValuePair<string, Action<IGraphics>> pair in TestShared.Renderers) {
                var ig = new SvgGraphics(Color.WhiteSmoke);
                Console.WriteLine($"=== Renderer {pair.Key}");
                pair.Value(ig);
                Console.WriteLine(ig.WriteSVGString());
            }
        }
    }
}
