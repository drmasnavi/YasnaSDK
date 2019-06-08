namespace CallerIdOjSegal.DeviceProcessors
{
    public class OjSegalTwoLineDeviceProcessor : YasnaBasedDeviceProcessor
    {

        public override string GetName(int lcid)
        {
            return lcid == 1065 ? "کالرآیدی دو خط پردازشگران اوج سگال" : "Oj Segal Two Lines CallerID";
        }


        public override string DeviceProcessorKey => "OjSegalTwoLineCallerId";


        public override int LinesCount => 2;
    }
}