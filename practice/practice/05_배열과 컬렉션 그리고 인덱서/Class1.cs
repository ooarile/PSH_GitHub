using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practice._5_배열과_컬렉션_그리고_인덱서
{
    internal class Class1
    {
        /* 배열과 컬렉션*/
        List<int> list = new List<int>();
        ArrayList Alist = new ArrayList();

        //키와 값으로 이루어진 데이터를 다룰 때 사용
        Hashtable ht = new Hashtable();
        public void Func()
        {
            ht.Add("key", "키");
            ht[1] = "책";
            ht["Cook"] = "요리사";
            ht.Remove(1);
        }
    }
    class MyList
    { 
        /* 인덱서 */
        private int[] array;
        public MyList()
        {
            array = new int[3];
        }
        public int this[int index]
        {
            get { return array[index]; }
            set
            {
                if (index >= array.Length)
                {
                    Array.Resize<int>(ref array, index + 1);
                    Console.WriteLine("Array Resized : {0}", array.Length);
                }
                array[index] = value;
            }
        }

    }
}
