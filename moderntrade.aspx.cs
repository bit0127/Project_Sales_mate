using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class moderntrade : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    [WebMethod(EnableSession = true)]
    public static string AddCustomerInfo(string customerName, string phone, string address, string email, string outlet, string zone, string remarks)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string querys = @"INSERT INTO T_MODERN_TRADE(CUSTOMER_NAME,PHONE,ADDRESS,EMAIL,OUTLET,ZONE,REMARKS,ENTRY_DATE)
                            VALUES ('" + customerName + "','" + phone + "','" + address + "','" + email + "','" + outlet + "','" + zone + "','" + remarks + "',TO_DATE(SYSDATE))";
                OracleCommand cmds = new OracleCommand(querys, conn);

                int cs = cmds.ExecuteNonQuery();
                if (cs > 0)
                {
                    msg = "Successful!";
                }
                else
                {
                    msg = "Not Successful";
                }
           
             
            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }  
    
  
                    
    [WebMethod(EnableSession = true)]
    public static string GetCustomerinfo(string dtes)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT * FROM T_MODERN_TRADE WHERE ENTRY_DATE=TO_DATE('" + dtes + "','DD/MM/YYYY')";                             
             
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string CUSTOMER_NAME = ds.Tables[0].Rows[i]["CUSTOMER_NAME"].ToString();
                    string PHONE = ds.Tables[0].Rows[i]["PHONE"].ToString();
                    string ADDRESS = ds.Tables[0].Rows[i]["ADDRESS"].ToString();
                    string EMAIL = ds.Tables[0].Rows[i]["EMAIL"].ToString();
                    string OUTLET = ds.Tables[0].Rows[i]["OUTLET"].ToString();
                    string ZONE = ds.Tables[0].Rows[i]["ZONE"].ToString();
                    string REMARKS = ds.Tables[0].Rows[i]["REMARKS"].ToString();
                    string ENTRY_DATE = Convert.ToDateTime(ds.Tables[0].Rows[i]["ENTRY_DATE"].ToString()).ToShortDateString();

                    msg = msg + ";" + CUSTOMER_NAME + ";" + PHONE + ";" + ADDRESS + ";" + EMAIL + ";" + OUTLET + ";" + ZONE + ";" + REMARKS + ";" + ENTRY_DATE;
                }

            }
            else
            {
                msg = "Not Exist";
            }
 
           
             
            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }   

}