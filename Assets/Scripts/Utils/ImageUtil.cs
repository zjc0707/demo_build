using UnityEngine;
public static class ImageUtil
{
    public const string FOLDER_NAME = "Image/";
    public static Sprite GetSprite(BaseItemData data)
    {
        return Resources.Load<Sprite>(FOLDER_NAME + data.Url);
    }
}