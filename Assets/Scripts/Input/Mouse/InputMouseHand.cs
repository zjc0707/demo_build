using UnityEngine;
/// <summary>
/// 鼠标移动相机事件
/// 左键拖动相机
/// </summary>
public class InputMouseHand : BaseInputMouse
{
    public const float moveSpeed = 0.6f;
    protected override void OnMouseLeftClick()
    {
        MoveCamera(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), moveSpeed);
    }
    private void MoveCamera(float _MouseX, float _MouseY, float _Speed)     //移动相机
    {
        Transform tourCamera = MyCamera.current.transform;
        tourCamera.position += tourCamera.right * -_MouseX * _Speed;
        tourCamera.position += tourCamera.up * -_MouseY * _Speed;
    }
}