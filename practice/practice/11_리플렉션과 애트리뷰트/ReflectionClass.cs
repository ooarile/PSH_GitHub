using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace practice._11_리플렉션과_애트리뷰트
{
    
    internal class ReflectionClass
    {
        /*
         * 런타임에 객체의 형식(Type)정보를 들여다보는 기능
         * System.Object은 형식 정보를 반환하는 GetType() 메소드 보유
         * 즉, 모든 데이터 형식은 System.Object 형식을 상송하므로 GetType()메소드 또한 보유
         */
        public void main()
        {
            int a = 0;
            Type type = a.GetType();
            FieldInfo[] fields = type.GetFields();

            foreach (FieldInfo field in fields)
            {
                Console.WriteLine("Type:{0}, Name:{1}", field.FieldType.Name, field.Name);
            }
        }

        #region 리플렉션을 이용한 객체 생성 및 활용
        class Profile
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public void Print()
            {
                Console.WriteLine("{0}, {1}", Name, Phone);
            }
        }
        //public void Func()
        //{
        //    Type type = typeof(Profile);
        //    Profile profile = (Profile)Activator.CreateInstance(type);

        //    PropertyInfo name = type.PropertyInfo("Name");
        //    PropertyInfo phone = type.PropertyInfo("Phone");
        //    name.SetValue(profile, "박찬호", null);
        //    phone.SetValue(profile, "997-5511", null);

        //    Console.WriteLine("{0}, {1}", name.GetValue(profile, null), phone.GetValue(profile,null));
        //}
        #endregion
    }
    #region 애트리뷰트 사용하기
    class MyClass
    {
        [Obsolete("OldMethod는 폐기되었습니다. NewMethod()를 이용하세요.")]
        public void OldMethod()
        {
            Console.WriteLine("I'm old");
        }
        public void NewMethod()
        {
            Console.WriteLine("I'm new");
        }
    }
    #endregion
    #region 사용자 정의 애트리뷰트


    #endregion
}