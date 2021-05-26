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

  class Player : Creature
  {
    protected PlayerType _type = PlayerType.None;
    protected Player(PlayerType type) : base(CreatureType.Player) => _type = type;

    public PlayerType GetPlayerType() => _type;
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
