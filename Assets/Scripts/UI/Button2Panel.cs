using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 绑定在Button上，关联Panel进行开关
/// </summary>
public class Button2Panel : MonoBehaviour {
    
    public GameObject panel;
    
	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(delegate
        {
            panel.SetActive(!panel.activeInHierarchy);
        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
