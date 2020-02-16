using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public static class SaveUtil
{
    public static void Save(string name, UnityAction action)
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
        foreach (PanelList.Item item in PanelList.current.items)
        {
            buildingSaveDatas.Add(new BuildingSaveData(item.building));
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
        //--save to sql
        WebUtil.Save(scene, rs =>
        {
            PanelLoading.current.Success(rs);
            PanelLoad.current.Fresh();
            action();
        }, err =>
        {
            PanelLoading.current.Error(err);
        });
    }
    public static void Load(int id)
    {
        WebUtil.DetailSence(id, scene =>
        {
            PanelLoading.current.SceneLoading();
            string json = System.Text.Encoding.UTF8.GetString(scene.Content);
            Debug.Log(json);
            SenceSaveData saveData = Json.Parse<SenceSaveData>(json);
            //--floor
            Floor.current.Load(saveData.FloorSaveData.X, saveData.FloorSaveData.Z);
            //--building
            PanelList.current.Reset();
            foreach (BuildingSaveData data in saveData.BuildingRoomSaveData.BuildingSaveDatas)
            {
                PanelList.current.Add(BuildingUtil.Create(data));
            }
            //--camera
            MyCamera.current.MoveAnim(TransformGroupUtil.Parse(saveData.CameraTransformGroup));
            PanelLoading.current.Close();
        }, err =>
        {
            PanelLoading.current.Error(err);
        });
    }
}