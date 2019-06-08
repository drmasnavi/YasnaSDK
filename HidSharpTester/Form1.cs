using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HidSharp;

namespace HidSharpTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //USB\VID_16C0 & PID_05DA\6 & F5178C3 & 0 & 3
            HidDeviceLoader hidDeviceLoader = new HidDeviceLoader();
            int? nullable = null;
            HidDevice hidDevice = hidDeviceLoader.GetDevices(5824, 1498, nullable, null).FirstOrDefault(d => d.MaxInputReportLength == 63);
            if (hidDevice != null)
            {
                object[] maxInputReportLength = new object[] { hidDevice.MaxInputReportLength, hidDevice.MaxOutputReportLength, hidDevice.MaxFeatureReportLength, hidDevice.DevicePath };
                HidStream hidStream;
                if (hidDevice.TryOpen(out hidStream))
                {
                    byte[] numArray = new byte[hidDevice.MaxInputReportLength];
                    try
                    {
                        var num = hidStream.Read(numArray, 0, (int)numArray.Length);
                    }
                    catch (TimeoutException timeoutException)
                    {
                        Console.WriteLine("Read timed out.");
                    }
                }
            }
        }
    }
}
