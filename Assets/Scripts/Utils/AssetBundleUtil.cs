using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
public static class AssetBundleUtil
{
    /// <summary>
    /// ab包提取的go缓存对象
    /// </summary>
    /// <typeparam name="int">modelId</typeparam>
    /// <typeparam name="GameObject"></typeparam>
    /// <returns></returns>
    public static Dictionary<int, GameObject> DicPrefab = new Dictionary<int, GameObject>();
    /// <summary>
    /// ab包提取的sprite对象
    /// </summary>
    /// <typeparam name="int">modelId</typeparam>
    /// <typeparam name="Sprite"></typeparam>
    /// <returns></returns>
    public static Dictionary<int, Sprite> DicSprite = new Dictionary<int, Sprite>();
    public static void Clear()
    {
        DicPrefab.Clear();
        DicSprite.Clear();
    }
    public static void Load()
    {
        Manifest localManifest = LocalAssetUtil.Manifest;
        WebUtil.FindModelTypeList(rs =>
        {
            localManifest.ModelType = rs;
        }, null);
        WebUtil.FindModelList(rs =>
        {
            Debug.Log(Json.Serialize(rs));
            List<Model> needDownload;
            if (localManifest.Models.Count == 0)
            {
                needDownload = rs;
            }
            else
            {
                needDownload = new List<Model>();
                rs.ForEach(m =>
                {
                    if (!localManifest.Models.Exists(i => i.Id == m.Id))
                    {
                        needDownload.Add(m);
                    }
                });
            }
            MyWebRequest.current.LoadAssetBundle(needDownload, localManifest, () =>
            {
                if (needDownload.Count == 0)
                {
                    Debug.Log("无更新项");
                }
                LocalAssetUtil.Manifest = localManifest;
            });
        },
        err =>
        {
            PanelLoading.current.Error("error:" + err + ",已加载缓存");
            MyWebRequest.current.LoadAssetBundle(null, localManifest, null);
        });
    }
}