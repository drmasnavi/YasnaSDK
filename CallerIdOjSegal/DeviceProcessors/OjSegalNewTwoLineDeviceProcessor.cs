namespace CallerIdOjSegal.DeviceProcessors
{
    public class OjSegalNewTwoLineDeviceProcessor : OjSegalBaseDeviceProcessor
    {

        public override string GetName(int lcid)
        {
            return lcid == 1065 ? "کالرآیدی دو خط جدید پردازشگران اوج سگال" : "Oj Segal New Two Lines CallerID";
        }


        public override string DeviceProcessorKey => "OjSegalNewTwoLineCallerId";


        public override int LinesCount => 2;
    }
}