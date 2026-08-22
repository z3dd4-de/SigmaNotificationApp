using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SigmaDockingLib;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace SigmaNotificationApp
{
    public partial class BikeInfoForm : Form
    {
        private SigmaReader reader = new SigmaReader();
        private System.Windows.Forms.Timer monitorTimer;
        private BikeComputerInfo? lastDetectedComputer = null;
        private bool isReading = false; // Verhindert parallele Zugriffe
        private string notConnectedText = "Nicht verbunden";
        private string tachoFoundText = "Tacho gefunden";
        private string tachoConnectedText = "Tacho verbunden";
        private string recognizedTachoText = "erkannt";
        private string msgWaitPlease = "Bitte warten, Zugriff läuft bereits...";
        private string msgDataRead = "Daten erfolgreich ausgelesen!";
        private string errorText = "Fehler";
        private string successText = "Erfolg";
        private string errorReadingTachoText = "Fehler beim Auslesen der Daten.\nBitte Tacho überprüfen.";
        private string tachoText = "Tacho";
        private string unknownText = "Unbekannt";
        AssignmentDictionary assignmentDictionary = new AssignmentDictionary();

        public BikeInfoForm()
        {
            InitializeComponent();
            LoadBikeList();
            assignmentDictionary.LoadSettings();
            LoadTachoList();
            loadLanguage(this, EventArgs.Empty);
        }

        private void loadLanguage(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Language == "de")
            {
                this.Text = "Tacho verknüpfen";
                modelLabel.Text = "Modellname";
                versionLabel.Text = "Version";
                serialLabel.Text = "Seriennummer";
                typeLabel.Text = "Typ";
                bikeLabel.Text = "Fahrrad";
                clearButton.Text = "Löschen";
                addModelButton.Text = "Hinzufügen";
                addBikeButton.Text = "Hinzufügen";
                assignButton.Text = "Zuweisen";
                dateiToolStripMenuItem.Text = "&Datei";
                beendenToolStripMenuItem.Text = "&Beenden";
                exitToolStripButton.Text = "Beenden";
                readToolStripButton.Text = "Auslesen";
            }
            else
            {
                this.Text = "Assign Speedometer";
                modelLabel.Text = "Model Name";
                versionLabel.Text = "Version";
                serialLabel.Text = "Serial Number";
                typeLabel.Text = "Type";
                bikeLabel.Text = "Bike";
                clearButton.Text = "Clear";
                addModelButton.Text = "Add";
                addBikeButton.Text = "Add";
                assignButton.Text = "Assign";
                dateiToolStripMenuItem.Text = "&File";
                beendenToolStripMenuItem.Text = "&Exit";
                exitToolStripButton.Text = "Exit";
                readToolStripButton.Text = "Read";
            }
        }

        private void LoadBikeList()
        {
            bikeComboBox.Items.Clear();
            string bikeCollection = Properties.Settings.Default.BikeCollection;
            if (!string.IsNullOrEmpty(bikeCollection))
            {
                var bikes = bikeCollection.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                bikeComboBox.Items.AddRange(bikes);
            }
        }

        private void LoadTachoList()
        {
            modelComboBox.Items.Clear();
            string tachoCollection = Properties.Settings.Default.TachoCollection;
            if (!string.IsNullOrEmpty(tachoCollection))
            {
                var tachos = tachoCollection.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                modelComboBox.Items.AddRange(tachos);
            }
        }

        private void addTachoButton_Click(object sender, EventArgs e)
        {
            if (modelComboBox.Text != String.Empty)
            {
                foreach (var item in modelComboBox.Items)
                {
                    if (item.ToString() == modelComboBox.Text)
                    {
                        modelComboBox.SelectedItem = modelComboBox.Text;
                        return;
                    }
                }
                modelComboBox.Items.Add(modelComboBox.Text);
                modelComboBox.SelectedItem = modelComboBox.Text;
                createTachoCollection();
            }
        }

        private void addBikeButton_Click(object sender, EventArgs e)
        {
            if (bikeComboBox.Text != String.Empty)
            {
                foreach (var item in bikeComboBox.Items)
                {
                    if (item.ToString() == bikeComboBox.Text)
                    {
                        bikeComboBox.SelectedItem = bikeComboBox.Text;
                        return;
                    }
                }
                bikeComboBox.Items.Add(bikeComboBox.Text);
                bikeComboBox.SelectedItem = bikeComboBox.Text;
                string tmp = String.Empty;
                foreach (var item in bikeComboBox.Items)
                {
                    tmp += item.ToString() + ";";
                }
                Properties.Settings.Default.BikeCollection = tmp;
            }
        }

        private void assignButton_Click(object sender, EventArgs e)
        {
            assignmentDictionary.AddAssignment(serialTextBox.Text, bikeComboBox.Text);
            assignmentDictionary.SaveSettings();    
            MessageBox.Show($"{tachoText} '{modelComboBox.Text}' {recognizedTachoText} und '{bikeComboBox.Text}' zugewiesen.",
                    successText, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void createTachoCollection()
        {
            string tmp = String.Empty;
            foreach (var item in modelComboBox.Items)
            {
                tmp += item.ToString() + ";";
            }
            Properties.Settings.Default.TachoCollection = tmp;
        }

        private void modelComboBox_TextChanged(object sender, EventArgs e)
        {
            if (modelComboBox.Text != String.Empty)
                addModelButton.Enabled = true;
            else
                addModelButton.Enabled = false;
            checkAssignButtonState();
        }

        private void bikeComboBox_TextChanged(object sender, EventArgs e)
        {
            if (bikeComboBox.Text != String.Empty)
                addBikeButton.Enabled = true;
            else
                addBikeButton.Enabled = false;
            checkAssignButtonState();
        }

        private void checkAssignButtonState()
        {
            assignButton.Enabled = modelComboBox.Text != String.Empty &&
                                   bikeComboBox.Text != String.Empty;
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void exitToolStripButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void readToolStripButton_Click(object sender, EventArgs e)
        {
            readTacho();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            modelComboBox.SelectedText = String.Empty;
            versionTextBox.Text = String.Empty;
            serialTextBox.Text = String.Empty;
            typeTextBox.Text = String.Empty;
            bikeComboBox.SelectedText = String.Empty;
        }

        private async void readTacho()
        {
            if (isReading)
            {
                MessageBox.Show(msgWaitPlease, "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                isReading = true;
                Cursor = Cursors.WaitCursor;

                // Async ausführen um UI nicht zu blockieren
                var result = await Task.Run(() => reader.ReadComplete());
                var (info, data) = result;

                if (info != null && data != null)
                {
                    lastDetectedComputer = info;

                    modelComboBox.Text = info.ModelName;
                    versionTextBox.Text = info.Version.ToString();
                    serialTextBox.Text = info.SerialNumber;
                    typeTextBox.Text = info.Type.ToString();
                    
                    MessageBox.Show(msgDataRead, successText,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(errorReadingTachoText,
                        errorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{errorText}: {ex.Message}", errorText,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isReading = false;
                Cursor = Cursors.Default;
            }
        }

        private void BikeInfoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            assignmentDictionary.SaveSettings();
            DialogResult = DialogResult.OK;
        }
    }
}
