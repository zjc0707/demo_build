namespace Jc.SqlTool.Core.Access
{
    using System;
    using System.Data;
    using MySql.Data.MySqlClient;
    using UnityEngine;
    using MyCommand;
    using MyCommand.Helper;
    public class SqlAccess
    {
        private MySqlConnection mySqlConnection;

        public SqlAccess()
        {
            this.OpenSql();
        }
        public void OpenSql()
        {
            try
            {
                string connectionString = string.Format("DataSource={0};Database={1};UserId={2};Password={3};port={4};charset=utf8",
                    SqlConfig.DATA_SOURCE, SqlConfig.DATABASE, SqlConfig.USER_ID, SqlConfig.PASSWORD, SqlConfig.PORT);
                mySqlConnection = new MySqlConnection(connectionString);
                Debug.Log("数据库连接成功");
            }
            catch (Exception e)
            {
                throw new Exception("数据库连接失败：" + e.ToString());
            }
        }
        public DataTable ExecuteQuery(MyCommand myCmd)
        {
            DataTable rs = null;
            //可能上次操作关闭失败，先关闭再打开避免后面操作卡住
            if (mySqlConnection.State == ConnectionState.Open)
            {
                mySqlConnection.Close();
            }
            try
            {
                mySqlConnection.Open();
                Debug.Log("打开链接");
                MySqlCommand cmd = mySqlConnection.CreateCommand();
                MyCommandHelper.Inject(myCmd, cmd);
                DataSet ds = new DataSet();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                rs = ds.Tables[0];
            }
            catch (Exception ee)
            {
                throw new Exception("SQL:" + myCmd.ToString() + "/n" + ee.Message.ToString());
            }
            finally
            {
                Debug.Log("关闭链接");
                mySqlConnection.Close();
            }
            return rs;
        }
        public bool ExecuteNonQuery(MyCommand myCmd)
        {
            bool flag = false;
            try
            {
                mySqlConnection.Open();
                Debug.Log("打开链接");
                MySqlCommand cmd = mySqlConnection.CreateCommand();
                MyCommandHelper.Inject(myCmd, cmd);
                flag = cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception ee)
            {
                throw new Exception("SQL:" + myCmd.ToString() + "/n" + ee.Message.ToString());
            }
            finally
            {
                Debug.Log("关闭链接");
                mySqlConnection.Close();
            }
            return flag;
        }
    }
}