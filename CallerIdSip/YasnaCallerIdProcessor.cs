using System.Collections;
using System.Collections.Generic;
using CallerIdSip.DeviceProcessors;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdSip
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
            if (lcid == 1065)
            {
                return "مركز تماس يسنا";
            }
            return "CallCenter CallerID";
        }

        public ICollection<ICallerIdDeviceProcessor> GetProcessors()
        {
            if (_processors != null) return _processors;
            var list = new ArrayList
            {
                new YasnaTwoLineDeviceProcessor(),
            };
            list.Sort();
            _processors = new List<ICallerIdDeviceProcessor>();
            foreach (ICallerIdDeviceProcessor processor in list)
            {
                _processors.Add(processor);
            }
            return _processors;
        }

        public string ProcessorKey => "TildaHIDCallerIdProcessor";

        public string DeveloperEmail => "sup@YasnaSystem.com";
    }
}

