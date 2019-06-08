using System.Collections;
using System.Collections.Generic;
using CallerIdYasnaSystem.DeviceProcessors;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdYasnaSystem
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
                return "کالرآیدی یسنا سیستم";
            }
            return "Yasna System CallerID";
        }

        public ICollection<ICallerIdDeviceProcessor> GetProcessors()
        {
            if (_processors == null)
            {
                var list = new ArrayList
                {
                    new YasnaSystemTwoLineDeviceProcessor(),
                    new YasnaSystemFourLineDeviceProcessor(),
                    new YasnaSystemEightLineDeviceProcessor(),
                    new YasnaCallerIdSimulator(),
                };
                list.Sort();
                _processors = new List<ICallerIdDeviceProcessor>();
                foreach (ICallerIdDeviceProcessor processor in list)
                {
                    _processors.Add(processor);
                }
            }
            return _processors;
        }

        public string ProcessorKey
        {
            get
            {
                return "YasnaSystemCallerIdProcessor";
            }
        }

        public string DeveloperEmail
        {
            get { return "sup@YasnaSystem.com"; }
        }
    }
}

