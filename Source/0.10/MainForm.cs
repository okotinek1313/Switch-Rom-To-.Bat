using System;
using System.IO;
using System.Windows.Forms;

namespace YuzuRomManager
{
    public partial class MainForm : Form
    {
        private System.Windows.Forms.Button btnSelectRom;

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnSelectRom = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // btnSelectRom
            // 
            btnSelectRom.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            btnSelectRom.FlatAppearance.BorderSize = 0;
            btnSelectRom.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            btnSelectRom.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            btnSelectRom.Location = new System.Drawing.Point(10, 9);
            btnSelectRom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            btnSelectRom.Name = "btnSelectRom";
            btnSelectRom.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            btnSelectRom.Size = new System.Drawing.Size(328, 55);
            btnSelectRom.TabIndex = 0;
            btnSelectRom.Text = "Select ROM";
            btnSelectRom.UseVisualStyleBackColor = true;
            btnSelectRom.UseWaitCursor = true;
            btnSelectRom.Click += btnSelectRom_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            ClientSize = new System.Drawing.Size(350, 75);
            Controls.Add(btnSelectRom);
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Text = "Yuzu ROM Manager";
            ResumeLayout(false);
        }

        private void btnSelectRom_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Switch ROMs (*.nsp;*.xci)|*.nsp;*.xci",
                Title = "Select a Switch ROM"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string romPath = ofd.FileName;
                string romName = Path.GetFileNameWithoutExtension(romPath);
                string gameDir = Path.Combine(Application.StartupPath, "Games", romName);
                string newRomPath = Path.Combine(gameDir, Path.GetFileName(romPath));

                Directory.CreateDirectory(gameDir);
                File.Copy(romPath, newRomPath, true);

                // Path to your Yuzu template folder - adjust if needed
                string yuzuSrc = Path.Combine(Application.StartupPath, "YuzuTemplate", "yuzu");
                string yuzuDst = Path.Combine(gameDir, "yuzu");

                CopyDirectory(yuzuSrc, yuzuDst);

                string batContent = @$"start """" ""yuzu\yuzu.exe"" ""{Path.GetFileName(newRomPath)}""";
                File.WriteAllText(Path.Combine(gameDir, "PlayGame.bat"), batContent);

                MessageBox.Show("Game setup complete!");
            }
        }

        private void CopyDirectory(string sourceDir, string targetDir)
        {
            foreach (string file in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(sourceDir, file);
                string destinationPath = Path.Combine(targetDir, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);
                File.Copy(file, destinationPath, true);
            }
        }
    }
}
