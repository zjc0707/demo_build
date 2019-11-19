using UnityEngine;
using UnityEngine.UI;
public class PanelSaveAs : BasePanel<PanelOperate>
{
    protected override int StackType { get { return UIStackType.OPERATE; } }
    public InputField inputFieldName;
    public Button buttonSubmit, buttonQuit;
    protected override void _Start()
    {
        if (inputFieldName == null)
        {
            inputFieldName = this.transform.Find("Content").Find("InputFieldName").GetComponent<InputField>();
        }
        if (buttonSubmit == null)
        {
            buttonSubmit = this.transform.Find("Content").Find("ButtonGroup").Find("ButtonSubmit").GetComponent<Button>();
        }
        if (buttonQuit == null)
        {
            buttonQuit = this.transform.Find("Content").Find("ButtonGroup").Find("ButtonQuit").GetComponent<Button>();
        }
        buttonQuit.onClick.AddListener(base.Close);
        buttonSubmit.onClick.AddListener(Submit);
        buttonSubmit.interactable = false;
        inputFieldName.onValueChanged.AddListener(delegate
        {
            buttonSubmit.interactable = !string.IsNullOrEmpty(inputFieldName.text);
        });
    }
    private void Submit()
    {
        SaveUtil.Save(inputFieldName.text, delegate
        {
            base.Clear();
        });
    }
}