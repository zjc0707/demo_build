using System;
using UnityEngine;
public class AnimData
{
    public string Name { get; set; }
    public TransformGroup Begin { get; set; }
    public TransformGroup End { get; set; }
    public float Duration { get; set; }
    /// <summary>
    /// true：相对动画，需算上target目前的状态，false：绝对动画，直接使用begin和end的数据
    /// </summary>
    /// <value></value>
    public bool IsRelative { get; set; }
    public Action<float> Lerp(Transform target, TransformGroup tg = null)
    {
        if (IsRelative)
        {
            Vector3 pos = tg != null ? tg.Position : target.localPosition,
                euler = tg != null ? tg.EulerAngles : target.localEulerAngles,
                scale = tg != null ? tg.Scale : target.localScale;
            return f =>
            {
                target.localPosition = Vector3.Lerp(pos + Begin.Position, pos + End.Position, f);
                target.localEulerAngles = Vector3.Lerp(euler + Begin.EulerAngles, euler + End.EulerAngles, f);
                target.localScale = Vector3.Lerp(scale + Begin.Scale, scale + End.Scale, f);
            };
        }
        else
        {
            return f =>
            {
                target.localPosition = Vector3.Lerp(Begin.Position, End.Position, f);
                target.localEulerAngles = Vector3.Lerp(Begin.EulerAngles, End.EulerAngles, f);
                target.localScale = Vector3.Lerp(Begin.Scale, End.Scale, f);
            };
        }
    }
    public AnimData Clone()
    {
        return new AnimData()
        {
            Name = this.Name,
            Begin = this.Begin.Clone(),
            End = this.End.Clone(),
            Duration = this.Duration,
            IsRelative = this.IsRelative
        };
    }
}