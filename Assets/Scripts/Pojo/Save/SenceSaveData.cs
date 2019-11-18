using UnityEngine;
public class SenceSaveData : AbstractToStringObject
{
    public TransformGroupSaveData CameraTransformGroup { get; set; }
    public BuildingRoomSaveData BuildingRoomSaveData { get; set; }
    public FloorSaveData FloorSaveData { get; set; }
    public SenceSaveData()
    {

    }
}