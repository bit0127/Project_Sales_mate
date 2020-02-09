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

public partial class topsheet : System.Web.UI.Page
{ 
    public double testDis = 0;
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                DataTable FreeTable = new DataTable();
                FreeTable.Columns.Add("ITEM_ID", typeof(string));
                FreeTable.Columns.Add("FREE_CTN", typeof(double));
                FreeTable.Columns.Add("FREE_PCS", typeof(double));
                
                string dm = Request.QueryString["dm"];
                //string sr = Request.QueryString["sr"];
                string orderdate = Request.QueryString["orderdate"];
                                
                string invoiceNo = "";
                
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDBMain"].ConnectionString;
                OracleConnection conn = new OracleConnection(connectionString); // C#

                conn.Open();
                string qrOrderInfo = @"SELECT T6.DM_ID,T6.DM_NAME,T5.* FROM
                                 (SELECT T4.ITEM_NAME,T4.FACTOR, 
                                 (((T4.FACTOR*T3.ITEM_CTN)+T3.ITEM_QTY)*T3.OUT_PRICE) TOTAL_AMT, T3.* FROM
                                 (SELECT T1.TRAN_ID,T1.ITEM_ID,T1.ITEM_CTN,T1.ITEM_QTY,T1.OUT_PRICE,T2.OUTLET_ID,T2.GRADE,T2.OUTLET_ADDRESS,T2.DIVISION_ID,T2.ZONE_ID FROM 
                                 (SELECT DISTINCT TRAN_ID, OUTLET_ID,ITEM_ID,SR_ID,ITEM_CTN,ITEM_QTY,OUT_PRICE FROM T_ORDER_DETAIL 
                                 WHERE ENTRY_DATE=TO_DATE('" + orderdate + "','DD/MM/YYYY') AND SR_ID IN (SELECT SR_ID FROM T_DM_SR WHERE DM_ID='" + dm + "') ) T1, ";
                qrOrderInfo = qrOrderInfo + @"(SELECT * FROM T_OUTLET) T2
                                 WHERE T1.OUTLET_ID=T2.OUTLET_ID ) T3,
                                 (SELECT * FROM T_ITEM) T4 
                                 WHERE T4.ITEM_ID=T3.ITEM_ID ) T5,
                                 (SELECT DM_ID,DM_NAME FROM T_DM) T6 WHERE DM_ID='" + dm + "'";

                OracleCommand cmdG = new OracleCommand(qrOrderInfo, conn);
                OracleDataAdapter da = new OracleDataAdapter(cmdG);

                DataSet ds = new DataSet();
                da.Fill(ds, "DataTable1");
                int c = ds.Tables[0].Rows.Count;


                if (c > 0 && ds.Tables[0].Rows[0]["ITEM_ID"].ToString() != "")
                {
                    DataView view = new DataView(ds.Tables[0]);
                    DataTable distinctValues = view.ToTable(true, "TRAN_ID");
                    int disCnt = distinctValues.Rows.Count;
                    for (int k = 0; k < disCnt; k++)
                    {
                        if (k == disCnt - 1)
                        {
                            invoiceNo = invoiceNo + distinctValues.Rows[k]["TRAN_ID"].ToString();
                        }
                        else
                        {
                            invoiceNo = invoiceNo + distinctValues.Rows[k]["TRAN_ID"].ToString() + ",";
                        }
                       
                    }


                        //Starting computing free offer
                        for (int i = 0; i < c; i++)
                        {
                            string iTEM_ID = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                            string OUTLET_ID = ds.Tables[0].Rows[i]["OUTLET_ID"].ToString();
                            double iTEM_CTN = Convert.ToDouble(ds.Tables[0].Rows[i]["ITEM_CTN"]);
                            double item_PCS = Convert.ToDouble(ds.Tables[0].Rows[i]["ITEM_QTY"]);
                            double factor = Convert.ToDouble(ds.Tables[0].Rows[i]["FACTOR"]);
                            string OUTLET_GRADE = Convert.ToString(ds.Tables[0].Rows[i]["GRADE"]);
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

                        }
                }


                conn.Close();

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

               


                ds.Tables[0].Columns.Remove("GRADE");
                ds.Tables[0].Columns.Remove("FACTOR");
                ds.Tables[0].Columns.Remove("OUTLET_ID");
                
