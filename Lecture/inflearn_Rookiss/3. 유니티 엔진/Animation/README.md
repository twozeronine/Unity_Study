## 애니메이션 블렌딩

유니티에서는 언리얼 엔진과 비슷하게 애니메이션을 섞는 기능을 제공한다.

Blend Tree를 만들어 이 기능을 사용하면 애니메이션 동작을 더 부드럽게끔 실행 시킬 수 있다.

```C#
// field에 선언 해준 값 ( 애니메이션을 얼마나 섞을지 결정하는 수치이다. )
 float wait_run_ratio = 0;

Animator anim = GetComponent<Animator>();
    // 목적지에 도착 하였을때
    if (_moveToDest)
    {
      wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10.0f * Time.deltaTime);
      anim.SetFloat("wait_run_ratio", wait_run_ratio);
      anim.Play("WAIT_RUN");
    }
    else
    {
      wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10.0f * Time.deltaTime);
      anim.SetFloat("wait_run_ratio", wait_run_ratio);
      anim.Play("WAIT_RUN");
    }

```
