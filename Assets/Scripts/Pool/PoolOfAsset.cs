using System.Collections.Generic;
using UnityEngine;
public class PoolOfAsset : BaseUniqueObject<PoolOfAsset>
{
    /// <summary>
    /// 对象池，key-对象modelId
    /// </summary>
    /// <returns></returns>
    private Dictionary<int, List<Item>> dicGoPool = new Dictionary<int, List<Item>>();
    /// <summary>
    /// 资源池中最大存活时间为checkSpan + maxAliveTime
    /// 单位 秒
    /// </summary>
    private float maxAliveTime = 5;
    private float checkSpan = 1;
    private float useTime = 0;
    /// <summary>
    /// 根据id创建go对象
    /// </summary>
    /// <param name="id">model数据的id</param>
    /// <returns></returns>
    public GameObject Create(int id)
    {
        List<Item> list = GetList(id);
        GameObject rs = null;
        if (list.Count == 0)
        {
            rs = GameObject.Instantiate(AssetBundleUtil.DicPrefab[id]);
        }
        else
        {
            Item item = list[list.Count - 1];
            rs = item.gameObject;
            rs.SetActive(true);
            list.RemoveAt(list.Count - 1);
        }
        return rs;
    }
    public void Destroy(Building building)
    {
        building.RecovercyMaterial();
        Destroy(building.modelDataId, building.gameObject);
    }
    public void Destroy(int id, GameObject go)
    {
        List<Item> list = GetList(id);
        go.SetActive(false);
        list.Add(new Item()
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
        foreach (List<Item> list in dicGoPool.Values)
        {
            for (int i = list.Count - 1; i >= 0; --i)
            {
                Item item = list[i];
                if (TimeUtil.UnixTimeSpan - item.saveTime > maxAliveTime)
                {
                    GameObject.Destroy(item.gameObject);
                    list.RemoveAt(i);
                }
            }
        }
    }
    private List<Item> GetList(int id)
    {
        List<Item> list = null;
        if (!dicGoPool.TryGetValue(id, out list))
        {
            list = new List<Item>();
            dicGoPool.Add(id, list);
        }
        return list;
    }

    class Item
    {
        public GameObject gameObject { get; set; }
        public long saveTime { get; set; }
    }
}