using UnityEngine;

public class TourCamera : MonoBehaviour
{
    //    输入控制： 
    //使用 鼠标右键 控制视角旋转。
    // 在场景中游览的相机（不要给相机加碰撞器！）
    public Transform tourCamera;
    public float moveSpeed = 0.6f;
    public float rotateSpeed = 180.0f;
    public float scaleSpeed = 2f;
    public float shiftRate = 2.0f;// 按住Shift加速
    void Start()
    {
        if (tourCamera == null) tourCamera = gameObject.transform;
    }
    void Update()
    {
        float y = transform.position.y / 10;
        moveSpeed = y > 1 ? 0.6f * y : 0.6f;
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
            tourCamera.RotateAround(tourCamera.position, tourCamera.right, -Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime);
        }
        tourCamera.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") * scaleSpeed, Space.Self);
    }
    private void MoveCamera(float _MouseX, float _MouseY, float _Speed)     //移动相机
    {
        tourCamera.position += tourCamera.right * -_MouseX * _Speed;
        tourCamera.position += tourCamera.up * -_MouseY * _Speed;
        // Debug.Log("up:" + tourCamera.up + "_right" + tourCamera.right);
    }

}
//--------------------- 
//作者：VDer
//来源：CSDN
//原文：https://blog.csdn.net/qq_21397217/article/details/78228801 
//版权声明：本文为博主原创文章，转载请附上博文链接！