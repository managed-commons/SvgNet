using SvgNet;
using SvgNet.SvgGdi;
using System;
using System.Drawing;

namespace SvgDotNetCoreTest
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            foreach (var pair in TestShared.Renderers)
            {
                var ig = new SvgGraphics(Color.WhiteSmoke);
                Console.WriteLine($"=== Renderer {pair.Key}");
                pair.Value(ig);
                Console.WriteLine(ig.WriteSVGString());
            }
        }
    }
}
