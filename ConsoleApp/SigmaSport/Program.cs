using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace SigmaDockingStation
{
    class Config
    {
        public const int TIMEOUT_MSEC = 6000;
        public const int TIMEOUT_SHORT = 1000;
    }

    enum BikeComputerModel
    {
        Unknown = 0,
        BC1612 = 0x15,
        BC1212 = 0x12,
        BC1612_STS = 0x16,
        AutoDetect = 0xFF
    }

    class Flags
    {
        public bool Clear { get; set; }
        public bool Dump { get; set; }
        public bool Miles { get; set; }
        public bool Ts { get; set; }
        public bool Zeros { get; set; }
        public bool Raw { get; set; }
        public bool Remove { get; set; }
        public bool Debug { get; set; }
        public bool Run { get; set; }
        public BikeComputerModel Model { get; set; }

        public Flags()
        {
            Clear = false;
            Dump = false;
            Miles = false;
            Ts = true;
            Zeros = true;
            Raw = false;
            Remove = false;
            Debug = false;
            Run = true;
            Model = BikeComputerModel.AutoDetect;
        }
    }

    class Program
    {
        static void Msg(string format, params object[] args)
        {
            Console.Error.WriteLine($"# {string.Format(format, args)}");
        }

        static void HexDump(string info, byte[] data, int count)
        {
            Console.Error.Write($"# {info}");
            for (int i = 0; i < Math.Min(count, data.Length); i++)
            {
                Console.Error.Write($" {data[i]:x2}");
            }
            Console.Error.WriteLine();
        }

        static void Main(string[] args)
        {
            var flags = new Flags();

            foreach (var arg in args)
            {
                switch (arg)
                {
                    case "--clear": flags.Clear = true; break;
                    case "--dump": flags.Dump = true; break;
                    case "--miles": flags.Miles = true; break;
                    case "--no-ts": flags.Ts = false; break;
                    case "--no-zeros": flags.Zeros = false; break;
                    case "--raw": flags.Raw = true; break;
                    case "--wait-remove": flags.Remove = true; break;
                    case "--debug": flags.Debug = true; flags.Dump = true; break;
                    case "--bc1612": flags.Model = BikeComputerModel.BC1612; break;
                    case "--bc1212": flags.Model = BikeComputerModel.BC1212; break;
                    case "--bc1612-sts": flags.Model = BikeComputerModel.BC1612_STS; break;
                    case "--auto": flags.Model = BikeComputerModel.AutoDetect; break;
                    default:
                        if (!arg.StartsWith("--")) continue;
                        flags.Run = false;
                        break;
                }
            }

            if (flags.Run)
                Run(flags);
            else
            {
                Msg("Usage: --clear --dump --miles --no-ts --no-zeros --raw --wait-remove --debug");
                Msg("       --bc1612 --bc1612-sts --bc1212 --auto");
            }
        }

        static void Run(Flags flags)
        {
            string? sigmaPort = null;
            var ports = SerialPort.GetPortNames();
            
            Msg("Available COM ports: {0}", string.Join(", ", ports));

            foreach (var portName in ports)
            {
                try
                {
                    using (var port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One))
                    {
                        port.ReadTimeout = Config.TIMEOUT_SHORT;
                        port.WriteTimeout = Config.TIMEOUT_SHORT;
                        port.Open();
                        port.Write(new byte[] { 0xf4 }, 0, 1);
                        Thread.Sleep(100);
                        
                        if (port.BytesToRead > 0)
                        {
                            sigmaPort = portName;
                            break;
                        }
                    }
                }
                catch { }
            }

            if (sigmaPort == null) sigmaPort = "COM7";
            Msg("Found SIGMA device on {0}", sigmaPort);

            try
            {
                using (var port = new SerialPort(sigmaPort, 9600, Parity.None, 8, StopBits.One))
                {
                    port.ReadTimeout = Config.TIMEOUT_MSEC;
                    port.WriteTimeout = Config.TIMEOUT_MSEC;
                    port.Open();
                    Msg("cradle found on {0}", sigmaPort);
                    ReadData(flags, port);
                }
            }
            catch (Exception ex)
            {
                Msg("Error: {0}", ex.Message);
            }
        }

        static void ReadData(Flags flags, SerialPort port)
        {
            byte[] buf = new byte[32];

            // Wait for unit
            while (true)
            {
                port.DiscardInBuffer();
                port.Write(new byte[] { 0xf4 }, 0, 1);
                Thread.Sleep(100);
                
                if (port.BytesToRead > 0 && port.ReadByte() == 1)
                {
                    Msg("unit found in the cradle");
                    break;
                }
                Thread.Sleep(1000);
            }

            // Send identify
            port.DiscardInBuffer();
            port.Write(new byte[] { 0xfe }, 0, 1);
            if (flags.Dump) Msg("Sent: fe (identify)");
            Thread.Sleep(500);

            // Send data fetch - returns identify data
            port.Write(new byte[] { 0xfb }, 0, 1);
            if (flags.Dump) Msg("Sent: fb (data fetch)");
            Thread.Sleep(1500);

            // Read identify (11 bytes)
            int total = ReadBytes(port, buf, 11);
            if (total != 11)
            {
                Msg("Identify failed: got {0} bytes", total);
                return;
            }

            if (flags.Dump) HexDump("Identify:", buf, 11);
            
            string model = buf[1] == 0x16 ? "BC 16.12 STS" : $"Unknown (0x{buf[1]:x2})";
            Msg("Model: {0}, Version: {1}, Serial: {2}{3}{4}{5}", 
                model, buf[6], buf[2], buf[3], buf[4], buf[5]);

            // Try to get data - test different approaches
            Msg("Attempting to fetch ride data...");
            
            // Approach 1: Send another 0xfb
            port.DiscardInBuffer();
            port.Write(new byte[] { 0xfb }, 0, 1);
            if (flags.Dump) Msg("Sent: fb (attempt 1)");
            Thread.Sleep(1500);
            
            total = ReadBytes(port, buf, 27, timeout: 3000);
            
            if (total == 0)
            {
                // Approach 2: Send 0xf4 as trigger
                Msg("No data, trying 0xf4 trigger...");
                port.Write(new byte[] { 0xf4 }, 0, 1);
                if (flags.Dump) Msg("Sent: f4 (trigger)");
                Thread.Sleep(1500);
                total = ReadBytes(port, buf, 27, timeout: 3000);
            }
            
            if (total == 0)
            {
                // Approach 3: Send another sequence
                Msg("Still no data, trying full sequence...");
                port.DiscardInBuffer();
                port.Write(new byte[] { 0xfb }, 0, 1);
                Thread.Sleep(500);
                port.Write(new byte[] { 0xf4 }, 0, 1);
                Thread.Sleep(1500);
                total = ReadBytes(port, buf, 27, timeout: 3000);
            }

            if (total < 27)
            {
                Msg("Data fetch failed: got {0} bytes (expected 27)", total);
                if (total > 0 && flags.Dump)
                {
                    HexDump("Partial data:", buf, total);
                }
                Msg("Your device may need a different protocol");
                Msg("The original Rust code works - maybe check the TL2012 cradle firmware?");
                return;
            }

            if (flags.Dump) HexDump("Data:", buf, 27);
            DecodeData(buf, flags);
        }

        static int ReadBytes(SerialPort port, byte[] buf, int expected, int timeout = 2000)
        {
            int total = 0;
            int maxWait = timeout / 100;
            
            for (int i = 0; i < maxWait && total < expected; i++)
            {
                if (port.BytesToRead > 0)
                {
                    int available = Math.Min(port.BytesToRead, expected - total);
                    total += port.Read(buf, total, available);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            return total;
        }

        static void DecodeData(byte[] buf, Flags flags)
        {
            uint dist = (uint)(buf[1] * 65536 + buf[2] * 256 + buf[3]);
            uint seconds = (uint)(buf[4] * 65536 + buf[5] * 256 + buf[6]);
            ushort meansp = (ushort)(buf[7] * 256 + buf[8]);
            ushort maxsp = (ushort)(buf[9] * 256 + buf[10]);
            byte cadence = buf[11];
            uint tsdist = (uint)(buf[14] * 65536 + buf[15] * 256 + buf[16]);
            uint tseconds = (uint)(buf[17] * 65536 + buf[18] * 256 + buf[19]);

            double conv = flags.Miles ? 1.609344 : 1.0;
            string udist = flags.Miles ? "mi" : "km";
            string uspeed = flags.Miles ? "mph" : "km/hr";

            if (conv != 1.0)
            {
                dist = (uint)(dist / conv);
                meansp = (ushort)(meansp / conv);
                maxsp = (ushort)(maxsp / conv);
                tsdist = (uint)(tsdist / conv);
            }

            if (flags.Raw)
            {
                Console.WriteLine($"{dist},{seconds},{meansp / 100}.{meansp % 100:d2},{maxsp / 100}.{maxsp % 100:d2},{cadence},{tsdist},{tseconds}");
            }
            else
            {
                if (dist > 0 || flags.Zeros)
                    Console.WriteLine($"distance: {dist / 1000}.{(dist % 1000) / 10:d2} {udist}");
                if (seconds > 0 || flags.Zeros)
                    Console.WriteLine($"time: {seconds / 3600}:{(seconds % 3600) / 60:d2}:{seconds % 60:d2}");
                if (meansp > 0 || flags.Zeros)
                    Console.WriteLine($"meanspeed: {meansp / 100}.{meansp % 100:d2} {uspeed}");
                if (maxsp > 0 || flags.Zeros)
                    Console.WriteLine($"maxspeed: {maxsp / 100}.{maxsp % 100:d2} {uspeed}");
                if (cadence > 0 || flags.Zeros)
                    Console.WriteLine($"cadence: {cadence}/min");
                if (flags.Ts && (tsdist > 0 || flags.Zeros))
                    Console.WriteLine($"ts_dist: {tsdist / 1000}.{(tsdist % 1000) / 10:d2} {udist}");
                if (flags.Ts && (tseconds > 0 || flags.Zeros))
                    Console.WriteLine($"ts_time: {tseconds / 3600}:{(tseconds % 3600) / 60:d2}:{tseconds % 60:d2}");
            }
        }
    }
}