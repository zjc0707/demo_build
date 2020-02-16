using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
[UIType(UIStackType.START)]
public class PanelLoad : PanelPage<PanelLoad>
{
    protected override void LoadNewPage()
    {
        WebUtil.PageSence(nowIndex, page =>
        {
            if (page.Records != null && page.Records.Count > 0)
            {
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
                        BuildingUtil.Fresh();
                        UGUITree.current.CloseStart();
                    });
                    operate.Find("ButtonDelete").GetComponent<Button>().onClick.AddListener(delegate
                    {

                    });
                    list.Add(clone.gameObject);
                }
                item.SetActive(false);
                pageCache.Add(nowIndex, list);
            }

            pages = page.Pages;
            FreshNavigation();
        }, err =>
        {
            PanelLoading.current.Error(err);
        });
    }
}