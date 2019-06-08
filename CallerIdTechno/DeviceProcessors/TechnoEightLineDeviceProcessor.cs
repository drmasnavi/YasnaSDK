namespace CallerIdTechno.DeviceProcessors
{
    public class TechnoEightLineDeviceProcessor : YasnaBasedDeviceProcessor
    {
        public override string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "کالرآیدی هشت خط تکنو";
            }
            return "Techno Eight Lines CallerID T-Line";
        }


        public override string DeviceProcessorKey => "TechnoEightLineCallerId";


        public override int LinesCount => 8;
    }
}