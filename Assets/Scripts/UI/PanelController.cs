using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour {

    private Button buttonMenu;

	// Use this for initialization
	void Start () {
        buttonMenu = transform.parent.Find("ButtonMenu").GetComponent<Button>();
        buttonMenu.onClick.AddListener(delegate
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        });
        Load();
    }
	
    private void Load()
    {
        List<PanelControllerItem> items = PanelControllerItem.List;
        Transform item = transform.Find("Item");
        foreach(PanelControllerItem t in items)
        {
            Transform clone = Instantiate(item.gameObject).transform;
            clone.GetComponentInChildren<Text>().text = t.Name;
            clone.GetComponentInChildren<Button>().onClick.AddListener(delegate
            {
                Debug.Log(t.Url);
                GameObject cube = Instantiate(Resources.Load("Prefabs/" + t.Url)) as GameObject;
                Person.current.Catch(cube.transform);
            });
            clone.SetParent(transform);
        }
        item.gameObject.SetActive(false);
    }
}
