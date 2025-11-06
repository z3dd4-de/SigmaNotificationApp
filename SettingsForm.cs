using System;
using System.Windows.Forms;

namespace SigmaNotificationApp
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
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
    }
}
