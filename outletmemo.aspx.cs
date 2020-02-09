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

public partial class outletmemo : System.Web.UI.Page
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
            string qrOrderInfo = @"SELECT (T6.SR_NAME||'-'||T6.SR_ID) SR, T6.MOBILE_NO SR_MOBILE_NO,(TOTAL_AMT+GST) AMOUNT_WITH_GST, T5.* FROM
                                 (SELECT T4.ITEM_NAME,T4.DP,T4.TP,T4.FACTOR,T4.VAT GST, (((((T4.FACTOR*T3.ITEM_CTN)+T3.ITEM_QTY)*T3.OUT_PRICE)*T4.VAT)/100) TOTAL_GST, (((T4.FACTOR*T3.ITEM_CTN)+T3.ITEM_QTY)*T3.OUT_PRICE) TOTAL_AMT, T3.* FROM
                                 (SELECT T1.ITEM_ID,T1.SR_ID,T1.ITEM_CTN,T1.ITEM_QTY,T1.OUT_PRICE,T2.OUTLET_ID,T2.OUTLET_NAME,T2.GRADE,T2.OUTLET_ADDRESS,T2.PROPRITOR_NAME,T2.MOBILE_NUMBER,T2.DIVISION_ID,T2.ZONE_ID FROM 
                                 (SELECT DISTINCT OUTLET_ID,ITEM_ID,SR_ID,ITEM_CTN,ITEM_QTY,OUT_PRICE FROM T_ORDER_DETAIL 
                                 WHERE ENTRY_DATE=TO_DATE('" + orderdate + "','DD/MM/YYYY') AND SR_ID='" + sr + "') T1, ";
            qrOrderInfo = qrOrderInfo + @" (SELECT * FROM T_OUTLET) T2
                                 WHERE T1.OUTLET_ID=T2.OUTLET_ID ) T3,
                                 (SELECT * FROM T_ITEM) T4 
                                 WHERE T4.ITEM_ID=T3.ITEM_ID ) T5,
                                 (SELECT SR_ID,SR_NAME,MOBILE_NO FROM T_SR_INFO) T6 
                                 WHERE T6.SR_ID=T5.SR_ID ORDER BY T5.OUTLET_NAME";

            OracleCommand cmdG = new OracleCommand(qrOrderInfo, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmdG);

            DataSet ds = new DataSet();
            da.Fill(ds, "DataTable1");
            int c = ds.Tables[0].Rows.Count;
            if (c > 0 && ds.Tables[0].Rows[0]["SR"].ToString() != "")
            {
                //DataTable dtOrderInfo = new DataTable();


                DataColumn dc = new DataColumn("DM_ID");
                dc.DataType = typeof(string);
                ds.Tables[0].Columns.Add(dc);

                DataColumn dc1 = new DataColumn("DM_NAME");
                dc1.DataType = typeof(string);
                ds.Tables[0].Columns.Add(dc1);


                DataColumn dc2 = new DataColumn("DM_MOBILE");
                dc2.DataType = typeof(string);
                ds.Tables[0].Columns.Add(dc2);


                DataColumn dc3 = new DataColumn("TRAN_ID");
                dc3.DataType = typeof(string);
                ds.Tables[0].Columns.Add(dc3);

                DataColumn dc4 = new DataColumn("DISCOUNT");
                dc4.DataType = typeof(string);
                ds.Tables[0].Columns.Add(dc4);


                DataColumn dc5 = new DataColumn("DAMAGE");
                dc5.DataType = typeof(string);
                dc5.DefaultValue = 0;
                ds.Tables[0].Columns.Add(dc5);

                string DM_ID = dm;
                string DM_NAME = "";
                string DM_MOBILE = "";

                string qrDm = @"SELECT * FROM T_DM WHERE DM_ID='" + dm + "'";

                OracleCommand cmDm = new OracleCommand(qrDm, conn);
                OracleDataAdapter daDm = new OracleDataAdapter(cmDm);

                DataSet dsDm = new DataSet();
                daDm.Fill(dsDm);
                int d = dsDm.Tables[0].Rows.Count;
                if (d > 0 && dsDm.Tables[0].Rows[0]["DM_ID"].ToString() != "")
                {
                    DM_NAME = dsDm.Tables[0].Rows[0]["DM_NAME"].ToString();
                    DM_MOBILE = dsDm.Tables[0].Rows[0]["MOBILE_NO"].ToString();
                }


                for (int i = 0; i < c; i++)
                {


                    DataRow drD = ds.Tables[0].Rows[i];
                    drD[23] = DM_ID;

                    DataRow drDD = ds.Tables[0].Rows[i];
                    drDD[24] = DM_NAME;

                    DataRow drFD = ds.Tables[0].Rows[i];
                    drFD[25] = DM_MOBILE;

                    string TRAN_ID = "";

                    string SR_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string OUTLET_ID = ds.Tables[0].Rows[i]["OUTLET_ID"].ToString();

                    string qrPO = @"SELECT TRAN_ID FROM T_ORDER_HEADER WHERE OUTLET_ID='" + OUTLET_ID + "' AND SR_ID='" + SR_ID + "' AND ENTRY_DATE=TO_DATE('" + orderdate + "','DD/MM/YYYY')";

                    OracleCommand cmdT = new OracleCommand(qrPO, conn);
                    OracleDataAdapter daT = new OracleDataAdapter(cmdT);

                    DataSet dsT = new DataSet();
                    daT.Fill(dsT);
                    int t = dsT.Tables[0].Rows.Count;
                    if (t > 0 && dsT.Tables[0].Rows[0]["TRAN_ID"].ToString() != "")
                    {
                        TRAN_ID = dsT.Tables[0].Rows[0]["TRAN_ID"].ToString();

                        DataRow dr = ds.Tables[0].Rows[i];
                        dr[26] = TRAN_ID;
                    }
                    DataRow drDamage = ds.Tables[0].Rows[i];
                    drDamage[27] = 0;

                }

                //--- END ADDING TRAN ID LOOP 

                //Start Computing Discount

                for (int i = 0; i < c; i++)
                {
                    string iTEM_ID = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    double iTEM_CTN = Convert.ToDouble(ds.Tables[0].Rows[i]["ITEM_CTN"]);
                    double item_PCS = Convert.ToDouble(ds.Tables[0].Rows[i]["ITEM_QTY"]);
                    double factor = Convert.ToDouble(ds.Tables[0].Rows[i]["FACTOR"]);
                    double total_AMT = Convert.ToDouble(ds.Tables[0].Rows[i]["TOTAL_AMT"]);
                    double dp = Convert.ToDouble(ds.Tables[0].Rows[i]["DP"]);
                    string itemDivision = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string itemZone = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();


                    string OUTLET_ID = ds.Tables[0].Rows[i]["OUTLET_ID"].ToString();
                    string OUTLET_NAME = ds.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                    string OUTLET_GRADE = ds.Tables[0].Rows[i]["GRADE"].ToString();
                    string OUTLET_ADDRESS = ds.Tables[0].Rows[i]["OUTLET_ADDRESS"].ToString();


                    string qrDis = @"SELECT * FROM T_TRADE_PROGRAM WHERE ITEM_ID='" + iTEM_ID + "' AND OUTLET_GRADE LIKE '%" + OUTLET_GRADE + "%'  AND  TO_DATE('" + orderdate + "','DD/MM/YYYY') BETWEEN FREE_FROM_DATE AND FREE_TO_DATE AND STATUS='Y' ORDER BY FREE_TYPE ASC,MIN_QTY DESC";


                    OracleCommand cmdDis = new OracleCommand(qrDis, conn);
                    OracleDataAdapter daDis = new OracleDataAdapter(cmdDis);

                    DataSet dsT = new DataSet();
                    daDis.Fill(dsT);

                    int t = dsT.Tables[0].Rows.Count;
                    double disCountAmount = 0;
                    int correctTradeRow = -1;
                    bool foundCorrectRow = false;

                    double disCountQty = 0;

                    if (dsT.Tables[0].Rows.Count > 0)
                    {
                        GetDiscount(dsT, iTEM_CTN, item_PCS,
                           factor, correctTradeRow, disCountAmount, disCountQty, foundCorrectRow, dp, itemDivision, itemZone);

                    }


                    if (testDis > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[i];
                        dr[27] = testDis;
                        testDis = 0;
                    }

                    else
                    {
                        DataRow dr = ds.Tables[0].Rows[i];
                        dr[27] = 0;
                        testDis = 0;
                    }
                }

                //--- END Computing Discount 

                //Starting computing free offer
                for (int i = 0; i < c; i++)
                {
                    string iTEM_ID = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string OUTLET_ID = ds.Tables[0].Rows[i]["OUTLET_ID"].ToString();
                    double iTEM_CTN = Convert.ToDouble(ds.Tables[0].Rows[i]["ITEM_CTN"]);
                    double item_PCS = Convert.ToDouble(ds.Tables[0].Rows[i]["ITEM_QTY"]);
                    double factor = Convert.ToDouble(ds.Tables[0].Rows[i]["FACTOR"]);
                    string OUTLET_NAME = ds.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                    string OUTLET_GRADE = ds.Tables[0].Rows[i]["GRADE"].ToString();
                    string OUTLET_ADDRESS = ds.Tables[0].Rows[i]["OUTLET_ADDRESS"].ToString();
                    string itemDivision = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string itemZone = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();


                    string qrOLAdrress = @"SELECT * FROM T_TRADE_PROGRAM WHERE ITEM_ID='" + iTEM_ID + "'" +
                    " AND OUTLET_GRADE LIKE '%" + OUTLET_GRADE + "%' AND TO_DATE('" + orderdate + "','DD/MM/YYYY') " +
                    " BETWEEN FREE_FROM_DATE AND FREE_TO_DATE AND STATUS='Y' ORDER BY FREE_TYPE ASC,MIN_QTY DESC";

                    OracleCommand cmdOL = new OracleCommand(qrOLAdrress, conn);
                    OracleDataAdapter daOL = new OracleDataAdapter(cmdOL);

                    DataSet dsF = new DataSet();
                    daOL.Fill(dsF);
                    int ol = dsF.Tables[0].Rows.Count;
                    bool foundCorrectRow = false;

                    if (dsF.Tables[0].Rows.Count > 0)
                    {
                        GetFree(dsF, FreeTable, iTEM_CTN, item_PCS, factor, itemDivision, itemZone, foundCorrectRow);

                    }


                    if (FreeTable.Rows.Count > 0)
                    {
                        var newFreeDt = new DataTable();
                        if (FreeTable.Rows.Count > 0)
                        {
                            newFreeDt = FreeTable.AsEnumerable()
                               .GroupBy(r => r.Field<string>("ITEM_ID"))
                               .Select(g =>
                               {
                                   var row = FreeTable.NewRow();

                                   row["ITEM_ID"] = g.Key;
                                   row["FREE_CTN"] = g.Sum(r => r.Field<double>("FREE_CTN"));
                                   row["FREE_PCS"] = g.Sum(r => r.Field<double>("FREE_PCS"));


                                   return row;
                               }).CopyToDataTable();
                        }

                        for (int p = 0; p < newFreeDt.Rows.Count; p++)
                        {
                            DataRow dataRow = null;
                            string freeItemId = Convert.ToString(newFreeDt.Rows[p]["ITEM_ID"]);
                            string freeItemName = "";
                            string FreeCtn = Convert.ToString(newFreeDt.Rows[p]["FREE_CTN"]);
                            string FreePcs = Convert.ToString(newFreeDt.Rows[p]["FREE_PCS"]);

                            string qrFreeItemName = @"SELECT ITEM_NAME FROM T_ITEM WHERE ITEM_ID='" + freeItemId + "'";
                            OracleCommand cmdFF = new OracleCommand(qrFreeItemName, conn);
                            OracleDataAdapter daFF = new OracleDataAdapter(cmdFF);

                            DataSet dsFF = new DataSet();
                            daFF.Fill(dsFF);
                            int s = dsFF.Tables[0].Rows.Count;
                            if (s > 0 && dsFF.Tables[0].Rows[0]["ITEM_NAME"].ToString() != "")
                            {
                                freeItemName = dsFF.Tables[0].Rows[0]["ITEM_NAME"].ToString();
                            }

                            dataRow = ds.Tables[0].NewRow();
                            dataRow["ITEM_ID"] = freeItemId;
                            dataRow["ITEM_NAME"] = freeItemName + "     Auto Free";
                            dataRow["ITEM_CTN"] = FreeCtn;
                            dataRow["ITEM_QTY"] = FreePcs;
                            dataRow["OUTLET_ID"] = OUTLET_ID;
                            dataRow["SR"] = "-";
                            dataRow["SR_MOBILE_NO"] = "-";
                            dataRow["AMOUNT_WITH_GST"] = "0";
                            dataRow["FACTOR"] = "0";
                            dataRow["GST"] = "0";
                            dataRow["TOTAL_GST"] = "0";
                            dataRow["TOTAL_AMT"] = "0";
                            dataRow["OUT_PRICE"] = "0";
                            dataRow["OUTLET_NAME"] = OUTLET_NAME;
                            dataRow["GRADE"] = "-";
                            dataRow["OUTLET_ADDRESS"] = OUTLET_ADDRESS;
                            dataRow["PROPRITOR_NAME"] = "-";
                            dataRow["MOBILE_NUMBER"] = "-";
                            dataRow["DM_ID"] = "-";
                            dataRow["DM_NAME"] = "-";
                            dataRow["DM_MOBILE"] = "-";
                            dataRow["TRAN_ID"] = "-";
                            dataRow["DISCOUNT"] = "0";
                            dataRow["DAMAGE"] = "0";

                            ds.Tables[0].Rows.Add(dataRow);

                        }


                        FreeTable.Clear();
                        newFreeDt.Clear();
                    }

                }

            }

            //start computing Damage here

            string damageQuery = @"SELECT T3.FACTOR,T3.TP,T3.DP,T2.OUTLET_NAME,T2.OUTLET_ADDRESS,T1.* FROM T_DAMAGE T1 
                                    INNER JOIN T_OUTLET T2 ON T1.OUTLET_ID=T2.OUTLET_ID 
                                    INNER JOIN T_ITEM T3 ON T1.ITEM_CODE=T3.ITEM_ID 
                                    WHERE T1.SR_ID='" + sr + "' AND T1.ENTRY_DATE=TO_DATE('" + orderdate + "','DD/MM/YYYY')";

            OracleCommand damageCommand = new OracleCommand(damageQuery, conn);
            OracleDataAdapter damageadapter = new OracleDataAdapter(damageCommand);

            DataSet dsDamage = new DataSet();
            damageadapter.Fill(dsDamage, "damageTable");
            int damageCount = dsDamage.Tables[0].Rows.Count;


            if (damageCount > 0)
            {

                for (int d = 0; d < damageCount; d++)
                {
                    DataRow dataRow = null;
                    double damageAmount = 0;
                    string damageItemId = Convert.ToString(dsDamage.Tables[0].Rows[d]["ITEM_CODE"]);
                    string damageItemName = Convert.ToString(dsDamage.Tables[0].Rows[d]["ITEM_NAME"]);
                    string damageCtn = Convert.ToString(dsDamage.Tables[0].Rows[d]["CARTON"]);
                    string damagePcs = Convert.ToString(dsDamage.Tables[0].Rows[d]["PCS"]);
                    string oUTLET_ID = Convert.ToString(dsDamage.Tables[0].Rows[d]["OUTLET_ID"]);
                    string oUTLET_NAME = Convert.ToString(dsDamage.Tables[0].Rows[d]["OUTLET_NAME"]);
                    string oUTLET_ADDRESS = Convert.ToString(dsDamage.Tables[0].Rows[d]["OUTLET_ADDRESS"]);
                    double fACTOR = Convert.ToDouble(dsDamage.Tables[0].Rows[d]["FACTOR"]);
                    double tp = Convert.ToDouble(dsDamage.Tables[0].Rows[d]["tp"]);

                    damageAmount = (Convert.ToDouble(damageCtn) * fACTOR + Convert.ToDouble(damagePcs)) * tp;


                    dataRow = ds.Tables[0].NewRow();
                    dataRow["ITEM_ID"] = damageItemId;
                    dataRow["ITEM_NAME"] = damageItemName + "     Damage Item";
                    dataRow["ITEM_CTN"] = damageCtn;
                    dataRow["ITEM_QTY"] = damagePcs;
                    dataRow["OUTLET_ID"] = oUTLET_ID;
                    dataRow["SR"] = "-";
                    dataRow["SR_MOBILE_NO"] = "-";
                    dataRow["AMOUNT_WITH_GST"] = "0";
                    dataRow["FACTOR"] = "0";
                    dataRow["GST"] = "0";
                    dataRow["TOTAL_GST"] = "0";
                    dataRow["TOTAL_AMT"] = "0";
                    dataRow["OUT_PRICE"] = tp;
                    dataRow["OUTLET_NAME"] = oUTLET_NAME;
                    dataRow["GRADE"] = "-";
                    dataRow["OUTLET_ADDRESS"] = oUTLET_ADDRESS;
                    dataRow["PROPRITOR_NAME"] = "-";
                    dataRow["MOBILE_NUMBER"] = "-";
                    dataRow["DM_ID"] = "-";
                    dataRow["DM_NAME"] = "-";
                    dataRow["DM_MOBILE"] = "-";
                    dataRow["TRAN_ID"] = "-";
                    dataRow["DISCOUNT"] = "0";
                    dataRow["DAMAGE"] = damageAmount;

                    ds.Tables[0].Rows.Add(dataRow);
                }

            }

            conn.Close();

            int count = ds.Tables[0].Rows.Count;
            double totalGST = 0;
            double totalAmountWithGST = 0;


            if (count > 0 && ds.Tables[0].Rows[0]["ITEM_ID"].ToString() != "")
            {
                for (int i = 0; i < count; i++)
                {
                    double tOTAL_AMT = Convert.ToDouble(ds.Tables[0].Rows[i]["TOTAL_AMT"]);
                    double dISCOUNT = Convert.ToDouble(ds.Tables[0].Rows[i]["DISCOUNT"]);
                    double dAMAGE = Convert.ToDouble(ds.Tables[0].Rows[i]["DAMAGE"]);
                    double gST = Convert.ToDouble(ds.Tables[0].Rows[i]["GST"]);
                    totalGST = ((tOTAL_AMT - dISCOUNT) * gST) / 100;
                    totalAmountWithGST = tOTAL_AMT - dISCOUNT + totalGST - dAMAGE;

                    ds.Tables[0].Rows[i]["TOTAL_GST"] = totalGST;
                    ds.Tables[0].Rows[i]["AMOUNT_WITH_GST"] = totalAmountWithGST;
                    ds.Tables[0].Rows[i]["GRADE"] = orderdate;
                }

            }

            var crReport = new ReportDocument();
            crReport.Load(Server.MapPath("OutletMemo.rpt"));
            Session["ReportDocument"] = crReport;
            crReport.SetDataSource(ds);

            // Binding the crystalReportViewer with our report object. 
            CrystalReportViewer1.ReportSource = crReport;
            CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
        }

        catch (Exception ex)
        {

        }
    }


    private void GetDiscount(DataSet dsTrade, double iTEM_CTN, double item_PCS,
       double factor, int correctTradeRow, double discountAmount, double disCountQty, bool foundCorrectRow, double dp, string itemDivision, string itemZone)
    {

        for (int h = 0; h < dsTrade.Tables[0].Rows.Count; h++)
        {
            string divisionID = dsTrade.Tables[0].Rows[h]["DIVISION_ID"].ToString();
            string zoneID = dsTrade.Tables[0].Rows[h]["ZONE_ID"].ToString();
            string freeType = Convert.ToString(dsTrade.Tables[0].Rows[h]["FREE_TYPE"]);
            double freeCTN = Convert.ToDouble(dsTrade.Tables[0].Rows[h]["FREE_CTN"]);
            double min_QTY_DISCOUNT = Convert.ToDouble(dsTrade.Tables[0].Rows[h]["MIN_QTY_DISCOUNT"]);
            double min_Qty_Free = Convert.ToDouble(dsTrade.Tables[0].Rows[h]["MIN_QTY"]);

            if ((divisionID.ToLower().Contains("ALL".ToLower()) && zoneID.ToLower().Contains("ALL".ToLower())) || (divisionID == itemDivision
                       && zoneID == itemZone) || (itemDivision == divisionID && zoneID.ToLower().Contains("ALL".ToLower())))
            {
                if (freeType == "Carton")
                {
                    if (min_QTY_DISCOUNT < min_Qty_Free || iTEM_CTN < min_QTY_DISCOUNT)
                    {

                        if (freeCTN > 0)
                        {
                            double min_Qty = Convert.ToDouble(dsTrade.Tables[0].Rows[h]["MIN_QTY"]);

                            if (iTEM_CTN >= min_Qty)
                            {
                                double disQty = iTEM_CTN / min_Qty;
                                disCountQty = Math.Round((disQty - Math.Truncate(disQty)) * min_Qty);
                                iTEM_CTN = disCountQty;
                                correctTradeRow = h;
                                break;
                            }
                        }

                        else
                        {
                            disCountQty = iTEM_CTN;
                            correctTradeRow = h;

                        }

                    }

                    else
                    {
                        double min_Qty = Convert.ToDouble(dsTrade.Tables[0].Rows[h]["MIN_QTY_DISCOUNT"]);

                        if (iTEM_CTN >= min_Qty)
                        {
                            //double disQty = iTEM_CTN / min_Qty;
                            //disCountQty = Math.Truncate(disQty) * min_Qty;
                            //iTEM_CTN = (disQty - Math.Truncate(disQty)) * min_Qty;

                            //disCountQty = iTEM_CTN;

                            disCountQty = (iTEM_CTN * factor) + item_PCS;

                            iTEM_CTN = 0;

                            correctTradeRow = h;
                            break;

                        }
                    }



                }

                else if (freeType == "Piece")
                {

                    double total_Item_PCS = (iTEM_CTN * factor) + item_PCS;


                    if (min_QTY_DISCOUNT < min_Qty_Free || total_Item_PCS < min_QTY_DISCOUNT)
                    {
                        int min_Qty = Convert.ToInt16(dsTrade.Tables[0].Rows[h]["MIN_QTY"]);


                        if (total_Item_PCS >= min_Qty)
                        {
                            double disQty = total_Item_PCS / min_Qty;
                            disCountQty = (disQty - Math.Truncate(disQty)) * min_Qty;

                            total_Item_PCS = disCountQty;
                            iTEM_CTN = Math.Round(Math.Truncate(total_Item_PCS / factor));
                            item_PCS = total_Item_PCS - (factor * iTEM_CTN);

                            correctTradeRow = h;
                            break;

                        }

                    }
                    else
                    {
                        double min_Qty = Convert.ToDouble(dsTrade.Tables[0].Rows[h]["MIN_QTY_DISCOUNT"]);

                        if (total_Item_PCS >= min_Qty)
                        {
                            disCountQty = total_Item_PCS;
                            iTEM_CTN = 0;
                            item_PCS = 0;

                            correctTradeRow = h;
                            break;

                        }
                    }


                }

            }

            else
            {
                testDis = 0;
            }

            if (h == dsTrade.Tables[0].Rows.Count - 1)
            {
                foundCorrectRow = true;
            }


        }

        if (foundCorrectRow == false && disCountQty > 0)
        {
            GetDiscount(dsTrade, iTEM_CTN, item_PCS, factor,
                correctTradeRow, discountAmount, disCountQty, foundCorrectRow, dp, itemDivision, itemZone);

        }

        string DISCOUNT_TYPE = "";

        if (correctTradeRow >= 0)
        {
            DISCOUNT_TYPE = Convert.ToString(dsTrade.Tables[0].Rows[correctTradeRow]["DISCOUNT_TYPE"]);
        }

        if (DISCOUNT_TYPE.ToLower() == "Percent".ToLower() && testDis == 0)
        {
            double mIN_QTY_DISCOUNT = Convert.ToDouble(dsTrade.Tables[0].Rows[correctTradeRow]["MIN_QTY_DISCOUNT"]);
            double mIN_QTY_FREE = Convert.ToDouble(dsTrade.Tables[0].Rows[correctTradeRow]["MIN_QTY"]);

            if (mIN_QTY_DISCOUNT <= disCountQty && mIN_QTY_DISCOUNT > 0)
            {
                double freeCtn = Convert.ToDouble(dsTrade.Tables[0].Rows[correctTradeRow]["FREE_CTN"]);
                double freePcs = Convert.ToDouble(dsTrade.Tables[0].Rows[correctTradeRow]["FREE_PCS"]);

                if (mIN_QTY_FREE > mIN_QTY_DISCOUNT)
                {
                    if (freeCtn > 0 || freePcs > 0)
                    {
                        testDis = (((freeCtn * factor) + item_PCS) * dp * (Convert.ToDouble(dsTrade.Tables[0].Rows[correctTradeRow]["DISCOUNT"]))) / 100;

                    }
                    else
                    {
                        //testDis = (disCountQty * factor * dp * (Convert.ToDouble(dsTrade.Tables[0].Rows[correctTradeRow]["DISCOUNT"]))) / 100;

                        testDis = (((disCountQty * factor) + item_PCS) * dp * (Convert.ToDouble(dsTrade.Tables[0].Rows[correctTradeRow]["DISCOUNT"]))) / 100;

                    }

                }
                else
                {
                    testDis = (disCountQty * dp * (Convert.ToDouble(dsTrade.Tables[0].Rows[correctTradeRow]["DISCOUNT"]))) / 100;
                }


            }
            else
            {
                testDis = 0;
            }
        }

        else if (DISCOUNT_TYPE.ToLower() == "Money".ToLower() && testDis == 0)
        {
            double mIN_QTY_DISCOUNT = Convert.ToDouble(dsTrade.Tables[0].Rows[correctTradeRow]["MIN_QTY_DISCOUNT"]);
            double freeCtn = Convert.ToDouble(dsTrade.Tables[0].Rows[correctTradeRow]["FREE_CTN"]);
            double freePcs = Convert.ToDouble(dsTrade.Tables[0].Rows[correctTradeRow]["FREE_CTN"]);

            if (mIN_QTY_DISCOUNT <= disCountQty && mIN_QTY_DISCOUNT > 0)
            {
                if (freeCtn > 0 || freePcs > 0)
                {
                    //testDis = (((freeCtn * factor) + freePcs) * dp) - Convert.ToDouble(dsTrade.Tables[0].Rows[correctTradeRow]["DISCOUNT"]);
                    testDis = Convert.ToDouble(dsTrade.Tables[0].Rows[correctTradeRow]["DISCOUNT"]);

                }
                else
                {
                    //testDis = (((disCountQty * factor) + freePcs) * dp) - Convert.ToDouble(dsTrade.Tables[0].Rows[correctTradeRow]["DISCOUNT"]);
                    testDis = Convert.ToDouble(dsTrade.Tables[0].Rows[correctTradeRow]["DISCOUNT"]);
                }

            }

            else
            {
                testDis = 0;
            }

        }


    }


    private void GetFree(DataSet dsTrade, DataTable freeTable, double iTEM_CTN, double item_PCS,
        double factor, string itemDivision, string itemZone, bool foundCorrectRow)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["OracleDBMain"].ConnectionString;
        OracleConnection conn = new OracleConnection(connectionString);


        for (int h = 0; h < dsTrade.Tables[0].Rows.Count; h++)
        {
            string divisionID = dsTrade.Tables[0].Rows[h]["DIVISION_ID"].ToString();
            string zoneID = dsTrade.Tables[0].Rows[h]["ZONE_ID"].ToString();
            string freeType = Convert.ToString(dsTrade.Tables[0].Rows[h]["FREE_TYPE"]);
            double min_QTY_DISCOUNT = Convert.ToDouble(dsTrade.Tables[0].Rows[h]["MIN_QTY_DISCOUNT"]);
            double min_Qty_Free = Convert.ToDouble(dsTrade.Tables[0].Rows[h]["MIN_QTY"]);


            if ((divisionID.ToLower().Contains("ALL".ToLower()) && zoneID.ToLower().Contains("ALL".ToLower())) || (divisionID == itemDivision
                            && zoneID == itemZone) || (itemDivision == divisionID && zoneID.ToLower().Contains("ALL".ToLower())))
            {
                if (freeType == "Carton")
                {
                    if (min_QTY_DISCOUNT < min_Qty_Free || iTEM_CTN < min_QTY_DISCOUNT)
                    {


                        if (Convert.ToDouble(dsTrade.Tables[0].Rows[h]["FREE_CTN"]) > 0 || Convert.ToDouble(dsTrade.Tables[0].Rows[h]["FREE_PCS"]) > 0)
                        {
                            double min_Qty = Convert.ToDouble(dsTrade.Tables[0].Rows[h]["MIN_QTY"]);


                            if (iTEM_CTN >= min_Qty)
                            {
                                double disQty = iTEM_CTN / min_Qty;

                                double freeFactor = Math.Truncate(disQty);

                                iTEM_CTN = Math.Round((disQty - Math.Truncate(disQty)) * min_Qty);

                                string[] arrayFreeItems = dsTrade.Tables[0].Rows[h]["FREE_ITEM_ID"].ToString().Split(',');
                                string[] arrayFreeCtn = dsTrade.Tables[0].Rows[h]["FREE_CTN"].ToString().Split(',');
                                string[] arrayFreePcs = dsTrade.Tables[0].Rows[h]["FREE_PCS"].ToString().Split(',');


                                if (arrayFreeItems.Length > 0)
                                {
                                    string freeItemId = "";
                                    double freeCtn = 0;
                                    double freePcs = 0;

                                    DataRow dataRow = null;


                                    for (int k = 0; k < arrayFreeItems.Length; k++)
                                    {
                                        freeItemId = arrayFreeItems[k].ToString().Trim();

                                        if (arrayFreeItems.Length == arrayFreeCtn.Length)
                                        {
                                            double freeNumber = Convert.ToDouble(arrayFreeCtn[k]);
                                            freeCtn = freeFactor * freeNumber;

                                        }

                                        else
                                        {
                                            double freeNumber = Convert.ToDouble(arrayFreeCtn[0]);
                                            freeCtn = freeFactor * freeNumber;

                                        }

                                        if (arrayFreeItems.Length == arrayFreePcs.Length)
                                        {
                                            double freeNumber = Convert.ToDouble(arrayFreePcs[k]);
                                            freePcs = freeFactor * freeNumber;

                                        }
                                        else
                                        {
                                            double freeNumber = Convert.ToDouble(arrayFreePcs[k]);
                                            freePcs = freeFactor * freeNumber;
                                        }

                                        dataRow = freeTable.NewRow();
                                        dataRow["ITEM_ID"] = freeItemId;
                                        dataRow["FREE_CTN"] = freeCtn;
                                        dataRow["FREE_PCS"] = freePcs;

                                        freeTable.Rows.Add(dataRow);
                                    }


                                }

                                break;
                            }

                        }
                    }
                    else
                    {
                        ////If Min_Qty_Discount is greater no free will be added
                    }

                }

                else if (freeType == "Piece")
                {
                    double min_Qty = Convert.ToDouble(dsTrade.Tables[0].Rows[h]["MIN_QTY"]);
                    double total_Item_PCS = (iTEM_CTN * factor) + item_PCS;

                    if (min_QTY_DISCOUNT < min_Qty_Free || total_Item_PCS < min_QTY_DISCOUNT)
                    {


                        if (total_Item_PCS >= min_Qty)
                        {
                            double disQty = total_Item_PCS / min_Qty;

                            double freeFactor = Math.Truncate(disQty);

                            total_Item_PCS = (disQty - Math.Truncate(disQty)) * min_Qty;
                            iTEM_CTN = Math.Truncate(total_Item_PCS / factor);

                            item_PCS = total_Item_PCS - (factor * iTEM_CTN);

                            string[] arrayFreeItems = dsTrade.Tables[0].Rows[h]["FREE_ITEM_ID"].ToString().Split(',');
                            string[] arrayFreeCtn = dsTrade.Tables[0].Rows[h]["FREE_CTN"].ToString().Split(',');
                            string[] arrayFreePcs = dsTrade.Tables[0].Rows[h]["FREE_PCS"].ToString().Split(',');


                            if (arrayFreeItems.Length > 0)
                            {
                                string freeItemId = "";
                                double freeCtn = 0;
                                double freePcs = 0;

                                DataRow dataRow = null;


                                for (int k = 0; k < arrayFreeItems.Length; k++)
                                {
                                    freeItemId = arrayFreeItems[k].ToString().Trim();

                                    if (arrayFreeItems.Length == arrayFreeCtn.Length)
                                    {
                                        double freeNumber = Convert.ToDouble(arrayFreeCtn[k]);
                                        freeCtn = freeFactor * freeNumber;

                                    }

                                    else
                                    {
                                        double freeNumber = Convert.ToDouble(arrayFreeCtn[0]);
                                        freeCtn = freeFactor * freeNumber;

                                    }

                                    if (arrayFreeItems.Length == arrayFreePcs.Length)
                                    {
                                        double freeNumber = Convert.ToDouble(arrayFreePcs[k]);
                                        freePcs = freeFactor * freeNumber;

                                    }
                                    else
                                    {
                                        double freeNumber = Convert.ToDouble(arrayFreePcs[k]);
                                        freePcs = freeFactor * freeNumber;
                                    }

                                    dataRow = freeTable.NewRow();
                                    dataRow["ITEM_ID"] = freeItemId;
                                    dataRow["FREE_CTN"] = freeCtn;
                                    dataRow["FREE_PCS"] = freePcs;

                                    freeTable.Rows.Add(dataRow);
                                }


                            }

                            break;
                        }
                    }

                    else
                    {
                        ////If Min_Qty_Discount is greater no free will be added
                    }


                }


            }

            if (h == dsTrade.Tables[0].Rows.Count - 1)
            {
                foundCorrectRow = true;
            }


        }

        if (foundCorrectRow == false)
        {
            GetFree(dsTrade, freeTable, iTEM_CTN, item_PCS, factor, itemDivision, itemZone, foundCorrectRow);

        }


    }


    #endregion 
}