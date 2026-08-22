using System;

namespace SigmaNotificationApp
{
    internal class Compass
    {
        private static readonly string[] DirectionNames = new string[]
        {
            "N", "NNO", "NO", "ONO", "O", "OSO", "SO", "SSO",
            "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW"
        };

        public static string GetDirectionName(double degrees)
        {
            // Normalisiert den Wert auf den Bereich [0, 360)
            degrees = (degrees % 360 + 360) % 360;

            // Berechnet den Index (22.5 Grad pro Schritt für 16 Richtungen)
            // Das Addieren von 11.25 sorgt für die korrekte kaufmännische Rundung zur nächsten Richtung
            int index = (int)Math.Floor((degrees + 11.25) / 22.5);

            // Falls über 348.75 Grad gerundet wird, springt der Index auf 16, was wieder Norden (0) entspricht
            return DirectionNames[index % 16];
        }
    }
}
