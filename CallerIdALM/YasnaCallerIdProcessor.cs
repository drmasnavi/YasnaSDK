using System.Collections;
using System.Collections.Generic;
using CallerIdALM.DeviceProcessors;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdALM
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
                return "کالرآیدی كاسپين";
            }
            return "ALM CallerID";
        }

        public ICollection<ICallerIdDeviceProcessor> GetProcessors()
        {
            if (_processors != null) return _processors;
            var list = new ArrayList
            {
                new CaspianTwoLineDeviceProcessor(),
                new CaspianFourLineDeviceProcessor(),
                new CaspianEightLineDeviceProcessor(),
            };
            list.Sort();
            _processors = new List<ICallerIdDeviceProcessor>();
            foreach (ICallerIdDeviceProcessor processor in list)
            {
                _processors.Add(processor);
            }
            return _processors;
        }

        public string ProcessorKey => "CaspianCallerIdProcessor";

        public string DeveloperEmail => "sup@YasnaSystem.com";
    }
}

