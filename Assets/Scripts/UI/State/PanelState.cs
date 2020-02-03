using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 用于切换操作状态，类似unity界面的左上角
/// </summary>
public class PanelState : BasePanel<PanelState>
{
    private static List<Item> items;
    public BaseInputMouse baseInputMouse;
    private InputMouseCamera inputMouseHand;
    private InputMouseMove inputMouseMove;
    public enum State
    {
        NORMAL = 0,//默认模式
        ANIM_EDITOR = 1,//动画编辑模式
        VIEW_MODEL = 2,//观赏模式
    }
    public State state = State.NORMAL;
    protected override void _Start()
    {
        base.Open();
        items = new List<Item>();
        items.Add(new Item(this.transform.Find("CameraTool"), delegate
        {
            baseInputMouse = inputMouseHand;
        }));
        items.Add(new Item(this.transform.Find("PositionTool"), delegate
        {
            baseInputMouse = inputMouseMove;
            Coordinate.Target = CoordinatePosition.current;
        }));
        items.Add(new Item(this.transform.Find("RotationTool"), delegate
        {
            baseInputMouse = inputMouseMove;
            Coordinate.Target = CoordinateRotation.current;
        }));
        items.Add(new Item(this.transform.Find("ScaleTool"), delegate
        {
            baseInputMouse = inputMouseMove;
            Coordinate.Target = CoordinateScale.current;
        }));

        inputMouseMove = new InputMouseMove();
        inputMouseHand = new InputMouseCamera();
        items[1].button.onClick.Invoke();
    }
    private void Update()
    {
        baseInputMouse.MyUpdate();
    }
    public void ViewModelTurnOn()
    {
        items[0].button.onClick.Invoke();
        state = State.VIEW_MODEL;
    }
    public void ViewModelTurnOff()
    {
        items[1].button.onClick.Invoke();
        state = State.NORMAL;
    }

    private class Item
    {
        public Button button { get; set; }
        public Image image { get; set; }
        public Text text { get; set; }
        public Item(Transform t, Action action)
        {
            this.button = t.GetComponent<Button>();
            this.image = t.GetComponent<Image>();
            this.text = t.Find("Text").GetComponent<Text>();
            this.button.onClick.AddListener(delegate
            {
                PanelState.items.ForEach(p =>
                {
                    p.image.color = Color.white;
                    p.text.color = Color.black;
                });
                this.image.color = Color.black;
                this.text.color = Color.white;
                if (action != null)
                {
                    action();
                }
            });
        }
    }
}