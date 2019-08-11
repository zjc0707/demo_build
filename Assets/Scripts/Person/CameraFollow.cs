using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public new Transform camera;
    public Vector3 offset = new Vector3(0, 5, -4);
    public float speed = 2;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        
    }

    private void LateUpdate()
    {
        camera.position = transform.position;
        camera.rotation = transform.rotation;
        camera.position += (camera.right * offset.x + camera.up * offset.y + camera.forward * offset.z);
        camera.LookAt(transform.position);
    }
}
