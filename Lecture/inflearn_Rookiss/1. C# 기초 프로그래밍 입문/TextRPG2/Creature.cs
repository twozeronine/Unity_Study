using System;

namespace TextRPG2
{

  public enum CreatureType
  {
    None = 0,
    Player,
    Monster
  }
  class Creature
  {
    CreatureType _type;
    protected int hp = 0;
    protected int attack = 0;
    protected Creature(CreatureType type) => _type = type;

    public void SetInfo(int hp, int attack)
    {
      this.hp = hp;
      this.attack = attack;
    }
    public int GetHp() => hp;
    public int GetAttack() => attack;
    public bool isDead() => hp <= 0;
    public void OnDamaged(int damage)
    {
      hp -= damage;
      if (hp < 0)
        hp = 0;
    }



  }
}
