using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : BaseUniqueObject<FloorTile>
{
    private List<GameObject> objList = new List<GameObject>();
    public const float thickness = 0.2f;
    private float amountTime = 5f;
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
                target.gameObject.SetActive(true);
                objList.Add(target.gameObject);
                // PoolOfAnim.current.AddQueue(amountTime / (x * z), f =>
                // {
                //     if (f == 1)
                //     {
                //         target.gameObject.SetActive(true);
                //         PoolOfAnim.current.AddList(2f, ff =>
                //         {
                //             target.localPosition = Vector3.Lerp(vector3, vector3 + Vector3.up * 10, ff);
                //             if (ff == 1)
                //             {
                //                 PoolOfAnim.current.AddList(2f, fff =>
                //                 {
                //                     target.localPosition = Vector3.Lerp(vector3 + Vector3.up * 10, vector3, fff);
                //                 });
                //             }
                //         });
                //     }
                // });
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
