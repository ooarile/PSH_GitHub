using Hello.고급문법;
using Hello.기초문법;
using System;
using System.Collections.Generic;
using static System.Console;

namespace Hello
{
    internal class MainApp /*: ProtectedClass*/
    {
        #region 연습
        //static void Main(string[] args)
        //{

        //    int a;
        //    //if(args.Length==0)
        //    //{
        //    //    Console.WriteLine("사용법 : Helloexe <이름>");
        //    //    return;
        //    //}
        //    //WriteLine("Hello, {0}!", args[0]);//Hello, world를 프롬포트에 출력
        //    a = sum(1);
        //    a = sum(1, 2);
        //    PrintProfile(name: a.ToString(), phone: a.ToString());
        //    MyMethod(1);
        //    MyMethod(1, 2);
        //    MyMethod(b: 1);
        //    BaseMethod();
        //}
        //static int sum(params int[] args)
        //{
        //    int sum = 0;
        //    for (int i = 0; i < args.Length; i++)
        //    {
        //        sum += args[i];
        //    }
        //    return sum;
        //}

        ////명명된 인수
        //static void PrintProfile(string name, string phone)
        //{
        //    Console.WriteLine("Name : {0}, Phone : {1}", name, phone);
        //}

        ////선택적 인수
        //static void MyMethod(int a = 0, int b = 0)
        //{
        //    Console.WriteLine("{0}, {1}", a, b);
        //}
        #endregion

        #region yield
        //static void Main(string[] args)
        //{
        //    yieldClass yieldClass = new yieldClass();
        //    //yieldClass.Main();
        //    yieldClass.Main2();
        //}
        #endregion

        #region 클래스
        //static void Main(string[] args)
        //{
        //    classClass cl = new classClass();
        //    cl.Name = "Test";
        //}
        #endregion

        #region 인덱서
        //static void Main(string[] args)
        //{
        //    IndexerClass indexerClass = new IndexerClass();

        //    // 인덱서 set 사용
        //    indexerClass[1] = 1024;

        //    // 인덱서 get 사용
        //    int i = indexerClass[1];
        //}
        #endregion

        #region 제너릭
        //static void Main(string[] args)
        //{
        //    #region 1
        //    //// 두 개의 서로 다른 타입을 갖는 스택 객체를 생성
        //    //GenericsClass<int> numberStack = new GenericsClass<int>();
        //    //GenericsClass<string> nameStack = new GenericsClass<string>();
        //    #endregion

        //    #region 2
        //    GenericsClass<int> numberStack = new GenericsClass<int>();
        //    //GenericsClass<string> nameStack = new GenericsClass<string>();
        //    #endregion
        //}
        #endregion

        #region 람다식
        static void Main(string[] args)
        {
            람다식 ramda = new 람다식();
            ramda.main2();
        }
        #endregion

    }
}
