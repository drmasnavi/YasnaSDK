namespace CallerIdTildaHID.DeviceProcessors
{
    public class TildaFourLineDeviceProcessor : YasnaBasedDeviceProcessor
    {
        public override string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "کالرآیدی چهار خط تيلداكيش";
            }
            return "Tilda Four Lines CallerID";
        }


        public override string DeviceProcessorKey => "TildaFourLineCallerId";


        public override int LinesCount => 4;
    }
}