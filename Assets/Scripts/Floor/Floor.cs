using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : BaseUniqueObject<Floor> {

    public float length = 10;
    public float width = 10;

    private void Awake()
    {
        Load();
        
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void Load()
    {
        this.transform.localScale = new Vector3(length, width, 1);
        this.transform.localPosition = new Vector3(length / 2, 0, width / 2);

        MyCamera.current.ResetByFloor(this);
        Person.current.ResetByFloor(this);
    }

    
}
