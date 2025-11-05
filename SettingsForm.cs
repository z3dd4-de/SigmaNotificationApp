using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        }

        private void addTachoButton_Click(object sender, EventArgs e)
        {

        }

        private void clearBikeBbutton_Click(object sender, EventArgs e)
        {

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
