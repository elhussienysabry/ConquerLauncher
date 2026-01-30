using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing.Text;

namespace ConquerLauncher
{
    public partial class Form1 : Form
    {
        // Web Server (للتحديثات والأخبار)
        private const string SERVER_URL = "https://clashconquer.games/updates/";  // ⬅️ المسار الجديد
        private const string VERSION_FILE = "version.txt";
        private const string FILES_LIST = "files.json";
        private const string NEWS_FILE = "news.html";
        private const string LOCAL_VERSION_FILE = "version.ini";
        
        // Game Server (سيرفر اللعبة نفسها)
        private const string GAME_SERVER_IP = "152.228.131.13";
        private const int GAME_SERVER_PORT = 9960;
        
        // Game Executables
        private const string GAME_EXECUTABLE = "Conquer.exe";
        private const string DX8_EXECUTABLE = "Start_DX8.bat";
        private const string DX9_EXECUTABLE = "Start_DX9.bat";

        private string gameRoot;
        private string localVersion = "0";
        private string serverVersion = "0";
        private List<ServerFile> serverFiles = new List<ServerFile>();
        private List<ServerFile> filesToUpdate = new List<ServerFile>();
        private HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(30) };
        private bool isUpdating = false;
        private bool gameServerOnline = false;

        public Form1()
        {
            InitializeComponent();
            gameRoot = AppDomain.CurrentDomain.BaseDirectory;
            // suppress script error dialogs from the underlying ActiveX control
            try
            {
                if (newsBox != null)
                {
                    newsBox.ScriptErrorsSuppressed = true;
                    SuppressWebBrowserScriptErrors(newsBox, true);
                }
            }
            catch { }

            Shown += async (s, e) => await InitializeLauncher();
        }

