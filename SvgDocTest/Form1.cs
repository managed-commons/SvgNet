/*
	Copyright c 2003 by RiskCare Ltd.  All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions
	are met:
	1. Redistributions of source code must retain the above copyright
	notice, this list of conditions and the following disclaimer.
	2. Redistributions in binary form must reproduce the above copyright
	notice, this list of conditions and the following disclaimer in the
	documentation and/or other materials provided with the distribution.

	THIS SOFTWARE IS PROVIDED BY THE AUTHOR AND CONTRIBUTORS ``AS IS'' AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
	IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
	ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE
	FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
	DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
	OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
	HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
	LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
	OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
	SUCH DAMAGE.
*/


using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using SvgNet;
using SvgNet.SvgElements;
using SvgNet.SvgTypes;
using System.IO;

namespace SvgDocTest
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox tbOut;
		private System.Windows.Forms.TextBox tbIn;
		private System.Windows.Forms.Button button3;
		private AxSVGACTIVEXLib.AxSVGCtl svgOut;
		private AxSVGACTIVEXLib.AxSVGCtl svgIn;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

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
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.button1 = new System.Windows.Forms.Button();
			this.tbOut = new System.Windows.Forms.TextBox();
			this.tbIn = new System.Windows.Forms.TextBox();
			this.button3 = new System.Windows.Forms.Button();
			this.svgOut = new AxSVGACTIVEXLib.AxSVGCtl();
			this.svgIn = new AxSVGACTIVEXLib.AxSVGCtl();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.svgOut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.svgIn)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(8, 16);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(128, 32);
			this.button1.TabIndex = 0;
			this.button1.Text = "Test an SVG file";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// tbOut
			// 
			this.tbOut.Location = new System.Drawing.Point(184, 296);
			this.tbOut.Multiline = true;
			this.tbOut.Name = "tbOut";
			this.tbOut.Size = new System.Drawing.Size(384, 248);
			this.tbOut.TabIndex = 2;
			this.tbOut.Text = "textBox1";
			// 
			// tbIn
			// 
			this.tbIn.Location = new System.Drawing.Point(184, 32);
			this.tbIn.Multiline = true;
			this.tbIn.Name = "tbIn";
			this.tbIn.Size = new System.Drawing.Size(384, 232);
			this.tbIn.TabIndex = 3;
			this.tbIn.Text = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(8, 64);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(128, 32);
			this.button3.TabIndex = 4;
			this.button3.Text = "Run Type Tests";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// svgOut
			// 
			this.svgOut.Enabled = true;
			this.svgOut.Location = new System.Drawing.Point(576, 296);
			this.svgOut.Name = "svgOut";
			this.svgOut.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("svgOut.OcxState")));
			this.svgOut.Size = new System.Drawing.Size(336, 248);
			this.svgOut.TabIndex = 5;
			// 
			// svgIn
			// 
			this.svgIn.Enabled = true;
			this.svgIn.Location = new System.Drawing.Point(576, 32);
			this.svgIn.Name = "svgIn";
			this.svgIn.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("svgIn.OcxState")));
			this.svgIn.Size = new System.Drawing.Size(336, 232);
			this.svgIn.TabIndex = 6;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(184, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 24);
			this.label1.TabIndex = 7;
			this.label1.Text = "Input:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(184, 272);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 24);
			this.label2.TabIndex = 8;
			this.label2.Text = "Output:";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(8, 112);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(128, 32);
			this.button2.TabIndex = 9;
			this.button2.Text = "Run Composition Tests";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(944, 581);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.button2,
																		  this.label2,
																		  this.label1,
																		  this.svgIn,
																		  this.svgOut,
																		  this.button3,
																		  this.tbIn,
																		  this.tbOut,
																		  this.button1});
			this.Name = "Form1";
			this.Text = "SvgNet doc reading/writing test";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.svgOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.svgIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		SvgElement _e;

		private void button1_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				string fname = dlg.FileName;
				StreamReader str = File.OpenText(dlg.FileName);
				tbIn.Text = str.ReadToEnd();

				XmlDocument doc = new XmlDocument();
				
				doc.Load(fname);

				svgIn.SRC = fname;

				_e = SvgFactory.LoadFromXML(doc, null);

				string output = _e.WriteSVGString(true);

				tbOut.Text = output;
 
				StreamWriter tw = new StreamWriter("c:\\temp\\foo.svg", false);

				tw.Write(output);

				tw.Close();

				svgOut.SRC = "c:\\temp\\foo.svg";
			}
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			
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

		private void button2_Click(object sender, System.EventArgs e)
		{
			SvgSvgElement root = new SvgSvgElement("4in", "4in", "0,0 100,100");


			//adding multiple children

			root.AddChildren(
				new SvgRectElement(5,5,5,5),
				new SvgEllipseElement(30,10,8,12),
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

			StreamWriter tw = new StreamWriter("c:\\temp\\foo.svg", false);

			tw.Write(s);

			tw.Close();

			svgOut.SRC = "c:\\temp\\foo.svg";
		}
	}


	public class Assert
	{
		public static void Equals(float a, float b)
		{
			if (a != b)
			{
				throw new Exception("Assert.Equals");
			}
		}

		public static void Equals(bool a, bool b)
		{
			if (a != b)
			{
				throw new Exception("Assert.Equals");
			}
		}
	}
}
