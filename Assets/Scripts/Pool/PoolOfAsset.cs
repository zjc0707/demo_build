using System.Collections.Generic;
using UnityEngine;
public class PoolOfAsset : BaseUniqueObject<PoolOfAsset>
{
    private Dictionary<int, List<AssetItem>> dicGoPool = new Dictionary<int, List<AssetItem>>();
    /// <summary>
    /// 资源池中最大存活时间为checkSpan + maxAliveTime
    /// 单位 秒
    /// </summary>
    private float maxAliveTime = 5;
    private float checkSpan = 1;
    private float useTime = 0;
    public GameObject Create(int id)
    {
        List<AssetItem> list = GetList(id);
        GameObject rs = null;
        if (list.Count == 0)
        {
            rs = GameObject.Instantiate(AssetBundleUtil.DicPrefab[id]);
        }
        else
        {
            AssetItem item = list[list.Count - 1];
            rs = item.gameObject;
            rs.SetActive(true);
            list.RemoveAt(list.Count - 1);
        }
        return rs;
    }
    public void Destroy(Building building)
    {
        Destroy(building.data.Id, building.gameObject);
    }
    public void Destroy(int id, GameObject go)
    {
        List<AssetItem> list = GetList(id);
        go.SetActive(false);
        list.Add(new AssetItem()
        {
            gameObject = go,
            saveTime = TimeUtil.UnixTimeSpan
        });
    }
    private void Update()
    {
        useTime += Time.deltaTime;
        if (useTime > checkSpan)
        {
            useTime = 0;
            CheckOutOfAliveTime();
        }
    }
    private void CheckOutOfAliveTime()
    {
        List<int> listIndex = new List<int>();
        foreach (List<AssetItem> list in dicGoPool.Values)
        {
            listIndex.Clear();
            for (int i = 0; i < list.Count; ++i)
            {
                AssetItem item = list[i];
                if (TimeUtil.UnixTimeSpan - item.saveTime > maxAliveTime)
                {
                    GameObject.Destroy(item.gameObject);
                    listIndex.Add(i);
                }
            }
            foreach (int index in listIndex)
            {
                list.RemoveAt(index);
            }
        }
    }
    private List<AssetItem> GetList(int id)
    {
        List<AssetItem> list = null;
        if (!dicGoPool.TryGetValue(id, out list))
        {
            list = new List<AssetItem>();
            dicGoPool.Add(id, list);
        }
        return list;
    }

    class AssetItem
    {
        public GameObject gameObject { get; set; }
        public long saveTime { get; set; }
    }
}