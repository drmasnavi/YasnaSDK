namespace CallerIdSip.DeviceProcessors
{
    public class YasnaTwoLineDeviceProcessor : YasnaBasedDeviceProcessor
    {

        public override string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "پشتيباني از دو تماس هم زمان";
            }
            return "Two Lines CallerID";
        }



        public override string DeviceProcessorKey => "TwoLineCallerId";


        public override int LinesCount => 2;
    }
}