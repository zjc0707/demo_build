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
    protected override void _Start()
    {
        base.Open();
        items = new List<Item>();
        items.Add(new Item(this.transform.Find("HandTool"), null));
        items.Add(new Item(this.transform.Find("MoveTool"), null));
        items.Add(new Item(this.transform.Find("RotateTool"), null));
        // items[1].button.onClick.Invoke();
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