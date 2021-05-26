using System;

namespace Event
{

  // Observer Pattern
  class InputManager
  {
    public delegate void OnInputKey();
    public event OnInputKey InputKey;
    public void Update()
    {
      if (Console.KeyAvailable == false) return;

      ConsoleKeyInfo info = Console.ReadKey();
      if (info.Key == ConsoleKey.A)
      {
        // 모두한테 A라는 키가 입력되었다고 알려준다.
        InputKey();
      }
    }
  }
}