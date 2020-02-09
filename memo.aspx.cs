using CrystalDecisions.CrystalReports.Engine;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class memo : System.Web.UI.Page
{
    public string srid = "";
    public string orderdate = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        srid = Request.QueryString["srid"].ToString();
        orderdate = Request.QueryString["orderdate"].ToString();

        ReportDocument rptDoc = new ReportDocument();
        dsDataSet ds = new dsDataSet(); // .xsd file name
        DataTable dt = new DataTable();

        // Just set the name of data table
        dt.TableName = "Crystal Report Example";
        dt = getAllOrders(); //This function is located below this function
        ds.Tables[0].Merge(dt);

        // Your .rpt file path will be below
        rptDoc.Load(Server.MapPath("CrystalReport2.rpt"));

        //set dataset to the report viewer.
        rptDoc.SetDataSource(ds);
        CrystalReportViewer1.Visible = true;
        CrystalReportViewer1.ReportSource = rptDoc;
    }

    public DataTable getAllOrders()
    {
        DataSet ds = new DataSet();
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
        OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
        try
        {
            conn.Open();

            string querys = @"SELECT T2.TRAN_ID,T1.* FROM
                            (SELECT OUTLET_ID,OUTLET_NAME,OUTLET_ADDRESS,PROPRITOR_NAME,MOBILE_NUMBER FROM T_OUTLET
                            WHERE OUTLET_ID IN(
                            SELECT DISTINCT OUTLET_ID FROM T_TRANSACTION WHERE ENTRY_DATE=TO_DATE('" + orderdate + "','DD/MM/YYYY') AND SR_ID='" + srid + "')) T1, ";
            querys = querys + @"(SELECT * FROM T_ORDER_HEADER WHERE ENTRY_DATE=TO_DATE('" + orderdate + "','DD/MM/YYYY')) T2 WHERE T1.OUTLET_ID=T2.OUTLET_ID";
            
            OracleCommand cmds = new OracleCommand(querys, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmds);
            DataSet dSet = new DataSet();
            da.Fill(dSet);
            int c = dSet.Tables[0].Rows.Count;
            if (c > 0 && dSet.Tables[0].Rows[0]["TRAN_ID"].ToString() != "")
            {
                for (int i = 0; i < c; i++)
                {
                    string OC = dSet.Tables[0].Rows[i]["TRAN_ID"].ToString();
                    string OUTLET_ID = dSet.Tables[0].Rows[i]["OUTLET_ID"].ToString();
                    string OUTLET_NAME = dSet.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                    string OUTLET_ADDRESS = dSet.Tables[0].Rows[i]["OUTLET_ADDRESS"].ToString();
                    string PROPRITOR_NAME = dSet.Tables[0].Rows[i]["PROPRITOR_NAME"].ToString();
                    string MOBILE_NUMBER = dSet.Tables[0].Rows[i]["MOBILE_NUMBER"].ToString();


                }
            }




            da.Fill(ds, "DataTable1");

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (conn.State != ConnectionState.Closed)
                conn.Close();
        }
        return ds.Tables[0];
    }
}