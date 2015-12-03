﻿namespace WGemCombiner
{
	partial class GemCombiner
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GemCombiner));
			this.schemeLabel = new System.Windows.Forms.Label();
			this.parseRecipeButton = new System.Windows.Forms.Button();
			this.instructionsLabel = new System.Windows.Forms.Label();
			this.combineButton = new System.Windows.Forms.Button();
			this.delayLabel = new System.Windows.Forms.Label();
			this.delayNumeric = new System.Windows.Forms.NumericUpDown();
			this.stepLabel = new System.Windows.Forms.Label();
			this.stepNumeric = new System.Windows.Forms.NumericUpDown();
			this.helpButton = new System.Windows.Forms.Button();
			this.exitButton = new System.Windows.Forms.Button();
			this.optionsButton = new System.Windows.Forms.Button();
			this.combineComboBox = new System.Windows.Forms.ComboBox();
			this.colorComboBox = new System.Windows.Forms.ComboBox();
			this.importFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.gemLocationsLabel = new System.Windows.Forms.Label();
			this.resultLabel = new System.Windows.Forms.Label();
			this.formulaInputRichTextBox = new System.Windows.Forms.RichTextBox();
			this.baseGemsListBox = new System.Windows.Forms.ListBox();
			this.slotLimitUpDown = new System.Windows.Forms.NumericUpDown();
			this.slotLimitLabel = new System.Windows.Forms.Label();
			this.instructionsTextBox = new System.Windows.Forms.RichTextBox();
			this.combineProgressBar = new WGemCombiner.TextProgressBar();
			((System.ComponentModel.ISupportInitialize)(this.delayNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.stepNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.slotLimitUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// schemeLabel
			// 
			this.schemeLabel.AutoSize = true;
			this.schemeLabel.Location = new System.Drawing.Point(12, 12);
			this.schemeLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.schemeLabel.Name = "schemeLabel";
			this.schemeLabel.Size = new System.Drawing.Size(161, 13);
			this.schemeLabel.TabIndex = 0;
			this.schemeLabel.Text = "Select the spec/combine recipe:";
			// 
			// parseRecipeButton
			// 
			this.parseRecipeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.parseRecipeButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.parseRecipeButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.parseRecipeButton.Location = new System.Drawing.Point(12, 200);
			this.parseRecipeButton.Name = "parseRecipeButton";
			this.parseRecipeButton.Size = new System.Drawing.Size(170, 23);
			this.parseRecipeButton.TabIndex = 2;
			this.parseRecipeButton.Text = "Parse custom recipe";
			this.parseRecipeButton.Click += new System.EventHandler(this.ParseRecipeButton_Click);
			// 
			// instructionsLabel
			// 
			this.instructionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.instructionsLabel.AutoSize = true;
			this.instructionsLabel.Location = new System.Drawing.Point(216, 12);
			this.instructionsLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.instructionsLabel.Name = "instructionsLabel";
			this.instructionsLabel.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.instructionsLabel.Size = new System.Drawing.Size(67, 13);
			this.instructionsLabel.TabIndex = 22;
			this.instructionsLabel.Text = "Instructions:";
			// 
			// combineButton
			// 
			this.combineButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.combineButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.combineButton.Location = new System.Drawing.Point(12, 369);
			this.combineButton.Name = "combineButton";
			this.combineButton.Size = new System.Drawing.Size(103, 23);
			this.combineButton.TabIndex = 23;
			this.combineButton.Text = "Combine";
			this.combineButton.Click += new System.EventHandler(this.CombineButton_Click);
			// 
			// delayLabel
			// 
			this.delayLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.delayLabel.AutoSize = true;
			this.delayLabel.Location = new System.Drawing.Point(12, 327);
			this.delayLabel.Name = "delayLabel";
			this.delayLabel.Size = new System.Drawing.Size(37, 13);
			this.delayLabel.TabIndex = 25;
			this.delayLabel.Text = "Delay:";
			// 
			// delayNumeric
			// 
			this.delayNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.delayNumeric.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.delayNumeric.Location = new System.Drawing.Point(15, 343);
			this.delayNumeric.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
			this.delayNumeric.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
			this.delayNumeric.Minimum = new decimal(new int[] {
            10,
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
			this.delayNumeric.ValueChanged += new System.EventHandler(this.DelayNumeric_ValueChanged);
			// 
			// stepLabel
			// 
			this.stepLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.stepLabel.AutoSize = true;
			this.stepLabel.Location = new System.Drawing.Point(81, 327);
			this.stepLabel.Name = "stepLabel";
			this.stepLabel.Size = new System.Drawing.Size(67, 13);
			this.stepLabel.TabIndex = 25;
			this.stepLabel.Text = "Start at step:";
			// 
			// stepNumeric
			// 
			this.stepNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.stepNumeric.Location = new System.Drawing.Point(84, 343);
			this.stepNumeric.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
			this.stepNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.stepNumeric.Name = "stepNumeric";
			this.stepNumeric.Size = new System.Drawing.Size(57, 20);
			this.stepNumeric.TabIndex = 26;
			this.stepNumeric.ValueChanged += new System.EventHandler(this.StepNumeric_ValueChanged);
			// 
			// helpButton
			// 
			this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.helpButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.helpButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.helpButton.Location = new System.Drawing.Point(300, 340);
			this.helpButton.Name = "helpButton";
			this.helpButton.Size = new System.Drawing.Size(68, 23);
			this.helpButton.TabIndex = 28;
			this.helpButton.Text = "Help";
			this.helpButton.Click += new System.EventHandler(this.HelpButton_Click);
			// 
			// exitButton
			// 
			this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.exitButton.Location = new System.Drawing.Point(300, 369);
			this.exitButton.Name = "exitButton";
			this.exitButton.Size = new System.Drawing.Size(68, 23);
			this.exitButton.TabIndex = 31;
			this.exitButton.Text = "Exit";
			this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
			// 
			// optionsButton
			// 
			this.optionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.optionsButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.optionsButton.Location = new System.Drawing.Point(219, 340);
			this.optionsButton.Name = "optionsButton";
			this.optionsButton.Size = new System.Drawing.Size(75, 23);
			this.optionsButton.TabIndex = 32;
			this.optionsButton.Text = "Options";
			this.optionsButton.Click += new System.EventHandler(this.OptionsButton_Click);
			// 
			// combineComboBox
			// 
			this.combineComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.combineComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.combineComboBox.Font = new System.Drawing.Font("Consolas", 9.75F);
			this.combineComboBox.Items.AddRange(new object[] {
            "Paste"});
			this.combineComboBox.Location = new System.Drawing.Point(12, 57);
			this.combineComboBox.Name = "combineComboBox";
			this.combineComboBox.Size = new System.Drawing.Size(170, 23);
			this.combineComboBox.Sorted = true;
			this.combineComboBox.TabIndex = 33;
			this.combineComboBox.SelectedIndexChanged += new System.EventHandler(this.CombineComboBox_SelectedIndexChanged);
			// 
			// colorComboBox
			// 
			this.colorComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.colorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.colorComboBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.colorComboBox.Location = new System.Drawing.Point(12, 28);
			this.colorComboBox.Name = "colorComboBox";
			this.colorComboBox.Size = new System.Drawing.Size(170, 23);
			this.colorComboBox.Sorted = true;
			this.colorComboBox.TabIndex = 34;
			this.colorComboBox.SelectedIndexChanged += new System.EventHandler(this.ColorComboBox_SelectedIndexChanged);
			// 
			// importFileDialog
			// 
			this.importFileDialog.Filter = "Text files|*.txt";
			// 
			// gemLocationsLabel
			// 
			this.gemLocationsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.gemLocationsLabel.AutoSize = true;
			this.gemLocationsLabel.Location = new System.Drawing.Point(216, 275);
			this.gemLocationsLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.gemLocationsLabel.Name = "gemLocationsLabel";
			this.gemLocationsLabel.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.gemLocationsLabel.Size = new System.Drawing.Size(84, 13);
			this.gemLocationsLabel.TabIndex = 36;
			this.gemLocationsLabel.Text = "Gem Locations:";
			// 
			// resultLabel
			// 
			this.resultLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.resultLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.resultLabel.Location = new System.Drawing.Point(12, 229);
			this.resultLabel.Margin = new System.Windows.Forms.Padding(3);
			this.resultLabel.Name = "resultLabel";
			this.resultLabel.Size = new System.Drawing.Size(170, 92);
			this.resultLabel.TabIndex = 39;
			this.resultLabel.Text = "Result Here\r\n(Grade + Cost)";
			// 
			// formulaInputRichTextBox
			// 
			this.formulaInputRichTextBox.AcceptsTab = true;
			this.formulaInputRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.formulaInputRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.formulaInputRichTextBox.Location = new System.Drawing.Point(12, 87);
			this.formulaInputRichTextBox.Name = "formulaInputRichTextBox";
			this.formulaInputRichTextBox.Size = new System.Drawing.Size(170, 108);
			this.formulaInputRichTextBox.TabIndex = 40;
			this.formulaInputRichTextBox.Text = "";
			// 
			// baseGemsListBox
			// 
			this.baseGemsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.baseGemsListBox.FormattingEnabled = true;
			this.baseGemsListBox.Location = new System.Drawing.Point(219, 291);
			this.baseGemsListBox.Name = "baseGemsListBox";
			this.baseGemsListBox.Size = new System.Drawing.Size(149, 43);
			this.baseGemsListBox.TabIndex = 42;
			// 
			// slotLimitUpDown
			// 
			this.slotLimitUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.slotLimitUpDown.Location = new System.Drawing.Point(153, 343);
			this.slotLimitUpDown.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
			this.slotLimitUpDown.Maximum = new decimal(new int[] {
            36,
            0,
            0,
            0});
			this.slotLimitUpDown.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.slotLimitUpDown.Name = "slotLimitUpDown";
			this.slotLimitUpDown.Size = new System.Drawing.Size(57, 20);
			this.slotLimitUpDown.TabIndex = 45;
			this.slotLimitUpDown.Value = new decimal(new int[] {
            36,
            0,
            0,
            0});
			this.slotLimitUpDown.ValueChanged += new System.EventHandler(this.SlotLimitUpDown_ValueChanged);
			// 
			// slotLimitLabel
			// 
			this.slotLimitLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.slotLimitLabel.AutoSize = true;
			this.slotLimitLabel.Location = new System.Drawing.Point(150, 327);
			this.slotLimitLabel.Name = "slotLimitLabel";
			this.slotLimitLabel.Size = new System.Drawing.Size(48, 13);
			this.slotLimitLabel.TabIndex = 44;
			this.slotLimitLabel.Text = "Slot limit:";
			// 
			// instructionsTextBox
			// 
			this.instructionsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.instructionsTextBox.BackColor = System.Drawing.SystemColors.Window;
			this.instructionsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.instructionsTextBox.DetectUrls = false;
			this.instructionsTextBox.Location = new System.Drawing.Point(219, 29);
			this.instructionsTextBox.Name = "instructionsTextBox";
			this.instructionsTextBox.ReadOnly = true;
			this.instructionsTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.instructionsTextBox.Size = new System.Drawing.Size(149, 238);
			this.instructionsTextBox.TabIndex = 46;
			this.instructionsTextBox.Text = "";
			// 
			// combineProgressBar
			// 
			this.combineProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.combineProgressBar.BackColor = System.Drawing.SystemColors.Window;
			this.combineProgressBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.combineProgressBar.Location = new System.Drawing.Point(122, 369);
			this.combineProgressBar.Minimum = 1;
			this.combineProgressBar.Name = "combineProgressBar";
			this.combineProgressBar.Size = new System.Drawing.Size(172, 23);
			this.combineProgressBar.Step = 1;
			this.combineProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.combineProgressBar.TabIndex = 43;
			this.combineProgressBar.Text = null;
			this.combineProgressBar.TextColor = System.Drawing.SystemColors.ControlText;
			this.combineProgressBar.Value = 1;
			// 
			// GemCombiner
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(380, 404);
			this.Controls.Add(this.slotLimitUpDown);
			this.Controls.Add(this.slotLimitLabel);
			this.Controls.Add(this.combineProgressBar);
			this.Controls.Add(this.baseGemsListBox);
			this.Controls.Add(this.formulaInputRichTextBox);
			this.Controls.Add(this.resultLabel);
			this.Controls.Add(this.gemLocationsLabel);
			this.Controls.Add(this.combineComboBox);
			this.Controls.Add(this.colorComboBox);
			this.Controls.Add(this.optionsButton);
			this.Controls.Add(this.exitButton);
			this.Controls.Add(this.helpButton);
			this.Controls.Add(this.stepNumeric);
			this.Controls.Add(this.delayNumeric);
			this.Controls.Add(this.stepLabel);
			this.Controls.Add(this.delayLabel);
			this.Controls.Add(this.combineButton);
			this.Controls.Add(this.instructionsLabel);
			this.Controls.Add(this.parseRecipeButton);
			this.Controls.Add(this.schemeLabel);
			this.Controls.Add(this.instructionsTextBox);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(396, 392);
			this.Name = "GemCombiner";
			this.Text = "Gem Combiner";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GemCombiner_FormClosing);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GemCombiner_MouseDown);
			((System.ComponentModel.ISupportInitialize)(this.delayNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.stepNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.slotLimitUpDown)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label schemeLabel;
		private System.Windows.Forms.Button parseRecipeButton;
		private System.Windows.Forms.Label instructionsLabel;
		private System.Windows.Forms.Button combineButton;
		private System.Windows.Forms.Label delayLabel;
		private System.Windows.Forms.NumericUpDown delayNumeric;
		private System.Windows.Forms.Label stepLabel;
		private System.Windows.Forms.NumericUpDown stepNumeric;
		private System.Windows.Forms.Button helpButton;
		private System.Windows.Forms.Button exitButton;
		private System.Windows.Forms.Button optionsButton;
		private System.Windows.Forms.ComboBox combineComboBox;
		private System.Windows.Forms.ComboBox colorComboBox;
		private System.Windows.Forms.OpenFileDialog importFileDialog;
		private System.Windows.Forms.Label gemLocationsLabel;
		private System.Windows.Forms.Label resultLabel;
		private System.Windows.Forms.RichTextBox formulaInputRichTextBox;
		private System.Windows.Forms.ListBox baseGemsListBox;
		private WGemCombiner.TextProgressBar combineProgressBar;
		private System.Windows.Forms.NumericUpDown slotLimitUpDown;
		private System.Windows.Forms.Label slotLimitLabel;
		private System.Windows.Forms.RichTextBox instructionsTextBox;
	}
}