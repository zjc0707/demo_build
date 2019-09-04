using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : BaseUniqueObject<MyCamera>
{

    public readonly float y = 10;
    public TransformGroup initTransformGroup = new TransformGroup(new Vector3(0, 5, 0),
                                                                    new Vector3(45, 45, 0));
    private bool isAnim = false;
    private TimeGroup timeGroup = new TimeGroup(0.5f);
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isAnim)
        {
            LerpUtil.Anim(this.transform, base.transformGroup, initTransformGroup, timeGroup, Time.deltaTime, delegate
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
        //this.transform.position = Floor.current.transform.position + new Vector3(0, this.y, 0);
        // this.transform.position = initTransformGroup.position;
        // this.transform.eulerAngles = initTransformGroup.eulerAngles;
        timeGroup.UseToZero();
        isAnim = true;
    }
}
