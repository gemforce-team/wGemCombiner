namespace WGemCombiner
{
    partial class Options
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.chooseSkinLabel = new System.Windows.Forms.Label();
            this.recommendedLabel = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.alwaysOnTopCheckBox = new System.Windows.Forms.CheckBox();
            this.bordersCheckBox = new System.Windows.Forms.CheckBox();
            this.hotkeyTextBox = new System.Windows.Forms.TextBox();
            this.hotkeyLabel = new System.Windows.Forms.Label();
            this.hidePanelsCheckBox = new System.Windows.Forms.CheckBox();
            this.winFormsSkinButton = new System.Windows.Forms.RadioButton();
            this.hellrageSkinButton = new System.Windows.Forms.RadioButton();
            this.autoCombineCheckBox = new System.Windows.Forms.CheckBox();
            this.namingBufferedGroupBox = new WGemCombiner.BufferedGroupBox();
            this.RestartLabel = new System.Windows.Forms.Label();
            this.useColorsButton = new System.Windows.Forms.RadioButton();
            this.useEffectsButton = new System.Windows.Forms.RadioButton();
            this.namingBufferedGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // chooseSkinLabel
            // 
            this.chooseSkinLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chooseSkinLabel.BackColor = System.Drawing.Color.Transparent;
            this.chooseSkinLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chooseSkinLabel.Location = new System.Drawing.Point(22, 9);
            this.chooseSkinLabel.Name = "chooseSkinLabel";
            this.chooseSkinLabel.Size = new System.Drawing.Size(82, 13);
            this.chooseSkinLabel.TabIndex = 2;
            this.chooseSkinLabel.Text = "Choose Skin:";
            // 
            // recommendedLabel
            // 
            this.recommendedLabel.AutoSize = true;
            this.recommendedLabel.BackColor = System.Drawing.Color.Transparent;
            this.recommendedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.recommendedLabel.Location = new System.Drawing.Point(12, 84);
            this.recommendedLabel.Name = "recommendedLabel";
            this.recommendedLabel.Size = new System.Drawing.Size(115, 13);
            this.recommendedLabel.TabIndex = 5;
            this.recommendedLabel.Text = "(Recommended: \"Off\")";
            this.recommendedLabel.Visible = false;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.closeButton.Location = new System.Drawing.Point(118, 281);
            this.closeButton.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(44, 23);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // alwaysOnTopCheckBox
            // 
            this.alwaysOnTopCheckBox.AutoSize = true;
            this.alwaysOnTopCheckBox.Location = new System.Drawing.Point(12, 110);
            this.alwaysOnTopCheckBox.Name = "alwaysOnTopCheckBox";
            this.alwaysOnTopCheckBox.Size = new System.Drawing.Size(98, 17);
            this.alwaysOnTopCheckBox.TabIndex = 7;
            this.alwaysOnTopCheckBox.Text = "Always On Top";
            this.alwaysOnTopCheckBox.UseVisualStyleBackColor = true;
            this.alwaysOnTopCheckBox.CheckedChanged += new System.EventHandler(this.AlwaysOnTopCheckBox_CheckedChanged);
            // 
            // bordersCheckBox
            // 
            this.bordersCheckBox.AutoSize = true;
            this.bordersCheckBox.Location = new System.Drawing.Point(12, 67);
            this.bordersCheckBox.Name = "bordersCheckBox";
            this.bordersCheckBox.Size = new System.Drawing.Size(104, 17);
            this.bordersCheckBox.TabIndex = 6;
            this.bordersCheckBox.Text = "Window Borders";
            this.bordersCheckBox.UseVisualStyleBackColor = true;
            this.bordersCheckBox.CheckedChanged += new System.EventHandler(this.BordersCheckBox_CheckedChanged);
            // 
            // hotkeyTextBox
            // 
            this.hotkeyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hotkeyTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.hotkeyTextBox.Location = new System.Drawing.Point(139, 249);
            this.hotkeyTextBox.MaxLength = 1;
            this.hotkeyTextBox.Name = "hotkeyTextBox";
            this.hotkeyTextBox.Size = new System.Drawing.Size(129, 20);
            this.hotkeyTextBox.TabIndex = 11;
            this.hotkeyTextBox.Text = "9";
            this.hotkeyTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hotkeyTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyTextBox_KeyDown);
            this.hotkeyTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.HotkeyTextBox_PreviewKeyDown);
            // 
            // hotkeyLabel
            // 
            this.hotkeyLabel.AutoSize = true;
            this.hotkeyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hotkeyLabel.Location = new System.Drawing.Point(12, 252);
            this.hotkeyLabel.Name = "hotkeyLabel";
            this.hotkeyLabel.Size = new System.Drawing.Size(121, 13);
            this.hotkeyLabel.TabIndex = 10;
            this.hotkeyLabel.Text = "Hotkey To Activate:";
            // 
            // hidePanelsCheckBox
            // 
            this.hidePanelsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hidePanelsCheckBox.AutoSize = true;
            this.hidePanelsCheckBox.Location = new System.Drawing.Point(152, 54);
            this.hidePanelsCheckBox.Name = "hidePanelsCheckBox";
            this.hidePanelsCheckBox.Size = new System.Drawing.Size(111, 43);
            this.hidePanelsCheckBox.TabIndex = 8;
            this.hidePanelsCheckBox.Text = "Automatically hide\r\ninfo panels\r\nbefore combining";
            this.hidePanelsCheckBox.UseVisualStyleBackColor = true;
            this.hidePanelsCheckBox.CheckedChanged += new System.EventHandler(this.HidePanelsCheckBox_CheckedChanged);
            // 
            // winFormsSkinButton
            // 
            this.winFormsSkinButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.winFormsSkinButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.winFormsSkinButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.winFormsSkinButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.winFormsSkinButton.Location = new System.Drawing.Point(22, 25);
            this.winFormsSkinButton.Name = "winFormsSkinButton";
            this.winFormsSkinButton.Size = new System.Drawing.Size(115, 23);
            this.winFormsSkinButton.TabIndex = 3;
            this.winFormsSkinButton.TabStop = true;
            this.winFormsSkinButton.Tag = "NoSkin";
            this.winFormsSkinButton.Text = "Windows Forms";
            this.winFormsSkinButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.winFormsSkinButton.UseVisualStyleBackColor = false;
            this.winFormsSkinButton.CheckedChanged += new System.EventHandler(this.WinFormsButton_CheckChanged);
            // 
            // hellrageSkinButton
            // 
            this.hellrageSkinButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.hellrageSkinButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.hellrageSkinButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("hellrageSkinButton.BackgroundImage")));
            this.hellrageSkinButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.hellrageSkinButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.hellrageSkinButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.hellrageSkinButton.Location = new System.Drawing.Point(143, 25);
            this.hellrageSkinButton.Name = "hellrageSkinButton";
            this.hellrageSkinButton.Size = new System.Drawing.Size(115, 23);
            this.hellrageSkinButton.TabIndex = 4;
            this.hellrageSkinButton.TabStop = true;
            this.hellrageSkinButton.Tag = "NoSkin";
            this.hellrageSkinButton.Text = "Hellrage\'s";
            this.hellrageSkinButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.hellrageSkinButton.UseVisualStyleBackColor = false;
            this.hellrageSkinButton.CheckedChanged += new System.EventHandler(this.HellrageSkinButton_CheckedChanged);
            // 
            // autoCombineCheckBox
            // 
            this.autoCombineCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.autoCombineCheckBox.AutoSize = true;
            this.autoCombineCheckBox.Location = new System.Drawing.Point(152, 103);
            this.autoCombineCheckBox.Name = "autoCombineCheckBox";
            this.autoCombineCheckBox.Size = new System.Drawing.Size(115, 30);
            this.autoCombineCheckBox.TabIndex = 9;
            this.autoCombineCheckBox.Text = "Automatically enter\r\ncombine mode";
            this.autoCombineCheckBox.UseVisualStyleBackColor = true;
            this.autoCombineCheckBox.CheckedChanged += new System.EventHandler(this.AutoCombineCheckBox_CheckedChanged);
            // 
            // namingBufferedGroupBox
            // 
            this.namingBufferedGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namingBufferedGroupBox.AutoSize = true;
            this.namingBufferedGroupBox.Controls.Add(this.RestartLabel);
            this.namingBufferedGroupBox.Controls.Add(this.useColorsButton);
            this.namingBufferedGroupBox.Controls.Add(this.useEffectsButton);
            this.namingBufferedGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.namingBufferedGroupBox.Location = new System.Drawing.Point(12, 175);
            this.namingBufferedGroupBox.Name = "namingBufferedGroupBox";
            this.namingBufferedGroupBox.Size = new System.Drawing.Size(255, 68);
            this.namingBufferedGroupBox.TabIndex = 13;
            this.namingBufferedGroupBox.TabStop = false;
            this.namingBufferedGroupBox.Text = "Naming Convention";
            // 
            // RestartLabel
            // 
            this.RestartLabel.AutoSize = true;
            this.RestartLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RestartLabel.Location = new System.Drawing.Point(45, 39);
            this.RestartLabel.Name = "RestartLabel";
            this.RestartLabel.Size = new System.Drawing.Size(147, 13);
            this.RestartLabel.TabIndex = 3;
            this.RestartLabel.Text = "(only takes effect after restart)";
            // 
            // useColorsButton
            // 
            this.useColorsButton.AutoSize = true;
            this.useColorsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.useColorsButton.Location = new System.Drawing.Point(134, 19);
            this.useColorsButton.Name = "useColorsButton";
            this.useColorsButton.Size = new System.Drawing.Size(75, 17);
            this.useColorsButton.TabIndex = 2;
            this.useColorsButton.TabStop = true;
            this.useColorsButton.Text = "Use colors";
            this.useColorsButton.UseVisualStyleBackColor = true;
            this.useColorsButton.CheckedChanged += new System.EventHandler(this.UseColorsButton_CheckedChanged);
            // 
            // useEffectsButton
            // 
            this.useEffectsButton.AutoSize = true;
            this.useEffectsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.useEffectsButton.Location = new System.Drawing.Point(27, 19);
            this.useEffectsButton.Name = "useEffectsButton";
            this.useEffectsButton.Size = new System.Drawing.Size(79, 17);
            this.useEffectsButton.TabIndex = 1;
            this.useEffectsButton.TabStop = true;
            this.useEffectsButton.Text = "Use effects";
            this.useEffectsButton.UseVisualStyleBackColor = true;
            this.useEffectsButton.CheckedChanged += new System.EventHandler(this.UseEffectsButton_CheckedChanged);
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(280, 313);
            this.Controls.Add(this.namingBufferedGroupBox);
            this.Controls.Add(this.autoCombineCheckBox);
            this.Controls.Add(this.hellrageSkinButton);
            this.Controls.Add(this.winFormsSkinButton);
            this.Controls.Add(this.hidePanelsCheckBox);
            this.Controls.Add(this.hotkeyLabel);
            this.Controls.Add(this.hotkeyTextBox);
            this.Controls.Add(this.bordersCheckBox);
            this.Controls.Add(this.alwaysOnTopCheckBox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.recommendedLabel);
            this.Controls.Add(this.chooseSkinLabel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(277, 341);
            this.Name = "Options";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Options_FormClosing);
            this.Load += new System.EventHandler(this.Options_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Options_MouseDown);
            this.namingBufferedGroupBox.ResumeLayout(false);
            this.namingBufferedGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label chooseSkinLabel;
        private System.Windows.Forms.Label recommendedLabel;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.CheckBox alwaysOnTopCheckBox;
        private System.Windows.Forms.CheckBox bordersCheckBox;
        private System.Windows.Forms.TextBox hotkeyTextBox;
        private System.Windows.Forms.Label hotkeyLabel;
        private System.Windows.Forms.CheckBox hidePanelsCheckBox;
        private System.Windows.Forms.RadioButton winFormsSkinButton;
        private System.Windows.Forms.RadioButton hellrageSkinButton;
        private System.Windows.Forms.CheckBox autoCombineCheckBox;
        private BufferedGroupBox namingBufferedGroupBox;
        private System.Windows.Forms.Label RestartLabel;
        private System.Windows.Forms.RadioButton useColorsButton;
        private System.Windows.Forms.RadioButton useEffectsButton;
    }
}