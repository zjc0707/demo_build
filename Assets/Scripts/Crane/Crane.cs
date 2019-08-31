using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : Building
{
    /// <summary>
    /// 整个顶部组合
    /// </summary>
    public Transform head;
    /// <summary>
    /// 吊钩上方的绳子
    /// </summary>
    public Transform line;
    /// <summary>
    /// 吊钩
    /// </summary>
    public Transform hook;
    /// <summary>
    /// 吊钩和绳子的父类，在head旋转时同时旋转该父类对象，使之一直竖直向下
    /// </summary>
    public Transform lineAndHook;
    /// <summary>
    /// head旋转的参照点
    /// </summary>
    public Transform headRotationPoint;
    /// <summary>
    /// 吊钩和绳子旋转的参照点
    /// </summary>
    public Transform lineHookRotationPoint;
    /// <summary>
    /// 伸展部分1
    /// </summary>
    public Transform part1;
    /// <summary>
    /// head旋转速度，考虑到可行性及需要同时矫正吊钩+绳子，所以没有使用lerp的形式进行动画
    /// </summary>
    private float rotateHeadSpeed = 12f;
    /// <summary>
    /// part1伸展范围，通过position移动
    /// </summary>
    private Vector3 part1StartPos = Vector3.zero, part1EndPos = new Vector3(-2.52f, 0.17f, 0f);
    /// <summary>
    /// 绳子伸缩范围，scale.y放大
    /// </summary>
    /// <returns></returns>
    private Vector3 lineStartScale = Vector3.one, lineEndScale = new Vector3(1f, 3f, 1f);
    /// <summary>
    /// 绳子初始长度
    /// </summary>
    private float lineLenth;
    /// <summary>
    /// 吊钩和绳子的初始化时距离参照点的值，后续不再改变，用于计算 *AfterChange的值
    /// </summary>
    private Vector3 distanceLineAndHook;
    /// <summary>
    /// 因绳子伸缩后改变的值，用于各处位置调整
    /// </summary>
    private Vector3 distanceLineAndHookAfterChange;
    /// <summary>
    /// 吊钩初始化的局部坐标，用于绳子伸缩后调整
    /// </summary>
    private Vector3 hookStartLocalPos;
    private TimeGroup expandPart1TimeGroup = new TimeGroup(2f);
    private TimeGroup rotateHeadTimeGroup = new TimeGroup(2f);
    private TimeGroup expandLineTimeGroup = new TimeGroup(2f);
    // Start is called before the first frame update
    void Start()
    {
        base.DownToFloor();
        distanceLineAndHookAfterChange = distanceLineAndHook = lineHookRotationPoint.position - lineAndHook.position;
        lineLenth = CenterAndSizeUtil.Get(line).Size.y;
        hookStartLocalPos = hook.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //J,K,L分别操作三个部位，shift组合键为反方向
        float deltaTime = Input.GetKey(KeyCode.LeftShift) ? -1 * Time.deltaTime : Time.deltaTime;
        //吊车头部向外旋转
        if (Input.GetKey(KeyCode.J))
        {
            RotationHead(deltaTime);
        }
        //伸出
        if (Input.GetKey(KeyCode.K))
        {
            ExpandPart1(deltaTime);
        }
        //伸缩绳子
        if (Input.GetKey(KeyCode.L))
        {
            ExpandLine(deltaTime);
        }
    }

    public override void MyUpdate()
    {

    }
    /// <summary>
    /// 旋转吊车头部
    /// </summary>
    private void RotationHead(float deltaTime)
    {
        if (!rotateHeadTimeGroup.Add(deltaTime)) return;
        deltaTime *= rotateHeadSpeed;
        head.RotateAround(headRotationPoint.localPosition, head.forward, -1 * deltaTime);
        lineAndHook.RotateAround(lineHookRotationPoint.localPosition, head.forward, deltaTime);
        lineAndHook.position = lineHookRotationPoint.position - distanceLineAndHookAfterChange;
    }
    /// <summary>
    /// 伸展吊车头部 1 部分
    /// </summary>
    private void ExpandPart1(float deltaTime)
    {
        expandPart1TimeGroup.Add(deltaTime);
        part1.localPosition = Vector3.Lerp(part1StartPos, part1EndPos, expandPart1TimeGroup.rate);
    }
    /// <summary>
    /// 伸缩绳子，并调整绳钩父类位置（-1/2*生长的值），再调整钩子的位置（再-1/2*生长的值）
    /// </summary>
    private void ExpandLine(float deltaTime)
    {
        expandLineTimeGroup.Add(deltaTime);
        line.localScale = Vector3.Lerp(lineStartScale, lineEndScale, expandLineTimeGroup.rate);
        float changeScaleY = lineEndScale.y - lineStartScale.y;
        Vector3 distance = Vector3.up * lineLenth * changeScaleY * expandLineTimeGroup.rate;
        distanceLineAndHookAfterChange = distanceLineAndHook + distance / 2;
        lineAndHook.position = lineHookRotationPoint.position - distanceLineAndHookAfterChange;
        hook.localPosition = hookStartLocalPos - distance / 2;
    }

}