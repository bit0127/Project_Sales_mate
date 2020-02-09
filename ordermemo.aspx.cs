using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ordermemo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod(EnableSession = true)]
    public static string GetDistributor(string company)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT DIST_ID,DIST_NAME FROM T_DISTRIBUTOR
                         WHERE STATUS='Y' AND ITEM_GROUP IN(
                         SELECT ITEM_GROUP_ID FROM T_ITEM_GROUP WHERE COMPANY_ID='" + company + "' AND STATUS='Y')";

            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["DIST_ID"].ToString() != "")
            {
                for (int j = 0; j < i; j++)
                {
                    string distId = ds.Tables[0].Rows[j]["DIST_ID"].ToString();
                    string distName = ds.Tables[0].Rows[j]["DIST_NAME"].ToString();
                    msg = msg + ";" + distId + ";" + distName;
                }
            }

            conn.Close();

            

        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod(EnableSession = true)]
    public static string GetSR(string dist)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT SR_ID,SR_NAME FROM T_SR_INFO WHERE DIST_ID='" + dist + "' AND STATUS='Y'";

            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["SR_ID"].ToString() != "")
            {
                for (int j = 0; j < i; j++)
                {
                    string SR_ID = ds.Tables[0].Rows[j]["SR_ID"].ToString();
                    string SR_NAME = ds.Tables[0].Rows[j]["SR_NAME"].ToString();
                    msg = msg + ";" + SR_ID + ";" + SR_NAME;
                }
            }

            conn.Close();



        }
        catch (Exception ex) { }

        return msg;
    }  


}