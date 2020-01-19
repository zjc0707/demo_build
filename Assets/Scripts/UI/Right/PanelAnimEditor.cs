using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
[UIType(UIStackType.RIGHT)]
public class PanelAnimEditor : BasePanel<PanelAnimEditor>
{
    private class Item
    {
        public InputFieldVector3 Position { get; }
        public InputFieldVector3 Rotation { get; }
        public InputFieldVector3 Scale { get; }
        public Button ButtonLock { get; }
        public Text ButtonText { get; }
        public bool IsLock { get; private set; }
        public TransformGroup TransformGroup
        {
            get
            {
                return new TransformGroup()
                {
                    Position = this.Position.Data,
                    EulerAngles = this.Rotation.Data,
                    Scale = this.Scale.Data,
                };
            }
        }
        private Transform target;
        public Transform Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
                UpdatePosition();
                UpdateRotation();
                UpdateScale();
            }
        }
        public Item(Transform parent)
        {
            IsLock = false;
            InputField[] inputFields = parent.GetComponentsInChildren<InputField>();
            Position = new InputFieldVector3(inputFields[0], inputFields[1], inputFields[2]);
            Rotation = new InputFieldVector3(inputFields[3], inputFields[4], inputFields[5]);
            Scale = new InputFieldVector3(inputFields[6], inputFields[7], inputFields[8]);
            ButtonLock = parent.Find("Button").GetComponent<Button>();
            ButtonText = parent.Find("Button/Text").GetComponent<Text>();
            //listener
            ButtonLock.onClick.AddListener(delegate
            {
                Lock(!IsLock);
                if (!IsLock)//解锁时更新面板
                {
                    UpdatePosition();
                    UpdateRotation();
                    UpdateScale();
                }
            });
            // Position.AddValueChangedListener(delegate
            // {
            //     if (Target == null) return;
            //     Target.localPosition = this.Position.Data;
            //     Coordinate.Target.SetTarget(Target);
            // });
            // Rotation.AddValueChangedListener(delegate
            // {
            //     if (Target == null) return;
            //     Target.localEulerAngles = this.Rotation.Data;
            //     Coordinate.Target.SetTarget(Target);
            // });
            // Scale.AddValueChangedListener(delegate
            // {
            //     if (Target == null) return;
            //     Target.localScale = this.Scale.Data;
            //     Coordinate.Target.SetTarget(Target);
            // });
        }
        public void Lock()
        {
            Lock(true);
        }
        public void QuitLock()
        {
            Lock(false);
        }
        private void Lock(bool flag)
        {
            Position.interactable = !flag;
            Rotation.interactable = !flag;
            Scale.interactable = !flag;
            ButtonText.text = flag ? "取消锁定" : "锁定";
            IsLock = flag;
        }
        public void UpdatePosition()
        {
            if (Target == null || IsLock) return;
            Position.Data = Target.localPosition;
        }
        public void UpdateRotation()
        {
            if (Target == null || IsLock) return;
            Rotation.Data = Target.localEulerAngles;
        }
        public void UpdateScale()
        {
            if (Target == null || IsLock) return;
            Scale.Data = Target.localScale;
        }
    }
    private Item begin, end;
    private InputField inputFieldDuration, inputFieldName;
    private Action<AnimData> actionResult;
    /// <summary>
    /// open时记录对象初始值，用于close时复原
    /// </summary>
    private TransformGroup initialValue;
    protected override void _Start()
    {
        begin = new Item(transform.Find("Content/DataBegin"));
        end = new Item(transform.Find("Content/DataEnd"));
        inputFieldDuration = transform.Find("Content/DataTime/InputField").GetComponent<InputField>();
        inputFieldName = transform.Find("Content/Name/InputField").GetComponent<InputField>();
        //listener
        transform.Find("Content/ButtonPlay").GetComponent<Button>().onClick.AddListener(delegate
        {

        });
        transform.Find("Content/ButtonSave").GetComponent<Button>().onClick.AddListener(delegate
        {
            if (actionResult == null)
            {
                Debug.LogError("actionResult == null");
                return;
            }
            if (!(begin.IsLock && end.IsLock))
            {
                PanelDialog.current.Open("请先锁定");
                return;
            }
            actionResult(new AnimData()
            {
                Name = inputFieldName.text,
                Duration = float.Parse(inputFieldDuration.text),
                Begin = begin.TransformGroup,
                End = end.TransformGroup
            });
            this.Close();
        });
    }
    public void UpdatePosition()
    {
        if (!this.gameObject.activeInHierarchy) return;
        begin.UpdatePosition();
        end.UpdatePosition();
    }
    public void UpdateRotation()
    {
        if (!this.gameObject.activeInHierarchy) return;
        begin.UpdateRotation();
        end.UpdateRotation();
    }
    public void UpdateScale()
    {
        if (!this.gameObject.activeInHierarchy) return;
        begin.UpdateScale();
        end.UpdateScale();
    }
    public override void Close()
    {
        initialValue.Inject(begin.Target);//复原
        Coordinate.Target.SetTarget(begin.Target);
        PanelState.current.state = PanelState.State.NORMAL;
        base.Close();
    }
    public override void Open()
    {
        PanelState.current.state = PanelState.State.ANIM_EDITOR;
        base.Open();
    }
    public void Open(AnimData data, Building building, Action<AnimData> actionResult)
    {
        Fresh();
        initialValue = building.transformGroup;
        Transform target = building.transform;
        //修改已有项，全部锁定，物体处于End位置
        inputFieldName.text = data.Name;
        inputFieldDuration.text = data.Duration.ToString();
        data.Begin.Inject(target);
        begin.Target = target;
        begin.Lock();
        data.End.Inject(target);
        end.Target = target;
        end.Lock();
        Coordinate.Target.SetTarget(target);
        this.actionResult = actionResult;
        this.Open();
    }
    public void Add(List<AnimData> animDatas, Building building, Action<AnimData> actionResult)
    {
        Fresh();
        initialValue = building.transformGroup;
        Transform target = building.transform;
        if (animDatas.Count > 0)
        {
            animDatas[animDatas.Count - 1].End.Inject(target);
            begin.Target = target;
            begin.Lock();
        }
        else
        {
            begin.Target = target;
        }
        end.Target = target;
        inputFieldName.text = (animDatas.Count + 1).ToString();
        inputFieldDuration.text = "1";
        Coordinate.Target.SetTarget(target);
        this.actionResult = actionResult;
        this.Open();
    }
    private void Fresh()
    {
        begin.QuitLock();
        end.QuitLock();
    }
}