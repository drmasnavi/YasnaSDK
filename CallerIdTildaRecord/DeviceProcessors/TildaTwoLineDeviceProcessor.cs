namespace CallerIdTildaRecord.DeviceProcessors
{
    public class TildaTwoLineDeviceProcessor : YasnaBasedDeviceProcessor
    {

        public override string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "کالرآیدی دو خط تيلداكيش";
            }
            return "Tilda Two Lines CallerID T-Line";
        }



        public override string DeviceProcessorKey => "TildaTwoLineCallerId";


        public override int LinesCount => 2;
    }
}