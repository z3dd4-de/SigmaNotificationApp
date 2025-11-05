using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using System.Text.Json;

namespace SigmaNotificationApp
{
    internal class TachoData
    {
        public string TachoName { get; set; }
        public string BikeName { get; set; }
        public float Distance { get; set; }
        public TimeSpan Time { get; set; }
        public float Vavg { get; set; }
        public float Vmax { get; set; }
        public float Cadence { get; set; }
        public float TsDistance { get; set; }
        public TimeSpan TsTime { get; set; }
    }
}
