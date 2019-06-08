namespace CallerIdOjSegal.DeviceProcessors
{
    public class OjSegalNewFourLineDeviceProcessor : OjSegalBaseDeviceProcessor
    {

        public override string GetName(int lcid)
        {
            return lcid == 1065 ? "کالرآیدی چهار خط جدید پردازشگران اوج سگال" : "Oj Segal Four Lines CallerID";
        }


        public override string DeviceProcessorKey => "OjSegalNewFourLineCallerId";


        public override int LinesCount => 4;
    }
}