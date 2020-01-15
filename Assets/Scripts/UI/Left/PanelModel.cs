using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[UIType(UIStackType.LEFT)]
public class PanelModel : BasePanel<PanelModel>
{
    public Transform baseItem;

    protected override void _Start()
    {

        // base.Open();
    }
    public override void Open()
    {
        base.Open();
        Load();
    }
    private void Load()
    {
        // List<ModelData> items = ModelDataTest.List;
        List<Manifest> localManifests = LocalAssetUtil.Manifests;
        localManifests.ForEach(manifest =>
        {
            manifest.Models.ForEach(model =>
            {
                Transform clone = Instantiate(baseItem.gameObject).transform;
                clone.GetComponentInChildren<Text>().text = model.Name;
                clone.GetComponentInChildren<Button>().onClick.AddListener(delegate
                {
                    Building building = BuildingHelper.Create(model);
                    building.Choose();
                    // MouseBehaviour.current.Catch(building);
                    PanelState.current.baseInputMouse.Catch(building);
                    int maxBuilding = building.IsTooBig();
                    if (maxBuilding != int.MinValue)
                    {
                        building.gameObject.SetActive(false);
                        PanelDialog.current.Open("场景过小，\n需扩建至(" + maxBuilding + "," + maxBuilding + ")", delegate
                        {
                            Floor.current.Load(maxBuilding, maxBuilding);
                        }, delegate
                        {
                            building.gameObject.SetActive(true);
                            WhenTooBig(building);
                        });
                    }
                    else
                    {
                        WhenTooBig(building);
                    }
                });
                // clone.Find("Image").GetComponent<Image>().sprite = ImageUtil.GetSprite(t);
                clone.Find("Image").GetComponent<Image>().sprite = AssetBundleUtil.DicSprite[model.Id];

                clone.SetParent(baseItem.parent);
            });
        });

        baseItem.gameObject.SetActive(false);
    }
    /// <summary>
    /// 当物体过大时进行相机的拉伸
    /// </summary>
    /// <param name="building"></param>
    private void WhenTooBig(Building building)
    {
        int y = (int)building.Size.y;
        int x = (int)building.Size.x;
        int z = (int)building.Size.z;
        int cameraY = (int)Math.Sqrt(x * x + y * y + z * z);
        Vector3 pos = MyCamera.current.transform.position;
        if (cameraY > pos.y)
        {
            pos.y = cameraY;
            MyCamera.current.MoveAnim(pos);
        }
    }
}
