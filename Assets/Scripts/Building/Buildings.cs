using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour {

    private static Buildings _instance;
    public static Buildings current
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Buildings>();
            }
            if(_instance == null)
            {
                _instance = GameObject.Find("Buildings").AddComponent<Buildings>();
            }
            return _instance;
        }
    }

    public void Build(Transform t)
    {
        t.SetParent(transform);
        t.localPosition = new Vector3(FixPos(t.localPosition.x), t.localPosition.y, FixPos(t.localPosition.z));
        t.localEulerAngles = new Vector3(FixEuler(t.localEulerAngles.x), FixEuler(t.localEulerAngles.y), FixEuler(t.localEulerAngles.z));
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