        private void SuppressWebBrowserScriptErrors(WebBrowser wb, bool hide)
        {
            if (wb == null) return;
            try
            {
                var ax = wb.GetType().GetProperty("ActiveXInstance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(wb, null);
                if (ax != null)
                {
                    ax.GetType().InvokeMember("Silent", System.Reflection.BindingFlags.SetProperty, null, ax, new object[] { hide });
                }
            }
            catch { }
        }

        private void SetUpdateUI(bool visible)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetUpdateUI(visible)));
                return;
            }

            lblStatus.Visible = visible;
            progressBarFile.Visible = visible;
            progressBarTotal.Visible = visible;

            if (visible)
            {
                lblStatus.Text = "Preparing update...";
                progressBarFile.Style = ProgressBarStyle.Marquee;
                progressBarTotal.Style = ProgressBarStyle.Continuous;
                progressBarTotal.Minimum = 0;
                progressBarTotal.Value = 0;
                progressBarTotal.Maximum = Math.Max(1, filesToUpdate.Count);
            }
            else
            {
                lblStatus.Text = "Ready";
                progressBarFile.Style = ProgressBarStyle.Continuous;
                progressBarFile.Value = 0;
                progressBarTotal.Value = 0;
            }
        }

        private async Task InitializeLauncher()
        {
            try
            {
                if (!ValidatePaths())
                {
                    ShowError("Critical files missing. Cannot start.");
                    DisableStartButton();
                    return;
                }

                localVersion = ReadLocalVersion();
                await LoadNews();
                await CheckServerVersion();
                await ValidateGameFiles();
                
                // Check game server status
                _ = CheckGameServerStatus();

                if (filesToUpdate.Count > 0)
                {
                    SetUpdateUI(true);
                    PromptForUpdate();
                }
                else
                {
                    SetUpdateUI(false);
                    EnableStartButton();
                }
            }
            catch (Exception ex)
            {
                Log("ERROR", ex.Message);
                ShowError("Initialization failed: " + ex.Message);
                DisableStartButton();
            }
        }

        private bool ValidatePaths()
        {
            try
            {
                if (!Directory.Exists(Path.Combine(gameRoot, "Logs")))
                    Directory.CreateDirectory(Path.Combine(gameRoot, "Logs"));

                if (!File.Exists(Path.Combine(gameRoot, LOCAL_VERSION_FILE)))
                    File.WriteAllText(Path.Combine(gameRoot, LOCAL_VERSION_FILE), "1000");

                return true;
            }
            catch (Exception ex)
            {
                Log("ERROR", ex.Message);
                return false;
            }
        }

        private string ReadLocalVersion()
        {
            try
            {
                return File.ReadAllText(Path.Combine(gameRoot, LOCAL_VERSION_FILE)).Trim();
            }
            catch { return "0"; }
        }

        private async Task LoadNews()
        {
            try
            {
                var newsUrl = SERVER_URL + NEWS_FILE;
                var html = await httpClient.GetStringAsync(newsUrl);
                if (newsBox != null) newsBox.DocumentText = html;
            }
            catch (Exception ex)
            {
                Log("WARN", "Failed to load news: " + ex.Message);
            }
        }

        private void PromptForUpdate()
        {
            try
            {
                var count = filesToUpdate?.Count ?? 0;
                // show status only when there are updates
                SetUpdateUI(count > 0);
                var dr = MessageBox.Show(this, $"{count} update(s) available. Download now?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    _ = DownloadAndApplyUpdates();
                }
                else
                {
                    // hide update UI if user declines
                    SetUpdateUI(false);
                    EnableStartButton(false);
                }
            }
            catch (Exception ex)
            {
                Log("ERROR", ex.Message);
            }
        }

        private async Task CheckServerVersion()
        {
            try
            {
                var ver = await httpClient.GetStringAsync(SERVER_URL + VERSION_FILE);
                serverVersion = ver.Trim();

                var filesJson = await httpClient.GetStringAsync(SERVER_URL + FILES_LIST);
                serverFiles = JsonConvert.DeserializeObject<List<ServerFile>>(filesJson);
            }
            catch (Exception ex)
            {
                Log("ERROR", ex.Message);
            }
        }

        private Task ValidateGameFiles()
        {
            filesToUpdate.Clear();
            foreach (var serverFile in serverFiles)
            {
                string localPath = Path.Combine(gameRoot, serverFile.RelativePath ?? string.Empty, serverFile.FileName);
                bool needsUpdate = false;
                try
                {
                    if (!File.Exists(localPath))
                    {
                        needsUpdate = true;
                        Log("INFO", "Missing: " + serverFile.FileName);
                    }
                }
                catch (Exception ex)
                {
                    Log("ERROR", ex.Message);
                    needsUpdate = true;
                }

                if (needsUpdate)
                    filesToUpdate.Add(serverFile);
            }

            EnableStartButton(filesToUpdate.Count == 0);
            return Task.CompletedTask;
        }

        private async Task DownloadAndApplyUpdates()
        {
            isUpdating = true;
            DisableStartButton();
            SetUpdateUI(true);
            var tempPath = Path.Combine(gameRoot, "_tmp_download");

            try
            {
                Directory.CreateDirectory(tempPath);
                int total = filesToUpdate.Count;
                int done = 0;
                progressBarTotal.Minimum = 0;
                progressBarTotal.Maximum = Math.Max(1, total);
                progressBarTotal.Value = 0;

                foreach (var file in filesToUpdate)
                {
                    lblStatus.Text = $"Downloading {file.FileName} ({done + 1}/{total})";
                    progressBarFile.Style = ProgressBarStyle.Marquee;

                    var tmpFile = Path.Combine(tempPath, file.FileName);
                    using (var resp = await httpClient.GetAsync(file.DownloadURL, HttpCompletionOption.ResponseHeadersRead))
                    {
                        resp.EnsureSuccessStatusCode();
                        using (var contentStream = await resp.Content.ReadAsStreamAsync())
                        using (var fs = new FileStream(tmpFile, FileMode.Create, FileAccess.Write))
                        {
                            await contentStream.CopyToAsync(fs);
                        }
                    }

                    var finalPath = Path.Combine(gameRoot, file.RelativePath ?? string.Empty, file.FileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(finalPath));
                    File.Copy(tmpFile, finalPath, true);

                    done++;
                    progressBarTotal.Value = Math.Min(progressBarTotal.Maximum, done);
                    progressBarFile.Style = ProgressBarStyle.Continuous;
                    progressBarFile.Value = 0;
                }

                File.Delete(Path.Combine(gameRoot, LOCAL_VERSION_FILE));
                File.WriteAllText(Path.Combine(gameRoot, LOCAL_VERSION_FILE), serverVersion);

                await ValidateGameFiles();
            }
            catch (Exception ex)
            {
                Log("ERROR", ex.Message);
                ShowError("Update failed: " + ex.Message);
            }
            finally
            {
                try { Directory.Delete(tempPath, true); } catch { }
                isUpdating = false;
                SetUpdateUI(false);
                if (filesToUpdate.Count == 0) EnableStartButton();
            }
        }



        private void EnableStartButton(bool enabled = true)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => btnStart.Enabled = enabled));
                return;
            }
            btnStart.Enabled = enabled;
            lblStatus.Text = enabled ? "Ready" : "Update required";
        }

        private void DisableStartButton()
        {
            EnableStartButton(false);
        }

        private void ShowError(string message)
        {
            MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Log(string level, string message)
        {
            try
            {
                var logPath = Path.Combine(gameRoot, "Logs", "Launcher.log");
                var entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}\n";
                File.AppendAllText(logPath, entry, Encoding.UTF8);
            }
            catch { }
        }

        private async Task CheckGameServerStatus()
        {
            try
            {
                Log("INFO", $"Checking game server: {GAME_SERVER_IP}:{GAME_SERVER_PORT}");
                
                using (var client = new System.Net.Sockets.TcpClient())
                {
                    // Try to connect with 5 second timeout
                    var connectTask = client.ConnectAsync(GAME_SERVER_IP, GAME_SERVER_PORT);
                    var timeoutTask = Task.Delay(5000);
                    
                    if (await Task.WhenAny(connectTask, timeoutTask) == connectTask)
                    {
                        if (client.Connected)
                        {
                            gameServerOnline = true;
                            Log("INFO", "Game server is ONLINE");
                            UpdateServerStatus("🟢 Server Online", Color.LimeGreen);
                            return;
                        }
                    }
                }
                
                // If we reach here, connection failed
                gameServerOnline = false;
                Log("WARN", "Game server is OFFLINE or unreachable");
                UpdateServerStatus("🔴 Server Offline", Color.Red);
            }
            catch (Exception ex)
            {
                gameServerOnline = false;
                Log("ERROR", $"Failed to check game server status: {ex.Message}");
                UpdateServerStatus("⚠️ Server Check Failed", Color.Orange);
            }
        }

        private void UpdateServerStatus(string text, Color color)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateServerStatus(text, color)));
                return;
            }
            
            // Update lblStatus if no update is in progress
            if (!isUpdating && lblStatus != null && !lblStatus.Visible)
            {
                lblStatus.Text = text;
                lblStatus.ForeColor = color;
                lblStatus.Visible = true;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _ = StartGameAsync();
        }

        private async Task StartGameAsync()
        {
            try
            {
                // Validate game files first
                await CheckServerVersion();
                await ValidateGameFiles();

                if (filesToUpdate.Count > 0)
                {
                    ShowError($"{filesToUpdate.Count} file(s) invalid. Update required!");
                    var dr = MessageBox.Show("Update now?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                        await DownloadAndApplyUpdates();
                    return;
                }

                if (CompareVersions(serverVersion, localVersion) > 0)
                {
                    ShowError("Version mismatch. Update required!");
                    return;
                }

                // Check game server status before launching
                await CheckGameServerStatus();
                
                if (!gameServerOnline)
                {
                    var result = MessageBox.Show(
                        $"⚠️ Warning: Game Server appears to be OFFLINE!\n\n" +
                        $"Server: {GAME_SERVER_IP}:{GAME_SERVER_PORT}\n\n" +
                        $"The game may not work properly. Do you want to continue anyway?",
                        "Server Offline Warning",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    
                    if (result != DialogResult.Yes)
                    {
                        Log("INFO", "User cancelled launch due to offline server");
                        return;
                    }
                    
                    Log("INFO", "User chose to launch despite offline server");
                }

                // Show graphics mode selection dialog
                ShowGraphicsModeSelection();
            }
            catch (Exception ex)
            {
                Log("ERROR", ex.Message);
                ShowError("Failed to start game: " + ex.Message);
            }
        }

        private void ShowGraphicsModeSelection()
        {
            // Create modal dialog
            var selectionForm = new Form
            {
                Text = "Select Graphics Mode",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(500, 300),
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(20, 20, 30),
                ShowInTaskbar = false
            };

            // Title label
            var lblTitle = new Label
            {
                Text = "Choose Graphics Mode",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(460, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            selectionForm.Controls.Add(lblTitle);

            // High Quality Button
            var btnHigh = new Button
            {
                Text = "HIGH QUALITY\n(DirectX 9)",
                Size = new Size(200, 150),
                Location = new Point(30, 80),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Tag = "HIGH"
            };
            btnHigh.FlatAppearance.BorderSize = 0;
            btnHigh.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 150, 255);
            
            // Try to load custom image for High button
            try
            {
                var highImgPath = Path.Combine(gameRoot, "Data", "img", "graphics_high.png");
                if (File.Exists(highImgPath))
                {
                    btnHigh.BackgroundImage = Image.FromFile(highImgPath);
                    btnHigh.BackgroundImageLayout = ImageLayout.Stretch;
                    btnHigh.Text = ""; // Hide text if image exists
                }
            }
            catch { }

            // Low Quality Button
            var btnLow = new Button
            {
                Text = "LOW QUALITY\n(DirectX 8)",
                Size = new Size(200, 150),
                Location = new Point(260, 80),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(100, 100, 100),
                ForeColor = Color.White,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Tag = "LOW"
            };
            btnLow.FlatAppearance.BorderSize = 0;
            btnLow.FlatAppearance.MouseOverBackColor = Color.FromArgb(130, 130, 130);
            
            // Try to load custom image for Low button
            try
            {
                var lowImgPath = Path.Combine(gameRoot, "Data", "img", "graphics_low.png");
                if (File.Exists(lowImgPath))
                {
                    btnLow.BackgroundImage = Image.FromFile(lowImgPath);
                    btnLow.BackgroundImageLayout = ImageLayout.Stretch;
                    btnLow.Text = ""; // Hide text if image exists
                }
            }
            catch { }

            // Button click handlers
            btnHigh.Click += (s, e) =>
            {
                selectionForm.DialogResult = DialogResult.OK;
                selectionForm.Tag = "HIGH";
                selectionForm.Close();
            };

            btnLow.Click += (s, e) =>
            {
                selectionForm.DialogResult = DialogResult.OK;
                selectionForm.Tag = "LOW";
                selectionForm.Close();
            };

            selectionForm.Controls.Add(btnHigh);
            selectionForm.Controls.Add(btnLow);

            // Show dialog and handle selection
            if (selectionForm.ShowDialog(this) == DialogResult.OK)
            {
                var mode = selectionForm.Tag as string;
                LaunchGameWithMode(mode);
            }
        }

        private void LaunchGameWithMode(string mode)
        {
            try
            {
                string executableName = mode == "HIGH" ? DX9_EXECUTABLE : DX8_EXECUTABLE;
                string executablePath = Path.Combine(gameRoot, executableName);

                // Check if the executable exists
                if (!File.Exists(executablePath))
                {
                    ShowError($"Game executable not found: {executableName}\n\nPlease make sure the file exists in the game directory.");
                    Log("ERROR", $"Executable not found: {executablePath}");
                    return;
                }

                Log("INFO", $"Launching game with {mode} quality mode: {executableName}");

                // Launch the batch file properly using cmd.exe
                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c \"{executablePath}\"",
                    WorkingDirectory = gameRoot,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process.Start(startInfo);

                // Close launcher after successful launch
                Task.Delay(1000).ContinueWith(_ => Application.Exit(), TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                Log("ERROR", $"Failed to launch game: {ex.Message}");
                ShowError($"Failed to launch game: {ex.Message}");
            }
        }

        private int CompareVersions(string a, string b)
        {
            try
            {
                var va = a.Split('.').Select(int.Parse).ToArray();
                var vb = b.Split('.').Select(int.Parse).ToArray();
                for (int i = 0; i < Math.Max(va.Length, vb.Length); i++)
                {
                    int ai = i < va.Length ? va[i] : 0;
                    int bi = i < vb.Length ? vb[i] : 0;
                    if (ai != bi) return ai.CompareTo(bi);
                }
            }
            catch { }
            return 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (isUpdating)
            {
                var dr = MessageBox.Show("An update is in progress. Are you sure you want to exit?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr != DialogResult.Yes) return;
            }
            Application.Exit();
        }

        private void btnDiscord_Click(object sender, EventArgs e)
        {
            try { Process.Start(new ProcessStartInfo { FileName = "https://discord.gg/CffXDaMJmY", UseShellExecute = true }); } catch { }
        }

        private void btnFacebook_Click(object sender, EventArgs e)
        {
            try { Process.Start(new ProcessStartInfo { FileName = "https://facebook.com/Clashconquerr", UseShellExecute = true }); } catch { }
        }

        private void btnWebsite_Click(object sender, EventArgs e)
        {
            try { Process.Start(new ProcessStartInfo { FileName = "https://clashconquer.games", UseShellExecute = true }); } catch { }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                // صفحة التسجيل في المسار الرئيسي مش في updates
                var regUrl = "https://clashconquer.games/register";
                Process.Start(new ProcessStartInfo { FileName = regUrl, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Log("ERROR", "Failed to open register page: " + ex.Message);
            }
        }

        // Mouse Hover Effects for START button
        private void btnStart_MouseEnter(object sender, EventArgs e)
        {
            if (btnStart.Enabled)
            {
                // Scale up effect
                btnStart.Size = new Size(215, 55);
                btnStart.Location = new Point(btnStart.Location.X - 7, btnStart.Location.Y - 2);
            }
        }

        private void btnStart_MouseLeave(object sender, EventArgs e)
        {
            if (btnStart.Enabled)
            {
                // Reset to original size
                int startWidth = 200;
                int startHeight = 50;
                int startX = (this.ClientSize.Width - startWidth) / 2;
                int startY = 300;  // نزل تحت أكتر
                
                btnStart.Size = new Size(startWidth, startHeight);
                btnStart.Location = new Point(startX, startY);
                btnStart.BackColor = Color.Transparent;
            }
        }

        // Mouse Hover Effects for REGISTER button
        private void btnRegister_MouseEnter(object sender, EventArgs e)
        {
            // Scale up effect
            btnRegister.Size = new Size(215, 55);
            btnRegister.Location = new Point(btnRegister.Location.X - 7, btnRegister.Location.Y - 2);
        }

        private void btnRegister_MouseLeave(object sender, EventArgs e)
        {
            // Reset to original size
            int startY = 300;  // نزل تحت أكتر
            int startHeight = 50;
            int regWidth = 200;
            int regHeight = 50;
            int regX = (this.ClientSize.Width - regWidth) / 2;
            int regY = startY + startHeight + 20;
            
            btnRegister.Size = new Size(regWidth, regHeight);
            btnRegister.Location = new Point(regX, regY);
        }
    }

    public class ServerFile
    {
        public string FileName { get; set; }
        public string RelativePath { get; set; }
        public string DownloadURL { get; set; }
    }
}
