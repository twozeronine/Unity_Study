### Position

오브젝트의 위치. 이동과 관련되어있다.

### Scale

오브젝트의 크기. 확대/축소

거대한 모델을 필요하다고 가정하면, 모델을 만들 때 처음부터 거대한 모델을 만들 필요 없이 모델은 작게 만들어주고 유니티에 와서 Scale을 통해 크기를 늘리면 된다.

### 이동

유니티에서 이동은 현실의 이동과 비슷한 로직으로 동작한다.  
예를들면 현실에서 거리 = 속력 _ 시간 이고, 유니티에서는 transform.position += new Vector3(0.0f, 0.0f, 1.0f) _ Time.deltaTime \* \_speed; 와 같이 표현해줄수있다.

플레이어의 이동을 구현할때는 World 좌표계와 Local 좌표계의 개념을 알아야하는데

플레이어의 오브젝트가 어느 방향을 바라보던 World 좌표계의 방향은 정해져있다. 그래서 실제로 플레이어가 보는 방향으로 이동을 해야한다면 transform.Translate(Vector3 translatrion);라는 함수를 이용해야한다. 이 함수를 사용하면 플레이어가 바라보는 방향인 Local 좌표계의 값과 실제 World 좌표계의 값을 계산하여 이동하게 한다.

### Vector3

Vector3는 유니티에서 제공하는 struct 구조체이다. 위치와 방향을 표현할 때 사용한다.

목적지 위치 벡터 - 출발지 위치 벡터 = 방향 벡터출발지로부터 목적지로 향하는 방향을 의미하게 된다.

Vector3.magnitude로 방향에 대한 크기(스칼라), 즉, 방향에 대한 거리를 알 수 있다.
Vector3.normalized로 크기가 1인 오직 방향에만 무게를 둔 단위 벡터로 방향을 알 수 있다.

### Rotation

오브젝트의 회전.

회전 값을 절대적으로 설정할 때 eulerAngles

유니티에선 Transform 의 rotation 은 Vecotr3 가 아닌 Quaternion 이다.(x,y,z,w 이렇게 4개의 값이 필요) 따라서 Vector3 형태로 X, Y, Z 축 이렇게 3 개의 회전 값으로 회전 값을 설정하고 싶다면 eulerAngles를 사용해야 한다.

```C#
  tranform.eulerAngles =new Vector3(0.0f, _yAngle,0.0f); // Y 축으로 _yAngle 각도 만큼 회전.
```

유니티에서는 eulerAngles 값을 연산을 통해 더하거나 빼지않고 새로운 Vector3 값을 만들어서 대입하기를 권장한다.
왜냐하면 오일러 각도는 360도를 넘어가면 값의 계산이 실패하기 때문인데 이것은 유니티에서는 Vector3를 값으로 받지만 실제 내부에서는 Quaternion으로 계산이 되기 때문이라고 한다.

회전 값을 상대적으로 설정할 때  
eulerAngles로는 얼만큼 더 회전할지의 델타 회전값으로 사용하는 것이 불가능 하기 때문에, 얼만큼 더 회전할지의 델타 회전 값을 설정해주는 것은 Rotate 함수를 사용하여야 한다. 현재 회전값으로부터 인수로 넘기는 Vector3 만큼 더 회전한다. eulerAngles와 비슷하게 Vector3를 인수로 받는다. (내부적으로 eulerAngles만큼 더 회전시킨다.)

그리고 유니티에서는 Vector3으로 방향을 표시하면 짐벌락 현상이 발생하기 때문에 실제로는 Quaternion을 사용한다.
해당 개념을 실제 이해하여 사용하기엔 수학적으로 복소수 개념을 이해하고 여러 어려운 개념을 이해하여야 하기 때문에 유니티에서 제공하는 함수를 사용하자

> 짐벌락은 [유튜브 링크](https://www.youtube.com/watch?v=zc8b2Jo7mno) 해당 링크에서 볼 수 있다.

Quaternion.LookRotation() 함수를 통해 우리가 원하는 방향을 바라 보는 회전 값을 쿼터니언으로 리턴한다.

하지만 좀 더 부드러운 회전 값을 얻기 위해 보간법을 사용해야 한다.  
Quaternion.Slerp( Vector3 a, Vector3 b , float t) 함수를 사용하면 좀 더 부드러운 회전 각을 얻을 수 있다.
보간법은 lerp와 slerp가 있는데 애니메이션의 형태에 따라서 보간 방법을 고르는 편이 좋다.

### InputManager.cs

디자인 패턴인 옵저버 패턴을 이용해서만든 InputManager 사용자의 입력이 들어오면 KeyAction에 등록된 함수를 실행시킨다.
