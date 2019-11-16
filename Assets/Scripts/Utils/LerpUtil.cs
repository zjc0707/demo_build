using UnityEngine;
public class LerpUtil
{
    public delegate void CallBack();
    public static void Anim(Transform target, TransformGroup start, TransformGroup end, TimeGroup timeGroup, float deltaTime, CallBack endCall)
    {
        timeGroup.Add(deltaTime);
        float rate = timeGroup.rate;
        target.position = Vector3.Lerp(start.Position, end.Position, rate);
        target.eulerAngles = Vector3.Lerp(start.EulerAngles, end.EulerAngles, rate);
        if (endCall != null)
        {
            endCall();
        }
    }
}