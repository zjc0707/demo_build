using System.Text;
using System.Net.Security;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Data;
using UnityEngine;
public class TestScript : BaseUniqueObject<TestScript>
{
    private void Start()
    {
        SqlTest();
        // ToStringTest();
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
        TestMapper testMapper = new TestMapper();
        List<Test> rs = testMapper.Select(test);
        foreach (Test t in rs)
        {
            Debug.Log(t);
        }
        Test selectByIdRS = testMapper.SelectById(1);
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

    // public void ToStringTest()
    // {
    //     // Test test = new Test();
    //     // Debug.Log(test.ToString());
    //     // test.Id = 1;
    //     // Debug.Log(test.ToString());
    //     // test.Content = "jcjcj";
    //     // Debug.Log(test.ToString());

    //     // Debug.Log(test.GetType().Name);
    //     // Debug.Log(test.GetType().GUID);
    //     // BaseModel baseModel = new BaseModel();
    //     // Debug.Log(baseModel.GetType().Name);
    //     // Debug.Log(baseModel.GetType().GUID);

    //     // List<WhereInfo> whereList = new List<WhereInfo>();
    //     // whereList.Add(WhereInfoHelper.Eq("int", 123));
    //     // whereList.Add(WhereInfoHelper.Ge("string", "string"));
    //     // Debug.Log(string.Join(" AND ", whereList));
    //     Parameter parameter = new Parameter()
    //     {
    //         Param = "param0",
    //         Value = "value0"
    //     };
    //     List<Parameter> parameters = new List<Parameter>();
    //     parameters.Add(new Parameter()
    //     {
    //         Param = "param1",
    //         Value = "value1"
    //     });
    //     parameters.Add(new Parameter()
    //     {
    //         Param = "param2",
    //         Value = "value2"
    //     });
    //     List<String> ss = new List<string>();
    //     ss.Add("hah");
    //     MyCommand myCommand = new MyCommand()
    //     {
    //         CommandText = "123",
    //         Parameters = parameters
    //     };
    //     // Debug.Log(parameter.ToString());
    //     // Debug.Log(parameters.ToString());
    //     // Debug.Log(myCommand.GetType().Name);
    //     Debug.Log(myCommand);
    // }

}