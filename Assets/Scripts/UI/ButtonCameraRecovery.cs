using UnityEngine;
using UnityEngine.UI;
public class ButtonCameraRecovery : BaseUniqueObject<ButtonCameraRecovery>
{
    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(delegate
        {
            MyCamera.current.Reset();
        });
    }
    private void Update()
    {
    }
}