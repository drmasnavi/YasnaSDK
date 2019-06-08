using System.Collections.Generic;
using System.Timers;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdYasnaSystem.DeviceProcessors
{
    public class YasnaSystemTwoLineDeviceProcessor : YasnaBasedDeviceProcessor
    {

        public override string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "کالرآیدی دو خط یسنا سیستم T-Line";
            }
            return "Yasna System Two Lines CallerID T-Line";
        }



        public override string DeviceProcessorKey
        {
            get { return "YasnaTwoLineCallerId"; }
        }


        public override int LinesCount
        {
            get { return 2; }
        }
    }
}