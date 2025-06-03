using System;
using System.Collections;
using UnityEngine;

public class Util_Coroutine
{
    public static IEnumerator Delay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public static IEnumerator ParabolaMove(Transform obj, Vector3 start, Vector3 end, float height, float duration, Action action = null)
    {
        float time = 0.0f;
        while(time < duration)
        {
            float t = time / duration;

            // 수평 보간
            Vector3 flatPos = Vector3.Lerp(start, end, t);

            // 위로 솟는 y값 (포물선)
            float y = Mathf.Sin(Mathf.PI * t) * height;

            obj.position = new Vector3(flatPos.x, flatPos.y + y, flatPos.z);

            time += Time.deltaTime;
            yield return null;
        }
        
        if(action != null)
            action?.Invoke();

        obj.position = end;
    }
}
