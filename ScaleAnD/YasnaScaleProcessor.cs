using System;
using System.Globalization;
using System.IO.Ports;
using YasnaScaleInterface;

namespace ScaleAnD
{
    public class YasnaScaleProcessor : IScaleProcessor
    {
        private SerialPort _port;

        public string GetName(int lcid)
        {
            return "ترازوی A&D";
        }

        public void StartMonitoring()
        {
            if (Port.IsOpen)
            {
                Port.Close();
                _port.DataReceived += Port_DataReceived;
                Port.Open();
                Port.DiscardInBuffer();
                
            }
            else
            {
                _port.DataReceived += Port_DataReceived;
                Port.Open();
                Port.DiscardInBuffer();
            }

        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (e.EventType == SerialData.Chars && this.Port.BytesToRead > 16)
                {
                    string str = this.Port.ReadExisting();
                    LogStringObject?.Invoke("DataReceived", str);
                    if (str.StartsWith("ST"))
                    {
                        var decGr = Convert.ToDecimal(str.Substring(4, 8));
                        WeightReceived?.Invoke(new WeightReceivedEventArgs(decGr / 1000));
                        LogStringObject?.Invoke("WeightReceivedEventArgs",
                            (decGr / 1000).ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        if (!str.Contains("ST")) return;
                        str = leftRotateShift(str, str.IndexOf("ST", StringComparison.Ordinal));
                        var decGr = Convert.ToDecimal(str.Substring(4, 8));
                        WeightReceived?.Invoke(new WeightReceivedEventArgs(decGr / 1000));
                        LogStringObject?.Invoke("WeightReceivedEventArgs",
                            (decGr / 1000).ToString(CultureInfo.InvariantCulture));
                    }
                }

                //ST,+00000.00  g
                
            }
            catch (System.Exception ex)
            {
                LogInternalException?.Invoke("Port_DataReceived", ex);
            }
        }

        public SerialPort Port
        {
            get
            {
                if (_port != null)
                {
                    return _port;
                }
                _port = new SerialPort
                {
                    BaudRate = 2400,
                    Parity = Parity.Even,
                    StopBits = StopBits.One,
                    DataBits = 7,
                    PortName = PortName,
                    ReceivedBytesThreshold = 16
                };
                return _port;
            }
            set { _port = value; }
        }

        public void StopMonitoring()
        {
            if (Port.IsOpen)
            {
                try
                {
                    _port.DataReceived -= Port_DataReceived;
                }
                catch (Exception)
                {
                    // ignored
                }
                Port.Close();
            }
        }

        public string PortName { get; set; }
        public string BaudRate { get; set; }
        public event WeightReceivedDeligate WeightReceived;
        public event LogInternalExceptionDeligate LogInternalException;
        public event LogStringObjectDeligate LogStringObject;

        private string leftRotateShift(string key, int shift)
        {
            shift %= key.Length;
            return key.Substring(shift) + key.Substring(0, shift);
        }

        private string rightRotateShift(string key, int shift)
        {
            shift %= key.Length;
            return key.Substring(key.Length - shift) + key.Substring(0, key.Length - shift);
        }
    }
}
