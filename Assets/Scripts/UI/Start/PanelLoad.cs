using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelLoad : BasePanel<PanelLoad>
{
    protected override int StackType { get { return UIStackType.START; } }
    /// <summary>
    /// 用于克隆的基础行
    /// </summary>
    public GameObject item;
    private Transform content;
    protected override void _Start()
    {
        content = item.transform.parent;
        item.SetActive(false);
    }
    public override void Open()
    {
        base.Open();
        item.SetActive(true);
        List<Scene> list = SceneService.current.SelectList();
        foreach (Scene scene in list)
        {
            Transform clone = Instantiate(item, content).transform;
            clone.Find("Name").GetComponentInChildren<Text>().text = scene.Name;
            clone.Find("DeployTime").GetComponentInChildren<Text>().text = TimeUtil.Format(scene.DeployTime);
            Transform operate = clone.Find("Operate");
            operate.Find("ButtonLoad").GetComponent<Button>().onClick.AddListener(delegate
            {
                SceneService.current.Load(scene.Id);
                // base.Clear();
                UGUITree.current.CloseStart();
            });
            operate.Find("ButtonDelete").GetComponent<Button>().onClick.AddListener(delegate
            {

            });
        }
        item.SetActive(false);
    }
}