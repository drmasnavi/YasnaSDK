namespace CallerIdIran.DeviceProcessors
{
    public class DanjehFourLineDeviceProcessor : YasnaBasedDeviceProcessor
    {
        public override string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "کالرآیدی چهار خط دانژه";
            }
            return "Danjeh Four Lines CallerID";
        }


        public override string DeviceProcessorKey => "DanjehFourLineCallerId";


        public override int LinesCount => 4;
    }
}