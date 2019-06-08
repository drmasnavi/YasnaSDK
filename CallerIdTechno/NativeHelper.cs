using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CallerIdTechno
{
    class NativeHelper
    {
        [DllImport("DllTechnoCaller.dll",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetInit();

        [DllImport("DllTechnoCaller.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void SetIpServer(string serverIp);

        [DllImport("DllTechnoCaller.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetListDevice();

        [DllImport("DllTechnoCaller.dll",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetNextCMD(byte[] PointerOfArray);

        [DllImport("DllTechnoCaller.dll",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetNextCMDString(byte[] PointerOfArray);

    }
}

/*
D1:DeviceConnect,Serial=0,CountLine=2.
D1:DeviceDisconnect.


گوشی بدون زنگ خوردن برداشته‌شده	D1:L1:HookOff.
*گوشی جواب دهی شده*	D1:L1:Answer.
گوشی گذاشته‌شده	D1:L1:HookOn,Time=0001.
تعداد زنگ ورودی خط	D1:L1:Ring=1.
کالرآیدی ورودی	D1:L1:CallerID=02166595961.
شماره گرفته‌شده با تلفن	D1:L1:Dial=9.
*خط تلفن از دستگاه جداشده است*	D1:L1:LineDisconnect.
*خط تلفن به دستگاه متصل شده است*	D1:L1:LineConnect.
*دستگاه از USB جداشده است *	D1:DeviceConnect.
*دستگاه به USB متصل شده است*	D1:DeviceDisconect.
*خط دارای خطا است*	D1:L1:ErrorCode=1.


D1:L1:HangUp.
D1:L1:Answer.
D1:L1:HangDown,Time=0001.
D1:L1:Ring=1.
D1:L1:CallerID=02166595961.
D1:L1:Dial=9.
D1:L1:LineDisconnect.
D1:L1:LineConnect.
D1:DeviceConnect.
D1:DeviceDisconect.
D1:L1:ErrorCode=1.
D1:L1:MissCall,Ring=10.

*/
