using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello.기초문법
{
    internal class classClass
    {
        #region 간단한 C# Class
        /*
         * 위의 예는 public class 이므로 모든 객체로부터 접근 가능하다. 만약 internal class 이면 해당 어셈블리 내에서만 접근 가능.
         * 클래스 생성자(Constructor)는 클래스로부터 객체가 만들어 질때 호출되는 것으로 주로 필드를 초기화 하는데 사용한다.
         * 클래스에 어떤 필드, 메서드, 속성을 만들 것인가는 주로 업무 분석을 통해 해당 클래스의 역할에 따라 결정된다.
         */

        // 필드
        private string name;
        private int age;

        // 이벤트 
        public event EventHandler NameChanged;

        // 생성자 (Constructor)
        public classClass()
        {
            name = string.Empty;
            age = -1;
        }

        // 속성
        public string Name
        {
            get { return this.name; }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    if (NameChanged != null)
                    {
                        NameChanged(this, EventArgs.Empty);
                    }
                }
            }
        }
        public int Age
        {
            get { return this.age; }
            set { this.age = value; }
        }

        // 메서드
        public string GetCustomerData()
        {
            string data = string.Format("Name: {0} (Age: {1})",
                        this.Name, this.Age);
            return data;
        }
        #endregion

        #region parial Class
        // 1. Partial Class - 3개로 분리한 경우
        partial class Class1
        {
            public void Run() { }
        }

        partial class Class1
        {
            public void Get() { }
        }

        partial class Class1
        {
            public void Put() { }
        }

        // 2. Partial Struct
        partial struct Struct1
        {
            public int ID;
        }

        partial struct Struct1
        {
            public string Name;

            public Struct1(int id, string name)
            {
                this.ID = id;
                this.Name = name;
            }
        }

        // 3. Partial Interface
        partial interface IDoable
        {
            string Name { get; set; }
        }

        partial interface IDoable
        {
            void Do();
        }

        // IDoable 인터페이스를 구현
        public class DoClass : IDoable
        {
            public string Name { get; set; }

            public void Do()
            {
            }
        }
        #endregion
    }
}
