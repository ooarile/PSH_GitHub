using System;

namespace Hello.고급문법
{
    internal class 람다식
    {
        /*
         * 람다식은 무명 메서드와 비슷하게 무명 함수(anonymous function)를 표현하는데 사용된다.
         * 람다식은 아래와 같이 입력 파라미터(0개 ~ N개)를 => 연산자 왼쪽에, 실행 문장들을 => 연산자 오른쪽에 둔다
         * 
         * 람다식 문법 : (입력 파라미터) => { 실행문장 블럭 };
         * 
         */

        #region 1
        // 1. 먼저 대리자 타입을 선언한다.
        delegate int Calculate(int a, int b);
        public void main()
        {
            // 2. 대리자 타입의 참조변수에 익명 메소드(람다식)를 참조시킨다.(대리자 인스텀스화)
            Calculate cal = (a, b) => a + b;

            // 3. 대리자를 호출한다.
            cal(10, 20);

            // 이렇게도 할수 있음
            Calculate cal2 = (int a, int b) => a + b;
        }
        #endregion

        #region 2
        /*
         * Func 대리자 : Func 대리자는 결과를 반환하는 메서드를 참조하기 위해 만들어 졌다.
         */
        delegate TResult Func<out TResult>();
        delegate TResult Func<in T, out TResult>(T arg);
        delegate TResult Func<in T1, in T2, out TResult>(T1 arg1, T1 arg2);

        public void main2()
        {
            Func<int> func1 = () => 123;
            Console.WriteLine("func1의 값 : {0}", func1());


            Func<int,int> func2 = (x) => x*2;
            Console.WriteLine("func2의 값 : {0}", func2(5));
        }
        #endregion

        #region 3
        /*
         * Action 대리자 : Action 대리자는 결과를 반환하지 않는 메서드를 참조하기 위해 만들어 졌다.
         */

        //public delegate void Action<>();
        //public delegate void Action<in T>(T arg);
        //public delegate void Action<in T1, in T2>(T1 arg, T1 arg2);

        public void main3()
        {
            Action act1 = () => Console.WriteLine("act1() 수행");
            act1();

            int result = 0;
            Action<int> act2 = (int num) =>
            {
                result = num * num;
                Console.WriteLine("result의 값은? {0}", result);
            };
            act2(5);
        }
        #endregion

    }
}
