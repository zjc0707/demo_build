using System.Collections.Generic;
using UnityEngine;
public static class AnimDataUtil
{
    public static AnimDataSaveData ToSaveData(AnimData animData)
    {
        return new AnimDataSaveData()
        {
            Name = animData.Name,
            Begin = TransformGroupUtil.ToSaveData(animData.Begin),
            End = TransformGroupUtil.ToSaveData(animData.End),
            Duration = animData.Duration,
            IsRelative = animData.IsRelative
        };
    }
    public static List<AnimDataSaveData> ToSaveData(List<AnimData> animDatas)
    {
        List<AnimDataSaveData> rs = new List<AnimDataSaveData>();
        animDatas.ForEach(p => rs.Add(ToSaveData(p)));
        return rs;
    }
    public static AnimData Parse(AnimDataSaveData animDataSaveData)
    {
        return new AnimData()
        {
            Name = animDataSaveData.Name,
            Begin = TransformGroupUtil.Parse(animDataSaveData.Begin),
            End = TransformGroupUtil.Parse(animDataSaveData.End),
            Duration = animDataSaveData.Duration,
            IsRelative = animDataSaveData.IsRelative
        };
    }
    public static List<AnimData> Parse(List<AnimDataSaveData> animDataSaveDatas)
    {
        List<AnimData> rs = new List<AnimData>();
        animDataSaveDatas.ForEach(p => rs.Add(Parse(p)));
        return rs;
    }
}