using System;
using static System.Console;

namespace Hello
{
    internal class MainApp : ProtectedClass
    {
        static void Main(string[] args)
        {
            int a;
            //if(args.Length==0)
            //{
            //    Console.WriteLine("사용법 : Helloexe <이름>");
            //    return;
            //}
            //WriteLine("Hello, {0}!", args[0]);//Hello, world를 프롬포트에 출력
            a= sum(1);
            a= sum(1,2);
            PrintProfile(name: a.ToString(), phone: a.ToString());
            MyMethod(1);
            MyMethod(1, 2);
            MyMethod(b: 1);
            BaseMethod();
        }

        static int sum(params int[] args)
        {
            int sum = 0;
            for(int i=0; i<args.Length;i++)
            {
                sum += args[i];
            }
            return sum;
        }

        //명명된 인수
        static void PrintProfile(string name, string phone)
        {
            Console.WriteLine("Name : {0}, Phone : {1}", name, phone);
        }

        //선택적 인수
        static void MyMethod(int a = 0, int b = 0)
        {
            Console.WriteLine("{0}, {1}", a, b);
        }
    }
}
