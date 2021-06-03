# 3. 유니티 엔진

단지 게임을 찍어내는 강의가 아닌 게임 제작을 할 수 있는 프레임워크를 구축하는 능력을 갖는것이 목표인 강의입니다.

## 개요

### 기본 단축키

Ctrl + Shift + N : 빈 GameObject 생성  
Ctrl + Shift + F : 선택한 해당 오브젝트의 위치를 현재 사용자가 보고있는 위치로 이동 (카메라에 적용시 유용)  
Ctrl + Shift + C : 콘솔창 ( 디버깅시 로그로 출력한 내용이 여기에 출력된다)

### Component 패턴

모든 코드를 부품화해서 관리를 한다는 디자인패턴이다.  
유니티의 오브젝트는 컴포넌트 패턴을 이용해서 만들어졌다.

---

## [Singleton](https://github.com/twozeronine/Unity_Study/tree/main/Lecture/inflearn_Rookiss/3.%20%EC%9C%A0%EB%8B%88%ED%8B%B0%20%EC%97%94%EC%A7%84/Singleton)

유니티에서 작업을 하다보면 어느순간 매니저와 같이 실제 게임에서의 인스턴스는 아니지만 게임을 관리해야하는 매니저가 필요하다. 이때 사용할 수 있는 디자인패턴으로 싱글톤 패턴이 있다.

---

## [Transform](https://github.com/twozeronine/Unity_Study/tree/main/Lecture/inflearn_Rookiss/3.%20%EC%9C%A0%EB%8B%88%ED%8B%B0%20%EC%97%94%EC%A7%84/Transform)

유니티 게임 씬에서 거의 99% 모든 오브젝트는 Transform 컴포넌트가 있다. 해당 컴포넌트로 오브젝트의 이동, 회전, 스케일등의 기능을 이용 할 수 있다.

## [Prefab](https://github.com/twozeronine/Unity_Study/tree/main/Lecture/inflearn_Rookiss/3.%20%EC%9C%A0%EB%8B%88%ED%8B%B0%20%EC%97%94%EC%A7%84/Prefab)

유니티에서 인스턴스를 만들때 사용하는 붕어빵 틀과 같은 기능.

유니티에서는 오브젝트를 만드는 방식은 정말 여러가지 있을 수 있다. 에디터 창에서 편집하여 만들거나 코드로 직접 구현하여 컴포넌트를 붙여서 만들수도 있겠지만, 이러한 작업은 오브젝트가 정말 여러개 이거나 해당 오브젝트가 정말 복잡하게 설계되있다면 불가능한 작업이다 따라서 유니티에서는 오브젝트를 만들때 프리팹을 이용하여 오브젝트를 생성하는 방법을 제공하는데 이 방법으로 정말 편하게 구현할 수 있다.

프리팹을 이용하여 오브젝트를 만드는건 C#에서 클래스를 이용하여 인스턴스를 이용하는것과 같이 상당히 유사한 로직이다. 그래서 이러한 점을 이용하여 프리팹을 하나 만든후 해당 프리팹의 값을 수정하는것으로 인스턴스에 모두 적용할 수 있다.

## [Physics](https://github.com/twozeronine/Unity_Study/tree/main/Lecture/inflearn_Rookiss/3.%20%EC%9C%A0%EB%8B%88%ED%8B%B0%20%EC%97%94%EC%A7%84/Physics)

게임에서 물리는 없어선 안될 가장 중요한 요소이다. 보통은 물리 기능을 구현하기 위해서 엄청나게 큰 수학적 지식과 노력을 필요하지만 유니티에서는 물리 연산을 하기위해 몇 가지 기능들을 제공한다.

## [Camera](https://github.com/twozeronine/Unity_Study/tree/main/Lecture/inflearn_Rookiss/3.%20%EC%9C%A0%EB%8B%88%ED%8B%B0%20%EC%97%94%EC%A7%84/Camera)

유니티 카메라 기능.

## [Animation](https://github.com/twozeronine/Unity_Study/tree/main/Lecture/inflearn_Rookiss/3.%20%EC%9C%A0%EB%8B%88%ED%8B%B0%20%EC%97%94%EC%A7%84/Animation)

유니티 애니메이션 기능.

## [UI](https://github.com/twozeronine/Unity_Study/tree/main/Lecture/inflearn_Rookiss/3.%20%EC%9C%A0%EB%8B%88%ED%8B%B0%20%EC%97%94%EC%A7%84/UI)

유니티 UI ( UGUI ) 기능.

## [Scene](https://github.com/twozeronine/Unity_Study/tree/main/Lecture/inflearn_Rookiss/3.%20%EC%9C%A0%EB%8B%88%ED%8B%B0%20%EC%97%94%EC%A7%84/UI)

게임에서 로비, 게임화면 등등 구현할때 씬을 나누는데 씬을 관리하는 방법에 대해서 알아보자.

## [Sound](https://github.com/twozeronine/Unity_Study/tree/main/Lecture/inflearn_Rookiss/3.%20%EC%9C%A0%EB%8B%88%ED%8B%B0%20%EC%97%94%EC%A7%84/UI)

게임에서 없으면 안될 중요한 사운드 어떻게 다루는지에 대해서 알아보자.

## [Pooling](https://github.com/twozeronine/Unity_Study/tree/main/Lecture/inflearn_Rookiss/3.%20%EC%9C%A0%EB%8B%88%ED%8B%B0%20%EC%97%94%EC%A7%84/Pooling)

유니티에서 게임 오브젝트의 삭제와 생성은 부하가 큰 작업이다. 따라서 미리 캐싱해두고 오브젝트가 필요할 시 활성화 하여 사용하고 사용이 끝나면 비활성화 시켜 풀에 다시 넣는 방법이 필요하다.

## Coroutine

코루틴은 유니티에서 제공하는 기능이 아닌 C#에 이미 있는 IEnumerator 인터페이스 열거형에서 yield return의 호출시에 메인 루틴으로 갔다가 다시 서브 루틴으로 돌아가는 특성을 이용한 기능이다 ( 심지어 비동기 방식으로 작동 하는것도 아니다. [참고 사이트](https://unityindepth.tistory.com/21) )

> [링크](https://github.com/twozeronine/Unity_Study/tree/main/Coroutine) 예전에 공부 하였을때 이미 한번 내용을 정리했었다.

## [Data](https://github.com/twozeronine/Unity_Study/tree/main/Lecture/inflearn_Rookiss/3.%20%EC%9C%A0%EB%8B%88%ED%8B%B0%20%EC%97%94%EC%A7%84/Data)

프로젝트의 규모가 커질수록 하드코딩으로 데이터를 입력 하는것보다 어느 한 곳에서 데이터를 저장하여 불러오는 식으로 해야한다.

## NavMesh

유니티에서는 길찾기를 구현하기 위해 NavMesh라는 기능을 제공한다. NavMesh란 말 그대로 걸을 수 있는 Mesh라는 뜻으로 게임상에서 만들어진 맵에서 NavMesh를 Bake하여 NavMeshAgent 컴포넌트를 가진 오브젝트가 그 위를 걷게 할 수 있다.

> [Unity_Technologies](https://github.com/Unity-Technologies/NavMeshComponents)에서 제공하는 NavMeshComponents를 이용하면 동적으로 Bake도 할 수 있다.
