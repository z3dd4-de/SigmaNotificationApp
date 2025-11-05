using System;
using System.IO.Ports;
using System.Threading;

namespace SigmaDockingLib
{
    public enum BikeComputerModel
    {
        Unknown = 0,
        BC1612 = 0x15,
        BC1212 = 0x12,
        BC1612_STS = 0x16
    }

    public class RideData
    {
        public string TachoName { get; set; }
        public string BikeName { get; set; }
        public DateTime Timestamp { get; set; }
        public uint DistanceMeters { get; set; }
        public uint TimeSeconds { get; set; }
        public double MeanSpeedKmh { get; set; }
        public double MaxSpeedKmh { get; set; }
        public byte Cadence { get; set; }
        public uint TripSectionDistanceMeters { get; set; }
        public uint TripSectionTimeSeconds { get; set; }

        public double DistanceKm => DistanceMeters / 1000.0;
        public TimeSpan Duration => TimeSpan.FromSeconds(TimeSeconds);
        public TimeSpan TripSectionDuration => TimeSpan.FromSeconds(TripSectionTimeSeconds);
    }

    public class BikeComputerInfo
    {
        public BikeComputerModel Model { get; set; }
        public string ModelName { get; set; } = "";
        public byte Version { get; set; }
        public string SerialNumber { get; set; } = "";
        public byte Type { get; set; }
    }

    public class SigmaReader
    {
        private const int TIMEOUT_MSEC = 6000;
        private const int TIMEOUT_SHORT = 1000;

        public event EventHandler<string>? LogMessage;

        private void Log(string message)
        {
            LogMessage?.Invoke(this, message);
        }

