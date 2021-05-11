# Coroutine

## 코루틴에 대해서

유니티에서 제공하는 코루틴은 하나의 프로세스를 여러 루틴들이 시간을 나눠서 사용하는 방식으로 스레드와는 다르다.
스레드는 동시에 여러 프로세스가 여러 작업을 병렬적으로 진행하는것. 코루틴은 직렬처리이며 병렬처리처럼 보이게끔 해주는 함수이다. 유니티는 병렬적으로 함수들을 동시에 여러가지 실행하지 못한다. 한번에 함수를 하나하나 실행시킨다.

> 코루틴 예시

```C#
void Start()
{
  StartCorutine(SomeCoroutine());
}

IEnumerator SomeCoroutine()
{
  Debug.Log("Start Coroutine");
  yield return new WaitForSeconds(1f);
  Debug.Log("Waited!");
  yield return null;
  Debug.Log("End Coroutine!");
}

```

위 코드의 동작 순서는

1. Start에서 SomeCoroutine 코루틴 시작. "Start Coroutine" 메세지 출력
2. yield return new WaitForSeconds(1f)에서 1초 대기. "Waited!" 메세지 출력
3. yield return null에서 1프레임 대기. "End Coroutine!" 메세지 출력
4. 코루틴 종료.

코루틴의 응용법은 많은데 주로 지연처리를 하거나 비동기적 로직을 처리 해야 할 때 이용한다.  
하지만 IEnumerator가 무엇이길래 이런 동작이 가능한것일까? 내부 구조를 보도록 하자.

## IEnumerator 와 IEnumerable 내부 구조

> [Csharp_Study/Array_Collection_Indexer](https://github.com/twozeronine/Csharp_Study/tree/main/Array_Collection_Indexer)참고

```C#
public interface IEnumerator
{
  object Current {get;}
  bool MoveNext();
  void Reset();
}

public interface IEnumerable
{
  IEnumerator GetEnumerator();
}

```

IEnumerator 와 IEnumerable의 본래 용도는 두 인터페이스를 상속 받아 열거형의 상태를 보관하는 클래스를 구현하여, 해당값을 반복하여 가져오거나 무한한 양의 데이터를 필요할때 가져와서 처리할 때 ex) 피보나치 수열, 소수(Prime Number) 사용한다.

> 아래는 실제로 IEnumerable, IEnumerator (IEnumerable\<T>,IEnumerator\<T>) 상속받은 Generic Collection인 List\<int>를 사용하여 foreach문의 동작을 관찰해본 결과이다.

```IL
IL_0009: callvirt instance valuetype [System.Private.CoreLib]System.Collections.Generic.List`1/Enumerator<!0> class [System.Private.CoreLib]System.Collections.Generic.List`1<int32>::GetEnumerator()
```

![foreach내부동작](https://user-images.githubusercontent.com/67315288/117762435-1f81b500-b264-11eb-99b8-906a3c35a40c.png)

1. GetEnumerator()로 IEnumerator를 구현한 객체 인스턴스를 받아온다.
2. get_Current()로 인스턴스의 Current 프로퍼티를 접근하여 현재 위치의 요소를 반환한다.
3. MoveNext()로 bool값을 확인한다. 이때 bool 값이 false이면 loop문을 종료한다.

C#의 모든 Collection은 IEnumerable를 (Generic형식의 IEnumeralbe\<T>도 있지만 다음에 다루겠다. ) 상속받아 구현하고 있기 때문에. 따라서 IEnumerable을 상속 받은 모든 클래스 객체들은 foreach문에서 돌릴 수 있다.

하지만 위 코루틴 예시 코드에서 IEnumerator를 상속받은 클래스를 구현하지 않았는데 어떻게 동작할까 ?

## yield란 무엇인가.

> [Csharp_Study/Array_Collection_Indexer](https://github.com/twozeronine/Csharp_Study/tree/main/Array_Collection_Indexer)참고

C#에서 Iterator 방식으로 yield를 사용하면, 명시적으로 별도의 Enumerator 클래스를 작성하지 않아도 컴파일러가 자동으로 해당 인터페이스를 구현한 클래스를 생성해준다.  
yield return 문은 현재 메소드(IEnumerator를 반환하는 GetEnumerator( ))의 실행은 일시 정지시켜놓고 호출자에게 결과를 반환한다.  
이때 메소드가 다시 호출되면, 일시 정지된 실행을 복구하여 yield return 또는 yield break 문을 만날 때까지 나머지 작업을 실행한다.

```C#
using System;
using System.Collections.Generic;

class Program
{
    static IEnumerable<int> GetNumber()
    {
        yield return 10;  // 첫번째 루프에서 리턴되는 값
        yield return 20;  // 두번째 루프에서 리턴되는 값
        yield return 30;  // 세번째 루프에서 리턴되는 값
    }

    static void Main(string[] args)
    {
        // (1) foreach 사용하여 Iteration
        foreach (var item in GetNumber())
        {
            Console.WriteLine(item); // 10, 20, 30
        }

        // (2) 수동 Iteration
        IEnumerator it = .GetNumber().GetEnumerator();
        it.MoveNext();
        Console.WriteLine(it.Current);  // 10
        it.MoveNext();
        Console.WriteLine(it.Current);  // 20
        it.MoveNext();
        Console.WriteLine(it.Current);  // 30
    }
}
```

> yield문의 주의사항
>
> 1. 메서드 밖에서 yield를 쓰면 에러.
> 2. 익명 메서드 혹은 람다에서 yield를 쓰면 에러.
> 3. yield return문은 try-catch문 안에서 쓸 수 없다.
> 4. yield break문은 try-catch문 안에서 쓸 수 있지만 finally 에선 쓸 수 없다.

유니티에서는 이러한 yield return문에서 해당 호출자의 루틴으로 가서 다시 동작하는 이와 같은 특성을 이용하여 코루틴을 구현한것이다.

이제 유니티의 코루틴의 내부구조를 직접 보자.
