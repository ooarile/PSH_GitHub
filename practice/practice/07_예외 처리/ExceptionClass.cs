using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practice._7_예외_처리
{
    internal class ExceptionClass
    {
        void main()
        {
            try
            {
                throw new Exception("오류오류!");
            }catch(Exception ex)
            {

            }
        }
    }    
}
