namespace CallerIdALM.DeviceProcessors
{
    public class CaspianEightLineDeviceProcessor : YasnaBasedDeviceProcessor
    {
        public override string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "کالرآیدی هشت خط كاسپين";
            }
            return "Caspian Eight Lines CallerID T-Line";
        }


        public override string DeviceProcessorKey => "CaspianEightLineCallerId";


        public override int LinesCount => 8;
    }
}