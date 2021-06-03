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
