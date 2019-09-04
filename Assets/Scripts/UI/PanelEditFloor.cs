using UnityEngine;
using UnityEngine.UI;
public class PanelEditFloor : BasePanel<PanelEditFloor>
{
    public InputField length;
    public InputField width;
    public Button submit, quit;
    protected override void _Start()
    {
        length.text = "" + Floor.current.x;
        width.text = "" + Floor.current.z;
        quit.onClick.AddListener(base.Close);
        submit.onClick.AddListener(EditFloor);
    }
    private void EditFloor()
    {
        Floor.current.x = int.Parse(length.text);
        Floor.current.z = int.Parse(width.text);
        Floor.current.Load();
        base.Close();
    }
}