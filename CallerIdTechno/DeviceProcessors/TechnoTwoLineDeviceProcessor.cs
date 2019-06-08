namespace CallerIdTechno.DeviceProcessors
{
    public class TechnoTwoLineDeviceProcessor : YasnaBasedDeviceProcessor
    {

        public override string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "کالرآیدی دو خط تکنو";
            }
            return "Techno Two Lines CallerID";
        }



        public override string DeviceProcessorKey => "TechnoTwoLineCallerId";


        public override int LinesCount => 2;
    }
}