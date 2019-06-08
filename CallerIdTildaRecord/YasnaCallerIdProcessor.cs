using System.Collections;
using System.Collections.Generic;
using CallerIdTildaRecord.DeviceProcessors;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdTildaRecord
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
                return "کالرآیدی تيلداكيش با قابلیت ضبط";
            }
            return "Tilda Recorder CallerID";
        }

        public ICollection<ICallerIdDeviceProcessor> GetProcessors()
        {
            if (_processors != null) return _processors;
            var list = new ArrayList
            {
                new TildaTwoLineDeviceProcessor(),
                new TildaFourLineDeviceProcessor(),
                new TildaSixLineDeviceProcessor(),
            };
            list.Sort();
            _processors = new List<ICallerIdDeviceProcessor>();
            foreach (ICallerIdDeviceProcessor processor in list)
            {
                _processors.Add(processor);
            }
            return _processors;
        }

        public string ProcessorKey => "TildaRecorderCallerIdProcessor";

        public string DeveloperEmail => "sup@YasnaSystem.com";
    }
}

