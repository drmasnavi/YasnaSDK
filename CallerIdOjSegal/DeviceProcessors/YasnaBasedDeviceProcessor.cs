using System;
using System.Collections.Generic;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdOjSegal.DeviceProcessors
{
    public abstract class YasnaBasedDeviceProcessor : ICallerIdDeviceProcessor, IComparable
    {
        private IPort _activePort;

        private Dictionary<string, ILine> _linesMap;

        public void OnDeviceChange(int mMsg, IntPtr mWParam, IntPtr mLParam, ref bool handled)
        {
        }

        public bool IsHid => false;

        public int CompareTo(object obj)
        {
            var processor = obj as ICallerIdDeviceProcessor;
            if (processor != null)
            {
                return LinesCount.CompareTo(processor.LinesCount);
            }
            return 1;
        }

        private LineStatus FindCallStatus(string strCallLog)
        {
            LineStatus lineStatus;
            if (strCallLog.IndexOf("L", StringComparison.Ordinal) != -1)
            {
                lineStatus = LineStatus.Ringing;
            }
            else if (strCallLog.IndexOf("HF", StringComparison.Ordinal) != -1)
            {
                lineStatus = LineStatus.HookOn;
            }
            else if (strCallLog.IndexOf("HND", StringComparison.Ordinal) == -1)
            {
                lineStatus = LineStatus.HookOn;
            }
            else
            {
                lineStatus = LineStatus.HookOff;
            }
            return lineStatus;
        }



        private int FindLineNumber(string strCallLog)
        {
            try
            {
                int num;
                var num1 = 1;
                while (true)
                {
                    if (num1 > this.LinesCount)
                    {
                        num = 0;
                        break;
                    }
                    if (strCallLog.IndexOf(this.GetLineKey(num1), StringComparison.Ordinal) == -1)
                    {
                        num1++;
                    }
                    else
                    {
                        num = num1;
                        break;
                    }
                }
                return num;
            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("FindLineNumber", ex);
            }
            return 0;

        }

        private string FindNumber(string strCallLog)
        {
            var str = string.Empty;
            try
            {
                var empty = string.Empty;
                if (strCallLog.IndexOf("L", StringComparison.Ordinal) == -1) return empty;
                var num = strCallLog.IndexOf("L", StringComparison.Ordinal);
                var num1 = strCallLog.IndexOf("@", num, StringComparison.Ordinal);
                var num2 = num + 2;
                empty = strCallLog.Substring(num2, num1 - num2);
                empty = FixPhoneNumber(empty);
                return empty;
            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("FindNumber", ex);
            }
            return str;
        }

        private int FindRingCounts(string strCallLog)
        {
            try
            {
                var num = 0;
                if (strCallLog.IndexOf("R", StringComparison.Ordinal) == -1) return num;
                var num1 = strCallLog.IndexOf("R", StringComparison.Ordinal);
                var num2 = strCallLog.IndexOf("@", num1, StringComparison.Ordinal);
                var num3 = num1 + 1;
                if (!int.TryParse(strCallLog.Substring(num3, num2 - num3), out num))
                {
                    num = 0;
                }
                return num;
            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("FindRingCounts", ex);
            }
            return 0;
        }

        private string GetLineKey(int lineNo)
        {
            return $"L{lineNo}";
        }

        private Dictionary<string, ILine> GetLinesMap()
        {
            if (_linesMap == null)
            {
                _linesMap = new Dictionary<string, ILine>(LinesCount);
                for (var i = 0; i < LinesCount; i++)
                {
                    ILine line = new YasnaCallerIdLine(i + 1);
                    _linesMap.Add(GetLineKey(line.LineNumber), line);
                }
            }
            return _linesMap;
        }

        public abstract string GetName(int lcid);



        private void SetHookOffTimerInterval(double interval)
        {
            if (interval > 0.0)
            {
                foreach (var line1 in Lines)
                {
                    var line = (YasnaCallerIdLine)line1;
                    line.SetTimerInterval(interval);
                }
            }
        }

        private void SetLinesState(string receivedData)
        {
            try
            {
                if (receivedData == null) return;
                var lineNo = FindLineNumber(receivedData);
                if (lineNo == 0) return;
                ILine line = null;
                if (GetLinesMap().ContainsKey(GetLineKey(lineNo)))
                {
                    line = GetLinesMap()[GetLineKey(lineNo)];
                }
                if (line == null) return;
                lock (line)
                {
                    var status = FindCallStatus(receivedData);
                    LogStringValue?.Invoke("Status", status.ToString());
                    if (status > 0)
                    {
                        line.Status = status;
                    }
                    SetDurationState(receivedData, line);
                    SetDialedNumberState(receivedData, line);
                    var str = FindNumber(receivedData);
                    if (str != string.Empty)
                    {
                        line.Number = str;
                        LogStringValue?.Invoke("PhoneNumber", str);
                    }
                    var num2 = FindRingCounts(receivedData);
                    if (num2 > 0)
                    {
                        line.RingCounts = num2;
                        LogIntValue?.Invoke("RingCount", num2);
                    }
                    line.LastUpdate = DateTime.UtcNow;
                }
            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("SetLinesState", ex);
            }
        }

        private void SetDurationState(string receivedData, ILine line)
        {
            var durationSecs = FindDuration(receivedData);
            LogIntValue?.Invoke("Duration", durationSecs);
            if (durationSecs >= 0)
            {
                line.DurationSecs = durationSecs;
            }
        }

        private void SetDialedNumberState(string receivedData, ILine line)
        {
            var dialedNumber = FindDialedNumber(receivedData);
            LogStringValue?.Invoke("DialedNumber", dialedNumber);
            if (!string.IsNullOrEmpty(dialedNumber))
            {
                line.DialedNumber = dialedNumber;
            }
        }

        private int FindDuration(string strCallLog)
        {
            var dur = -1;
            try
            {
                if (strCallLog.IndexOf("T", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    var index = strCallLog.IndexOf("T", StringComparison.OrdinalIgnoreCase);
                    var num2 = strCallLog.IndexOf("@", index, StringComparison.OrdinalIgnoreCase);
                    var startIndex = (index + "T".Length);
                    if (num2 == startIndex) return -1;
                    var str = strCallLog.Substring(startIndex, num2 - startIndex);
                    dur = Convert.ToInt32(str);
                }
            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("FindDuration", ex);
            }
            return dur;

        }

        private string FindDialedNumber(string strCallLog)
        {
            var str = string.Empty;
            try
            {
                if ((strCallLog.IndexOf("D", StringComparison.OrdinalIgnoreCase) != -1) && (strCallLog.IndexOf("HND", StringComparison.OrdinalIgnoreCase) == -1))
                {
                    var index = strCallLog.IndexOf("D", StringComparison.OrdinalIgnoreCase);
                    var num2 = strCallLog.IndexOf("@", index, StringComparison.OrdinalIgnoreCase);
                    var startIndex = (index + "D".Length);
                    if (num2 == startIndex) return String.Empty;
                    str = strCallLog.Substring(startIndex, num2 - startIndex);
                    str = FixPhoneNumber(str);
                }
            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("FindDialedNumber", ex);
            }
            return str;
        }

        private static string FixPhoneNumber(string str)
        {
            if (str.StartsWith("0098") || str.StartsWith("++98"))
            {
                str = $"0{str.Substring(4, str.Length - 4)}";
            }
            if (str.StartsWith("98"))
            {
                str = $"0{str.Substring(2, str.Length - 2)}";
            }
            if ((((str.StartsWith("911") || str.StartsWith("912")) || (str.StartsWith("913") || str.StartsWith("932"))) ||
                 (str.StartsWith("935") || str.StartsWith("936"))) || str.StartsWith("919"))
            {
                str = $"0{str}";
            }
            if ((((str.StartsWith("914") || str.StartsWith("915")) || (str.StartsWith("916") || str.StartsWith("917"))) ||
                (str.StartsWith("918") || str.StartsWith("937"))) || str.StartsWith("938"))
            {
                str = $"0{str}";
            }
            if ((((str.StartsWith("939") || str.StartsWith("920")) || (str.StartsWith("921") || str.StartsWith("901"))) ||
                (str.StartsWith("902") || str.StartsWith("903"))) || str.StartsWith("904"))
            {
                str = $"0{str}";
            }
            return str;
        }

        public void StartMonitoring(string port, string baudRate, IntPtr handle = default(IntPtr))
        {
            
            if (ActivePort == null) return;

            _activePort.PortDataReceived += ActivePort_PortDataReceived;
            LogObjectValue?.Invoke("ActivePort", ActivePort);
            LogStringValue?.Invoke("Port", port);
            LogStringValue?.Invoke("BaudRate", baudRate);
            _activePort.LogObjectValue += delegate (string title, object value) { LogObjectValue?.Invoke(title, value); };
            ActivePort.StartListenning(port, baudRate);
            LogMessageValue?.Invoke("Monitoring Started Successfully!");
        }

        void ActivePort_PortDataReceived(PortDataReceivedEventArgs args)
        {
            lock (this)
            {
                LogObjectValue?.Invoke("PortDataReceivedEventArgs", args);
                SetLinesState(args.Data as string);
            }
        }

        public void StopMonitoring()
        {
            if (ActivePort == null) return;
            _activePort.PortDataReceived -= ActivePort_PortDataReceived;
            ActivePort.StopListenning();
        }

        private IPort ActivePort
        {
            get
            {
                SetHookOffTimerInterval(10000);
                if (_activePort != null) return _activePort;
                _activePort = new YasnaCustomSerialPort();
                return _activePort;
            }
        }

        public abstract string DeviceProcessorKey { get; }

        public ICollection<ILine> Lines => GetLinesMap().Values;

        public abstract int LinesCount { get; }
        public event LogInternalException LogInternalException;
        public event LogIntValue LogIntValue;
        public event LogStringValue LogStringValue;
        public event LogObjectValue LogObjectValue;
        public event LogMessageValue LogMessageValue;
    }
}

