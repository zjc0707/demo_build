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
        int x = int.Parse(length.text);
        int z = int.Parse(width.text);

        if (x < Floor.current.x || z < Floor.current.z)
        {
            PanelDialog.current.Open("场景将缩小，超出边界物体将自动调整，是否继续", delegate
            {
                Edit(x, z);
            });
        }
        else
        {
            Edit(x, z);
        }
    }

    private void Edit(int x, int z)
    {
        Floor.current.x = x;
        Floor.current.z = z;
        Floor.current.Load();
        base.Close();
    }
}