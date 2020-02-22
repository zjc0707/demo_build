using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
[UIType(UIStackType.RIGHT)]
public class PanelAnimEditor : BasePanel<PanelAnimEditor>
{
    private Item data, begin, end;
    private InputField inputFieldDuration, inputFieldName;
    private bool isRelative;
    private Action<AnimData> actionResult;
    private Button buttonPlay;
    private Toggle toggleAbsolute, toggleRelative;
    private bool isAdd;
    /// <summary>
    /// open时记录对象在PanelAnim下的值，用于close时复原
    /// </summary>
    private TransformGroup openTransformGroup;
    protected override void _Start()
    {
        data = new Item(transform.Find("Content/Data"));
        begin = new Item(transform.Find("Content/DataBegin"));
        end = new Item(transform.Find("Content/DataEnd"));
        inputFieldDuration = transform.Find("Content/DataTime/InputField").GetComponent<InputField>();
        inputFieldName = transform.Find("Content/Name/InputField").GetComponent<InputField>();
        buttonPlay = transform.Find("Content/ButtonPlay").GetComponent<Button>();
        toggleAbsolute = transform.Find("Content/ToggleGroup/ToggleAbsolute").GetComponent<Toggle>();
        toggleRelative = transform.Find("Content/ToggleGroup/ToggleRelative").GetComponent<Toggle>();
        #region listener
        data.Position.AddValueChangedListener(delegate
        {
            if (data.Target == null) return;
            data.Target.localPosition = data.Position.Data;
            Coordinate.Target.SetTarget(data.Target);
            begin.UpdatePosition();
            end.UpdatePosition();
        });
        data.Rotation.AddValueChangedListener(delegate
        {
            if (data.Target == null) return;
            data.Target.localEulerAngles = data.Rotation.Data;
            Coordinate.Target.SetTarget(data.Target);
            begin.UpdateRotation();
            end.UpdateRotation();
        });
        data.Scale.AddValueChangedListener(delegate
        {
            if (data.Target == null) return;
            data.Target.localScale = data.Scale.Data;
            Coordinate.Target.SetTarget(data.Target);
            begin.UpdateScale();
            end.UpdateScale();
        });
        transform.Find("Content/ButtonSave").GetComponent<Button>().onClick.AddListener(Save);
        buttonPlay.onClick.AddListener(delegate
        {
            if (!(begin.IsLock && end.IsLock))
            {
                PanelDialog.current.Open("请先锁定");
                return;
            }
            AnimData animData = GetAnimData();
            buttonPlay.interactable = false;
            Action<float> action = animData.Lerp(begin.Target, PanelAnim.current.openTransformGroup);
            PoolOfAnim.current.AddItem(animData.Duration, f =>
            {
                action(f);
                Coordinate.Target.SetTarget(begin.Target);
                data.UpdatePosition();
                data.UpdateRotation();
                data.UpdateScale();
            }, () =>
            {
                buttonPlay.interactable = true;
            });
        });
        toggleAbsolute.onValueChanged.AddListener(isOn =>
        {
            if (isOn)
            {
                isRelative = false;
                begin.IsRelative = isRelative;
                end.IsRelative = isRelative;
            }
        });
        toggleRelative.onValueChanged.AddListener(isOn =>
        {
            if (isOn)
            {
                isRelative = true;
                begin.IsRelative = isRelative;
                end.IsRelative = isRelative;
            }
        });
        #endregion
    }
    #region update
    public void UpdatePosition()
    {
        if (!this.gameObject.activeInHierarchy) return;
        data.UpdatePosition();
        begin.UpdatePosition();
        end.UpdatePosition();
    }
    public void UpdateRotation()
    {
        if (!this.gameObject.activeInHierarchy) return;
        data.UpdateRotation();
        begin.UpdateRotation();
        end.UpdateRotation();
    }
    public void UpdateScale()
    {
        if (!this.gameObject.activeInHierarchy) return;
        data.UpdateScale();
        begin.UpdateScale();
        end.UpdateScale();
    }
    #endregion
    public void Save()
    {
        if (!(begin.IsLock && end.IsLock))
        {
            PanelDialog.current.Open("请先锁定");
            return;
        }
        AnimData data = GetAnimData();
        actionResult(data);
        if (isAdd)
        {
            if (data.IsRelative)
            {
                (PanelAnim.current.openTransformGroup + data.End).Inject(begin.Target);
            }
            else
            {
                data.End.Inject(begin.Target);
            }
        }
        else
        {
            openTransformGroup.Inject(begin.Target);
        }
        Coordinate.Target.SetTarget(begin.Target);
        this.Close();
    }
    public override void Close()
    {
        PanelState.current.state = PanelState.State.NORMAL;
        base.Close();
    }
    public override void Open()
    {
        PanelState.current.state = PanelState.State.ANIM_EDITOR;
        base.Open();
    }
    /// <summary>
    /// 打开已有项，将building的transformGroup设为animData.End，startUI和endUI均赋值和锁定
    /// </summary>
    /// <param name="animData"></param>
    /// <param name="building"></param>
    /// <param name="actionResult"></param>
    public void Open(AnimData animData, Building building, Action<AnimData> actionResult)
    {
        Fresh();
        isAdd = false;
        openTransformGroup = building.transformGroup;
        toggleAbsolute.isOn = !animData.IsRelative;
        toggleRelative.isOn = animData.IsRelative;
        Transform target = building.transform;
        //修改已有项，全部锁定，物体处于End位置
        inputFieldName.text = animData.Name;
        inputFieldDuration.text = animData.Duration.ToString();

        if (animData.IsRelative)
        {
            (PanelAnim.current.openTransformGroup + animData.Begin).Inject(target);
        }
        else
        {
            animData.Begin.Inject(target);
        }
        begin.Target = target;
        begin.Lock();
        if (animData.IsRelative)
        {
            (PanelAnim.current.openTransformGroup + animData.End).Inject(target);
        }
        else
        {
            animData.End.Inject(target);
        }
        end.Target = target;
        end.Lock();

        data.Target = target;
        Coordinate.Target.SetTarget(target);
        this.actionResult = actionResult;
        this.Open();
    }
    /// <summary>
    /// 新建项，若有animDatas不为空，将building的transformGroup设为上一项的End，锁定startUI，若无则不改变
    /// </summary>
    /// <param name="animDatas"></param>
    /// <param name="building"></param>
    /// <param name="actionResult"></param>
    public void Add(List<AnimData> animDatas, Building building, Action<AnimData> actionResult)
    {
        Fresh();
        isAdd = true;
        Transform target = building.transform;
        if (animDatas.Count > 0)
        {
            AnimData animData = animDatas[animDatas.Count - 1];
            toggleAbsolute.isOn = !animData.IsRelative;
            toggleRelative.isOn = animData.IsRelative;
            (animData.IsRelative ? (PanelAnim.current.openTransformGroup + animData.End) : animData.End).Inject(target);
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
        data.Target = target;
        Coordinate.Target.SetTarget(target);
        this.actionResult = actionResult;
        this.Open();
    }
    private AnimData GetAnimData()
    {
        return new AnimData()
        {
            Name = inputFieldName.text,
            Duration = float.Parse(inputFieldDuration.text),
            Begin = begin.TransformGroup,
            End = end.TransformGroup,
            IsRelative = isRelative
        };
    }
    private void Fresh()
    {
        begin.Unlock();
        end.Unlock();
    }
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
        // private TransformGroup openTransformGroup;
        private TransformGroup OpenTransformGroup
        {
            get
            {
                // if (openTransformGroup == null)
                // {
                //     openTransformGroup = PanelAnim.current.openTransformGroup;
                // }
                // return openTransformGroup;
                return PanelAnim.current.openTransformGroup;
            }
        }
        private bool isRelative;
        public bool IsRelative
        {
            set
            {
                if (isRelative == value)
                {
                    return;
                }
                isRelative = value;
                if (isRelative)
                {
                    Position.Data -= OpenTransformGroup.Position;
                    Rotation.Data -= OpenTransformGroup.EulerAngles;
                    Scale.Data -= OpenTransformGroup.Scale;
                }
                else
                {
                    Position.Data += OpenTransformGroup.Position;
                    Rotation.Data += OpenTransformGroup.EulerAngles;
                    Scale.Data += OpenTransformGroup.Scale;
                }
            }
            get
            {
                return isRelative;
            }
        }
        public Item(Transform parent)
        {
            IsLock = false;
            InputField[] inputFields = parent.GetComponentsInChildren<InputField>();
            Position = new InputFieldVector3(inputFields[0], inputFields[1], inputFields[2]);
            Rotation = new InputFieldVector3(inputFields[3], inputFields[4], inputFields[5]);
            Scale = new InputFieldVector3(inputFields[6], inputFields[7], inputFields[8]);
            if (parent.Find("Button") == null)
            {
                return;
            }
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

        }
        public void Lock()
        {
            Lock(true);
        }
        public void Unlock()
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
            Position.Data = isRelative ? Target.localPosition - OpenTransformGroup.Position : Target.localPosition;
        }
        public void UpdateRotation()
        {
            if (Target == null || IsLock) return;
            Rotation.Data = isRelative ? Target.localEulerAngles - OpenTransformGroup.EulerAngles : Target.localEulerAngles;
        }
        public void UpdateScale()
        {
            if (Target == null || IsLock) return;
            Scale.Data = isRelative ? Target.localScale - OpenTransformGroup.Scale : Target.localScale;
        }
        public override string ToString()
        {
            return string.Format("pos:{0};eul:{1};scale:{2}", Position.Data, Rotation.Data, Scale.Data);
        }
    }
}