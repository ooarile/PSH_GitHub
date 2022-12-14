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
}
