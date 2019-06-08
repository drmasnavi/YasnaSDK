namespace CallerIdALM.DeviceProcessors
{
    public class CaspianTwoLineDeviceProcessor : YasnaBasedDeviceProcessor
    {

        public override string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "کالرآیدی دو خط كاسپين";
            }
            return "Caspian Two Lines CallerID T-Line";
        }



        public override string DeviceProcessorKey => "CaspianTwoLineCallerId";


        public override int LinesCount => 2;
    }
}