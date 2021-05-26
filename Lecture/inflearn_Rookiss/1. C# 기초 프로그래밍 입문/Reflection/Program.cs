using System;
using System.Reflection;

namespace Reflection
{
  class Program
  {
    class Important : System.Attribute
    {
      string message;
      public Important(string message) => this.message = message;
    }
    class Monster
    {
      //hp입니다. 아주 중요한 정보입니다.
      [Important("Very Important")]
      public int hp = 0;
      protected int attack = 0;
      private float speed = 0;

      void Attack() { }
    }
    static void Main(string[] args)
    {
      // Reflection : X - Ray
      Monster monster = new Monster();
      Type type = monster.GetType();

      var fields = type.GetFields(BindingFlags.Public
                    | BindingFlags.NonPublic
                    | BindingFlags.Static
                    | BindingFlags.Instance);

      foreach (FieldInfo field in fields)
      {
        string access = "protected";
        if (field.IsPublic)
          access = "public";
        else if (field.IsPrivate)
          access = "private";

        var attributes = field.GetCustomAttributes();

        Console.WriteLine($"{access} {field.FieldType.Name} {field.Name}");
      }
    }
  }
}
