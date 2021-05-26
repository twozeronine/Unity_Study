using System;

namespace Event
{
  class Program
  {
    static void OnInputTest() => Console.WriteLine("Input Received");
    static void Main(string[] args)
    {
      InputManager inputManager = new InputManager();

      //이벤트 구독
      inputManager.InputKey += OnInputTest;

      while (true)
      {
        inputManager.Update();

        // 사용 불가능
        //inputManager.InputKey();
      }
    }
  }
}
