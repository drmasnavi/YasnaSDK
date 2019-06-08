using System;
using System.IO.Ports;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdOjSegal.DeviceProcessors
{
    internal class YasnaCustomSerialPort : SerialPort, IPort
    {
        public event DataReceiving PortDataReceived;
        public event LogObjectValue LogObjectValue;
        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                LogObjectValue?.Invoke("SerialDataReceivedEventArgs", e);
                if (PortDataReceived == null)
                {
                    return;
                }
                var args = new PortDataReceivedEventArgs(PortName, "Serial", ReadExisting());
                LogObjectValue?.Invoke("Received Data", args);
                PortDataReceived(args);

            }
            catch (Exception ex)
            {
                LogObjectValue?.Invoke("SerialPortDataReceived Ex", ex);
            }
        }


        public void StartListenning(string port, string baudRate)
        {
            LogObjectValue?.Invoke("StartListening!", "");
            try
            {
                if (IsOpen) return;
                DataReceived += SerialPortDataReceived;
                PortName = port;
                BaudRate = Convert.ToInt32(baudRate);
                Open();
            }
            catch (Exception ex)
            {
                LogObjectValue?.Invoke("StartListenning Ex", ex);
            }
        }

        public void StopListenning()
        {
            try
            {
                if (!IsOpen) return;
                DataReceived -= SerialPortDataReceived;
                Close();
            }
            catch (Exception ex)
            {
                LogObjectValue?.Invoke("StopListenning Ex", ex);
            }
        }
    }
}

