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
			this.getInstructionsButton = new System.Windows.Forms.Button();
			this.instructionsListBox = new System.Windows.Forms.ListBox();
			this.instructionsLabel = new System.Windows.Forms.Label();
			this.combineButton = new System.Windows.Forms.Button();
			this.delayLabel = new System.Windows.Forms.Label();
			this.delayNumeric = new System.Windows.Forms.NumericUpDown();
			this.stepLabel = new System.Windows.Forms.Label();
			this.stepNumeric = new System.Windows.Forms.NumericUpDown();
			this.copyListButton = new System.Windows.Forms.Button();
			this.helpButton = new System.Windows.Forms.Button();
			this.exitButton = new System.Windows.Forms.Button();
			this.optionsButton = new System.Windows.Forms.Button();
			this.combineComboBox = new System.Windows.Forms.ComboBox();
			this.colorComboBox = new System.Windows.Forms.ComboBox();
			this.importFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.gemLocationsLabel = new System.Windows.Forms.Label();
			this.baseGemsTextBox = new System.Windows.Forms.TextBox();
			this.resultLabel = new System.Windows.Forms.Label();
			this.formulaInputRichTextBox = new System.Windows.Forms.RichTextBox();
			((System.ComponentModel.ISupportInitialize)(this.delayNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.stepNumeric)).BeginInit();
			this.SuspendLayout();
			// 
			// schemeLabel
			// 
			resources.ApplyResources(this.schemeLabel, "schemeLabel");
			this.schemeLabel.Name = "schemeLabel";
			// 
			// getInstructionsButton
			// 
			resources.ApplyResources(this.getInstructionsButton, "getInstructionsButton");
			this.getInstructionsButton.Name = "getInstructionsButton";
			this.getInstructionsButton.Click += new System.EventHandler(this.GetInstructionsButton_Click);
			// 
			// instructionsListBox
			// 
			resources.ApplyResources(this.instructionsListBox, "instructionsListBox");
			this.instructionsListBox.FormattingEnabled = true;
			this.instructionsListBox.Name = "instructionsListBox";
			// 
			// instructionsLabel
			// 
			resources.ApplyResources(this.instructionsLabel, "instructionsLabel");
			this.instructionsLabel.Name = "instructionsLabel";
			// 
			// combineButton
			// 
			resources.ApplyResources(this.combineButton, "combineButton");
			this.combineButton.Name = "combineButton";
			this.combineButton.Click += new System.EventHandler(this.CombineButton_Click);
			// 
			// delayLabel
			// 
			resources.ApplyResources(this.delayLabel, "delayLabel");
			this.delayLabel.Name = "delayLabel";
			// 
			// delayNumeric
			// 
			resources.ApplyResources(this.delayNumeric, "delayNumeric");
			this.delayNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.delayNumeric.Name = "delayNumeric";
			this.delayNumeric.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.delayNumeric.ValueChanged += new System.EventHandler(this.DelayNumeric_ValueChanged);
			// 
			// stepLabel
			// 
			resources.ApplyResources(this.stepLabel, "stepLabel");
			this.stepLabel.Name = "stepLabel";
			// 
			// stepNumeric
			// 
			resources.ApplyResources(this.stepNumeric, "stepNumeric");
			this.stepNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.stepNumeric.Name = "stepNumeric";
			this.stepNumeric.ValueChanged += new System.EventHandler(this.StepNumeric_ValueChanged);
			// 
			// copyListButton
			// 
			resources.ApplyResources(this.copyListButton, "copyListButton");
			this.copyListButton.Name = "copyListButton";
			this.copyListButton.UseVisualStyleBackColor = false;
			this.copyListButton.Click += new System.EventHandler(this.CopyList_Click);
			// 
			// helpButton
			// 
			resources.ApplyResources(this.helpButton, "helpButton");
			this.helpButton.Name = "helpButton";
			this.helpButton.Click += new System.EventHandler(this.HelpButton_Click);
			// 
			// exitButton
			// 
			resources.ApplyResources(this.exitButton, "exitButton");
			this.exitButton.Name = "exitButton";
			this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
			// 
			// optionsButton
			// 
			resources.ApplyResources(this.optionsButton, "optionsButton");
			this.optionsButton.Name = "optionsButton";
			this.optionsButton.Click += new System.EventHandler(this.OptionsButton_Click);
			// 
			// combineComboBox
			// 
			this.combineComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			resources.ApplyResources(this.combineComboBox, "combineComboBox");
			this.combineComboBox.Items.AddRange(new object[] {
            resources.GetString("combineComboBox.Items")});
			this.combineComboBox.Name = "combineComboBox";
			this.combineComboBox.SelectedIndexChanged += new System.EventHandler(this.CombineComboBox_SelectedIndexChanged);
			// 
			// colorComboBox
			// 
			this.colorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			resources.ApplyResources(this.colorComboBox, "colorComboBox");
			this.colorComboBox.Name = "colorComboBox";
			this.colorComboBox.SelectedIndexChanged += new System.EventHandler(this.ColorComboBox_SelectedIndexChanged);
			// 
			// importFileDialog
			// 
			resources.ApplyResources(this.importFileDialog, "importFileDialog");
			// 
			// gemLocationsLabel
			// 
			resources.ApplyResources(this.gemLocationsLabel, "gemLocationsLabel");
			this.gemLocationsLabel.Name = "gemLocationsLabel";
			// 
			// baseGemsTextBox
			// 
			resources.ApplyResources(this.baseGemsTextBox, "baseGemsTextBox");
			this.baseGemsTextBox.Name = "baseGemsTextBox";
			// 
			// resultLabel
			// 
			resources.ApplyResources(this.resultLabel, "resultLabel");
			this.resultLabel.Name = "resultLabel";
			// 
			// formulaInputRichTextBox
			// 
			this.formulaInputRichTextBox.AcceptsTab = true;
			this.formulaInputRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			resources.ApplyResources(this.formulaInputRichTextBox, "formulaInputRichTextBox");
			this.formulaInputRichTextBox.Name = "formulaInputRichTextBox";
			// 
			// GemCombiner
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.formulaInputRichTextBox);
			this.Controls.Add(this.resultLabel);
			this.Controls.Add(this.baseGemsTextBox);
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
			this.Controls.Add(this.instructionsListBox);
			this.Controls.Add(this.getInstructionsButton);
			this.Controls.Add(this.schemeLabel);
			this.Controls.Add(this.copyListButton);
			this.DoubleBuffered = true;
			this.MaximizeBox = false;
			this.Name = "GemCombiner";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GemCombiner_FormClosing);
			this.Load += new System.EventHandler(this.GemCombiner_Load);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GemCombiner_MouseDown);
			((System.ComponentModel.ISupportInitialize)(this.delayNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.stepNumeric)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label schemeLabel;
        private System.Windows.Forms.Button getInstructionsButton;
        private System.Windows.Forms.ListBox instructionsListBox;
        private System.Windows.Forms.Label instructionsLabel;
        private System.Windows.Forms.Button combineButton;
        private System.Windows.Forms.Label delayLabel;
        private System.Windows.Forms.NumericUpDown delayNumeric;
        private System.Windows.Forms.Label stepLabel;
        private System.Windows.Forms.NumericUpDown stepNumeric;
        private System.Windows.Forms.Button copyListButton;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button optionsButton;
        private System.Windows.Forms.ComboBox combineComboBox;
        private System.Windows.Forms.ComboBox colorComboBox;
        private System.Windows.Forms.OpenFileDialog importFileDialog;
        private System.Windows.Forms.Label gemLocationsLabel;
		private System.Windows.Forms.TextBox baseGemsTextBox;
		private System.Windows.Forms.Label resultLabel;
		private System.Windows.Forms.RichTextBox formulaInputRichTextBox;
	}
}

