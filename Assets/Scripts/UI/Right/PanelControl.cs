using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[UIType(UIStackType.RIGHT)]
public class PanelControl : BasePanel<PanelControl>
{
    private InputField objectName;
    private InputFieldVector3 position, rotation, scale;
    private Button buttonClear, buttonMaterialRecovery, buttonNormalAnim, buttonAppearanceAnim;
    private Toggle toggleAnim;
    private const int INPUT_FIELD_NUM = 10;
    private Transform target;
    private Building targetBuilding;
    private bool isFirst = true;
    protected override void _Start()
    {
        Load();
        AddListener();
        base.Close();
    }
    public void SetData(Building building)
    {
        targetBuilding = building;
        target = targetBuilding.transform;
        objectName.text = target.name;
        position.Data = target.localPosition;
        rotation.Data = target.localEulerAngles;
        scale.Data = target.localScale;
        base.Open();
    }
    public void UpdatePosData()
    {
        if (target == null) return;
        position.Data = target.localPosition;
    }
    public void UpdateRotData()
    {
        if (target == null) return;
        rotation.Data = target.localEulerAngles;
    }
    public void UpdateScaleData()
    {
        if (target == null) return;
        scale.Data = target.localScale;
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
        Debug.Log("Addlistener");
        // if (!isFirst) return;
        // isFirst = false;
        //输入框监听事件
        this.position.AddValueChangedListener(delegate
        {
            if (target == null) return;
            target.localPosition = this.position.Data;
            Coordinate.Target.SetTarget(target);
        });
        this.rotation.AddValueChangedListener(delegate
        {
            if (target == null) return;
            target.localEulerAngles = this.rotation.Data;
            Coordinate.Target.SetTarget(target);
        });
        this.scale.AddValueChangedListener(delegate
        {
            if (target == null) return;
            target.localScale = this.scale.Data;
            Coordinate.Target.SetTarget(target);
        });
        // 输入框编辑完监听事件
        this.position.AddEndEditListener(delegate
        {
            if (target == null) return;
            this.position.Data = target.localPosition;
        });
        this.rotation.AddEndEditListener(delegate
        {
            if (target == null) return;
            this.rotation.Data = target.localEulerAngles;
        });
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
            if (target == null) return;
            targetBuilding.RecovercyMaterial();
        });
        this.buttonNormalAnim.onClick.AddListener(delegate
        {
            if (target == null) return;
            PanelAnim.current.Open(targetBuilding, PanelAnim.AnimType.NORMAL);
        });
        this.buttonAppearanceAnim.onClick.AddListener(delegate
        {
            if (target == null) return;
            PanelAnim.current.Open(targetBuilding, PanelAnim.AnimType.APPEARANCE);
        });
    }
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
        buttonClear = content.Find("ButtonClear").GetComponent<Button>();
        buttonMaterialRecovery = content.Find("Material").Find("ButtonGroup").Find("ButtonRecovery").GetComponent<Button>();
        buttonNormalAnim = content.Find("Anim/Content/Button").GetComponent<Button>();
        buttonAppearanceAnim = content.Find("Anim/Content2/Button").GetComponent<Button>();
        toggleAnim = content.Find("Anim/Content/Toggle").GetComponent<Toggle>();
    }
}
