using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using SigmaDockingLib;
using System.Device.Location;
using System.Net.Http;
using System.Linq;

namespace SigmaNotificationApp
{
    public partial class MainForm : Form
    {
        private SigmaReader reader = new SigmaReader();
        private Timer monitorTimer;
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
        private string fileSavedText = "Datei erfolgreich gespeichert: ";
        private string fileSaveErrorText = "Fehler beim Speichern der Datei.";
        private string errorReadingLocationText = "Fehler beim Abrufen der Standortdaten.\nBitte Standortdienste aktivieren.";
        private string weatherDataDisabledText = "Wetterdaten deaktiviert";
        private string weatherDataEnabledText = "Bitte erst Wetterdaten abrufen...";
        private string weatherDataButtonText = "Wetterdaten abrufen";
        private string enterCityOrEnableLocationText = "Bitte geben Sie eine Stadt ein oder aktivieren Sie die automatische Standortbestimmung.";
        private string errorReadingWeatherText = "Fehler bei der Anfrage:";
        AssignmentDictionary assignmentDictionary = new AssignmentDictionary();

        private enum AppState
        {
            Normal,
            NotifyIcon
        }

        public GeoCoordinate GpsCoordinate { get; set; }
        private bool locationFound = false;
        public WeatherData LastWeatherData { get; set; }

        public MainForm()
        {
            InitializeComponent();
            SetFormSizeAndLocation();
            cloudLabel.Text = String.Empty;
            tempLabel.Text = String.Empty;
            winddirectionLabel.Text = String.Empty;
            windspeedLabel.Text = String.Empty;
            weatherLogLabel.Text = String.Empty;
            timestampLabel.Text = String.Empty;
            if (Properties.Settings.Default.Language == "de")
                SetLanguage("de");
            else
                SetLanguage("en");

            connectedToolStripStatusLabel.Visible = false;
            notConnectedToolStripStatusLabel.Visible = false;
            tachoToolStripStatusLabel.Visible = false;
            tachoLabel.Text = notConnectedText;
            LoadBikeList();
            LoadCityList();
            assignmentDictionary.LoadSettings();

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
            monitorTimer = new Timer();
            monitorTimer.Interval = 10000; // 10 Sekunden
            monitorTimer.Tick += MonitorTimer_Tick;
            monitorTimer.Start();

            GetLocationProperty();
            LastWeatherData = new WeatherData(GetPath());
        }

        private void SetFormSizeAndLocation()
        {
            if (Properties.Settings.Default.WindowSize != Size.Empty)
            {
                Location = Properties.Settings.Default.WindowLocation;
                Size = Properties.Settings.Default.WindowSize;
            }
            else
            {
                // Standardgröße und -position setzen, falls keine gespeicherten Werte vorhanden sind
                Size = new Size(368, 603);
                Location = new Point(50, 50);
            }
        }

        private string GetPath()
        {
            string path = Properties.Settings.Default.SaveFolder;
            if (string.IsNullOrEmpty(path))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            return path;
        }

        public void GetLocationProperty()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);

            watcher.PositionChanged += (sender, e) =>
            {
                GeoCoordinate coord = e.Position.Location;
                if (!coord.IsUnknown)
                {
                    GpsCoordinate = coord;
                    Console.WriteLine($"Breitengrad: {coord.Latitude}, Längengrad: {coord.Longitude}");
                    locationFound = true;
                }
                else
                {
                    Console.WriteLine("Location unknown.");
                    GpsCoordinate = null;
                    locationFound = false;
                }
            };

