using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
  static Managers s_instance; // 유일성이 보장된다
  public static Managers Instance { get { init(); return s_instance; } } // 유일한 매니저를 갖고온다

  void Start()
  {
    init();
  }

  static void init()
  {
    // 인스턴스가 없을때만 초기화가 시작된다.
    // GameObject.Find 메소드의 부하가 크기 때문에 한번만 실행되도록 한다.
    if (s_instance == null)
    {
      // @Managers의 이름을 가진 매니저를 찾는다.
      GameObject go = GameObject.Find("@Managers");

      if (go == null)
      {
        // 매니저가 현재 씬에서 없기 때문에 @Managers라는 이름으로 빈 게임오브젝트를 만들고 컴포넌트를 추가한다.
        go = new GameObject { name = "@Managers" };
        go.AddComponent<Managers>();
      }

      // 중요한 오브젝트이기 때문에 유니티에서 씬이 로드되거나 할때 삭제되지 않도록 설정한다.
      DontDestroyOnLoad(go);
      // 마지막으로 static s_instace에 컴포넌트를 추가해준다.
      s_instance = go.GetComponent<Managers>();
    }
  }
}
