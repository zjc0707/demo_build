using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[UIType(UIStackType.START)]
public class PanelLoad : BasePanel<PanelLoad>
{
    /// <summary>
    /// 用于克隆的基础行
    /// </summary>
    public GameObject item;
    public Transform navigation;
    public Button buttonPre, buttonNext;
    public Text pageIndex;
    private int nowIndex = int.MinValue;
    /// <summary>
    /// 总页数
    /// </summary>
    private int pages = 0;
    private Transform content;
    private Dictionary<int, List<GameObject>> pageCache = new Dictionary<int, List<GameObject>>();
    protected override void _Start()
    {
        content = item.transform.parent;
        item.SetActive(false);

        buttonPre = navigation.Find("ButtonPre").GetComponent<Button>();
        buttonNext = navigation.Find("ButtonNext").GetComponent<Button>();
        pageIndex = navigation.Find("PageIndex").Find("Text").GetComponent<Text>();

        buttonPre.onClick.AddListener(delegate
        {
            Load(nowIndex - 1);
        });
        buttonNext.onClick.AddListener(delegate
        {
            Load(nowIndex + 1);
        });
    }
    public override void Open()
    {
        base.Open();
        if (pageCache.Count == 0)
        {
            nowIndex = int.MinValue;
            Load(1);
        }

    }
    public void Fresh()
    {
        pageCache.Clear();
    }

    private void Load(int pageIndex)
    {
        if (pageIndex == nowIndex) return;
        nowIndex = pageIndex;

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
            LoadNavigation();
        }
        else
        {
            Debug.Log(nowIndex);
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
                LoadNavigation();
            });

        }
    }
    private void LoadNavigation()
    {
        buttonPre.interactable = nowIndex > 1;
        buttonNext.interactable = nowIndex < pages;
        pageIndex.text = string.Format("{0}/{1}", nowIndex, pages);
    }
}