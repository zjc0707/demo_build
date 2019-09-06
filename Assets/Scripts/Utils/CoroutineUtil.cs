using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

public static class CoroutineUtil
{
    public static IEnumerator LoadSprite(Image image, BaseItemData data)
    {
        if (image == null)
        {
            yield break;
        }
        Debug.Log("异步加载缩略图：" + data.Name);
        Object obj = Resources.Load(data.Url);
        Texture2D texture2D = null;
        while (texture2D == null)
        {
            texture2D = AssetPreview.GetAssetPreview(obj);
            Debug.Log("还没好：" + data.Name);
            yield return texture2D;
        }
        Debug.Log("异步加载完成：" + data.Name);
        Texture2D tex = texture2D;
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        image.sprite = sprite;
        // Resources.UnloadUnusedAssets();
    }
}