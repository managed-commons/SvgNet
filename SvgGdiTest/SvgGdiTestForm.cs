/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using SvgNet;
using SvgNet.Interfaces;

namespace SvgGdiTest;
/// <summary>
/// Summary description for Form1.
/// </summary>
public partial class SvgGdiTestForm : Form {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main() {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("nb-NO"); // To catch culture formatting errors
        Application.Run(new SvgGdiTestForm());
    }

    private void ComboWhat_SelectedIndexChanged(object sender, EventArgs e) {
        using var ig = new SvgGraphics(Color.WhiteSmoke);
        Render(ig);
        string s = ig.WriteSVGString();
        tbSVG.Text = s;
        string tempFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "foo.svg");
        var tw = new StreamWriter(tempFile, false);
        tw.Write(s);
        tw.Close();

        svgCtl.Navigate(new Uri(tempFile));
        svgCtl.Refresh(WebBrowserRefreshOption.Completely);

        panel1.Invalidate();
    }

    private void PanelPaint(object sender, PaintEventArgs e) {
        Graphics g = e.Graphics;
        Render(new GdiGraphics(g));
        g.Flush();
    }

    private void Render(IGraphics ig) {
        string s = cbWhat.Text;
        if (string.IsNullOrEmpty(s))
            return;
        if (TestShared.Renderers.ContainsKey(s))
            TestShared.Renderers[s](ig);
        else
            throw new NotImplementedException();
    }

    private void RenderImages(IGraphics ig) {
        var ike = new Icon(GetType(), "App.ico");
        ig.DrawIcon(ike, 10, 10);
        ig.DrawIcon(ike, new Rectangle(50, 10, ike.Width * 2, ike.Height * 3));

        var bmp = new Bitmap(GetType(), "test.bmp");
        ig.DrawImage(bmp, 100f, 150f);
        GraphicsContainer cnt = ig.BeginContainer();
        ig.RotateTransform(7.5f);
        ig.DrawImage(bmp, 160f, 50f, 120f, 70f);
        ig.EndContainer(cnt);
        //ig.DrawImageUnscaled(bmp, 270, 450, 20, 20);
    }

    private void SvgGdiTestForm_Load(object sender, EventArgs e) {
        TestShared.Renderers.Add("Images", RenderImages);
        TestShared.Renderers.Add("Text Rect Aligned", ig => RectAlignedTextTest.RenderRectAlignedText(ig, panel1.ClientSize.Width, panel1.ClientSize.Height, Font));
        cbWhat.Items.Clear();
        cbWhat.Items.AddRange([.. TestShared.Renderers.Keys.OrderBy(s => s)]);
    }
}
