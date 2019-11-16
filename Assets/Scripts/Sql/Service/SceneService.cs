using System.Collections.Generic;
using System;
using UnityEngine;
public class SceneService : BaseService<Scene, SceneMapper, SceneService>
{
    public void Save(string name)
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
        Debug.Log(json);
        //--to bytes
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);

        Scene scene = new Scene()
        {
            Name = name,
            Content = bytes,
            DeployTime = TimeUtil.UnixTimeSpan
        };
        Debug.Log(scene);
        base.Insert(scene);
    }
}