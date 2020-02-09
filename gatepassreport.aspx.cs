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

public partial class gatepassreport : System.Web.UI.Page
{
    public double testDis = 0;
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            try
            {
                string dm = Request.QueryString["dm"];
                string orderdate = Request.QueryString["orderdate"];
                                
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDBMain"].ConnectionString;
                OracleConnection conn = new OracleConnection(connectionString); // C#

                conn.Open();
                string qrOrderInfo = @"SELECT T8.DM_ID,T8.DM_NAME,
                                 T7.*,T7.OUTLET_NAME OUTLET,(T7.SR_ID ||'-'||T7.SR_NAME)SR FROM
                                 (SELECT T6.SR_NAME,(TOTAL_AMT+TOTAL_GST) AMOUNT_WITH_GST,T5.* FROM (SELECT T4.ITEM_NAME,T4.DP,T4.TP,T4.FACTOR, 
                                 T4.VAT GST, (((((T4.FACTOR*T3.ITEM_CTN)+T3.ITEM_QTY)*T3.OUT_PRICE)*T4.VAT)/100) TOTAL_GST,(((T4.FACTOR*T3.ITEM_CTN)+T3.ITEM_QTY)*T3.OUT_PRICE) TOTAL_AMT, T3.* FROM
                                 (SELECT T1.TRAN_ID,T1.ITEM_ID,T1.ITEM_CTN,T1.ITEM_QTY,T1.OUT_PRICE,
                                 T2.OUTLET_ID,T2.GRADE,T2.OUTLET_ADDRESS,T2.OUTLET_NAME,T1.SR_ID,T2.DIVISION_ID,T2.ZONE_ID FROM 
                                 (SELECT DISTINCT TRAN_ID, ITEM_ID,OUTLET_ID,SR_ID,ITEM_CTN,ITEM_QTY,
                                 OUT_PRICE FROM T_ORDER_DETAIL 
                                 WHERE ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') AND SR_ID IN (SELECT SR_ID as SR1 FROM T_DM_SR WHERE DM_ID='" + dm.Trim() + "') ) T1, ";
                qrOrderInfo = qrOrderInfo + @"(SELECT OUTLET_ID,OUTLET_NAME,OUTLET_ADDRESS,ZONE_ID,DIVISION_ID,GRADE FROM T_OUTLET) T2
                                 WHERE T1.OUTLET_ID=T2.OUTLET_ID ) T3,
                                 (SELECT ITEM_ID,ITEM_NAME,FACTOR,VAT,TP,DP FROM T_ITEM) T4 
                                 WHERE T4.ITEM_ID=T3.ITEM_ID) T5,                                
                                 (SELECT SR_ID,SR_NAME FROM T_SR_INFO) T6 WHERE T5.SR_ID=T6.SR_ID) T7,
                                (SELECT DM_ID,DM_NAME FROM T_DM) T8 WHERE DM_ID='" + dm.Trim() + "'";

                OracleCommand cmdG = new OracleCommand(qrOrderInfo, conn);
                OracleDataAdapter da = new OracleDataAdapter(cmdG);
                DataSet ds = new DataSet();
                da.Fill(ds, "DataTable1");
                int c = ds.Tables[0].Rows.Count;

                DataColumn dcOrderInfo = null;
                dcOrderInfo = new DataColumn();
                dcOrderInfo.ColumnName = "DISCOUNT";
                ds.Tables[0].Columns.Add(dcOrderInfo);


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


                    string qrDis = @"SELECT * FROM T_TRADE_PROGRAM WHERE ITEM_ID='" + iTEM_ID.Trim() + "' AND OUTLET_GRADE LIKE '%" + OUTLET_GRADE.Trim() + "%'  AND  TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') BETWEEN FREE_FROM_DATE AND FREE_TO_DATE AND STATUS='Y' ORDER BY MIN_QTY DESC";


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
                        dr[25] = testDis;
                        testDis = 0;
                    }

                    else
                    {
                        DataRow dr = ds.Tables[0].Rows[i];
                        dr[25] = 0;
                        testDis = 0;
                    }
                }


