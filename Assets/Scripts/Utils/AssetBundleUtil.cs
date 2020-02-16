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
        List<Manifest> localManifests = LocalAssetUtil.Manifests;
        WebUtil.FindModelList(rs =>
        {
            Debug.Log(Json.Serialize(rs));
            Manifest manifestType0 = localManifests.Find(m => m.ModelType.Id == 0);
            List<Model> needDownload;
            if (null == manifestType0)
            {
                needDownload = rs;
            }
            else
            {
                needDownload = new List<Model>();
                rs.ForEach(m =>
                {
                    if (!manifestType0.Models.Exists(i => i.Id == m.Id))
                    {
                        needDownload.Add(m);
                    }
                });
            }
            MyWebRequest.current.LoadAssetBundle(needDownload, localManifests, () =>
            {
                if (needDownload.Count == 0)
                {
                    Debug.Log("无更新项");
                    return;
                }
                LocalAssetUtil.Manifests = localManifests;
            });
        },
        err =>
        {
            PanelLoading.current.Error("error:" + err + ",已加载缓存");
            MyWebRequest.current.LoadAssetBundle(null, localManifests, null);
        });
    }
}