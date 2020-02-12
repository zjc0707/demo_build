using System;
using UnityEngine;
public class AnimData
{
    public string Name { get; set; }
    public TransformGroup Begin { get; set; }
    public TransformGroup End { get; set; }
    public float Duration { get; set; }
    public void Lerp(Transform target, float f)
    {
        target.localPosition = Vector3.Lerp(Begin.Position, End.Position, f);
        target.localEulerAngles = Vector3.Lerp(Begin.EulerAngles, End.EulerAngles, f);
        target.localScale = Vector3.Lerp(Begin.Scale, End.Scale, f);
    }
    public AnimData Clone()
    {
        return new AnimData()
        {
            Name = this.Name,
            Begin = this.Begin.Clone(),
            End = this.End.Clone(),
            Duration = this.Duration
        };
    }
}