namespace CallerIdTechno.DeviceProcessors
{
    public class TechnoFourLineDeviceProcessor : YasnaBasedDeviceProcessor
    {
        public override string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "کالرآیدی چهار خط تکنو";
            }
            return "Techno Four Lines CallerID";
        }


        public override string DeviceProcessorKey => "TechnoFourLineCallerId";


        public override int LinesCount => 4;
    }
}