using System.Linq;
using System.Text;
using System.Net.Security;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
public class TestScript : BaseUniqueObject<TestScript>
{
    private void Start()
    {
        // SqlTest();
        ToStringTest();
        // FileTest();
        // Debug.Log(SceneService.current.Test());
        // Debug.Log(StringUtil.IsNullOrEmpty(null));
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
        FileUtil.Save("123", "content");
    }

    public void SqlTest()
    {
        // SqlAccess sqlAccess = new SqlAccess();
        // DataTable table = sqlAccess.ExecuteQuery("select * from test");
        // Debug.Log(table.Rows[0][1].ToString());
        // Debug.Log(table.ToString());

        Test test = new Test()
        {
            Id = 1
        };
        List<Test> rs = TestService.current.Select();
        foreach (Test t in rs)
        {
            Debug.Log(t);
        }
        Test selectByIdRS = TestService.current.SelectById(1);
        Debug.Log(selectByIdRS);
        // rs = testMapper.Select(new Test());
        // foreach (Test t in rs)
        // {
        //     Debug.Log(t);
        // }
        // Test insert = new Test()
        // {
        //     Content = "insertWithNull",
        //     // OtherContent = "other"
        // };
        // testMapper.Insert(insert);
        // rs = testMapper.Select(new Test());
        // foreach (Test t in rs)
        // {
        //     Debug.Log(t);
        // }
    }

}