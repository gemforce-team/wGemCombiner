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
            this.closeHelpButton = new System.Windows.Forms.Button();
            this.leftButton = new System.Windows.Forms.Button();
            this.rightButton = new System.Windows.Forms.Button();
            this.helpMessageTextBox = new System.Windows.Forms.RichTextBox();
            this.helpTitleTextBox = new System.Windows.Forms.TextBox();
            this.helpMessagePanel = new System.Windows.Forms.Panel();
            this.helpMessagePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeHelpButton
            // 
            this.closeHelpButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.closeHelpButton.BackColor = System.Drawing.SystemColors.Control;
            this.closeHelpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.closeHelpButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closeHelpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.closeHelpButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.closeHelpButton.Location = new System.Drawing.Point(189, 567);
            this.closeHelpButton.Name = "closeHelpButton";
            this.closeHelpButton.Size = new System.Drawing.Size(75, 23);
            this.closeHelpButton.TabIndex = 0;
            this.closeHelpButton.Text = "Close";
            this.closeHelpButton.UseVisualStyleBackColor = false;
            this.closeHelpButton.Click += new System.EventHandler(this.closeHelpButton_Click);
            // 
            // leftButton
            // 
            this.leftButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.leftButton.BackColor = System.Drawing.SystemColors.Control;
            this.leftButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.leftButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.leftButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.leftButton.Location = new System.Drawing.Point(72, 567);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(111, 23);
            this.leftButton.TabIndex = 2;
            this.leftButton.Text = "Previous page";
            this.leftButton.UseVisualStyleBackColor = false;
            this.leftButton.Click += new System.EventHandler(this.leftButton_Click);
            // 
            // rightButton
            // 
            this.rightButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.rightButton.BackColor = System.Drawing.SystemColors.Control;
            this.rightButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rightButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rightButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rightButton.Location = new System.Drawing.Point(270, 567);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(111, 23);
            this.rightButton.TabIndex = 3;
            this.rightButton.Text = "Next page";
            this.rightButton.UseVisualStyleBackColor = false;
            this.rightButton.Click += new System.EventHandler(this.rightButton_Click);
            // 
            // helpMessageTextBox
            // 
            this.helpMessageTextBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.helpMessageTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.helpMessageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.helpMessageTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.helpMessageTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.helpMessageTextBox.Location = new System.Drawing.Point(12, -2);
            this.helpMessageTextBox.MaximumSize = new System.Drawing.Size(442, 537);
            this.helpMessageTextBox.Name = "helpMessageTextBox";
            this.helpMessageTextBox.ReadOnly = true;
            this.helpMessageTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.helpMessageTextBox.Size = new System.Drawing.Size(411, 503);
            this.helpMessageTextBox.TabIndex = 4;
            this.helpMessageTextBox.Text = "";
            // 
            // helpTitleTextBox
            // 
            this.helpTitleTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.helpTitleTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.helpTitleTextBox.Location = new System.Drawing.Point(12, 20);
            this.helpTitleTextBox.Name = "helpTitleTextBox";
            this.helpTitleTextBox.ReadOnly = true;
            this.helpTitleTextBox.Size = new System.Drawing.Size(428, 26);
            this.helpTitleTextBox.TabIndex = 5;
            this.helpTitleTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // helpMessagePanel
            // 
            this.helpMessagePanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.helpMessagePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.helpMessagePanel.Controls.Add(this.helpMessageTextBox);
            this.helpMessagePanel.Location = new System.Drawing.Point(12, 53);
            this.helpMessagePanel.Name = "helpMessagePanel";
            this.helpMessagePanel.Size = new System.Drawing.Size(428, 508);
            this.helpMessagePanel.TabIndex = 6;
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(452, 601);
            this.Controls.Add(this.helpMessagePanel);
            this.Controls.Add(this.helpTitleTextBox);
            this.Controls.Add(this.rightButton);
            this.Controls.Add(this.leftButton);
            this.Controls.Add(this.closeHelpButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "HelpForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Help";
            this.Load += new System.EventHandler(this.HelpForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HelpForm_MouseDown);
            this.helpMessagePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeHelpButton;
        private System.Windows.Forms.Button leftButton;
        private System.Windows.Forms.Button rightButton;
        private System.Windows.Forms.RichTextBox helpMessageTextBox;
        private System.Windows.Forms.TextBox helpTitleTextBox;
        private System.Windows.Forms.Panel helpMessagePanel;
    }
}