            watcher.Start();
        }

        private async Task GetCoordinate()
        {
            if (GpsCoordinate != null)
            {
                double latitude = GpsCoordinate.Latitude;
                double longitude = GpsCoordinate.Longitude;
                Console.WriteLine($"Breitengrad: {latitude}, Längengrad: {longitude}");

                await GetWeatherDataAsync(latitude, longitude);
            }
            else
            {
                Console.WriteLine("Location unknown.");
            }
        }

        public async Task GetWeatherDataAsync(double latitude, double longitude)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={Properties.Settings.Default.ApiKey}&units=metric&lang={Properties.Settings.Default.Language}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // JSON parsen mit System.Text.Json
                    using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                    {
                        JsonElement root = doc.RootElement;
                        JsonElement wind = root.GetProperty("wind");
                        double speed = wind.GetProperty("speed").GetDouble();
                        double deg = wind.GetProperty("deg").GetDouble();

                        //Console.WriteLine($"Windgeschwindigkeit: {speed} m/s");
                        //Console.WriteLine($"Windrichtung: {deg}°");
                        string direction = Compass.GetDirectionName(deg);
                        int bft = ConvertMpsToBeaufort(speed);
                        winddirectionLabel.Text = $"{deg}° ({direction})";
                        windspeedLabel.Text = $"{speed} m/s ({bft} Bft)";

                        string cityName = root.GetProperty("name").GetString();
                        double temp = root.GetProperty("main").GetProperty("temp").GetDouble();
                        string description = root.GetProperty("weather")[0].GetProperty("description").GetString();
                        string weatherIcon = root.GetProperty("weather")[0].GetProperty("icon").GetString();
                        LastWeatherData.GetPngPath(weatherIcon);

                        weatherPictureBox.ImageLocation = LastWeatherData.GetPngPath(weatherIcon);
                        weatherLogLabel.Text = String.Empty;

                        //Console.WriteLine($"Stadt: {cityName}");
                        locationComboBox.SelectedText = $"{cityName}";

                        //Console.WriteLine($"Temperatur: {temp} °C");
                        tempLabel.Text = $"{temp} °C";

                        //Console.WriteLine($"Zustand: {description}");
                        cloudLabel.Text = $"{description}";

                        timestampLabel.Text = $"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}";

                        LastWeatherData.Temperature = temp;
                        LastWeatherData.WindSpeed = speed;
                        LastWeatherData.WindDirection = deg;
                        LastWeatherData.WindCompass = direction;
                        LastWeatherData.WeatherDescription = description;
                        LastWeatherData.WeatherId = weatherIcon;
                        LastWeatherData.ImageLocation = LastWeatherData.GetPngPath(weatherIcon);
                        LastWeatherData.Timestamp = DateTime.Now;
                    }
                }
                catch (HttpRequestException e)
                {
                    //Console.WriteLine($"Fehler bei der Anfrage: {e.Message}");
                    weatherLogLabel.Text = $"{errorReadingWeatherText} {e.Message}";
                }
            }
        }

        public async Task GetWeatherDataAsync(string cityName)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={Properties.Settings.Default.ApiKey}&units=metric&lang={Properties.Settings.Default.Language}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    // JSON parsen mit System.Text.Json
                    using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                    {
                        JsonElement root = doc.RootElement;
                        JsonElement wind = root.GetProperty("wind");
                        double speed = wind.GetProperty("speed").GetDouble();
                        double deg = wind.GetProperty("deg").GetDouble();
                        //Console.WriteLine($"Windgeschwindigkeit: {speed} m/s");
                        //Console.WriteLine($"Windrichtung: {deg}°");
                        string direction = Compass.GetDirectionName(deg);
                        int bft = ConvertMpsToBeaufort(speed);
                        winddirectionLabel.Text = $"{deg}° ({direction})";
                        windspeedLabel.Text = $"{speed} m/s ({bft} Bft)";
                        string cityNameResponse = root.GetProperty("name").GetString();
                        double temp = root.GetProperty("main").GetProperty("temp").GetDouble();
                        string description = root.GetProperty("weather")[0].GetProperty("description").GetString();
                        string weatherIcon = root.GetProperty("weather")[0].GetProperty("icon").GetString();
                        
                        weatherPictureBox.ImageLocation = LastWeatherData.GetPngPath(weatherIcon);
                        weatherLogLabel.Text = String.Empty;

                        //Console.WriteLine($"Stadt: {cityName}");
                        locationComboBox.SelectedText = $"{cityName}";

                        //Console.WriteLine($"Temperatur: {temp} °C");
                        tempLabel.Text = $"{temp} °C";

                        //Console.WriteLine($"Zustand: {description}");
                        cloudLabel.Text = $"{description}";

                        timestampLabel.Text = $"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}";

                        LastWeatherData.Temperature = temp;
                        LastWeatherData.WindSpeed = speed;
                        LastWeatherData.WindDirection = deg;
                        LastWeatherData.WindCompass = direction;
                        LastWeatherData.WeatherDescription = description;
                        LastWeatherData.WeatherId = weatherIcon;
                        LastWeatherData.ImageLocation = LastWeatherData.GetPngPath(weatherIcon);
                        LastWeatherData.Timestamp = DateTime.Now;
                    }
                }
                catch (HttpRequestException e)
                {
                    //Console.WriteLine($"Fehler bei der Anfrage: {e.Message}");
                    weatherLogLabel.Text = $"{errorReadingWeatherText} {e.Message}";
                }
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

        private void LoadCityList()
        {
            locationComboBox.Items.Clear();
            string cityCollection = Properties.Settings.Default.ManualCity;
            if (!string.IsNullOrEmpty(cityCollection))
            {
                var cities = cityCollection.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                locationComboBox.Items.AddRange(cities);
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

                    string bike = assignmentDictionary.GetBikeForTacho(info.SerialNumber.ToString());
                    //Console.WriteLine($"Serial: {info.SerialNumber}, Assigned bike: {bike}");
                    foreach (var item in bikeComboBox.Items)
                    {
                        if (item.ToString() == bike)
                        {
                            bikeComboBox.SelectedItem = item;
                            //Console.WriteLine($"Bike '{item}' selected for serial {info.SerialNumber}");
                            break;
                        }
                    }

                    distanceTextBox.Text = data.DistanceKm.ToString("F2");
                    timeTextBox.Text = data.Duration.ToString(@"h\:mm\:ss");
                    vavgTextBox.Text = data.MeanSpeedKmh.ToString("F2");
                    vmaxTextBox.Text = data.MaxSpeedKmh.ToString("F2");
                    cadenceTextBox.Text = data.Cadence.ToString();

                    // Fahrrad vorauswählen basierend auf Seriennummer
                    //SelectBikeBySerial(info.SerialNumber);

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
            // Mapping Seriennummer -> Fahrrad speichern
            if (lastDetectedComputer != null && !string.IsNullOrEmpty(bikeComboBox.Text))
            {
                assignmentDictionary.AddAssignment(lastDetectedComputer.SerialNumber.ToString(), bikeComboBox.Text);
                Properties.Settings.Default.Save();
            }
        }

        private void einstellungenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            if (settingsForm.ShowDialog(this) == DialogResult.OK) 
            { 
                LoadBikeList();
                LastWeatherData.RootDir = Properties.Settings.Default.SaveFolder;
                LastWeatherData.CheckPngDir();
            }
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
                TachoName = lastDetectedComputer?.ModelName ?? unknownText,
                BikeName = bikeComboBox.Text,
                DistanceMeters = distanceMeters,
                TimeSeconds = timeSeconds,
                MeanSpeedKmh = double.TryParse(vavgTextBox.Text, out double vavg) ? vavg : 0,
                MaxSpeedKmh = double.TryParse(vmaxTextBox.Text, out double vmax) ? vmax : 0,
                Cadence = (byte)(byte.TryParse(cadenceTextBox.Text, out byte cad) ? cad : 0),
                Timestamp = dateTimePicker.Value,
                TripSectionDistanceMeters = (uint)(double.TryParse(tsDistanceTextBox.Text, out double tsDist) ? tsDist * 1000 : 0),
                TripSectionTimeSeconds = (uint)(TimeSpan.TryParse(tsTimeTextBox.Text, out TimeSpan tsDur) ? tsDur.TotalSeconds : 0),
                MinAltitudeMeters = double.TryParse(minHeightTextBox.Text, out double minh) ? minh : 0,
                MaxAltitudeMeters = double.TryParse(maxHeightTextBox.Text, out double maxh) ? maxh : 0,
                // Wetterdaten (12.8.26)
                WeatherId = LastWeatherData.WeatherId ?? string.Empty,
                Weather = LastWeatherData.WeatherDescription ?? string.Empty,
                WindSpeed = LastWeatherData.WindSpeed,
                WindDirection = LastWeatherData.WindDirection,
                WindCompass = LastWeatherData.WindCompass ?? string.Empty,
                Temperature = LastWeatherData.Temperature,
                WeatherTimestamp = LastWeatherData.Timestamp,
                WeatherIcon = LastWeatherData.ImageLocation ?? string.Empty,
                // HeartRate und NormalizedPower (14.8.26)
                HeartRate = double.TryParse(avgHeartRateTextBox.Text, out double avgHr) ? avgHr : 0,
                NormalizedPower = double.TryParse(normalizedPowerTextBox.Text, out double np) ? np : 0
            };

            string fileName = "bikedata_" + dateTimePicker.Value.ToString("yyyyMMdd-HHmm") + ".json";
            string jsonString = JsonSerializer.Serialize(rideData);
            string fullpath = Path.Combine(Properties.Settings.Default.SaveFolder, fileName);
            File.WriteAllText(fullpath, jsonString);
            if (File.Exists(fullpath))
            {
                MessageBox.Show(fileSavedText + fullpath, successText, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(fileSaveErrorText, errorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }   
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
            if (Properties.Settings.Default.WindowSize != Size.Empty)
            {
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
            if (Properties.Settings.Default.UseWeather)
            {
                weatherToolStripButton.Enabled = true;
            }
            else
            {
                weatherToolStripButton.Enabled = false;
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
                label12.Text = "Min Höhenmeter [m ü.NN]";
                label13.Text = "Max Höhenmeter [m ü.NN]";
                label16.Text = "Ø Herzfrequenz [min⁻¹]";
                label20.Text = "Normalisierte Leistung [W]";

                //Wetter Tab
                label2.Text = "Wetter";
                label15.Text = "Temperatur";
                label17.Text = "Ort";
                label18.Text = "Windgeschwindigkeit";
                label19.Text = "Windrichtung";
                label14.Text = "Uhrzeit";
                addCityButton.Text = "&Hinzufügen";

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
                fileSavedText = "Datei erfolgreich gespeichert: ";
                fileSaveErrorText = "Fehler beim Speichern der Datei.";
                tachoMitRadVerknüpfenToolStripMenuItem.Text = "Tacho mit Rad verknüpfen";
                if (Properties.Settings.Default.UseWeather)
                    weatherLogLabel.Text = "Bitte erst Wetterdaten abrufen...";
                else 
                    weatherLogLabel.Text = "Wetterdaten deaktiviert";
                weatherToolStripButton.Text = "Wetterdaten abrufen";

                errorReadingLocationText = "Fehler beim Abrufen der Standortdaten.\nBitte Standortdienste aktivieren.";
                weatherDataDisabledText = "Wetterdaten deaktiviert";
                weatherDataEnabledText = "Bitte erst Wetterdaten abrufen...";
                weatherDataButtonText = "Wetterdaten abrufen";
                enterCityOrEnableLocationText = "Bitte geben Sie eine Stadt ein oder aktivieren Sie die automatische Standortbestimmung.";
                errorReadingWeatherText = "Fehler bei der Anfrage:";

                tabControl1.TabPages[0].Text = "Tacho";
                tabControl1.TabPages[1].Text = "Wetter";
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
                label12.Text = "Min height [m aSL]";
                label13.Text = "Max height [m aSL]";
                label16.Text = "Ø Heart rate [min⁻¹]";
                label20.Text = "Normalized power [W]";

                // Weather tab
                label2.Text = "Weather";
                label15.Text = "Temperature";
                label17.Text = "Location";
                label18.Text = "Wind speed";
                label19.Text = "Wind direction";
                label14.Text = "Time";
                addCityButton.Text = "&Add";

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
                fileSavedText = "File successfully saved: ";
                fileSaveErrorText = "Error saving file.";
                tachoMitRadVerknüpfenToolStripMenuItem.Text = "Assign speedometer";
                if (Properties.Settings.Default.UseWeather)
                    weatherLogLabel.Text = "Please fetch weather data first...";
                else
                    weatherLogLabel.Text = "Weather data disabled";
                weatherToolStripButton.Text = "Fetch weather data";

                errorReadingLocationText = "Error retrieving location data.\nPlease enable location services.";
                weatherDataDisabledText = "Weather data disabled";
                weatherDataEnabledText = "Please fetch weather data first...";
                weatherDataButtonText = "Fetch weather data";
                enterCityOrEnableLocationText = "Please enter a city or enable automatic location detection.";
                errorReadingWeatherText = "Error during request:";

                tabControl1.TabPages[0].Text = "Speedometer";
                tabControl1.TabPages[1].Text = "Weather";
            }
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Language = "en";
            Properties.Settings.Default.Save();
            SetLanguage("en");
        }

        private void tachoMitRadVerknüpfenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BikeInfoForm bikeInfoForm = new BikeInfoForm();
            if (bikeInfoForm.ShowDialog(this) == DialogResult.OK)
            {
                LoadBikeList();
            }
        }

        private async void weatherToolStripButton_Click(object sender, EventArgs e)
        {
            GetLocationProperty();
            if (Properties.Settings.Default.UseLocalization)
            {
                if (!locationFound)
                {
                    MessageBox.Show(errorReadingLocationText, errorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    await GetCoordinate();
                }
            }
            if (!Properties.Settings.Default.UseLocalization && !string.IsNullOrWhiteSpace(Properties.Settings.Default.ManualCity))
            {
                await GetWeatherDataAsync(Properties.Settings.Default.ManualCity);
            }
            else if (!Properties.Settings.Default.UseLocalization && string.IsNullOrWhiteSpace(Properties.Settings.Default.ManualCity))
            {
                MessageBox.Show(enterCityOrEnableLocationText, errorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static int ConvertMpsToBeaufort(double mps)
        {
            if (mps < 0) return 0;
            if (mps < 0.3) return 0;
            if (mps < 1.6) return 1;
            if (mps < 3.4) return 2;
            if (mps < 5.5) return 3;
            if (mps < 8.0) return 4;
            if (mps < 10.8) return 5;
            if (mps < 13.9) return 6;
            if (mps < 17.2) return 7;
            if (mps < 20.8) return 8;
            if (mps < 24.5) return 9;
            if (mps < 28.5) return 10;
            if (mps < 32.7) return 11;
            return 12; // ab 32.7 m/s ist es Beaufort 12
        }

        private void addCityButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(locationComboBox.Text))
            {
                string newCity = locationComboBox.Text.Trim();
                if (!locationComboBox.Items.Contains(newCity))
                {
                    locationComboBox.Items.Add(newCity);
                    // Save to settings
                    var cities = locationComboBox.Items.Cast<string>().ToArray();
                    Properties.Settings.Default.ManualCity = string.Join(";", cities);
                    Properties.Settings.Default.Save();
                }
            }
        }
    }
}
