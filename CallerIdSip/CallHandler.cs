using System;
using Ozeki.Media;
using Ozeki.Media.MediaHandlers;
using Ozeki.VoIP;

namespace CallerIdSip
{
    class CallHandler
    {
        ICall _call;
        MediaConnector _mediaConnector;
        MP3StreamPlayback _mp3Player;
        PhoneCallAudioSender _phoneCallAudioSender;

        public CallHandler(ICall call)
        {
            _call = call;

            _phoneCallAudioSender = new PhoneCallAudioSender();
            _mp3Player = new MP3StreamPlayback(@"..\..\test.mp3");
            _mp3Player.Stopped += mp3Player_Stopped;
            _phoneCallAudioSender.AttachToCall(call);
            _mediaConnector = new MediaConnector();
            _mediaConnector.Connect(_mp3Player, _phoneCallAudioSender);
        }

        public event EventHandler Completed;

        public void Start()
        {
            _call.CallStateChanged += call_CallStateChanged;
            OnCompleted();
            //_call.Answer();
        }

        void mp3Player_Stopped(object sender, EventArgs e)
        {
            _call.HangUp();
            OnCompleted();
        }

        void OnCompleted()
        {
            _mediaConnector.Dispose();
            _mp3Player.Dispose();

            var handler = Completed;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        void call_CallStateChanged(object sender, CallStateChangedArgs e)
        {
            OnCompleted();

//            if (e.State == CallState.Answered)
//                _mp3Player.Start();
//            else if (e.State.IsCallEnded())
//                OnCompleted();
        }
    }
}
