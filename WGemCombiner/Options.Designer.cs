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
			this.winFormsSkinButton = new System.Windows.Forms.Button();
			this.hellrageSkinButton = new System.Windows.Forms.Button();
			this.chooseSkinLabel = new System.Windows.Forms.Label();
			this.recommendedLabel = new System.Windows.Forms.Label();
			this.closeButton = new System.Windows.Forms.Button();
			this.alwaysOnTopCheckBox = new System.Windows.Forms.CheckBox();
			this.bordersCheckBox = new System.Windows.Forms.CheckBox();
			this.hotkeyTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// winFormsSkinButton
			// 
			this.winFormsSkinButton.Location = new System.Drawing.Point(145, 5);
			this.winFormsSkinButton.Name = "winFormsSkinButton";
			this.winFormsSkinButton.Size = new System.Drawing.Size(115, 23);
			this.winFormsSkinButton.TabIndex = 0;
			this.winFormsSkinButton.Text = "Windows Forms";
			this.winFormsSkinButton.UseVisualStyleBackColor = true;
			this.winFormsSkinButton.Click += new System.EventHandler(this.WinFormsSkinButton_Click);
			// 
			// hellrageSkinButton
			// 
			this.hellrageSkinButton.Location = new System.Drawing.Point(145, 34);
			this.hellrageSkinButton.Name = "hellrageSkinButton";
			this.hellrageSkinButton.Size = new System.Drawing.Size(115, 23);
			this.hellrageSkinButton.TabIndex = 1;
			this.hellrageSkinButton.Text = "Hellrage\'s";
			this.hellrageSkinButton.UseVisualStyleBackColor = true;
			this.hellrageSkinButton.Click += new System.EventHandler(this.HellrageSkinButton_Click);
			// 
			// chooseSkinLabel
			// 
			this.chooseSkinLabel.AutoSize = true;
			this.chooseSkinLabel.BackColor = System.Drawing.Color.Transparent;
			this.chooseSkinLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.chooseSkinLabel.Location = new System.Drawing.Point(12, 23);
			this.chooseSkinLabel.Name = "chooseSkinLabel";
			this.chooseSkinLabel.Size = new System.Drawing.Size(82, 13);
			this.chooseSkinLabel.TabIndex = 3;
			this.chooseSkinLabel.Text = "Choose Skin:";
			// 
			// recommendedLabel
			// 
			this.recommendedLabel.AutoSize = true;
			this.recommendedLabel.BackColor = System.Drawing.Color.Transparent;
			this.recommendedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.recommendedLabel.Location = new System.Drawing.Point(12, 64);
			this.recommendedLabel.Name = "recommendedLabel";
			this.recommendedLabel.Size = new System.Drawing.Size(127, 13);
			this.recommendedLabel.TabIndex = 5;
			this.recommendedLabel.Text = "Recommended: \"Off\"";
			this.recommendedLabel.Visible = false;
			// 
			// closeButton
			// 
			this.closeButton.Location = new System.Drawing.Point(216, 135);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(44, 23);
			this.closeButton.TabIndex = 6;
			this.closeButton.Text = "Close";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
			// 
			// alwaysOnTopCheckBox
			// 
			this.alwaysOnTopCheckBox.AutoSize = true;
			this.alwaysOnTopCheckBox.Location = new System.Drawing.Point(145, 86);
			this.alwaysOnTopCheckBox.Name = "alwaysOnTopCheckBox";
			this.alwaysOnTopCheckBox.Size = new System.Drawing.Size(98, 17);
			this.alwaysOnTopCheckBox.TabIndex = 8;
			this.alwaysOnTopCheckBox.Text = "Always On Top";
			this.alwaysOnTopCheckBox.UseVisualStyleBackColor = true;
			this.alwaysOnTopCheckBox.CheckedChanged += new System.EventHandler(this.AlwaysOnTopCheckBox_CheckedChanged);
			// 
			// bordersCheckBox
			// 
			this.bordersCheckBox.AutoSize = true;
			this.bordersCheckBox.Location = new System.Drawing.Point(145, 63);
			this.bordersCheckBox.Name = "bordersCheckBox";
			this.bordersCheckBox.Size = new System.Drawing.Size(104, 17);
			this.bordersCheckBox.TabIndex = 9;
			this.bordersCheckBox.Text = "Window Borders";
			this.bordersCheckBox.UseVisualStyleBackColor = true;
			this.bordersCheckBox.CheckedChanged += new System.EventHandler(this.BordersCheckBox_CheckedChanged);
			// 
			// hotkeyTextBox
			// 
			this.hotkeyTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.hotkeyTextBox.Location = new System.Drawing.Point(145, 109);
			this.hotkeyTextBox.MaxLength = 1;
			this.hotkeyTextBox.Name = "hotkeyTextBox";
			this.hotkeyTextBox.Size = new System.Drawing.Size(115, 20);
			this.hotkeyTextBox.TabIndex = 10;
			this.hotkeyTextBox.Text = "9";
			this.hotkeyTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.hotkeyTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyTextBox_KeyDown);
			this.hotkeyTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.HotkeyTextBox_PreviewKeyDown);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 112);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(121, 13);
			this.label1.TabIndex = 11;
			this.label1.Text = "Hotkey To Activate:";
			// 
			// Options
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(267, 166);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.hotkeyTextBox);
			this.Controls.Add(this.bordersCheckBox);
			this.Controls.Add(this.alwaysOnTopCheckBox);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.recommendedLabel);
			this.Controls.Add(this.chooseSkinLabel);
			this.Controls.Add(this.hellrageSkinButton);
			this.Controls.Add(this.winFormsSkinButton);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Options";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Options";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Options_FormClosing);
			this.Load += new System.EventHandler(this.Options_Load);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Options_MouseDown);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button winFormsSkinButton;
        private System.Windows.Forms.Button hellrageSkinButton;
        private System.Windows.Forms.Label chooseSkinLabel;
        private System.Windows.Forms.Label recommendedLabel;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.CheckBox alwaysOnTopCheckBox;
        private System.Windows.Forms.CheckBox bordersCheckBox;
        private System.Windows.Forms.TextBox hotkeyTextBox;
        private System.Windows.Forms.Label label1;
    }
}