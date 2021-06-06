## Stat

RPG에서 스탯은 필수요소이다.

```C#

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
  [SerializeField] protected int _level;
  [SerializeField] protected int _hp;
  [SerializeField] protected int _maxHp;
  [SerializeField] protected int _attack;
  [SerializeField] protected int _defense;
  [SerializeField] protected float _moveSpeed;

  // 처음부터 프로퍼티를 설정하지 않은 이유는 유니티에서 제공하는 SerializeField는 private에만 적용 가능함.
  public int Level { get => _level; set { _level = value; } }
  public int Hp { get => _hp; set { _hp = value; } }
  public int MaxHp { get => _maxHp; set { _maxHp = value; } }
  public int Attack { get => _attack; set { _attack = value; } }
  public int Defense { get => _defense; set { _defense = value; } }
  public float MoveSpeed { get => _moveSpeed; set { _moveSpeed = value; } }

  private void Start()
  {
    _level = 1;
    _hp = 100;
    _maxHp = 100;
    _attack = 10;
    _defense = 5;
    _moveSpeed = 5.0f;
  }
}


```

모든 스탯의 공통 요소는 Stat에 넣고

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
  [SerializeField] protected int _exp;
  [SerializeField] protected int _gold;

  public int Exp { get => _exp; set { _exp = value; } }
  public int Gold { get => _gold; set { _gold = value; } }

  private void Start()
  {
    _level = 1;
    _hp = 100;
    _maxHp = 100;
    _attack = 10;
    _defense = 5;
    _moveSpeed = 5.0f;
    _exp = 0;
    _gold = 0;
  }
}

```

Player의 스탯은 Stat을 상속받아서 넣었다.

참고로 유니티에서 protected와 private에 대한 SerializeField attribute를 지원하지만 프로퍼티는 지원을 안하므로 프로퍼티를 따로 구현을 해주어야한다.

## MouseCursor

RPG게임에서 MouseCursor모양은 자주 바뀐다. 롤이나 디아블로와 같은 마우스로 이동하는 게임에서 커서의 모양은 현재 하는 행동의 정보 혹은 현재 타겟팅하고 있는 오브젝트등 정보를 제공하는 역할을 하기도 하는데 마우스 커서의 모양이 바뀌도록 구현해보자.

## HP Bar

체력바도 하나의 UI이다. 하지만 게임안에서 오브젝트와 같이 3D로 있기 위해서는 UI 캔버스 렌더러 모드를 World Space로 바꿔주어야 한다.

그리고 체력바를 오브젝트의 자식으로 넣어주게 되면 되는데 하지만 부모오브젝트가 회전시 같이 회전하는 문제가 있으므로 이를 수정하려면 빌보드를 사용해야한다.

> 빌보드란 카메라가 바라보는 방향으로 동일하게 UI이가 카메라를 바라보도록 회전하는 개념이다. 즉 부모 오브젝트의 회전값을 따라가는게 아니고, 카메라를 고정적으로 바라보도록 회전 시키는것이다.

```C#

  void Update()
  {
    Transform parent = transform.parent;
    // 부모 오브젝트의 콜라이더.Y 크기 보다 1만큼 더한 위치에 HpBar를 위치시킴.
    // 이로써 어떤 오브젝트가 와도 콜라이더의 크기에 따라 자연스럽게 HPBar 위치가 잡히게된다.
    transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
    transform.rotation = Camera.main.transform.rotation;
  }

```

## Monster AI

몬스터 AI의 구현은 Player의 State와 매우 비슷한 로직으로 동작한다. 즉, 대기, 이동, 공격과 같이 플레이어와 같은 방식으로 동작하는데 이는 몬스터 또한 Player와 비슷하게 코드를 작성한다는 뜻이고 이는 상속을 이용하면 간단하게 구현이 가능하다는 뜻이다.

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
  [SerializeField] protected Vector3 _destPos;
  [SerializeField] protected Define.State _state = Define.State.Idle;
  [SerializeField] protected GameObject _lockTarget;

  public virtual Define.State State
  {
    get => _state;
    set
    {
      _state = value;

      Animator anim = GetComponent<Animator>();
      switch (_state)
      {
        case Define.State.Die:
          break;
        case Define.State.Idle:
          anim.CrossFade("WAIT", 0.1f);
          break;
        case Define.State.Moving:
          anim.CrossFade("RUN", 0.1f);
          break;
        case Define.State.Skill:
          anim.CrossFade("ATTACK", 0.1f, -1, 0);
          break;
      }
    }
  }

  private void Start()
  {
    Init();
  }

  void Update()
  {
    switch (State)
    {
      case Define.State.Die:
        UpdateDie();
        break;
      case Define.State.Moving:
        UpdateMoving();
        break;
      case Define.State.Idle:
        UpdateIdle();
        break;
      case Define.State.Skill:
        UpdateSkill();
        break;
    }
  }

  public abstract void Init();
  protected virtual void UpdateDie() { }
  protected virtual void UpdateMoving() { }
  protected virtual void UpdateIdle() { }
  protected virtual void UpdateSkill() { }
}


```

