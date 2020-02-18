using System.Collections.Generic;
using UnityEngine;
public static class AnimDataUtil
{
    public static List<AnimDataSaveData> ToSaveData(List<AnimData> animDatas)
    {
        List<AnimDataSaveData> rs = new List<AnimDataSaveData>();
        animDatas.ForEach(p => rs.Add((AnimDataSaveData)p));
        return rs;
    }
    public static List<AnimData> Parse(List<AnimDataSaveData> animDataSaveDatas)
    {
        List<AnimData> rs = new List<AnimData>();
        animDataSaveDatas.ForEach(p => rs.Add((AnimData)p));
        return rs;
    }
}