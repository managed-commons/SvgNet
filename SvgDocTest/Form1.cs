/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2019 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using SvgNet;
using SvgNet.SvgElements;
using SvgNet.SvgTypes;
using System;
using System.IO;
using System.Windows.Forms;

namespace SvgDocTest
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class Form1 : Form
    {
        public Form1()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private static readonly string _tempFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "foo.svg");
        private Button button1;
        private Button button2;
        private Button button3;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private Label label1;
        private Label label2;
        private WebBrowser svgIn;
        private WebBrowser svgOut;
        private TextBox tbIn;
        private TextBox tbOut;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() => Application.Run(new Form1());

        private static void RefreshBrowserFrom(WebBrowser browser, string filename)
        {
            browser.Navigate(new Uri(filename));
            browser.Refresh(WebBrowserRefreshOption.Completely);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                AutoUpgradeEnabled = true,
                CheckFileExists = true,
                DefaultExt = ".svg",
                Filter = "Scalable Vector Graphics|*.svg",
                Multiselect = false,
                Title = "Choose one Scalable Vector Graphics file"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ProcessSvgFile(dlg.FileName);
            }
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            SvgSvgElement root = new SvgSvgElement("4in", "4in", "-10,-10 250,250");

            //adding multiple children


            root.AddChildren(
                new SvgRectElement(5, 5, 5, 5),
                new SvgEllipseElement(20, 20, 8, 12)
                {
                    Style = "fill:yellow;stroke:red"
                },

                new SvgAElement("https://github.com/managed-commons/SvgNet").AddChildren(
                    new SvgTextElement("Textastic!", 30, 20)
                    {
                        Style = "fill:midnightblue;stroke:navy;stroke-width:1px;font-size:30px;font-family:Calibri"
                    })
                );

            //group and path

            SvgGroupElement grp = new SvgGroupElement("green_group")
            {
                Style = "fill:green;stroke:black;"
            };

            grp.AddChild(new SvgRectElement(30, 30, 5, 20));

            SvgEllipseElement ell = new SvgEllipseElement
            {
                CX = 50,
                CY = 50,
                RX = 10,
                RY = 20
            };

            SvgPathElement pathy = new SvgPathElement
            {
                D = "M 20,80 C 20,90 30,80 70,100 C 70,100 40,60 50,60 z",
                Style = ell.Style
            };

            root.AddChild(grp);

            //cloning and style arithmetic

            grp.AddChildren(ell, pathy);

            grp.Style.Set("fill", "blue");

            SvgGroupElement grp2 = (SvgGroupElement)SvgFactory.CloneElement(grp);

            grp2.Id = "cloned_red_group";

            grp2.Style.Set("fill", "red");

            grp2.Style += "opacity:0.5";

            grp2.Transform = "scale (1.2, 1.2)  translate(10)";

            root.AddChild(grp2);

            //output

            string s = root.WriteSVGString(true);

            tbOut.Text = s;

            string tempFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "foo.svg");

            StreamWriter tw = new StreamWriter(tempFile, false);

            tw.Write(s);

            tw.Close();

            svgOut.Navigate(new Uri(tempFile));
            svgOut.Refresh(WebBrowserRefreshOption.Completely);
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            SvgNumList a = "3, 5.6 901		-7  ";
            Assert.Equals(a[3], -7f);

            SvgTransformList b = "rotate ( 45 ), translate (11, 10)skewX(3)";
            Assert.Equals(b[1].Matrix.OffsetX, 11f);

            SvgColor c = "rgb( 100%, 100%, 50%)";
            Assert.Equals(c.Color.B, 0x7f);

            SvgColor d = "#abc";
            Assert.Equals(d.Color.G, 0xbb);

            SvgPath f = "M 5,5 L 1.1 -6    , Q 1,3 9,10  z";
            Assert.Equals(f.Count, 4f);
            Assert.Equals(f[1].Abs, true);
            Assert.Equals(f[2].Data[3], 10f);

            MessageBox.Show("Tests completed Ok");
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new System.Windows.Forms.Button();
            tbOut = new System.Windows.Forms.TextBox();
            tbIn = new System.Windows.Forms.TextBox();
            button3 = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            button2 = new System.Windows.Forms.Button();
            svgIn = new System.Windows.Forms.WebBrowser();
            svgOut = new System.Windows.Forms.WebBrowser();
            SuspendLayout();
            //
            // button1
            //
            button1.Location = new System.Drawing.Point(24, 33);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(128, 32);
            button1.TabIndex = 0;
            button1.Text = "Test an SVG file";
            button1.Click += new System.EventHandler(button1_Click);
            //
            // tbOut
            //
            tbOut.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            tbOut.Location = new System.Drawing.Point(184, 313);
            tbOut.Multiline = true;
            tbOut.Name = "tbOut";
            tbOut.Size = new System.Drawing.Size(384, 232);
            tbOut.TabIndex = 2;
            tbOut.Text = "textBox1";
            //
            // tbIn
            //
            tbIn.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            tbIn.Location = new System.Drawing.Point(184, 32);
            tbIn.Multiline = true;
            tbIn.Name = "tbIn";
            tbIn.Size = new System.Drawing.Size(384, 232);
            tbIn.TabIndex = 3;
            tbIn.Text = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>";
            //
            // button3
            //
            button3.Location = new System.Drawing.Point(24, 81);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(128, 32);
            button3.TabIndex = 4;
            button3.Text = "Run Type Tests";
            button3.Click += new System.EventHandler(button3_Click);
            //
            // label1
            //
            label1.Location = new System.Drawing.Point(184, 7);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(136, 24);
            label1.TabIndex = 7;
            label1.Text = "Input:";
            //
            // label2
            //
            label2.Location = new System.Drawing.Point(184, 289);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(136, 24);
            label2.TabIndex = 8;
            label2.Text = "Output:";
            //
            // button2
            //
            button2.Location = new System.Drawing.Point(24, 129);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(128, 32);
            button2.TabIndex = 9;
            button2.Text = "Run Composition Tests";
            button2.Click += new System.EventHandler(button2_Click);
            //
            // svgIn
            //
            svgIn.Location = new System.Drawing.Point(576, 32);
            svgIn.MinimumSize = new System.Drawing.Size(20, 20);
            svgIn.Name = "svgIn";
            svgIn.Size = new System.Drawing.Size(336, 232);
            svgIn.TabIndex = 10;
            //
            // svgOut
            //
            svgOut.Location = new System.Drawing.Point(576, 313);
            svgOut.MinimumSize = new System.Drawing.Size(20, 20);
            svgOut.Name = "svgOut";
            svgOut.Size = new System.Drawing.Size(336, 232);
            svgOut.TabIndex = 11;
            //
            // Form1
            //
            AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            ClientSize = new System.Drawing.Size(944, 581);
            Controls.Add(svgOut);
            Controls.Add(svgIn);
            Controls.Add(button2);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button3);
            Controls.Add(tbIn);
            Controls.Add(tbOut);
            Controls.Add(button1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Name = "Form1";
            Text = "SvgNet doc reading/writing test";
            Load += new System.EventHandler(Form1_Load);
            ResumeLayout(false);
            PerformLayout();
        }

        private void ProcessSvgFile(string svgFileName)
        {
            tbIn.Text = svgFileName.LoadText();
            RefreshBrowserFrom(svgIn, svgFileName);
            tbOut.Text = SvgFactory.LoadFromXML(svgFileName.LoadXml(), null).WriteSVGString(true);
            File.WriteAllText(_tempFileName, tbOut.Text);
            RefreshBrowserFrom(svgOut, _tempFileName);
        }
    }
}
