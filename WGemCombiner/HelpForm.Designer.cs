namespace WGemCombiner
{
    partial class HelpForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpForm));
			this.helpTitleTextBox = new System.Windows.Forms.TextBox();
			this.helpMessageTextBox = new System.Windows.Forms.RichTextBox();
			this.bufferedTableLayoutPanel1 = new WGemCombiner.BufferedTableLayoutPanel();
			this.closeHelpButton = new System.Windows.Forms.Button();
			this.leftButton = new System.Windows.Forms.Button();
			this.rightButton = new System.Windows.Forms.Button();
			this.bufferedTableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// helpTitleTextBox
			// 
			this.helpTitleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.helpTitleTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.helpTitleTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.helpTitleTextBox.Location = new System.Drawing.Point(12, 12);
			this.helpTitleTextBox.Name = "helpTitleTextBox";
			this.helpTitleTextBox.ReadOnly = true;
			this.helpTitleTextBox.Size = new System.Drawing.Size(362, 26);
			this.helpTitleTextBox.TabIndex = 6;
			this.helpTitleTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// helpMessageTextBox
			// 
			this.helpMessageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.helpMessageTextBox.BackColor = System.Drawing.SystemColors.Window;
			this.helpMessageTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.helpMessageTextBox.Location = new System.Drawing.Point(12, 45);
			this.helpMessageTextBox.Name = "helpMessageTextBox";
			this.helpMessageTextBox.ReadOnly = true;
			this.helpMessageTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.helpMessageTextBox.Size = new System.Drawing.Size(362, 453);
			this.helpMessageTextBox.TabIndex = 5;
			this.helpMessageTextBox.Text = "";
			this.helpMessageTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.HelpMessageTextBox_LinkClicked);
			// 
			// bufferedTableLayoutPanel1
			// 
			this.bufferedTableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.bufferedTableLayoutPanel1.ColumnCount = 7;
			this.bufferedTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.bufferedTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.bufferedTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.bufferedTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.bufferedTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.bufferedTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.bufferedTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.bufferedTableLayoutPanel1.Controls.Add(this.closeHelpButton, 3, 0);
			this.bufferedTableLayoutPanel1.Controls.Add(this.leftButton, 1, 0);
			this.bufferedTableLayoutPanel1.Controls.Add(this.rightButton, 5, 0);
			this.bufferedTableLayoutPanel1.Location = new System.Drawing.Point(12, 504);
			this.bufferedTableLayoutPanel1.Name = "bufferedTableLayoutPanel1";
			this.bufferedTableLayoutPanel1.RowCount = 1;
			this.bufferedTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.bufferedTableLayoutPanel1.Size = new System.Drawing.Size(362, 23);
			this.bufferedTableLayoutPanel1.TabIndex = 8;
			// 
			// closeHelpButton
			// 
			this.closeHelpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.closeHelpButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.closeHelpButton.Location = new System.Drawing.Point(143, 0);
			this.closeHelpButton.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.closeHelpButton.Name = "closeHelpButton";
			this.closeHelpButton.Size = new System.Drawing.Size(75, 23);
			this.closeHelpButton.TabIndex = 0;
			this.closeHelpButton.Text = "Close";
			this.closeHelpButton.UseVisualStyleBackColor = false;
			this.closeHelpButton.Click += new System.EventHandler(this.CloseHelpButton_Click);
			// 
			// leftButton
			// 
			this.leftButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.leftButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.leftButton.Location = new System.Drawing.Point(13, 0);
			this.leftButton.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.leftButton.Name = "leftButton";
			this.leftButton.Size = new System.Drawing.Size(111, 23);
			this.leftButton.TabIndex = 2;
			this.leftButton.Text = "Previous page";
			this.leftButton.UseVisualStyleBackColor = false;
			this.leftButton.Click += new System.EventHandler(this.LeftButton_Click);
			// 
			// rightButton
			// 
			this.rightButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.rightButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.rightButton.Location = new System.Drawing.Point(237, 0);
			this.rightButton.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.rightButton.Name = "rightButton";
			this.rightButton.Size = new System.Drawing.Size(111, 23);
			this.rightButton.TabIndex = 3;
			this.rightButton.Text = "Next page";
			this.rightButton.UseVisualStyleBackColor = false;
			this.rightButton.Click += new System.EventHandler(this.RightButton_Click);
			// 
			// HelpForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(386, 539);
			this.Controls.Add(this.bufferedTableLayoutPanel1);
			this.Controls.Add(this.helpMessageTextBox);
			this.Controls.Add(this.helpTitleTextBox);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.HelpButton = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(333, 200);
			this.Name = "HelpForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Help";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HelpForm_FormClosing);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HelpForm_MouseDown);
			this.bufferedTableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox helpTitleTextBox;
		private System.Windows.Forms.RichTextBox helpMessageTextBox;
		private System.Windows.Forms.Button leftButton;
		private System.Windows.Forms.Button closeHelpButton;
		private System.Windows.Forms.Button rightButton;
		private BufferedTableLayoutPanel bufferedTableLayoutPanel1;
	}
}