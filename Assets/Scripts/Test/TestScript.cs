using System.Linq;
using System.Text;
using System.Net.Security;
using System.Collections;
using System;
using System.Reflection;
using System.Data;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class TestScript : BaseUniqueObject<TestScript>
{
    private void Start()
    {
        // SqlTest();
        // ToStringTest();
        // FileTest();
        // Debug.Log(SceneService.current.Test());
        // Debug.Log(StringUtil.IsNullOrEmpty(null));
        // WWWTest();
    }
    public void WWWTest()
    {
        StartCoroutine(Page());
    }

    IEnumerator Page()
    {
        WWWForm form = new WWWForm();
        form.AddField("startIndex", 0);
        form.AddField("pageSize", 5);
        UnityWebRequest webRequest = UnityWebRequest.Post("http://127.0.0.1:4567/unity/scene/page", form);
        webRequest.timeout = 10;
        webRequest.SendWebRequest();
        while (!webRequest.isDone)
        {
            // Debug.Log(webRequest.downloadProgress);
            PanelLoading.current.Open();
            yield return 1;
        }
        PanelLoading.current.Close();
        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            Debug.Log(webRequest.downloadHandler.text);
            Page<Scene> page = Json.Parse<Page<Scene>>(webRequest.downloadHandler.text);
            Debug.Log(page);
            string str = System.Text.Encoding.UTF8.GetString(page.Records[0].Content);
            Debug.Log(str);
        }
    }
    public void ToStringTest()
    {
        // SenceSaveData saveData = new SenceSaveData()
        // {
        //     CameraTransformGroup = TransformGroupUtil.ToSaveData(MyCamera.current.transformGroup),
        //     BuildingRoomSaveData = new BuildingRoomSaveData(),
        //     FloorSaveData = new FloorSaveData()
        // };
        // saveData.FloorSaveData.X = Floor.current.x;
        // saveData.FloorSaveData.Z = Floor.current.z;
        // List<BuildingSaveData> buildingSaveDatas = new List<BuildingSaveData>();
        // buildingSaveDatas.Add(new BuildingSaveData()
        // {
        //     ModelDataId = 1,
        //     IsLock = true,
        //     TransformGroup = TransformGroupSaveData.Zero()
        // });
        // saveData.BuildingRoomSaveData.BuildingSaveDatas = buildingSaveDatas;
        // // Debug.Log(new Vector3SaveData(1, 1, 1));
        // Debug.Log(saveData.CameraTransformGroup);
        // Debug.Log(saveData);
        TestData test = new TestData()
        {
            Str = "test",
            array = new int[] { 1, 2, 3, 4 }
        };
        // object array = test.array;
        // Type t = array.GetType();
        // Debug.Log(t.IsArray);
        // int length = Convert.ToInt32(t.GetProperty("Length").GetValue(array));
        // Debug.Log(length);
        // Array rs = (Array)array;
        // Debug.Log(rs.Length);
        // for (int i = 0; i < rs.Length; i++)
        // {
        //     Debug.Log(rs.GetValue(i));
        // }
        Debug.Log(test);
    }
    public void FileTest()
    {
        // GameObject.Find("ButtonModel").GetComponent<Button>().onClick.AddListener(delegate
        // {

        // });
        // FileUtil.Save("123", "content");
    }

    public void SqlTest()
    {
        // List<Test> list = new List<Test>();
        // Debug.Log(list.GetType().Namespace);
        // Debug.Log(StringUtil.IsBulitinType(list.GetType()));
        // Test test = new Test()
        // {
        //     Id = 1
        // };
        // List<Test> rs = TestService.current.Select();
        // foreach (Test t in rs)
        // {
        //     Debug.Log(t);
        // }
        // Test selectByIdRS = TestService.current.SelectById(1);
        // Debug.Log(selectByIdRS);
        // Debug.Log(TestService.current.Count());
        // Debug.Log(page);
    }

}