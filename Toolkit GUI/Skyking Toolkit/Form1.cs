using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Skyking_Toolkit
{
    public partial class Form1 : Form
    {
        private int parallaxTotalSets = 0;
        private int parallaxDoneSets = 0;
        private int sourceTotalSets = 0;
        private int sourceDoneSets = 0;
        private int convertTotalSets = 0;
        private int convertDoneSets = 0;

        private readonly List<Process> runningProcesses = new List<Process>();
        private readonly object runningProcessesLock = new object();
        private bool isClosing = false;

        public Form1()
        {
            InitializeComponent();

            Text = "Skyking Toolkit";

            rbParallaxBoth.Checked = true;
            rbSourceBoth.Checked = true;
            rbConvertToPBR.Checked = true;
            if (txtJsonName != null && string.IsNullOrWhiteSpace(txtJsonName.Text))
                txtJsonName.Text = "my_materials";

            FormClosing -= Form1_FormClosing;
            FormClosing += Form1_FormClosing;

            btnBrowseSourceInput.Click -= btnBrowseSourceInput_Click;
            btnBrowseSourceInput.Click += btnBrowseSourceInput_Click;

            btnBrowseSourceOutput.Click -= btnBrowseSourceOutput_Click;
            btnBrowseSourceOutput.Click += btnBrowseSourceOutput_Click;

            btnRunSource.Click -= btnRunSource_Click_1;
            btnRunSource.Click -= btnRunSource_Click;
            btnRunSource.Click += btnRunSource_Click;

            btnBrowseConvertInput.Click -= btnBrowseConvertInput_Click;
            btnBrowseConvertInput.Click += btnBrowseConvertInput_Click;

            btnBrowseConvertOutput.Click -= btnBrowseConvertOutput_Click;
            btnBrowseConvertOutput.Click += btnBrowseConvertOutput_Click;

            btnRunConvert.Click -= btnRunConvert_Click;
            btnRunConvert.Click += btnRunConvert_Click;

            btnBrowseJsonInput.Click -= btnBrowseJsonInput_Click;
            btnBrowseJsonInput.Click += btnBrowseJsonInput_Click;

            btnRunJson.Click -= btnRunJson_Click;
            btnRunJson.Click += btnRunJson_Click;

            btnClearGlobalLog.Click -= btnClearGlobalLog_Click;
            btnClearGlobalLog.Click += btnClearGlobalLog_Click;

            btnSaveGlobalLog.Click -= btnSaveGlobalLog_Click;
            btnSaveGlobalLog.Click += btnSaveGlobalLog_Click;
        }

        private string FindToolkitRoot()
        {
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "runtime")) && Directory.Exists(Path.Combine(dir.FullName, "apps")))
                    return dir.FullName;
                dir = dir.Parent;
            }

            string fallback = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."));
            if (Directory.Exists(Path.Combine(fallback, "runtime")) && Directory.Exists(Path.Combine(fallback, "apps")))
                return fallback;

            return AppDomain.CurrentDomain.BaseDirectory;
        }

        private string ToolkitPath(params string[] parts)
        {
            string path = FindToolkitRoot();
            foreach (string part in parts)
                path = Path.Combine(path, part);
            return path;
        }

        private string Q(string value)
        {
            return "\"" + value + "\"";
        }

        private void BrowseInto(TextBox box)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    box.Text = dialog.SelectedPath;
            }
        }

        // =========================
        // Parallax Old Mods tab
        // =========================

        private void btnBrowseParallaxSource_Click(object sender, EventArgs e)
        {
            BrowseInto(txtParallaxSource);
        }

        private void btnBrowseParallaxOutput_Click(object sender, EventArgs e)
        {
            BrowseInto(txtParallaxOutput);
        }

        private void btnRunParallax_Click(object sender, EventArgs e)
        {
            txtParallaxLog.Clear();
            ResetParallaxProgress();

            if (string.IsNullOrWhiteSpace(txtParallaxSource.Text))
            {
                AppendLog("ERROR: No source folder selected.", txtParallaxLog);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtParallaxOutput.Text))
            {
                AppendLog("ERROR: No output folder selected.", txtParallaxLog);
                return;
            }

            string mode = "both";
            if (rbParallaxComplex.Checked) mode = "complex";
            else if (rbParallaxPBR.Checked) mode = "pbr";

            RunPython(
                ToolkitPath("apps", "parallax generator", "generate_parallax.py"),
                Q(txtParallaxSource.Text) + " " + Q(txtParallaxOutput.Text) + " " + mode,
                txtParallaxLog);
        }

        // =========================
        // Parallax From Source tab
        // =========================

        private void btnBrowseSourceInput_Click(object sender, EventArgs e)
        {
            BrowseInto(txtSourceInput);
        }

        private void btnBrowseSourceOutput_Click(object sender, EventArgs e)
        {
            BrowseInto(txtSourceOutput);
        }

        private void btnRunSource_Click(object sender, EventArgs e)
        {
            txtSourceLog.Clear();
            ResetSourceProgress();

            if (string.IsNullOrWhiteSpace(txtSourceInput.Text))
            {
                AppendLog("ERROR: No source folder selected.", txtSourceLog);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSourceOutput.Text))
            {
                AppendLog("ERROR: No output folder selected.", txtSourceLog);
                return;
            }

            string mode = "both";
            if (rbSourcePBR.Checked) mode = "pbr";
            else if (rbSourceComplex.Checked) mode = "complex";

            string flipArg = chkSourceFlipGreen.Checked ? "y" : "n";
            string pbrScript = ToolkitPath("apps", "pbr generator", "build_pbr.py");
            string pbrArgs = Q(txtSourceInput.Text) + " " + Q(txtSourceOutput.Text) + " " + flipArg;

            SetSourceRunEnabled(false);
            SetSourceProgressBlocksOnly();

            AppendLog("Starting source build...", txtSourceLog);

            RunPython(pbrScript, pbrArgs, txtSourceLog, exitCode =>
            {
                if (isClosing) return;

                if (exitCode != 0)
                {
                    AppendLog("Source build failed. Complex Parallax conversion was not started.", txtSourceLog);
                    SetSourceRunEnabled(true);
                    return;
                }

                if (mode == "pbr")
                {
                    if (sourceTotalSets > 0)
                        SetSourceProgress(sourceTotalSets, sourceTotalSets);
                    SetSourceRunEnabled(true);
                    return;
                }

                AppendLog("", txtSourceLog);
                AppendLog("PBR build completed. Starting Complex Parallax conversion...", txtSourceLog);

                string cpScript = ToolkitPath("apps", "complex parallax to pbr", "convert_pbr_to_complex.py");
                string cpArgs = Q(txtSourceOutput.Text) + " " + Q(txtSourceOutput.Text);

                RunPython(cpScript, cpArgs, txtSourceLog, cpExitCode =>
                {
                    if (isClosing) return;
                    if (cpExitCode == 0 && sourceTotalSets > 0)
                        SetSourceProgress(sourceTotalSets, sourceTotalSets);
                    SetSourceRunEnabled(true);
                });
            });
        }

        // =========================
        // PBR ⇆ Complex Parallax tab
        // =========================

        private void btnBrowseConvertInput_Click(object sender, EventArgs e)
        {
            BrowseInto(txtConvertInput);
        }

        private void btnBrowseConvertOutput_Click(object sender, EventArgs e)
        {
            BrowseInto(txtConvertOutput);
        }

        private void btnRunConvert_Click(object sender, EventArgs e)
        {
            txtConvertLog.Clear();
            ResetConvertProgress();

            if (string.IsNullOrWhiteSpace(txtConvertInput.Text))
            {
                AppendLog("ERROR: No input/source folder selected.", txtConvertLog);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtConvertOutput.Text))
            {
                AppendLog("ERROR: No output folder selected.", txtConvertLog);
                return;
            }

            string input = txtConvertInput.Text;
            string output = txtConvertOutput.Text;
            string script;
            string direction;

            // Designer text currently says:
            // rbConvertToPBR: "PBR → Complex Parallax"
            // rbConvertToCP : "Complex Parallax → PBR"
            if (rbConvertToPBR.Checked)
            {
                script = ToolkitPath("apps", "complex parallax to pbr", "convert_pbr_to_complex.py");
                direction = "PBR → Complex Parallax";
                SetConvertProgressTotal(CountTextureCandidates(input, false));
            }
            else
            {
                script = ToolkitPath("apps", "complex parallax to pbr", "convert.py");
                direction = "Complex Parallax → PBR";
                SetConvertProgressTotal(CountTextureCandidates(input, true));
            }

            string args = Q(input) + " " + Q(output);

            SetConvertRunEnabled(false);
            AppendLog("Starting conversion: " + direction, txtConvertLog);

            RunPython(script, args, txtConvertLog, exitCode =>
            {
                if (isClosing) return;

                if (exitCode == 0 && convertTotalSets > 0)
                    SetConvertProgress(convertTotalSets, convertTotalSets);

                SetConvertRunEnabled(true);
            });
        }

        private int CountTextureCandidates(string rootFolder, bool complexToPbr)
        {
            try
            {
                if (!Directory.Exists(rootFolder)) return 0;

                HashSet<string> candidates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (string file in Directory.EnumerateFiles(rootFolder, "*.*", SearchOption.AllDirectories))
                {
                    string ext = Path.GetExtension(file).ToLowerInvariant();
                    if (ext != ".dds" && ext != ".png" && ext != ".tga" && ext != ".jpg" && ext != ".jpeg" && ext != ".bmp")
                        continue;

                    string stem = Path.GetFileNameWithoutExtension(file);
                    string lower = stem.ToLowerInvariant();

                    bool marker = complexToPbr
                        ? lower.EndsWith("_m")
                        : lower.EndsWith("_p") || lower.EndsWith("_rmaos");

                    if (!marker) continue;

                    string baseName = stem;
                    if (baseName.EndsWith("_m", StringComparison.OrdinalIgnoreCase)) baseName = baseName.Substring(0, baseName.Length - 2);
                    if (baseName.EndsWith("_p", StringComparison.OrdinalIgnoreCase)) baseName = baseName.Substring(0, baseName.Length - 2);
                    if (baseName.EndsWith("_rmaos", StringComparison.OrdinalIgnoreCase)) baseName = baseName.Substring(0, baseName.Length - 6);

                    candidates.Add(Path.Combine(Path.GetDirectoryName(file) ?? string.Empty, baseName));
                }

                return candidates.Count;
            }
            catch
            {
                return 0;
            }
        }

        // =========================
        // JSON Generator tab
        // =========================

        private void btnBrowseJsonInput_Click(object sender, EventArgs e)
        {
            BrowseInto(txtJsonInput);
        }

        private void btnRunJson_Click(object sender, EventArgs e)
        {
            txtJsonLog.Clear();

            if (string.IsNullOrWhiteSpace(txtJsonInput.Text))
            {
                AppendLog("ERROR: No mod folder selected.", txtJsonLog);
                return;
            }

            string modFolder = txtJsonInput.Text.Trim();
            if (!Directory.Exists(modFolder))
            {
                AppendLog("ERROR: Mod folder does not exist: " + modFolder, txtJsonLog);
                return;
            }

            string outputName = GetSafeJsonOutputName(modFolder);
            if (txtJsonName != null && !string.IsNullOrWhiteSpace(txtJsonName.Text))
                outputName = MakeSafeFileName(txtJsonName.Text.Trim());

            string script = ToolkitPath("apps", "pbr json builder", "generate.py");
            string args = Q(modFolder) + " " + Q(outputName);

            SetJsonRunEnabled(false);
            AppendLog("Starting JSON generation...", txtJsonLog);
            AppendLog("Output name: " + outputName + ".json", txtJsonLog);
            AppendLog("", txtJsonLog);

            RunPython(script, args, txtJsonLog, exitCode =>
            {
                if (isClosing) return;

                if (exitCode == 0)
                    AppendLog("JSON generation completed.", txtJsonLog);
                else
                    AppendLog("JSON generation failed.", txtJsonLog);

                SetJsonRunEnabled(true);
            });
        }

        private string GetSafeJsonOutputName(string folderPath)
        {
            string name = new DirectoryInfo(folderPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)).Name;
            return MakeSafeFileName(name);
        }

        private string MakeSafeFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = "my_materials";

            foreach (char c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');

            return name;
        }

        private void SetJsonRunEnabled(bool enabled)
        {
            if (btnRunJson == null) return;
            if (btnRunJson.InvokeRequired)
            {
                btnRunJson.Invoke(new Action(() => SetJsonRunEnabled(enabled)));
                return;
            }

            btnRunJson.Enabled = enabled;
            btnBrowseJsonInput.Enabled = enabled;
        }

        // =========================
        // Logs tab
        // =========================

        private void btnClearGlobalLog_Click(object sender, EventArgs e)
        {
            if (txtGlobalLogBox != null)
                txtGlobalLogBox.Clear();
        }

        private void btnSaveGlobalLog_Click(object sender, EventArgs e)
        {
            if (txtGlobalLogBox == null || string.IsNullOrWhiteSpace(txtGlobalLogBox.Text))
            {
                MessageBox.Show("There is no log text to save.", "Skyking Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = "Save Log";
                dialog.Filter = "Text files (*.txt)|*.txt|Log files (*.log)|*.log|All files (*.*)|*.*";
                dialog.FileName = "Skyking Toolkit Log.txt";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(dialog.FileName, txtGlobalLogBox.Text);
                }
            }
        }

        // =========================
        // Shared Python runner
        // =========================

        private void RunPython(string scriptPath, string args, TextBox logBox)
        {
            RunPython(scriptPath, args, logBox, null);
        }

        private void RunPython(string scriptPath, string args, TextBox logBox, Action<int> onExit)
        {
            string pythonExe = ToolkitPath("runtime", "python-embed", "python.exe");
            string root = FindToolkitRoot();

            AppendLog("Python: " + pythonExe, logBox);
            AppendLog("Script: " + scriptPath, logBox);
            AppendLog("Args: " + args, logBox);
            AppendLog("", logBox);

            if (!File.Exists(pythonExe))
            {
                AppendLog("ERROR: Missing python.exe at " + pythonExe, logBox);
                onExit?.Invoke(-1);
                return;
            }

            if (!File.Exists(scriptPath))
            {
                AppendLog("ERROR: Missing script at " + scriptPath, logBox);
                onExit?.Invoke(-1);
                return;
            }

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = "-u " + Q(scriptPath) + " " + args,
                WorkingDirectory = root,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            psi.EnvironmentVariables["PYTHONUNBUFFERED"] = "1";

            Process process = new Process();
            process.StartInfo = psi;
            process.EnableRaisingEvents = true;

            process.OutputDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    HandleProcessOutput(e.Data, logBox);
            };

            process.ErrorDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    AppendLog("[ERROR] " + e.Data, logBox);
            };

            process.Exited += (s, e) =>
            {
                int exitCode = -1;
                try { exitCode = process.ExitCode; } catch { }

                RemoveRunningProcess(process);

                AppendLog("", logBox);
                AppendLog("Exit code: " + exitCode, logBox);

                if (exitCode == 0)
                {
                    if (logBox == txtParallaxLog && parallaxTotalSets > 0)
                        SetParallaxProgress(parallaxTotalSets, parallaxTotalSets);

                    if (logBox == txtSourceLog && sourceTotalSets > 0)
                        SetSourceProgress(sourceTotalSets, sourceTotalSets);

                    if (logBox == txtConvertLog && convertTotalSets > 0)
                        SetConvertProgress(convertTotalSets, convertTotalSets);
                }

                try { process.Dispose(); } catch { }

                if (!isClosing && onExit != null)
                    onExit(exitCode);
            };

            try
            {
                process.Start();
                AddRunningProcess(process);
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }
            catch (Exception ex)
            {
                RemoveRunningProcess(process);
                AppendLog("FAILED TO START PROCESS:", logBox);
                AppendLog(ex.Message, logBox);
                try { process.Dispose(); } catch { }
                if (onExit != null)
                    onExit(-1);
            }
        }

        private void AddRunningProcess(Process process)
        {
            lock (runningProcessesLock)
            {
                runningProcesses.Add(process);
            }
        }

        private void RemoveRunningProcess(Process process)
        {
            lock (runningProcessesLock)
            {
                runningProcesses.Remove(process);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isClosing = true;

            List<Process> processesToKill;
            lock (runningProcessesLock)
            {
                processesToKill = new List<Process>(runningProcesses);
                runningProcesses.Clear();
            }

            foreach (Process process in processesToKill)
            {
                try
                {
                    if (process != null && !process.HasExited)
                        process.Kill();
                }
                catch { }
            }
        }

        private void HandleProcessOutput(string line, TextBox logBox)
        {
            if (line.StartsWith("__SKYKING_TOTAL__="))
            {
                int total;
                if (int.TryParse(line.Substring("__SKYKING_TOTAL__=".Length), out total))
                {
                    if (logBox == txtSourceLog)
                        SetSourceProgressTotal(total);
                    else if (logBox == txtConvertLog)
                        SetConvertProgressTotal(total);
                    else
                        SetParallaxProgressTotal(total);
                }
                return;
            }

            if (line.StartsWith("__SKYKING_PROGRESS__="))
            {
                Match m = Regex.Match(line, @"__SKYKING_PROGRESS__=(\d+)/(\d+)");
                if (m.Success)
                {
                    int done = int.Parse(m.Groups[1].Value);
                    int total = int.Parse(m.Groups[2].Value);

                    if (logBox == txtSourceLog)
                        SetSourceProgress(done, total);
                    else if (logBox == txtConvertLog)
                        SetConvertProgress(done, total);
                    else
                        SetParallaxProgress(done, total);
                }
                return;
            }

            if (line.StartsWith("Processing ") && logBox == txtConvertLog)
                AdvanceConvertProgressOneStep();

            AppendLog(line, logBox);
        }

        // =========================
        // Progress helpers
        // =========================

        private void ResetParallaxProgress()
        {
            parallaxTotalSets = 0;
            parallaxDoneSets = 0;
            if (progressParallax != null)
            {
                progressParallax.Style = ProgressBarStyle.Blocks;
                progressParallax.Value = 0;
            }
        }

        private void SetParallaxProgressTotal(int total)
        {
            if (progressParallax == null) return;
            if (progressParallax.InvokeRequired)
            {
                progressParallax.Invoke(new Action(() => SetParallaxProgressTotal(total)));
                return;
            }

            parallaxTotalSets = Math.Max(total, 0);
            parallaxDoneSets = 0;
            progressParallax.Style = ProgressBarStyle.Blocks;
            progressParallax.Minimum = 0;
            progressParallax.Maximum = Math.Max(parallaxTotalSets, 1);
            progressParallax.Value = 0;
        }

        private void SetParallaxProgress(int done, int total)
        {
            if (progressParallax == null) return;
            if (progressParallax.InvokeRequired)
            {
                progressParallax.Invoke(new Action(() => SetParallaxProgress(done, total)));
                return;
            }

            parallaxTotalSets = Math.Max(total, 0);
            parallaxDoneSets = Math.Max(done, 0);
            progressParallax.Style = ProgressBarStyle.Blocks;
            progressParallax.Minimum = 0;
            progressParallax.Maximum = Math.Max(parallaxTotalSets, 1);
            progressParallax.Value = Math.Min(parallaxDoneSets, progressParallax.Maximum);
        }

        private void ResetSourceProgress()
        {
            sourceTotalSets = 0;
            sourceDoneSets = 0;
            if (progressSource != null)
            {
                progressSource.Style = ProgressBarStyle.Blocks;
                progressSource.Value = 0;
            }
        }

        private void SetSourceProgressBlocksOnly()
        {
            if (progressSource == null) return;
            if (progressSource.InvokeRequired)
            {
                progressSource.Invoke(new Action(SetSourceProgressBlocksOnly));
                return;
            }

            progressSource.Style = ProgressBarStyle.Blocks;
            progressSource.MarqueeAnimationSpeed = 0;
        }

        private void SetSourceProgressMarquee(bool enabled)
        {
            SetSourceProgressBlocksOnly();
        }

        private void SetSourceProgressTotal(int total)
        {
            if (progressSource == null) return;
            if (progressSource.InvokeRequired)
            {
                progressSource.Invoke(new Action(() => SetSourceProgressTotal(total)));
                return;
            }

            sourceTotalSets = Math.Max(total, 0);
            sourceDoneSets = 0;
            progressSource.Style = ProgressBarStyle.Blocks;
            progressSource.Minimum = 0;
            progressSource.Maximum = Math.Max(sourceTotalSets, 1);
            progressSource.Value = 0;
        }

        private void SetSourceProgress(int done, int total)
        {
            if (progressSource == null) return;
            if (progressSource.InvokeRequired)
            {
                progressSource.Invoke(new Action(() => SetSourceProgress(done, total)));
                return;
            }

            sourceTotalSets = Math.Max(total, 0);
            sourceDoneSets = Math.Max(done, 0);
            progressSource.Style = ProgressBarStyle.Blocks;
            progressSource.Minimum = 0;
            progressSource.Maximum = Math.Max(sourceTotalSets, 1);
            progressSource.Value = Math.Min(sourceDoneSets, progressSource.Maximum);
        }

        private void ResetConvertProgress()
        {
            convertTotalSets = 0;
            convertDoneSets = 0;
            if (ProgressConvert != null)
            {
                ProgressConvert.Style = ProgressBarStyle.Blocks;
                ProgressConvert.Minimum = 0;
                ProgressConvert.Maximum = 1;
                ProgressConvert.Value = 0;
            }
        }

        private void SetConvertProgressTotal(int total)
        {
            if (ProgressConvert == null) return;
            if (ProgressConvert.InvokeRequired)
            {
                ProgressConvert.Invoke(new Action(() => SetConvertProgressTotal(total)));
                return;
            }

            convertTotalSets = Math.Max(total, 0);
            convertDoneSets = 0;
            ProgressConvert.Style = ProgressBarStyle.Blocks;
            ProgressConvert.Minimum = 0;
            ProgressConvert.Maximum = Math.Max(convertTotalSets, 1);
            ProgressConvert.Value = 0;
        }

        private void SetConvertProgress(int done, int total)
        {
            if (ProgressConvert == null) return;
            if (ProgressConvert.InvokeRequired)
            {
                ProgressConvert.Invoke(new Action(() => SetConvertProgress(done, total)));
                return;
            }

            convertTotalSets = Math.Max(total, 0);
            convertDoneSets = Math.Max(done, 0);
            ProgressConvert.Style = ProgressBarStyle.Blocks;
            ProgressConvert.Minimum = 0;
            ProgressConvert.Maximum = Math.Max(convertTotalSets, 1);
            ProgressConvert.Value = Math.Min(convertDoneSets, ProgressConvert.Maximum);
        }

        private void AdvanceConvertProgressOneStep()
        {
            if (ProgressConvert == null) return;
            if (ProgressConvert.InvokeRequired)
            {
                ProgressConvert.Invoke(new Action(AdvanceConvertProgressOneStep));
                return;
            }

            ProgressConvert.Style = ProgressBarStyle.Blocks;

            if (convertTotalSets <= 0)
            {
                convertTotalSets = 1;
                ProgressConvert.Minimum = 0;
                ProgressConvert.Maximum = 1;
            }

            convertDoneSets = Math.Min(convertDoneSets + 1, Math.Max(convertTotalSets, 1));
            ProgressConvert.Value = Math.Min(convertDoneSets, ProgressConvert.Maximum);
        }

        private void SetSourceRunEnabled(bool enabled)
        {
            if (btnRunSource == null) return;
            if (btnRunSource.InvokeRequired)
            {
                btnRunSource.Invoke(new Action(() => SetSourceRunEnabled(enabled)));
                return;
            }

            btnRunSource.Enabled = enabled;
            btnBrowseSourceInput.Enabled = enabled;
            btnBrowseSourceOutput.Enabled = enabled;
        }

        private void SetConvertRunEnabled(bool enabled)
        {
            if (btnRunConvert == null) return;
            if (btnRunConvert.InvokeRequired)
            {
                btnRunConvert.Invoke(new Action(() => SetConvertRunEnabled(enabled)));
                return;
            }

            btnRunConvert.Enabled = enabled;
            btnBrowseConvertInput.Enabled = enabled;
            btnBrowseConvertOutput.Enabled = enabled;
        }

        // =========================
        // Logging
        // =========================

        private void AppendLog(string text)
        {
            AppendLog(text, txtParallaxLog);
        }

        private void AppendLog(string text, TextBox logBox)
        {
            if (isClosing) return;
            if (logBox == null) logBox = txtParallaxLog;
            if (logBox == null || logBox.IsDisposed) return;

            if (logBox.InvokeRequired)
            {
                try
                {
                    logBox.BeginInvoke(new Action(() => AppendLog(text, logBox)));
                }
                catch { }
                return;
            }

            logBox.AppendText(text + Environment.NewLine);
            logBox.SelectionStart = logBox.TextLength;
            logBox.ScrollToCaret();

            if (txtGlobalLogBox != null && txtGlobalLogBox != logBox && !txtGlobalLogBox.IsDisposed)
            {
                txtGlobalLogBox.AppendText(text + Environment.NewLine);
                txtGlobalLogBox.SelectionStart = txtGlobalLogBox.TextLength;
                txtGlobalLogBox.ScrollToCaret();
            }
        }

        // Empty designer-created event handlers are safe to leave for now.
        private void label1_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void radioButton1_CheckedChanged(object sender, EventArgs e) { }
        private void radioButton2_CheckedChanged(object sender, EventArgs e) { }
        private void radioButton3_CheckedChanged(object sender, EventArgs e) { }
        private void progressParallax_Click(object sender, EventArgs e) { }
        private void btnRunSource_Click_1(object sender, EventArgs e) { }
        private void tabJson_Click(object sender, EventArgs e) { }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
