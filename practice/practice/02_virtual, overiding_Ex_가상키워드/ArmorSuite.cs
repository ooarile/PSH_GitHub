using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practice.overiding_Ex
{
    internal class ArmorSuite
    {
        public virtual void Initialize()
        {
            Console.WriteLine("Armored");
        }
    }
    class IronMan:ArmorSuite
    {
        //ArmorSuite의 virtual Initialize 파생클래스에서 재정의

        public override void Initialize()
        {
            //base.Initialize();
            Console.WriteLine("Repulsor Ray Armed");
        }
    }
}
