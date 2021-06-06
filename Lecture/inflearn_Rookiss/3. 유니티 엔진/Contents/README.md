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
