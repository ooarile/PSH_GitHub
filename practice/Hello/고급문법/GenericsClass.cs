using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello.고급문법
{
    // http://www.csharpstudy.com/CSharp/CSharp-generics.aspx
    #region 1
    //internal class GenericsClass<T>
    //{
    //    // 어떤 요소 타입도 받아들 일 수 있는
    //    // 스택 클래스를 C# 제네릭을 이용하여 정의

    //    T[] _elements;
    //    int pos = 0;

    //    public GenericsClass()
    //    {
    //        _elements = new T[100];
    //    }

    //    public void Push(T element)
    //    {
    //        _elements[++pos] = element;
    //    }

    //    public T Pop()
    //    {
    //        return _elements[pos--];
    //    }
    //}
    #endregion
    /* 
     * // T는 Value 타입
     * class MyClass<T> where T : struct
     * // T는 Reference 타입
     * class MyClass<T> where T : class
     * // T는 디폴트 생성자를 가져야 함
     * class MyClass<T> where T : new() 
     * // T는 MyBase의 파생클래스이어야 함
     * class MyClass<T> where T : MyBase
     * // T는 IComparable 인터페이스를 가져야 함
     * class MyClass<T> where T : IComparable
     * 
     * // 좀 더 복잡한 제약들
     * class EmployeeList<T> where T : Employee,IEmployee, IComparable<T>, new()
     * {
     * }
     * // 복수 타입 파라미터 제약
     * class MyClass<T, U> where T : class where U : struct
     * {
     * }
     */

    #region 2_제네릭 타입 제약 (Type Constraint)
    internal class GenericsClass<T> where T : struct
    {
        // 어떤 요소 타입도 받아들 일 수 있는
        // 스택 클래스를 C# 제네릭을 이용하여 정의

        T[] _elements;
        int pos = 0;

        public GenericsClass()
        {
            _elements = new T[100];
        }

        public void Push(T element)
        {
            _elements[++pos] = element;
        }

        public T Pop()
        {
            return _elements[pos--];
        }
    }
    #endregion
}
