/*
	Copyright © 2003 RiskCare Ltd. All rights reserved.
	Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
	Copyright © 2015 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

	Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System;
using System.IO;
using System.Windows.Forms;
using SvgNet;
using SvgNet.SvgElements;
using SvgNet.SvgTypes;

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
		private static void Main()
		{
			Application.Run(new Form1());
		}

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
			SvgSvgElement root = new SvgSvgElement("4in", "4in", "0,0 100,100");

			//adding multiple children

			root.AddChildren(
				new SvgRectElement(5, 5, 5, 5),
				new SvgEllipseElement(30, 10, 8, 12),
				new SvgTextElement("Textastic!", 3, 20)
				);

			//group and path

			SvgGroupElement grp = new SvgGroupElement("green_group");

			grp.Style = "fill:green;stroke:black;";

			SvgEllipseElement ell = new SvgEllipseElement();
			ell.CX = 50;
			ell.CY = 50;
			ell.RX = 10;
			ell.RY = 20;

			SvgPathElement pathy = new SvgPathElement();
			pathy.D = "M 20,80 C 20,90 30,80 70,100 C 70,100 40,60 50,60 z";
			pathy.Style = ell.Style;

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
			Assert.Equals((float)b[1].Matrix.OffsetX, 11f);

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
			this.button1 = new System.Windows.Forms.Button();
			this.tbOut = new System.Windows.Forms.TextBox();
			this.tbIn = new System.Windows.Forms.TextBox();
			this.button3 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.svgIn = new System.Windows.Forms.WebBrowser();
			this.svgOut = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(24, 33);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(128, 32);
			this.button1.TabIndex = 0;
			this.button1.Text = "Test an SVG file";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// tbOut
			// 
			this.tbOut.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbOut.Location = new System.Drawing.Point(184, 313);
			this.tbOut.Multiline = true;
			this.tbOut.Name = "tbOut";
			this.tbOut.Size = new System.Drawing.Size(384, 232);
			this.tbOut.TabIndex = 2;
			this.tbOut.Text = "textBox1";
			// 
			// tbIn
			// 
			this.tbIn.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbIn.Location = new System.Drawing.Point(184, 32);
			this.tbIn.Multiline = true;
			this.tbIn.Name = "tbIn";
			this.tbIn.Size = new System.Drawing.Size(384, 232);
			this.tbIn.TabIndex = 3;
			this.tbIn.Text = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(24, 81);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(128, 32);
			this.button3.TabIndex = 4;
			this.button3.Text = "Run Type Tests";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(184, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 24);
			this.label1.TabIndex = 7;
			this.label1.Text = "Input:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(184, 289);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 24);
			this.label2.TabIndex = 8;
			this.label2.Text = "Output:";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(24, 129);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(128, 32);
			this.button2.TabIndex = 9;
			this.button2.Text = "Run Composition Tests";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// svgIn
			// 
			this.svgIn.Location = new System.Drawing.Point(576, 32);
			this.svgIn.MinimumSize = new System.Drawing.Size(20, 20);
			this.svgIn.Name = "svgIn";
			this.svgIn.Size = new System.Drawing.Size(336, 232);
			this.svgIn.TabIndex = 10;
			// 
			// svgOut
			// 
			this.svgOut.Location = new System.Drawing.Point(576, 313);
			this.svgOut.MinimumSize = new System.Drawing.Size(20, 20);
			this.svgOut.Name = "svgOut";
			this.svgOut.Size = new System.Drawing.Size(336, 232);
			this.svgOut.TabIndex = 11;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(944, 581);
			this.Controls.Add(this.svgOut);
			this.Controls.Add(this.svgIn);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.tbIn);
			this.Controls.Add(this.tbOut);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "Form1";
			this.Text = "SvgNet doc reading/writing test";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

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
