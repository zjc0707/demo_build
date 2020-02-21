using System.IO;
using System.Collections.Generic;
using UnityEngine;
public static class LocalAssetUtil
{
    private static Manifest manifest;
    public static Manifest Manifest
    {
        set
        {
            manifest = value;
            SaveManifests(value);
        }
        get
        {
            if (null == manifest)
            {
                manifest = LoadManifests();
            }
            return manifest;
        }
    }
    public static Model GetModel(int id)
    {
        foreach (Model model in manifest.Models)
        {
            if (model.Id == id)
            {
                return model;
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
    private static Manifest LoadManifests()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Debug.Log("exist:" + json);
            return string.IsNullOrEmpty(json) ? new Manifest() : Json.Parse<Manifest>(json);
        }
        else
        {
            File.Create(path).Dispose();
            Debug.Log("create:" + path);
            return new Manifest();
        }
    }
    private static void SaveManifests(Manifest manifest)
    {
        string json = Json.Serialize(manifest);
        Debug.Log("更新本地manifest：" + json);
        File.WriteAllText(path, json);
    }
}