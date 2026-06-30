namespace Skyking_Toolkit
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabWindow = new System.Windows.Forms.TabControl();
            this.tabParallax = new System.Windows.Forms.TabPage();
            this.lblParallaxLog = new System.Windows.Forms.Label();
            this.txtParallaxLog = new System.Windows.Forms.TextBox();
            this.btnBrowseParallaxOutput = new System.Windows.Forms.Button();
            this.txtParallaxOutput = new System.Windows.Forms.TextBox();
            this.btnBrowseParallaxSource = new System.Windows.Forms.Button();
            this.txtParallaxSource = new System.Windows.Forms.TextBox();
            this.btnRunParallax = new System.Windows.Forms.Button();
            this.progressParallax = new System.Windows.Forms.ProgressBar();
            this.lblParallaxType = new System.Windows.Forms.Label();
            this.rbParallaxBoth = new System.Windows.Forms.RadioButton();
            this.rbParallaxPBR = new System.Windows.Forms.RadioButton();
            this.rbParallaxComplex = new System.Windows.Forms.RadioButton();
            this.lblOutputFolder = new System.Windows.Forms.Label();
            this.lblParallaxSource = new System.Windows.Forms.Label();
            this.tabSource = new System.Windows.Forms.TabPage();
            this.grpSourceOptions = new System.Windows.Forms.GroupBox();
            this.txtSourceExplainBox = new System.Windows.Forms.TextBox();
            this.chkSourceFlipGreen = new System.Windows.Forms.CheckBox();
            this.rbSourceBoth = new System.Windows.Forms.RadioButton();
            this.rbSourceComplex = new System.Windows.Forms.RadioButton();
            this.rbSourcePBR = new System.Windows.Forms.RadioButton();
            this.btnRunSource = new System.Windows.Forms.Button();
            this.progressSource = new System.Windows.Forms.ProgressBar();
            this.lblSourceLog = new System.Windows.Forms.Label();
            this.txtSourceLog = new System.Windows.Forms.TextBox();
            this.btnBrowseSourceOutput = new System.Windows.Forms.Button();
            this.btnBrowseSourceInput = new System.Windows.Forms.Button();
            this.txtSourceOutput = new System.Windows.Forms.TextBox();
            this.lblSourceOutput = new System.Windows.Forms.Label();
            this.txtSourceInput = new System.Windows.Forms.TextBox();
            this.lblSourceInput = new System.Windows.Forms.Label();
            this.tabConvert = new System.Windows.Forms.TabPage();
            this.grpConvertMode = new System.Windows.Forms.GroupBox();
            this.rbConvertToCP = new System.Windows.Forms.RadioButton();
            this.rbConvertToPBR = new System.Windows.Forms.RadioButton();
            this.btnRunConvert = new System.Windows.Forms.Button();
            this.ProgressConvert = new System.Windows.Forms.ProgressBar();
            this.lblConvertLog = new System.Windows.Forms.Label();
            this.txtConvertLog = new System.Windows.Forms.TextBox();
            this.btnBrowseConvertOutput = new System.Windows.Forms.Button();
            this.btnBrowseConvertInput = new System.Windows.Forms.Button();
            this.txtConvertOutput = new System.Windows.Forms.TextBox();
            this.lblConvertOutput = new System.Windows.Forms.Label();
            this.txtConvertInput = new System.Windows.Forms.TextBox();
            this.lblConvertInput = new System.Windows.Forms.Label();
            this.tabJson = new System.Windows.Forms.TabPage();
            this.lblJasonLog = new System.Windows.Forms.Label();
            this.txtJsonLog = new System.Windows.Forms.TextBox();
            this.btnRunJson = new System.Windows.Forms.Button();
            this.txtJsonName = new System.Windows.Forms.TextBox();
            this.lblJsonName = new System.Windows.Forms.Label();
            this.btnBrowseJsonInput = new System.Windows.Forms.Button();
            this.txtJsonInput = new System.Windows.Forms.TextBox();
            this.lblJsonInput = new System.Windows.Forms.Label();
            this.tabLogs = new System.Windows.Forms.TabPage();
            this.btnSaveGlobalLog = new System.Windows.Forms.Button();
            this.btnClearGlobalLog = new System.Windows.Forms.Button();
            this.lblGlobalLog = new System.Windows.Forms.Label();
            this.txtGlobalLogBox = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.tabWindow.SuspendLayout();
            this.tabParallax.SuspendLayout();
            this.tabSource.SuspendLayout();
            this.grpSourceOptions.SuspendLayout();
            this.tabConvert.SuspendLayout();
            this.grpConvertMode.SuspendLayout();
            this.tabJson.SuspendLayout();
            this.tabLogs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // tabWindow
            // 
            this.tabWindow.Controls.Add(this.tabParallax);
            this.tabWindow.Controls.Add(this.tabSource);
            this.tabWindow.Controls.Add(this.tabConvert);
            this.tabWindow.Controls.Add(this.tabJson);
            this.tabWindow.Controls.Add(this.tabLogs);
            this.tabWindow.ItemSize = new System.Drawing.Size(50, 30);
            this.tabWindow.Location = new System.Drawing.Point(32, 67);
            this.tabWindow.Name = "tabWindow";
            this.tabWindow.SelectedIndex = 0;
            this.tabWindow.Size = new System.Drawing.Size(668, 382);
            this.tabWindow.TabIndex = 0;
            // 
            // tabParallax
            // 
            this.tabParallax.BackColor = System.Drawing.Color.Silver;
            this.tabParallax.Controls.Add(this.lblParallaxLog);
            this.tabParallax.Controls.Add(this.txtParallaxLog);
            this.tabParallax.Controls.Add(this.btnBrowseParallaxOutput);
            this.tabParallax.Controls.Add(this.txtParallaxOutput);
            this.tabParallax.Controls.Add(this.btnBrowseParallaxSource);
            this.tabParallax.Controls.Add(this.txtParallaxSource);
            this.tabParallax.Controls.Add(this.btnRunParallax);
            this.tabParallax.Controls.Add(this.progressParallax);
            this.tabParallax.Controls.Add(this.lblParallaxType);
            this.tabParallax.Controls.Add(this.rbParallaxBoth);
            this.tabParallax.Controls.Add(this.rbParallaxPBR);
            this.tabParallax.Controls.Add(this.rbParallaxComplex);
            this.tabParallax.Controls.Add(this.lblOutputFolder);
            this.tabParallax.Controls.Add(this.lblParallaxSource);
            this.tabParallax.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tabParallax.Location = new System.Drawing.Point(4, 34);
            this.tabParallax.Name = "tabParallax";
            this.tabParallax.Padding = new System.Windows.Forms.Padding(3);
            this.tabParallax.Size = new System.Drawing.Size(660, 344);
            this.tabParallax.TabIndex = 1;
            this.tabParallax.Text = "Parallax Old Mods";
            // 
            // lblParallaxLog
            // 
            this.lblParallaxLog.AutoSize = true;
            this.lblParallaxLog.BackColor = System.Drawing.Color.Transparent;
            this.lblParallaxLog.ForeColor = System.Drawing.SystemColors.MenuText;
            this.lblParallaxLog.Location = new System.Drawing.Point(312, 13);
            this.lblParallaxLog.Name = "lblParallaxLog";
            this.lblParallaxLog.Size = new System.Drawing.Size(25, 13);
            this.lblParallaxLog.TabIndex = 13;
            this.lblParallaxLog.Text = "Log";
            // 
            // txtParallaxLog
            // 
            this.txtParallaxLog.BackColor = System.Drawing.Color.DarkGray;
            this.txtParallaxLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtParallaxLog.Location = new System.Drawing.Point(315, 29);
            this.txtParallaxLog.Multiline = true;
            this.txtParallaxLog.Name = "txtParallaxLog";
            this.txtParallaxLog.ReadOnly = true;
            this.txtParallaxLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtParallaxLog.Size = new System.Drawing.Size(336, 274);
            this.txtParallaxLog.TabIndex = 12;
            this.txtParallaxLog.WordWrap = false;
            // 
            // btnBrowseParallaxOutput
            // 
            this.btnBrowseParallaxOutput.BackColor = System.Drawing.Color.DimGray;
            this.btnBrowseParallaxOutput.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBrowseParallaxOutput.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnBrowseParallaxOutput.Location = new System.Drawing.Point(9, 138);
            this.btnBrowseParallaxOutput.Name = "btnBrowseParallaxOutput";
            this.btnBrowseParallaxOutput.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseParallaxOutput.TabIndex = 11;
            this.btnBrowseParallaxOutput.Text = "Browse";
            this.btnBrowseParallaxOutput.UseVisualStyleBackColor = false;
            this.btnBrowseParallaxOutput.Click += new System.EventHandler(this.btnBrowseParallaxOutput_Click);
            // 
            // txtParallaxOutput
            // 
            this.txtParallaxOutput.BackColor = System.Drawing.Color.DarkGray;
            this.txtParallaxOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtParallaxOutput.Location = new System.Drawing.Point(9, 112);
            this.txtParallaxOutput.Name = "txtParallaxOutput";
            this.txtParallaxOutput.Size = new System.Drawing.Size(300, 20);
            this.txtParallaxOutput.TabIndex = 10;
            // 
            // btnBrowseParallaxSource
            // 
            this.btnBrowseParallaxSource.BackColor = System.Drawing.Color.DimGray;
            this.btnBrowseParallaxSource.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBrowseParallaxSource.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnBrowseParallaxSource.Location = new System.Drawing.Point(9, 55);
            this.btnBrowseParallaxSource.Name = "btnBrowseParallaxSource";
            this.btnBrowseParallaxSource.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseParallaxSource.TabIndex = 9;
            this.btnBrowseParallaxSource.Text = "Browse";
            this.btnBrowseParallaxSource.UseVisualStyleBackColor = false;
            this.btnBrowseParallaxSource.Click += new System.EventHandler(this.btnBrowseParallaxSource_Click);
            // 
            // txtParallaxSource
            // 
            this.txtParallaxSource.BackColor = System.Drawing.Color.DarkGray;
            this.txtParallaxSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtParallaxSource.Location = new System.Drawing.Point(9, 29);
            this.txtParallaxSource.Name = "txtParallaxSource";
            this.txtParallaxSource.Size = new System.Drawing.Size(300, 20);
            this.txtParallaxSource.TabIndex = 8;
            // 
            // btnRunParallax
            // 
            this.btnRunParallax.BackColor = System.Drawing.Color.DimGray;
            this.btnRunParallax.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnRunParallax.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnRunParallax.Location = new System.Drawing.Point(9, 273);
            this.btnRunParallax.Name = "btnRunParallax";
            this.btnRunParallax.Size = new System.Drawing.Size(75, 30);
            this.btnRunParallax.TabIndex = 7;
            this.btnRunParallax.Text = "Generate";
            this.btnRunParallax.UseVisualStyleBackColor = false;
            this.btnRunParallax.Click += new System.EventHandler(this.btnRunParallax_Click);
            // 
            // progressParallax
            // 
            this.progressParallax.Location = new System.Drawing.Point(9, 315);
            this.progressParallax.Name = "progressParallax";
            this.progressParallax.Size = new System.Drawing.Size(642, 23);
            this.progressParallax.TabIndex = 6;
            this.progressParallax.Click += new System.EventHandler(this.progressParallax_Click);
            // 
            // lblParallaxType
            // 
            this.lblParallaxType.AutoSize = true;
            this.lblParallaxType.BackColor = System.Drawing.Color.Transparent;
            this.lblParallaxType.ForeColor = System.Drawing.SystemColors.MenuText;
            this.lblParallaxType.Location = new System.Drawing.Point(6, 178);
            this.lblParallaxType.Name = "lblParallaxType";
            this.lblParallaxType.Size = new System.Drawing.Size(71, 13);
            this.lblParallaxType.TabIndex = 5;
            this.lblParallaxType.Text = "Parallax Type";
            this.lblParallaxType.Click += new System.EventHandler(this.label3_Click);
            // 
            // rbParallaxBoth
            // 
            this.rbParallaxBoth.AutoSize = true;
            this.rbParallaxBoth.BackColor = System.Drawing.Color.Transparent;
            this.rbParallaxBoth.ForeColor = System.Drawing.SystemColors.InfoText;
            this.rbParallaxBoth.Location = new System.Drawing.Point(9, 240);
            this.rbParallaxBoth.Name = "rbParallaxBoth";
            this.rbParallaxBoth.Size = new System.Drawing.Size(47, 17);
            this.rbParallaxBoth.TabIndex = 4;
            this.rbParallaxBoth.Text = "Both";
            this.rbParallaxBoth.UseVisualStyleBackColor = false;
            this.rbParallaxBoth.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // rbParallaxPBR
            // 
            this.rbParallaxPBR.AutoSize = true;
            this.rbParallaxPBR.BackColor = System.Drawing.Color.Transparent;
            this.rbParallaxPBR.ForeColor = System.Drawing.SystemColors.InfoText;
            this.rbParallaxPBR.Location = new System.Drawing.Point(9, 217);
            this.rbParallaxPBR.Name = "rbParallaxPBR";
            this.rbParallaxPBR.Size = new System.Drawing.Size(47, 17);
            this.rbParallaxPBR.TabIndex = 3;
            this.rbParallaxPBR.Text = "PBR";
            this.rbParallaxPBR.UseVisualStyleBackColor = false;
            this.rbParallaxPBR.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // rbParallaxComplex
            // 
            this.rbParallaxComplex.AutoSize = true;
            this.rbParallaxComplex.BackColor = System.Drawing.Color.Transparent;
            this.rbParallaxComplex.Checked = true;
            this.rbParallaxComplex.ForeColor = System.Drawing.SystemColors.InfoText;
            this.rbParallaxComplex.Location = new System.Drawing.Point(9, 194);
            this.rbParallaxComplex.Name = "rbParallaxComplex";
            this.rbParallaxComplex.Size = new System.Drawing.Size(105, 17);
            this.rbParallaxComplex.TabIndex = 2;
            this.rbParallaxComplex.TabStop = true;
            this.rbParallaxComplex.Text = "Complex Parallax";
            this.rbParallaxComplex.UseVisualStyleBackColor = false;
            this.rbParallaxComplex.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // lblOutputFolder
            // 
            this.lblOutputFolder.AutoSize = true;
            this.lblOutputFolder.BackColor = System.Drawing.Color.Transparent;
            this.lblOutputFolder.ForeColor = System.Drawing.SystemColors.MenuText;
            this.lblOutputFolder.Location = new System.Drawing.Point(6, 96);
            this.lblOutputFolder.Name = "lblOutputFolder";
            this.lblOutputFolder.Size = new System.Drawing.Size(71, 13);
            this.lblOutputFolder.TabIndex = 1;
            this.lblOutputFolder.Text = "Output Folder";
            // 
            // lblParallaxSource
            // 
            this.lblParallaxSource.AutoSize = true;
            this.lblParallaxSource.BackColor = System.Drawing.Color.Transparent;
            this.lblParallaxSource.ForeColor = System.Drawing.SystemColors.MenuText;
            this.lblParallaxSource.Location = new System.Drawing.Point(6, 13);
            this.lblParallaxSource.Name = "lblParallaxSource";
            this.lblParallaxSource.Size = new System.Drawing.Size(73, 13);
            this.lblParallaxSource.TabIndex = 0;
            this.lblParallaxSource.Text = "Source Folder";
            this.lblParallaxSource.Click += new System.EventHandler(this.label1_Click);
            // 
            // tabSource
            // 
            this.tabSource.BackColor = System.Drawing.Color.Silver;
            this.tabSource.Controls.Add(this.grpSourceOptions);
            this.tabSource.Controls.Add(this.btnRunSource);
            this.tabSource.Controls.Add(this.progressSource);
            this.tabSource.Controls.Add(this.lblSourceLog);
            this.tabSource.Controls.Add(this.txtSourceLog);
            this.tabSource.Controls.Add(this.btnBrowseSourceOutput);
            this.tabSource.Controls.Add(this.btnBrowseSourceInput);
            this.tabSource.Controls.Add(this.txtSourceOutput);
            this.tabSource.Controls.Add(this.lblSourceOutput);
            this.tabSource.Controls.Add(this.txtSourceInput);
            this.tabSource.Controls.Add(this.lblSourceInput);
            this.tabSource.Location = new System.Drawing.Point(4, 34);
            this.tabSource.Name = "tabSource";
            this.tabSource.Padding = new System.Windows.Forms.Padding(3);
            this.tabSource.Size = new System.Drawing.Size(660, 344);
            this.tabSource.TabIndex = 2;
            this.tabSource.Text = "Build From Source";
            // 
            // grpSourceOptions
            // 
            this.grpSourceOptions.Controls.Add(this.txtSourceExplainBox);
            this.grpSourceOptions.Controls.Add(this.chkSourceFlipGreen);
            this.grpSourceOptions.Controls.Add(this.rbSourceBoth);
            this.grpSourceOptions.Controls.Add(this.rbSourceComplex);
            this.grpSourceOptions.Controls.Add(this.rbSourcePBR);
            this.grpSourceOptions.Location = new System.Drawing.Point(9, 168);
            this.grpSourceOptions.Name = "grpSourceOptions";
            this.grpSourceOptions.Size = new System.Drawing.Size(230, 95);
            this.grpSourceOptions.TabIndex = 22;
            this.grpSourceOptions.TabStop = false;
            this.grpSourceOptions.Text = "Output Options";
            // 
            // txtSourceExplainBox
            // 
            this.txtSourceExplainBox.BackColor = System.Drawing.Color.Silver;
            this.txtSourceExplainBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSourceExplainBox.Location = new System.Drawing.Point(108, 43);
            this.txtSourceExplainBox.Multiline = true;
            this.txtSourceExplainBox.Name = "txtSourceExplainBox";
            this.txtSourceExplainBox.ReadOnly = true;
            this.txtSourceExplainBox.Size = new System.Drawing.Size(116, 40);
            this.txtSourceExplainBox.TabIndex = 5;
            this.txtSourceExplainBox.Text = "This will flip the normal\'s green channel";
            // 
            // chkSourceFlipGreen
            // 
            this.chkSourceFlipGreen.AutoSize = true;
            this.chkSourceFlipGreen.Checked = true;
            this.chkSourceFlipGreen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSourceFlipGreen.Location = new System.Drawing.Point(108, 20);
            this.chkSourceFlipGreen.Name = "chkSourceFlipGreen";
            this.chkSourceFlipGreen.Size = new System.Drawing.Size(116, 17);
            this.chkSourceFlipGreen.TabIndex = 4;
            this.chkSourceFlipGreen.Text = "Flip Green Channel";
            this.chkSourceFlipGreen.UseVisualStyleBackColor = true;
            // 
            // rbSourceBoth
            // 
            this.rbSourceBoth.AutoSize = true;
            this.rbSourceBoth.Location = new System.Drawing.Point(6, 65);
            this.rbSourceBoth.Name = "rbSourceBoth";
            this.rbSourceBoth.Size = new System.Drawing.Size(47, 17);
            this.rbSourceBoth.TabIndex = 3;
            this.rbSourceBoth.TabStop = true;
            this.rbSourceBoth.Text = "Both";
            this.rbSourceBoth.UseVisualStyleBackColor = true;
            // 
            // rbSourceComplex
            // 
            this.rbSourceComplex.AutoSize = true;
            this.rbSourceComplex.Location = new System.Drawing.Point(6, 42);
            this.rbSourceComplex.Name = "rbSourceComplex";
            this.rbSourceComplex.Size = new System.Drawing.Size(105, 17);
            this.rbSourceComplex.TabIndex = 2;
            this.rbSourceComplex.TabStop = true;
            this.rbSourceComplex.Text = "Complex Parallax";
            this.rbSourceComplex.UseVisualStyleBackColor = true;
            // 
            // rbSourcePBR
            // 
            this.rbSourcePBR.AutoSize = true;
            this.rbSourcePBR.Location = new System.Drawing.Point(6, 19);
            this.rbSourcePBR.Name = "rbSourcePBR";
            this.rbSourcePBR.Size = new System.Drawing.Size(47, 17);
            this.rbSourcePBR.TabIndex = 1;
            this.rbSourcePBR.TabStop = true;
            this.rbSourcePBR.Text = "PBR";
            this.rbSourcePBR.UseVisualStyleBackColor = true;
            // 
            // btnRunSource
            // 
            this.btnRunSource.Location = new System.Drawing.Point(9, 269);
            this.btnRunSource.Name = "btnRunSource";
            this.btnRunSource.Size = new System.Drawing.Size(75, 30);
            this.btnRunSource.TabIndex = 17;
            this.btnRunSource.Text = "Generate";
            this.btnRunSource.UseVisualStyleBackColor = true;
            this.btnRunSource.Click += new System.EventHandler(this.btnRunSource_Click);
            // 
            // progressSource
            // 
            this.progressSource.Location = new System.Drawing.Point(9, 315);
            this.progressSource.Name = "progressSource";
            this.progressSource.Size = new System.Drawing.Size(642, 23);
            this.progressSource.TabIndex = 16;
            // 
            // lblSourceLog
            // 
            this.lblSourceLog.AutoSize = true;
            this.lblSourceLog.Location = new System.Drawing.Point(312, 9);
            this.lblSourceLog.Name = "lblSourceLog";
            this.lblSourceLog.Size = new System.Drawing.Size(25, 13);
            this.lblSourceLog.TabIndex = 15;
            this.lblSourceLog.Text = "Log";
            // 
            // txtSourceLog
            // 
            this.txtSourceLog.BackColor = System.Drawing.Color.DarkGray;
            this.txtSourceLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSourceLog.Location = new System.Drawing.Point(315, 25);
            this.txtSourceLog.Multiline = true;
            this.txtSourceLog.Name = "txtSourceLog";
            this.txtSourceLog.ReadOnly = true;
            this.txtSourceLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSourceLog.Size = new System.Drawing.Size(336, 274);
            this.txtSourceLog.TabIndex = 14;
            this.txtSourceLog.WordWrap = false;
            // 
            // btnBrowseSourceOutput
            // 
            this.btnBrowseSourceOutput.Location = new System.Drawing.Point(6, 130);
            this.btnBrowseSourceOutput.Name = "btnBrowseSourceOutput";
            this.btnBrowseSourceOutput.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseSourceOutput.TabIndex = 11;
            this.btnBrowseSourceOutput.Text = "Browse";
            this.btnBrowseSourceOutput.UseVisualStyleBackColor = true;
            // 
            // btnBrowseSourceInput
            // 
            this.btnBrowseSourceInput.Location = new System.Drawing.Point(6, 51);
            this.btnBrowseSourceInput.Name = "btnBrowseSourceInput";
            this.btnBrowseSourceInput.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseSourceInput.TabIndex = 10;
            this.btnBrowseSourceInput.Text = "Browse";
            this.btnBrowseSourceInput.UseVisualStyleBackColor = true;
            // 
            // txtSourceOutput
            // 
            this.txtSourceOutput.BackColor = System.Drawing.Color.DarkGray;
            this.txtSourceOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSourceOutput.Location = new System.Drawing.Point(6, 104);
            this.txtSourceOutput.Name = "txtSourceOutput";
            this.txtSourceOutput.Size = new System.Drawing.Size(300, 20);
            this.txtSourceOutput.TabIndex = 9;
            // 
            // lblSourceOutput
            // 
            this.lblSourceOutput.AutoSize = true;
            this.lblSourceOutput.Location = new System.Drawing.Point(6, 88);
            this.lblSourceOutput.Name = "lblSourceOutput";
            this.lblSourceOutput.Size = new System.Drawing.Size(71, 13);
            this.lblSourceOutput.TabIndex = 8;
            this.lblSourceOutput.Text = "Output Folder";
            // 
            // txtSourceInput
            // 
            this.txtSourceInput.BackColor = System.Drawing.Color.DarkGray;
            this.txtSourceInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSourceInput.Location = new System.Drawing.Point(6, 25);
            this.txtSourceInput.Name = "txtSourceInput";
            this.txtSourceInput.Size = new System.Drawing.Size(300, 20);
            this.txtSourceInput.TabIndex = 7;
            // 
            // lblSourceInput
            // 
            this.lblSourceInput.AutoSize = true;
            this.lblSourceInput.Location = new System.Drawing.Point(6, 9);
            this.lblSourceInput.Name = "lblSourceInput";
            this.lblSourceInput.Size = new System.Drawing.Size(73, 13);
            this.lblSourceInput.TabIndex = 6;
            this.lblSourceInput.Text = "Source Folder";
            // 
            // tabConvert
            // 
            this.tabConvert.BackColor = System.Drawing.Color.Silver;
            this.tabConvert.Controls.Add(this.grpConvertMode);
            this.tabConvert.Controls.Add(this.btnRunConvert);
            this.tabConvert.Controls.Add(this.ProgressConvert);
            this.tabConvert.Controls.Add(this.lblConvertLog);
            this.tabConvert.Controls.Add(this.txtConvertLog);
            this.tabConvert.Controls.Add(this.btnBrowseConvertOutput);
            this.tabConvert.Controls.Add(this.btnBrowseConvertInput);
            this.tabConvert.Controls.Add(this.txtConvertOutput);
            this.tabConvert.Controls.Add(this.lblConvertOutput);
            this.tabConvert.Controls.Add(this.txtConvertInput);
            this.tabConvert.Controls.Add(this.lblConvertInput);
            this.tabConvert.Location = new System.Drawing.Point(4, 34);
            this.tabConvert.Name = "tabConvert";
            this.tabConvert.Padding = new System.Windows.Forms.Padding(3);
            this.tabConvert.Size = new System.Drawing.Size(660, 344);
            this.tabConvert.TabIndex = 3;
            this.tabConvert.Text = "PBR ⇆ Complex Parallax";
            // 
            // grpConvertMode
            // 
            this.grpConvertMode.Controls.Add(this.rbConvertToCP);
            this.grpConvertMode.Controls.Add(this.rbConvertToPBR);
            this.grpConvertMode.Location = new System.Drawing.Point(11, 172);
            this.grpConvertMode.Name = "grpConvertMode";
            this.grpConvertMode.Size = new System.Drawing.Size(230, 65);
            this.grpConvertMode.TabIndex = 33;
            this.grpConvertMode.TabStop = false;
            this.grpConvertMode.Text = "Output Options";
            // 
            // rbConvertToCP
            // 
            this.rbConvertToCP.AutoSize = true;
            this.rbConvertToCP.Location = new System.Drawing.Point(6, 42);
            this.rbConvertToCP.Name = "rbConvertToCP";
            this.rbConvertToCP.Size = new System.Drawing.Size(144, 17);
            this.rbConvertToCP.TabIndex = 2;
            this.rbConvertToCP.TabStop = true;
            this.rbConvertToCP.Text = "Complex Parallax → PBR";
            this.rbConvertToCP.UseVisualStyleBackColor = true;
            // 
            // rbConvertToPBR
            // 
            this.rbConvertToPBR.AutoSize = true;
            this.rbConvertToPBR.Location = new System.Drawing.Point(6, 19);
            this.rbConvertToPBR.Name = "rbConvertToPBR";
            this.rbConvertToPBR.Size = new System.Drawing.Size(144, 17);
            this.rbConvertToPBR.TabIndex = 1;
            this.rbConvertToPBR.TabStop = true;
            this.rbConvertToPBR.Text = "PBR → Complex Parallax";
            this.rbConvertToPBR.UseVisualStyleBackColor = true;
            // 
            // btnRunConvert
            // 
            this.btnRunConvert.Location = new System.Drawing.Point(11, 273);
            this.btnRunConvert.Name = "btnRunConvert";
            this.btnRunConvert.Size = new System.Drawing.Size(75, 30);
            this.btnRunConvert.TabIndex = 32;
            this.btnRunConvert.Text = "Convert";
            this.btnRunConvert.UseVisualStyleBackColor = true;
            // 
            // ProgressConvert
            // 
            this.ProgressConvert.Location = new System.Drawing.Point(11, 315);
            this.ProgressConvert.Name = "ProgressConvert";
            this.ProgressConvert.Size = new System.Drawing.Size(642, 23);
            this.ProgressConvert.TabIndex = 31;
            // 
            // lblConvertLog
            // 
            this.lblConvertLog.AutoSize = true;
            this.lblConvertLog.Location = new System.Drawing.Point(314, 13);
            this.lblConvertLog.Name = "lblConvertLog";
            this.lblConvertLog.Size = new System.Drawing.Size(25, 13);
            this.lblConvertLog.TabIndex = 30;
            this.lblConvertLog.Text = "Log";
            // 
            // txtConvertLog
            // 
            this.txtConvertLog.BackColor = System.Drawing.Color.DarkGray;
            this.txtConvertLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConvertLog.Location = new System.Drawing.Point(317, 29);
            this.txtConvertLog.Multiline = true;
            this.txtConvertLog.Name = "txtConvertLog";
            this.txtConvertLog.ReadOnly = true;
            this.txtConvertLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtConvertLog.Size = new System.Drawing.Size(336, 274);
            this.txtConvertLog.TabIndex = 29;
            this.txtConvertLog.WordWrap = false;
            // 
            // btnBrowseConvertOutput
            // 
            this.btnBrowseConvertOutput.Location = new System.Drawing.Point(8, 134);
            this.btnBrowseConvertOutput.Name = "btnBrowseConvertOutput";
            this.btnBrowseConvertOutput.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseConvertOutput.TabIndex = 28;
            this.btnBrowseConvertOutput.Text = "Browse";
            this.btnBrowseConvertOutput.UseVisualStyleBackColor = true;
            // 
            // btnBrowseConvertInput
            // 
            this.btnBrowseConvertInput.Location = new System.Drawing.Point(8, 55);
            this.btnBrowseConvertInput.Name = "btnBrowseConvertInput";
            this.btnBrowseConvertInput.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseConvertInput.TabIndex = 27;
            this.btnBrowseConvertInput.Text = "Browse";
            this.btnBrowseConvertInput.UseVisualStyleBackColor = true;
            // 
            // txtConvertOutput
            // 
            this.txtConvertOutput.BackColor = System.Drawing.Color.DarkGray;
            this.txtConvertOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConvertOutput.Location = new System.Drawing.Point(8, 108);
            this.txtConvertOutput.Name = "txtConvertOutput";
            this.txtConvertOutput.Size = new System.Drawing.Size(300, 20);
            this.txtConvertOutput.TabIndex = 26;
            // 
            // lblConvertOutput
            // 
            this.lblConvertOutput.AutoSize = true;
            this.lblConvertOutput.Location = new System.Drawing.Point(8, 92);
            this.lblConvertOutput.Name = "lblConvertOutput";
            this.lblConvertOutput.Size = new System.Drawing.Size(71, 13);
            this.lblConvertOutput.TabIndex = 25;
            this.lblConvertOutput.Text = "Output Folder";
            // 
            // txtConvertInput
            // 
            this.txtConvertInput.BackColor = System.Drawing.Color.DarkGray;
            this.txtConvertInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConvertInput.Location = new System.Drawing.Point(8, 29);
            this.txtConvertInput.Name = "txtConvertInput";
            this.txtConvertInput.Size = new System.Drawing.Size(300, 20);
            this.txtConvertInput.TabIndex = 24;
            // 
            // lblConvertInput
            // 
            this.lblConvertInput.AutoSize = true;
            this.lblConvertInput.Location = new System.Drawing.Point(8, 13);
            this.lblConvertInput.Name = "lblConvertInput";
            this.lblConvertInput.Size = new System.Drawing.Size(73, 13);
            this.lblConvertInput.TabIndex = 23;
            this.lblConvertInput.Text = "Source Folder";
            // 
            // tabJson
            // 
            this.tabJson.BackColor = System.Drawing.Color.Silver;
            this.tabJson.Controls.Add(this.lblJasonLog);
            this.tabJson.Controls.Add(this.txtJsonLog);
            this.tabJson.Controls.Add(this.btnRunJson);
            this.tabJson.Controls.Add(this.txtJsonName);
            this.tabJson.Controls.Add(this.lblJsonName);
            this.tabJson.Controls.Add(this.btnBrowseJsonInput);
            this.tabJson.Controls.Add(this.txtJsonInput);
            this.tabJson.Controls.Add(this.lblJsonInput);
            this.tabJson.Location = new System.Drawing.Point(4, 34);
            this.tabJson.Name = "tabJson";
            this.tabJson.Padding = new System.Windows.Forms.Padding(3);
            this.tabJson.Size = new System.Drawing.Size(660, 344);
            this.tabJson.TabIndex = 4;
            this.tabJson.Text = "Json Generator";
            this.tabJson.Click += new System.EventHandler(this.tabJson_Click);
            // 
            // lblJasonLog
            // 
            this.lblJasonLog.AutoSize = true;
            this.lblJasonLog.Location = new System.Drawing.Point(312, 16);
            this.lblJasonLog.Name = "lblJasonLog";
            this.lblJasonLog.Size = new System.Drawing.Size(25, 13);
            this.lblJasonLog.TabIndex = 17;
            this.lblJasonLog.Text = "Log";
            // 
            // txtJsonLog
            // 
            this.txtJsonLog.BackColor = System.Drawing.Color.DarkGray;
            this.txtJsonLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJsonLog.Location = new System.Drawing.Point(315, 32);
            this.txtJsonLog.Multiline = true;
            this.txtJsonLog.Name = "txtJsonLog";
            this.txtJsonLog.ReadOnly = true;
            this.txtJsonLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtJsonLog.Size = new System.Drawing.Size(336, 306);
            this.txtJsonLog.TabIndex = 16;
            this.txtJsonLog.WordWrap = false;
            // 
            // btnRunJson
            // 
            this.btnRunJson.Location = new System.Drawing.Point(9, 158);
            this.btnRunJson.Name = "btnRunJson";
            this.btnRunJson.Size = new System.Drawing.Size(75, 23);
            this.btnRunJson.TabIndex = 14;
            this.btnRunJson.Text = "Generate";
            this.btnRunJson.UseVisualStyleBackColor = true;
            // 
            // txtJsonName
            // 
            this.txtJsonName.BackColor = System.Drawing.Color.DarkGray;
            this.txtJsonName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJsonName.Location = new System.Drawing.Point(9, 118);
            this.txtJsonName.Name = "txtJsonName";
            this.txtJsonName.Size = new System.Drawing.Size(300, 20);
            this.txtJsonName.TabIndex = 19;
            this.txtJsonName.Text = "my_materials";
            // 
            // lblJsonName
            // 
            this.lblJsonName.AutoSize = true;
            this.lblJsonName.Location = new System.Drawing.Point(6, 102);
            this.lblJsonName.Name = "lblJsonName";
            this.lblJsonName.Size = new System.Drawing.Size(66, 13);
            this.lblJsonName.TabIndex = 18;
            this.lblJsonName.Text = "JSON Name";
            // 
            // btnBrowseJsonInput
            // 
            this.btnBrowseJsonInput.Location = new System.Drawing.Point(9, 58);
            this.btnBrowseJsonInput.Name = "btnBrowseJsonInput";
            this.btnBrowseJsonInput.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseJsonInput.TabIndex = 13;
            this.btnBrowseJsonInput.Text = "Browse";
            this.btnBrowseJsonInput.UseVisualStyleBackColor = true;
            this.btnBrowseJsonInput.Click += new System.EventHandler(this.btnBrowseJsonInput_Click);
            // 
            // txtJsonInput
            // 
            this.txtJsonInput.BackColor = System.Drawing.Color.DarkGray;
            this.txtJsonInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJsonInput.Location = new System.Drawing.Point(9, 32);
            this.txtJsonInput.Name = "txtJsonInput";
            this.txtJsonInput.Size = new System.Drawing.Size(300, 20);
            this.txtJsonInput.TabIndex = 12;
            // 
            // lblJsonInput
            // 
            this.lblJsonInput.AutoSize = true;
            this.lblJsonInput.Location = new System.Drawing.Point(6, 16);
            this.lblJsonInput.Name = "lblJsonInput";
            this.lblJsonInput.Size = new System.Drawing.Size(60, 13);
            this.lblJsonInput.TabIndex = 11;
            this.lblJsonInput.Text = "Mod Folder";
            // 
            // tabLogs
            // 
            this.tabLogs.BackColor = System.Drawing.Color.Silver;
            this.tabLogs.Controls.Add(this.btnSaveGlobalLog);
            this.tabLogs.Controls.Add(this.btnClearGlobalLog);
            this.tabLogs.Controls.Add(this.lblGlobalLog);
            this.tabLogs.Controls.Add(this.txtGlobalLogBox);
            this.tabLogs.Location = new System.Drawing.Point(4, 34);
            this.tabLogs.Name = "tabLogs";
            this.tabLogs.Padding = new System.Windows.Forms.Padding(3);
            this.tabLogs.Size = new System.Drawing.Size(660, 344);
            this.tabLogs.TabIndex = 5;
            this.tabLogs.Text = "Logs";
            // 
            // btnSaveGlobalLog
            // 
            this.btnSaveGlobalLog.Location = new System.Drawing.Point(90, 315);
            this.btnSaveGlobalLog.Name = "btnSaveGlobalLog";
            this.btnSaveGlobalLog.Size = new System.Drawing.Size(75, 23);
            this.btnSaveGlobalLog.TabIndex = 21;
            this.btnSaveGlobalLog.Text = "Save Log";
            this.btnSaveGlobalLog.UseVisualStyleBackColor = true;
            // 
            // btnClearGlobalLog
            // 
            this.btnClearGlobalLog.Location = new System.Drawing.Point(9, 315);
            this.btnClearGlobalLog.Name = "btnClearGlobalLog";
            this.btnClearGlobalLog.Size = new System.Drawing.Size(75, 23);
            this.btnClearGlobalLog.TabIndex = 20;
            this.btnClearGlobalLog.Text = "Clear Log";
            this.btnClearGlobalLog.UseVisualStyleBackColor = true;
            // 
            // lblGlobalLog
            // 
            this.lblGlobalLog.AutoSize = true;
            this.lblGlobalLog.Location = new System.Drawing.Point(6, 17);
            this.lblGlobalLog.Name = "lblGlobalLog";
            this.lblGlobalLog.Size = new System.Drawing.Size(25, 13);
            this.lblGlobalLog.TabIndex = 19;
            this.lblGlobalLog.Text = "Log";
            // 
            // txtGlobalLogBox
            // 
            this.txtGlobalLogBox.BackColor = System.Drawing.Color.DarkGray;
            this.txtGlobalLogBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGlobalLogBox.Location = new System.Drawing.Point(9, 33);
            this.txtGlobalLogBox.Multiline = true;
            this.txtGlobalLogBox.Name = "txtGlobalLogBox";
            this.txtGlobalLogBox.ReadOnly = true;
            this.txtGlobalLogBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtGlobalLogBox.Size = new System.Drawing.Size(645, 276);
            this.txtGlobalLogBox.TabIndex = 18;
            this.txtGlobalLogBox.WordWrap = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Gotham", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblTitle.Location = new System.Drawing.Point(26, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(293, 35);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "SKYKING TOOLKIT";
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.Transparent;
            this.picLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.InitialImage = ((System.Drawing.Image)(resources.GetObject("picLogo.InitialImage")));
            this.picLogo.Location = new System.Drawing.Point(650, 18);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(50, 50);
            this.picLogo.TabIndex = 2;
            this.picLogo.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(734, 461);
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.tabWindow);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(750, 500);
            this.MinimumSize = new System.Drawing.Size(750, 500);
            this.Name = "Form1";
            this.Text = "Skyking Toolkit";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabWindow.ResumeLayout(false);
            this.tabParallax.ResumeLayout(false);
            this.tabParallax.PerformLayout();
            this.tabSource.ResumeLayout(false);
            this.tabSource.PerformLayout();
            this.grpSourceOptions.ResumeLayout(false);
            this.grpSourceOptions.PerformLayout();
            this.tabConvert.ResumeLayout(false);
            this.tabConvert.PerformLayout();
            this.grpConvertMode.ResumeLayout(false);
            this.grpConvertMode.PerformLayout();
            this.tabJson.ResumeLayout(false);
            this.tabJson.PerformLayout();
            this.tabLogs.ResumeLayout(false);
            this.tabLogs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabWindow;
        private System.Windows.Forms.TabPage tabParallax;
        private System.Windows.Forms.TabPage tabSource;
        private System.Windows.Forms.TabPage tabConvert;
        private System.Windows.Forms.TabPage tabJson;
        private System.Windows.Forms.ProgressBar progressParallax;
        private System.Windows.Forms.Label lblParallaxType;
        private System.Windows.Forms.RadioButton rbParallaxBoth;
        private System.Windows.Forms.RadioButton rbParallaxPBR;
        private System.Windows.Forms.RadioButton rbParallaxComplex;
        private System.Windows.Forms.Label lblOutputFolder;
        private System.Windows.Forms.Label lblParallaxSource;
        private System.Windows.Forms.Button btnRunParallax;
        private System.Windows.Forms.Button btnBrowseParallaxSource;
        private System.Windows.Forms.TextBox txtParallaxSource;
        private System.Windows.Forms.TextBox txtParallaxOutput;
        private System.Windows.Forms.Button btnBrowseParallaxOutput;
        private System.Windows.Forms.TextBox txtParallaxLog;
        private System.Windows.Forms.Label lblParallaxLog;
        private System.Windows.Forms.TabPage tabLogs;
        private System.Windows.Forms.Button btnBrowseSourceOutput;
        private System.Windows.Forms.Button btnBrowseSourceInput;
        private System.Windows.Forms.TextBox txtSourceOutput;
        private System.Windows.Forms.Label lblSourceOutput;
        private System.Windows.Forms.TextBox txtSourceInput;
        private System.Windows.Forms.Label lblSourceInput;
        private System.Windows.Forms.Button btnRunSource;
        private System.Windows.Forms.ProgressBar progressSource;
        private System.Windows.Forms.Label lblSourceLog;
        private System.Windows.Forms.TextBox txtSourceLog;
        private System.Windows.Forms.GroupBox grpSourceOptions;
        private System.Windows.Forms.RadioButton rbSourceBoth;
        private System.Windows.Forms.RadioButton rbSourceComplex;
        private System.Windows.Forms.RadioButton rbSourcePBR;
        private System.Windows.Forms.CheckBox chkSourceFlipGreen;
        private System.Windows.Forms.TextBox txtSourceExplainBox;
        private System.Windows.Forms.Button btnBrowseJsonInput;
        private System.Windows.Forms.TextBox txtJsonInput;
        private System.Windows.Forms.Label lblJsonInput;
        private System.Windows.Forms.Label lblJsonName;
        private System.Windows.Forms.TextBox txtJsonName;
        private System.Windows.Forms.Button btnRunJson;
        private System.Windows.Forms.Label lblJasonLog;
        private System.Windows.Forms.TextBox txtJsonLog;
        private System.Windows.Forms.Button btnSaveGlobalLog;
        private System.Windows.Forms.Button btnClearGlobalLog;
        private System.Windows.Forms.Label lblGlobalLog;
        private System.Windows.Forms.TextBox txtGlobalLogBox;
        private System.Windows.Forms.GroupBox grpConvertMode;
        private System.Windows.Forms.RadioButton rbConvertToCP;
        private System.Windows.Forms.RadioButton rbConvertToPBR;
        private System.Windows.Forms.Button btnRunConvert;
        private System.Windows.Forms.ProgressBar ProgressConvert;
        private System.Windows.Forms.Label lblConvertLog;
        private System.Windows.Forms.TextBox txtConvertLog;
        private System.Windows.Forms.Button btnBrowseConvertOutput;
        private System.Windows.Forms.Button btnBrowseConvertInput;
        private System.Windows.Forms.TextBox txtConvertOutput;
        private System.Windows.Forms.Label lblConvertOutput;
        private System.Windows.Forms.TextBox txtConvertInput;
        private System.Windows.Forms.Label lblConvertInput;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox picLogo;
    }
}

