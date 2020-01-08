using System.IO;
using System.Collections.Generic;
using UnityEngine;
public static class LocalAssetUtil
{
    public static List<AssetItem> Manifest
    {
        set
        {
            SaveManifest(value);
        }
        get
        {
            return LoadManifest();
        }
    }
    private static string _path;
    private static string path
    {
        get
        {
            if (null == _path)
            {
                _path = MyWebRequest.current.LOCAL_ASSET_PATH + "manifest.jc";
            }
            return _path;
        }
    }
    private static List<AssetItem> LoadManifest()
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
    private static void SaveManifest(List<AssetItem> manifest)
    {
        string json = Json.Serialize(manifest);
        Debug.Log("更新本地manifest：" + json);
        File.WriteAllText(path, json);
    }
}