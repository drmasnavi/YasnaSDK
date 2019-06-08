using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdYasnaSystem.DeviceProcessors
{
    public class YasnaCallerIdSimulator : ICallerIdDeviceProcessor
    {
        #region فیلدها
        private Timer _timer;
        private int _cursor;
        private Dictionary<string, ILine> _linesMap;
        public event LogInternalException LogInternalException;
        public event LogIntValue LogIntValue;
        public event LogStringValue LogStringValue;
        public event LogObjectValue LogObjectValue;
        public event LogMessageValue LogMessageValue;
        #endregion
        public void OnDeviceChange(int mMsg, IntPtr mWParam, IntPtr mLParam, ref bool handled)
        {
        }

        public bool IsHid => false;
        public string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "سیمولاتور صرفا جهت دمو";
            }
            return "Yasna System CallerID - Simulator";
        }

        public void StartMonitoring(string port, string baudRate, IntPtr handle = default(IntPtr))
        {
            _timer = new Timer(30000.0) { Enabled = true };
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                lock (this)
                {
                    _cursor++;
                    if (_cursor > LinesCount)
                    {
                        _cursor = 1;
                    }
                    var cursor = _cursor;
                    var lineNo = cursor + 1;
                    if (lineNo > LinesCount)
                    {
                        lineNo = 1;
                    }
                    var line = GetLinesMap().ContainsKey(GetLineKey(cursor)) ? GetLinesMap()[GetLineKey(cursor)] : null;
                    var line2 = GetLinesMap().ContainsKey(GetLineKey(lineNo)) ? GetLinesMap()[GetLineKey(lineNo)] : null;
                    if ((line != null) && (line2 != null))
                    {
                        var status = line.Status;
                        var num = line.Number;
                        line.Status = line2.Status;
                        line.Number = line2.Number;
                        line2.Status = status;
                        line2.Number = num;
                    }
                }
            }
            catch (Exception ex)
            {
                if (LogInternalException != null)
                {
                    LogInternalException("TimerElapsed", ex);
                }
            }

        }

        public void StopMonitoring()
        {
            if (_timer != null)
            {
                _timer.Close();
            }
        }

        public string DeviceProcessorKey
        {
            get { return "YasnaSimulatorCallerId"; }
        }

        public ICollection<ILine> Lines
        {
            get
            {
                return GetLinesMap().Values;
            }
        }

        public int LinesCount
        {
            get { return 8; }
        }

        private Dictionary<string, ILine> GetLinesMap()
        {
            if (_linesMap != null) return _linesMap;
            _linesMap = new Dictionary<string, ILine>(LinesCount);
            for (var i = 0; i < LinesCount; i++)
            {
                ILine line = new YasnaCallerIdLine(i + 1)
                {
                    Status = (LineStatus)Math.Min(i + 1, 3),
                };
                //حالت های مختلف نمایش شماره توسط مخابرات
                var strings = new List<string>
                {
                    "9122800039","+989122800039","09122800039","+989121991334","00989122800039","09121991334","00989121991334","00982122514321","02122514321","2122514321","22514321","+982122514321","09104400039"
                };
                if (LogObjectValue != null) LogObjectValue("ListOfSampleNumbers", strings);
                //انتخاب یک شماره به صورت اختیاری
                line.Number = strings.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                _linesMap.Add(GetLineKey(line.LineNumber), line);
            }
            return _linesMap;
        }
        private string GetLineKey(int lineNo)
        {
            return $"L{lineNo}";
        }
    }
}