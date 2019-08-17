using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : BaseUniqueObject<MyCamera> {

    public float y = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResetByFloor(Floor floor)
    {
        this.transform.position = floor.transform.position + new Vector3(0, this.y, 0);
    }
}
