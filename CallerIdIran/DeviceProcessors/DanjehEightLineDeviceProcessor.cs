namespace CallerIdIran.DeviceProcessors
{
    public class DanjehEightLineDeviceProcessor : YasnaBasedDeviceProcessor
    {
        public override string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "کالرآیدی هشت خط دانژه";
            }
            return "Danjeh Eight Lines CallerID T-Line";
        }


        public override string DeviceProcessorKey => "DanjehEightLineCallerId";


        public override int LinesCount => 8;
    }
}