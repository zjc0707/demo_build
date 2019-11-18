using System.Collections.Generic;
using System;
using UnityEngine;
using Jc.SqlTool.Core.Toolkit;
using Jc.SqlTool.Core.Page;
public class SceneService : BaseService<Scene, SceneMapper, SceneService>
{
    public void Load(int id)
    {
        Scene scene = this.SelectContentById(id);
        string json = System.Text.Encoding.UTF8.GetString(scene.Content);
        Debug.Log(json);
        SenceSaveData saveData = Json.Parse<SenceSaveData>(json);
        //--floor
        Floor.current.Load(saveData.FloorSaveData.X, saveData.FloorSaveData.Z);
        //--building
        BuildingRoom.current.buildingList.Clear();
        foreach (BuildingSaveData data in saveData.BuildingRoomSaveData.BuildingSaveDatas)
        {
            BuildingRoom.current.Add(BuildingHelper.Create(data));
        }
        //--camera
        MyCamera.current.MoveAnim(TransformGroupUtil.Parse(saveData.CameraTransformGroup));
    }
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
    public Page<Scene> Page(Page<Scene> page)
    {
        return base.Mapper.Select(Wrappers.Query(new Scene()).Select("id", "name", "deploy_time").OrderByDesc("deploy_time"), page);
    }
    public Scene SelectContentById(int id)
    {
        Scene scene = new Scene()
        {
            Id = id
        };
        return base.Mapper.SelectOne(Wrappers.Query(scene).Select("content"));
    }
}