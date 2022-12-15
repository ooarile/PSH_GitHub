using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practice._8_대리자와_이벤트_Delegator__Event
{
    internal class delegatorClass
    {
        public delegate void myDelegate(object sender, object data);
        public myDelegate DelSendEvent;
        private void main()
        {
            DelSendEvent(this, "Hi");
        }

    }
}
