using System;
using System.IO;
using System.Windows.Forms;

using SvgNet;
using SvgNet.Elements;
using SvgNet.Types;

namespace SvgDocTest {
    public partial class DocForm : Form {
        public DocForm() => InitializeComponent();

        private static readonly string _tempFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "foo.svg");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() => Application.Run(new DocForm());

        private void Button1_Click(object sender, EventArgs e) {
            using (var dlg = new OpenFileDialog {
                AutoUpgradeEnabled = true,
                CheckFileExists = true,
                DefaultExt = ".svg",
                Filter = "Scalable Vector Graphics|*.svg",
                Multiselect = false,
                Title = "Choose one Scalable Vector Graphics file"
            }) {
                if (dlg.ShowDialog() == DialogResult.OK) {
                    ProcessSvgFile(dlg.FileName);
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e) {
            var root = new SvgSvgElement("4in", "4in", "-10,-10 250,250");

            //adding multiple children

            _ = root.AddChildren(
                new SvgRectElement(5, 5, 5, 5),
                new SvgEllipseElement(20, 20, 8, 12) {
                    Style = "fill:yellow;stroke:red"
                },

                new SvgAElement("https://github.com/managed-commons/SvgNet").AddChildren(
                    new SvgTextElement("Textastic!", "30px", "20px") {
                        Style = "fill:midnightblue;stroke:navy;stroke-width:1px;font-size:30px;font-family:Calibri"
                    })
                );

            //group and path

            var grp = new SvgGroupElement("green_group") {
                Style = "fill:green;stroke:black;"
            };

            grp.AddChild(new SvgRectElement(30, 30, 5, 20));

            var ell = new SvgEllipseElement {
                CX = 50,
                CY = 50,
                RX = 10,
                RY = 20
            };

            var pathy = new SvgPathElement {
                D = "M 20,80 C 20,90 30,80 70,100 C 70,100 40,60 50,60 z",
                Style = ell.Style
            };

            root.AddChild(grp);

            //cloning and style arithmetic

            _ = grp.AddChildren(ell, pathy);

            grp.Style.Set("fill", "blue");

            var grp2 = (SvgGroupElement)SvgFactory.CloneElement(grp);

            grp2.Id = "cloned_red_group";

            grp2.Style.Set("fill", "red");

            grp2.Style += "opacity:0.5";

            grp2.Transform = "scale (1.2, 1.2)  translate(10)";

            root.AddChild(grp2);

            //output
            SvgFactory.ResetNamespaces();
            string s = root.WriteSVGString(true);
            tbIn.Text = s;
            tbOut.Text = s;

            string tempFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "foo.svg");

            using (var tw = new StreamWriter(tempFile, false))
                tw.Write(s);
            panelTop.Text = $"Input: {tempFile}";
            svgOut.RefreshFrom(tempFile);
            svgIn.RefreshFrom(tempFile);
        }

        private void Button3_Click(object sender, EventArgs e) {
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

            _ = MessageBox.Show("Tests completed Ok");
        }
        private void ProcessSvgFile(string svgFileName) {
            panelTop.Text = $"Input: {svgFileName}";
            tbIn.Text = svgFileName.LoadText();
            tbOut.Text = SvgFactory.LoadFromXML(svgFileName.LoadXml(), null).WriteSVGString(true);
            File.WriteAllText(_tempFileName, tbOut.Text);
            svgIn.RefreshFrom(svgFileName);
            svgOut.RefreshFrom(_tempFileName);
        }
    }
}
