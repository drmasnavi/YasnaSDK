using System;
using System.Timers;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdALM.DeviceProcessors
{
    public class YasnaCallerIdLine : ILine
    {
        private Timer _hangerTimer;
        private string _number = string.Empty;
        private int _ringCounts;
        private LineStatus _status;
        private int _durationSecs;
        private string _dialedNumber = string.Empty;

        public event NumberChange NumberChanged;

        public event RingCountsChange RingCountsChanged;

        public event StatusChange StatusChanged;
        public event DurationChange DurationChanged;
        public event DialedNumberChange DialedNumberChanged;

        public YasnaCallerIdLine(int lineNumber)
        {
            LineNumber = lineNumber;
            _status = LineStatus.HookOff;
        }

        private void HangerTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Status = LineStatus.HookOff;
        }

        internal void SetTimerInterval(double interval)
        {
            if (interval > 0.0)
            {
                _hangerTimer = new Timer(interval) { Enabled = false };
                _hangerTimer.Elapsed += HangerTimerElapsed;
                _hangerTimer.Interval = interval;
            }
            else if (_hangerTimer != null)
            {
                _hangerTimer.Close();
                _hangerTimer = null;
            }
        }

        public DateTime LastUpdate { get; set; } = DateTime.MinValue;

        public int LineNumber { get; }

        public string Number
        {
            get
            {
                return _number;
            }
            set
            {
                //if (_number != value)
                {
                    var number = _number;
                    _number = value;
                    NumberChanged?.Invoke(this, number);
                }
            }
        }

        public string DialedNumber
        {
            get { return _dialedNumber; }
            set
            {
                if (_dialedNumber == value) return;
                var preDialed = _dialedNumber;
                _dialedNumber = value;
                DialedNumberChanged?.Invoke(this, preDialed);
            }
        }

        public int DurationSecs
        {
            get { return _durationSecs; }
            set
            {
                if (_durationSecs == value) return;
                if (_hangerTimer != null)
                {
                    _hangerTimer.Stop();
                    _hangerTimer.Start();
                }
                var preDure = _durationSecs;
                _durationSecs = value;
                DurationChanged?.Invoke(this, preDure);
            }
        }

        public int RingCounts
        {
            get
            {
                return _ringCounts;
            }
            set
            {
                if (_ringCounts == value) return;
                if (_hangerTimer != null)
                {
                    _hangerTimer.Stop();
                    _hangerTimer.Start();
                }
                var ringCounts = _ringCounts;
                _ringCounts = value;
                if (_ringCounts > 0)
                {
                    Status = LineStatus.Ringing;
                }
                RingCountsChanged?.Invoke(this, ringCounts);
            }
        }

        public LineStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    var previousState = _status;
                    _status = value;
                    if (_hangerTimer != null)
                    {
                        _hangerTimer.Enabled = _status == LineStatus.Ringing;
                    }
                    StatusChanged?.Invoke(this, previousState);
                }
            }
        }
    }
}

