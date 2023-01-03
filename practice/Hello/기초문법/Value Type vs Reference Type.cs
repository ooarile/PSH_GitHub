using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello.기초문법
{
    internal class Value_Type_vs_Reference_Type
    {
        #region 1
        // System.Int32 (Value Type)
        public struct Int32
        {
            //....
        }

        // System.String (Reference Type)
        public sealed class String
        {
            //....
        }
        #endregion

        #region 2
        // 구조체 정의
        struct MyPoint
        {
            public int X;
            public int Y;

            public MyPoint(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public override string ToString()
            {
                return string.Format("({0}, {1})", X, Y);
            }
        }

        public void Main()
        {
            // 구조체 사용
            MyPoint pt = new MyPoint(10, 12);
            Console.WriteLine(pt.ToString());
        }
        #endregion
    }
}
