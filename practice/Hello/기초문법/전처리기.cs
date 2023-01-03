#define TEST_ENV
//#define PROD_ENV

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello.기초문법
{
    internal class 전처리기
    {
        //http://www.csharpstudy.com/CSharp/CSharp-preprocessor.aspx

        #region 1
        public void Main()
        {
            bool verbose = false;
            // ...

#if (TEST_ENV)
            Console.WriteLine("Test Environment: Verbose option is set.");
            verbose = true;
#else
            Console.WriteLine("Production");
#endif

            if (verbose)
            {
                //....
            }
        }
        #endregion

        /*
         * #undef 는 #define과 반대로 지정된 심벌을 해제 할 때 사용한다.
         * #elif 는 #if와 함께 사용하여 else if를 나타낸다.
         * #line 은 거의 사용되진 않지만, 라인번호를 임의로 변경하거나 파일명을 임의로 다르게 설정할 수 있게 해준다.
         * #error 는 전처리시 Preprocessing을 중단하고 에러 메시지를 출력하게 한다.
         * #warning 은 경고 메서지를 출력하지만 Preprocessing은 계속 진행한다.
         * warning과 error는 특정 컴포넌트가 어떤 환경에서 실행되지 않아야 할 때 이를 경고나 에러를 통해 알리고자 할 때 사용될 수 있다.
         */

#if DEBUG
#else
#endif
    }
}
