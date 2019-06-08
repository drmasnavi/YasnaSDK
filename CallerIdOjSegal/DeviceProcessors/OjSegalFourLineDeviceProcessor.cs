namespace CallerIdOjSegal.DeviceProcessors
{
    public class OjSegalFourLineDeviceProcessor : YasnaBasedDeviceProcessor
    {
        public override string GetName(int lcid)
        {
            if (lcid == 1065)
            {
                return "کالرآیدی چهار خط پردازشگران اوج سگال";
            }
            return "Oj Segal Four Lines CallerID";
        }

        public override string DeviceProcessorKey => "OjSegalFourLineCallerId";

        public override int LinesCount => 4;
    }
}