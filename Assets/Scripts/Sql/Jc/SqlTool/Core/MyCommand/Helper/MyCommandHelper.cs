namespace Jc.SqlTool.Core.MyCommand.Helper
{
    using MySql.Data.MySqlClient;
    using UnityEngine;
    public static class MyCommandHelper
    {
        public static void Inject(MyCommand myCommand, MySqlCommand cmd)
        {
            Debug.Log("执行sql:" + myCommand);
            cmd.CommandText = myCommand.CommandText;
            foreach (Parameter parameter in myCommand.Parameters)
            {
                cmd.Parameters.AddWithValue(parameter.Param, parameter.Value);
            }
        }
    }
}