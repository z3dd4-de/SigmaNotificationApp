namespace SigmaNotificationApp
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.saveFolderTextBox = new System.Windows.Forms.TextBox();
            this.pathButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tachoComboBox = new System.Windows.Forms.ComboBox();
            this.addTachoTextBox = new System.Windows.Forms.TextBox();
            this.clearTachoButton = new System.Windows.Forms.Button();
            this.addTachoButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.bikeComboBox = new System.Windows.Forms.ComboBox();
            this.addBikeTextBox = new System.Windows.Forms.TextBox();
            this.clearBikeBbutton = new System.Windows.Forms.Button();
            this.addBikeButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.saveLocationToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(420, 252);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel4);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 165);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(414, 63);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Speicherort";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.Controls.Add(this.saveFolderTextBox, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.pathButton, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(408, 44);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // saveFolderTextBox
            // 
            this.saveFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.saveFolderTextBox.Location = new System.Drawing.Point(3, 12);
            this.saveFolderTextBox.Name = "saveFolderTextBox";
            this.saveFolderTextBox.Size = new System.Drawing.Size(320, 20);
            this.saveFolderTextBox.TabIndex = 8;
            // 
            // pathButton
            // 
            this.pathButton.Image = ((System.Drawing.Image)(resources.GetObject("pathButton.Image")));
            this.pathButton.Location = new System.Drawing.Point(329, 3);
            this.pathButton.Name = "pathButton";
            this.pathButton.Size = new System.Drawing.Size(75, 38);
            this.pathButton.TabIndex = 5;
            this.saveLocationToolTip.SetToolTip(this.pathButton, "Suche den Speicherort für Tachodaten");
            this.pathButton.UseVisualStyleBackColor = true;
            this.pathButton.Click += new System.EventHandler(this.pathButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(414, 75);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tacho";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.tachoComboBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.addTachoTextBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.clearTachoButton, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.addTachoButton, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(408, 56);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tachoComboBox
            // 
            this.tachoComboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.tachoComboBox.FormattingEnabled = true;
            this.tachoComboBox.Items.AddRange(new object[] {
            "BC 12.12",
            "BC 16.16",
            "BC 16.16 STS"});
            this.tachoComboBox.Location = new System.Drawing.Point(3, 3);
            this.tachoComboBox.Name = "tachoComboBox";
            this.tachoComboBox.Size = new System.Drawing.Size(198, 21);
            this.tachoComboBox.TabIndex = 0;
            // 
            // addTachoTextBox
            // 
            this.addTachoTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.addTachoTextBox.Location = new System.Drawing.Point(207, 3);
            this.addTachoTextBox.Name = "addTachoTextBox";
            this.addTachoTextBox.Size = new System.Drawing.Size(198, 20);
            this.addTachoTextBox.TabIndex = 2;
            this.addTachoTextBox.TextChanged += new System.EventHandler(this.addTachoTextBox_TextChanged);
            // 
            // clearTachoButton
            // 
            this.clearTachoButton.Enabled = false;
            this.clearTachoButton.Location = new System.Drawing.Point(3, 31);
            this.clearTachoButton.Name = "clearTachoButton";
            this.clearTachoButton.Size = new System.Drawing.Size(75, 22);
            this.clearTachoButton.TabIndex = 1;
            this.clearTachoButton.Text = "Löschen";
            this.clearTachoButton.UseVisualStyleBackColor = true;
            this.clearTachoButton.Click += new System.EventHandler(this.clearTachoButton_Click);
            // 
            // addTachoButton
            // 
            this.addTachoButton.Enabled = false;
            this.addTachoButton.Location = new System.Drawing.Point(207, 31);
            this.addTachoButton.Name = "addTachoButton";
            this.addTachoButton.Size = new System.Drawing.Size(75, 22);
            this.addTachoButton.TabIndex = 3;
            this.addTachoButton.Text = "Hinzufügen";
            this.addTachoButton.UseVisualStyleBackColor = true;
            this.addTachoButton.Click += new System.EventHandler(this.addTachoButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 84);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(414, 75);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Fahrrad";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.bikeComboBox, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.addBikeTextBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.clearBikeBbutton, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.addBikeButton, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(408, 56);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // bikeComboBox
            // 
            this.bikeComboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.bikeComboBox.FormattingEnabled = true;
            this.bikeComboBox.Location = new System.Drawing.Point(3, 3);
            this.bikeComboBox.Name = "bikeComboBox";
            this.bikeComboBox.Size = new System.Drawing.Size(198, 21);
            this.bikeComboBox.TabIndex = 4;
            // 
            // addBikeTextBox
            // 
            this.addBikeTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.addBikeTextBox.Location = new System.Drawing.Point(207, 3);
            this.addBikeTextBox.Name = "addBikeTextBox";
            this.addBikeTextBox.Size = new System.Drawing.Size(198, 20);
            this.addBikeTextBox.TabIndex = 6;
            this.addBikeTextBox.TextChanged += new System.EventHandler(this.addBikeTextBox_TextChanged);
            // 
            // clearBikeBbutton
            // 
            this.clearBikeBbutton.Location = new System.Drawing.Point(3, 31);
            this.clearBikeBbutton.Name = "clearBikeBbutton";
            this.clearBikeBbutton.Size = new System.Drawing.Size(75, 22);
            this.clearBikeBbutton.TabIndex = 5;
            this.clearBikeBbutton.Text = "Löschen";
            this.clearBikeBbutton.UseVisualStyleBackColor = true;
            this.clearBikeBbutton.Click += new System.EventHandler(this.clearBikeBbutton_Click);
            // 
            // addBikeButton
            // 
            this.addBikeButton.Enabled = false;
            this.addBikeButton.Location = new System.Drawing.Point(207, 31);
            this.addBikeButton.Name = "addBikeButton";
            this.addBikeButton.Size = new System.Drawing.Size(75, 22);
            this.addBikeButton.TabIndex = 7;
            this.addBikeButton.Text = "Hinzufügen";
            this.addBikeButton.UseVisualStyleBackColor = true;
            this.addBikeButton.Click += new System.EventHandler(this.addBikeButton_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 252);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Einstellungen";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsForm_FormClosed);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox tachoComboBox;
        private System.Windows.Forms.TextBox addTachoTextBox;
        private System.Windows.Forms.Button clearTachoButton;
        private System.Windows.Forms.Button addTachoButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ComboBox bikeComboBox;
        private System.Windows.Forms.TextBox addBikeTextBox;
        private System.Windows.Forms.Button clearBikeBbutton;
        private System.Windows.Forms.Button addBikeButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button pathButton;
        private System.Windows.Forms.TextBox saveFolderTextBox;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ToolTip saveLocationToolTip;
    }
}