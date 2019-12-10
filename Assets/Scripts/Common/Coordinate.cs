using System;
using UnityEngine;
public class Coordinate : BaseUniqueObject<Coordinate>
{
    private Item x, y, z;
    private const float multiple = 2f;
    private void Start()
    {
        x = FindItem("X");
        y = FindItem("Y");
        z = FindItem("Z");
        float posZ = z.line.GetComponent<MeshRenderer>().bounds.size.z;
        x.SetHeadPosZ(posZ);
        y.SetHeadPosZ(posZ);
        z.SetHeadPosZ(posZ);
    }
    public void ChangeSizeByDistanceToCamera(Transform c)
    {
        float distance = Math.Abs(Vector3.Distance(c.position, this.transform.position)) / multiple;
        x.SetScale(distance);
        y.SetScale(distance);
        z.SetScale(distance);
        this.transform.localScale = Vector3.one * distance;
    }
    private Item FindItem(string name)
    {
        Transform t = this.transform.Find(name);
        return new Item(t);
    }
    class Item
    {
        public Transform target { get; set; }
        public Transform line { get; set; }
        public Transform head { get; set; }
        private float defaultLineXY = 0.0005f;
        private float defaultLineZ = 0.01f;
        public Item(Transform t)
        {
            target = t;
            line = t.Find("Line");
            head = t.Find("Head");
        }
        public void SetHeadPosZ(float f)
        {
            head.localPosition = new Vector3(0, 0, f);
        }
        public void SetScale(float f)
        {
            line.localScale = new Vector3(defaultLineXY / f, defaultLineXY / f, defaultLineZ);

        }
    }
}