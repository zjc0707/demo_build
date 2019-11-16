using System.Collections.Generic;
using UnityEngine;
public static class SaveUtil
{
    public static void Save(string name)
    {
        SenceSaveData saveData = new SenceSaveData()
        {
            CameraTransformGroup = TransformGroupUtil.ToSaveData(MyCamera.current.transformGroup),
            BuildingRoomSaveData = new BuildingRoomSaveData(),
            FloorSaveData = new FloorSaveData()
        };
        //--floor
        saveData.FloorSaveData.X = Floor.current.x;
        saveData.FloorSaveData.Z = Floor.current.z;
        //--building
        List<BuildingSaveData> buildingSaveDatas = new List<BuildingSaveData>();
        foreach (Building building in BuildingRoom.current.buildingList)
        {
            buildingSaveDatas.Add(new BuildingSaveData(building));
        }
        saveData.BuildingRoomSaveData.BuildingSaveDatas = buildingSaveDatas;
        //--toJson
        string json = Json.Serialize(saveData);
        Debug.Log(saveData);
        Debug.Log(json);
        SenceSaveData fromJson = Json.Parse<SenceSaveData>(json);
        Debug.Log(fromJson);
        //--to bytes
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        string fromBytes = System.Text.Encoding.UTF8.GetString(bytes);
        Debug.Log(fromBytes);
        //--save to sql
    }
    public static void Load(int id)
    {

    }
}