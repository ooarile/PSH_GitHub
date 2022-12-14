using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practice.abstract_Ex_추상클래스
{
    /*
     * 설명
     * 추상 클래스는 메소드의 구현을 가질 수 있음(클래스와 같음)
     * - 추상클래스에서 구현을 가지지 않는 메소드를 "추상메소드(Abstract Method)"라고 함
     * - 추상클래스의 파생클래스는 추상메소드를 구현해야 함
     * 추상 클래스는 객체를 생성할 수 없음(인터페이스와 같음)
     * 인터페이스를 제공하되 기본적인 구현을 함께 제공하고 싶을 경우 사용
    */


    /* Virtual(가상 키워드), Abstract(추상 키워드), Interface 차이점
     * https://hongjinhyeon.tistory.com/93
     * 결론
        -Vritual은 하나의 기능을 하는 완전한 클래스이며, 파생클래스에서 상속해서 추가적인 기능추가 및 virtual 한정자가 달린 것을 재정의해서
         사용가능합니다.
        -Abstract는 여러개의 파생 클래스에서 공유할 기본 클래스의 공통적인 정의만 하고 ,파생클래스에서 abstract 한정자가 달린 것을
        모두 재정의(필수)해야 합니다.
        -Interface에서도 abstract와 비슷하지만 멤버변수를 사용할 수 없습니다. 
        보통 abstract는 개념적으로 계층적인 구조에서 사용이 되며(동물이나 어떤 사물의 계층적인 구조가있을때) Interface는 서로다른 계층이나
        타입이라도 같은기능을 추가하고 싶을때 사용합니다.(사람이나 기계가 말을하게(speak)하는 인터페이스를 추가할때)
     */

    abstract class abstractClass
    {
        int num;
        public void Func()
        {
            Console.WriteLine("Func");
        }

        virtual public void VirtualFunc()
        {
            Console.WriteLine("Virtual Func");
        }

        public abstract void SomeMethod();
    }
    class Derived : abstractClass
    {
        public override void SomeMethod()
        {
            // 함수 정의
        }
    } 

}
