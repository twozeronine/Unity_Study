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

### 디버깅 모드에서 실행 결과

![디버깅모드](https://user-images.githubusercontent.com/67315288/126036001-f2845ba0-a17e-4aff-90a1-6f4486040f9a.png)

위 코드는 Debug 모드에서 실행 시 아무런 문제를 일으키지 않는 코드이다. 하지만 Release모드로 실제 프로그램을 빌드하여 사용하게 되면 오류를 일으키게 되는데

---

### 릴리즈 모드에서 실행 결과

![릴리즈 모드](https://user-images.githubusercontent.com/67315288/126036002-07398af2-d7cc-4778-9468-24ce9bc6490d.png)

이런식으로 무한정 대기를 하게된다.

![어셈블리어](https://user-images.githubusercontent.com/67315288/126036192-2058a1bc-3852-42ed-9c33-f0d08fee110f.png)

> ecx에 7FF9F3C22BF2H에 있는 값을 가져와서  
> test ecx, ecx ecx값을 and연산자를 이용해 비교  
> je 에서 ZF플래그를 동일 여부를 확인하고 두 수가 같으면 jump를 실행한다.  
> 결국 계속 while문이 무한하게 반복된다.

---

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

## 캐시 이론

CPU 코어의 내부에는 ALU라는 연산을 하는 장치와 캐시가 있는데, 캐시는 요청이 빈번하게 발생하는 데이터들을 미리 저장시켜 응답하는 방식이다.

연산시 매번 RAM으로 가서 데이터를 가져오는것은 물리적으로 멀리 떨어져있기 때문에 자주 쓰일 것 같은 데이터는 캐시에 미리 올린다.

이 기준은 크게 시간과 공간의 상대성으로 2가지이다.

### 시간 지역성 (Temporal Locality)

시간 기준으로 방금 참조된 데이터가 또 참조될 확률이 높다.

### 공간 지역성 (Spatial Locality)

공간 기준으로 방금 참조된 데이터 근처의 데이터가 참조될 확률이 높다.

이 캐시 기능은 딱히 개발자가 프로그래밍하지 않아도 알아서 작동하게 된다.

```C#
class Program
    {
        static void Main(string[] args)
        {
            int[,] arr = new int[10000, 10000];
            {
                long now = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; y++)
                    for (int x = 0; x < 10000; x++)
                        arr[y, x] = 1;
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(y,x) 순서 걸린 시간 {end - now}");
            }
            {
                long now = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; y++)
                    for (int x = 0; x < 10000; x++)
                        arr[x,y] = 1;
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(x,y) 순서 걸린 시간 {end - now}");
            }
        }
    }
```

![캐시 실습](https://user-images.githubusercontent.com/67315288/126037647-af138e94-aa28-4788-b6e2-94001a1b5863.png)

실제로 두개의 로직은 똑같지만 실행시간은 엄청나게 차이가 난다.

왜냐하면 캐시메모리에는 [][][][][] [][][][][] [][][][][] [][][][][] 이런식으로 이미 참조할 데이터를 올려놓았고

1번째 반복문은 arr 배열의 x열 부터 순회하기 때문에 캐시 메모리에 올라와있는 순서대로 참조를 하며 빠르게 순회하지만

2번째 반복문은 arr 배열의 y행 부터 순회하기 때문에 캐시 메모리에 올라와있는 순서와는 다르게 참조하기 때문이다.

캐시는 이렇듯 연산처리를 빠르게 해줄수있게 해준다.  
하지만 멀티 쓰레드 환경에서는 캐시가 꽉 찼을 때 프로세서가 새 데이터가 들어갈 자리를 마련하기 위해 캐시에서 데이터를 제거하게 되는데 제거할 데이터를 고를 때 최근에 사용되지 않은 데이터를 고른다.  
이렇게 하면 대게 이전 타임 슬라이스의 데이터가 선택되는데, 서로 상대방의 데이터를 제거하기 때문에 성능이 오히려 저하된다.

## 메모리 배리어

이전에 컴파일러의 최적화에 대해서 설명했는데 최적화는 컴파일러 뿐 아니라 하드웨어적으로도 일어나게 된다.

현대 CPU는 성능을 좋기 하기 위해 최적화를 거쳐 순서를 바꿔서 실행시킬 수 있는데 이 또한 멀티 쓰레드 환경을 고려하지 않은 최적화 일 경우 문제가 발생한다.

```C#
  class Program
    {
        static int x = 0;
        static int y = 0;
        static int r1 = 0;
        static int r2 = 0;

        static void Thread_1()
        {
            y = 1; // Store y

            r1 = x; // Load x
        }

        static void Thread_2()
        {
            x = 1; // Store x

            r2 = y; // Load y
        }

        static void Main(string[] args)
        {
            int count = 0;
            while(true)
            {
                count++;
                x = y = r1 = r2 = 0;

                Task t1 = new Task(Thread_1);
                Task t2 = new Task(Thread_2);
                t1.Start();
                t2.Start();

                Task.WaitAll(t1, t2);

                if (r1 == 0 && r2 == 0)
                    break;
            }

            Console.WriteLine($"{count}번만에 빠져나옴!");
        }
    }
```

위의 코드는 로직대로라면 절대로 while문을 빠져나오지 못해야 하지만 실행하면 while을 탈출하게된다.

---

![하드웨어 최적화](https://user-images.githubusercontent.com/67315288/126038981-c29f5388-c4be-4b03-bd40-55b218a0d27f.png)

하지만 이렇게 while을 탈출하게 되는데 그 이유는 바로 하드웨어 최적화 때문이다.

```C#
//서로 독립된 변수에 값을 할당 하기 때문에 하드웨어는 이 코드의 순서를 바꿔도 문제가 없다고 생각하여 코드의 순서를 바꿔 실행하게 된다.
//이와같이 순서를 바꾸면서 r1 과 r2가 0이 되는 조건을 만족하는 시점이 생기는것이다.
        static void Thread_1()
        {
            r1 = x; // Load x

            y = 1; // Store y

        }

        static void Thread_2()
        {
            r2 = y; // Load y

            x = 1; // Store x
        }

///////
// 이와같이 메모리 배리어로 막으면된다.
///////
                static void Thread_1()
        {
            y = 1; // Store y

            //------------------------------------
            Thread.MemoryBarrier();

            r1 = x; // Load x
        }

        static void Thread_2()
        {
            x = 1; // Store x

            //------------------------------------
            Thread.MemoryBarrier();

            r2 = y; // Load y
        }

```

## Interlocked

멀티쓰레드를 사용하면 겪게 될 문제 중 하나로 Race Condition이 있다.

### 경합 조건 ( Race condition )

[Race Condition](https://en.wikipedia.org/wiki/Race_condition)은 공유 자원에 대해 여러 개의 쓰레드가 동시에 접근을 시도할 때 접근의 타이밍이나 순서 등이 결과 값에 영향을 줄 수 있는 상태를 말한다.

```C#

class Program
    {
        static int number = 0;

        static void Thread_1()
        {

            for (int i = 0; i < 100000; i++)
            {
                 number++;
            }
        }
        static void Thread_2()
        {
            for (int i = 0; i < 100000; i++)
            {
                number--;
            }
        }
        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(number);
        }
    }

```

> 실행 결과

![레이스 컨디션](https://user-images.githubusercontent.com/67315288/126055425-e824c367-140f-47a2-b606-b2c5409d39e0.png)

기존의 싱글쓰레드 환경이었다면 0이 나와야 하지만 이상한 값이 나오게 된다.

이 실행 코드를 디스어셈블리 해보면

![레이스 컨디션 디스 어셈블리](https://user-images.githubusercontent.com/67315288/126055536-ee852291-b87f-4fc0-8319-0cece6541247.png)

이런 결과가 나온다.

> mov 7FFDB3BD 값을 ecx로 옮기고  
> inc ecx ecx값을 1 증가 시킨다음  
> mov 다시 그 값을 7FFDB3BD로 옮긴다.

즉, 개발자는 1줄의 코드를 작성했지만 실제 기계어 동작에서는 3단계로 나눠서 실행하게 된다. 기계어에서는 동작을 왜 3단계로 나누는지 이상하게 생각할 수 있지만 값을 더한다는 동작 자체가 위와 같이 3단계로 진행 해야되는 애당초 더이상 쪼갤수없는 원자성의 최소 단위 동작이기 때문이다.

```C#
// 기계어를 의사코드로 표현한 동작.
static void Thread_1()
        {
            // atomic = 원자성

            for (int i = 0; i < 100000; i++)
            {
                int temp = number; // 0
                temp += 1; // 1
                number = temp; // number = 1
            }
        }
        static void Thread_2()
        {
            for (int i = 0; i < 100000; i++)
            {
                int temp = number; // 0
                temp -= 1; // -1
                number = temp; // number = -1
            }
        }
```

그리하여 위와 같이 동작을 하게될때 참조하는 number의 값이 순서대로 동작한다면 상관이 없지만 연산을 하기전에 참조하였던 값과 연산을 하고나서의 참조값이 같다는 보장을 할 수 없기 때문에 race condition을 겪게 되는것이다.

```C#
   static void Thread_1()
        {
            // atomic = 원자성
            for (int i = 0; i < 100000; i++)
            {
                Interlocked.Increment(ref number);
            }
        }
        static void Thread_2()
        {
            for (int i = 0; i < 100000; i++)
            {
                Interlocked.Decrement(ref number);
            }
        }

```

위처럼 Interlocked를 사용하면 공유 데이터를 어떤 한 쓰레드에서 연산할때 동안 다른 쓰레드에서는 연산을 하지않게된다.  
그리고 ref 키워드를 사용하면 어느 시점의 값을 가져와서 증가 시키는 것이 아닌 직접 주소의 참조값에 접근하여 어떤 값이 있는진 모르지만 1 증가시킨다라는 연산을 하게된다.

## 데드락

교착 상태라고도 하며 공유 자원을 동시에 여러 곳에서 사용하려고 할 때 발생할 수 있다.

여러가지 상황이 있는데, 개발자의 설계 실수로 lock을 걸고 안풀어주는 경우도 있고, lock의 순서를 서로 다른 쓰레드가 반대로 접근해 서로 다음 연산을 못하는 경우도 있고, 동시에 락을 걸어서 빠져나가지 못하는 경우도 있다.

```C#
// 데드락 예제

 class SessionManager
    {
        static object _lock = new object();

        public static void TestSession()
        {
            lock (_lock)
            {

            }
        }

        public static void Test()
        {
            lock (_lock)
            {
               UserManager.TestUser();
            }
        }

    }
    class UserManager
    {
        static object _lock = new object();

        public static void Test()
        {
            lock (_lock)
            {
                SessionManager.TestSession();
            }
        }

        public static void TestUser()
        {
            lock (_lock)
            {

            }
        }
    }
    class Program
    {
        static int number = 0;
        static object _obj = new object();

        static void Thread_1()
        {
            for (int i = 0; i < 100; i++)
            {
                SessionManager.Test();
            }
        }

        // 데드락 DeadLock

        static void Thread_2()
        {
            for (int i = 0; i < 100; i++)
            {
                UserManager.Test();
            }
        }
        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();

            Thread.Sleep(1000);

            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(number);
        }
    }

```

---

lock에는 몇 가지 구현 방법이 존재한다.  
구현 방법에 대해 알아보자

### 1. 무조건 대기 시키는 방법

SpinLock

락이 풀릴때까지 계속 기다리며 접근하는 방법이다.

```C#
 class SpinLock
    {
         volatile int _locked = 0;

        public void Acquire()
        {
            while (true)
            {
                // version 1
                //int original = Interlocked.Exchange(ref _locked, 1);
                //if (original == 0)
                //    break;

                // version 2
                // CAS Compare-And-Swap
                int expected = 0;
                int desired = 1;
                if(Interlocked.CompareExchange(ref _locked, desired, expected)==expected)
                    break;
            }
        }

        public void Release()
        {
            _locked = 0;
        }
    }

    class Program
    {
        static int _num = 0;
        static SpinLock _lock = new SpinLock();

        static void Thread_1()
        {
            for(int i=0; i<100000; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();

            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num--;
                _lock.Release();
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(_num);
        }
    }
```

기본 동작은 lock이 걸려있으면 계속해서 접근하여 lock이 풀린지 확인하고 lock이 풀리는 순간 공유 자원에 접근한 다음 락을 거는 형식이다.

단점은 계속해서 lock을 확인하기 때문에 CPU 점유율이 높아져 부하가 걸린다는 점이 있다. 그래서 설계시 lock이 금방 풀릴 것 같은 상황에서 사용하는것이 바람직하다.

데드락 방지로 interlock.CompareExchange 함수를 사용해 비교를 하고 값을 변경한다

CPU 차원에서 메모리 락을 걸어 한 쓰레드만 접근해서 메모리 조작을 할 수 있게 막아버리기 때문에 데드락 현상이 발생하지 않게 된다.

---

### 2. 양보하는 방법

```C#
    class SpinLock
    {
         volatile int _locked = 0;

        public void Acquire()
        {
            while (true)
            {
                // version 1
                //int original = Interlocked.Exchange(ref _locked, 1);
                //if (original == 0)
                //    break;

                // version 2
                // CAS Compare-And-Swap
                int expected = 0;
                int desired = 1;
                if(Interlocked.CompareExchange(ref _locked, desired, expected)==expected)
                    break;

                //Thread.Sleep(1); // 무조건 휴식 => 무조건 1ms 정도 쉰다.
                //Thread.Sleep(0); // 조건부 양보 => 나보다 우선순위가 낮은 애들한테는 양보 불가. => 우선순위가 나보다 같거나 높은 쓰레드가 없으면 다시 본인한테
                Thread.Yield();  // 관대한 양보 => 지금 실행이 가능한 쓰레드가 있으면 실행한다 => 실행 가능한 애가 없으면 남은 시간 소진
            }
        }

        public void Release()
        {
            _locked = 0;
        }
    }


```

기본 동작은 기존 스핀락에서 대기하는 부분만 추가되었다.

lock이 걸려있으면 일단 대기 후 ( 다른 쓰레드에게 양보 ) 일정 시간후에 다시 접근하여 lock이 풀려있으면 실행을 하게된다.

단점은 lock이 걸려있을때 일정 시간 대기 후 다시 돌아와 접근해도 lock이 풀려있다는 보장이 없기 때문에 기아 현상이 발생 할 수 도 있다.  
그리고 대기를 하기위해 Context Switching 한다는 것 자체가 현재의 Task를 저장하고 다음 진행할 Task의 상태 값을 읽어 적용하는 과정인데 결국 이러한 과정이 계속 발생하면 부하가 크다.

경우에 따라서는 운영 체제의 유저 모드에서 머무르며 계속해서 접근하는 스핀락 기법이 더 도움된다.

---

### 3-1 커널단계에 lock이 풀림을 알려주는 요청을 하는 방법 / AutoResetEvent & Manualresetevent

```C#
 class Lock
    {
        // bool <= 커널에서 관리하는 변수
        AutoResetEvent _available = new AutoResetEvent(true);

        public void Acquire()
        {
            _available.WaitOne(); // 입장 시도 // 자동으로 Reset 해줌
            //_available.Reset(); // bool = false
        }
        public void Release()
        {
            _available.Set(); // flag = true
        }
    }

    class Program
    {
        static int _num = 0;
        static Lock _lock = new Lock();
        static void Thread_1()
        {
            for(int i=0; i<10000; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.Acquire();
                _num--;
                _lock.Release();
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(_num);
        }
    }
```

lock이 걸려있으면 대기를 하게 되는데 lock이 풀리는 순간을 커널 단계에 요청을 하게된다.  
그리고 대기중이던 쓰레드가 접근하여 lock을 다시 걸고 동작을 수행하게 된다.  
이 또한 Context Switching이 일어나게 되므로 매우 느린 동작을 한다.

### 3-2 커널단계에 lock이 풀림을 알려주는 요청을 하는 방법 / Mutex

```C#
  class Program
    {
        static int _num = 0;
        static Mutex _lock = new Mutex();
        static void Thread_1()
        {
            for(int i=0; i<10000; i++)
            {
                _lock.WaitOne();
                _num++;
                _lock.ReleaseMutex();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.WaitOne();
                _num--;
                _lock.ReleaseMutex();
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(_num);
        }
    }

```

커널 단계에 요청을 해서 lock을 구현하는 방법으로는 또 Mutex도 있다.
Mutex 클래스는 해당 머신의 프로세스간에서 조차도 배타적 locking을 하는데 사용한다 때문에 엄청나게 느리다.

### shared-exclusive lock

기존에 위에서 구현했던 lock은 공유자원에 접근하는 스레드를 오직 한 개로 제한하기 때문에 안전하지만, 어떤 경우에는 비효율적이었다. 예를들어 여러 스레드가 공유 자원에 동시에 접근해야 하지만 그 중 정말 일부 쓰레드만 값을 변경하는 경우이다.

이런 경우 값을 읽기만 하는 스레드는 동시에 접근을 해도 상관없다. 하지만 어떤 스레드가 값을 변경하고 있으면, 다른 스레드는 공유 자원에 접근해서는 안된다. 반대로 다른 스레드가 공유 자원에 접근하고 있는 중에는 값을 변경하는 스레드는 접근해서는 안된다.

이때 사용하는 것이 바로 shared-exclusive lock이라고도 하는 readers-writer lock이다. 이미 read lock이 잡혀있는 reader-writer lock에 read lock을 잡으면 바로 lock이 잡히고 다음 코드를 실행할 수 있지만, write lock을 잡으면, lock을 잡지 못하고 read lock이 풀릴 때 까지 기다린다.

```C#
 // 재귀적 락을 허용할지 (Yes) WriteLock->WriteLock OK, WriteLock->ReadLock OK, READLock->WriteLock NO
    // 스핀락 정책 (5000번 -> Yield)
    class Lock
    {
        const int EMPTY_FLAG = 0x00000000;
        const int WRITE_MASK = 0x7FFF0000;
        const int READ_MASK = 0x0000FFFF;
        const int MAX_SPIN_COUNT = 5000;

        // [Unuserd(1)] [WriteThreadId(15)] [ReadCount(16)]
        int _flag = EMPTY_FLAG;
        int _writeCount = 0;

        public void WriteLock()
        {
            // 동일 쓰레드가 WriteLock을 이미 획득하고 있는지 확인
            int lockThreadId = (_flag & WRITE_MASK) >> 16;
            if (Thread.CurrentThread.ManagedThreadId == lockThreadId)
            {
                _writeCount++;
                return;
            }

            // 아무도 WriteLock or ReadLock을 획독하고 있지 않을 때, 경합해서 소유권을 얻는다
            int desired = (Thread.CurrentThread.ManagedThreadId << 16) & WRITE_MASK;
            while (true)
            {
               for(int i=0; i<MAX_SPIN_COUNT; i++)
                {
                    // 시도를 해서 성공하면 return
                    if (Interlocked.CompareExchange(ref _flag, desired, EMPTY_FLAG) == EMPTY_FLAG)
                    {
                        _writeCount = 1;
                        return;
                    }
                }
                Thread.Yield();
            }
        }

        public void WriteUnlock()
        {
            int lockCount = --_writeCount;
            if (lockCount == 0)
                Interlocked.Exchange(ref _flag, EMPTY_FLAG);
        }

        public void ReadLock()
        {
            // 동일 쓰레드가 WriteLock을 이미 획득하고 있는지 확인
            int lockThreadId = (_flag & WRITE_MASK) >> 16;
            if (Thread.CurrentThread.ManagedThreadId == lockThreadId)
            {
                Interlocked.Increment(ref _flag);
                return;
            }

            // 아무도 WriteLock을 획득하고 있지 않으면, ReadCount를 1 늘린다.
            while (true)
            {
                for (int i = 0; i < MAX_SPIN_COUNT; i++)
                {
                    int expected = (_flag & READ_MASK);
                    if (Interlocked.CompareExchange(ref _flag, expected + 1, expected) == expected)
                        return;
                }

                Thread.Yield();
            }
        }

        public void ReadUnlock()
        {
            Interlocked.Decrement(ref _flag);
        }
    }

```
