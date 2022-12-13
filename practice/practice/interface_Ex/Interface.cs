using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practice.interface_Ex
{
    //https://jeong-pro.tistory.com/52
    //인터페이스 사용 이유 : 클래스는 다중상속이 불가하다
    //C#에서 클래스는 하나 이상의 인터페이스를 상속합니다. 그러나 클래스는 하나의 추상클래스만 상속 할 수 있습니다.

    //https://holjjack.tistory.com/41
    //추상클래스와 인터페이스의 차이
    //C#에서 인터페이스는 생성자를 선언할 수 없습니다. 추상 클래스는 생성자를 선언할 수 있습니다.
    //C#에서 인터페이스는 클래스의 외부 능력을 정의하는 데 사용됩니다. 추상 클래스는 클래스의 실제 ID를 정의하는 데 사용되며 객체 또는 동일한 유형으로 사용됩니다.
    //C#에서 다양한 구현이 메소드 서명 만 공유하는 경우 인터페이스가 사용됩니다. 다양한 구현이 동일한 종류이고 동일한 동작 또는 상태를 사용하는 경우 추상 클래스가 사용됩니다.
    //C#에서 새 메소드가 인터페이스에 추가 된 경우 모든 인터페이스가 구현 된 위치를 추적하고 해당 메소드의 구현도 추가해야합니다.  추상 클래스에서 새 메소드가 추가 된 경우 기본 구현을 추가 할 수있는 옵션이 있으므로 모든 기존 코드가 올바르게 작동합니다.

    // https://frozenpond.tistory.com/27
    // 인터페이스 사용 이유


    #region 인터페이스 사용 할 경우
    //interface IAnimal
    //{
    //    void Eat();
    //}


    //internal class Person : IAnimal
    //{
    //    public void Eat()
    //    {
    //        Console.WriteLine("밥을 먹습니다.");
    //    }
    //}

    //class Lion : IAnimal
    //{
    //    public void Eat()
    //    {
    //        Console.WriteLine("고기를 먹습니다.");
    //    }
    //}

    //class Camel
    //{
    //    public void Eat()
    //    {
    //        Console.WriteLine("풀을 먹습니다.");
    //    }
    //}
    #endregion


    #region 인터페이스 사용 안할 경우
    internal class Person 
    {
        public void Eat()
        {
            Console.WriteLine("밥을 먹습니다.");
        }
    }

    class Lion 
    {
        public void Eat()
        {
            Console.WriteLine("고기를 먹습니다.");
        }
    }

    class Camel
    {
        public void Eat()
        {
            Console.WriteLine("풀을 먹습니다.");
        }
    }
    #endregion
}
