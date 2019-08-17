using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 行走脚本
/// </summary>
public class MoveAndRotate : MonoBehaviour {

    public float speedMove = 3f;
    public float speedRotate = 40f;

    private KeyCode up, down, left, right;

    private void Awake()
    {
        WASD();
    }
    // Update is called once per frame
    void Update () {
        if (!State.current.IsPerson)
        {
            return;
        }
        if (Input.GetKey(up))
        {
            transform.position += transform.forward * speedMove * Time.deltaTime;
        }
        if (Input.GetKey(down))
        {
            transform.position -= transform.forward * speedMove * Time.deltaTime;
        }
        if (Input.GetKey(left))
        {
            transform.Rotate(transform.up, -1 * speedRotate * Time.deltaTime);
        }
        if (Input.GetKey(right))
        {
            transform.Rotate(transform.up, speedRotate * Time.deltaTime);
        }
        //按下方向键时的其他处理
        
	}

    //private void OnGUI()
    //{
    //    if (Input.anyKeyDown)
    //    {
    //        Event e = Event.current;
    //        if (e.isKey && IsCurrent(e.keyCode))
    //        {
    //            PanelControl.current.Close();
    //        }
    //    }
    //}

    private bool IsCurrent(KeyCode key)
    {
        return key == up || key == down || key == left || key == right;
    }

    private void WASD()
    {
        up = KeyCode.W;
        down = KeyCode.S;
        left = KeyCode.A;
        right = KeyCode.D;
    }

    private void Arrow()
    {
        up = KeyCode.UpArrow;
        down = KeyCode.DownArrow;
        left = KeyCode.LeftArrow;
        right = KeyCode.RightArrow;
    }
}
