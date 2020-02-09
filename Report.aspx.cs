using CrystalDecisions.CrystalReports.Engine;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

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

            string querys = @"SELECT SR_NAME NAME,EMAIL_ADDRESS ADDRESS FROM T_SR_INFO";
            OracleCommand cmds = new OracleCommand(querys, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmds);

            da.Fill(ds,"DataTable1");

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