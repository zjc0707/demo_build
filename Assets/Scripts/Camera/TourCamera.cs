using UnityEngine;

public class TourCamera : MonoBehaviour
{
    //    输入控制： 
    //使用 鼠标右键 控制视角旋转。
    // 在场景中游览的相机（不要给相机加碰撞器！）
    public Transform tourCamera;
    public float moveSpeed = 0.1f;
    public float rotateSpeed = 60.0f;
    public float shiftRate = 2.0f;// 按住Shift加速
    void Start()
    {
        if (tourCamera == null) tourCamera = gameObject.transform;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) moveSpeed *= shiftRate;
        if (Input.GetKeyUp(KeyCode.LeftShift)) moveSpeed /= shiftRate;
        if (Input.GetMouseButton(0))
        {
            MoveCamera(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), moveSpeed);
        }
        if (Input.GetMouseButton(1))
        {
            // 转相机朝向
            tourCamera.RotateAround(tourCamera.position, Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);
            tourCamera.Rotate(tourCamera.right, -Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime);
        }
    }
    private void MoveCamera(float _MouseX, float _MouseY, float _Speed)     //移动相机
    {
        tourCamera.Translate(tourCamera.right * -_MouseX * _Speed);
        tourCamera.Translate(tourCamera.forward * _MouseY * _Speed);
    }
}
//--------------------- 
//作者：VDer
//来源：CSDN
//原文：https://blog.csdn.net/qq_21397217/article/details/78228801 
//版权声明：本文为博主原创文章，转载请附上博文链接！