                int count = ds.Tables[0].Rows.Count;
                double totalGST = 0;
                double totalAmountWithGST = 0;

                if (count > 0 && ds.Tables[0].Rows[0]["ITEM_ID"].ToString() != "")
                {
                    for (int i = 0; i < count; i++)
                    {
                        double tOTAL_AMT = Convert.ToDouble(ds.Tables[0].Rows[i]["TOTAL_AMT"]);
                        double dISCOUNT = Convert.ToDouble(ds.Tables[0].Rows[i]["DISCOUNT"]);
                        double gST = Convert.ToDouble(ds.Tables[0].Rows[i]["GST"]);
                        totalGST = ((tOTAL_AMT - dISCOUNT) * gST) / 100;
                        totalAmountWithGST = tOTAL_AMT - dISCOUNT + totalGST;

                        ds.Tables[0].Rows[i]["TOTAL_GST"] = totalGST;
                        ds.Tables[0].Rows[i]["AMOUNT_WITH_GST"] = totalAmountWithGST;
                        ds.Tables[0].Rows[i]["GRADE"] = orderdate;
                    }

                }


                ds.Tables[0].Columns.Remove("GRADE");
                ds.Tables[0].Columns.Remove("FACTOR");
                //ds.Tables[0].Columns.Remove("OUTLET_ID");

                ds.Tables[0].Columns.Remove("OUT_PRICE");
                ds.Tables[0].Columns.Remove("DIVISION_ID");
                ds.Tables[0].Columns.Remove("ZONE_ID");
                ds.Tables[0].Columns.Remove("OUTLET_ADDRESS");
                ds.Tables[0].Columns.Remove("TOTAL_AMT");
                //ds.Tables[0].Columns.Remove("SR_ID");
                ds.Tables[0].Columns.Remove("SR_NAME");
                ds.Tables[0].Columns.Remove("OUTLET_NAME");
                ds.Tables[0].Columns.Remove("DISCOUNT");
                ds.Tables[0].Columns.Remove("ITEM_NAME");
                ds.Tables[0].Columns.Remove("ITEM_ID");
                ds.Tables[0].Columns.Remove("GST");
                ds.Tables[0].Columns.Remove("TOTAL_GST");
                ds.Tables[0].Columns.Remove("ITEM_CTN");
                ds.Tables[0].Columns.Remove("ITEM_QTY");


