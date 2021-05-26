using System;

namespace TextRPG2
{
  class Program
  {
    static void Main(string[] args)
    {
      Player player = new Knight();
      Player player2 = new Archer();
      Monster monster = new Orc();

      int damage = player.GetAttack();
      Console.WriteLine(monster.GetHp());
      monster.OnDamaged(damage);
      Console.WriteLine(monster.GetHp());
      player2.OnDamaged(player);

    }
  }
}