이를 이용해서 오브젝트 (플레이어나 몬스터등과 같은 ) 상태를 관리하는 BaseController를 만들고 그것을 상속하는 MonsterController를 만들어서 상태에 따른 동작만 구현을 하면 된다.

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
  Stat _stat;

  [SerializeField] float _scanRange = 10;
  [SerializeField] float _attackRange = 2;

  public override void Init()
  {
    _stat = gameObject.GetComponent<Stat>();

    if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
      Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
  }

  protected override void UpdateIdle()
  {
    Debug.Log("Monster UpdateIdle");

    // TODO : 매니저가 생기면 옮기자
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    if (player == null)
      return;

    float distance = (player.transform.position - transform.position).magnitude;
    if (distance <= _scanRange)
    {
      _lockTarget = player;
      State = Define.State.Moving;
      return;
    }
  }
  protected override void UpdateMoving()
  {
    // 플레이어가 내 사정거리보다 가까우면 공격
    if (_lockTarget != null)
    {
      _destPos = _lockTarget.transform.position;
      float distance = (_destPos - transform.position).magnitude;
      if (distance <= _attackRange)
      {
        NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
        nma.SetDestination(transform.position);
        State = Define.State.Skill;
        return;
      }
    }

    // 이동
    Vector3 dir = _destPos - transform.position;
    // 도착
    if (dir.magnitude < 0.1f)
    {
      State = Define.State.Idle;
    }
    else
    {
      // TODO
      NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
      nma.SetDestination(_destPos);
      nma.speed = _stat.MoveSpeed;

      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
    }
  }
  protected override void UpdateSkill()
  {
    if (_lockTarget != null)
    {
      Vector3 dir = _lockTarget.transform.position - transform.position;
      Quaternion quat = Quaternion.LookRotation(dir);
      transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
    }
  }

  void OnHitEvent()
  {
    if (_lockTarget != null)
    {
      // 체력
      Stat targetStat = _lockTarget.GetComponent<Stat>();
      int damage = Mathf.Max(0, _stat.Attack - targetStat.Defense);
      targetStat.Hp = targetStat.Hp - damage;

      if (targetStat.Hp > 0)
      {
        Debug.Log(targetStat.Hp);
        float distance = (_lockTarget.transform.position - transform.position).magnitude;
        if (distance <= _attackRange)
          State = Define.State.Skill;
        else
          State = Define.State.Moving;
      }
      else
      {
        State = Define.State.Idle;
      }
    }
    else
    {
      State = Define.State.Idle;
    }
  }
}

```

## Destroy

유니티에서 오브젝트 Destroy 처리에 관하여

유니티에서 Destroy로 오브젝트를 제거하면 신기하게도 오브젝트를 참조 하고있는 모든 참조값에 댕글링 포인터 값이 되는게 아닌 "null"이라고 fakenull이 들어가게 된다. 실제로도 유니티에서는 오브젝트를 삭제하면 실제로 메모리에서 해제가 되지않고 유니티 엔진이 여전히 참조 값을 들고있고 fakenull을 반환 하는것이다.

> 그렇기 때문에 삭제된것 같아 보이는 오브젝트라도 컴포넌트는 여전히 접근할 수 있기 때문에 코드를 설계할 때 런타임중 Destroy 될 어떠한 gameobject의 컴포넌트를 참조하지 않도록 gameobject가 해체되면 그 오브젝트의 컴포넌트도 접근할 수 없게끔 설계 해야한다.

그리고 유니티 내부 C++코드에서 gameobject를 null과 비교를 하게끔 operator를 구현해서 비교를 할 수 있게끔 구현을 해둔것이다.

그래서 실제로 gameobject가 유효한지 아닌지 검사를 할 수 있는것이다.

```C#
public class Object
  {
    public static bool operator ==(Object x, Object y);
    public static bool operator !=(Object x, Object y);

    public static implicit operator bool(Object exists);
  }
```
