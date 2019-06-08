using System.Collections;
using System.Collections.Generic;
using CallerIdIran.DeviceProcessors;
using YasnaCallerID.CallerIDInterface;

namespace CallerIdIran
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
                return "کالرآیدی دانژه";
            }
            return "Danjeh CallerID";
        }

        public ICollection<ICallerIdDeviceProcessor> GetProcessors()
        {
            if (_processors == null)
            {
                var list = new ArrayList
                {
                    new DanjehTwoLineDeviceProcessor(),
                    new DanjehFourLineDeviceProcessor(),
                    new DanjehEightLineDeviceProcessor(),
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

        public string ProcessorKey => "DanjehCallerIdProcessor";

        public string DeveloperEmail => "sup@YasnaSystem.com";
    }
}

