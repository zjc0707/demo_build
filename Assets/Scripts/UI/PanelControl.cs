using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelControl : BasePanel<PanelControl>
{
    private InputField objectName;
    private InputFieldVector3 position, rotation, scale;
    private const int INPUT_FIELD_NUM = 10;

    protected override void _Start()
    {
        Load();
    }

    protected override void OnButtonCloseClickSupply()
    {
        Building.LastRecovery();
    }

    public void SetData(Building building)
    {
        if(this.gameObject.activeInHierarchy == false)
        {
            this.gameObject.SetActive(true);
        }

        Transform target = building.transform;
        objectName.text = target.name;
        position.Set(target.localPosition);
        rotation.Set(target.localEulerAngles);
        scale.Set(target.localScale);
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
        for(int i = 1; i < INPUT_FIELD_NUM; i++)
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
