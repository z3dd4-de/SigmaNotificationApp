using System;
using System.Windows.Forms;

namespace SigmaNotificationApp
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            loadLanguage(this, EventArgs.Empty);
        }

        private void clearTachoButton_Click(object sender, EventArgs e)
        {
            // Not implemented yet
        }

        private void addTachoButton_Click(object sender, EventArgs e)
        {
            if (addTachoTextBox.Text != String.Empty)
            {
                tachoComboBox.Items.Add(addTachoTextBox.Text);
                string tmp = String.Empty;
                foreach (var item in tachoComboBox.Items)
                {
                    if (item.ToString() == addTachoTextBox.Text)
                    {
                        tachoComboBox.SelectedItem = item;
                        //break;
                    }
                    tmp += item.ToString() + ";";
                }
                Properties.Settings.Default.TachoCollection = tmp;
                addTachoTextBox.Clear();
            }
        }

        private void clearBikeBbutton_Click(object sender, EventArgs e)
        {
            bikeComboBox.Items.Clear();
            Properties.Settings.Default.BikeCollection = String.Empty;
        }

        private void addBikeButton_Click(object sender, EventArgs e)
        {
            bikeComboBox.Items.Add(addBikeTextBox.Text);
            string tmp = String.Empty;
            foreach (var item in bikeComboBox.Items)
            {
                if (item.ToString() == addBikeTextBox.Text)
                {
                    bikeComboBox.SelectedItem = item;
                    //break;
                }
                tmp += item.ToString() + ";";
            }
            Properties.Settings.Default.BikeCollection = tmp;
            addBikeTextBox.Clear();
        }

        private void addTachoTextBox_TextChanged(object sender, EventArgs e)
        {
            if (addTachoTextBox.Text != String.Empty)
                addTachoButton.Enabled = true;
            else
                addTachoButton.Enabled = false;
        }

        private void addBikeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (addBikeTextBox.Text != String.Empty)
                addBikeButton.Enabled = true;
            else
                addBikeButton.Enabled = false;
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void pathButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                saveFolderTextBox.Text = folderBrowserDialog.SelectedPath;
                Properties.Settings.Default.SaveFolder = folderBrowserDialog.SelectedPath;
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // Load TachoCollection
            string[] tachos = Properties.Settings.Default.TachoCollection.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var tacho in tachos)
            {
                tachoComboBox.Items.Add(tacho);
            }
            // Load BikeCollection
            string[] bikes = Properties.Settings.Default.BikeCollection.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var bike in bikes)
            {
                bikeComboBox.Items.Add(bike);
            }
            // Load SaveFolder
            saveFolderTextBox.Text = Properties.Settings.Default.SaveFolder;
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void loadLanguage(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Language == "de")
            {
                this.Text = "Einstellungen";
                groupBox1.Text = "Tacho";
                groupBox2.Text = "Fahrrad";
                groupBox3.Text = "Speicherort";
                clearTachoButton.Text = "Löschen";
                clearBikeBbutton.Text = "Löschen";
                addTachoButton.Text = "Hinzufügen";
                addBikeButton.Text = "Hinzufügen";
                if (string.IsNullOrWhiteSpace(saveFolderTextBox.Text))
                    saveFolderTextBox.Text = "Kein Speicherort ausgewählt";
            }
            else
            {
                this.Text = "Settings";
                groupBox1.Text = "Speedometer";
                groupBox2.Text = "Bike";
                groupBox3.Text = "Save location";
                clearTachoButton.Text = "Delete";
                clearBikeBbutton.Text = "Delete";
                addTachoButton.Text = "Add";
                addBikeButton.Text = "Add";
                if (string.IsNullOrWhiteSpace(saveFolderTextBox.Text))
                    saveFolderTextBox.Text = "No save location selected";
            }
        }
    }
}
