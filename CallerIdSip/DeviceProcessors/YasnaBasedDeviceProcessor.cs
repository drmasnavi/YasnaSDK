using System;
using System.Collections.Generic;
using Ozeki.Common;
using Ozeki.Media;
using Ozeki.Network;
using Ozeki.VoIP;
using Ozeki.VoIP.SDK.Protection;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdSip.DeviceProcessors
{
    public abstract class YasnaBasedDeviceProcessor : ICallerIdDeviceProcessor, IComparable
    {
        private static List<CallHandler> _callHandlers;
        private Softphone _softphone;
        private static Dictionary<string, ILine> _linesMap;
        public void OnDeviceChange(int mMsg, IntPtr mWParam, IntPtr mLParam, ref bool handled)
        {
        }

        public bool IsHid => false;
        public int CompareTo(object obj)
        {
            var processor = obj as ICallerIdDeviceProcessor;
            return processor != null ? LinesCount.CompareTo(processor.LinesCount) : 1;
        }

        public abstract string GetName(int lcid);

        public void StopMonitoring()
        {
            if (_softphone == null) return;
            _softphone.IncomigCall += softphone_IncomigCall;

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

        static void softphone_IncomigCall(object sender, VoIPEventArgs<Ozeki.VoIP.IPhoneCall> e)
        {
            try
            {
                ILine line = null;
                line = GetLinesMap()[GetLineKey(1)];
                if (line == null) return;
                lock (line)
                {
                    line.Status = LineStatus.Ringing;
                    line.Number = FixPhoneNumber(e.Item.DialInfo.CallerID);
                    line.LastUpdate = DateTime.UtcNow;
                }
                var callHandler = new CallHandler(e.Item);
                callHandler.Completed += callHandler_Completed;

                lock (_callHandlers)
                    _callHandlers.Add(callHandler);

                callHandler.Start();
            }
            catch (Exception ex)
            {
               
            }
           
        }
        static void callHandler_Completed(object sender, EventArgs e)
        {
            lock (_callHandlers)
                _callHandlers.Remove((CallHandler)sender);
        }
        public void StartMonitoring(string sip, string extention, IntPtr handle = default(IntPtr))
        {
            var key = "UDoyMDMzLTEyLTI1LFVQOjIwMzMtMDEtMDEsTUNDOjUwMCxNUEw6NTAwLE1TTEM6NTAwLE1GQzo1MDAsRzcyOTp0cnVlLE1XUEM6NTAwLE1JUEM6NTAwfE" +
                                "xGSTZEMDd6R3NNZlZUU3pHM1Uwb2dXNXlLNkV3SncxcitpZU90Zm9xN2dFV2d5R0tSMlc3dXZFODA2Q0NvaGR1b3V4N1VjY2VaamZMdnRPemtuTk1nPT0=";
            // For user name you can put any user name it wll work.
            var userName = "I-Warez 2015";
            LicenseManager.Instance.SetLicense(userName, key);
            _softphone = new Softphone();
            _softphone.IncomigCall += softphone_IncomigCall;
            _softphone.Register(true, extention, extention, extention, extention, sip.Substring(0, sip.IndexOf(':')), Convert.ToInt32(sip.Substring(sip.IndexOf(':'))));
            SetHookOffTimerInterval(10000);

            LogMessageValue?.Invoke("Monitoring Started Successfully!");
        }
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
        public abstract string DeviceProcessorKey { get; }

        public ICollection<ILine> Lines => GetLinesMap().Values;
        private static Dictionary<string, ILine> GetLinesMap()
        {
            if (_linesMap == null)
            {
                _linesMap = new Dictionary<string, ILine>(2);
                for (var i = 0; i < 2; i++)
                {
                    ILine line = new YasnaCallerIdLine(i + 1);
                    _linesMap.Add(GetLineKey(line.LineNumber), line);
                }
            }
            return _linesMap;
        }
        private static string GetLineKey(int lineNo)
        {
            return $"L{lineNo}";
        }

        public abstract int LinesCount { get; }
      

        public event LogInternalException LogInternalException;
        public event LogIntValue LogIntValue;
        public event LogStringValue LogStringValue;
        public event LogObjectValue LogObjectValue;
        public event LogMessageValue LogMessageValue;
    }
}

