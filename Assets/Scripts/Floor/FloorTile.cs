using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : BaseUniqueObject<FloorTile>
{
    public float thickness = 0.2f;

    private void Start()
    {
        Load();
    }

    private void Load()
    {
        Vector3 start = new Vector3(0.5f, 0, 0.5f);
        for (int i = 0; i < Floor.current.width; i++)
        {
            for (int j = 0; j < Floor.current.length; j++)
            {
                Transform cube = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                Vector3 vector3 = start + Vector3.right * i + Vector3.forward * j;
                cube.name = string.Format("({0},{1})", i, j);
                cube.parent = this.transform;
                cube.localPosition = vector3;
                cube.localScale = new Vector3(1, thickness, 1);
                cube.GetComponent<Renderer>().sharedMaterial = MaterialStatic.GREEN;
            }
        }
    }
}
