using System;
using System.Collections.Generic;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdTildaRecord.DeviceProcessors
{
    public abstract class YasnaBasedDeviceProcessor : ICallerIdDeviceProcessor, IComparable
    {
        private TildaCIDHID.Record.TildaRecordAllLines _activePort;

        private Dictionary<string, ILine> _linesMap;
        public void OnDeviceChange(int mMsg, IntPtr mWParam, IntPtr mLParam, ref bool handled)
        {
        }

        public bool IsHid => false;

        public int CompareTo(object obj)
        {
            return obj is ICallerIdDeviceProcessor processor ? LinesCount.CompareTo(processor.LinesCount) : 1;
        }

        private LineStatus FindCallStatus(TildaCIDHID.Public.CalllogDetail strCallLog)
        {
            switch (strCallLog.Status)
            {
                case "Idle":
                    return LineStatus.HookOff;

                case "Recording":
                case "Busy":
                    return LineStatus.HookOff;
                    
                case "Ringing":
                    return LineStatus.Ringing;

                case "NoLine":
                    return LineStatus.HookOff;

                case "Missed Call":
                    return LineStatus.HookOff;
            }
            return LineStatus.HookOff;
            /* if (strCallLog.sUpdateFileds.IndexOf("Rings", StringComparison.OrdinalIgnoreCase) != -1)
             {
                 return LineStatus.Ringing;
             }
             if (strCallLog.sUpdateFileds.IndexOf("Hookon", StringComparison.OrdinalIgnoreCase) != -1)
             {
                 return LineStatus.HookOff;//تلفن قطع : تیلدا کیش
             }
             if (strCallLog.sUpdateFileds.IndexOf("Hookoff", StringComparison.OrdinalIgnoreCase) != -1)
             {
                 return LineStatus.HookOn;//تلفن وصل : تیلداکیش
             }
             if (strCallLog.sUpdateFileds.IndexOf("DialNumber", StringComparison.OrdinalIgnoreCase) != -1)
             {
                 return LineStatus.Ringing;//تلفن وصل : تیلداکیش
             }
             return 0;*/
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
                    if (receivedData.sUpdateFileds.IndexOf("StatusRecord", StringComparison.Ordinal) != -1)
                    {
                        if (!receivedData.Status_Record)
                        {
                            ActivePort.StartRecordLine(line.LineNumber);

                        }
                        else
                        {
                            ActivePort.StopRecordLine(line.LineNumber);

                        }
                    }
                    if (receivedData.sUpdateFileds.IndexOf("Duration", StringComparison.Ordinal) != -1)
                    {
                        if (receivedData.Duration != -1)
                        {
                            SetDurationState(receivedData, line);

                        }
                    }

                    if (receivedData.sUpdateFileds.IndexOf("DialNumber", StringComparison.Ordinal) != -1)
                    {
                        if (!string.IsNullOrEmpty(receivedData.DialNumber))
                        {
                            SetDialedNumberState(receivedData, line);

                        }
                    }
                    if (receivedData.sUpdateFileds.IndexOf("Number-", StringComparison.Ordinal) != -1)
                    {
                        if (!string.IsNullOrEmpty(receivedData.Number))
                        {
                            var str = FindNumber(receivedData);
                            if (str != string.Empty)
                            {
                                line.Number = str;
                                LogStringValue?.Invoke("PhoneNumber", str);
                            }
                        }
                    }
                    if (receivedData.sUpdateFileds.IndexOf("Rings", StringComparison.Ordinal) != -1)
                    {
                        if (!string.IsNullOrEmpty(receivedData.Rings))
                        {
                            var num2 = FindRingCounts(receivedData);
                            if (num2 > 0)
                            {
                                line.RingCounts = num2;
                                LogIntValue?.Invoke("RingCount", num2);
                            }
                        }
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
            if (str.StartsWith("009"))
                str = str.Substring(1);
            if (str.StartsWith("00"))
                str = str.Substring(2);
            if (str.StartsWith("0") && str.Length==9)
                str = str.Substring(1);
            if (str.StartsWith("0098") || str.StartsWith("+98"))
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
            _activePort.AllRecDataReceive += _activePort_AllRecDataReceive;
            _activePort.AllRecConnect += _activePort_AllRecConnect;
            _activePort.AllRecDisConnect += _activePort_AllRecDisConnect;
            _activePort.AllRecStandby += _activePort_AllRecStandby;
            _activePort.AllRecData += _activePort_AllRecData;
            //_activePort.PortConnect(port, 0);
            _activePort.AutoConnect();
            LogMessageValue?.Invoke("Monitoring Started Successfully!");
        }

        private void _activePort_AllRecDataReceive(TildaCIDHID.Public.CalllogDetail callogdetail, object mysender)
        {
            lock (this)
            {
                LogObjectValue?.Invoke("PortDataReceivedEventArgs", callogdetail.sUpdateFileds);
                SetLinesState(callogdetail);
            }
        }

        private void _activePort_AllRecData(string data, object mysender)
        {
            LogMessageValue?.Invoke("From CallerID:"+ data);

        }

        private void _activePort_AllRecStandby(int trynumber, object mysender)
        {
            LogMessageValue?.Invoke("CallerId Connecting!");
        }

        private void _activePort_AllRecDisConnect(object mysender)
        {
            LogMessageValue?.Invoke("CallerId Disconnected!");
        }

        private void _activePort_AllRecConnect(object mysender)
        {
            LogMessageValue?.Invoke("CallerId Connected!");

        }

        public void StopMonitoring()
        {
            if (ActivePort == null) return;
            _activePort.AllRecDataReceive -= _activePort_AllRecDataReceive;
            _activePort.AllRecConnect -= _activePort_AllRecConnect;
            _activePort.AllRecDisConnect -= _activePort_AllRecDisConnect;
            _activePort.AllRecStandby -= _activePort_AllRecStandby;
            _activePort.AllRecData -= _activePort_AllRecData;
            _activePort.Dispose();
        }

        private TildaCIDHID.Record.TildaRecordAllLines ActivePort
        {
            get
            {
                SetHookOffTimerInterval(10000);
                if (_activePort != null) return _activePort;
                _activePort = new TildaCIDHID.Record.TildaRecordAllLines();
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

