using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[UIType(UIStackType.START)]
public class PanelLoad : PanelPage<PanelLoad>
{
    protected override void LoadNewPage()
    {
        WebUtil.PageSence(nowIndex, rs =>
        {
            Page<Scene> page = Json.Parse<Page<Scene>>(rs);
            List<GameObject> list = new List<GameObject>();
            item.SetActive(true);
            foreach (Scene scene in page.Records)
            {
                Transform clone = Instantiate(item, content).transform;
                clone.Find("Name").GetComponentInChildren<Text>().text = scene.Name;
                clone.Find("DeployTime").GetComponentInChildren<Text>().text = TimeUtil.Format(scene.DeployTime);
                Transform operate = clone.Find("Operate");
                operate.Find("ButtonLoad").GetComponent<Button>().onClick.AddListener(delegate
                {
                    SaveUtil.Load(scene.Id);
                    UGUITree.current.CloseStart();
                });
                operate.Find("ButtonDelete").GetComponent<Button>().onClick.AddListener(delegate
                {

                });
                list.Add(clone.gameObject);
            }
            item.SetActive(false);
            pageCache.Add(nowIndex, list);
            pages = page.Pages;
            FreshNavigation();
        });
    }
}