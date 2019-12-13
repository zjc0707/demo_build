using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : BaseUniqueObject<FloorTile>
{
    private List<GameObject> objList = new List<GameObject>();
    public const float thickness = 0.2f;
    private int index = 0;
    private bool isAnim = false;
    private float useTime = 0f;
    private float amountTime = 2f;
    public void Load(int x, int z)
    {
        Clear();

        Vector3 start = new Vector3(0.5f, 0, 0.5f);
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                Transform target = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                Vector3 vector3 = start + Vector3.right * i + Vector3.forward * j;
                target.name = string.Format("({0},{1})", i, j);
                target.parent = this.transform;
                target.localPosition = vector3;
                target.localScale = new Vector3(1, thickness, 1);
                target.GetComponent<Renderer>().sharedMaterial = ResourceStatic.GREY;
                target.gameObject.SetActive(false);
                objList.Add(target.gameObject);
            }
        }
        isAnim = true;
    }
    private void Update()
    {
        if (isAnim)
        {
            if (index >= objList.Count)
            {
                isAnim = false;
                return;
            }
            float onceTime = amountTime / objList.Count;
            useTime += Time.deltaTime;
            int a = (int)(useTime / onceTime);
            if (a > 1)
            {
                useTime = 0;
                while (a > 1)
                {
                    objList[index].SetActive(true);
                    index++;
                    if (index >= objList.Count)
                    {
                        isAnim = false;
                        return;
                    }
                    a--;
                }
            }
            else
            {

            }
        }
    }
    private void Clear()
    {
        foreach (GameObject obj in objList)
        {
            DestroyImmediate(obj);
        }
        objList.Clear();
    }
}