                ds.Tables[0].Columns.Remove("OUT_PRICE");
                ds.Tables[0].Columns.Remove("DIVISION_ID");
                ds.Tables[0].Columns.Remove("ZONE_ID");
                ds.Tables[0].Columns.Remove("OUTLET_ADDRESS");
                ds.Tables[0].Columns.Remove("TRAN_ID");


                DataTable dtCloned = ds.Tables[0].Clone();
                dtCloned.Columns[3].DataType = typeof(double);
                dtCloned.Columns[5].DataType = typeof(double);
                dtCloned.Columns[6].DataType = typeof(double);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    dtCloned.ImportRow(row);
                }



                var newSort = from row in dtCloned.AsEnumerable()
                              group row by new { ITEM_ID = row.Field<string>("ITEM_ID"),
                                                 ITEM_NAME = row.Field<string>("ITEM_NAME"),
                                                 DM_ID = row.Field<string>("DM_ID"),
                                                 DM_NAME = row.Field<string>("DM_NAME")
                                                } 
                              into grp
                              select new
                              {
                                  ITEM_ID = grp.Key.ITEM_ID,
                                  ITEM_NAME = grp.Key.ITEM_NAME,
                                  DM_ID=grp.Key.DM_ID,
                                  DM_NAME = grp.Key.DM_NAME,
                                  SumITEM_CTN = grp.Sum(r => r.Field<double>("ITEM_CTN")),
                                  SumITEM_QTY = grp.Sum(r => r.Field<double>("ITEM_QTY")),
                                  SumTOTAL_AMT= grp.Sum(r => r.Field<double>("TOTAL_AMT"))

                              };


                DataTable dTGroupBy = new DataTable("DataTable1");
                dTGroupBy.Columns.Add("DM_ID");
                dTGroupBy.Columns.Add("DM_NAME");
                dTGroupBy.Columns.Add("ITEM_ID");
                dTGroupBy.Columns.Add("ITEM_NAME");
                dTGroupBy.Columns.Add("ITEM_CTN");
                dTGroupBy.Columns.Add("ITEM_QTY");
                dTGroupBy.Columns.Add("TOTAL_AMT");

                foreach (var item in newSort)
                {
                    dTGroupBy.Rows.Add(item.DM_ID,item.DM_NAME,item.ITEM_ID, item.ITEM_NAME, item.SumITEM_CTN,item.SumITEM_QTY,item.SumTOTAL_AMT);
                }



                DataColumn dcOrderInfo = null;
                //dcOrderInfo.DataType = typeof(double);
                //dcOrderInfo.DefaultValue = 0;


                dcOrderInfo = new DataColumn();
                dcOrderInfo.ColumnName = "FREE_CTN";
                dTGroupBy.Columns.Add(dcOrderInfo);

                dcOrderInfo = new DataColumn();
                dcOrderInfo.ColumnName = "FREE_PCS";
                dTGroupBy.Columns.Add(dcOrderInfo);

                dcOrderInfo = new DataColumn();
                dcOrderInfo.ColumnName = "TOTAL_CTN";
                dTGroupBy.Columns.Add(dcOrderInfo);


                dcOrderInfo = new DataColumn();
                dcOrderInfo.ColumnName = "TOTAL_PCS";
                dTGroupBy.Columns.Add(dcOrderInfo);

                //same item ordered & free 

                for (int i = 0; i < newFreeDt.Rows.Count; i++)
                {
                    string itemID = newFreeDt.Rows[i]["ITEM_ID"].ToString();
                    
                    for(int h=0;h<dTGroupBy.Rows.Count;h++)
                    {
                        if (itemID == dTGroupBy.Rows[h]["ITEM_ID"].ToString())
                        {
                            dTGroupBy.Rows[h]["FREE_CTN"] = newFreeDt.Rows[i]["FREE_CTN"];
                            dTGroupBy.Rows[h]["FREE_PCS"] = newFreeDt.Rows[i]["FREE_PCS"];

                            newFreeDt.Rows[i]["FREE_CTN"] = -1;
                            newFreeDt.Rows[i]["FREE_PCS"] = -1;
                        }
                    }             

                }

                //item not ordered only free

                for (int i = 0; i < newFreeDt.Rows.Count; i++)
                {
                    if (newFreeDt.Rows[i]["FREE_CTN"].ToString() != "-1")
                    {
                        DataRow dataRow = null;
                        dataRow = dTGroupBy.NewRow();
                        dataRow["DM_ID"] = dm;
                        dataRow["DM_Name"] = dm;
                        dataRow["ITEM_ID"] = newFreeDt.Rows[i]["ITEM_ID"].ToString();
                        dataRow["ITEM_NAME"] = newFreeDt.Rows[i]["ITEM_ID"].ToString();
                        dataRow["TOTAL_CTN"] = 0;
                        dataRow["ITEM_CTN"] = 0;
                        dataRow["ITEM_QTY"] = 0;
                        dataRow["FREE_CTN"] = Convert.ToDouble(newFreeDt.Rows[i]["FREE_CTN"]);
                        dataRow["FREE_PCS"] = Convert.ToDouble(newFreeDt.Rows[i]["FREE_PCS"]);
                        dTGroupBy.Rows.Add(dataRow);

                    }
                
                
                }



                int count = dTGroupBy.Rows.Count;
                double totalCTN = 0;
                double totalPCS = 0;

                if (count > 0 && dTGroupBy.Rows[0]["ITEM_ID"].ToString() != "")
                {
                    for (int i = 0; i < count; i++)
                    {
                        double freePCS = 0;
                        double freeCTN = 0;
                        double iTEM_CTN = 0;
                        double iTEM_PCS = 0;

                        if (string.IsNullOrEmpty(dTGroupBy.Rows[i]["FREE_PCS"].ToString()))
                        {
                            freePCS = 0;
                        }

                        else
                        {
                            freePCS = Convert.ToDouble(dTGroupBy.Rows[i]["FREE_PCS"]);
                        }

                        if (string.IsNullOrEmpty(dTGroupBy.Rows[i]["FREE_CTN"].ToString()))
                        {
                            freeCTN = 0;
                        }

                        else
                        {
                            freeCTN = Convert.ToDouble(dTGroupBy.Rows[i]["FREE_CTN"]);
                        }


                        if (string.IsNullOrEmpty(dTGroupBy.Rows[i]["ITEM_CTN"].ToString()))
                        {
                            iTEM_CTN = 0;
                        }

                        else
                        {
                            iTEM_CTN = Convert.ToDouble(dTGroupBy.Rows[i]["ITEM_CTN"]);
                        }

                        if (string.IsNullOrEmpty(dTGroupBy.Rows[i]["ITEM_QTY"].ToString()))
                        {
                            iTEM_PCS = 0;
                        }

                        else
                        {
                            iTEM_PCS = Convert.ToDouble(dTGroupBy.Rows[i]["ITEM_QTY"]);
                        }
                       
                        

                        totalCTN = iTEM_CTN + freeCTN;
                        totalPCS = iTEM_PCS + freePCS;


                        dTGroupBy.Rows[i]["TOTAL_CTN"] = totalCTN;
                        dTGroupBy.Rows[i]["TOTAL_PCS"] = totalPCS;
                        
                    }

                }


                DataColumn dc = new DataColumn("ORDER_DATE");
                dc.DataType = typeof(string);
                dc.DefaultValue = Convert.ToString(orderdate);
                dTGroupBy.Columns.Add(dc);

                DataColumn dc1 = new DataColumn("TRAN_ID");
                dc1.DataType = typeof(string);
                dc1.DefaultValue = Convert.ToString(invoiceNo);
                dTGroupBy.Columns.Add(dc1);

                //DataSet dsFinal = new DataSet();
                //dsFinal.Tables.Add(dTGroupBy);

             
                var crReport = new ReportDocument();
                crReport.Load(Server.MapPath("CrystalReport2.rpt"));
                Session["ReportDocument"] = crReport;
                crReport.SetDataSource(dTGroupBy);
                

                // Binding the crystalReportViewer with our report object. 
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



            if ((divisionID.ToLower().Contains("ALL".ToLower()) && zoneID.ToLower().Contains("ALL".ToLower())) || (divisionID == itemDivision
                            && zoneID == itemZone) || (itemDivision == divisionID && zoneID.ToLower().Contains("ALL".ToLower())))
            {
                if (freeType == "Carton")
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

                else if (freeType == "Piece")
                {

                    double min_Qty = Convert.ToDouble(dsTrade.Tables[0].Rows[h]["MIN_QTY"]);
                    double total_Item_PCS = (iTEM_CTN * factor) + item_PCS;

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

}