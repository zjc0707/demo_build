using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 唯一对象，存放所有放置建筑的父类
/// </summary>
public class BuildingRoom : BaseUniqueObject<BuildingRoom>
{

    public void Build(Building building)
    {
        building.Recovery();
        
        Transform t = building.transform;
        t.SetParent(transform);
        //t.localPosition = new Vector3(FixPos(t.localPosition.x), t.localPosition.y, FixPos(t.localPosition.z));
        t.localEulerAngles = new Vector3(FixEuler(t.localEulerAngles.x), 
            FixEuler(t.localEulerAngles.y), 
            FixEuler(t.localEulerAngles.z));
        
        Vector3 afterFix = new Vector3(FixPos(building.LeftFrontBottom.x),
            FixPos(building.LeftFrontBottom.y),
            FixPos(building.LeftFrontBottom.z));
        Debug.Log(building.LeftFrontBottom + "_after:" + afterFix + "_pos:" + t.localPosition);
        t.localPosition += afterFix - building.LeftFrontBottom;
    }

    private int FixEuler(float f)
    {
        return ((int)(f + 45) / 90) * 90;
    }

    private int FixPos(float f)
    {
        if (f > 0)
            return (int)(f + 0.5);
        else
            return (int)(f - 0.5);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
