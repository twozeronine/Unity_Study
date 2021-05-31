## Collider

Collider는 연산을 하기위한 참고용이라고 생각하면 된다. 보통 오브젝트의 Mesh는 굉장히 복잡하게 많은 Polygon으로 이루어져있는데 이 Polygon 자체로 물리 연산을 한다면 성능에 엄청나게 큰 부하를 줄것이다. 그래서 보통은 어느정도의 기준을 잡아서 그 기준 안에서의 물리 충돌을 연산하도록 하는데 Collider는 그 연산의 기준이 된다.

> FPS와 같은 신체 부위 별로 데미지를 다르게 하는 게임 장르에서는 더 세밀하게 설정한다.

그래서 Collider 만으로는 물리적 충돌, 관통등 실제 물리적 행동을 할 수 없다.

## Rigidbody

Collider를 참고하여 물리적 연산을 하는 두뇌에 해당한다.

## Collision & Trigger

Collsion : 물리적으로 부딪친 오브젝트에 대한 정보가 담겨있다.
Trigger : 물리적으로 부딫치지 않아도 내 Collider 범위 안에 들어온 오브젝트에 대한 정보가 담겨있다.

해당 정보들을 조회하기 위해서는 메소드가 필요한데 유니티에서 기본적으로 제공해주는 메소드가 있다.

OnCollision/ Enter, Exit, Stay  
OnTrigger/ Enter, Exit, Stay

그리고 해당 메소드가 호출되려면 어떠한 조건들이 필요한데 버전 마다 달라 질 수 있으니 [유니티 공식 사이트](https://docs.unity3d.com/Manual/CollidersOverview.html)를 참고하자.

OnCollision

1. 나 혹은 상대한테 RigidBody 있어야한다 (IsKinematic : Off)
2. 나한테 Collider가 있어야 한다 (IsTrigger : Off)
3. 상대한테 Collider가 있어야 한다 (IsTrigger : Off)

OnTrigger

1. 둘 다 Collider가 있어야 한다
2. 둘 중 하나는 IsTrigger : On
3. 둘 중 하나는 RigidBody가 있어야 한다

## Raycast

직선으로 광선을 쏴서 충돌되는 물체가 있는지 없는지를 검사하는 Raycasting

- 유니티 에디터에서 실제 화면에 보이는 오브젝트를 클릭할 때 클릭이 되는데 이것이 Raycast 기능을 이용한 것이다.
- 3인칭 카메라 시점에서 카메라와 플레이어 사이의 장애물이 있을때 장애물을 가리거나 혹은 카메라를 장애물 앞으로 이동할때도 쓰일 수 있다.
- 롤이나 스타크래프트와 같은 마우스로 화면상에 찍은 곳으로 이동 시킬때도 Raycast를 활용 가능하다.

유니티에는 해당 기능을 구현하기 위해 함수를 제공한다.

1. Raycast : Ray에 부딪힌 물체 하나만을 처리할때 사용
2. RaycastAll : Ray에 부딪힌 물체를 모두 처리

두개의 메소드는 둘 다 오버라이딩이 많이 되어있어서 사용시 필요한 인수들을 잘 조합하여 사용하자.

### 투영의 개념

실제 Raycast를 사용하여 좌표를 얻어 올때는 사용자가 보는 2D 화면에서 마우스 커서의 screen 좌표로 Ray를 쏴서 3D 게임상에서 오브젝트의 World 좌표를 구해오게 되는데 이때 카메라가 보고있는 3D 화면을 2D 화면으로 보여지게끔 하는것을 투영이라고 한다. 이를통해 Z축은 무시하고, X,Y 좌표만 고려한 실제 게임 월드 위치 상에서의 비율이 지켜진다.

실제 구현을 해보자.  
Local <-> World <-> ( Viewport <-> Screen ) ( 화면 )

```C#
    // Input.mousePosision으로 마우스 커서 위치 좌표를 구한다.
    //그리고 그 좌표값과 Camera의 nearClipPlane을 합친 Vector3 값으로 World좌표값을 구한다.
    Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

    // 위에서 구한 그 좌표값과 카메라가 존재하는 벡터 값을 빼면 실제 World 좌표로 향하는 방향 벡터가 나온다.
    Vector3 dir = mousePos - Camera.main.transform.position;
    // 그 벡터를 크기가 1인 벡터로 방향만 가져온다.
    dir = dir.normalized;

    // 클릭한 부분까지 100.0f 거리의 Ray를 그려준다.
    Debug.DrawRay(Camera.main.transform.position, dir * 100.0f, Color.red, 1.0f);


    // 위의 코드는 개념을 설명하기 위함이었고 실제로는 이렇게 사용한다.
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
```

### LayerMask

레이 캐스팅은 무거운 연산이다.
따라서 모든 충돌 가능한 콜라이더를 연산한다면 부하가 커질것이다. 따라서 최적화를 위해서 유니티에서는 Layer 기능을 제공한다. 그래서 Layer를 적용하여 해당 Layer만을 연산하도록 최적화 작업을 해줘야한다.

유니티에서는 Layer를 지정할 수 있도록 LayerMask를 제공하는데 LayerMask는 32비트의 int 형이다.

```C#
// Layer를 가져오는 2가지 방법

  // 비트플래그를 통하여 가져오기
  // 첫번째 비트를 8번째 까지 왼쪽으로 시프트
  int mask = (1 << 8) | (1 << 9);

  // 레이어 이름을 통하여 가져오기
      LayerMask mask = LayerMask.GetMask("Monster") | LayerMask.GetMask("Wall");
```

### Tag

별다른 기능은 없고 그냥 추가적으로 오브젝트들을 그룹지어 구분해주기 위한 정보이다.

게임 오브젝트를 Tag를 사용하여 찾아내거나 해당 Tag를 가진 오브젝트만 연산을 할때에 사용한다.
