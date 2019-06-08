using System;
using System.IO.Ports;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdTechno.DeviceProcessors
{
    internal class YasnaCustomSerialPort : SerialPort, IPort
    {
        public event DataReceiving PortDataReceived;
        public event LogObjectValue LogObjectValue;

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (PortDataReceived == null) return;
                var args = new PortDataReceivedEventArgs(PortName, "Serial", ReadLine());
                PortDataReceived?.Invoke(args);
            }
            catch (Exception)
            {
              
            }
        }

      
        public void StartListenning(string port, string baudRate)
        {
            if (IsOpen) return;
            DataReceived += SerialPortDataReceived;
            PortName = port;
            BaudRate = Convert.ToInt32(baudRate);
            Open();
        }

        public void StopListenning()
        {
            if (!IsOpen) return;
            DataReceived -= SerialPortDataReceived;
            Close();
        }
    }
}

