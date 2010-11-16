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
using SvgNet;
using SvgNet.SvgElements;
using System.Xml;
using System.Text;
using System.IO;
using SvgNet.SvgTypes;
using SvgNet.SvgGdi;
using System.Drawing.Drawing2D;


namespace SvgGdiTest
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox tbSVG;
		private AxSVGACTIVEXLib.AxSVGCtl svgCtl;
		private System.Windows.Forms.ComboBox cbWhat;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.tbSVG = new System.Windows.Forms.TextBox();
			this.svgCtl = new AxSVGACTIVEXLib.AxSVGCtl();
			this.cbWhat = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.svgCtl)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Location = new System.Drawing.Point(8, 56);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(400, 304);
			this.panel1.TabIndex = 2;
			this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelPaint);
			// 
			// tbSVG
			// 
			this.tbSVG.BackColor = System.Drawing.SystemColors.Info;
			this.tbSVG.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tbSVG.Location = new System.Drawing.Point(440, 48);
			this.tbSVG.Multiline = true;
			this.tbSVG.Name = "tbSVG";
			this.tbSVG.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbSVG.Size = new System.Drawing.Size(752, 640);
			this.tbSVG.TabIndex = 3;
			this.tbSVG.Text = "";
			this.tbSVG.WordWrap = false;
			// 
			// svgCtl
			// 
			this.svgCtl.Enabled = true;
			this.svgCtl.Location = new System.Drawing.Point(8, 392);
			this.svgCtl.Name = "svgCtl";
			this.svgCtl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("svgCtl.OcxState")));
			this.svgCtl.Size = new System.Drawing.Size(400, 304);
			this.svgCtl.TabIndex = 4;
			// 
			// cbWhat
			// 
			this.cbWhat.Items.AddRange(new object[] {
														"Clipping",
														"Transforms",
														"Arcs/Pies",
														"Lines",
														"Curves",
														"Transparency",
														"Images",
														"Text",
														"Fills"});
			this.cbWhat.Location = new System.Drawing.Point(184, 8);
			this.cbWhat.Name = "cbWhat";
			this.cbWhat.Size = new System.Drawing.Size(312, 21);
			this.cbWhat.TabIndex = 5;
			this.cbWhat.SelectedIndexChanged += new System.EventHandler(this.cbWhat_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 24);
			this.label1.TabIndex = 6;
			this.label1.Text = "GDI:";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(8, 368);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 24);
			this.label2.TabIndex = 7;
			this.label2.Text = "SVG:";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(1200, 717);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label2,
																		  this.label1,
																		  this.cbWhat,
																		  this.tbSVG,
																		  this.panel1,
																		  this.svgCtl});
			this.Name = "Form1";
			this.Text = "SvgNet.SvgGdi demonstration/test app";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.svgCtl)).EndInit();
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

		private void Render(IGraphics ig)
		{

			string s = cbWhat.Text;

			if (s == "Clipping")
			{
				Pen pn = new Pen(Color.LightGray, 5);
				Pen pn2 = new Pen(Color.Yellow);

				ig.Clear(Color.Black);

				GraphicsContainer cnt = ig.BeginContainer();

				ig.SmoothingMode = SmoothingMode.HighQuality;

				ig.SetClip(new Rectangle(35,35,120,120));

				ig.DrawRectangle(pn, 5,5,45,70);
				ig.DrawRectangle(pn, 15,25,90,120);
				ig.DrawRectangle(pn, 50,30,100,170);
				ig.DrawRectangle(pn, 5,80,180,30);
				ig.DrawRectangle(pn, 75,10,40,160);

				ig.EndContainer(cnt);

				ig.DrawRectangle(pn2, 5,5,45,70);
				ig.DrawRectangle(pn2, 15,25,90,120);
				ig.DrawRectangle(pn2, 50,30,100,170);
				ig.DrawRectangle(pn2, 5,80,180,30);
				ig.DrawRectangle(pn2, 75,10,40,160);
			}
			else if (s == "Transforms")
			{
			
				ig.Clear(Color.Black);

				ig.RotateTransform(15);
				ig.DrawRectangle(new Pen(Color.Red,2), 260,80,50,40);
				ig.ResetTransform();
				ig.DrawRectangle(new Pen(Color.Red,2), 260,80,50,40);

				ig.TranslateTransform(15,-5);

				GraphicsContainer cnt = ig.BeginContainer();

					ig.SmoothingMode = SmoothingMode.HighQuality;

					ig.RotateTransform(5);
					ig.FillEllipse(new SolidBrush(Color.Orange), 100,100,80,40);
					ig.DrawRectangle(new Pen(Color.Orange,2), 60,80,40,40);
					

					GraphicsContainer cnt2  = ig.BeginContainer();

						ig.SmoothingMode = SmoothingMode.None;

						ig.RotateTransform(5);
						ig.ScaleTransform(1.1f, 1.2f);

						ig.FillEllipse(new SolidBrush(Color.YellowGreen), 130,180,80,40);
						ig.DrawRectangle(new Pen(Color.YellowGreen,2), 62,80,40,40);

						GraphicsContainer cnt3  = ig.BeginContainer();

							ig.SmoothingMode = SmoothingMode.HighQuality;

							Matrix mm = new Matrix();
							mm.Shear(0.3f, 0f);
							ig.Transform = mm;
							
							ig.FillEllipse(new SolidBrush(Color.Green), 180,120,80,40);
							ig.DrawRectangle(new Pen(Color.Green,2), 62,84,40,40);

						ig.EndContainer(cnt3);

					ig.EndContainer(cnt2);

					ig.FillEllipse(new SolidBrush(Color.Blue), 120,150,80,40);
					ig.DrawRectangle(new Pen(Color.Blue,2), 64,80,40,40);

				ig.EndContainer(cnt);

				ig.FillEllipse(new SolidBrush(Color.Indigo), 80,210,80,40);
				ig.DrawRectangle(new Pen(Color.Indigo,2), 66,80,40,40);

				ig.DrawRectangle(new Pen(Color.White,2), 270,30,50,40);
				ig.ResetTransform();
				ig.DrawRectangle(new Pen(Color.White,2), 270,30,50,40);


			}
			else if (s == "Lines")
			{
				ig.SmoothingMode = SmoothingMode.AntiAlias;

				Pen ow = new Pen(Color.Purple, 12);
				ow.EndCap = LineCap.Round;
				ow.StartCap = LineCap.Round;
				ow.MiterLimit = 6f;
				ow.LineJoin = LineJoin.Miter;

				ig.SmoothingMode = SmoothingMode.None;

				Pen tp = new Pen(Color.Red, 2);
				tp.DashStyle = DashStyle.DashDot;
			
				ig.DrawLine(tp, 70,20,190,20);

				tp.DashStyle = DashStyle.Dash;

				ig.DrawLine(tp, 70,30,190,30);

				tp.DashStyle = DashStyle.Custom;
				tp.DashPattern = new float[] {1,8,2,2};

				ig.DrawLine(tp, 70,40,190,40);

				ig.SmoothingMode = SmoothingMode.AntiAlias;
 
				PointF[] pts = new PointF[4];
				pts[0] = new PointF(20,50);
				pts[1] = new PointF(30,90);
				pts[2] = new PointF(65,60);
				pts[3] = new PointF(50,40);
				ig.DrawLines(ow, pts);

				Point[] polly = new Point[]
				{
				new Point(200, 40),
				new Point(220, 140),
				new Point(240, 100),
				new Point(290, 70),
				new Point(230, 10)
				};

				ig.DrawPolygon(tp, polly);

				//arrows
				Pen arr = new Pen(Color.DarkGoldenrod, 5);
				AdjustableArrowCap aac = new AdjustableArrowCap(5,3, false);
				arr.EndCap = LineCap.Custom;
				arr.CustomEndCap = aac;
				arr.StartCap = LineCap.ArrowAnchor;
				ig.DrawLine(arr, 50,120, 150,200);

				arr.Width = 7f;
				arr.EndCap = LineCap.RoundAnchor;
				arr.StartCap = LineCap.SquareAnchor;
				ig.DrawLine(arr, 100,120, 200,200);

				arr.Width = 9;
				arr.EndCap = LineCap.DiamondAnchor;
				arr.StartCap = LineCap.ArrowAnchor;
				ig.DrawLine(arr, 150,120, 250,200);

				Point[] al = new Point[]
				{
					new Point(200, 100),
					new Point(300, 200),
					new Point(300, 150)
				};

				arr.EndCap = LineCap.DiamondAnchor;
				arr.StartCap = LineCap.DiamondAnchor;
				ig.DrawLines(arr, al);

			}
			else if (s == "Curves")
			{

				

				PointF[] bezzie = new PointF[]
			{
				new PointF(20, 150),

				new PointF(110, 190),
				new PointF(120, 200),
				new PointF(50, 220),

				new PointF(60, 200),
				new PointF(140, 180),
				new PointF(100, 160),

				new PointF(180, 260),
				new PointF(200, 210),
				new PointF(190, 210)
			};

				Pen bpn = new Pen(Color.MediumSeaGreen, 2);
				bpn.DashStyle = DashStyle.Custom;
				bpn.DashPattern = new float[]{6,1,5,2,4,3,3,4,2,5,6,1};
				ig.DrawBeziers(bpn, bezzie);

			

				PointF[] curvy = new PointF[]
			{
				new PointF(130, 40),
				new PointF(70, 70),
				new PointF(50, 20),
				new PointF(120, 120),
				new PointF(150, 80),
				new PointF(80, 150),
				new PointF(80, 110)
			};

				ig.DrawCurve(new Pen(Color.Blue, 5), curvy);
				ig.DrawCurve(new Pen(Color.Red, 2), curvy, 2, 3);
				ig.DrawCurve(new Pen(Color.Yellow, 1), curvy, 1f);

				Point[] ccurvy = new Point[]
			{
				new Point(280, 30),
				new Point(260, 60),
				new Point(200, 20),
				new Point(290, 120),
				new Point(290, 80),
				new Point(230, 150),
				new Point(150, 50)
			};
				ig.DrawClosedCurve(new Pen(Color.Green, 3), ccurvy, 1f, FillMode.Alternate);
				ig.DrawClosedCurve(new Pen(Color.Purple, 1), ccurvy, 0f, FillMode.Alternate);

				

				Point[] fcc = new Point[]
			{
				new Point(160, 350),
				new Point(190, 370),
				new Point(130, 390),
				new Point(190, 400),
				new Point(195, 410),
				new Point(100, 430),
				new Point(160, 450)
			};
				ig.FillClosedCurve(new SolidBrush(Color.Red), fcc, FillMode.Winding, 1f);
				ig.FillClosedCurve(new SolidBrush(Color.Aquamarine), fcc, FillMode.Alternate, .2f);



			}
			else if (s == "Transparency")
			{
				Point[] fillpoly = new Point[]
				{
					new Point(20, 130),
					new Point(60, 90),
					new Point(30, 20),
					new Point(80, 20),
					new Point(15, 90),
					new Point(100, 50),
					new Point(0, 50)
				};

				Color col = Color.FromArgb(96,255,0,0);

				ig.FillEllipse(new SolidBrush(Color.Ivory), 60, 140, 60, 30);
				ig.FillPolygon(new SolidBrush(Color.Ivory), fillpoly, FillMode.Winding);

				ig.TranslateTransform(10,10);
				ig.FillEllipse(new SolidBrush(col), 60, 140, 60, 30);
				ig.FillPolygon(new SolidBrush(col), fillpoly, FillMode.Alternate);
				ig.ResetTransform();

				ig.FillPie(new SolidBrush(Color.FromArgb(100,255,0,0)), 10, 200, 200, 80, 315, 90);
				ig.FillPie(new SolidBrush(Color.FromArgb(100,128,128,0)), 10, 200, 200, 80, 250, -90);
				ig.FillPie(new SolidBrush(Color.FromArgb(100,128,0,128)), 15, 205, 190, 70, 180, 270);
				ig.FillPie(new SolidBrush(Color.FromArgb(100,200,60,60)), 20, 210, 180, 60, 45, -270);

			}
			else if (s == "Fills")
			{
				
				LinearGradientBrush gbr1 = new LinearGradientBrush(new Point(0,0), new Point(30,20), Color.Blue, Color.Plum);
			
				ColorBlend blend = new ColorBlend(3);
				blend.Colors =  new Color[] {Color.Red, Color.Yellow, Color.MediumSlateBlue};
				blend.Positions = new float[] {0, .3f, 1f};
				gbr1.InterpolationColors = blend;
			
			
				Point[] sp = new Point[]
				{
					new Point(145, 145),
					new Point(305, 250),
					new Point(220, 250),
					new Point(180, 250)
				};
				ig.FillPolygon(gbr1, sp);

				LinearGradientBrush gbr2 = new LinearGradientBrush(new Point(0,0), new Point(10,20), Color.WhiteSmoke, Color.CornflowerBlue);
				gbr2.WrapMode = WrapMode.TileFlipXY;
				Point[] sp2 = new Point[]
				{
					new Point(25, 205),
					new Point(75, 150),
					new Point(110, 110),
					new Point(40, 80)
				};
				ig.FillPolygon(gbr2, sp2);

				ig.FillRectangle(new HatchBrush(HatchStyle.DiagonalBrick, Color.Khaki, Color.Peru), 000,5,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.Vertical, Color.Bisque, Color.Peru), 020,5,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.DarkVertical, Color.Tan, Color.Peru), 040,5,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.DiagonalCross, Color.Chocolate, Color.Peru), 060,5,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.WideDownwardDiagonal, Color.BurlyWood, Color.Peru), 080,5,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.LargeConfetti, Color.Wheat, Color.Peru), 100,5,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.ZigZag, Color.SaddleBrown, Color.Peru), 120,5,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.HorizontalBrick, Color.Linen, Color.Peru), 140,5,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.LightHorizontal, Color.Maroon, Color.Peru), 160,5,20,20);

				ig.FillRectangle(new HatchBrush(HatchStyle.Percent05, Color.CornflowerBlue, Color.LemonChiffon), 000,30,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.Percent10, Color.CornflowerBlue, Color.LemonChiffon), 020,30,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.Percent20, Color.CornflowerBlue, Color.LemonChiffon), 040,30,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.Percent25, Color.CornflowerBlue, Color.LemonChiffon), 060,30,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.Percent30, Color.CornflowerBlue, Color.LemonChiffon), 080,30,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.Percent40, Color.CornflowerBlue, Color.LemonChiffon), 100,30,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.Percent50, Color.CornflowerBlue, Color.LemonChiffon), 120,30,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.Percent60, Color.CornflowerBlue, Color.LemonChiffon), 140,30,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.Percent70, Color.CornflowerBlue, Color.LemonChiffon), 160,30,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.Percent75, Color.CornflowerBlue, Color.LemonChiffon), 180,30,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.Percent80, Color.CornflowerBlue, Color.LemonChiffon), 200,30,20,20);
				ig.FillRectangle(new HatchBrush(HatchStyle.Percent90, Color.CornflowerBlue, Color.LemonChiffon), 220,30,20,20);

			}
			else if (s == "Arcs/Pies")
			{
				//GDI does not seem to draw arcs correctly except when the ellipse is a circle.
				//These arcs demonstrate the problem.  SVGGraphics calculates arcs correctly.
				ig.DrawArc(new Pen(Color.Black, 2), 120+5*3, 120, 110*3, 110, 0, 240);
				ig.DrawArc(new Pen(Color.Black, 2), 120+10*3, 125, 100*3, 100, 0, 210);
				ig.DrawArc(new Pen(Color.Black, 2), 120+15*3, 130, 90*3, 90, 0, 180);
				ig.DrawArc(new Pen(Color.Black, 2), 120+20*3, 135, 80*3, 80, 0, 150);
				ig.DrawArc(new Pen(Color.Black, 2), 120+25*3, 140, 70*3, 70, 0, 120);
				ig.DrawArc(new Pen(Color.Black, 2), 120+30*3, 145, 60*3, 60, 0, 90);
				ig.DrawArc(new Pen(Color.Black, 2), 120+35*3, 150, 50*3, 50, 0, 60);
				ig.DrawArc(new Pen(Color.Black, 2), 120+40*3, 155, 40*3, 40, 0, 270);

				ig.DrawPie(new Pen(Color.Pink, 2), 110, 50, 100, 100, 315, 90);
				ig.DrawPie(new Pen(Color.Purple, 2), 110, 50, 100, 100, 250, -90);
				ig.DrawPie(new Pen(Color.DarkRed, 2), 115, 55, 90, 90, 180, 270);
				ig.DrawPie(new Pen(Color.Red, 2), 120, 60, 80, 80, 45, -270);

				
			}
			else if (s == "Text")
			{
				
				Font fnt1 = new Font("Helvetica", 12, FontStyle.Italic | FontStyle.Bold);
				Font fnt2 = new Font(FontFamily.GenericMonospace, 16, FontStyle.Bold);
				Font fnt3 = new Font("", 40, FontStyle.Underline);

				Rectangle rc1 = new Rectangle(30, 30, 220,20);
				StringFormat fmt1= new StringFormat();
				fmt1.Alignment = StringAlignment.Near;

				ig.DrawRectangle(new Pen(Color.Blue),  rc1);
				ig.DrawString("Text...1", fnt1, new SolidBrush(Color.DarkGreen), rc1, fmt1);

				Rectangle rc2 = new Rectangle(0,0, 120,20);
				StringFormat fmt2= new StringFormat();
				fmt2.Alignment = StringAlignment.Center;

				ig.TranslateTransform(30,160);
				ig.RotateTransform(90);

				ig.DrawRectangle(new Pen(Color.Blue),  rc2);
				ig.DrawString("Text...2", fnt2, new SolidBrush(Color.DarkGreen), rc2, fmt2);

				ig.ResetTransform();

				Rectangle rc3 = new Rectangle(30, 90, 300,30);
				StringFormat fmt3= new StringFormat();
				fmt3.Alignment = StringAlignment.Far;

				ig.DrawRectangle(new Pen(Color.Blue),  rc3);
				ig.DrawString("Text...3", fnt3, new SolidBrush(Color.DarkGreen), rc3, fmt3);

				//measurestring
				string mme = "MeasureString Is Impossible To Emulate";
				SizeF siz = ig.MeasureString(mme, fnt1);
				ig.DrawRectangle(new Pen(Color.Red), 20, 200, siz.Width, siz.Height);
				siz = ig.MeasureString(mme, fnt1, 150);
				ig.DrawRectangle(new Pen(Color.Orange), 20, 230, siz.Width, siz.Height);
				siz = ig.MeasureString(mme, fnt1,  new SizeF(150, 150), new StringFormat(StringFormatFlags.DirectionVertical));
				ig.DrawRectangle(new Pen(Color.Yellow), 20, 200, siz.Width, siz.Height);

			}
			else if (s == "Images")
			{

				Icon ike = new Icon(GetType(), "App.ico");
				ig.DrawIcon(ike, 10,10);
				//ig.DrawIcon(ike, new Rectangle(270, 400, 30, 40));

				Bitmap bmp = new Bitmap(GetType(), "test.bmp");
				ig.DrawImage(bmp, 100f, 150f);
				GraphicsContainer cnt = ig.BeginContainer();
				ig.RotateTransform(5);
				ig.DrawImage(bmp, 160f, 50f, 120f, 70f);
				ig.EndContainer(cnt);
				//ig.DrawImageUnscaled(bmp, 270, 450, 20, 20);

			}
			else
			{
				
			}
			
		}


		private void PanelPaint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			GdiGraphics gg = new GdiGraphics(g);

			Render(gg);

			g.Flush();
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
		
		}

		private void cbWhat_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			SvgGraphics ig;

			ig = new SvgGraphics();
			Render(ig);

			string s = ig.WriteSVGString();

			tbSVG.Text = s;
 
			StreamWriter tw = new StreamWriter("c:\\temp\\foo.svg", false);

			tw.Write(s);

			tw.Close();

			svgCtl.SRC = "c:\\temp\\foo.svg";

			this.panel1.Invalidate();
		
		}
	}
}
