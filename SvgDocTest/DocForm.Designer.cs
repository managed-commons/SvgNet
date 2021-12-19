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
            this.svgOut = new System.Windows.Forms.WebBrowser();
            this.svgIn = new System.Windows.Forms.WebBrowser();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.tbIn = new System.Windows.Forms.TextBox();
            this.tbOut = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // svgOut
            // 
            this.svgOut.Location = new System.Drawing.Point(404, 309);
            this.svgOut.MinimumSize = new System.Drawing.Size(20, 20);
            this.svgOut.Name = "svgOut";
            this.svgOut.Size = new System.Drawing.Size(495, 232);
            this.svgOut.TabIndex = 20;
            // 
            // svgIn
            // 
            this.svgIn.Location = new System.Drawing.Point(404, 41);
            this.svgIn.MinimumSize = new System.Drawing.Size(20, 20);
            this.svgIn.Name = "svgIn";
            this.svgIn.Size = new System.Drawing.Size(495, 230);
            this.svgIn.TabIndex = 19;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Cascadia Code", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.DarkMagenta;
            this.button2.Location = new System.Drawing.Point(549, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(242, 32);
            this.button2.TabIndex = 18;
            this.button2.Text = "Run Composition Tests";
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Cascadia Code", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 285);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 24);
            this.label2.TabIndex = 17;
            this.label2.Text = "Output:";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Cascadia Code", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 24);
            this.label1.TabIndex = 16;
            this.label1.Text = "Input:";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Cascadia Code", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.DarkMagenta;
            this.button3.Location = new System.Drawing.Point(307, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(242, 32);
            this.button3.TabIndex = 15;
            this.button3.Text = "Run Type Tests";
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // tbIn
            // 
            this.tbIn.Font = new System.Drawing.Font("Cascadia Code", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbIn.Location = new System.Drawing.Point(12, 41);
            this.tbIn.Multiline = true;
            this.tbIn.Name = "tbIn";
            this.tbIn.Size = new System.Drawing.Size(384, 232);
            this.tbIn.TabIndex = 14;
            this.tbIn.Text = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>";
            // 
            // tbOut
            // 
            this.tbOut.Font = new System.Drawing.Font("Cascadia Code", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbOut.Location = new System.Drawing.Point(12, 309);
            this.tbOut.Multiline = true;
            this.tbOut.Name = "tbOut";
            this.tbOut.Size = new System.Drawing.Size(384, 232);
            this.tbOut.TabIndex = 13;
            this.tbOut.Text = "textBox1";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Cascadia Code", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.DarkMagenta;
            this.button1.Location = new System.Drawing.Point(65, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(242, 32);
            this.button1.TabIndex = 12;
            this.button1.Text = "Test an SVG file";
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // DocForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 555);
            this.Controls.Add(this.svgOut);
            this.Controls.Add(this.svgIn);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.tbIn);
            this.Controls.Add(this.tbOut);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Name = "DocForm";
            this.Text = "DocForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser svgOut;
        private System.Windows.Forms.WebBrowser svgIn;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox tbIn;
        private System.Windows.Forms.TextBox tbOut;
        private System.Windows.Forms.Button button1;
    }
}