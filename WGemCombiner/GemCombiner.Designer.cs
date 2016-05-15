namespace WGemCombiner
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
            this.parseRecipeParButton = new System.Windows.Forms.Button();
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
            this.gemLocationsLabel = new System.Windows.Forms.Label();
            this.resultLabel = new System.Windows.Forms.Label();
            this.recipeInputRichTextBox = new System.Windows.Forms.RichTextBox();
            this.baseGemsListBox = new System.Windows.Forms.ListBox();
            this.slotLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.slotLimitLabel = new System.Windows.Forms.Label();
            this.instructionsTextBox = new System.Windows.Forms.RichTextBox();
            this.testAllButton = new System.Windows.Forms.Button();
            this.parseRecipeEqsButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.combineProgressBar = new WGemCombiner.TextProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.delayNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slotLimitNumeric)).BeginInit();
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
            // parseRecipeParButton
            // 
            this.parseRecipeParButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.parseRecipeParButton.AutoSize = true;
            this.parseRecipeParButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.parseRecipeParButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.parseRecipeParButton.Location = new System.Drawing.Point(12, 204);
            this.parseRecipeParButton.Name = "parseRecipeParButton";
            this.parseRecipeParButton.Size = new System.Drawing.Size(90, 22);
            this.parseRecipeParButton.TabIndex = 2;
            this.parseRecipeParButton.Text = "Parenthesis";
            this.parseRecipeParButton.Click += new System.EventHandler(this.ParseRecipeButton_Click);
            // 
            // instructionsLabel
            // 
            this.instructionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.instructionsLabel.AutoSize = true;
            this.instructionsLabel.Location = new System.Drawing.Point(204, 12);
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
            this.combineButton.Location = new System.Drawing.Point(12, 367);
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
            this.delayLabel.Location = new System.Drawing.Point(12, 325);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(37, 13);
            this.delayLabel.TabIndex = 25;
            this.delayLabel.Text = "Delay:";
            // 
            // delayNumeric
            // 
            this.delayNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.delayNumeric.ImeMode = System.Windows.Forms.ImeMode.On;
            this.delayNumeric.Location = new System.Drawing.Point(15, 341);
            this.delayNumeric.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.delayNumeric.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.delayNumeric.Name = "delayNumeric";
            this.delayNumeric.Size = new System.Drawing.Size(46, 20);
            this.delayNumeric.TabIndex = 26;
            this.delayNumeric.ValueChanged += new System.EventHandler(this.DelayNumeric_ValueChanged);
            this.delayNumeric.Validating += new System.ComponentModel.CancelEventHandler(this.NumericUpDown_Validating);
            // 
            // stepLabel
            // 
            this.stepLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stepLabel.AutoSize = true;
            this.stepLabel.Location = new System.Drawing.Point(70, 325);
            this.stepLabel.Name = "stepLabel";
            this.stepLabel.Size = new System.Drawing.Size(67, 13);
            this.stepLabel.TabIndex = 25;
            this.stepLabel.Text = "Start at step:";
            // 
            // stepNumeric
            // 
            this.stepNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stepNumeric.Location = new System.Drawing.Point(73, 341);
            this.stepNumeric.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.stepNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.stepNumeric.Name = "stepNumeric";
            this.stepNumeric.Size = new System.Drawing.Size(64, 20);
            this.stepNumeric.TabIndex = 26;
            this.stepNumeric.ValueChanged += new System.EventHandler(this.StepNumeric_ValueChanged);
            this.stepNumeric.Validating += new System.ComponentModel.CancelEventHandler(this.NumericUpDown_Validating);
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.helpButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.helpButton.Location = new System.Drawing.Point(288, 338);
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
            this.exitButton.Location = new System.Drawing.Point(288, 367);
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
            this.optionsButton.Location = new System.Drawing.Point(207, 338);
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
            this.combineComboBox.DropDownHeight = 242;
            this.combineComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combineComboBox.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.combineComboBox.IntegralHeight = false;
            this.combineComboBox.Items.AddRange(new object[] {
            "Paste"});
            this.combineComboBox.Location = new System.Drawing.Point(12, 57);
            this.combineComboBox.Name = "combineComboBox";
            this.combineComboBox.Size = new System.Drawing.Size(186, 23);
            this.combineComboBox.Sorted = true;
            this.combineComboBox.TabIndex = 33;
            this.combineComboBox.SelectedIndexChanged += new System.EventHandler(this.CombineComboBox_SelectedIndexChanged);
            // 
            // colorComboBox
            // 
            this.colorComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.colorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorComboBox.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.colorComboBox.Location = new System.Drawing.Point(12, 28);
            this.colorComboBox.Name = "colorComboBox";
            this.colorComboBox.Size = new System.Drawing.Size(186, 23);
            this.colorComboBox.Sorted = true;
            this.colorComboBox.TabIndex = 34;
            this.colorComboBox.SelectedIndexChanged += new System.EventHandler(this.ColorComboBox_SelectedIndexChanged);
            // 
            // gemLocationsLabel
            // 
            this.gemLocationsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gemLocationsLabel.AutoSize = true;
            this.gemLocationsLabel.Location = new System.Drawing.Point(204, 273);
            this.gemLocationsLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.gemLocationsLabel.Name = "gemLocationsLabel";
            this.gemLocationsLabel.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.gemLocationsLabel.Size = new System.Drawing.Size(84, 13);
            this.gemLocationsLabel.TabIndex = 36;
            this.gemLocationsLabel.Text = "Gem Locations:";
            // 
            // resultLabel
            // 
            this.resultLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultLabel.Font = new System.Drawing.Font("Consolas", 9F);
            this.resultLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.resultLabel.Location = new System.Drawing.Point(12, 232);
            this.resultLabel.Margin = new System.Windows.Forms.Padding(3);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(189, 87);
            this.resultLabel.TabIndex = 39;
            this.resultLabel.Text = "Result Here\r\n|\r\n|\r\n|\r\n|\r\n|";
            // 
            // recipeInputRichTextBox
            // 
            this.recipeInputRichTextBox.AcceptsTab = true;
            this.recipeInputRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.recipeInputRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.recipeInputRichTextBox.Location = new System.Drawing.Point(12, 86);
            this.recipeInputRichTextBox.Name = "recipeInputRichTextBox";
            this.recipeInputRichTextBox.Size = new System.Drawing.Size(186, 98);
            this.recipeInputRichTextBox.TabIndex = 40;
            this.recipeInputRichTextBox.Text = "";
            // 
            // baseGemsListBox
            // 
            this.baseGemsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.baseGemsListBox.FormattingEnabled = true;
            this.baseGemsListBox.Location = new System.Drawing.Point(207, 289);
            this.baseGemsListBox.Name = "baseGemsListBox";
            this.baseGemsListBox.Size = new System.Drawing.Size(149, 43);
            this.baseGemsListBox.TabIndex = 42;
            // 
            // slotLimitNumeric
            // 
            this.slotLimitNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.slotLimitNumeric.Location = new System.Drawing.Point(149, 341);
            this.slotLimitNumeric.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.slotLimitNumeric.Maximum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.slotLimitNumeric.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.slotLimitNumeric.Name = "slotLimitNumeric";
            this.slotLimitNumeric.Size = new System.Drawing.Size(45, 20);
            this.slotLimitNumeric.TabIndex = 45;
            this.slotLimitNumeric.Value = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.slotLimitNumeric.ValueChanged += new System.EventHandler(this.SlotLimitNumeric_ValueChanged);
            this.slotLimitNumeric.Validating += new System.ComponentModel.CancelEventHandler(this.NumericUpDown_Validating);
            // 
            // slotLimitLabel
            // 
            this.slotLimitLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.slotLimitLabel.AutoSize = true;
            this.slotLimitLabel.Location = new System.Drawing.Point(146, 325);
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
            this.instructionsTextBox.Location = new System.Drawing.Point(207, 29);
            this.instructionsTextBox.Name = "instructionsTextBox";
            this.instructionsTextBox.ReadOnly = true;
            this.instructionsTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.instructionsTextBox.Size = new System.Drawing.Size(149, 236);
            this.instructionsTextBox.TabIndex = 46;
            this.instructionsTextBox.Text = "";
            // 
            // testAllButton
            // 
            this.testAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.testAllButton.AutoSize = true;
            this.testAllButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.testAllButton.Location = new System.Drawing.Point(304, 2);
            this.testAllButton.Name = "testAllButton";
            this.testAllButton.Size = new System.Drawing.Size(52, 23);
            this.testAllButton.TabIndex = 47;
            this.testAllButton.Text = "Test All";
            this.testAllButton.UseVisualStyleBackColor = true;
            this.testAllButton.Click += new System.EventHandler(this.TestAll_Click);
            // 
            // parseRecipeEqsButton
            // 
            this.parseRecipeEqsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.parseRecipeEqsButton.AutoSize = true;
            this.parseRecipeEqsButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.parseRecipeEqsButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.parseRecipeEqsButton.Location = new System.Drawing.Point(108, 204);
            this.parseRecipeEqsButton.Name = "parseRecipeEqsButton";
            this.parseRecipeEqsButton.Size = new System.Drawing.Size(90, 22);
            this.parseRecipeEqsButton.TabIndex = 48;
            this.parseRecipeEqsButton.Tag = "true";
            this.parseRecipeEqsButton.Text = "Equations";
            this.parseRecipeEqsButton.Click += new System.EventHandler(this.ParseRecipeButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 190);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 49;
            this.label1.Text = "Parse recipe and show:";
            // 
            // combineProgressBar
            // 
            this.combineProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.combineProgressBar.BackColor = System.Drawing.SystemColors.Window;
            this.combineProgressBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combineProgressBar.Location = new System.Drawing.Point(121, 367);
            this.combineProgressBar.Minimum = 1;
            this.combineProgressBar.Name = "combineProgressBar";
            this.combineProgressBar.Size = new System.Drawing.Size(161, 23);
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
            this.ClientSize = new System.Drawing.Size(376, 402);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.parseRecipeEqsButton);
            this.Controls.Add(this.testAllButton);
            this.Controls.Add(this.slotLimitNumeric);
            this.Controls.Add(this.slotLimitLabel);
            this.Controls.Add(this.combineProgressBar);
            this.Controls.Add(this.baseGemsListBox);
            this.Controls.Add(this.recipeInputRichTextBox);
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
            this.Controls.Add(this.parseRecipeParButton);
            this.Controls.Add(this.schemeLabel);
            this.Controls.Add(this.instructionsTextBox);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(392, 392);
            this.Name = "GemCombiner";
            this.Text = "Gem Combiner 1.1.90";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GemCombiner_FormClosing);
            this.Shown += new System.EventHandler(this.GemCombiner_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GemCombiner_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.delayNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slotLimitNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label schemeLabel;
        private System.Windows.Forms.Button parseRecipeParButton;
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
        private System.Windows.Forms.Label gemLocationsLabel;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.RichTextBox recipeInputRichTextBox;
        private System.Windows.Forms.ListBox baseGemsListBox;
        private WGemCombiner.TextProgressBar combineProgressBar;
        private System.Windows.Forms.NumericUpDown slotLimitNumeric;
        private System.Windows.Forms.Label slotLimitLabel;
        private System.Windows.Forms.RichTextBox instructionsTextBox;
        private System.Windows.Forms.Button testAllButton;
        private System.Windows.Forms.Button parseRecipeEqsButton;
        private System.Windows.Forms.Label label1;
    }
}
