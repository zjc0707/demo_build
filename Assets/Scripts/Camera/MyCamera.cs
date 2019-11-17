using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : BaseUniqueObject<MyCamera>
{

    public readonly float y = 10;
    public readonly TransformGroup initTransformGroup = new TransformGroup(new Vector3(0, 5, 0), new Vector3(45, 45, 0));
    private TransformGroup aim;
    private bool isAnim = false;
    private TimeGroup timeGroup = new TimeGroup(0.5f);
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isAnim && aim != null)
        {
            LerpUtil.Anim(this.transform, base.transformGroup, aim, timeGroup, Time.deltaTime, delegate
            {
                if (timeGroup.rate == 1)
                {
                    isAnim = false;
                }
            });
        }
    }
    public new void Reset()
    {
        MoveAnim(initTransformGroup);
    }
    public void MoveAnim(TransformGroup t)
    {
        aim = t;
        timeGroup.UseToZero();
        isAnim = true;
    }
}
