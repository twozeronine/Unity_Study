## file format

데이터를 저장하는 포맷은 주로 2가지가 있다.

하나는 xml, 하나는 json이다.

가독성은 json이 더 높지만 계층구조를 파악하기엔 XML이 더 좋다.

## Data 불러오기

json으로 저장된 데이터를 불러올때 유니티에서 자체 지원하는 JsonUtility를 사용할 수 있다.

JsonUtility를 사용하여 데이터를 받아올때는 해당 데이터의 형식과 맞는 class를 구현해줘야한다.

```json
{
  "stats": [
    {
      "level": "1",
      "hp": "100",
      "attack": "10"
    },
    {
      "level": "2",
      "hp": "150",
      "attack": "15"
    },
    {
      "level": "3",
      "hp": "200",
      "attack": "20"
    }
  ]
}
```

이런 형식의 데이터라면 C#에서 이런식의 class를 구현해야한다. 그리고 변수의 이름은 데이터 필드의 이름과 같아야만 파싱이된다.

```C#
[Serializable]
public class Stat
{
	public int level; // ID
	public int hp;
	public int attack;
}
```
