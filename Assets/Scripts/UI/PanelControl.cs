﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelControl : BasePanel<PanelControl>
{
    private InputField objectName;
    private InputFieldVector3 position, rotation, scale;
    private const int INPUT_FIELD_NUM = 10;
    private Transform target;
    private bool isFirst = true;

    protected override void _Start()
    {
        Load();
        this.gameObject.SetActive(false);
    }

    protected override void OnButtonCloseClickSupply()
    {
        Building.LastRecovery();
        State.current.SetStateToPerson();
    }

    public void SetData(Building building)
    {
        Debug.Log("更新面板信息：" + building.name);
        this.gameObject.SetActive(true);

        target = building.transform;
        objectName.text = target.name;
        position.Set(target.localPosition);
        rotation.Set(target.localEulerAngles);
        scale.Set(target.localScale);
        AddValueChangedListener();
    }

    /// <summary>
    /// 绑定输入框值变换监听事件
    /// </summary>
    private void AddValueChangedListener()
    {
        if (!isFirst) return;
        this.position.AddValueChangedListener(delegate
        {
            if (target == null) return;
            target.localPosition = this.position.ToVector3();
        });

        this.rotation.AddValueChangedListener(delegate
        {
            if (target == null) return;
            target.localEulerAngles = this.rotation.ToVector3();
        });

        this.scale.AddValueChangedListener(delegate
        {
            if (target == null) return;
            target.localScale = this.scale.ToVector3();
        });
    }

    /// <summary>
    /// 匹配输入框
    /// </summary>
    private void Load()
    {
        InputField[] inputFields = GetComponentsInChildren<InputField>();
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
    }

    private void DataTest()
    {
        objectName.text = "zjc";
        position.Set(1, 2, 3);
        rotation.Set(4, 5, 6);
        scale.Set(7, 8, 9);
    }

}
