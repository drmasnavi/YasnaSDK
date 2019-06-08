namespace CallerIdIran.DeviceProcessors
{
    public class DanjehTwoLineDeviceProcessor : YasnaBasedDeviceProcessor
    {

        public override string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "کالرآیدی دو خط دانژه";
            }
            return "Danjeh Two Lines CallerID";
        }



        public override string DeviceProcessorKey => "DanjehTwoLineCallerId";


        public override int LinesCount => 2;
    }
}