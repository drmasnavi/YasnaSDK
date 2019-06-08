using System.Collections.Generic;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdYasnaSystem.DeviceProcessors
{
    public class YasnaSystemFourLineDeviceProcessor : YasnaBasedDeviceProcessor
    {
        public override string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "کالرآیدی چهار خط یسنا سیستم T-Line";
            }
            return "Yasna System Four Lines CallerID";
        }


        public override string DeviceProcessorKey
        {
            get { return "YasnaFourLineCallerId"; }
        }



        public override int LinesCount
        {
            get { return 4; }
        }
    }
}