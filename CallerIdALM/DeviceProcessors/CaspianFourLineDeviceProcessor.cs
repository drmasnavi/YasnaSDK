namespace CallerIdALM.DeviceProcessors
{
    public class CaspianFourLineDeviceProcessor : YasnaBasedDeviceProcessor
    {
        public override string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "کالرآیدی چهار خط كاسپين";
            }
            return "Caspian Four Lines CallerID";
        }


        public override string DeviceProcessorKey => "CaspianFourLineCallerId";


        public override int LinesCount => 4;
    }
}