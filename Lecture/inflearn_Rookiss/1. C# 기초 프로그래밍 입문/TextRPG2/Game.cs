using System;

namespace TextRPG2
{
  public enum GameMode
  {
    None,
    Lobby,
    Town,
    Field
  }
  class Game
  {
    private GameMode mode = GameMode.Lobby;
    private Player player = null;
    private Monster monster = null;
    private Random rand = new Random();
    public void Process()
    {
      switch (mode)
      {
        case GameMode.Lobby:
          ProcessLobby();
          break;
        case GameMode.Town:
          ProcessTown();
          break;
        case GameMode.Field:
          ProcessField();
          break;
      }
    }

    public void ProcessLobby()
    {
      Console.WriteLine("직업을 선택하세요");
      Console.WriteLine("[1] 기사");
      Console.WriteLine("[2] 궁수");
      Console.WriteLine("[3] 법사");

      string input = Console.ReadLine();
      player = input switch
      {
        "1" => new Knight(),
        "2" => new Archer(),
        "3" => new Mage(),
        _ => null
      };
      mode = GameMode.Town;
    }

    public void ProcessTown()
    {
      Console.WriteLine("마을에 입장했습니다!");
      Console.WriteLine("[1] 필드로 가기");
      Console.WriteLine("[2] 로비로 돌아가기");

      string input = Console.ReadLine();
      mode = input switch
      {
        "1" => GameMode.Field,
        "2" => GameMode.Lobby,
        _ => GameMode.None
      };
    }

    public void ProcessField()
    {
      Console.WriteLine("필드에 입장했습니다!");
      Console.WriteLine("[1] 싸우기");
      Console.WriteLine("[2] 일정 확률로 마을 돌아가기");

      int randValue = rand.Next(0, 3);
      monster = randValue switch
      {
        0 => new Slime(),
        1 => new Orc(),
        2 => new Skeleton(),
        _ => null
      };
      var name = monster.GetType().Name;
      Console.WriteLine($"{name}이 생성 되었습니다.");

      string input = Console.ReadLine();
      switch (input)
      {
        case "1":
          ProcessFight();
          break;
        case "2":
          TryEscape();
          break;
      }
    }

    private void TryEscape()
    {
      int randValue = rand.Next(0, 101);
      if (randValue < 33)
      {
        mode = GameMode.Town;
      }
      else
      {
        ProcessFight();
      }
    }

    private void ProcessFight()
    {
      while (true)
      {
        int damage = player.GetAttack();
        monster.OnDamaged(damage);
        if (monster.isDead())
        {
          Console.WriteLine("승리했습니다");
          Console.WriteLine($"남은 체력{player.GetHp()}");
          break;
        }

        damage = monster.GetAttack();
        player.OnDamaged(damage);
        if (player.isDead())
        {
          Console.WriteLine("패배했습니다");
          break;
        }

      }
    }
  }

}
