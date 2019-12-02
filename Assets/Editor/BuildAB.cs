using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEditor;

public class BuildAB : Editor
{
    public const string abDirectory = "Assets/AB";
    public const string suffix = ".zjc";
    public const string menu = "My Editor";

    [MenuItem(menu + "/Build AB from window Project select ")]
    static void Build()
    {
        Object[] selects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        EditorCoroutineRunner.StartEditorCoroutine(CoroutineBuild(selects));
    }
    // [MenuItem(menu + "/Build AB from window Project select (single)")]
    // static void Build2()
    // {
    //     Object[] selects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
    //     Object obj = selects[0];
    //     string assetPath = AssetDatabase.GetAssetPath(obj);
    //     EditorCoroutineRunner.StartEditorCoroutine(CoroutineBuild(obj, assetPath));
    //     // for (int i = 0; i < selects.Length; ++i)
    //     // {
    //     //     Object obj = selects[i];
    //     //     string assetPath = AssetDatabase.GetAssetPath(obj);
    //     //     EditorCoroutineRunner.StartEditorCoroutine(CoroutineBuild(obj, assetPath));
    //     // }
    // }
    private static IEnumerator CoroutineBuild(Object[] selects)
    {
        Debug.Log("Start");
        List<AssetBundleBuild> list = new List<AssetBundleBuild>();
        int index = 0;
        int wait = 0;
        while (index < selects.Length)
        {
            Object obj = selects[index];
            Debug.Log(index + "    " + obj.name);
            string path = AssetDatabase.GetAssetPath(obj);
            string newPrefabPath = EditScript(obj, path);
            while (AssetPreview.GetAssetPreview(obj) == null)
            {
                Debug.Log("Load texture2D_" + (wait++) + ":" + obj.name);
                yield return null;
            }
            Texture2D texture2D = AssetPreview.GetAssetPreview(obj);
            string newImgPath = path.Split('.')[0] + "-sprite.png";
            File.WriteAllBytes(newImgPath, texture2D.EncodeToPNG());
            AssetBundleBuild assetBundle = new AssetBundleBuild();
            assetBundle.assetBundleName = obj.name + suffix;
            assetBundle.assetNames = new string[] { newPrefabPath, newImgPath };
            list.Add(assetBundle);

            index++;
            wait = 0;
        }
        AssetDatabase.Refresh();
        ObjToAB(list.ToArray());
    }
    private static IEnumerator CoroutineBuild(Object obj, string path)
    {
        Debug.Log("Start");
        string newPrefabPath = EditScript(obj, path);
        Texture2D texture2D = AssetPreview.GetAssetPreview(obj);
        int count = 0;
        while (texture2D == null)
        {
            Debug.Log(count++);
            yield return null;
        }
        string newImgPath = path.Split('.')[0] + "-sprite.png";
        File.WriteAllBytes(newImgPath, texture2D.EncodeToPNG());
        AssetDatabase.Refresh();
        ObjToAB(obj.name, newPrefabPath, newImgPath);
    }
    private static void ObjToAB(AssetBundleBuild[] buildArray)
    {
        if (BuildPipeline.BuildAssetBundles(abDirectory, buildArray, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX))
        {
            Debug.Log("build AB success");
        }
        else
        {
            Debug.Log("build AB failure");
        }
        foreach (AssetBundleBuild build in buildArray)
        {
            foreach (string path in build.assetNames)
            {
                File.Delete(path);
            }
        }
        AssetDatabase.Refresh();
    }
    private static void ObjToAB(string name, string path, string imagePath)
    {
        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
        buildMap[0].assetBundleName = name + suffix;
        buildMap[0].assetNames = new string[] { path, imagePath };
        if (BuildPipeline.BuildAssetBundles(abDirectory, buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX))
        {
            Debug.Log("build AB success:" + name);
        }
        else
        {
            Debug.Log("build AB failure:" + name);
        }
        File.Delete(path);
        File.Delete(imagePath);
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 加载prefab并添加building脚本进行init()，再另存为带-edit后缀的新prefab
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>新prefab的路径</returns>
    private static string EditScript(Object obj, string assetPath)
    {
        GameObject gameObj = PrefabUtility.InstantiatePrefab(obj) as GameObject;
        BuildingUtil.GetComponentBuilding(gameObj.transform).Init();
        string newPath = assetPath.Replace(".", "-edit.");
        bool success;
        PrefabUtility.SaveAsPrefabAsset(gameObj, newPath, out success);
        DestroyImmediate(gameObj);
        Debug.Log(newPath + "    " + success);
        return newPath;
    }
}
