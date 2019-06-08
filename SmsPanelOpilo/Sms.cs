using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpiloWebserviceLibrary
{
    public class Sms
    {
        public string id { get; set; }
        public string text { get; set; }
        public string to { get; set; }
        public string from { get; set; }
        public override string ToString()
        {
            return this.text;
        }
    }
}
