using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serial_protocol.Data
{
    internal struct DataStruct
    {
        public DateTime Time { get; set; }
        public string Content { get; set; }
        //public string Grade { get; set; }
        //public int Score { get; set; }
    }
}