        /// <summary>
        /// Checks if a bike computer is present in the cradle
        /// </summary>
        public bool IsBikeComputerPresent(string? portName = null)
        {
            try
            {
                string port = portName ?? FindSigmaPort() ?? "COM7";

                using (var serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One))
                {
                    serialPort.ReadTimeout = TIMEOUT_SHORT;
                    serialPort.WriteTimeout = TIMEOUT_SHORT;
                    serialPort.Open();

                    serialPort.Write(new byte[] { 0xf4 }, 0, 1);
                    Thread.Sleep(100);

                    if (serialPort.BytesToRead > 0)
                    {
                        return serialPort.ReadByte() == 1;
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Finds the COM port with a Sigma device
        /// </summary>
        public string? FindSigmaPort()
        {
            var ports = SerialPort.GetPortNames();

            foreach (var portName in ports)
            {
                try
                {
                    using (var port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One))
                    {
                        port.ReadTimeout = TIMEOUT_SHORT;
                        port.WriteTimeout = TIMEOUT_SHORT;
                        port.Open();
                        port.Write(new byte[] { 0xf4 }, 0, 1);
                        Thread.Sleep(100);

                        if (port.BytesToRead > 0)
                        {
                            return portName;
                        }
                    }
                }
                catch { }
            }

            return null;
        }

        /// <summary>
        /// Reads bike computer information (model, serial, etc.)
        /// </summary>
        public BikeComputerInfo? ReadBikeComputerInfo(string? portName = null)
        {
            try
            {
                string port = portName ?? FindSigmaPort() ?? "COM7";

                using (var serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One))
                {
                    serialPort.ReadTimeout = TIMEOUT_MSEC;
                    serialPort.WriteTimeout = TIMEOUT_MSEC;
                    serialPort.Open();

                    Log($"Connected to {port}");

                    // Wait for unit
                    if (!WaitForUnit(serialPort))
                    {
                        Log("No unit found in cradle");
                        return null;
                    }

                    Log("Unit found in cradle");

                    // Send identify
                    serialPort.DiscardInBuffer();
                    serialPort.Write(new byte[] { 0xfe }, 0, 1);
                    Thread.Sleep(500);

                    // Send data fetch - returns identify data
                    serialPort.Write(new byte[] { 0xfb }, 0, 1);
                    Thread.Sleep(1500);

                    // Read identify (11 bytes)
                    byte[] buf = new byte[32];
                    int total = ReadBytes(serialPort, buf, 11);

                    if (total != 11)
                    {
                        Log($"Identify failed: got {total} bytes");
                        return null;
                    }

                    var info = new BikeComputerInfo
                    {
                        Type = buf[0],
                        Version = buf[6],
                        SerialNumber = $"{buf[2]}{buf[3]}{buf[4]}{buf[5]}"
                    };

                    // Determine model
                    info.Model = buf[1] switch
                    {
                        0x15 => BikeComputerModel.BC1612,
                        0x12 => BikeComputerModel.BC1212,
                        0x16 => BikeComputerModel.BC1612_STS,
                        _ => BikeComputerModel.Unknown
                    };

                    info.ModelName = info.Model switch
                    {
                        BikeComputerModel.BC1612 => "BC 16.12",
                        BikeComputerModel.BC1212 => "BC 12.12",
                        BikeComputerModel.BC1612_STS => "BC 16.12 STS",
                        _ => $"Unknown (0x{buf[1]:x2})"
                    };

                    Log($"Model: {info.ModelName}, Version: {info.Version}, Serial: {info.SerialNumber}");

                    return info;
                }
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Reads ride data from the bike computer
        /// </summary>
        public RideData? ReadRideData(string? portName = null)
        {
            try
            {
                string port = portName ?? FindSigmaPort() ?? "COM7";

                using (var serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One))
                {
                    serialPort.ReadTimeout = TIMEOUT_MSEC;
                    serialPort.WriteTimeout = TIMEOUT_MSEC;
                    serialPort.Open();

                    Log($"Connected to {port}");

                    // Wait for unit
                    if (!WaitForUnit(serialPort))
                    {
                        Log("No unit found in cradle");
                        return null;
                    }

                    // Send identify sequence
                    serialPort.DiscardInBuffer();
                    serialPort.Write(new byte[] { 0xfe }, 0, 1);
                    Thread.Sleep(500);
                    serialPort.Write(new byte[] { 0xfb }, 0, 1);
                    Thread.Sleep(1500);

                    // Read and discard identify data
                    byte[] buf = new byte[32];
                    ReadBytes(serialPort, buf, 11);

                    Log("Fetching ride data...");

                    // Fetch ride data
                    serialPort.DiscardInBuffer();
                    serialPort.Write(new byte[] { 0xfb }, 0, 1);
                    Thread.Sleep(1500);

                    int total = ReadBytes(serialPort, buf, 27, timeout: 3000);

                    if (total == 0)
                    {
                        Log("No data, trying trigger...");
                        serialPort.Write(new byte[] { 0xf4 }, 0, 1);
                        Thread.Sleep(1500);
                        total = ReadBytes(serialPort, buf, 27, timeout: 3000);
                    }

                    if (total < 27)
                    {
                        Log($"Data fetch failed: got {total} bytes (expected 27)");
                        return null;
                    }

                    Log("Data received successfully");

                    // Decode data
                    var data = new RideData
                    {
                        DistanceMeters = (uint)(buf[1] * 65536 + buf[2] * 256 + buf[3]),
                        TimeSeconds = (uint)(buf[4] * 65536 + buf[5] * 256 + buf[6]),
                        MeanSpeedKmh = (buf[7] * 256 + buf[8]) / 100.0,
                        MaxSpeedKmh = (buf[9] * 256 + buf[10]) / 100.0,
                        Cadence = buf[11],
                        TripSectionDistanceMeters = (uint)(buf[14] * 65536 + buf[15] * 256 + buf[16]),
                        TripSectionTimeSeconds = (uint)(buf[17] * 65536 + buf[18] * 256 + buf[19])
                    };

                    return data;
                }
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Reads both bike computer info and ride data in one operation
        /// </summary>
        public (BikeComputerInfo? info, RideData? data) ReadComplete(string? portName = null)
        {
            try
            {
                string port = portName ?? FindSigmaPort() ?? "COM7";

                using (var serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One))
                {
                    serialPort.ReadTimeout = TIMEOUT_MSEC;
                    serialPort.WriteTimeout = TIMEOUT_MSEC;
                    serialPort.Open();

                    Log($"Connected to {port}");

                    if (!WaitForUnit(serialPort))
                    {
                        Log("No unit found in cradle");
                        return (null, null);
                    }

                    Log("Unit found in cradle");

                    // Get identification
                    serialPort.DiscardInBuffer();
                    serialPort.Write(new byte[] { 0xfe }, 0, 1);
                    Thread.Sleep(500);
                    serialPort.Write(new byte[] { 0xfb }, 0, 1);
                    Thread.Sleep(1500);

                    byte[] buf = new byte[32];
                    int total = ReadBytes(serialPort, buf, 11);

                    if (total != 11)
                    {
                        Log($"Identify failed: got {total} bytes");
                        return (null, null);
                    }

                    var info = new BikeComputerInfo
                    {
                        Type = buf[0],
                        Version = buf[6],
                        SerialNumber = $"{buf[2]}{buf[3]}{buf[4]}{buf[5]}"
                    };

                    info.Model = buf[1] switch
                    {
                        0x15 => BikeComputerModel.BC1612,
                        0x12 => BikeComputerModel.BC1212,
                        0x16 => BikeComputerModel.BC1612_STS,
                        _ => BikeComputerModel.Unknown
                    };

                    info.ModelName = info.Model switch
                    {
                        BikeComputerModel.BC1612 => "BC 16.12",
                        BikeComputerModel.BC1212 => "BC 12.12",
                        BikeComputerModel.BC1612_STS => "BC 16.12 STS",
                        _ => $"Unknown (0x{buf[1]:x2})"
                    };

                    Log($"Model: {info.ModelName}, Version: {info.Version}, Serial: {info.SerialNumber}");

                    // Get ride data
                    Log("Fetching ride data...");
                    serialPort.DiscardInBuffer();
                    serialPort.Write(new byte[] { 0xfb }, 0, 1);
                    Thread.Sleep(1500);

                    total = ReadBytes(serialPort, buf, 27, timeout: 3000);

                    if (total == 0)
                    {
                        Log("No data, trying trigger...");
                        serialPort.Write(new byte[] { 0xf4 }, 0, 1);
                        Thread.Sleep(1500);
                        total = ReadBytes(serialPort, buf, 27, timeout: 3000);
                    }

                    if (total < 27)
                    {
                        Log($"Data fetch failed: got {total} bytes");
                        return (info, null);
                    }

                    Log("Data received successfully");

                    var data = new RideData
                    {
                        DistanceMeters = (uint)(buf[1] * 65536 + buf[2] * 256 + buf[3]),
                        TimeSeconds = (uint)(buf[4] * 65536 + buf[5] * 256 + buf[6]),
                        MeanSpeedKmh = (buf[7] * 256 + buf[8]) / 100.0,
                        MaxSpeedKmh = (buf[9] * 256 + buf[10]) / 100.0,
                        Cadence = buf[11],
                        TripSectionDistanceMeters = (uint)(buf[14] * 65536 + buf[15] * 256 + buf[16]),
                        TripSectionTimeSeconds = (uint)(buf[17] * 65536 + buf[18] * 256 + buf[19])
                    };

                    return (info, data);
                }
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message}");
                return (null, null);
            }
        }

        /// <summary>
        /// Clears the bike computer's counters
        /// </summary>
        public bool ClearCounters(string? portName = null)
        {
            try
            {
                string port = portName ?? FindSigmaPort() ?? "COM7";

                using (var serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One))
                {
                    serialPort.ReadTimeout = TIMEOUT_MSEC;
                    serialPort.WriteTimeout = TIMEOUT_MSEC;
                    serialPort.Open();

                    if (!WaitForUnit(serialPort))
                        return false;

                    // Send reset command
                    byte[] resetCmd = new byte[] { 0xf0, 0x02 };
                    serialPort.Write(resetCmd, 0, resetCmd.Length);
                    Thread.Sleep(500);

                    // Trigger response
                    serialPort.Write(new byte[] { 0xf4 }, 0, 1);
                    Thread.Sleep(1500);

                    if (serialPort.BytesToRead > 0)
                    {
                        byte response = (byte)serialPort.ReadByte();
                        if (response == 0)
                        {
                            Log("Counters cleared successfully");
                            return true;
                        }
                    }

                    Log("Clear counters failed");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log($"Error clearing counters: {ex.Message}");
                return false;
            }
        }

        private bool WaitForUnit(SerialPort port, int maxAttempts = 30)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                port.DiscardInBuffer();
                port.Write(new byte[] { 0xf4 }, 0, 1);
                Thread.Sleep(100);

                if (port.BytesToRead > 0 && port.ReadByte() == 1)
                    return true;

                Thread.Sleep(1000);
            }
            return false;
        }

        private int ReadBytes(SerialPort port, byte[] buf, int expected, int timeout = 2000)
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
    }
}
