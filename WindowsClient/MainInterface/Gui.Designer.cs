namespace WindowsClient.MainInterface
{
    partial class Gui
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Gui));
            this.menuStrip_Menu = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.soundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox_Intelisense = new System.Windows.Forms.TextBox();
            this.textBox_CommandBox = new System.Windows.Forms.TextBox();
            this.timer_UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.myRichTextBox_MainText = new WindowsClient.MainInterface.MyRichTextBox();
            this.triggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip_Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip_Menu
            // 
            this.menuStrip_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip_Menu.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_Menu.Name = "menuStrip_Menu";
            this.menuStrip_Menu.Size = new System.Drawing.Size(624, 24);
            this.menuStrip_Menu.TabIndex = 1;
            this.menuStrip_Menu.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.guiToolStripMenuItem,
            this.mapToolStripMenuItem,
            this.soundToolStripMenuItem,
            this.triggerToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // guiToolStripMenuItem
            // 
            this.guiToolStripMenuItem.Name = "guiToolStripMenuItem";
            this.guiToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.guiToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.guiToolStripMenuItem.Text = "Gui";
            this.guiToolStripMenuItem.Click += new System.EventHandler(this.GuiSettings);
            // 
            // mapToolStripMenuItem
            // 
            this.mapToolStripMenuItem.Name = "mapToolStripMenuItem";
            this.mapToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.mapToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.mapToolStripMenuItem.Text = "Map";
            this.mapToolStripMenuItem.Click += new System.EventHandler(this.ToggleMap);
            // 
            // soundToolStripMenuItem
            // 
            this.soundToolStripMenuItem.Name = "soundToolStripMenuItem";
            this.soundToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.soundToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.soundToolStripMenuItem.Text = "Sound";
            this.soundToolStripMenuItem.Click += new System.EventHandler(this.ToggleSound);
            // 
            // textBox_Intelisense
            // 
            this.textBox_Intelisense.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Intelisense.Location = new System.Drawing.Point(12, 383);
            this.textBox_Intelisense.Name = "textBox_Intelisense";
            this.textBox_Intelisense.ReadOnly = true;
            this.textBox_Intelisense.Size = new System.Drawing.Size(600, 20);
            this.textBox_Intelisense.TabIndex = 2;
            this.textBox_Intelisense.Click += new System.EventHandler(this.SelectTextEntry);
            // 
            // textBox_CommandBox
            // 
            this.textBox_CommandBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_CommandBox.Location = new System.Drawing.Point(12, 409);
            this.textBox_CommandBox.Name = "textBox_CommandBox";
            this.textBox_CommandBox.Size = new System.Drawing.Size(600, 20);
            this.textBox_CommandBox.TabIndex = 3;
            this.textBox_CommandBox.TextChanged += new System.EventHandler(this.Intelisense);
            this.textBox_CommandBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CatchTabAutoComplete);
            this.textBox_CommandBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.CatchTabAutoComplete);
            // 
            // timer_UpdateTimer
            // 
            this.timer_UpdateTimer.Tick += new System.EventHandler(this.UpdateGui);
            // 
            // myRichTextBox_MainText
            // 
            this.myRichTextBox_MainText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myRichTextBox_MainText.BackColor = System.Drawing.Color.Black;
            this.myRichTextBox_MainText.Font = new System.Drawing.Font("Lucida Console", 8.25F);
            this.myRichTextBox_MainText.ForeColor = System.Drawing.Color.White;
            this.myRichTextBox_MainText.HideSelection = false;
            this.myRichTextBox_MainText.Location = new System.Drawing.Point(12, 27);
            this.myRichTextBox_MainText.Name = "myRichTextBox_MainText";
            this.myRichTextBox_MainText.Size = new System.Drawing.Size(600, 350);
            this.myRichTextBox_MainText.TabIndex = 0;
            this.myRichTextBox_MainText.Text = "";
            this.myRichTextBox_MainText.Click += new System.EventHandler(this.SelectTextEntry);
            this.myRichTextBox_MainText.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.CatchTypeInMainScreen);
            // 
            // triggerToolStripMenuItem
            // 
            this.triggerToolStripMenuItem.Name = "triggerToolStripMenuItem";
            this.triggerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.triggerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.triggerToolStripMenuItem.Text = "Trigger";
            this.triggerToolStripMenuItem.Click += new System.EventHandler(this.TriggerSettings);
            // 
            // GraphicalUserInterFace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.textBox_CommandBox);
            this.Controls.Add(this.textBox_Intelisense);
            this.Controls.Add(this.myRichTextBox_MainText);
            this.Controls.Add(this.menuStrip_Menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip_Menu;
            this.Name = "GraphicalUserInterFace";
            this.Text = "Eternal Realms";
            this.Load += new System.EventHandler(this.LoadForm);
            this.menuStrip_Menu.ResumeLayout(false);
            this.menuStrip_Menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MyRichTextBox myRichTextBox_MainText;
        private System.Windows.Forms.MenuStrip menuStrip_Menu;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem soundToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox_Intelisense;
        private System.Windows.Forms.TextBox textBox_CommandBox;
        private System.Windows.Forms.Timer timer_UpdateTimer;
        private System.Windows.Forms.ToolStripMenuItem triggerToolStripMenuItem;
    }
}