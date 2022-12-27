using EcmaScript.NET;
using IronPython.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practice._12_Dynamic
{
    class Duck
    {
        public void Walk() { Console.WriteLine("Duck.Walk"); }
        public void Swim() { Console.WriteLine("Duck.Swim"); }
        public void Quack() { Console.WriteLine("Duck.Quack"); }
    }
    class Mallard : Duck { }
    class Robot
    {
        public void Walk() { Console.WriteLine("Robot.Walk"); }
        public void Swim() { Console.WriteLine("Robot.Swim"); }
        public void Quack() { Console.WriteLine("Robot.Quack"); }
    }
    internal class DynamicClass
    {
        /*
         * 런타임에 형식 검사가 이루어지는 데이터 형식
         * dynamic을 제외한 C#의 모든 데이터 형식은 컴파일 단계에서 형식검사 수행
         */
        public void Func()
        {
            dynamic[] arr = new dynamic[] { new Duck(), new Mallard(), new Robot() };
            foreach(dynamic duck in arr)
            {
                Console.WriteLine(duck.GetType());
                duck.Walk();
                duck.Swim();
                duck.Quack();

                Console.WriteLine();
            }
        }
    }

    /*
     * COM(Component Object Model) 상호운용성
     * c#초기(~v3.0)에는 dynamic 형식 없이 COM객체 사용
     *      COM은 메소다가 결과를 반활할 때, 실제 형식이 아닌 object형식으로 반환하며, C#고드에서는 이 결과를 다시 실제 형식으로 변환해서 사용
     *      C#의 선택적 인수는 4.0에 도입되었기 때문에 그 전까지는 COM메소드의 선택적 인수 중 사용하지 않는 인수에 대해 Type.Missing을 입력
     * C# 4.0이후
     *      dynamic형식 도입을 통해 번거로운 형식 변환 문제 해결
     *      선택적 인수 도입을 통해 사용하지 않는 인수 생략 가능해짐
     */
    class ComClass
    {
        //public static void NewWay(string[,] data, string savePath)
        //{
        //    Excel.Application excelApp = new Excel.Application();
        //    excelApp.WorkBooks.Add();
        //    excelApp._Worksheet workSheet = excelApp.ActiveSheet;
        //    for(int i = 0; i < data.GetLength(0); i++)
        //    {
        //        workSheet.Cells[i + 1, 1] = data[i, 0];
        //        workSheet.Cells[i + 1, 2] = data[i, 1];
        //    }
        //    workSheet.SaveAs(savePath + "\\shpark-book-dynamic.xlsx");
        //    excelApp.Quit();
        //}
        //public void main()
        //{
        //    ScriptRuntime runtime = Python.CreateRuntime();
        //    dynamic result = runtime.ExecuteFile("namecard.py");
        //    result.name = "박상현";
        //    result.phone = "010-123-4556";
        //    result.printNameCard();

        //    Console.WriteLine("{0},{1}", result.name, result.phone);
        //}
    }
    /*
     * C#에서 파이썬 코드 사용하기
     *      DLR은 CLR위에서 동작하며, 파이썬이나 루비와 같은 동적 언어를 실행
     *      DLR은 C#같은 정적 언어 코드에서 파이썬같은 동적언어코드와의 상호동작 지원
     *      DLR과 CLR사이의 상호동작은 dynamic형식을 통해 이루어 짐
     */
}
