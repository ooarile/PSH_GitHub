using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practice._6_일반화_메소드
{
    internal class Method
    {
        public void CopyArray<T>(T[] source, T[] target)
        {
            for (int i=0; i < source.Length; i++)
            {
                target[i] = source[i];
            }
        }
    }
    class MyList<T> where T : Method
    {
        // ....
        void CopyArray<T>(T[] source, T[] target) where T : struct
        {
            for (int i = 0; i < source.Length; i++)
            {
                target[i] = source[i];
            }
        }
    }

    /* 형식 매개 변수 제약
    * where T : struct --> T는 값 형식이어야 합니다.
    * where T : class  --> T는 참조 형식이어야 합니다.
    * where T : new()  --> T는 반드시 매개 변수가 없는 생성자가 있어야 합니다.
    * where T : 기반_클래스_이름  --> T는 명시한 기반 클래스의 파생클래스여야 합니다.
    * where T : 인터페이스_이름   --> T는 명시한 인터페이스를 반드시 구현해야 합니다. 인터페이스_이름에는 여러 개의 인터페이스를 명시할 수도 있습니다.
    * where T : U                 --> T는 또 다른 형식 매개변수 U로부터 상속받은 클래스 여야 합니다.
    */

    /* 값 형식(Value Types)과 참조 형식(Reference Types)의 주요 차이점
     * 값 유형은 스택 메모리에 저장되고 참조 유형은 힙 메모리에 저장됩니다.
     * 구조체, 문자열, 열거형을 제외한 모든 기본 데이터 유형은 값 유형의 Example입니다. Class, string, array, delegate, interface는 참조 유형의 Example입니다.
     * 값 유형이 다른 값 유형에 복사되면 실제 값은 복사되지만 참조 유형이 다른 참조 유형에 복사되면 값의 참조 주소는 복사입니다.
     * 값 유형은 0 으로 초기화 하고 참조 유형은 NULL 로 초기화 할 수 있습니다 
     */
    #region 형식 매개 변수 예문
    class StructArray<T> where T : struct
    {
        public T[] Array;//{ get; set; }
        public int Length;// { get; set; }

        public StructArray(int size)
        {
            Array = new T[size];
            Length = size;
        }
    }
    class RefArray<T> where T : class
    {
        public T[] Array { get; set; }
        public int Length { get; set; }

        public RefArray(int size)
        {
            Array = new T[size];
            Length = size;
        }
    }
    class Base { }
    class Derived : Base { }
    class BaseArray<U> where U : Base
    {
        public U[] Array { get; set; }
        public int Length { get; set; }

        public BaseArray(int size)
        {
            Array = new U[size];
            Length = size;
        }

        public void CopyArray<T>(T[] Source) where T : U
        {
            Source.CopyTo(Array, 0);
        }
    }
    public interface IInterface
    {
    }

    public interface IWhatever : IInterface
    {
        void ShowArrayLength();
    }

    public class InterfaceArray<T> where T : IInterface
    {
        public T[] Array { get; set; }
        public int Length { get; set; }
        public InterfaceArray(int size)
        {
            Array = new T[size];
            Length = Array.Length;
        }
    }

    class ProgramEx
    {
        public static T CreateInstance<T>() where T : new()
        {
            return new T();
        }

        public static void MainFunc()
        {
            // 값 형식으로 제약
            StructArray<int> a = new StructArray<int>(3);
            a.Array[0] = 1;
            a.Array[1] = 2;
            a.Array[2] = 3;

            for (int i = 0; i < a.Length; i++)
            {
                Console.WriteLine(a.Array[i]);
            }
            Console.WriteLine("/////////");

            //참조형식으로 제약
            RefArray<StructArray<double>> b = new RefArray<StructArray<double>>(3);
            b.Array[0] = new StructArray<double>(5);
            b.Array[1] = new StructArray<double>(10);
            b.Array[2] = new StructArray<double>(200);

            for (int i = 0; i < b.Length; i++)
            {
                Console.WriteLine(b.Array[i].Length);
            }
            Console.WriteLine("/////////");

            // 기반 클래스로 제약
            BaseArray<Base> c = new BaseArray<Base>(3);
            c.Array[0] = new Base();
            c.Array[1] = new Derived();
            c.Array[2] = CreateInstance<Base>();

            for (int i = 0; i < c.Length; i++)
            {
                Console.WriteLine(c.Array[i]);
            }
            Console.WriteLine("/////////");

            // 기반 클래스로 제약
            BaseArray<Derived> d = new BaseArray<Derived>(3);
            d.Array[0] = new Derived(); //Derived가 기반 클래스가 되기 때문에 Base 할당 불가
            d.Array[1] = CreateInstance<Derived>();
            d.Array[2] = CreateInstance<Derived>();

            for (int i = 0; i < d.Length; i++)
            {
                Console.WriteLine(d.Array[i]);
            }
            Console.WriteLine("/////////");

            // U로부터 상속받은
            BaseArray<Derived> e = new BaseArray<Derived>(3);
            e.CopyArray<Derived>(d.Array);

            Console.WriteLine(e.Array.Length);

            //인터페이스 제약
            InterfaceArray<IWhatever> f = new InterfaceArray<IWhatever>(3);
        }
    }
    #endregion

    /* 일반화 컬렉션
     * 컬렉션(ArrayList, Queue, Stack, Hashtable)은 object 형식 기반
     * 컬렉션의 요소에 접근할 때마다 발생하는 박싱/언박싱은 성은의 저하로 이어짐
     * 일반화 컬렉션은 형식 매개변수를 이용하여 버그와 성능저하를 줄임(컴파일 단계에서 형식매개변수가 특정 형식으로 치환되기 때문)
     * 일반화 컬렉션은 System.Collection.Genereic 네임스페이스에 위치
     * 우리가 공부할 컬렉션은
     * List<T>
     * Queue<T>
     * Stack<T>
     * Dictionary<TKey,TValue>
     */
}
