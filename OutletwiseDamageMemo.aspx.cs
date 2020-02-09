using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OutletwiseDamageMemo : System.Web.UI.Page
{
    double testDis = 0;



    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            GetReportData();
        }
        else
        {
            ReportDocument doc = (ReportDocument)Session["ReportDocument"];
            CrystalReportViewer1.ReportSource = doc;
        }


    }




    #region Private Method

    private void GetReportData()
    {
        try
        {
            DataTable FreeTable = new DataTable();
            FreeTable.Columns.Add("ITEM_ID", typeof(string));
            FreeTable.Columns.Add("FREE_CTN", typeof(double));
            FreeTable.Columns.Add("FREE_PCS", typeof(double));

            string dm = Request.QueryString["dm"];
            string sr = Request.QueryString["sr"];
            string orderdate = Request.QueryString["orderdate"];



            string connectionString = ConfigurationManager.ConnectionStrings["OracleDBMain"].ConnectionString;
            OracleConnection conn = new OracleConnection(connectionString); // C#

            conn.Open();
            string damageQuery = @"SELECT T3.FACTOR,T3.TP,T3.DP,T2.OUTLET_NAME,T2.OUTLET_ADDRESS,T2.PROPRITOR_NAME,T2.MOBILE_NUMBER,T5.SR_NAME,T4.COMPANY_FULL_NAME COMPANY_NAME,
                                    T4.COMPANY_ID,T4.GST_NUMBER,T4.ADDRESS COMAPNY_ADDRESS,T1.* FROM T_DAMAGE T1 
                                    INNER JOIN T_OUTLET T2 ON T1.OUTLET_ID=T2.OUTLET_ID 
                                    INNER JOIN T_ITEM T3 ON T1.ITEM_CODE=T3.ITEM_ID
                                    INNER JOIN T_COMPANY T4 ON T3.OWN_COMPANY=T4.COMPANY_ID  
                                    INNER JOIN T_SR_INFO T5 ON T1.SR_ID=T5.SR_ID 
                                    WHERE T1.SR_ID='" + sr.Trim() + "' AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY')";

            OracleCommand damageCommand = new OracleCommand(damageQuery, conn);
            OracleDataAdapter damageadapter = new OracleDataAdapter(damageCommand);

            DataSet dsDamage = new DataSet();
            damageadapter.Fill(dsDamage, "DataTable1");
            int damageCount = dsDamage.Tables[0].Rows.Count;

                if (damageCount > 0 && dsDamage.Tables[0].Rows[0]["SR_ID"].ToString() != "")
                {

                    DataColumn dc = new DataColumn("DM_ID");
                    dc.DataType = typeof(string);
                    dsDamage.Tables[0].Columns.Add(dc);

                    DataColumn dc1 = new DataColumn("DM_NAME");
                    dc1.DataType = typeof(string);
                    dsDamage.Tables[0].Columns.Add(dc1);


                    DataColumn dc2 = new DataColumn("DM_MOBILE");
                    dc2.DataType = typeof(string);
                    dsDamage.Tables[0].Columns.Add(dc2);


                    DataColumn dc3 = new DataColumn("TRAN_ID");
                    dc3.DataType = typeof(string);
                    dsDamage.Tables[0].Columns.Add(dc3);

                    DataColumn dc4 = new DataColumn("DAMAGE");
                    dc4.DataType = typeof(string);
                    dc4.DefaultValue = 0;
                    dsDamage.Tables[0].Columns.Add(dc4);

                    DataColumn dc6 = new DataColumn("ORDER_DATE");
                    dc6.DataType = typeof(string);
                    dsDamage.Tables[0].Columns.Add(dc6);

                    string DM_ID = dm;
                    string DM_NAME = "";
                    string DM_MOBILE = "";

                    string qrDm = @"SELECT * FROM T_DM WHERE DM_ID='" + dm.Trim() + "'";

                    OracleCommand cmDm = new OracleCommand(qrDm, conn);
                    OracleDataAdapter daDm = new OracleDataAdapter(cmDm);

                    DataSet dsDm = new DataSet();
                    daDm.Fill(dsDm);
                    int count = dsDm.Tables[0].Rows.Count;
                    if (count > 0 && dsDm.Tables[0].Rows[0]["DM_ID"].ToString() != "")
                    {
                        DM_NAME = dsDm.Tables[0].Rows[0]["DM_NAME"].ToString();
                        DM_MOBILE = dsDm.Tables[0].Rows[0]["MOBILE_NO"].ToString();
                    }


                    for (int d = 0; d < damageCount; d++)
                    {
                        double damageAmount = 0;
                        string srID = Convert.ToString(dsDamage.Tables[0].Rows[d]["SR_ID"]);
                        string damageItemId = Convert.ToString(dsDamage.Tables[0].Rows[d]["ITEM_CODE"]);
                        string damageItemName = Convert.ToString(dsDamage.Tables[0].Rows[d]["ITEM_NAME"]);
                        string damageCtn = Convert.ToString(dsDamage.Tables[0].Rows[d]["CARTON"]);
                        string damagePcs = Convert.ToString(dsDamage.Tables[0].Rows[d]["PCS"]);
                        string OUTLET_ID = Convert.ToString(dsDamage.Tables[0].Rows[d]["OUTLET_ID"]);
                        string UTLET_NAME = Convert.ToString(dsDamage.Tables[0].Rows[d]["OUTLET_NAME"]);
                        string UTLET_ADDRESS = Convert.ToString(dsDamage.Tables[0].Rows[d]["OUTLET_ADDRESS"]);
                        double fACTOR = Convert.ToDouble(dsDamage.Tables[0].Rows[d]["FACTOR"]);
                        double tp = Convert.ToDouble(dsDamage.Tables[0].Rows[d]["tp"]);

                        damageAmount = (Convert.ToDouble(damageCtn) * fACTOR + Convert.ToDouble(damagePcs)) * tp;


                        DataRow drD = dsDamage.Tables[0].Rows[d];
                        drD[21] = DM_ID;

                        DataRow drDD = dsDamage.Tables[0].Rows[d];
                        drDD[22] = DM_NAME;

                        DataRow drFD = dsDamage.Tables[0].Rows[d];
                        drFD[23] = DM_MOBILE;

                        string TRAN_ID = "";

                        string qrPO = @"SELECT TRAN_ID FROM T_ORDER_HEADER WHERE OUTLET_ID='" + OUTLET_ID.Trim() + "' AND SR_ID='" + srID.Trim() + "' AND ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY')";

                        OracleCommand cmdT = new OracleCommand(qrPO, conn);
                        OracleDataAdapter daT = new OracleDataAdapter(cmdT);

                        DataSet dsT = new DataSet();
                        daT.Fill(dsT);
                        int t = dsT.Tables[0].Rows.Count;
                        if (t > 0 && dsT.Tables[0].Rows[0]["TRAN_ID"].ToString() != "")
                        {
                            TRAN_ID = dsT.Tables[0].Rows[0]["TRAN_ID"].ToString();

                            DataRow dr = dsDamage.Tables[0].Rows[d];
                            dr[24] = TRAN_ID;
                        }

                        DataRow drDamage = dsDamage.Tables[0].Rows[d];
                        drDamage[25] = damageAmount;
                    }
                 
            }

            conn.Close();

            int rowcount = dsDamage.Tables[0].Rows.Count;

            if (rowcount > 0 && dsDamage.Tables[0].Rows[0]["ITEM_CODE"].ToString() != "")
            {
                for (int i = 0; i < rowcount; i++)
                {

                    dsDamage.Tables[0].Rows[i]["ORDER_DATE"] = orderdate;
                }

            }

            var crReport = new ReportDocument();
            crReport.Load(Server.MapPath("RptOutletwiseDamageMemo.rpt"));
            Session["ReportDocument"] = crReport;
            crReport.SetDataSource(dsDamage);

            // Binding the crystalReportViewer with our report object. 
            CrystalReportViewer1.ReportSource = crReport;
            CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }



    #endregion
}