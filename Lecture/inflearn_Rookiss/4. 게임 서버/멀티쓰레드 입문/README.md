# 멀티쓰레드 프로그래밍

## 멀티쓰레드 개론

이전의 기존 운영체제에서는 1개의 CPU코어로 하나의 프로세스을 실행시켰다.

그러다 점차 CPU코어의 성능 발전으로 여러 프로세스를 [context switching](https://en.wikipedia.org/wiki/Context_switch)하며, 동시에 여러 프로그램을 실행 시킬 수 있었다.

그리고 각 프로세스를 실행 비중을 운영체제의 커널에서 스케줄링을 통해 프로세스를 실행한다.

그러다가 물리적으로 CPU의 연산속도를 더이상 늘릴 수 없는 한계점에 도달하였고, CPU 코어를 여러개 만들기로함.

그리고 프로세스의 실행단위는 더 작은 실행단위인 쓰레드로 나눠서 쓸 수 있는데 이것을 멀티쓰레드라고 한다.

보통의 개발에서는 쓰레드를 코어 수 만큼 1 ~ 1.5 : 1로 비율 정도로 유지한다.

쓰레드는 Heap영역, 데이터 stack영역(static 변수)을 공유해서 사용하고 사용스택 영역은 각자 독립해서 사용한다.

여기서의 문제점은 같은 데이터인 Heap과 stack 영역을 공유해서 사용하기 때문에 설계할때 교착상태에 빠지지 않도록 잘 설계해야 한다.

## 컴파일러 최적화

멀티쓰레드를 사용하면 기존 싱글쓰레드와는 달리 많은 문제점들이 생긴다. 그 중 컴파일러 최적화 문제에 대해서 알아보자.

> 해당 테스트는 Visual Studio 2019 버전 .NET 5.0에서 테스트한 결과입니다.

```C#
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Program
    {
        // 전역 변수는 메인과 쓰레드에서 동시에 접근 가능.
        static bool _stop = false;

        static void ThreadMain()
        {
            Console.WriteLine("쓰레드 시작 !");

            while (_stop == false)
            {
                // 누군가 stop 신호를 해주기를 기다린다.
            }

            Console.WriteLine("쓰레드 종료 !");
        }

        static void Main(string[] args)
        {
            Task t = new Task(ThreadMain);
            t.Start();

            Thread.Sleep(1000); // 1초 대기

            _stop = true;

            Console.WriteLine("Stop 호출");
            Console.WriteLine("종료 대기중");
            t.Wait(); // 쓰레드가 끝남을 알림.
            Console.WriteLine("종료 성공");
        }
    }
}
```

![디버깅모드](https://user-images.githubusercontent.com/67315288/126036001-f2845ba0-a17e-4aff-90a1-6f4486040f9a.png)

위 코드는 Debug 모드에서 실행 시 아무런 문제를 일으키지 않는 코드이다. 하지만 Release모드로 실제 프로그램을 빌드하여 사용하게 되면 오류를 일으키게 되는데

![릴리즈 모드](https://user-images.githubusercontent.com/67315288/126036002-07398af2-d7cc-4778-9468-24ce9bc6490d.png)

이런식으로 무한정 대기를 하게된다.

![어셈블리어](https://user-images.githubusercontent.com/67315288/126036192-2058a1bc-3852-42ed-9c33-f0d08fee110f.png)

> ecx에 7FF9F3C22BF2H에 있는 값을 가져와서
> test ecx, ecx ecx값을 and연산자를 이용해 비교
> je 에서 ZF플래그를 동일 여부를 확인하고 두 수가 같으면 jump를 실행한다.
> 결국 계속 while문이 무한하게 반복된다.

```C#
// 어셈블리를 의사코드로 표현

if(_stop == false)
{
  while(true)
  {

  }
}
```

이런식으로 컴파일러가 마음대로 최적화를 시켜서 이런 오류가 발생하게 된다.  
왜냐하면 컴파일러는 멀티 쓰레드라는 개념을 이해하지 못하고 while(\_stop == false) 구문 안에서 \_stop의 값이 변경되지 않으니 위와 같이 최적화를 해버리는것이다.

이것이 멀티쓰레드를 사용 할때에 생길 수 있는 컴파일러 최적화 문제다.
