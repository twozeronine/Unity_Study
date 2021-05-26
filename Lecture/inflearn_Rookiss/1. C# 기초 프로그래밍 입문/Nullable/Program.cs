using System;

namespace Nullable
{
  class Program
  {
    class Monster
    {
      public int Id { get; set; }
    }
    static void Main(string[] args)
    {
      // Nullable -> Null + able
      int? number = null;

      //number에 있는 값 뽑아오기 
      if (number != null)
      {
        int a = number.Value;
        Console.WriteLine(a);
      }

      if (number.HasValue)
      {
        int a = number.Value;
        Console.WriteLine(a);
      }

      //number에 있는 값 뽑아오기 새로운 문법
      int b = number ?? 0;
      Console.WriteLine(b);

      Monster monster = null;

      if (monster != null)
      {
        int monsterId = monster.Id;
      }

      // Nullable -> 형식 ?
      // null
      int? id = monster?.Id;

    }
  }
}
