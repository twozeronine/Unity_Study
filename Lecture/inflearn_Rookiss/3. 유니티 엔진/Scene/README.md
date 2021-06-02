## 게임 구조

게임을 처음 시작 했을때 플레이어를 배치하거나 몬스터를 배치하고 맵을 배치하는등의 작업을 해야하는데 그런 작업을 씬이 관리하여 게임을 진행 하도록 하자.

BaseScene이라는 추상클래스를 만들어서 모든 씬들이 상속받도록 한다.

```C#
public abstract class BaseScene : MonoBehaviour
{
  public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

  // Start대신 Awake를 쓰는 이유는 Awake는 Start보다 먼저 실행되며, 오브젝트만 활성화 되어 있다면 실행된다 (컴포넌트가 생성될 때 실행되고 컴포넌트가 비활성화되어도 실행되는 부분이다.)
  void Awake()
  {
    Init();
  }

  protected virtual void Init()
  {
    // EventSystem을 추가한다.
    // 씬을 전환할때 EventSystem이 없이 전환이 되게 되는데 거의 모든 게임은 UI가 있기 때문에
    // 되도록이면 EventSystem을 만들도록 하는것이다.
    Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
    if (obj == null)
    // Resources 폴더에 있는 EventSystem 프리팹을 불러와 생성한다.
      Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
  }

  // 씬이 종료됐을때 날려줘야 하는 부분
  public abstract void Clear();
}

```

```C#

public class GameScene : BaseScene
{
  // Init 호출 부분이 없어도 BaseScene에 Awake가 실행되면서 Init을 실행하게 되는데
  // 가상함수의 특성상 가상 함수 테이블을 참조해서 override 된 함수가 있으면 그버전으로 먼저 호출해준다.
  // 따라서 GameScene의 Init이 호출된다.

  protected override void Init()
  {
    base.Init();
    SceneType = Define.Scene.Game;


    //GameScene에서 UI가 필요하므로 UI를 생성한다.
    Managers.UI.ShowSceneUI<UI_Inven>();
  }
  public override void Clear()
  {

  }
}

```
