### Collider

Collider는 연산을 하기위한 참고용이라고 생각하면 된다. 보통 오브젝트의 Mesh는 굉장히 복잡하게 많은 Polygon으로 이루어져있는데 이 Polygon 자체로 물리 연산을 한다면 성능에 엄청나게 큰 부하를 줄것이다. 그래서 보통은 어느정도의 기준을 잡아서 그 기준 안에서의 물리 충돌을 연산하도록 하는데 Collider는 그 연산의 기준이 된다.

> FPS와 같은 신체 부위 별로 데미지를 다르게 하는 게임 장르에서는 더 세밀하게 설정한다.

그래서 Collider 만으로는 물리적 충돌, 관통등 실제 물리적 행동을 할 수 없다.

### Rigidbody

Collider를 참고하여 물리적 연산을 하는 두뇌에 해당한다.

### Collision & Trigger

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
