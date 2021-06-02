## Sound Manager

MP3 Player, 소리를 재생하는곳 -> Audio Source 컴포넌트
MP3 음원, 음원 -> Audio Clip
귀 -> Audio Listner 컴포넌트

Audio Source 컴포넌트에는 많은 설정들이 들어있다.

- Volume : 소리크기
- Pitch : 재생 속도 ( 느리고 빠르게 )
- Mute : 음소거
- Loop : 반복 재생

```C#
//사운드를 재생하는 방법중 몇가지 자주 쓰이는 방법.

// 소리를 한번만 재생시킴.
// 중복되어 실행하면 전에 있던 소리는 정지되지않고 중복되서 같이 출력된다.
// 중간에 소리를 제어할 수 없다.
audioSource.PlayOneShot(audioClip);

// audioClip을 직접 넣는 PlayOneShor 방식과는 다르게 이미 들어있는 Clip을 재생시킨다.
// 중간에 간섭하여 소리를 중단 시키는것도 가능하다.
audioSource.Play();

```

SoundManager를 사용하는 이유는

Audio Source 컴포넌트가 붙어있는 게임 오브젝트가 비활성화 혹은 파괴되면 재생하던 소리들까지 전부 중단되기 때문에 끊김 현상이 없게 하기 위해서는 오브젝트에 종속적인 아니면서 사운드를 재생할 어떤 오브젝트가 필요하다 그것이 바로 사운드 매니저이다.

```C#
public class SoundManager // 코드 중 일부
{

// 오디오 클립을 캐싱해 두기 위해서 Dictionary 추가.
AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

// Resources 폴더 경로에 접근하여 AudioClip을 가져온다.
AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
  {
    if (path.Contains("Sounds/") == false)
      path = $"Sounds/{path}";

    AudioClip audioClip = null;

    // 배경음악은 정말 어쩌다 한번 바뀌기 때문에 따로 캐싱하지않고 바로 불러온다.
    if (type == Define.Sound.Bgm)
    {
      audioClip = Managers.Resource.Load<AudioClip>(path);
    }

    // 이팩트는 정말 자주 바뀌기 때문에 캐싱을하여 사용한다.
    else
    {
      if (_audioClips.TryGetValue(path, out audioClip) == false)
      {
        audioClip = Managers.Resource.Load<AudioClip>(path);
        _audioClips.Add(path, audioClip);
      }
    }

    if (audioClip == null)
      Debug.Log("AudioClip Missing ! {path}");

    return audioClip;
  }
}
```

### 3D 사운드

AudioSource 컴포넌트에는 3D 사운드를 낼 수 있게끔 하는 설정이 있다. 설정을 이용하면 거리에 따른 소리의 출력을 다르게 하거나 얼마나 소리가 퍼질건지에 대한 설정도 가능하다.

구현하는 방법은 어떤 오브젝트에 AudioSource 컴포넌트를 붙여 직접 소리를 내게 하는 방법이 있다. 하지만 이와 같은 방법은 SoundManager를 만들었던 장점 ( 오브젝트가 파괴되거나 비활성화 되면 소리가 끊김 )을 잃게 되므로 다른 방법을 찾아야한다.

물론 유니티에서 그러한 기능을 제공하기 위해서 PlayClipAtPoint(Audio Clip , Vector3 ) 함수를 제공하지만 현재는 안쓰는 함수 같다.

> 내 생각이지만 나중에 만들 ObjectPool에서 AudioSource 컴포넌트를 붙인 사운드 재생용 빈 오브젝트를 만들어서 해당 사운드가 재생되어야 할 부모 오브젝트의 자식으로 넣어주어 따라다니게 하고, 그 부모 오브젝트가 비활성화 되거나 삭제될 경우 다시 자식인 빈 오브젝트를 풀로 넣어주면 되지 않을까? 생각해본다.

### Audio Listner

오디오를 듣는 직접적인 청자이다. 만약 FPS와 같은 장르의 게임에서 더욱 실감이 나게 소리를 듣게 하고 싶다면 Player에게 붙이면 된다 ( default로 Camera에 붙어있다.)
