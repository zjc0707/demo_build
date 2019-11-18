using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Jc.SqlTool.Core.Page;
public class PanelLoad : BasePanel<PanelLoad>
{
    protected override int StackType { get { return UIStackType.START; } }
    /// <summary>
    /// 用于克隆的基础行
    /// </summary>
    public GameObject item;
    public Transform navigation;
    public Button buttonPre, buttonNext;
    public Text pageIndex;
    private Page<Scene> page;
    private int nowIndex = int.MinValue;
    private Transform content;
    private Dictionary<int, List<GameObject>> pageCache = new Dictionary<int, List<GameObject>>();
    protected override void _Start()
    {
        content = item.transform.parent;
        item.SetActive(false);

        buttonPre = navigation.Find("ButtonPre").GetComponent<Button>();
        buttonNext = navigation.Find("ButtonNext").GetComponent<Button>();
        pageIndex = navigation.Find("PageIndex").Find("Text").GetComponent<Text>();

        page = new Page<Scene>(0, 5);
        buttonPre.onClick.AddListener(delegate
        {
            page.StartIndex--;
            Load(page);
        });
        buttonNext.onClick.AddListener(delegate
        {
            page.StartIndex++;
            Load(page);
        });
    }
    public override void Open()
    {
        base.Open();
        Load(page);
    }

    private void Load(Page<Scene> page)
    {
        if (page.StartIndex == nowIndex) return;
        nowIndex = page.StartIndex;

        foreach (Transform t in content)
        {
            t.gameObject.SetActive(false);
        }
        if (pageCache.ContainsKey(nowIndex))
        {
            foreach (GameObject obj in pageCache[nowIndex])
            {
                obj.SetActive(true);
            }
        }
        else
        {
            page = SceneService.current.Page(page);
            List<GameObject> list = new List<GameObject>();
            item.SetActive(true);
            foreach (Scene scene in page.Data)
            {
                Transform clone = Instantiate(item, content).transform;
                clone.Find("Name").GetComponentInChildren<Text>().text = scene.Name;
                clone.Find("DeployTime").GetComponentInChildren<Text>().text = TimeUtil.Format(scene.DeployTime);
                Transform operate = clone.Find("Operate");
                operate.Find("ButtonLoad").GetComponent<Button>().onClick.AddListener(delegate
                {
                    SceneService.current.Load(scene.Id);
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
        buttonPre.interactable = !page.IsFirst;
        buttonNext.interactable = !page.IsEnd;
        pageIndex.text = string.Format("{0}/{1}", nowIndex + 1, page.PageCount);

    }
}