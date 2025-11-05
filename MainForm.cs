using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using SigmaDockingLib;

namespace SigmaNotificationApp
{
    public partial class MainForm : Form
    {
        private SigmaReader reader = new SigmaReader();
        private System.Windows.Forms.Timer monitorTimer;
        private BikeComputerInfo? lastDetectedComputer = null;
        private bool isReading = false; // Verhindert parallele Zugriffe
        private enum AppState
        {
            Normal,
            NotifyIcon
        }

        public MainForm()
        {
            InitializeComponent();
            connectedToolStripStatusLabel.Visible = false;
            notConnectedToolStripStatusLabel.Visible = false;
            tachoToolStripStatusLabel.Visible = false;
            tachoLabel.Text = "<nicht verbunden>";

            // Event für Log-Meldungen
            reader.LogMessage += (s, msg) =>
            {
                // Invoke für Thread-Safety bei UI-Updates
                if (InvokeRequired)
                    Invoke(new Action(() => Console.WriteLine(msg)));
                else
                    Console.WriteLine(msg);
            };

            // Timer für Hintergrund-Monitoring
            monitorTimer = new System.Windows.Forms.Timer();
            monitorTimer.Interval = 5000; // 5 Sekunden
            monitorTimer.Tick += MonitorTimer_Tick;
            monitorTimer.Start();
        }

