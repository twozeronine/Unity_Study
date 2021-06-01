## 애니메이션 블렌딩

유니티에서는 언리얼 엔진과 비슷하게 애니메이션을 섞는 기능을 제공한다.

Blend Tree를 만들어 이 기능을 사용하면 애니메이션 동작을 더 부드럽게끔 실행 시킬 수 있다.

```C#
// field에 선언 해준 값 ( 애니메이션을 얼마나 섞을지 결정하는 수치이다. )
 float wait_run_ratio = 0;

Animator anim = GetComponent<Animator>();
    // 목적지에 도착 하였을때
    if (_moveToDest)
    {
      wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10.0f * Time.deltaTime);
      anim.SetFloat("wait_run_ratio", wait_run_ratio);
      anim.Play("WAIT_RUN");
    }
    else
    {
      wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10.0f * Time.deltaTime);
      anim.SetFloat("wait_run_ratio", wait_run_ratio);
      anim.Play("WAIT_RUN");
    }

```

## State 디자인 패턴

```C#
bool isFalling;
bool isJumping;
bool isSkillCasting;
bool isSkillChanneling;

if(isJumping)
{

}
else
{
    if(isSkillChanneling && isFalling)
    {

    }
    else
    {
        if(isSkillCasting)
        {

        }
    }
}
```

게임을 만들때 애니메이션이 추가될때마다 bool값을 추가시켜 이런식으로 계속 만든다면 나중에 스파게티 코드가 되어서 코드의 복잡성이 엄청나게 높아질 위험성이 있다. 그래서 이러한 문제점을 보안하기 위해서 디자인 패턴인 State 패턴을 사용하자.

```C#

    // 미리 enum으로 상태들을 정의하고 case 문을 사용한다.
    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
    }

    PlayerState _state = PlayerState.Idle;

    void Update()
    {
        switch (_state)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
        }
    }

    // 해당 상태에 해당하는 메소드들을 구현해준다.
    void UpdateDie()
    {
        // 아무것도 못함
    }

    void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.0001f)  // 도착함
        {
          // 도착했으므로 상태를 Idle로 갱신
            _state = PlayerState.Idle;
        }
        else   // 도착 X
        {
            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
            transform.position = transform.position + dir.normalized * moveDist;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            transform.LookAt(_destPos);
        }

        // 애니메이션
        wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10.0f * Time.deltaTime);
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("wait_run_ratio", wait_run_ratio);
        anim.Play("WAIT_RUN");
    }

    void UpdateIdle()
    {
        // 애니메이션
        wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10.0f * Time.deltaTime);
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("wait_run_ratio", wait_run_ratio);
        anim.Play("WAIT_RUN");
    }


    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die)
            return;

        if (evt != Define.MouseEvent.Click)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Wall")))
        {
            _destPos = hit.point;
            // 움직여야 되므로 상태를 이동 상태로 갱신.
            _state = PlayerState.Moving;
        }
    }
```

이런식으로 구현할 수 있다. 하지만 bool 상태 플래그 변수와는 다르게 동시에 두가지 상태를 가질 순 없다 ex) 점프하며 공격, 스킬 쓰며 이동등  
하지만 이와 같은 특성 때문에 여러 동작이 꼬일 일이 없다는 장점이 있기 때문에 어느 정도 규모가 더 커기지 전까지는 사용 가능한 디자인 패턴이다.

```C#

isJumping && isAttacking  // 가능

_state == PlayerState.Jump && _state == PlayerState.Attack  // 불가능

```

## State Machine

State 패턴과 관련이 깊다. 어떤 상태이냐 따라 재생시킬 애니메이션이 다르다. State Machine 에서 하나의 State는 하나의 Animation 클립에 대응한다.

### Has Exit Time

애니메이션 재생 종류 후 다음 전이로 이동한다. ( Condition 조건에 따라 전이 되는게 아님 )

- 체크 -> 종료 시점이 존재하게 되는 것.
  -- Settings 의 Fixed Duration이 체크되어 있다면 Exit Time 값이 “시간”으로 해석된다. Exit Time 값이 0.7 이고 Has Exit TIme이 체크되어 있다면 절대적인 “0.7초”의 시간 후에 다음 전이가 일어난다.
  -- Settings 의 Fixed Duration이 체크되어 있지 않다면 Exit Time 값이 “% 비율”로 해석된다. Exit Time 값이 0.7 이고 Has Exit TIme이 체크되어 있다면 “해당 애니메이션의 전체 재생 길이의 70 %”까지만 재생한 후에 다음 전이가 일어난다.
- 체크 해제 -> 애니메이션이 다 재생 되더라도 전이가 일어나지 않는다.
  -- 전이가 일어나려면 Contidion 조건을 만족해야 한다.
  -- 따라서 파라미터 값으로 Condition 조건을 통해 전이가 즉각 일어나게 하려면 Has Exit Time 을 체크 해제해주어야 한다.

어떠한 파라미터 값을 넣고 그 값의 상태에 따라 애니메이션을 전환하고 싶다면 Has Exit Time 체크를 해제 해야된다고 이해하자.

## KeyFrame Animaion

이미 만들어져 있는, 모델에 종속된 애니메이션이 아닌 게임에 의존적인 에니메이션을 직접 만들때 사용하는 기능.

Animation에서 녹화 버튼을 눌러 어디로 이동하거나 Transform, Rigidbody, collider등 설정 하여 애니메이션을 구현할 수 있다.

응용 방법은 너무도 다양하다. 텍스쳐를 투명하게 바꿔 오브젝트를 안보이게도 할 수 있고 스케일을 바꿔 크기등을 바꿀수도있고 여러가지로 가능하다.
