/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

using System.ComponentModel;
using System.Windows.Forms;

namespace SvgGdiTest {
    public partial class SvgGdiTestForm
    {
        public SvgGdiTestForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">if should be disposing the components</param>
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

        private ComboBox cbWhat;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components  = null;

        private Label label1;
        private Label label2;
        private Panel panel1;
        private WebBrowser svgCtl;
        private TextBox tbSVG;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbSVG = new System.Windows.Forms.TextBox();
            this.cbWhat = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.svgCtl = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(8, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(426, 300);
            this.panel1.TabIndex = 2;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelPaint);
            // 
            // tbSVG
            // 
            this.tbSVG.BackColor = System.Drawing.SystemColors.Info;
            this.tbSVG.Font = new System.Drawing.Font("Cascadia Code", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSVG.Location = new System.Drawing.Point(440, 46);
            this.tbSVG.Multiline = true;
            this.tbSVG.Name = "tbSVG";
            this.tbSVG.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbSVG.Size = new System.Drawing.Size(664, 643);
            this.tbSVG.TabIndex = 3;
            // 
            // cbWhat
            // 
            this.cbWhat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWhat.Items.AddRange(new object[] {
            "Clipping",
            "Transforms",
            "Arcs/Pies",
            "Lines",
            "Curves",
            "Transparency",
            "Images",
            "Text",
            "Rect-aligned Text",
            "Fills",
            "Path",
            "Path Polygon",
            "Path 2 (Slow)"});
            this.cbWhat.Location = new System.Drawing.Point(732, 12);
            this.cbWhat.MaxDropDownItems = 30;
            this.cbWhat.Name = "cbWhat";
            this.cbWhat.Size = new System.Drawing.Size(372, 22);
            this.cbWhat.TabIndex = 5;
            this.cbWhat.SelectedIndexChanged += new System.EventHandler(this.ComboWhat_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "GDI:";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 365);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 21);
            this.label2.TabIndex = 7;
            this.label2.Text = "SVG:";
            // 
            // svgCtl
            // 
            this.svgCtl.Location = new System.Drawing.Point(8, 389);
            this.svgCtl.MinimumSize = new System.Drawing.Size(20, 20);
            this.svgCtl.Name = "svgCtl";
            this.svgCtl.Size = new System.Drawing.Size(426, 300);
            this.svgCtl.TabIndex = 8;
            // 
            // SvgGdiTestForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 15);
            this.ClientSize = new System.Drawing.Size(1116, 697);
            this.Controls.Add(this.svgCtl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbWhat);
            this.Controls.Add(this.tbSVG);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SvgGdiTestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SvgNet.SvgGdi demonstration/test app";
            this.Load += new System.EventHandler(this.SvgGdiTestForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
