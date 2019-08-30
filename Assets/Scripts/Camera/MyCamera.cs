using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : BaseUniqueObject<MyCamera>
{

    public readonly float y = 10;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public new void Reset()
    {
        this.transform.position = Floor.current.transform.position + new Vector3(0, this.y, 0);
    }
}
