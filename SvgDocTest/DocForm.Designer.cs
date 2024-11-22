namespace SvgDocTest {
    partial class DocForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.svgIn = new System.Windows.Forms.WebBrowser();
            this.tbIn = new System.Windows.Forms.TextBox();
            this.panelBottom = new System.Windows.Forms.GroupBox();
            this.tbOut = new System.Windows.Forms.TextBox();
            this.svgOut = new System.Windows.Forms.WebBrowser();
            this.panelTop = new System.Windows.Forms.GroupBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panelBottom.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // svgIn
            // 
            this.svgIn.AllowWebBrowserDrop = false;
            this.svgIn.Location = new System.Drawing.Point(760, 28);
            this.svgIn.Margin = new System.Windows.Forms.Padding(4);
            this.svgIn.MinimumSize = new System.Drawing.Size(27, 25);
            this.svgIn.Name = "svgIn";
            this.svgIn.Size = new System.Drawing.Size(857, 420);
            this.svgIn.TabIndex = 19;
            // 
            // tbIn
            // 
            this.tbIn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbIn.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbIn.Font = new System.Drawing.Font("Cascadia Code", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbIn.Location = new System.Drawing.Point(3, 28);
            this.tbIn.Margin = new System.Windows.Forms.Padding(4);
            this.tbIn.Multiline = true;
            this.tbIn.Name = "tbIn";
            this.tbIn.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbIn.Size = new System.Drawing.Size(757, 415);
            this.tbIn.TabIndex = 14;
            this.tbIn.Text = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>";
            // 
            // panelBottom
            // 
            this.panelBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelBottom.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelBottom.Controls.Add(this.tbOut);
            this.panelBottom.Controls.Add(this.svgOut);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelBottom.Location = new System.Drawing.Point(0, 526);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1620, 451);
            this.panelBottom.TabIndex = 21;
            this.panelBottom.TabStop = false;
            this.panelBottom.Text = "Output";
            // 
            // tbOut
            // 
            this.tbOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbOut.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbOut.Font = new System.Drawing.Font("Cascadia Code", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbOut.Location = new System.Drawing.Point(3, 28);
            this.tbOut.Margin = new System.Windows.Forms.Padding(4);
            this.tbOut.Multiline = true;
            this.tbOut.Name = "tbOut";
            this.tbOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbOut.Size = new System.Drawing.Size(757, 420);
            this.tbOut.TabIndex = 23;
            this.tbOut.Text = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>";
            // 
            // svgOut
            // 
            this.svgOut.AllowWebBrowserDrop = false;
            this.svgOut.Location = new System.Drawing.Point(760, 28);
            this.svgOut.Margin = new System.Windows.Forms.Padding(4);
            this.svgOut.MinimumSize = new System.Drawing.Size(27, 25);
            this.svgOut.Name = "svgOut";
            this.svgOut.Size = new System.Drawing.Size(857, 420);
            this.svgOut.TabIndex = 24;
            // 
            // panelTop
            // 
            this.panelTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelTop.BackColor = System.Drawing.Color.LightCoral;
            this.panelTop.Controls.Add(this.svgIn);
            this.panelTop.Controls.Add(this.tbIn);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTop.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.panelTop.Location = new System.Drawing.Point(0, 80);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1620, 446);
            this.panelTop.TabIndex = 22;
            this.panelTop.TabStop = false;
            this.panelTop.Text = "Input";
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.Khaki;
            this.panelButtons.Controls.Add(this.button2);
            this.panelButtons.Controls.Add(this.button3);
            this.panelButtons.Controls.Add(this.button1);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1620, 74);
            this.panelButtons.TabIndex = 23;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.Navy;
            this.button2.FlatAppearance.BorderSize = 2;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Cascadia Code", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.DarkMagenta;
            this.button2.Location = new System.Drawing.Point(603, 13);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(323, 39);
            this.button2.TabIndex = 21;
            this.button2.Text = "Run Composition Tests";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.White;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.Navy;
            this.button3.FlatAppearance.BorderSize = 2;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Cascadia Code", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.DarkMagenta;
            this.button3.Location = new System.Drawing.Point(1069, 13);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(323, 39);
            this.button3.TabIndex = 20;
            this.button3.Text = "Run Type Tests";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Navy;
            this.button1.FlatAppearance.BorderSize = 2;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Cascadia Code", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.DarkMagenta;
            this.button1.Location = new System.Drawing.Point(137, 13);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(323, 39);
            this.button1.TabIndex = 19;
            this.button1.Text = "Test an SVG file";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // DocForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1620, 977);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DocForm";
            this.Text = "DocForm";
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.WebBrowser svgIn;
        private System.Windows.Forms.TextBox tbIn;
        private System.Windows.Forms.GroupBox panelBottom;
        private System.Windows.Forms.GroupBox panelTop;
        private System.Windows.Forms.TextBox tbOut;
        private System.Windows.Forms.WebBrowser svgOut;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
    }
}