namespace WGemCombiner
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
            this.label1 = new System.Windows.Forms.Label();
            this.getInstructionsButton = new System.Windows.Forms.Button();
            this.parenthesisRadioButton = new System.Windows.Forms.RadioButton();
            this.equationsRadioButton = new System.Windows.Forms.RadioButton();
            this.instructionsListBox = new System.Windows.Forms.ListBox();
            this.instructionsLabel = new System.Windows.Forms.Label();
            this.combineButton = new System.Windows.Forms.Button();
            this.resultInfoLabel = new System.Windows.Forms.Label();
            this.delayLabel = new System.Windows.Forms.Label();
            this.delayNumeric = new System.Windows.Forms.NumericUpDown();
            this.stepLabel = new System.Windows.Forms.Label();
            this.stepNumeric = new System.Windows.Forms.NumericUpDown();
            this.copyListButton = new System.Windows.Forms.Button();
            this.helpButton = new System.Windows.Forms.Button();
            this.formulaInputTextBox = new System.Windows.Forms.RichTextBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.optionsButton = new System.Windows.Forms.Button();
            this.combineComboBox = new System.Windows.Forms.ComboBox();
            this.colorComboBox = new System.Windows.Forms.ComboBox();
            this.importFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.gemsListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.delayNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Formula/Combine scheme:";
            // 
            // getInstructionsButton
            // 
            this.getInstructionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.getInstructionsButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.getInstructionsButton.Location = new System.Drawing.Point(18, 197);
            this.getInstructionsButton.Name = "getInstructionsButton";
            this.getInstructionsButton.Size = new System.Drawing.Size(99, 23);
            this.getInstructionsButton.TabIndex = 2;
            this.getInstructionsButton.Text = "Get Instructions";
            this.getInstructionsButton.Click += new System.EventHandler(this.getInstructionsButton_Click);
            // 
            // parenthesisRadioButton
            // 
            this.parenthesisRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.parenthesisRadioButton.AutoSize = true;
            this.parenthesisRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.parenthesisRadioButton.Checked = true;
            this.parenthesisRadioButton.Location = new System.Drawing.Point(89, 176);
            this.parenthesisRadioButton.Name = "parenthesisRadioButton";
            this.parenthesisRadioButton.Size = new System.Drawing.Size(80, 17);
            this.parenthesisRadioButton.TabIndex = 19;
            this.parenthesisRadioButton.TabStop = true;
            this.parenthesisRadioButton.Text = "Parenthesis";
            this.parenthesisRadioButton.UseVisualStyleBackColor = false;
            // 
            // equationsRadioButton
            // 
            this.equationsRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.equationsRadioButton.AutoSize = true;
            this.equationsRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.equationsRadioButton.Location = new System.Drawing.Point(18, 176);
            this.equationsRadioButton.Name = "equationsRadioButton";
            this.equationsRadioButton.Size = new System.Drawing.Size(72, 17);
            this.equationsRadioButton.TabIndex = 20;
            this.equationsRadioButton.Text = "Equations";
            this.equationsRadioButton.UseVisualStyleBackColor = false;
            // 
            // instructionsListBox
            // 
            this.instructionsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.instructionsListBox.FormattingEnabled = true;
            this.instructionsListBox.Location = new System.Drawing.Point(218, 29);
            this.instructionsListBox.Name = "instructionsListBox";
            this.instructionsListBox.Size = new System.Drawing.Size(149, 199);
            this.instructionsListBox.TabIndex = 21;
            // 
            // instructionsLabel
            // 
            this.instructionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.instructionsLabel.AutoSize = true;
            this.instructionsLabel.Location = new System.Drawing.Point(218, 13);
            this.instructionsLabel.Name = "instructionsLabel";
            this.instructionsLabel.Size = new System.Drawing.Size(64, 13);
            this.instructionsLabel.TabIndex = 22;
            this.instructionsLabel.Text = "Instructions:";
            // 
            // combineButton
            // 
            this.combineButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.combineButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.combineButton.Location = new System.Drawing.Point(21, 327);
            this.combineButton.Name = "combineButton";
            this.combineButton.Size = new System.Drawing.Size(128, 23);
            this.combineButton.TabIndex = 23;
            this.combineButton.Text = "Combine";
            this.combineButton.Click += new System.EventHandler(this.combineButton_Click);
            // 
            // resultInfoLabel
            // 
            this.resultInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.resultInfoLabel.AutoSize = true;
            this.resultInfoLabel.Location = new System.Drawing.Point(18, 223);
            this.resultInfoLabel.Name = "resultInfoLabel";
            this.resultInfoLabel.Size = new System.Drawing.Size(75, 26);
            this.resultInfoLabel.TabIndex = 24;
            this.resultInfoLabel.Text = "Result Here\r\n(Grade + Cost)";
            // 
            // delayLabel
            // 
            this.delayLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.delayLabel.AutoSize = true;
            this.delayLabel.Location = new System.Drawing.Point(18, 280);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(37, 13);
            this.delayLabel.TabIndex = 25;
            this.delayLabel.Text = "Delay:";
            // 
            // delayNumeric
            // 
            this.delayNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.delayNumeric.Location = new System.Drawing.Point(21, 296);
            this.delayNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.delayNumeric.Name = "delayNumeric";
            this.delayNumeric.Size = new System.Drawing.Size(57, 20);
            this.delayNumeric.TabIndex = 26;
            this.delayNumeric.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.delayNumeric.ValueChanged += new System.EventHandler(this.delayNumeric_ValueChanged);
            // 
            // stepLabel
            // 
            this.stepLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stepLabel.AutoSize = true;
            this.stepLabel.Location = new System.Drawing.Point(89, 280);
            this.stepLabel.Name = "stepLabel";
            this.stepLabel.Size = new System.Drawing.Size(67, 13);
            this.stepLabel.TabIndex = 25;
            this.stepLabel.Text = "Start at step:";
            // 
            // stepNumeric
            // 
            this.stepNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stepNumeric.Location = new System.Drawing.Point(92, 296);
            this.stepNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.stepNumeric.Name = "stepNumeric";
            this.stepNumeric.Size = new System.Drawing.Size(57, 20);
            this.stepNumeric.TabIndex = 26;
            this.stepNumeric.ValueChanged += new System.EventHandler(this.stepNumeric_ValueChanged);
            // 
            // copyListButton
            // 
            this.copyListButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.copyListButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.copyListButton.Location = new System.Drawing.Point(301, 5);
            this.copyListButton.Name = "copyListButton";
            this.copyListButton.Size = new System.Drawing.Size(66, 21);
            this.copyListButton.TabIndex = 27;
            this.copyListButton.Text = "Copy List";
            this.copyListButton.UseVisualStyleBackColor = false;
            this.copyListButton.Click += new System.EventHandler(this.copyList_Click);
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.helpButton.Location = new System.Drawing.Point(301, 302);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(66, 23);
            this.helpButton.TabIndex = 28;
            this.helpButton.Text = "Help";
            this.helpButton.Click += new System.EventHandler(this.helpButton_Click);
            // 
            // formulaInputTextBox
            // 
            this.formulaInputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formulaInputTextBox.EnableAutoDragDrop = true;
            this.formulaInputTextBox.Location = new System.Drawing.Point(18, 84);
            this.formulaInputTextBox.Name = "formulaInputTextBox";
            this.formulaInputTextBox.Size = new System.Drawing.Size(166, 88);
            this.formulaInputTextBox.TabIndex = 29;
            this.formulaInputTextBox.Text = "";
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.exitButton.Location = new System.Drawing.Point(218, 331);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 31;
            this.exitButton.Text = "Exit";
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // optionsButton
            // 
            this.optionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.optionsButton.Location = new System.Drawing.Point(218, 302);
            this.optionsButton.Name = "optionsButton";
            this.optionsButton.Size = new System.Drawing.Size(75, 23);
            this.optionsButton.TabIndex = 32;
            this.optionsButton.Text = "Options";
            this.optionsButton.UseVisualStyleBackColor = true;
            this.optionsButton.Click += new System.EventHandler(this.optionsButton_Click);
            // 
            // combineComboBox
            // 
            this.combineComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combineComboBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combineComboBox.FormattingEnabled = true;
            this.combineComboBox.Items.AddRange(new object[] {
            "Paste"});
            this.combineComboBox.Location = new System.Drawing.Point(18, 55);
            this.combineComboBox.Name = "combineComboBox";
            this.combineComboBox.Size = new System.Drawing.Size(142, 23);
            this.combineComboBox.TabIndex = 33;
            this.combineComboBox.SelectedIndexChanged += new System.EventHandler(this.combineComboBox_SelectedIndexChanged);
            // 
            // colorComboBox
            // 
            this.colorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorComboBox.FormattingEnabled = true;
            this.colorComboBox.Location = new System.Drawing.Point(18, 31);
            this.colorComboBox.Name = "colorComboBox";
            this.colorComboBox.Size = new System.Drawing.Size(142, 21);
            this.colorComboBox.TabIndex = 34;
            this.colorComboBox.SelectedIndexChanged += new System.EventHandler(this.colorComboBox_SelectedIndexChanged);
            // 
            // importFileDialog
            // 
            this.importFileDialog.Filter = "Text files|*.txt";
            this.importFileDialog.InitialDirectory = "N:\\Каталоги\\Git\\wGemCombiner\\WGemCombiner\\Resources";
            // 
            // gemsListBox
            // 
            this.gemsListBox.FormattingEnabled = true;
            this.gemsListBox.Location = new System.Drawing.Point(218, 253);
            this.gemsListBox.Name = "gemsListBox";
            this.gemsListBox.Size = new System.Drawing.Size(149, 43);
            this.gemsListBox.TabIndex = 35;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(218, 236);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 36;
            this.label2.Text = "Gem Locations:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 366);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gemsListBox);
            this.Controls.Add(this.combineComboBox);
            this.Controls.Add(this.colorComboBox);
            this.Controls.Add(this.optionsButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.formulaInputTextBox);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.stepNumeric);
            this.Controls.Add(this.delayNumeric);
            this.Controls.Add(this.stepLabel);
            this.Controls.Add(this.delayLabel);
            this.Controls.Add(this.resultInfoLabel);
            this.Controls.Add(this.combineButton);
            this.Controls.Add(this.instructionsLabel);
            this.Controls.Add(this.instructionsListBox);
            this.Controls.Add(this.parenthesisRadioButton);
            this.Controls.Add(this.equationsRadioButton);
            this.Controls.Add(this.getInstructionsButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.copyListButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(383, 349);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gem Combiner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.delayNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button getInstructionsButton;
        private System.Windows.Forms.RadioButton parenthesisRadioButton;
        private System.Windows.Forms.RadioButton equationsRadioButton;
        private System.Windows.Forms.ListBox instructionsListBox;
        private System.Windows.Forms.Label instructionsLabel;
        private System.Windows.Forms.Button combineButton;
        private System.Windows.Forms.Label resultInfoLabel;
        private System.Windows.Forms.Label delayLabel;
        private System.Windows.Forms.NumericUpDown delayNumeric;
        private System.Windows.Forms.Label stepLabel;
        private System.Windows.Forms.NumericUpDown stepNumeric;
        private System.Windows.Forms.Button copyListButton;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.RichTextBox formulaInputTextBox;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button optionsButton;
        private System.Windows.Forms.ComboBox combineComboBox;
        private System.Windows.Forms.ComboBox colorComboBox;
        private System.Windows.Forms.OpenFileDialog importFileDialog;
        private System.Windows.Forms.ListBox gemsListBox;
        private System.Windows.Forms.Label label2;
    }
}

