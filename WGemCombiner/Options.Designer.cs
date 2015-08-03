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
            this.winFormsSkinButton.Click += new System.EventHandler(this.winFormsSkinButton_Click);
            // 
            // hellrageSkinButton
            // 
            this.hellrageSkinButton.Location = new System.Drawing.Point(145, 34);
            this.hellrageSkinButton.Name = "hellrageSkinButton";
            this.hellrageSkinButton.Size = new System.Drawing.Size(115, 23);
            this.hellrageSkinButton.TabIndex = 1;
            this.hellrageSkinButton.Text = "Hellrage\'s";
            this.hellrageSkinButton.UseVisualStyleBackColor = true;
            this.hellrageSkinButton.Click += new System.EventHandler(this.hellrageSkinButton_Click);
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
            this.recommendedLabel.Location = new System.Drawing.Point(12, 63);
            this.recommendedLabel.Name = "recommendedLabel";
            this.recommendedLabel.Size = new System.Drawing.Size(127, 13);
            this.recommendedLabel.TabIndex = 5;
            this.recommendedLabel.Text = "Recommended: \"Off\"";
            this.recommendedLabel.Visible = false;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(216, 109);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(44, 23);
            this.closeButton.TabIndex = 6;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
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
            this.alwaysOnTopCheckBox.CheckedChanged += new System.EventHandler(this.alwaysOnTopCheckBox_CheckedChanged);
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
            this.bordersCheckBox.CheckedChanged += new System.EventHandler(this.bordersCheckBox_CheckedChanged);
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 138);
            this.Controls.Add(this.bordersCheckBox);
            this.Controls.Add(this.alwaysOnTopCheckBox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.recommendedLabel);
            this.Controls.Add(this.chooseSkinLabel);
            this.Controls.Add(this.hellrageSkinButton);
            this.Controls.Add(this.winFormsSkinButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.Text = "Options";
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
    }
}