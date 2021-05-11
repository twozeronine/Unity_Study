using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
    // Base class for all /yield/ instructions.
    [StructLayout(LayoutKind.Sequential)]
    [UsedByNativeCode]
    public class YieldInstruction
    {
    }

    // 이 클래스를 상속하여 나만의 커스텀 코루틴을 만들 수 있다.
  // 요약:
  //     Base class for custom yield instructions to suspend coroutines.
  public abstract class CustomYieldInstruction : IEnumerator
  {
    protected CustomYieldInstruction();

    //
    // 요약:
    //     Indicates if coroutine should be kept suspended.
    public abstract bool keepWaiting { get; }
    public object Current { get; }

    public bool MoveNext() {return keepWaiting; };
    public virtual void Reset() {};
  }


  

  // UnityEngine에서 지원하는 yield return문에서 생성되는 IEnumerator 객체 내부 소스코드
  // 요약:
  //     Suspends the coroutine execution until the supplied delegate evaluates to true.
  public sealed class WaitUntil : CustomYieldInstruction
    {
        Func<bool> m_Predicate;

        public override bool keepWaiting { get { return !m_Predicate(); } }

        public WaitUntil(Func<bool> predicate) { m_Predicate = predicate; }
    }


    // Waits until the end of the frame after all cameras and GUI is rendered, just before displaying the frame on screen.
    [RequiredByNativeCode]
    public sealed class WaitForEndOfFrame : YieldInstruction
    {

    }

    public sealed class WaitWhile : CustomYieldInstruction
    {
        Func<bool> m_Predicate;

        public override bool keepWaiting { get { return m_Predicate(); } }

        public WaitWhile(Func<bool> predicate) { m_Predicate = predicate; }
    }

    // Suspends the coroutine execution for the given amount of seconds.
    [StructLayout(LayoutKind.Sequential)]
    [RequiredByNativeCode]
    public sealed class WaitForSeconds : YieldInstruction
    {
        internal float m_Seconds;

        // Creates a yield instruction to wait for a given number of seconds
        public WaitForSeconds(float seconds) { m_Seconds = seconds; }
    }

    public class WaitForSecondsRealtime : CustomYieldInstruction
    {
        public float waitTime { get; set; }
        float m_WaitUntilTime = -1;

        public override bool keepWaiting
        {
            get
            {
                if (m_WaitUntilTime < 0)
                {
                    m_WaitUntilTime = Time.realtimeSinceStartup + waitTime;
                }

                bool wait =  Time.realtimeSinceStartup < m_WaitUntilTime;
                if (!wait)
                {
                    // Reset so it can be reused.
                    Reset();
                }
                return wait;
            }
        }

        public WaitForSecondsRealtime(float time)
        {
            waitTime = time;
        }

        public override void Reset()
        {
            m_WaitUntilTime = -1;
        }
    }

    // Waits until next fixed frame rate update function. SA: MonoBehaviour::pref::FixedUpdate.
    [RequiredByNativeCode]
    public sealed class WaitForFixedUpdate : YieldInstruction
    {
    }
}





