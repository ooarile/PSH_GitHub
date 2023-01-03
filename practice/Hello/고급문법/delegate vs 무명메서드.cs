using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello.고급문법
{
    internal class delegate_vs_무명메서드
    {

        /*
         * http://www.csharpstudy.com/CSharp/CSharp-anonymous-method.aspx
         * 
        // Delegate 타입 : 
        public delegate int SumDelegate(int a, int b);

        // Delegate 사용 : 
        SumDelegate sumDel = new SumDelegate(mySum);

        // 무명메서드1 
        button1.Click += new EventHandler(delegate (object s, EventArgs a) { MessageBox.Show("OK"); });

        // 무명메서드2
        button1.Click += (EventHandler) delegate (object s, EventArgs a) { MessageBox.Show("OK"); };

        // 무명메서드3 
        button1.Click += delegate (object s, EventArgs a) { MessageBox.Show("OK"); };

        // 무명메서드4 
        button1.Click += delegate { MessageBox.Show("OK"); };
        */



        /*
         * // 틀림: 컴파일 에러 발생
         * this.Invoke(delegate {button1.Text = s;} );
         * 
         * // 맞는 표현 
         * MethodInvoker mi = new MethodInvoker(delegate() { button1.Text = s; });
         * this.Invoke(mi);
         * 
         * // 축약된 표현
         * this.Invoke((MethodInvoker) delegate { button1.Text = s; });
         * 
         * //MethodInvoker는 입력 파라미터가 없고, 리턴 타입이 void인 델리게이트이다.
         * //MethodInvoker는 System.Windows.Forms 에 다음과 같이 정의되어 있다.
         * //public delegate void MethodInvoker();
*/
    }
}
