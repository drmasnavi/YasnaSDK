namespace CallerIdTildaRecord.DeviceProcessors
{
    public class TildaSixLineDeviceProcessor : YasnaBasedDeviceProcessor
    {
        public override string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "کالرآیدی شش خط تيلداكيش";
            }
            return "Tilda Six Lines CallerID T-Line";
        }


        public override string DeviceProcessorKey => "TildaSixLineCallerId";


        public override int LinesCount => 6;
    }
}