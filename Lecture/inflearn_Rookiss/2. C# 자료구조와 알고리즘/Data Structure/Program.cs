using System;

namespace Data_Structure
{
  class Program
  {
    class Graph
    {
      //그래프를 표시하는 첫번째 방법 행렬 만들기
      int[,] adj = new int[6, 6]
      {
          {0,1,0,1,0,0},
          {1,0,1,1,0,0},
          {0,1,0,0,0,0},
          {1,1,0,0,1,0},
          {0,0,0,1,0,1},
          {0,0,0,0,1,0},
      };

      //리스트로 만들기

      List<int>[] adj2 = new List<int>[]
      {
        new List<int>() {1,3 },
        new List<int>() {0,2,3 },
        new List<int>() {1 },
        new List<int>() {0,1,4 },
        new List<int>() {3,5},
        new List<int>() {4 },
      };
    }
    static void Main(string[] args)
    {
      // DFS (Depth First Search 깊이 우선 탐색 )

      // BFS (Breadth First Search 너비 우선 탐색 )
    }
  }
}
