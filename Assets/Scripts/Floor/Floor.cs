using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : BaseUniqueObject<Floor>
{

    public int x = 10;
    public int z = 11;

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

    public void Load()
    {
        this.transform.localScale = new Vector3(x, z, 1);
        this.transform.localPosition = new Vector3(x / 2f, 0, z / 2f);

        FloorTile.current.Load();

        MyCamera.current.Reset();
        //Person.current.Reset();
    }


}
