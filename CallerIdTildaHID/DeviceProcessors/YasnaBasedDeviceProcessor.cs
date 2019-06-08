using System;
using System.Collections.Generic;
using TildaCIDHID.HID;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdTildaHID.DeviceProcessors
{
    public abstract class YasnaBasedDeviceProcessor : ICallerIdDeviceProcessor, IComparable
    {
        private HIDCallerID _activePort;

        private Dictionary<string, ILine> _linesMap;
        public void OnDeviceChange(int mMsg, IntPtr mWParam, IntPtr mLParam, ref bool handled)
        {
            _activePort?.OnDeviceChange(mMsg, mWParam, mLParam, ref handled);
        }

        public bool IsHid => true;

        public int CompareTo(object obj)
        {
            var processor = obj as ICallerIdDeviceProcessor;
            return processor != null ? LinesCount.CompareTo(processor.LinesCount) : 1;
        }

        private LineStatus FindCallStatus(TildaCIDHID.Public.CalllogDetail strCallLog)
        {
            if (strCallLog.sUpdateFileds.IndexOf("Rings", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return LineStatus.Ringing;
            }
            if (strCallLog.sUpdateFileds.IndexOf("Hookon", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return LineStatus.HookOn;//تلفن قطع : تیلدا کیش
            }
            if (strCallLog.sUpdateFileds.IndexOf("Hookoff", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return LineStatus.HookOff;//تلفن وصل : تیلداکیش
            }
            if (strCallLog.sUpdateFileds.IndexOf("DialNumber", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return LineStatus.Ringing;//تلفن وصل : تیلداکیش
            }
            return 0;
        }


        

        private string FindNumber(TildaCIDHID.Public.CalllogDetail strCallLog)
        {
          
            return FixPhoneNumber(strCallLog.Number);
        }

        private int FindRingCounts(TildaCIDHID.Public.CalllogDetail strCallLog)
        {
            try
            {
                return Convert.ToInt32(strCallLog.Rings);

            }
            catch (Exception)
            {
                return 1;
            }
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

        private void SetLinesState(TildaCIDHID.Public.CalllogDetail receivedData)
        {
            try
            {
                if (receivedData == null) return;
                var lineNo = receivedData.LineNumber;
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

        private void SetDurationState(TildaCIDHID.Public.CalllogDetail receivedData, ILine line)
        {
            var durationSecs = receivedData.Duration;
            LogIntValue?.Invoke("Duration", durationSecs);
            if (durationSecs >= 0)
            {
                line.DurationSecs = durationSecs;
            }
        }

        private void SetDialedNumberState(TildaCIDHID.Public.CalllogDetail receivedData, ILine line)
        {
            var dialedNumber = receivedData.DialNumber;
            LogStringValue?.Invoke("DialedNumber", dialedNumber);
            if (!string.IsNullOrEmpty(dialedNumber))
            {
                line.DialedNumber = dialedNumber;
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

        public void StartMonitoring(string port, string baudRate,IntPtr handle = default(IntPtr))
        {
            if (ActivePort == null) return;

            _activePort.HIDConnect += _activePort_HIDConnect;
            _activePort.HIDDisConnect += _activePort_HIDDisConnect;
            _activePort.HIDDataReceived += _activePort_HIDDataReceived;
            _activePort.Rawdata = false;
            _activePort.tryFindDevice(handle);
            LogMessageValue?.Invoke("Monitoring Started Successfully!");
        }

       
        public void StopMonitoring()
        {
            if (ActivePort == null) return;
            _activePort.HIDConnect -= _activePort_HIDConnect;
            _activePort.HIDDisConnect -= _activePort_HIDDisConnect;
            _activePort.HIDDataReceived -= _activePort_HIDDataReceived;
            _activePort.Dispose();
        }

        private HIDCallerID ActivePort
        {
            get
            {
                SetHookOffTimerInterval(10000);
                if (_activePort != null) return _activePort;
                _activePort = new HIDCallerID();
                return _activePort;
            }
        }

        private void _activePort_HIDDataReceived(TildaCIDHID.Public.CalllogDetail callogdetail)
        {
            lock (this)
            {
                LogObjectValue?.Invoke("PortDataReceivedEventArgs", callogdetail.sUpdateFileds);
                SetLinesState(callogdetail);
            }
        }

        private void _activePort_HIDDisConnect()
        {
            LogMessageValue?.Invoke("CallerId Disconnected!");
        }

        private void _activePort_HIDConnect()
        {
            LogMessageValue?.Invoke("CallerId Connected!");
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

