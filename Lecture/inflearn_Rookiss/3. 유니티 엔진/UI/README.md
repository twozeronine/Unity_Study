## Rect Transform

Rect Transform은 부모 UI의 크기가 변할 때의 그 자식 UI 의 크기는 어떻게 변할 것 인지에 대한 처리를 하는 Trasnform이다.

부모 UI 의 크기가 변하면 자식 UI의 크기와 비율도 융통성 있게 바뀌어야 하는데, 특히 모바일 환경에서는 모바일 기종마다 화면 스크린 크기가 모두 다 다르기 때문에 UI 캔버스 크기도 달라질 수 있다. 그렇기 때문에 UI들의 크기와 비율도 어떻게 달라져야 할지 고려 해야한다.

### 앵커

앵커는 FPS 조준선과 같은 모양으로 생겼는데 이 아이콘을 옮겨서 부모와 자식간의 크기와 비율 관계를 조절할 수 있다.

부모 UI <--> 앵커 <--> 자식 UI

- 부모 UI와 앵커의 사이 거리는 비율로 유지된다.
- 앵커와 자식 UI 사이 거리는 고정 거리 ( 웹에서의 padding과 같은 개념)으로 유지된다.

따라서 3가지의 유형이 존재하는데

1. 부모 UI <- 거리 0 -> 앵커 <--> 자식 UI인 경우

![유형1](https://user-images.githubusercontent.com/67315288/120322669-a1bf3f80-c31f-11eb-977d-698c6b0febc0.png)

이 경우에는 비율로 유지되는 부분이 없기 때문에 부모 UI의 크기가 줄어들면 그 크기만큼 절댓값으로 똑같이 자식 UI 크기가 줄어드게 된다. ( 부모 UI 오른쪽 크기가 5 줄었다면 자식 UI 오른쪽에서도 똑같이 5가 줄어든다.)

2. 앵커가 중앙에 위치한 경우

![유형2](https://user-images.githubusercontent.com/67315288/120322671-a2f06c80-c31f-11eb-91d3-58e681ca92dc.png)

이 경우에는 고정 거리 즉 Padding이 없기 때문에 부모의 UI크기가 줄어들어도 자식 UI의 크기는 줄어들지 않는다.

3. 앵커가 자식 UI 크기와 같은 경우

![유형3](https://user-images.githubusercontent.com/67315288/120322673-a2f06c80-c31f-11eb-831f-3e7a20060b01.png)

이 경우에는 비율을 유지한채로 변경되므로 부모 UI 크기 변동이 많고 잦다면 제일 좋은 방법이다.

## UI 관리

### UI 입력 이벤트만 처리

롤이나 디아블로 탑뷰 RPG 게임 같은 경우에서 지면을 클릭하는것과 UI를 클릭하는것은 구분이 가야한다. 해당 구현은 마우스 입력을 받는 코드에서 UI를 클릭 할때는 마우스 입력을 받지 않도록 하게 구현해야한다.

```C#
// UI가 클릭된 상황이라면  return
    if (EventSystem.current.IsPointerOverGameObject())
      return;
```

### UI 바인딩 자동화

게임에서 UI는 수백개에 이를 정도로 많아지는데 이를 관리하기 위해서는 구조적으로 잘 짜임새있게 짜야한다. 만약 유니티 툴을 이용하여 드래그 & 드롭 형식으로 UI를 이곳저곳 매핑 하다보면 어느 순간 스파게티 코드처럼 꼬여버릴 위험이 있기 때문에 이를 관리하기 위해 바인딩 자동화를 한다.

UI_Base.cs : 모든 UI의 조상, 모든 UI 캔버스들이 가지고 있는 공통적인 부분들.

### UI 이벤트 처리

UI로 인벤토리나 여러가지 기능들을 구현할 때 이벤트가 발생되는데 여러가지 UI 이벤트들을 UI마다 하나하나 개별로 처리를 한다면 복잡도가 높혀지기 때문에 처리하기 쉽게끔 코드를 구조적으로 만들어야 한다.

UI_EventHandler.cs : 옵저버 패턴을 이용하여 해당 컴포넌트를 갖고있는 오브젝트에서 이벤트 발생 시 구독된 메소드들을 동작하게 한다.

```C#

// Unity에서 제공하는 Interface인 IDragHandler, IPointClickHandler를 상속받고 기능을 구현하면 드래그와 클릭을 할 수 있게된다.
public class UI_EventHandler : MonoBehaviour, IDragHandler, IPointerClickHandler
{
  public Action<PointerEventData> OnClickHandler = null;
  public Action<PointerEventData> OnDragHandler = null;

  public void OnPointerClick(PointerEventData eventData)
  {
    if (OnClickHandler != null)
      OnClickHandler.Invoke(eventData);
  }
  public void OnDrag(PointerEventData eventData)
  {
    if (OnDragHandler != null)
      OnDragHandler.Invoke(eventData);
  }
}

```
