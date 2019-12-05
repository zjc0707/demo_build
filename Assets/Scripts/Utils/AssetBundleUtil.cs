using System.IO;
using System.Collections.Generic;
using UnityEngine;
public static class AssetBundleUtil
{
    public static Dictionary<int, GameObject> DicPrefab = new Dictionary<int, GameObject>();
    public static Dictionary<int, Sprite> DicSprite = new Dictionary<int, Sprite>();
    public static void Load()
    {
        string PATH_MANIFEST = MyWebRequest.current.PATH_FOLDER_AB_LOACL + "manifest.jc";
        List<AssetItem> manifest = LoadManifest(PATH_MANIFEST);
        List<ModelData> modelDatas = ModelDataTest.List,
                        loadLocal = new List<ModelData>(),
                        loadInternet = new List<ModelData>();
        bool existCustom = false;
        foreach (ModelData data in modelDatas)
        {
            existCustom = false;
            foreach (AssetItem item in manifest)
            {
                if (item.Id == data.Id)
                {
                    // if (item.HashCode.Equals(data.HashCode))
                    {
                        //id和hash值都相同时加载本地资源
                        loadLocal.Add(data);
                        existCustom = true;
                        break;
                    }
                }
            }
            if (!existCustom)
            {
                //本地不存在，下载新资源
                loadInternet.Add(data);
            }
        }
        MyWebRequest.current.LoadAssetBundle(loadInternet, loadLocal, () =>
        {
            if (modelDatas.Count != manifest.Count)
            {
                // 最后更新本地manifest
                manifest.Clear();
                foreach (ModelData data in modelDatas)
                {
                    manifest.Add(new AssetItem()
                    {
                        Id = data.Id,
                        HashCode = data.HashCode
                    });
                }
                SaveManifest(manifest, PATH_MANIFEST);
            }
        });
    }
    private static List<AssetItem> LoadManifest(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Debug.Log("exist:" + json);
            return string.IsNullOrEmpty(json) ? new List<AssetItem>() : Json.Parse<List<AssetItem>>(json);
        }
        else
        {
            File.Create(path).Dispose();
            Debug.Log("create");
            return new List<AssetItem>();
        }
    }
    private static void SaveManifest(List<AssetItem> manifest, string path)
    {
        string json = Json.Serialize(manifest);
        Debug.Log("更新本地manifest：" + json);
        File.WriteAllText(path, json);
    }
    public class AssetItem
    {
        public int Id { get; set; }
        public string HashCode { get; set; }
    }
}