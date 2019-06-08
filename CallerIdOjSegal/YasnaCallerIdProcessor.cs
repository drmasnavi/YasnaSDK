using System.Collections;
using System.Collections.Generic;
using CallerIdOjSegal.DeviceProcessors;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdOjSegal
{
    public class YasnaCallerIdProcessor : ICallerIdProcessor
    {
        private ICollection<ICallerIdDeviceProcessor> _processors;
        /// <summary>
        /// نام محصول را جهت نمایش در مایکروسافت بر می گرداند
        /// </summary>
        /// <param name="lcid">کد زبان بعنوان نمونه 1065 برای فارسی</param>
        /// <returns>نام محصول</returns>
        public string GetName(int lcid)
        {
            return lcid == 1065 ? "کالرآیدی پردازشگران اوج سگال" : "Oj Segal CallerID";
        }

        public ICollection<ICallerIdDeviceProcessor> GetProcessors()
        {
            if (_processors != null) return _processors;
            var list = new ArrayList
            {
                new OjSegalTwoLineDeviceProcessor(),
                new OjSegalFourLineDeviceProcessor(),
                new OjSegalNewTwoLineDeviceProcessor(),
                new OjSegalNewFourLineDeviceProcessor(),


            };
            list.Sort();
            _processors = new List<ICallerIdDeviceProcessor>();
            foreach (ICallerIdDeviceProcessor processor in list)
            {
                _processors.Add(processor);
            }
            return _processors;
        }

        public string ProcessorKey => "OjSegalCallerIdProcessor";

        public string DeveloperEmail => "sup@YasnaSystem.com";
    }
}

