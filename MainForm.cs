using System;
using System.Drawing;
using System.IO;
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
            tachoLabel.Text = notConnectedText;
            LoadBikeList();

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
            monitorTimer.Interval = 10000; // 10 Sekunden
            monitorTimer.Tick += MonitorTimer_Tick;
            monitorTimer.Start();
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
                    tachoLabel.Text = tachoConnectedText;
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
                            dsNotifyIcon.ShowBalloonTip(3000, tachoFoundText,
                                $"{info.ModelName} {recognizedTachoText}", ToolTipIcon.Info);
                            tachoLabel.Text = $"{tachoText}: {info.ModelName}";
                        }
                    }
                }
                else
                {
                    tachoLabel.Text = notConnectedText;
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

                    distanceTextBox.Text = data.DistanceKm.ToString("F2");
                    timeTextBox.Text = data.Duration.ToString(@"h\:mm\:ss");
                    vavgTextBox.Text = data.MeanSpeedKmh.ToString("F2");
                    vmaxTextBox.Text = data.MaxSpeedKmh.ToString("F2");
                    cadenceTextBox.Text = data.Cadence.ToString();

                    // Fahrrad vorauswählen basierend auf Seriennummer
                    SelectBikeBySerial(info.SerialNumber);

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
            if (settingsForm.ShowDialog(this) == DialogResult.OK)
                LoadBikeList();
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
            string fullpath = Path.Combine(Properties.Settings.Default.SaveFolder, fileName);
            File.WriteAllText(fullpath, jsonString);
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
            SetLanguage(Properties.Settings.Default.Language);
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
            Properties.Settings.Default.Language = "de";
            Properties.Settings.Default.Save();
            SetLanguage("de");
        }

        private void SetLanguage(string lang)
        {
            if (lang == "de")
            {
                label11.Text = "Datum";
                label1.Text = "Tacho";
                label3.Text = "Fahrrad";
                label4.Text = "Gefahrende [km]";
                label5.Text = "Fahrzeit [h:mm:ss]";
                label6.Text = "Ø Geschwindigkeit [km/h]";
                label7.Text = "Max. Geschwindigkeit [km/h]";
                label8.Text = "Kadenz [U/min]";
                label9.Text = "Teilstrecke Distanz [km]";
                label10.Text = "Teilstrecke Zeit [h:mm:ss]";

                dateiToolStripMenuItem.Text = "&Datei";
                beendenToolStripMenuItem.Text = "Be&enden";
                beendenToolStripMenuItem1.Text = "Be&enden";
                tachoToolStripMenuItem.Text = "&Tacho";
                auslesenToolStripMenuItem.Text = "&Auslesen";
                einstellungenToolStripMenuItem.Text = "&Einstellungen";
                hilfeToolStripMenuItem.Text = "&Hilfe";
                infoToolStripMenuItem.Text = "&Info";
                hilfeToolStripMenuItem1.Text = "&Hilfe";
                spracheToolStripMenuItem.Text = "&Sprache";
                optionenToolStripMenuItem.Text = "&Optionen";
                deutschToolStripMenuItem.Text = "&Deutsch";
                englishToolStripMenuItem.Text = "&English";
                hauptfensterToolStripMenuItem.Text = "&Hauptfenster";
                connectedToolStripStatusLabel.Text = "Verbunden";
                notConnectedToolStripStatusLabel.Text = "Nicht verbunden";
                notConnectedText = "Nicht verbunden";
                tachoFoundText = "Tacho gefunden";
                tachoConnectedText = "Tacho verbunden";
                recognizedTachoText = "erkannt";
                msgWaitPlease = "Bitte warten, Zugriff läuft bereits...";
                msgDataRead = "Daten erfolgreich ausgelesen!";
                errorText = "Fehler";
                successText = "Erfolg";
                errorReadingTachoText = "Fehler beim Auslesen der Daten.\nBitte Tacho überprüfen.";
                tachoText = "Tacho";
                unknownText = "Unbekannt";
                saveButton.Text = "Speichern";
                clearButton.Text = "Löschen";
                readTachoToolStripButton.Text = "Auslesen";
                exitToolStripButton.Text = "Beenden";
            }
            else if (lang == "en")
            {
                label11.Text = "Date";
                label1.Text = "Speedometer";
                label3.Text = "Bike";
                label4.Text = "Distance [km]";
                label5.Text = "Time [h:mm:ss]";
                label6.Text = "Ø speed [km/h]";
                label7.Text = "Max. speed [km/h]";
                label8.Text = "Cadence [U/min]";
                label9.Text = "Leg distance [km]";
                label10.Text = "Leg time [h:mm:ss]";

                dateiToolStripMenuItem.Text = "&File";
                beendenToolStripMenuItem.Text = "&Exit";
                beendenToolStripMenuItem1.Text = "&Exit";
                tachoToolStripMenuItem.Text = "Speedo&meter";
                auslesenToolStripMenuItem.Text = "&Read";
                einstellungenToolStripMenuItem.Text = "&Settings";
                hilfeToolStripMenuItem.Text = "&Help";
                infoToolStripMenuItem.Text = "&Info";
                hilfeToolStripMenuItem1.Text = "&Help";
                spracheToolStripMenuItem.Text = "&Language";
                optionenToolStripMenuItem.Text = "&Options";
                deutschToolStripMenuItem.Text = "&Deutsch";
                englishToolStripMenuItem.Text = "&English";
                hauptfensterToolStripMenuItem.Text = "&Main window";
                connectedToolStripStatusLabel.Text = "Connected";
                notConnectedToolStripStatusLabel.Text = "Not connected";
                notConnectedText = "Not connected";
                tachoFoundText = "Speedometer found";
                tachoConnectedText = "Speedometer connected";
                recognizedTachoText = "recognized";
                msgWaitPlease = "Please wait, already accessing...";
                msgDataRead = "Data successfully read!";
                errorText = "Error";
                successText = "Success";
                errorReadingTachoText = "Error reading data.\nPlease check speedometer.";
                tachoText = "Speedometer";
                unknownText = "Unknown";
                saveButton.Text = "Save";
                clearButton.Text = "Delete";
                readTachoToolStripButton.Text = "Read";
                exitToolStripButton.Text = "Exit";
            }
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Language = "en";
            Properties.Settings.Default.Save();
            SetLanguage("en");
        }
    }
}
