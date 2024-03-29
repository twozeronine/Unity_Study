﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
  #region  동적 배열 구현
  //   class MyList<T>
  //   {
  //     const int DEFAULTSize = 1;
  //     T[] _data = new T[DEFAULTSize];

  //     public int Count; // 실제로 사용중인 데이터 개수
  //     public int Capacity { get => _data.Length; } // 예약된 데이터 개수

  //     // O(1) 예외 케이스 : 이사 비용은 무시한다 -> Count가 Capacity보다 클때만 연산하기 때문. O(N)이 아님.
  //     public void Add(T item)
  //     {
  //       // 1. 공간이 충분히 남아 있는지 확인한다
  //       if (Count >= Capacity)
  //       {
  //         // 공간을 다시 늘려서 확보한다
  //         T[] newArray = new T[Count * 2];
  //         for (int i = 0; i < Count; i++)
  //           newArray[i] = _data[i];
  //         _data = newArray;
  //       }

  //       // 2. 공간에다가 데이터를 넣어준다
  //       _data[Count] = item;
  //       Count++;
  //     }

  //     // O(1)
  //     public T this[int index]
  //     {
  //       get { return _data[index]; }
  //       set { _data[index] = value; }
  //     }

  //     // O(N)
  //     public void RemoveAt(int index)
  //     {
  //       // 101 102 103 104 105
  //       for (int i = index; i < Count - 1; i++)
  //         _data[i] = _data[i + 1];
  //       _data[Count - 1] = default(T);
  //       Count--;
  //     }
  //   }
  #endregion

  #region  연결 리스트 구현
  //   class MyLinkedListNode<T>
  //   {
  //     public T Data;
  //     public MyLinkedListNode<T> Next;
  //     public MyLinkedListNode<T> Prev;
  //   }

  //   class MyLinkedList<T>
  //   {
  //     public MyLinkedListNode<T> Head = null; // 첫번째
  //     public MyLinkedListNode<T> Tail = null; // 마지막
  //     public int Count = 0;

  //     // O(1)
  //     public MyLinkedListNode<T> AddLast(T data)
  //     {
  //       MyLinkedListNode<T> newRoom = new MyLinkedListNode<T>();
  //       newRoom.Data = data;

  //       //만약에 아직 방이 아예 없었다면, 새로 추가한 첫번째 방이 곧 Head이다.
  //       if (Head = null)
  //         Head = newRoom;

  //       // 기존에 [마지막 방]과 새로 추가되는 방을 연결해준다
  //       if (Tail != null)
  //       {
  //         Tail.Next = newRoom;
  //         newRoom.Prev = Tail;
  //       }

  //       // [새로 추가되는 방]을 [마지막 방]으로 인정한다
  //       Tail = newRoom;
  //       Count++;
  //       return newRoom;

  //     }

  //     // O(1)
  //     public void Remove(MyLinkedListNode<T> room)
  //     {
  //       // [기존의 첫번째 방 다음 방]을 [첫번째 방으로] 인정한다.
  //       if (Head == room)
  //         Head = Head.Next;

  //       // [기존의 마지막 방의 이전 방]을 [마지막 방으로] 인정한다.
  //       if (Tail == room)
  //         Tail = Tail.Prev;

  //       if (room.Prev != null)
  //         room.Prev.Next = room.Next;

  //       if (room.Next != null)
  //         room.Next.Prev = room.Prev;

  //       Count--;
  //     }

  //   }
  #endregion

  class Board
  {
    const char CIRCLE = '\u25cf';
    public TileType[,] Tile { get; private set; }// 배열
    public int Size { get; private set; }

    // 목적지
    public int DestY { get; private set; }
    public int DestX { get; private set; }

    Player _player;

    Random rand = new Random();

    public enum TileType
    {
      Empty,
      Wall,
    }
    public void Initialize(int size, Player player)
    {
      if (size % 2 == 0)
        return;

      _player = player;

      Tile = new TileType[size, size];
      Size = size;

      DestY = Size - 2;
      DestX = Size - 2;

      // Mazes for Programmers

      GenerateMap();
      // GenerateByBinaryTree();
      GenerateBySideWinder();
    }

    private void GenerateMap()
    {
      // 일단 길을 다 막아버리는 작업
      for (int y = 0; y < Size; y++)
      {
        for (int x = 0; x < Size; x++)
        {
          if (x % 2 == 0 || y % 2 == 0)
            Tile[y, x] = TileType.Wall;
          else
            Tile[y, x] = TileType.Empty;
        }
      }
    }

    private void GenerateByBinaryTree()
    {
      // 랜덤으로 우측 혹은 아래로 길을 뚫는 작업
      // Binary Tree Algorithm
      for (int y = 0; y < Size; y++)
      {
        for (int x = 0; x < Size; x++)
        {
          if (x % 2 == 0 || y % 2 == 0)
            continue;

          if (y == Size - 2 && x == Size - 2)
            continue;

          if (y == Size - 2)
          {
            Tile[y, x + 1] = TileType.Empty;
            continue;
          }

          if (x == Size - 2)
          {
            Tile[y + 1, x] = TileType.Empty;
            continue;
          }

          if (rand.Next(0, 2) == 0)
          {
            Tile[y, x + 1] = TileType.Empty;
          }
          else
            Tile[y + 1, x] = TileType.Empty;
        }
      }
    }


    private void GenerateBySideWinder()
    {
      // 랜덤으로 우측 혹은 아래로 길을 뚫는 작업
      // Side Winder Algorithm

      for (int y = 0; y < Size; y++)
      {
        int count = 1;
        for (int x = 0; x < Size; x++)
        {
          if (x % 2 == 0 || y % 2 == 0)
            continue;

          if (y == Size - 2 && x == Size - 2)
            continue;

          if (y == Size - 2)
          {
            Tile[y, x + 1] = TileType.Empty;
            continue;
          }

          if (x == Size - 2)
          {
            Tile[y + 1, x] = TileType.Empty;
            continue;
          }
          if (rand.Next(0, 2) == 0)
          {
            Tile[y, x + 1] = TileType.Empty;
            count++;
          }
          else
          {
            int randomIndex = rand.Next(0, count);
            Tile[y + 1, x - randomIndex * 2] = TileType.Empty;
            count = 1;
          }
        }
      }
    }

    public void Render()
    {
      ConsoleColor prevColor = Console.ForegroundColor;
      for (int y = 0; y < 25; y++)
      {
        for (int x = 0; x < 25; x++)
        {
          // 플레이어 좌표를 갖고 와서, 그 좌표랑 현재 y, x가 일치하면 플레이어 전용 색상으로 표시.
          if (y == _player.PosY && x == _player.PosX)
            Console.ForegroundColor = ConsoleColor.Blue;
          else if (y == DestY && x == DestX)
            Console.ForegroundColor = ConsoleColor.Yellow;
          else
            Console.ForegroundColor = GetTileColor(Tile[y, x]);

          Console.Write(CIRCLE);
        }
        Console.WriteLine();
      }
      Console.ForegroundColor = prevColor;
    }

    ConsoleColor GetTileColor(TileType type)
    {
      return type switch
      {
        TileType.Empty => ConsoleColor.Green,
        TileType.Wall => ConsoleColor.Red,
        _ => ConsoleColor.Green,
      };
    }
  }
}
