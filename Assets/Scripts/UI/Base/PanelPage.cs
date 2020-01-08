using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class PanelPage<T> : BasePanel<T> where T : MonoBehaviour
{
    /// <summary>
    /// 用于克隆的基础行
    /// </summary>
    public GameObject item;
    public Transform navigation;
    public Button buttonPre, buttonNext;
    public Text pageIndex;
    protected int nowIndex = int.MinValue;
    /// <summary>
    /// 总页数
    /// </summary>
    protected int pages = 0;
    protected Transform content;
    protected Dictionary<int, List<GameObject>> pageCache = new Dictionary<int, List<GameObject>>();
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
    protected abstract void LoadNewPage();
    protected void Load(int pageIndex)
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
            FreshNavigation();
        }
        else
        {
            LoadNewPage();
        }
    }
    protected void FreshNavigation()
    {
        buttonPre.interactable = nowIndex > 1;
        buttonNext.interactable = nowIndex < pages;
        pageIndex.text = string.Format("{0}/{1}", nowIndex, pages);
    }
}