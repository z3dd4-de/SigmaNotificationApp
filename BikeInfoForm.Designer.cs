namespace SigmaNotificationApp
{
    partial class BikeInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BikeInfoForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.exitToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.readToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.modelLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.serialLabel = new System.Windows.Forms.Label();
            this.typeLabel = new System.Windows.Forms.Label();
            this.bikeLabel = new System.Windows.Forms.Label();
            this.versionTextBox = new System.Windows.Forms.TextBox();
            this.serialTextBox = new System.Windows.Forms.TextBox();
            this.typeTextBox = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.addBikeButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.addModelButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.assignButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.modelComboBox = new System.Windows.Forms.ComboBox();
            this.bikeComboBox = new System.Windows.Forms.ComboBox();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripButton,
            this.readToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(386, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // exitToolStripButton
            // 
            this.exitToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.exitToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("exitToolStripButton.Image")));
            this.exitToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exitToolStripButton.Name = "exitToolStripButton";
            this.exitToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.exitToolStripButton.Text = "Beenden";
            this.exitToolStripButton.Click += new System.EventHandler(this.exitToolStripButton_Click);
            // 
            // readToolStripButton
            // 
            this.readToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.readToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("readToolStripButton.Image")));
            this.readToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.readToolStripButton.Name = "readToolStripButton";
            this.readToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.readToolStripButton.Text = "Tacho auslesen";
            this.readToolStripButton.Click += new System.EventHandler(this.readToolStripButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(386, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.beendenToolStripMenuItem});
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.dateiToolStripMenuItem.Text = "&Datei";
            // 
            // beendenToolStripMenuItem
            // 
            this.beendenToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("beendenToolStripMenuItem.Image")));
            this.beendenToolStripMenuItem.Name = "beendenToolStripMenuItem";
            this.beendenToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.beendenToolStripMenuItem.Text = "B&eenden";
            this.beendenToolStripMenuItem.Click += new System.EventHandler(this.beendenToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.modelLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.versionLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.serialLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.typeLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.bikeLabel, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.versionTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.serialTextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.typeTextBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel3, 1, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 49);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(386, 220);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // modelLabel
            // 
            this.modelLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.modelLabel.AutoSize = true;
            this.modelLabel.Location = new System.Drawing.Point(3, 11);
            this.modelLabel.Name = "modelLabel";
            this.modelLabel.Size = new System.Drawing.Size(64, 13);
            this.modelLabel.TabIndex = 0;
            this.modelLabel.Text = "Modellname";
            // 
            // versionLabel
            // 
            this.versionLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(3, 46);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(42, 13);
            this.versionLabel.TabIndex = 1;
            this.versionLabel.Text = "Version";
            // 
            // serialLabel
            // 
            this.serialLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.serialLabel.AutoSize = true;
            this.serialLabel.Location = new System.Drawing.Point(3, 81);
            this.serialLabel.Name = "serialLabel";
            this.serialLabel.Size = new System.Drawing.Size(74, 13);
            this.serialLabel.TabIndex = 2;
            this.serialLabel.Text = "Seriennummer";
            // 
            // typeLabel
            // 
            this.typeLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(3, 116);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(25, 13);
            this.typeLabel.TabIndex = 3;
            this.typeLabel.Text = "Typ";
            // 
            // bikeLabel
            // 
            this.bikeLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.bikeLabel.AutoSize = true;
            this.bikeLabel.Location = new System.Drawing.Point(3, 151);
            this.bikeLabel.Name = "bikeLabel";
            this.bikeLabel.Size = new System.Drawing.Size(43, 13);
            this.bikeLabel.TabIndex = 4;
            this.bikeLabel.Text = "Fahrrad";
            // 
            // versionTextBox
            // 
            this.versionTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.versionTextBox.Enabled = false;
            this.versionTextBox.Location = new System.Drawing.Point(123, 42);
            this.versionTextBox.Name = "versionTextBox";
            this.versionTextBox.Size = new System.Drawing.Size(170, 20);
            this.versionTextBox.TabIndex = 2;
            // 
            // serialTextBox
            // 
            this.serialTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.serialTextBox.Enabled = false;
            this.serialTextBox.Location = new System.Drawing.Point(123, 77);
            this.serialTextBox.Name = "serialTextBox";
            this.serialTextBox.Size = new System.Drawing.Size(170, 20);
            this.serialTextBox.TabIndex = 3;
            // 
            // typeTextBox
            // 
            this.typeTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.typeTextBox.Enabled = false;
            this.typeTextBox.Location = new System.Drawing.Point(123, 112);
            this.typeTextBox.Name = "typeTextBox";
            this.typeTextBox.Size = new System.Drawing.Size(170, 20);
            this.typeTextBox.TabIndex = 4;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.bikeComboBox);
            this.flowLayoutPanel1.Controls.Add(this.addBikeButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(123, 143);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(260, 29);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // addBikeButton
            // 
            this.addBikeButton.Enabled = false;
            this.addBikeButton.Location = new System.Drawing.Point(176, 3);
            this.addBikeButton.Name = "addBikeButton";
            this.addBikeButton.Size = new System.Drawing.Size(75, 23);
            this.addBikeButton.TabIndex = 6;
            this.addBikeButton.Text = "Hinzufügen";
            this.addBikeButton.UseVisualStyleBackColor = true;
            this.addBikeButton.Click += new System.EventHandler(this.addBikeButton_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.modelComboBox);
            this.flowLayoutPanel2.Controls.Add(this.addModelButton);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(123, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(260, 29);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // addModelButton
            // 
            this.addModelButton.Enabled = false;
            this.addModelButton.Location = new System.Drawing.Point(176, 3);
            this.addModelButton.Name = "addModelButton";
            this.addModelButton.Size = new System.Drawing.Size(75, 23);
            this.addModelButton.TabIndex = 1;
            this.addModelButton.Text = "Hinzufügen";
            this.addModelButton.UseVisualStyleBackColor = true;
            this.addModelButton.Click += new System.EventHandler(this.addTachoButton_Click);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.assignButton);
            this.flowLayoutPanel3.Controls.Add(this.clearButton);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(123, 178);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(260, 29);
            this.flowLayoutPanel3.TabIndex = 7;
            // 
            // assignButton
            // 
            this.assignButton.Enabled = false;
            this.assignButton.Location = new System.Drawing.Point(3, 3);
            this.assignButton.Name = "assignButton";
            this.assignButton.Size = new System.Drawing.Size(122, 23);
            this.assignButton.TabIndex = 7;
            this.assignButton.Text = "Zuordnen";
            this.assignButton.UseVisualStyleBackColor = true;
            this.assignButton.Click += new System.EventHandler(this.assignButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(131, 3);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(122, 23);
            this.clearButton.TabIndex = 8;
            this.clearButton.Text = "Löschen";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // modelComboBox
            // 
            this.modelComboBox.FormattingEnabled = true;
            this.modelComboBox.Location = new System.Drawing.Point(3, 3);
            this.modelComboBox.Name = "modelComboBox";
            this.modelComboBox.Size = new System.Drawing.Size(167, 21);
            this.modelComboBox.Sorted = true;
            this.modelComboBox.TabIndex = 0;
            this.modelComboBox.TextChanged += new System.EventHandler(this.modelComboBox_TextChanged);
            // 
            // bikeComboBox
            // 
            this.bikeComboBox.FormattingEnabled = true;
            this.bikeComboBox.Location = new System.Drawing.Point(3, 3);
            this.bikeComboBox.Name = "bikeComboBox";
            this.bikeComboBox.Size = new System.Drawing.Size(167, 21);
            this.bikeComboBox.Sorted = true;
            this.bikeComboBox.TabIndex = 5;
            this.bikeComboBox.TextChanged += new System.EventHandler(this.bikeComboBox_TextChanged);
            // 
            // BikeInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 269);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BikeInfoForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tacho verknüpfen";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BikeInfoForm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton exitToolStripButton;
        private System.Windows.Forms.ToolStripButton readToolStripButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label modelLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label serialLabel;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.Label bikeLabel;
        private System.Windows.Forms.TextBox versionTextBox;
        private System.Windows.Forms.TextBox serialTextBox;
        private System.Windows.Forms.TextBox typeTextBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button addBikeButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button addModelButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Button assignButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.ComboBox bikeComboBox;
        private System.Windows.Forms.ComboBox modelComboBox;
    }
}