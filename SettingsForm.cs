using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ConquerLauncher
{
    public class SettingsForm : Form
    {
        private ListBox lstCursors;
        private ListBox lstShadows;
        private ComboBox cmbFonts;
        private Button btnOk;
        private Button btnCancel;

        public SettingsForm()
        {
            InitializeComponent();
            LoadOptions();
        }

        private void InitializeComponent()
        {
            this.Text = "Settings";
            this.ClientSize = new Size(520, 420);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var lblCursor = new Label() { Text = "Change Cursor", Location = new Point(10, 10), ForeColor = Color.White };
            lstCursors = new ListBox() { Location = new Point(10, 30), Size = new Size(240, 220) };

            var lblShadow = new Label() { Text = "Change Shadow", Location = new Point(270, 10), ForeColor = Color.White };
            lstShadows = new ListBox() { Location = new Point(270, 30), Size = new Size(240, 220) };

            var lblFont = new Label() { Text = "Change Font", Location = new Point(10, 260), ForeColor = Color.White };
            cmbFonts = new ComboBox() { Location = new Point(10, 280), Size = new Size(300, 24), DropDownStyle = ComboBoxStyle.DropDownList };

            btnOk = new Button() { Text = "OK", Location = new Point(340, 360), Size = new Size(80, 32) };
            btnCancel = new Button() { Text = "Cancel", Location = new Point(430, 360), Size = new Size(80, 32) };

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { lblCursor, lstCursors, lblShadow, lstShadows, lblFont, cmbFonts, btnOk, btnCancel });

            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;
        }

        private void LoadOptions()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var imgDir = Path.Combine(baseDir, "setting");

            // load cursor images (filenames starting with cursor_)
            try
            {
                if (Directory.Exists(imgDir))
                {
                    var cursorFiles = Directory.GetFiles(imgDir, "cursor_*.png").OrderBy(x => x).ToArray();
                    lstCursors.Items.AddRange(cursorFiles.Select(Path.GetFileName).ToArray());

                    var shadowFiles = Directory.GetFiles(imgDir, "shadow_*.png").OrderBy(x => x).ToArray();
                    lstShadows.Items.AddRange(shadowFiles.Select(Path.GetFileName).ToArray());
                }
            }
            catch { }

            // load system fonts
            foreach (var f in System.Drawing.FontFamily.Families)
            {
                cmbFonts.Items.Add(f.Name);
            }
            if (cmbFonts.Items.Count > 0) cmbFonts.SelectedIndex = 0;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            // apply selected options (save to local config file)
            try
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var cfg = Path.Combine(baseDir, "launcher_settings.json");
                var obj = new
                {
                    Cursor = lstCursors.SelectedItem as string,
                    Shadow = lstShadows.SelectedItem as string,
                    Font = cmbFonts.SelectedItem as string
                };
                File.WriteAllText(cfg, Newtonsoft.Json.JsonConvert.SerializeObject(obj));
            }
            catch { }
            this.Close();
        }
    }
}
