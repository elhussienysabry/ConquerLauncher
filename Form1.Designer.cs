using System;
using System.IO;
using System.Drawing;

namespace ConquerLauncher
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBarFile;
        private System.Windows.Forms.ProgressBar progressBarTotal;
        private System.Windows.Forms.WebBrowser newsBox;
        private System.Windows.Forms.PictureBox btnDiscord;
        private System.Windows.Forms.PictureBox btnFacebook;
        private System.Windows.Forms.PictureBox btnWebsite;
        private System.Windows.Forms.PictureBox btnRegister;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressBarFile = new System.Windows.Forms.ProgressBar();
            this.progressBarTotal = new System.Windows.Forms.ProgressBar();
            this.newsBox = new System.Windows.Forms.WebBrowser();
            this.btnDiscord = new System.Windows.Forms.PictureBox();
            this.btnFacebook = new System.Windows.Forms.PictureBox();
            this.btnWebsite = new System.Windows.Forms.PictureBox();
            this.btnRegister = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDiscord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFacebook)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnWebsite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRegister)).BeginInit();
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            try { this.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "img", "bg.png")); } catch { try { this.BackgroundImage = global::ConquerLauncher.Properties.Resources.bg; } catch { this.BackgroundImage = null; } }
            // 
            // picLogo (hidden - logo is in background)
            // 
            this.picLogo.Visible = false;
            this.picLogo.Location = new System.Drawing.Point(300, 20);
            this.picLogo.Size = new System.Drawing.Size(400, 100);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.BackColor = System.Drawing.Color.Transparent;
            try { this.picLogo.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "img", "logo.png")); } catch { this.picLogo.Image = null; }
            // 
            // btnClose (PictureBox)
            // 
            try { this.btnClose.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "img", "close.png")); } catch { try { this.btnClose.Image = global::ConquerLauncher.Properties.Resources.close; } catch { this.btnClose.Image = null; } }
            this.btnClose.Location = new System.Drawing.Point(950, 10);
            this.btnClose.Size = new System.Drawing.Size(40, 40);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.btnClose.TabStop = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnStart (positioned below logo in background, same size as register)
            // 
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            try { this.btnStart.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "img", "start.png")); } catch { try { this.btnStart.BackgroundImage = global::ConquerLauncher.Properties.Resources.start; } catch { } }
            int startWidth = 200; int startHeight = 50;
            int startX = (this.ClientSize.Width - startWidth) / 2;
            int startY = 300; // نزل تحت أكتر
            this.btnStart.Location = new System.Drawing.Point(startX, startY);
            this.btnStart.Size = new System.Drawing.Size(startWidth, startHeight);
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.Enabled = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            this.btnStart.MouseEnter += new System.EventHandler(this.btnStart_MouseEnter);
            this.btnStart.MouseLeave += new System.EventHandler(this.btnStart_MouseLeave);
            // 
            // btnRegister (centered below start button, similar size)
            // 
            this.btnRegister.BackColor = System.Drawing.Color.Transparent;
            this.btnRegister.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.btnRegister.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            int regWidth = 200; int regHeight = 50;
            int regX = (this.ClientSize.Width - regWidth) / 2;
            int regY = startY + startHeight + 20; // 20px gap below start
            this.btnRegister.Location = new System.Drawing.Point(regX, regY);
            this.btnRegister.Size = new System.Drawing.Size(regWidth, regHeight);
            try { this.btnRegister.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "img", "register.png")); } catch { this.btnRegister.Image = null; }
            this.btnRegister.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            this.btnRegister.MouseEnter += new System.EventHandler(this.btnRegister_MouseEnter);
            this.btnRegister.MouseLeave += new System.EventHandler(this.btnRegister_MouseLeave);
            // 
            // lblStatus and progress bars hidden by default
            // 
            this.lblStatus.Text = "Ready";
            this.lblStatus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.AutoSize = false;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatus.Location = new System.Drawing.Point(300, 480);
            this.lblStatus.Size = new System.Drawing.Size(400, 25);
            this.lblStatus.Visible = false;
            // 
            this.progressBarFile.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarFile.Size = new System.Drawing.Size(900, 18);
            this.progressBarFile.Location = new System.Drawing.Point(50, 495);
            this.progressBarFile.ForeColor = System.Drawing.Color.Cyan;
            this.progressBarFile.Visible = false;
            // 
            this.progressBarTotal.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarTotal.Size = new System.Drawing.Size(900, 18);
            this.progressBarTotal.Location = new System.Drawing.Point(50, 520);
            this.progressBarTotal.ForeColor = System.Drawing.Color.LimeGreen;
            this.progressBarTotal.Visible = false;
            // 
            // newsBox hidden (user requested only icons and buttons centered)
            // 
            this.newsBox.Visible = false;
            // 
            // social icons small at bottom-right
            // 
            this.btnDiscord.Visible = true;
            this.btnDiscord.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnDiscord.Size = new System.Drawing.Size(36,36);
            this.btnDiscord.Location = new System.Drawing.Point(780, 520);
            try { this.btnDiscord.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "img", "discord.png")); } catch { try { this.btnDiscord.Image = global::ConquerLauncher.Properties.Resources.discord; } catch { } }
            this.btnDiscord.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDiscord.Click += new System.EventHandler(this.btnDiscord_Click);

            this.btnFacebook.Visible = true;
            this.btnFacebook.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnFacebook.Size = new System.Drawing.Size(36,36);
            this.btnFacebook.Location = new System.Drawing.Point(820, 520);
            try { this.btnFacebook.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "img", "facebook.png")); } catch { try { this.btnFacebook.Image = global::ConquerLauncher.Properties.Resources.facebook; } catch { } }
            this.btnFacebook.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFacebook.Click += new System.EventHandler(this.btnFacebook_Click);

            // hide the old small settings icon variant (we use centered btnSettings now)
            this.btnWebsite.Visible = false;
            // end icons

            // enable double buffering
            this.DoubleBuffered = true;

            // 
            // Form1 Controls
            // 
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.btnClose);
            // ensure close button is above all other controls
            this.btnClose.BringToFront();
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBarFile);
            this.Controls.Add(this.progressBarTotal);
            this.Controls.Add(this.btnDiscord);
            this.Controls.Add(this.btnFacebook);

            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDiscord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFacebook)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnWebsite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRegister)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
