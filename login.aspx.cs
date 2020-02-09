using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class login : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    //[WebMethod(EnableSession = true)]
    //public static string CheckUserData(string uid, string pwd)
    //{
    //    string msg = "";
    //    OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

    //    try
    //    {
    //        OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
    //        conn.Open();

    //        string query = @"SELECT USER_ID,USER_NAME,USER_TYPE,ITEM_GROUP_ID FROM T_USER WHERE USER_ID='" + uid + "' AND USER_PWD='" + pwd + "'";
    //        OracleCommand cmd = new OracleCommand(query, conn);
    //        OracleDataAdapter da = new OracleDataAdapter(cmd);
    //        DataSet ds = new DataSet();
    //        da.Fill(ds);
    //        int i = ds.Tables[0].Rows.Count;
    //        if (i > 0)
    //        {
    //            msg = ds.Tables[0].Rows[0]["USER_TYPE"].ToString();
    //            HttpContext.Current.Session["user_type"] = msg;
    //            HttpContext.Current.Session["item_group"] = ds.Tables[0].Rows[0]["ITEM_GROUP_ID"].ToString();
    //            HttpContext.Current.Session["user_name"] = ds.Tables[0].Rows[0]["USER_NAME"].ToString();
    //            HttpContext.Current.Session["userid"] = ds.Tables[0].Rows[0]["USER_ID"].ToString();

    //            string qr = @"INSERT INTO T_USER_LOG(USER_ID,USER_NAME,LOGIN_DATE,LOGIN_TIME,LOGIN_VIA)" +
    //                       "VALUES('" + uid + "','" + HttpContext.Current.Session["user_name"].ToString() + "',TO_DATE(SYSDATE),SYSDATE,'WEB')";
    //            OracleCommand cmdU = new OracleCommand(qr, conn);
    //            int k = cmdU.ExecuteNonQuery();

    //        }
    //        else
    //        {
    //            msg = "You are not permitted";
    //        }

    //        conn.Close();
    //    }
    //    catch (Exception ex) {
    //        msg = ex.ToString();
    //    }

    //    return msg;
    //}

   [WebMethod(EnableSession = true)]
    public static string CheckUserData(string uid, string pwd)
    {
        
       string msg = ""; 

       try
       {
        DataSet ds = new DataSet();
        string queryString = @"SELECT USER_ID,USER_NAME,USER_TYPE FROM T_USER WHERE USER_ID='" + uid + "' AND USER_PWD='" + pwd + "'";

        ds = CommonDBSvc.GetDataSet(queryString);
        
           int i = ds.Tables[0].Rows.Count;
                if (i > 0)
                {
                    msg = ds.Tables[0].Rows[0]["USER_TYPE"].ToString();
                    HttpContext.Current.Session["user_type"] = msg;
                    //HttpContext.Current.Session["item_group"] = ds.Tables[0].Rows[0]["ITEM_GROUP_ID"].ToString();
                    HttpContext.Current.Session["user_name"] = ds.Tables[0].Rows[0]["USER_NAME"].ToString();
                    HttpContext.Current.Session["userid"] = ds.Tables[0].Rows[0]["USER_ID"].ToString();

                    //string qr = @"INSERT INTO T_USER_LOG(USER_ID,USER_NAME,LOGIN_DATE,LOGIN_TIME,LOGIN_VIA)" +
                    //           "VALUES('" + uid + "','" + HttpContext.Current.Session["user_name"].ToString() + "',TO_DATE(SYSDATE),SYSDATE,'WEB')";
                    //OracleCommand cmdU = new OracleCommand(qr, conn);
                    //int k = cmdU.ExecuteNonQuery();

                }
                else
                {
                    msg = "You are not permitted";
                }


            }
            catch (Exception ex) 
            {
                msg = ex.ToString();
            }

            return msg;
    }

}