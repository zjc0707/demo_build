using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelControl : BasePanel<PanelControl>
{
    private InputField objectName;
    private InputFieldVector3 position, rotation, scale;
    private Button buttonOperate, buttonClear, buttonMaterialRecovery;
    private const int INPUT_FIELD_NUM = 10;
    private Transform target;
    private Building targetBuilding;
    private bool isFirst = true;
    protected override void _Start()
    {
        Load();
        base.Close();
    }
    // public override void Close()
    // {
    //     base.Close();
    //     // 
    // }
    public void SetData(Building building)
    {
        base.Open();
        targetBuilding = building;
        target = targetBuilding.transform;
        objectName.text = target.name;
        position.Set(target.localPosition);
        rotation.Set(target.localEulerAngles);
        scale.Set(target.localScale);
        // buttonOperate.gameObject.SetActive(targetBuilding.data.Operate == 1);
        AddListener();
    }
    public void UpdatePosData()
    {
        position.Set(target.localPosition);
    }
    public void UpdateRotData()
    {
        rotation.Set(target.localEulerAngles);
    }
    public void UpdateScaleData()
    {
        scale.Set(target.localScale);
    }
    public void SetTargetMaterial(Material material)
    {
        targetBuilding.SetMaterial(material);
    }
    /// <summary>
    /// 绑定事件
    /// </summary>
    private void AddListener()
    {
        if (!isFirst) return;
        isFirst = false;
        //输入框监听事件
        this.position.AddValueChangedListener(delegate
        {
            if (target == null) return;
            target.localPosition = this.position.ToVector3();
            Coordinate.Target.SetTarget(target);
        });
        this.rotation.AddValueChangedListener(delegate
        {
            if (target == null) return;
            target.localEulerAngles = this.rotation.ToVector3();
            Coordinate.Target.SetTarget(target);
        });
        this.scale.AddValueChangedListener(delegate
        {
            if (target == null) return;
            target.localScale = this.scale.ToVector3();
            Coordinate.Target.SetTarget(target);
        });
        //输入框编辑完监听事件
        this.position.AddEndEditListener(delegate
        {
            this.position.Set(target.localPosition);
        });
        this.rotation.AddEndEditListener(delegate
        {
            this.rotation.Set(target.localEulerAngles);
        });
        //进入操作模式
        // this.buttonOperate.onClick.AddListener(delegate
        // {
        //     Sport.current.TurnToOperate(targetBuilding);
        // });
        this.buttonClear.onClick.AddListener(delegate
        {
            if (target == null)
            {
                return;
            }
            PanelList.current.Remove(targetBuilding);
            base.Close();
        });
        this.buttonMaterialRecovery.onClick.AddListener(delegate
        {
            targetBuilding.RecovercyMaterial();
        });
    }

    /// <summary>
    /// 匹配输入框
    /// </summary>
    private void Load()
    {
        InputField[] inputFields = this.GetComponentsInChildren<InputField>();
        if (inputFields.Length != INPUT_FIELD_NUM)
        {
            throw new Exception("输入框数量不匹配：" + inputFields.Length);
        }

        objectName = inputFields[0];
        for (int i = 1; i < INPUT_FIELD_NUM; i++)
        {
            inputFields[i].contentType = InputField.ContentType.DecimalNumber;
        }
        position = new InputFieldVector3(inputFields[1], inputFields[2], inputFields[3]);
        rotation = new InputFieldVector3(inputFields[4], inputFields[5], inputFields[6]);
        scale = new InputFieldVector3(inputFields[7], inputFields[8], inputFields[9]);
        Transform content = this.transform.Find("Content");
        buttonOperate = content.Find("ButtonOperate").GetComponent<Button>();
        buttonClear = content.Find("ButtonClear").GetComponent<Button>();
        buttonMaterialRecovery = content.Find("Material").Find("ButtonGroup").Find("ButtonRecovery").GetComponent<Button>();
    }
}
