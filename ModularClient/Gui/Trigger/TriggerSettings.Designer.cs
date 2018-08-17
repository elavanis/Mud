namespace Client.Trigger
{
    partial class TriggerSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TriggerSettings));
            this.label_Trigger = new System.Windows.Forms.Label();
            this.comboBox_TriggerName = new System.Windows.Forms.ComboBox();
            this.label_TagType = new System.Windows.Forms.Label();
            this.comboBox_TagType = new System.Windows.Forms.ComboBox();
            this.groupBox_Trigger = new System.Windows.Forms.GroupBox();
            this.richTextBox_Code = new System.Windows.Forms.RichTextBox();
            this.button_Save = new System.Windows.Forms.Button();
            this.label_Code = new System.Windows.Forms.Label();
            this.textBox_Regex = new System.Windows.Forms.TextBox();
            this.label_Regex = new System.Windows.Forms.Label();
            this.textBox_Name = new System.Windows.Forms.TextBox();
            this.label_TriggerName = new System.Windows.Forms.Label();
            this.button_New = new System.Windows.Forms.Button();
            this.button_Delete = new System.Windows.Forms.Button();
            this.groupBox_Trigger.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_Trigger
            // 
            this.label_Trigger.AutoSize = true;
            this.label_Trigger.Location = new System.Drawing.Point(23, 9);
            this.label_Trigger.Name = "label_Trigger";
            this.label_Trigger.Size = new System.Drawing.Size(40, 13);
            this.label_Trigger.TabIndex = 7;
            this.label_Trigger.Text = "Trigger";
            // 
            // comboBox_TriggerName
            // 
            this.comboBox_TriggerName.FormattingEnabled = true;
            this.comboBox_TriggerName.Location = new System.Drawing.Point(98, 6);
            this.comboBox_TriggerName.Name = "comboBox_TriggerName";
            this.comboBox_TriggerName.Size = new System.Drawing.Size(121, 21);
            this.comboBox_TriggerName.TabIndex = 6;
            this.comboBox_TriggerName.SelectedIndexChanged += new System.EventHandler(this.SetSelectedItem);
            // 
            // label_TagType
            // 
            this.label_TagType.AutoSize = true;
            this.label_TagType.Location = new System.Drawing.Point(326, 25);
            this.label_TagType.Name = "label_TagType";
            this.label_TagType.Size = new System.Drawing.Size(67, 13);
            this.label_TagType.TabIndex = 5;
            this.label_TagType.Text = "Trigger Type";
            // 
            // comboBox_TagType
            // 
            this.comboBox_TagType.FormattingEnabled = true;
            this.comboBox_TagType.Location = new System.Drawing.Point(403, 22);
            this.comboBox_TagType.Name = "comboBox_TagType";
            this.comboBox_TagType.Size = new System.Drawing.Size(121, 21);
            this.comboBox_TagType.TabIndex = 4;
            // 
            // groupBox_Trigger
            // 
            this.groupBox_Trigger.Controls.Add(this.richTextBox_Code);
            this.groupBox_Trigger.Controls.Add(this.button_Save);
            this.groupBox_Trigger.Controls.Add(this.label_Code);
            this.groupBox_Trigger.Controls.Add(this.textBox_Regex);
            this.groupBox_Trigger.Controls.Add(this.label_Regex);
            this.groupBox_Trigger.Controls.Add(this.textBox_Name);
            this.groupBox_Trigger.Controls.Add(this.label_TriggerName);
            this.groupBox_Trigger.Controls.Add(this.label_TagType);
            this.groupBox_Trigger.Controls.Add(this.comboBox_TagType);
            this.groupBox_Trigger.Location = new System.Drawing.Point(15, 45);
            this.groupBox_Trigger.Name = "groupBox_Trigger";
            this.groupBox_Trigger.Size = new System.Drawing.Size(537, 304);
            this.groupBox_Trigger.TabIndex = 8;
            this.groupBox_Trigger.TabStop = false;
            this.groupBox_Trigger.Text = "Trigger";
            // 
            // richTextBox_Code
            // 
            this.richTextBox_Code.Location = new System.Drawing.Point(83, 75);
            this.richTextBox_Code.Name = "richTextBox_Code";
            this.richTextBox_Code.Size = new System.Drawing.Size(441, 194);
            this.richTextBox_Code.TabIndex = 14;
            this.richTextBox_Code.Text = resources.GetString("richTextBox_Code.Text");
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(230, 275);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(75, 23);
            this.button_Save.TabIndex = 13;
            this.button_Save.Text = "Save";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // label_Code
            // 
            this.label_Code.AutoSize = true;
            this.label_Code.Location = new System.Drawing.Point(8, 78);
            this.label_Code.Name = "label_Code";
            this.label_Code.Size = new System.Drawing.Size(32, 13);
            this.label_Code.TabIndex = 11;
            this.label_Code.Text = "Code";
            // 
            // textBox_Regex
            // 
            this.textBox_Regex.Location = new System.Drawing.Point(83, 49);
            this.textBox_Regex.Name = "textBox_Regex";
            this.textBox_Regex.Size = new System.Drawing.Size(441, 20);
            this.textBox_Regex.TabIndex = 10;
            // 
            // label_Regex
            // 
            this.label_Regex.AutoSize = true;
            this.label_Regex.Location = new System.Drawing.Point(8, 52);
            this.label_Regex.Name = "label_Regex";
            this.label_Regex.Size = new System.Drawing.Size(38, 13);
            this.label_Regex.TabIndex = 9;
            this.label_Regex.Text = "Regex";
            // 
            // textBox_Name
            // 
            this.textBox_Name.Location = new System.Drawing.Point(83, 22);
            this.textBox_Name.Name = "textBox_Name";
            this.textBox_Name.Size = new System.Drawing.Size(140, 20);
            this.textBox_Name.TabIndex = 8;
            // 
            // label_TriggerName
            // 
            this.label_TriggerName.AutoSize = true;
            this.label_TriggerName.Location = new System.Drawing.Point(6, 25);
            this.label_TriggerName.Name = "label_TriggerName";
            this.label_TriggerName.Size = new System.Drawing.Size(71, 13);
            this.label_TriggerName.TabIndex = 7;
            this.label_TriggerName.Text = "Trigger Name";
            // 
            // button_New
            // 
            this.button_New.Location = new System.Drawing.Point(245, 4);
            this.button_New.Name = "button_New";
            this.button_New.Size = new System.Drawing.Size(75, 23);
            this.button_New.TabIndex = 14;
            this.button_New.Text = "New";
            this.button_New.UseVisualStyleBackColor = true;
            this.button_New.Click += new System.EventHandler(this.button_New_Click);
            // 
            // button_Delete
            // 
            this.button_Delete.Location = new System.Drawing.Point(344, 4);
            this.button_Delete.Name = "button_Delete";
            this.button_Delete.Size = new System.Drawing.Size(75, 23);
            this.button_Delete.TabIndex = 15;
            this.button_Delete.Text = "Delete";
            this.button_Delete.UseVisualStyleBackColor = true;
            this.button_Delete.Click += new System.EventHandler(this.button_Delete_Click);
            // 
            // TriggerSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 361);
            this.Controls.Add(this.button_Delete);
            this.Controls.Add(this.button_New);
            this.Controls.Add(this.groupBox_Trigger);
            this.Controls.Add(this.label_Trigger);
            this.Controls.Add(this.comboBox_TriggerName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TriggerSettings";
            this.Text = "TriggerSettings";
            this.groupBox_Trigger.ResumeLayout(false);
            this.groupBox_Trigger.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Trigger;
        private System.Windows.Forms.ComboBox comboBox_TriggerName;
        private System.Windows.Forms.Label label_TagType;
        private System.Windows.Forms.ComboBox comboBox_TagType;
        private System.Windows.Forms.GroupBox groupBox_Trigger;
        private System.Windows.Forms.Label label_Code;
        private System.Windows.Forms.TextBox textBox_Regex;
        private System.Windows.Forms.Label label_Regex;
        private System.Windows.Forms.TextBox textBox_Name;
        private System.Windows.Forms.Label label_TriggerName;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_New;
        private System.Windows.Forms.Button button_Delete;
        private System.Windows.Forms.RichTextBox richTextBox_Code;
    }
}