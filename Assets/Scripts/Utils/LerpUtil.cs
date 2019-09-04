using UnityEngine;
public class LerpUtil
{
    public delegate void CallBack();
    public static void Anim(Transform target, TransformGroup start, TransformGroup end, TimeGroup timeGroup, float deltaTime, CallBack endCall)
    {
        timeGroup.Add(deltaTime);
        float rate = timeGroup.rate;
        target.position = Vector3.Lerp(start.position, end.position, rate);
        target.eulerAngles = Vector3.Lerp(start.eulerAngles, end.eulerAngles, rate);
        if (endCall != null)
        {
            endCall();
        }
    }
}