                DataTable dtCloned = ds.Tables[0].Clone();
                dtCloned.Columns[2].DataType = typeof(double);


                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    dtCloned.ImportRow(row);
                }



                var newSort = from row in dtCloned.AsEnumerable()
                              group row by new
                              {
                                  SR = row.Field<string>("SR"),
                                  SR_ID = row.Field<string>("SR_ID"),
                                  OUTLET = row.Field<string>("OUTLET"),
                                  OUTLET_ID = row.Field<string>("OUTLET_ID"),
                                  DM_ID = row.Field<string>("DM_ID"),
                                  DM_NAME = row.Field<string>("DM_NAME"),
                                  TRAN_ID = row.Field<string>("TRAN_ID"),
                              }
                                  into grp
                                  select new
                                  {
                                      SR = grp.Key.SR,
                                      SR_ID = grp.Key.SR_ID,
                                      OUTLET = grp.Key.OUTLET,
                                      OUTLET_ID = grp.Key.OUTLET_ID,
                                      DM_ID = grp.Key.DM_ID,
                                      DM_NAME = grp.Key.DM_NAME,
                                      TRAN_ID = grp.Key.TRAN_ID,
                                      AMOUNT_WITH_GST = grp.Sum(r => r.Field<double>("AMOUNT_WITH_GST"))


                                  };

                DataTable dTGroupBy = new DataTable("DataTable1");
                dTGroupBy.Columns.Add("DM_ID");
                dTGroupBy.Columns.Add("DM_NAME");
                dTGroupBy.Columns.Add("SR");
                dTGroupBy.Columns.Add("SR_ID");
                dTGroupBy.Columns.Add("OUTLET");
                dTGroupBy.Columns.Add("OUTLET_ID");
                dTGroupBy.Columns.Add("TRAN_ID");
                dTGroupBy.Columns.Add("TOTAL");


                foreach (var item in newSort)
                {
                    dTGroupBy.Rows.Add(item.DM_ID, item.DM_NAME, item.SR, item.SR_ID, item.OUTLET, item.OUTLET_ID, item.TRAN_ID, item.AMOUNT_WITH_GST);
                }

                DataColumn dc = new DataColumn("ORDER_DATE");
                dc.DataType = typeof(string);
                dc.DefaultValue = Convert.ToString(orderdate);
                dTGroupBy.Columns.Add(dc);


                string damageQuery = @"SELECT SUM((((T1.CARTON * T3.FACTOR) + T1.PCS) * T3.TP)) DAMAGE,T2.OUTLET_ID,T1.SR_ID,T4.DM_ID FROM T_DAMAGE T1 
                                    INNER JOIN T_OUTLET T2 ON T1.OUTLET_ID=T2.OUTLET_ID 
                                    INNER JOIN T_ITEM T3 ON T1.ITEM_CODE=T3.ITEM_ID 
                                    INNER JOIN T_DM_SR T4 ON T1.SR_ID=T4.SR_ID  
                                    WHERE T4.DM_ID='" + dm.Trim() + "' AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') GROUP BY T2.OUTLET_ID,T1.SR_ID,T4.DM_ID";

                OracleCommand damageCommand = new OracleCommand(damageQuery, conn);
                OracleDataAdapter damageadapter = new OracleDataAdapter(damageCommand);

                DataSet dsDamage = new DataSet();
                damageadapter.Fill(dsDamage, "damageTable");
                int damageCount = dsDamage.Tables[0].Rows.Count;

                if (damageCount > 0)
                {
                    for (int d = 0; d < damageCount; d++)
                    {
                        double dAMAGE = Convert.ToDouble(dsDamage.Tables[0].Rows[d]["DAMAGE"]);
                        string oUTLET_ID = Convert.ToString(dsDamage.Tables[0].Rows[d]["OUTLET_ID"]);
                        string sR_ID = Convert.ToString(dsDamage.Tables[0].Rows[d]["SR_ID"]);
                        string dM_ID = Convert.ToString(dsDamage.Tables[0].Rows[d]["DM_ID"]);

                        for (int t = 0; t < dTGroupBy.Rows.Count; t++)
                        {
                            double total = Convert.ToDouble(dTGroupBy.Rows[t]["TOTAL"]);
                            string oUTLET_ID2 = Convert.ToString(dTGroupBy.Rows[t]["OUTLET_ID"]);
                            string sR_ID2 = Convert.ToString(dTGroupBy.Rows[t]["SR_ID"]);
                            string dM_ID2 = Convert.ToString(dTGroupBy.Rows[t]["DM_ID"]);

                            if (oUTLET_ID == oUTLET_ID2 && sR_ID == sR_ID2 && dM_ID == dM_ID2)
                            {
                                double totalWithoutDamage = total - dAMAGE;

                                dTGroupBy.Rows[t]["TOTAL"] = totalWithoutDamage;
                            }



                        }




                    }

                }



                var crReport = new ReportDocument();
                crReport.Load(Server.MapPath("gatepassreports.rpt"));
                Session["ReportDocument"] = crReport;
                crReport.SetDataSource(dTGroupBy);

                CrystalReportViewer1.ReportSource = crReport;
                CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
            }
            catch (Exception ex)
            {

            }

        }
        else
        {
            ReportDocument doc = (ReportDocument)Session["ReportDocument"];
            CrystalReportViewer1.ReportSource = doc;
        }


    }



    private void GetDiscount(DataSet dsTrade, double iTEM_CTN, double item_PCS, double factor, int correctTradeRow, double discountAmount, double disCountQty, bool foundCorrectRow, double dp, string itemDivision, string itemZone)
    {
        try
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
                                ////double disQty = iTEM_CTN / min_Qty;
                                ////disCountQty = Math.Truncate(disQty) * min_Qty;
                                ////iTEM_CTN = (disQty - Math.Truncate(disQty)) * min_Qty;

                                ////disCountQty = iTEM_CTN;

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

        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }


    }

}