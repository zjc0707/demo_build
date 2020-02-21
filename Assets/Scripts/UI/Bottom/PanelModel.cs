using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[UIType(UIStackType.BOTTOM)]
public class PanelModel : BasePanel<PanelModel>
{
    public Transform baseItem, baseTypeItem;
    private bool load = false;
    private Dictionary<int, Item> itemDic;
    protected override void _Start()
    {

    }
    public override void Open()
    {
        base.Open();
        if (!load)
        {
            Load();
        }
    }
    private void Load()
    {
        Manifest manifest = LocalAssetUtil.Manifest;
        itemDic = new Dictionary<int, Item>(manifest.ModelType.Count);
        manifest.ModelType.ForEach(p =>
        {
            Transform clone = Instantiate(baseTypeItem.gameObject, baseTypeItem.parent).transform;
            clone.GetComponentInChildren<Text>().text = p.Name;
            itemDic.Add(p.Id, new Item(clone));
        });
        manifest.Models.ForEach(model =>
        {
            Transform clone = Instantiate(baseItem.gameObject, baseItem.parent).transform;
            clone.GetComponentInChildren<Text>().text = model.Name.Substring(0, model.Name.LastIndexOf('.'));
            clone.GetComponentInChildren<Button>().onClick.AddListener(delegate
            {
                Building building = BuildingUtil.Create(model);
                PanelState.current.baseInputMouse.Catch(building);
                int maxBuilding = building.IsTooBig();
                if (maxBuilding != int.MinValue)
                {
                    building.gameObject.SetActive(false);
                    PanelDialog.current.Open("场景过小，\n需扩建至(" + maxBuilding + "," + maxBuilding + ")", () =>
                    {
                        Floor.current.Load(maxBuilding, maxBuilding);
                        building.gameObject.SetActive(true);
                        WhenTooBig(building);
                    }, () =>
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
            clone.Find("Image").GetComponent<Image>().sprite = AssetBundleUtil.DicSprite[model.Id];
            clone.gameObject.SetActive(false);
            itemDic[model.ModelTypeId].contents.Add(clone);
        });

        baseTypeItem.gameObject.SetActive(false);
        baseItem.gameObject.SetActive(false);

        itemDic[manifest.ModelType[0].Id].Select(true);
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

    class Item
    {
        public static Item lastClick;
        public List<Transform> contents;
        public Image background;
        public Item(Transform type)
        {
            this.background = type.GetComponent<Image>();
            this.contents = new List<Transform>();
            type.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (lastClick != null)
                {
                    lastClick.Select(false);
                }
                this.Select(true);
            });
        }
        public void Select(bool flag)
        {
            contents.ForEach(p => p.gameObject.SetActive(flag));
            background.color = flag ? Color.yellow : Color.white;
            if (flag)
            {
                lastClick = this;
            }
        }
    }
}
