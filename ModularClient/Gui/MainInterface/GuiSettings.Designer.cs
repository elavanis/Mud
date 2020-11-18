namespace Client.MainInterface
{
    partial class GuiSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GuiSettings));
            this.numericUpDown_FontSize = new System.Windows.Forms.NumericUpDown();
            this.label_FontSize = new System.Windows.Forms.Label();
            this.comboBox_ShortCutKey = new System.Windows.Forms.ComboBox();
            this.textBox_ShortCutCommand = new System.Windows.Forms.TextBox();
            this.label_ShortCutKey = new System.Windows.Forms.Label();
            this.label_ShortCutCommand = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_FontSize)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDown_FontSize
            // 
            this.numericUpDown_FontSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown_FontSize.Location = new System.Drawing.Point(152, 70);
            this.numericUpDown_FontSize.Name = "numericUpDown_FontSize";
            this.numericUpDown_FontSize.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown_FontSize.TabIndex = 0;
            this.numericUpDown_FontSize.ValueChanged += new System.EventHandler(this.ResizeScreen);
            // 
            // label_FontSize
            // 
            this.label_FontSize.AutoSize = true;
            this.label_FontSize.Location = new System.Drawing.Point(12, 72);
            this.label_FontSize.Name = "label_FontSize";
            this.label_FontSize.Size = new System.Drawing.Size(51, 13);
            this.label_FontSize.TabIndex = 1;
            this.label_FontSize.Text = "Font Size";
            // 
            // comboBox_ShortCutKey
            // 
            this.comboBox_ShortCutKey.FormattingEnabled = true;
            this.comboBox_ShortCutKey.Location = new System.Drawing.Point(15, 25);
            this.comboBox_ShortCutKey.Name = "comboBox_ShortCutKey";
            this.comboBox_ShortCutKey.Size = new System.Drawing.Size(121, 21);
            this.comboBox_ShortCutKey.TabIndex = 22;
            this.comboBox_ShortCutKey.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChanged);
            this.comboBox_ShortCutKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ComboBoxKeyDown);
            // 
            // textBox_ShortCutCommand
            // 
            this.textBox_ShortCutCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ShortCutCommand.Location = new System.Drawing.Point(152, 25);
            this.textBox_ShortCutCommand.Name = "textBox_ShortCutCommand";
            this.textBox_ShortCutCommand.Size = new System.Drawing.Size(120, 20);
            this.textBox_ShortCutCommand.TabIndex = 23;
            this.textBox_ShortCutCommand.TextChanged += new System.EventHandler(this.UpdateShortCutText);
            // 
            // label_ShortCutKey
            // 
            this.label_ShortCutKey.AutoSize = true;
            this.label_ShortCutKey.Location = new System.Drawing.Point(12, 9);
            this.label_ShortCutKey.Name = "label_ShortCutKey";
            this.label_ShortCutKey.Size = new System.Drawing.Size(72, 13);
            this.label_ShortCutKey.TabIndex = 24;
            this.label_ShortCutKey.Text = "Short Cut Key";
            // 
            // label_ShortCutCommand
            // 
            this.label_ShortCutCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_ShortCutCommand.AutoSize = true;
            this.label_ShortCutCommand.Location = new System.Drawing.Point(149, 9);
            this.label_ShortCutCommand.Name = "label_ShortCutCommand";
            this.label_ShortCutCommand.Size = new System.Drawing.Size(101, 13);
            this.label_ShortCutCommand.TabIndex = 25;
            this.label_ShortCutCommand.Text = "Short Cut Command";
            // 
            // GuiSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 116);
            this.Controls.Add(this.label_ShortCutCommand);
            this.Controls.Add(this.label_ShortCutKey);
            this.Controls.Add(this.textBox_ShortCutCommand);
            this.Controls.Add(this.comboBox_ShortCutKey);
            this.Controls.Add(this.label_FontSize);
            this.Controls.Add(this.numericUpDown_FontSize);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GuiSettings";
            this.Text = "GuiSettings";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_FontSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDown_FontSize;
        private System.Windows.Forms.Label label_FontSize;
        private System.Windows.Forms.ComboBox comboBox_ShortCutKey;
        private System.Windows.Forms.TextBox textBox_ShortCutCommand;
        private System.Windows.Forms.Label label_ShortCutKey;
        private System.Windows.Forms.Label label_ShortCutCommand;
    }
}