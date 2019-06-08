using System;
using System.Collections.Generic;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdIran.DeviceProcessors
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
            return processor != null ? LinesCount.CompareTo(processor.LinesCount) : 1;
        }

        private static LineStatus FindCallStatus(string strCallLog)
        {
            if (strCallLog.IndexOf("hook-off", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return LineStatus.HookOff;//تلفن قطع : تیلدا کیش
            }
            if (strCallLog.IndexOf("hook-on", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return LineStatus.HookOn;//تلفن وصل : تیلداکیش
            }
            if (strCallLog.IndexOf("Dialed", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return LineStatus.HookOff;//تلفن قطع : تیلداکیش
            }
            if (strCallLog.IndexOf("@", StringComparison.OrdinalIgnoreCase) != -1 && strCallLog.IndexOf("hook", StringComparison.OrdinalIgnoreCase) == -1 && strCallLog.IndexOf("Dialed", StringComparison.OrdinalIgnoreCase) == -1)
            {
                return LineStatus.Ringing;
            }
            return 0;

        }



        private int FindLineNumber(string strCallLog)
        {
            try
            {
                for (var i = 1; i <= LinesCount; i++)
                {
                    if (strCallLog.IndexOf(GetLineKey(i), StringComparison.Ordinal) == -1) continue;
                    LogIntValue?.Invoke("LineNumber", i);
                    return i;
                }
            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("FindLineNumber", ex);
            }
            return 0;

        }

        private string FindNumber(string strCallLog,string lineKey)
        {
            if (strCallLog.Contains("Dial")) return string.Empty;
            if (strCallLog.Contains("hook")) return string.Empty;
            var str = string.Empty;
            try
            {
                if (strCallLog.IndexOf(lineKey, StringComparison.OrdinalIgnoreCase) != -1 && strCallLog.IndexOf("Dialed", StringComparison.OrdinalIgnoreCase)==-1 && strCallLog.IndexOf("hook-off", StringComparison.OrdinalIgnoreCase) == -1 && strCallLog.IndexOf("hook-on", StringComparison.OrdinalIgnoreCase) == -1)
                {
                    var index = strCallLog.IndexOf(lineKey, StringComparison.OrdinalIgnoreCase);
                    var num2 = strCallLog.IndexOf("@", index, StringComparison.OrdinalIgnoreCase);
                    var startIndex = index + lineKey.Length+1;
                    if(num2==startIndex)return string.Empty;
                    str = strCallLog.Substring(startIndex, num2 - startIndex);
                    str = FixPhoneNumber(str);
                }
            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("FindNumber", ex);
            }
            return str;
        }

        private int FindRingCounts(string strCallLog)
        {
            var result = 1;
            return result;
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
                    
                    var str = FindNumber(receivedData, GetLineKey(lineNo));
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
            if (str.StartsWith("911") || str.StartsWith("912") || str.StartsWith("913") || str.StartsWith("932") || str.StartsWith("935") || str.StartsWith("936") || str.StartsWith("919"))
            {
                str = $"0{str}";
            }
            if (str.StartsWith("914") || str.StartsWith("915") || str.StartsWith("916") || str.StartsWith("917") || str.StartsWith("918") || str.StartsWith("937") || str.StartsWith("938"))
            {
                str = $"0{str}";
            }
            if (str.StartsWith("939") || str.StartsWith("920") || str.StartsWith("921") || str.StartsWith("901") || str.StartsWith("902") || str.StartsWith("903") || str.StartsWith("904"))
            {
                str = $"0{str}";
            }
            return str;
        }

        public void StartMonitoring(string port, string baudRate, IntPtr handle = default(IntPtr))
        {
            if (ActivePort == null) return;

            _activePort.PortDataReceived += ActivePort_PortDataReceived;
            ActivePort.StartListenning(port, baudRate);
            LogMessageValue?.Invoke("Monitoring Started Successfully!");
        }

        private void ActivePort_PortDataReceived(PortDataReceivedEventArgs args)
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

