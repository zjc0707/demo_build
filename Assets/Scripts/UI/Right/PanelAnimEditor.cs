using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
[UIType(UIStackType.RIGHT)]
public class PanelAnimEditor : BasePanel<PanelAnimEditor>
{
    private Item data, begin, end;
    private InputField inputFieldDuration, inputFieldName;
    private Action<AnimData> actionResult;
    private Button buttonPlay;
    private bool isAdd;
    /// <summary>
    /// open时记录对象初始值，用于close时复原
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
        #region listener
        data.Position.AddValueChangedListener(delegate
        {
            if (data.Target == null) return;
            begin.UpdatePosition();
            end.UpdatePosition();
            data.Target.localPosition = data.Position.Data;
            Coordinate.Target.SetTarget(data.Target);
        });
        data.Rotation.AddValueChangedListener(delegate
        {
            if (data.Target == null) return;
            begin.UpdateRotation();
            end.UpdateRotation();
            data.Target.localEulerAngles = data.Rotation.Data;
            Coordinate.Target.SetTarget(data.Target);
        });
        data.Scale.AddValueChangedListener(delegate
        {
            if (data.Target == null) return;
            begin.UpdateScale();
            end.UpdateScale();
            data.Target.localScale = data.Scale.Data;
            Coordinate.Target.SetTarget(data.Target);
        });
        transform.Find("Content/ButtonSave").GetComponent<Button>().onClick.AddListener(delegate
        {
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
            if (isAdd)
            {
                end.TransformGroup.Inject(begin.Target);
            }
            else
            {
                openTransformGroup.Inject(begin.Target);
            }
            Coordinate.Target.SetTarget(begin.Target);
            this.Close();
        });
        buttonPlay.onClick.AddListener(delegate
        {
            if (!(begin.IsLock && end.IsLock))
            {
                PanelDialog.current.Open("请先锁定");
                return;
            }
            AnimData animData = new AnimData()
            {
                Name = inputFieldName.text,
                Duration = float.Parse(inputFieldDuration.text),
                Begin = begin.TransformGroup,
                End = end.TransformGroup
            };
            buttonPlay.interactable = false;
            PoolOfAnim.current.AddItem(animData.Duration, f =>
            {
                animData.Lerp(begin.Target, f);
                Coordinate.Target.SetTarget(begin.Target);
                data.UpdatePosition();
                data.UpdateRotation();
                data.UpdateScale();
            }, () =>
            {
                buttonPlay.interactable = true;
            });
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
        Transform target = building.transform;
        //修改已有项，全部锁定，物体处于End位置
        inputFieldName.text = animData.Name;
        inputFieldDuration.text = animData.Duration.ToString();
        animData.Begin.Inject(target);
        begin.Target = target;
        begin.Lock();
        animData.End.Inject(target);
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
        data.Target = target;
        Coordinate.Target.SetTarget(target);
        this.actionResult = actionResult;
        this.Open();
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
}