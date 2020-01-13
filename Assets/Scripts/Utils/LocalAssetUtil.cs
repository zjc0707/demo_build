using System.IO;
using System.Collections.Generic;
using UnityEngine;
public static class LocalAssetUtil
{
    private static List<Manifest> manifests;
    public static List<Manifest> Manifests
    {
        set
        {
            manifests = value;
            SaveManifests(value);
        }
        get
        {
            if (null == manifests)
            {
                manifests = LoadManifests();
            }
            return manifests;
        }
    }
    public static Model GetModel(int id)
    {
        foreach (Manifest manifest in manifests)
        {
            foreach (Model model in manifest.Models)
            {
                if (model.Id == id)
                {
                    return model;
                }
            }
        }
        return null;
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
    private static List<Manifest> LoadManifests()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Debug.Log("exist:" + json);
            return string.IsNullOrEmpty(json) ? new List<Manifest>() : Json.Parse<List<Manifest>>(json);
        }
        else
        {
            File.Create(path).Dispose();
            Debug.Log("create:" + path);
            return new List<Manifest>();
        }
    }
    private static void SaveManifests(List<Manifest> manifest)
    {
        string json = Json.Serialize(manifest);
        Debug.Log("更新本地manifest：" + json);
        File.WriteAllText(path, json);
    }
}