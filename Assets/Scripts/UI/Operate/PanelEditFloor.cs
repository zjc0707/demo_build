using UnityEngine;
using UnityEngine.UI;
[UIType(UIStackType.OPERATE)]
public class PanelEditFloor : BasePanel<PanelEditFloor>
{
    public InputField length;
    public InputField width;
    public Button submit, quit;
    protected override void _Start()
    {
        quit.onClick.AddListener(base.Close);
        submit.onClick.AddListener(EditFloor);
    }
    public override void Open()
    {
        length.text = "" + Floor.current.x;
        width.text = "" + Floor.current.z;
        base.Open();
    }
    private void EditFloor()
    {
        int x = int.Parse(length.text);
        int z = int.Parse(width.text);

        if (x < Floor.current.x || z < Floor.current.z)
        {
            PanelDialog.current.Open("场景将缩小，超出边界物体将自动调整，是否继续", () => Edit(x, z));
        }
        else
        {
            Edit(x, z);
        }
    }

    private void Edit(int x, int z)
    {
        Floor.current.Load(x, z);
        base.Close();
    }
}