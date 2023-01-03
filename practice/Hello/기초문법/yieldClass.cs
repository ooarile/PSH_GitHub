using System;
using System.Collections;
using System.Collections.Generic;

namespace Hello.기초문법
{
    internal class yieldClass
    {
        /*
         * http://www.csharpstudy.com/CSharp/CSharp-yield.aspx
         */

        #region 예제1
        IEnumerable<int> GetNumber()
        {
            yield return 10;
            yield return 20;
            yield return 30;
        }

        int[] getInt()
        {
            return new int[] { 1, 2, 3 };
        }

        public void Main()
        {
            foreach (int i in GetNumber())
            { Console.WriteLine(i); }

            //test : 비슷하게 동작할것 같아 만들어 봄
            foreach (int i in getInt())
            { Console.WriteLine(i); }
        }
        #endregion

        #region 예제2
        public class MyList
        {
            private int[] data = { 1, 2, 3, 4, 5 };
            public IEnumerator GetEnumerator()
            {
                int i = 0;
                while(i< data.Length) 
                { 
                    yield return data[i];
                    i++; 
                }
            }
        }

        public void Main2()
        {
            //(1) foreach 사용하여 Iteration
            var list = new MyList();
            foreach(var item in list)
            {
                Console.WriteLine(item);
            }
            //(2) 수동 Iteration
            IEnumerator it = list.GetEnumerator();
            it.MoveNext();
            Console.WriteLine(it.Current);  // 1
            it.MoveNext();
            Console.WriteLine(it.Current);  // 2
        }
        #endregion

        #region 예제3
        public System.Collections.Generic.IEnumerable<int> Power(int num, int exponent)
        {
            int result = 1;
            for (int i = 0; i < exponent; i++)
            {
                result *= num;
                yield return result;
            }
        }

        public void Main3()
        {
            // Display powers of 2 up to the exponent of 8:
            foreach (int i in Power(2, 8))
            {
                Console.Write("{0} ", i);
            }

            // Output: 2 4 8 16 32 64 128 256
        }


        #endregion
    }
}
