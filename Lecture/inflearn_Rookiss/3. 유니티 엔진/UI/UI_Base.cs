using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
  protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
  protected void Bind<T>(Type type) where T : UnityEngine.Object
  {
    // Enum 타입에서 이름을 string으로 얻어옴
    string[] names = Enum.GetNames(type);
    // Enum에서 선언한 갯수만큼 object배열을 만듬
    UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
    // 해당 배열을 딕셔너리에 넣음 나중에 Type을 통해서 배열 접근 가능.
    _objects.Add(typeof(T), objects);

    for (int i = 0; i < names.Length; i++)
    {
      if (typeof(T) == typeof(GameObject))
        objects[i] = Util.FindChild(gameObject, names[i], true);
      else
        objects[i] = Util.FindChild<T>(gameObject, names[i], true);

      if (objects[i] == null)
        Debug.Log($"Failed to bind({names[i]})");
    }
  }

  protected T Get<T>(int idx) where T : UnityEngine.Object
  {
    UnityEngine.Object[] objects = null;
    if (_objects.TryGetValue(typeof(T), out objects) == false)
      return null;

    return objects[idx] as T;
  }

  protected Text GetText(int idx) => Get<Text>(idx);
  protected Button GetButton(int idx) => Get<Button>(idx);
  protected Image GetImage(int idx) => Get<Image>(idx);

}

//보완할점 
// 1. UI가 아닌 오브젝트에서 해당 스크립트를 붙일경우 오류가 날수 있기 때문에 enum 형식으로 미리 만들어질 UI들을 생성해놓자.