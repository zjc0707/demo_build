using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : BaseUniqueObject<Floor>
{
    public int x = 10;
    public int z = 11;
    private void Awake()
    {
        Load();
    }
    public void Load()
    {
        this.transform.localScale = new Vector3(x, z, 1);
        this.transform.localPosition = new Vector3(x / 2f, 0, z / 2f);
        FloorTile.current.Load();
    }
    public void Load(int x, int z)
    {
        if (x == this.x && z == this.z)
        {
            return;
        }
        this.x = x;
        this.z = z;
        this.Load();
    }
    public void Reset()
    {
        Load(10, 11);
    }

}
