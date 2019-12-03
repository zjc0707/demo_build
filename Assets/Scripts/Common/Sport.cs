using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sport : BaseUniqueObject<Sport>
{
    private readonly int INIT = 0;
    private readonly int OPERATE = 1;
    private readonly int TURN_TO_INIT = 2;
    private readonly int TURN_TO_OPERATE = 3;
    private int state;
    public new Camera camera;
    /// <summary>
    /// 相机初始状态
    /// </summary>
    private TransformGroup cameraStart;
    /// <summary>
    /// 相机移动的目标位置，根据目标物体计算
    /// </summary>
    private TransformGroup cameraEnd;
    private TimeGroup cameraTimeGroup;
    private Building target;
    // Start is called before the first frame update
    void Start()
    {
        //MyCamera的Reset()调用在awake中，所以这里得到的是定位后的坐标
        // cameraStart = new TransformGroup(camera.transform.position, camera.transform.eulerAngles);
        // cameraEnd = new TransformGroup(11, 2, 5, 0, -90, 0);
        cameraTimeGroup = new TimeGroup(0.5f);
        state = INIT;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == OPERATE)
        {
            target.MyUpdate();
        }
        if (state == TURN_TO_OPERATE)
        {
            this.ChangeCamera(Time.deltaTime);
        }
        if (state == TURN_TO_INIT)
        {
            this.ChangeCamera(-1 * Time.deltaTime);
        }
    }

    public void TurnToOperate(Building building)
    {
        ChangeUI(true);
        target = building;
        cameraStart = new TransformGroup(camera.transform.position, camera.transform.eulerAngles);
        //根据target计算相机移动后的位置
        UpdateCameraEnd(building);

        state = TURN_TO_OPERATE;
    }

    public void TurnToInit()
    {
        ChangeUI(false);

        state = TURN_TO_INIT;
    }

    private void ChangeUI(bool isOperate)
    {
        UGUITree.current.canvasOperate.gameObject.SetActive(isOperate);
        UGUITree.current.canvasInit.gameObject.SetActive(!isOperate);
        PanelHelp.current.ChangeText(isOperate);
    }
    private void UpdateCameraEnd(Building building)
    {
        Transform t = building.transform;

        Vector3 euler = t.eulerAngles;
        t.Rotate(transform.up, -90f);
        Vector3 endEuler = t.eulerAngles;
        t.eulerAngles = euler;

        Vector3 endPos = t.position + t.right * building.Size.z;

        cameraEnd = new TransformGroup(endPos, endEuler);
    }
    /// <summary>
    /// 在状态切换的过程中改变相机位置，timeGroup.rate==1则进入operate状态，==0进入init状态
    /// </summary>
    /// <param name="deltaTime"></param>
    private void ChangeCamera(float deltaTime)
    {
        // if (!cameraTimeGroup.Add(deltaTime))
        // {
        //     if (cameraTimeGroup.rate == 1)
        //     {
        //         state = OPERATE;
        //     }
        //     if (cameraTimeGroup.rate == 0)
        //     {
        //         state = INIT;
        //     }
        //     // return;
        // }
        // this.camera.transform.position = Vector3.Lerp(cameraStart.position, cameraEnd.position, cameraTimeGroup.rate);
        // this.camera.transform.eulerAngles = Vector3.Lerp(cameraStart.eulerAngles, cameraEnd.eulerAngles, cameraTimeGroup.rate);

        LerpUtil.Anim(this.camera.transform, cameraStart, cameraEnd, cameraTimeGroup, deltaTime, delegate
        {
            if (cameraTimeGroup.rate == 1)
            {
                state = OPERATE;
            }
            if (cameraTimeGroup.rate == 0)
            {
                state = INIT;
            }
        });
    }


}
