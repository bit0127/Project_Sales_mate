using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //ReportViewer1.ProcessingMode = ProcessingMode.Local;
            //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report7.rdlc");

            //Customers dsCustomers = GetData();
            //ReportDataSource datasource = new ReportDataSource("Customers", dsCustomers.Tables[0]);
            //ReportViewer1.LocalReport.DataSources.Clear();
            //ReportViewer1.LocalReport.DataSources.Add(datasource);            
        }
    }
 
 
    private Customers GetData()
    {
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
        OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
        conn.Open();
        string sql = "SELECT * FROM T_REPORT";
        OracleCommand cmd = new OracleCommand(sql, conn);
        //OracleDataAdapter da = new OracleDataAdapter(cmd);

        //da.Fill(dsReport, "DataTable1");

        //conn.Close();

       
            using ( OracleDataAdapter da = new OracleDataAdapter())
            {
                cmd.Connection = conn;

                da.SelectCommand = cmd;
                using (Customers dsCustomers = new Customers())
                {
                    da.Fill(dsCustomers, "DataTable1");
                    return dsCustomers;
                }
            }
       
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        ReportViewer1.ProcessingMode = ProcessingMode.Local;
        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report7.rdlc");

        Customers dsCustomers = GetData();
        ReportDataSource datasource = new ReportDataSource("Customers", dsCustomers.Tables[0]);
        ReportViewer1.LocalReport.DataSources.Clear();
        ReportViewer1.LocalReport.DataSources.Add(datasource);      
    }
}