using System;

namespace SigmaNotificationApp
{
    public class WeatherData
    {
        public string WeatherDescription { get; set; }
        public string WeatherId { get; set; }
        public string ImageLocation { get; set; }
        public double WindSpeed { get; set; }
        public double WindDirection { get; set; }
        public string WindCompass { get; set; }
        public double Temperature { get; set; }
        public DateTime Timestamp { get; set; }
        public string RootDir { get; set; }

        public static string PNG_DIR = "WeatherPNGs";

        public WeatherData(string root_dir)
        {
            WeatherDescription = string.Empty;
            WeatherId = string.Empty;
            ImageLocation = string.Empty;
            WindSpeed = 0;
            WindDirection = 0;
            WindCompass = string.Empty;
            Temperature = 0;
            Timestamp = DateTime.Now;
            RootDir = root_dir;

            CheckPngDir();
        }

        public void CheckPngDir()
        {
            if (!System.IO.Directory.Exists(RootDir + "\\" + PNG_DIR))
            {
                System.IO.Directory.CreateDirectory(RootDir + "\\" + PNG_DIR);
            }
        }

        public bool CheckIfPngExists(string pngName)
        {
            string pngPath = System.IO.Path.Combine(RootDir, PNG_DIR, pngName);
            return System.IO.File.Exists(pngPath);
        }

        public void SavePng(byte[] pngData, string pngName)
        {
            string pngPath = System.IO.Path.Combine(RootDir, PNG_DIR, pngName);
            System.IO.File.WriteAllBytes(pngPath, pngData);
        }

        public string GetPngPath(string weatherIcon)
        {
            //$"http://openweathermap.org/img/wn/{weatherIcon}@2x.png";
            string pngName = weatherIcon + "@2x.png";
            if (!CheckIfPngExists(pngName))
            {
                // Download the PNG from OpenWeatherMap
                using (var client = new System.Net.WebClient())
                {
                    string url = $"http://openweathermap.org/img/wn/{weatherIcon}@2x.png";
                    byte[] pngData = client.DownloadData(url);
                    SavePng(pngData, pngName);
                }
            }
            return System.IO.Path.Combine(RootDir, PNG_DIR, pngName);
        }
    }
}
