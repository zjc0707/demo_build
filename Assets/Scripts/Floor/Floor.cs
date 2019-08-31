using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : BaseUniqueObject<Floor>
{

    public float x = 10;
    public float z = 20;

    private void OnMouseOver()
    {

    }
    private void Awake()
    {
        Load();
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Load()
    {
        this.transform.localScale = new Vector3(x, z, 1);
        this.transform.localPosition = new Vector3(x / 2, 0, z / 2);

        MyCamera.current.Reset();
        //Person.current.Reset();
    }


}
