using practice.interface_Ex;
using practice.overiding_Ex;
using practice._4_property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using practice._5_배열과_컬렉션_그리고_인덱서;
using practice._6_일반화_메소드;

namespace practice
{
    public partial class Form1 : Form
    {
        #region 인터페이스 사용 할 경우
        //public Form1()
        //{
        //    InitializeComponent();
        //    Person myPerson = new Person();
        //    Lion myLion = new Lion();
        //    Camel myCamel = new Camel();

        //    myPerson.Eat();
        //    myLion.Eat();
        //    myCamel.Eat();

        //    Console.WriteLine();

        //    MyEatFunc(myPerson);
        //    MyEatFunc(myLion);
        //    MyEatFunc(myCamel);
        //}
        //private static void MyEatFunc(object obj)
        //{
        //    IAnimal target = obj as IAnimal;
        //    if (target != null)
        //    {
        //        target.Eat();
        //    }
        //}
        #endregion

        #region 인터페이스 사용 안할 경우
        //public Form1()
        //{
        //    InitializeComponent();
        //    Person myPerson = new Person();
        //    Lion myLion = new Lion();
        //    Camel myCamel = new Camel();

        //    myPerson.Eat();
        //    myLion.Eat();
        //    myCamel.Eat();

        //    Console.WriteLine();

        //    MyEatFunc(myPerson);
        //    MyEatFunc(myLion);
        //    MyEatFunc(myCamel);
        //}
        //private static void MyEatFunc(object obj)
        //{
        //    Person target1 = obj as Person;
        //    if (target1 != null)
        //    {
        //        target1.Eat();
        //    }

        //    Lion target2 = obj as Lion;
        //    if (target2 != null)
        //    {
        //        target2.Eat();
        //    }

        //    Camel target3 = obj as Camel;
        //    if (target3 != null)
        //    {
        //        target3.Eat();
        //    }
        //}
        #endregion

        #region 오버라이딩(virtualm overide)
        //public Form1()
        //{
        //    InitializeComponent();
        //    ArmorSuite ar = new ArmorSuite();
        //    ar.Initialize();
        //    IronMan im = new IronMan();
        //    im.Initialize();
        //}
        #endregion

        #region 프로퍼티 설명
        //public Form1()
        //{
        //    property py = new property();
        //}
        #endregion 

        #region 배열과 컬렉션 그리고 인덱서
        //public Form1()
        //{
        //    Class1 cl = new Class1();
        //    cl.Func();

        //    MyList list = new MyList();
        //    for (int i = 0; i < 5; i++)
        //    {
        //        list[i] = i;
        //    }
        //    // 버전이 안맞나? 이거 왜 안되지?
        //    //for (int i = 0; i < list.Length; i++)
        //    //{
        //    //    Console.WriteLine(list[i]);
        //    //}
        //}
        #endregion

        #region 일반화 메소드
        public Form1()
        {
            int[] source = { 1, 2, 3, 4, 5 };
            int[] target = new int[source.Length];
            Method method = new Method();
            method.CopyArray<int>(source, target);
            foreach(int a in target)
            {
                Console.WriteLine(a);
            }
        }
        #endregion
    }
}
