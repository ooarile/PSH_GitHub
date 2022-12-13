using practice.interface_Ex;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public Form1()
        {
            InitializeComponent();
            Person myPerson = new Person();
            Lion myLion = new Lion();
            Camel myCamel = new Camel();

            myPerson.Eat();
            myLion.Eat();
            myCamel.Eat();

            Console.WriteLine();

            MyEatFunc(myPerson);
            MyEatFunc(myLion);
            MyEatFunc(myCamel);
        }
        private static void MyEatFunc(object obj)
        {
            Person target1 = obj as Person;
            if (target1 != null)
            {
                target1.Eat();
            }

            Lion target2 = obj as Lion;
            if (target2 != null)
            {
                target2.Eat();
            }

            Camel target3 = obj as Camel;
            if (target3 != null)
            {
                target3.Eat();
            }
        }
        #endregion
    }
}
