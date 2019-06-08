using System;
using System.Collections.Generic;
using System.Threading;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdTechno.DeviceProcessors
{
    public abstract class YasnaBasedDeviceProcessor : ICallerIdDeviceProcessor, IComparable
    {

        private Dictionary<string, ILine> _linesMap;
        public void OnDeviceChange(int mMsg, IntPtr mWParam, IntPtr mLParam, ref bool handled)
        {
        }

        public bool IsHid => false;


        public int CompareTo(object obj)
        {
            return obj is ICallerIdDeviceProcessor processor ? LinesCount.CompareTo(processor.LinesCount) : 1;
        }

        private static LineStatus FindCallStatus(string strCallLog)
        {
            if (strCallLog.IndexOf("HangDown", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return LineStatus.HookOff;//تلفن قطع : تیلدا کیش
            }
            if (strCallLog.IndexOf("HangUp", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return LineStatus.HookOn;//تلفن وصل : تیلداکیش
            }
            if (strCallLog.IndexOf("Dial", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return LineStatus.HookOn;//تلفن قطع : تیلداکیش
            }
            if (strCallLog.IndexOf("LineDisconnect", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return LineStatus.HookOff;//تلفن قطع : تیلداکیش
            }
            
            if (strCallLog.IndexOf("LineConnect", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return LineStatus.HookOn;//تلفن قطع : تیلداکیش
            }
            if (strCallLog.IndexOf("Miss", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return LineStatus.HookOff;//تلفن قطع : تیلداکیش
            }
            
            if (strCallLog.IndexOf("Ring", StringComparison.OrdinalIgnoreCase) != -1 )
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
            if (!strCallLog.Contains("CallerID")) return string.Empty;
            var str = string.Empty;
            try
            {
                /*if (strCallLog.IndexOf(lineKey, StringComparison.OrdinalIgnoreCase) != -1 && strCallLog.IndexOf("Dial", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    //D1:L1:Dial=9.
                    var index = strCallLog.IndexOf(lineKey, StringComparison.OrdinalIgnoreCase);
                    var num2 = strCallLog.IndexOf(".", index, StringComparison.OrdinalIgnoreCase);
                    var startIndex = index + lineKey.Length + 7;
                    if (num2 == startIndex) return string.Empty;
                    str = strCallLog.Substring(startIndex, num2 - startIndex);
                    str = FixPhoneNumber(str);
                }*/
                if (strCallLog.IndexOf(lineKey, StringComparison.OrdinalIgnoreCase) != -1 && strCallLog.IndexOf("CallerID", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    //D1:L1:CallerID=02166595961.
                    var index = strCallLog.IndexOf(lineKey, StringComparison.OrdinalIgnoreCase);
                    var num2 = strCallLog.IndexOf(".", index, StringComparison.OrdinalIgnoreCase);
                    var startIndex = index + lineKey.Length + 11;
                    if (num2 == startIndex) return string.Empty;
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
            if (!strCallLog.Contains("Ring")) return result;
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

        public int FlagForEnd;

        public void StartMonitoring(string port, string baudRate, IntPtr handle = default(IntPtr))
        {
            if (NativeHelper.SetInit() == 1)
            {
                Thread tr = new Thread(RunFunc);
                tr.Start();
                NativeHelper.GetListDevice();
            }
            SetHookOffTimerInterval(10000);

            LogMessageValue?.Invoke("Monitoring Started Successfully!");
        }

        private void RunFunc()
        {
            byte[] dataArray = new byte[65];
            while (FlagForEnd == 0)
            {
                Thread.Sleep(10);
                /*NativeHelper.GetNextCMD(DataArray);
                if (DataArray[1] != 0)
                {
                    switch (DataArray[1])
                    {
                        case 1:
                            //Don`t use.
                            break;

                        case 2: //Caller ID
                            str = "";
                            for (int i = 0; i < DataArray[4]; i++)
                            {
                                str = str + (char)(DataArray[5 + i]);
                            }
                            LCallerID(DataArray[2], DataArray[3], str);
                            break;

                        case 3://Hang Down
                            if (DataArray[2] == 1)
                            {
                                LMissCall(DataArray[3], DataArray[4], DataArray[5]);
                            }
                            if (DataArray[2] == 2)
                            {
                                LineHangDown(DataArray[3], DataArray[4], DataArray[5] * 256 + DataArray[5]);
                            }
                            break;

                        case 4://Hang Up
                            LineHangUp(DataArray[2], DataArray[3], DataArray[4]);
                            break;

                        case 5://Dialed Number
                            LinePhoneDigit(DataArray[2], DataArray[3], (char)DataArray[4]);
                            break;

                        case 6://Line Ring
                            LineRing(DataArray[2], DataArray[3], DataArray[4]);
                            break;

                        case 7://Line DisConnect
                            LDisConnect(DataArray[2], DataArray[3]);
                            break;

                        case 8:
                            //Don`t use.
                            break;

                        case 9://Device Connect
                            DeviceConnect(DataArray[2], DataArray[3], DataArray[4]);
                            break;

                        case 10://Device Disconnect
                            DeviceDisConnect(DataArray[2], DataArray[3]);
                            break;

                        case 11:
                            //Don`t use.
                            break;

                        case 12:
                            LineError(DataArray[2], DataArray[3], DataArray[4]);
                            //Don`t use.
                            break;

                        case 64:
                            str = "";
                            for (int i = 0; i < DataArray[3]; i++)
                            {
                                str = str + (char)(DataArray[4 + i]);
                            }
                            LOfflineCallerId(DataArray[2], str, DataArray[4]);
                            break;

                        default:
                            break;
                    }
                    DataArray[1] = 0;
                }*/
                NativeHelper.GetNextCMDString(dataArray);
                if (dataArray[1] == 0) continue;
                var str = "";
                for (var i = 0; i < dataArray[1]; i++)
                {
                    str = str + (char)(dataArray[2 + i]);
                }
                OutStringData(str);
                dataArray[1] = 0;
            }
        }
     
        private void OutStringData(string args)
        {
            lock (this)
            {
                LogObjectValue?.Invoke("PortDataReceivedEventArgs", args);
                SetLinesState(args);
            }
        }
      
        public void StopMonitoring()
        {
            FlagForEnd = 1;
        }

        private void SetLinesState(string receivedData)
        {
            try
            {
                if (receivedData == null) return;
                LogStringValue?.Invoke("Data", receivedData);
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

