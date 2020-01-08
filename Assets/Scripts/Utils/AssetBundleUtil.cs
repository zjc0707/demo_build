using System.IO;
using System.Collections.Generic;
using UnityEngine;
public static class AssetBundleUtil
{
    public static Dictionary<int, GameObject> DicPrefab = new Dictionary<int, GameObject>();
    public static Dictionary<int, Sprite> DicSprite = new Dictionary<int, Sprite>();
    public static void Clear()
    {
        DicPrefab.Clear();
        DicSprite.Clear();
    }
    public static void Load()
    {
        List<AssetItem> localManifest = LocalAssetUtil.Manifest;
        List<ModelData> modelDatas = ModelDataTest.List,
                        loadLocal = new List<ModelData>(),
                        loadInternet = new List<ModelData>();
        foreach (ModelData data in modelDatas)
        {
            bool existCustom = false;
            foreach (AssetItem item in localManifest)
            {
                if (item.Id == data.Id)
                {
                    loadLocal.Add(data);
                    existCustom = true;
                    break;
                }
            }
            if (!existCustom)
            {
                loadInternet.Add(data);
            }
        }
        MyWebRequest.current.LoadAssetBundle(loadInternet, loadLocal, () =>
        {
            if (modelDatas.Count != localManifest.Count)
            {
                // 最后更新本地manifest
                localManifest.Clear();
                foreach (ModelData data in modelDatas)
                {
                    localManifest.Add(new AssetItem()
                    {
                        Id = data.Id
                    });
                }
                LocalAssetUtil.Manifest = localManifest;
            }
        });
    }
}