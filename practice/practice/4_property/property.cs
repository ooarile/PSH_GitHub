using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practice._4_property
{
    /* Property 사용이유 
     * 우선 앞서 말했듯이 객체지향언어는 정보은닉을 지향하는 언어이다.
     * 정보은닉이란 객체 내부의 데이터를 외부에 보이지 않게 하지 않음으로써 해당 클래스의 내부 구현 로직을 모르고도 어떠한 메소드를 호출하여 사용할 수 있게 한다는 것이다. 
     * 예를 들어 보통 점수는 0~100점의 값을 갖는 int형 변수이다. 그렇기에 해당 score라는 변수의 값에 101을 넣으면 오류이다.
     * 이러한 상황에서 get set을 사용하여 우리는 예제1의 내용처럼 해당 변수에 set에 조건을 달아줘서 0~100점의 값만 가질수 있게끔 설정할 수 있다.
     * 이것이 바로 get set의 사용 이유이다.
     * 
     * 정리 : 프로퍼티는 특별한 속성(조건)을 안넣어도 된다면 일반 전역 변수로 선언하여 사용하는 것이 성능 상 더 유리하다
     * */
    internal class property
    {
        
    }
}