        private void MonitorTimer_Tick(object sender, EventArgs e)
        {
            // Verhindere parallele Zugriffe
            if (isReading) return;

            try
            {
                isReading = true;

                // NUR prüfen ob Tacho da ist - NICHT die Daten lesen!
                if (reader.IsBikeComputerPresent())
                {
                    tachoLabel.Text = "Tacho verbunden";
                    connectedToolStripStatusLabel.Visible = true;
                    notConnectedToolStripStatusLabel.Visible = false;

                    // Nur beim ersten Mal Balloon zeigen
                    if (lastDetectedComputer == null)
                    {
                        // Info lesen (einmalig)
                        var info = reader.ReadBikeComputerInfo();
                        if (info != null)
                        {
                            lastDetectedComputer = info;
                            dsNotifyIcon.ShowBalloonTip(3000, "Tacho gefunden",
                                $"{info.ModelName} erkannt", ToolTipIcon.Info);
                            tachoLabel.Text = $"Tacho: {info.ModelName}";
                        }
                    }
                }
                else
                {
                    tachoLabel.Text = "<nicht verbunden>";
                    connectedToolStripStatusLabel.Visible = false;
                    notConnectedToolStripStatusLabel.Visible = true;
                    lastDetectedComputer = null; // Reset für nächstes Mal
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Monitor error: {ex.Message}");
            }
            finally
            {
                isReading = false;
            }
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void exitToolStripButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void auslesenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            readTacho();
        }

        private void readTachoToolStripButton_Click(object sender, EventArgs e)
        {
            readTacho();
        }

        private async void readTacho()
        {
            if (isReading)
            {
                MessageBox.Show("Bitte warten, Zugriff läuft bereits...", "Info",
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

                    distanceTextBox.Text = data.DistanceKm.ToString("F2");
                    timeTextBox.Text = data.Duration.ToString(@"h\:mm\:ss");
                    vavgTextBox.Text = data.MeanSpeedKmh.ToString("F2");
                    vmaxTextBox.Text = data.MaxSpeedKmh.ToString("F2");
                    cadenceTextBox.Text = data.Cadence.ToString();

                    // Fahrrad vorauswählen basierend auf Seriennummer
                    SelectBikeBySerial(info.SerialNumber);

                    MessageBox.Show("Daten erfolgreich ausgelesen!", "Erfolg",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Fehler beim Auslesen der Daten.\nBitte Tacho überprüfen.",
                        "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isReading = false;
                Cursor = Cursors.Default;
            }
        }

        private void SelectBikeBySerial(string serialNumber)
        {
            // Versuche gespeichertes Fahrrad für diese Seriennummer zu laden
            string settingKey = $"Bike_{serialNumber}";
            string? savedBike = Properties.Settings.Default[settingKey] as string;

            if (!string.IsNullOrEmpty(savedBike))
            {
                // Fahrrad in ComboBox auswählen
                int index = bikeComboBox.Items.IndexOf(savedBike);
                if (index >= 0)
                {
                    bikeComboBox.SelectedIndex = index;
                }
                else
                {
                    bikeComboBox.Text = savedBike; // Falls nicht in Liste, als Text setzen
                }
            }
        }

        // Beim Speichern der JSON-Datei das Mapping speichern
        private void btnSave_Click(object sender, EventArgs e)
        {
            // ... dein JSON-Speicher-Code ...

            // Mapping Seriennummer -> Fahrrad speichern
            if (lastDetectedComputer != null && !string.IsNullOrEmpty(bikeComboBox.Text))
            {
                string settingKey = $"Bike_{lastDetectedComputer.SerialNumber}";
                Properties.Settings.Default[settingKey] = bikeComboBox.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void einstellungenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.ShowDialog(this);
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showInfo();
        }

        private void infoToolStripButton_Click(object sender, EventArgs e)
        {
            showInfo();
        }

        private void showInfo()
        {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.ShowDialog();
        }

        private void hilfeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HelpForm helpForm = new HelpForm();
            helpForm.ShowDialog(this);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            double distanceMeters_d = double.TryParse(distanceTextBox.Text, out double dist) ? dist : 0;
            uint distanceMeters = (uint)(distanceMeters_d * 1000); // km to m
            TimeSpan duration = TimeSpan.TryParse(timeTextBox.Text, out TimeSpan dur) ? dur : TimeSpan.Zero;
            uint timeSeconds = (uint)duration.TotalSeconds;

            RideData rideData = new RideData
            {
                TachoName = lastDetectedComputer?.ModelName ?? "Unbekannt",
                BikeName = bikeComboBox.Text,   
                DistanceMeters = distanceMeters,
                TimeSeconds = timeSeconds,
                MeanSpeedKmh = double.TryParse(vavgTextBox.Text, out double vavg) ? vavg : 0,
                MaxSpeedKmh = double.TryParse(vmaxTextBox.Text, out double vmax) ? vmax : 0,
                Cadence = (byte)(byte.TryParse(cadenceTextBox.Text, out byte cad) ? cad : 0),
                Timestamp = dateTimePicker.Value,
                TripSectionDistanceMeters = (uint)(double.TryParse(tsDistanceTextBox.Text, out double tsDist) ? tsDist * 1000 : 0),
                TripSectionTimeSeconds = (uint)(TimeSpan.TryParse(tsTimeTextBox.Text, out TimeSpan tsDur) ? tsDur.TotalSeconds : 0)
            };

            string fileName = "bikedata.json";
            string jsonString = JsonSerializer.Serialize(rideData);
            File.WriteAllText(fileName, jsonString);
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            distanceTextBox.Clear();
            timeTextBox.Clear();
            vmaxTextBox.Clear();
            vavgTextBox.Clear();
            tsDistanceTextBox.Clear();
            tsTimeTextBox.Clear();
            cadenceTextBox.Clear();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            Properties.Settings.Default.WindowLocation = this.Location;
            Properties.Settings.Default.WindowSize = this.Size;
            //Hide this in the system tray instead of in the taskbar
            if (WindowState == FormWindowState.Minimized)
            {
                Properties.Settings.Default.AppMode = (int)AppState.NotifyIcon;
                dsNotifyIcon.Visible = true;
                Hide(); 
            }
            else
            {
                dsNotifyIcon.Visible = false;
                Properties.Settings.Default.AppMode = (int)AppState.Normal;
            }
            Properties.Settings.Default.Save();
        }

        private void hauptfensterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            dsNotifyIcon.Visible = false;
        }

        private void beendenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.AppMode = (int)AppState.Normal;
                Properties.Settings.Default.WindowLocation = this.Location;
                Properties.Settings.Default.WindowSize = this.Size;
            }
            else if (dsNotifyIcon.Visible)
            {
                Properties.Settings.Default.AppMode = (int)AppState.NotifyIcon;
            }
            Properties.Settings.Default.Save();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.WindowSize != Size.Empty) { 
                Location = Properties.Settings.Default.WindowLocation;
                Size = Properties.Settings.Default.WindowSize;
                AppState savedState = (AppState)Properties.Settings.Default.AppMode;
                if (savedState == AppState.NotifyIcon)
                {
                    dsNotifyIcon.Visible = true;
                    Hide();
                }
                else
                {
                    dsNotifyIcon.Visible = false;
                    Show();
                }
            }
        }

        private void deutschToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
