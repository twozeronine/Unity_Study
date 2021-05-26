using System;

namespace TextRPG2
{

  public enum PlayerType
  {
    None = 0,
    Knight,
    Archer,
    Mage
  }

  class Player
  {
    protected PlayerType _type = PlayerType.None;
    protected int hp = 0;
    protected int attack = 0;

    protected Player(PlayerType type) => _type = type;

    public void SetInfo(int hp, int attack)
    {
      this.hp = hp;
      this.attack = attack;
    }

    public int GetHp() => hp;
    public int GetAttack() => attack;

  }

  class Knight : Player
  {
    public Knight() : base(PlayerType.Knight)
    {
      SetInfo(100, 10);
    }
  }
  class Archer : Player
  {
    public Archer() : base(PlayerType.Archer)
    {
      SetInfo(75, 12);
    }
  }
  class Mage : Player
  {
    public Mage() : base(PlayerType.Mage)
    {
      SetInfo(50, 13);
    }
  }
}
