using System.Collections.Generic;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdYasnaSystem.DeviceProcessors
{
    public class YasnaSystemEightLineDeviceProcessor : YasnaBasedDeviceProcessor
    {
        public override string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "کالرآیدی هشت خط یسنا سیستم T-Line";
            }
            return "Yasna System Eight Lines CallerID T-Line";
        }


        public override string DeviceProcessorKey
        {
            get { return "YasnaEightLineCallerId"; }
        }



        public override int LinesCount
        {
            get { return 8; }
        }
    }
}