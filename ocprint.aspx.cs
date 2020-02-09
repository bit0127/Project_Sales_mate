using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ocprint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        ddlCountry.Items.Clear();
        ListItem newItemC = new ListItem();
        newItemC.Text = "---Select---";
        newItemC.Value = "0";
        ddlCountry.Items.Add(newItemC);
        
            newItemC = new ListItem();
            newItemC.Text = "Bangladesh";
            newItemC.Value = "Bangladesh";
            ddlCountry.Items.Add(newItemC);

            newItemC = new ListItem();
            newItemC.Text = "India";
            newItemC.Value = "India";
            ddlCountry.Items.Add(newItemC);

            newItemC = new ListItem();
            newItemC.Text = "Malaysia";
            newItemC.Value = "Malaysia";
            ddlCountry.Items.Add(newItemC);

            newItemC = new ListItem();
            newItemC.Text = "Nepal";
            newItemC.Value = "Nepal";
            ddlCountry.Items.Add(newItemC);

            newItemC = new ListItem();
            newItemC.Text = "Oman";
            newItemC.Value = "Oman";
            ddlCountry.Items.Add(newItemC);

            newItemC = new ListItem();
            newItemC.Text = "UAE";
            newItemC.Value = "UAE";
            ddlCountry.Items.Add(newItemC);


        //---load division------------------------------------------------------------------------------
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string countryName = ddlCountry.Text.Trim();

            string selectSQL = "SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION";

            OracleCommand cmd = new OracleCommand(selectSQL, conn);
            OracleDataReader reader;

            try
            {
                ddlDivision.Items.Clear();
                ListItem newItem = new ListItem();
                newItem.Text = "---Select---";
                newItem.Value = "0";
                ddlDivision.Items.Add(newItem);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    newItem = new ListItem();
                    newItem.Text = reader["DIVISION_NAME"].ToString();
                    newItem.Value = reader["DIVISION_ID"].ToString();
                    ddlDivision.Items.Add(newItem);
                }
                reader.Close();
            }
            catch (Exception err)
            {
                //TODO
            }
            finally
            {
                conn.Close();
            }
        
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        ReportViewer1.ProcessingMode = ProcessingMode.Local;
        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report7.rdlc");

        Customers dsCustomers = GetData();
        ReportDataSource datasource = new ReportDataSource("Customers", dsCustomers.Tables[0]);
        ReportViewer1.LocalReport.DataSources.Clear();
        ReportViewer1.LocalReport.DataSources.Add(datasource); 
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


        using (OracleDataAdapter da = new OracleDataAdapter())
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
}