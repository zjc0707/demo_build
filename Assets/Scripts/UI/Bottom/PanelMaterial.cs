using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelMaterial : BasePanel<PanelMaterial>
{
    private Image main;
    private Transform top;
    private List<Item> items;
    private List<Image> history;
    private bool isMove;
    private Vector3 relativePosData;
    private Material material;
    private bool isFirstChange;
    protected override void _Start()
    {
        items = new List<Item>(4);
        history = new List<Image>(5);
        main = this.transform.Find("Content/Main").GetComponent<Image>();
        top = this.transform.Find("Top/Button");
        EventTriggerListener.Get(top).onLeftDown += OnTopLeftDown;
        items.Add(new Item(this.transform.Find("Content/Control/R"), onValueChanged));
        items.Add(new Item(this.transform.Find("Content/Control/G"), onValueChanged));
        items.Add(new Item(this.transform.Find("Content/Control/B"), onValueChanged));
        items.Add(new Item(this.transform.Find("Content/Control/A"), onValueChanged));
        history.AddRange(this.transform.Find("Content/History/Content").GetComponentsInChildren<Image>());
        history.ForEach(p =>
        {
            p.GetComponent<Button>().onClick.AddListener(() => ChangeData(p.color));
        });
    }
    public override void Open()
    {
        base.Open();
        //change by targetBuilding
        this.material = null;
        ChangeData(PanelControl.current.targetBuilding.Material.color);
        //init data
        this.material = MaterialUtil.Create();
        isFirstChange = true;
    }
    public override void Close()
    {
        base.Close();
        Image image = history[4];
        history.RemoveAt(4);
        history.Insert(0, image);
        image.transform.SetAsFirstSibling();
        image.color = main.color;
    }
    private void ChangeData(Color color)
    {
        items[0].Value = color.r * 255;
        items[1].Value = color.g * 255;
        items[2].Value = color.b * 255;
        items[3].Value = color.a * 255;
    }
    private void Update()
    {
        if (isMove)
        {
            this.transform.position = Input.mousePosition + this.relativePosData;
            if (Input.GetMouseButtonUp(0))
            {
                isMove = false;
            }
        }
    }
    private void OnTopLeftDown(GameObject go)
    {
        this.relativePosData = this.transform.position - Input.mousePosition;
        isMove = true;
    }
    private void onValueChanged()
    {
        main.color = new Color(items[0].Value / 255, items[1].Value / 255, items[2].Value / 255, items[3].Value / 255);
        if (this.material == null)
        {
            return;
        }
        this.material.color = main.color;
        if (isFirstChange)
        {
            PanelControl.current.targetBuilding.Material = this.material;
            isFirstChange = false;
        }
    }
    private class Item
    {
        private Slider slider;
        private InputField inputField;
        public float Value
        {
            get
            {
                return slider.value;
            }
            set
            {
                slider.value = value;
            }
        }
        public Item(Transform parent, Action onValueChanged)
        {
            this.slider = parent.Find("Slider").GetComponent<Slider>();
            this.inputField = parent.Find("InputField").GetComponent<InputField>();
            this.slider.onValueChanged.AddListener(f =>
            {
                Debug.Log("slider:" + f);
                this.inputField.text = ((int)f).ToString();
            });
            this.inputField.onValueChanged.AddListener(s =>
            {
                Debug.Log("inputField:" + s);
                int i;
                int.TryParse(s, out i);
                if (i > 255)
                {
                    i = 255;
                }
                else if (i < 0)
                {
                    i = 0;
                }
                this.slider.value = i;
                onValueChanged();
            });
        }
    }
}