
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class diststock : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    [WebMethod]
    public static string GetDistName(string distId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT DIST_ID,DIST_NAME FROM T_DISTRIBUTOR WHERE DIST_ID='" + distId + "'";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                string dtId = ds.Tables[0].Rows[0]["DIST_ID"].ToString();
                string distName = ds.Tables[0].Rows[0]["DIST_NAME"].ToString();
                msg = distName;
            }
            else
            {
                msg = "No Distributor";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
    
    [WebMethod]
    public static string GetWarehouseInfo(string distId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT WH_CODE,WH_NAME FROM T_WARE_HOUSE WHERE DIST_CODE='" + distId + "'";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                string whId = ds.Tables[0].Rows[0]["WH_CODE"].ToString();
                string whName = ds.Tables[0].Rows[0]["WH_NAME"].ToString();
                msg = msg + ";" + whId + ";" + whName;
            }
            else
            {
                msg = "No WH";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
    
    [WebMethod]
    public static string GetDistStockInfo(string ocNum)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT T1.*,T2.WH_NAME FROM (SELECT * FROM T_DIST_STOCK WHERE OC_NUMBER='" + ocNum + "') T1,(SELECT WH_CODE,WH_NAME FROM T_WARE_HOUSE) T2 WHERE T1.WH_CODE=T2.WH_CODE";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string distId = ds.Tables[0].Rows[i]["DIST_CODE"].ToString();
                    string distName = ds.Tables[0].Rows[i]["DIST_NAME"].ToString();
                    string whId = ds.Tables[0].Rows[i]["WH_CODE"].ToString();
                    string whName = ds.Tables[0].Rows[i]["WH_NAME"].ToString();
                    string itemCode = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string itemName = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    string carton = ds.Tables[0].Rows[i]["ITEM_CTN"].ToString();
                    string piece = ds.Tables[0].Rows[i]["ITEM_PCS"].ToString();
                    string recdate = Convert.ToDateTime(ds.Tables[0].Rows[i]["RECEIVING_DATE"].ToString()).ToShortDateString();

                    msg = msg + ";" + ocNum + ";" + distId + ";" + distName + ";" + whId + ";" + whName + ";" + itemCode + ";" + itemName + ";" + carton + ";" + piece + ";" + recdate;
                }
            }
            else
            {
                msg = "No Info";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
      
    [WebMethod]
    public static string GetDistFreeStockInfo(string ocNumber)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT OC_NUMBER,ITEM_CODE,FREE_ITEM,FREE_QTY FROM T_DIST_FREE_ITEM 
                            WHERE OC_NUMBER='" + ocNumber + "'"; 

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string ocN = ds.Tables[0].Rows[i]["OC_NUMBER"].ToString();
                    string itemCode = ds.Tables[0].Rows[i]["ITEM_CODE"].ToString();
                    string freeItem = ds.Tables[0].Rows[i]["FREE_ITEM"].ToString();
                    string qty = ds.Tables[0].Rows[i]["FREE_QTY"].ToString();


                    msg = msg + ";" + ocN + ";" + itemCode + ";" + freeItem + ";" + qty;
                }
            }
            else
            {
                msg = "No Info";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
 
      
    [WebMethod]
    public static string DelDistFreeItemInfo(string itemId, string ocN, string free)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"DELETE FROM T_DIST_FREE_ITEM 
                            WHERE OC_NUMBER='" + ocN + "' AND ITEM_CODE='" + itemId + "' AND FREE_ITEM ='" + free + "'"; 

            OracleCommand cmd = new OracleCommand(query, conn);
            int c = cmd.ExecuteNonQuery();
            if (c > 0)
            {
                msg = "Successfuly Deleted";
            }
            else
            {
                msg = "Not Successful!";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod]
    public static string GetRoute(string srId, string orderDate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT ROUTE_ID,ROUTE_NAME FROM T_ROUTE WHERE
                            ROUTE_ID IN 
                            (SELECT DISTINCT T1.ROUTE_ID FROM 
                            (SELECT TRAN_ID,SR_ID,ROUTE_ID FROM T_ORDER_HEADER 
                            WHERE ENTRY_DATE=TO_DATE('" + orderDate.Trim() + "','DD/MM/YYYY') AND SR_ID='" + srId + "') T1, (SELECT * FROM T_ORDER_DETAIL) T2 WHERE T1.TRAN_ID=T2.TRAN_ID)";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string comId = ds.Tables[0].Rows[i]["ROUTE_ID"].ToString();
                    string comName = ds.Tables[0].Rows[i]["ROUTE_NAME"].ToString();
                    msg = msg + ";" + comId + ";" + comName;
                }

            }
            else
            {
                msg = "NotExist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
 
                                                           
    [WebMethod]
    public static string EditOrderInfo(string trnId, string itemId, string cartons, string pcs)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"UPDATE T_ORDER_DETAIL SET ITEM_CTN='" + cartons + "', ITEM_QTY='" + pcs + "' WHERE TRAN_ID='" + trnId + "' AND ITEM_ID='" + itemId + "'";

            OracleCommand cmd = new OracleCommand(query, conn);
            int i = cmd.ExecuteNonQuery();
             
            if (i > 0)
            {
                msg = "Successful!";
            }
            else
            {
                msg = "Not Successful!";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
   

    [WebMethod]
    public static string GetOutletWiseOrder(string srId, string orderDate, string outletId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT T4.*,T3.TRAN_ID,T3.ITEM_CTN,T3.ITEM_QTY,T3.ENTRY_DATE FROM
                            (SELECT DISTINCT T2.TRAN_ID,T2.ITEM_ID,T2.ITEM_CTN,T2.ITEM_QTY,T2.ENTRY_DATE FROM 
                            (SELECT TRAN_ID,SR_ID,ROUTE_ID FROM T_ORDER_HEADER 
                            WHERE ENTRY_DATE=TO_DATE('" + orderDate.Trim() + "','DD/MM/YYYY') AND SR_ID='" + srId + "' AND OUTLET_ID='" + outletId + "') T1, ";
            query = query + @"(SELECT * FROM T_ORDER_DETAIL) T2
                            WHERE T1.TRAN_ID=T2.TRAN_ID) T3,
                            (SELECT ITEM_ID,ITEM_NAME FROM T_ITEM) T4 
                            WHERE T3.ITEM_ID=T4.ITEM_ID";

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string trnId = ds.Tables[0].Rows[i]["TRAN_ID"].ToString();
                    string itemId = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string itemName = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    string carton = ds.Tables[0].Rows[i]["ITEM_CTN"].ToString();
                    string pcs = ds.Tables[0].Rows[i]["ITEM_QTY"].ToString();
                    string entryDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["ENTRY_DATE"].ToString()).ToShortDateString();
                    msg = msg + ";" + itemId + ";" + itemName + ";" + carton + ";" + pcs + ";" + entryDate + ";" + trnId;
                }

            }
            else
            {
                msg = "Not Order";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }

       
    [WebMethod]
    public static string GetDistSingleItemInfo(string itemId, string ocN)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT T1.*,T2.WH_NAME FROM (SELECT * FROM T_DIST_STOCK WHERE ITEM_ID ='" + itemId + "' AND OC_NUMBER='" + ocN + "') T1,(SELECT WH_CODE,WH_NAME FROM T_WARE_HOUSE) T2 WHERE T1.WH_CODE=T2.WH_CODE";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string distId = ds.Tables[0].Rows[i]["DIST_CODE"].ToString();
                    string distName = ds.Tables[0].Rows[i]["DIST_NAME"].ToString();
                    string whId = ds.Tables[0].Rows[i]["WH_CODE"].ToString();                     
                    string itemGroupId = ds.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                    string itemCode = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string carton = ds.Tables[0].Rows[i]["ITEM_CTN"].ToString();
                    string piece = ds.Tables[0].Rows[i]["ITEM_PCS"].ToString();
                    string factor = ds.Tables[0].Rows[i]["FACTOR"].ToString();
                    string factorType = ds.Tables[0].Rows[i]["FACTOR_TYPE"].ToString();
                    string dpPrice = ds.Tables[0].Rows[i]["DP_PRICE"].ToString();
                    string remarks = ds.Tables[0].Rows[i]["REMARKS"].ToString();
                    string d = Convert.ToDateTime(ds.Tables[0].Rows[i]["RECEIVING_DATE"].ToString()).Day.ToString();
                    d = d.Length == 1 ? "0" + d : d;
                    string m = Convert.ToDateTime(ds.Tables[0].Rows[i]["RECEIVING_DATE"].ToString()).Month.ToString();
                    m = m.Length == 1 ? "0" + m : m;
                    string y = Convert.ToDateTime(ds.Tables[0].Rows[i]["RECEIVING_DATE"].ToString()).Year.ToString();

                    string recdate = d + "/" + m + "/" + y;

                    msg = msg + ";" + ocN + ";" + distId + ";" + distName + ";" + whId + ";" + itemGroupId + ";" + itemCode + ";" + carton + ";" + piece + ";" + dpPrice + ";" + factor + ";" + factorType + ";" + recdate + ";" + remarks;
                }
            }
            else
            {
                msg = "No Info";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }

    [WebMethod]
    public static string GetOutletList(string routeId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT DISTINCT OUTLET_ID,OUTLET_NAME FROM T_OUTLET
                            WHERE ROUTE_ID='" + routeId + "'";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string comId = ds.Tables[0].Rows[i]["OUTLET_ID"].ToString();
                    string comName = ds.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                    msg = msg + ";" + comId + ";" + comName;
                }

            }
            else
            {
                msg = "NotExist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
    
    [WebMethod]
    public static string GetProductGroupInfo(string distId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP  
                            WHERE ITEM_GROUP_ID IN(SELECT ITEM_GROUP FROM T_DISTRIBUTOR WHERE DIST_ID='" + distId + "' AND STATUS='Y')";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                string whId = ds.Tables[0].Rows[0]["ITEM_GROUP_ID"].ToString();
                string whName = ds.Tables[0].Rows[0]["ITEM_GROUP_NAME"].ToString();
                msg = msg + ";" + whId + ";" + whName;
            }
            else
            {
                msg = "No WH";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
    
    [WebMethod]
    public static string GetGroupWiseItem(string groupId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT ITEM_ID,ITEM_NAME FROM T_ITEM WHERE ITEM_GROUP='" + groupId + "' AND ACTIVENESS='Y' ORDER BY ITEM_ID";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string whId = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string whName = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    msg = msg + ";" + whId + ";" + whName;
                }
            }
            else
            {
                msg = "No WH";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
    
    [WebMethod]
    public static string GetItemInformation(string itemId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT FACTOR,FACTOR_CATEGORY,DP FROM T_ITEM WHERE ITEM_ID='" + itemId + "' AND ACTIVENESS='Y'";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                string FACTOR = ds.Tables[0].Rows[0]["FACTOR"].ToString();
                string FACTOR_CATEGORY = ds.Tables[0].Rows[0]["FACTOR_CATEGORY"].ToString();
                string DP = ds.Tables[0].Rows[0]["DP"].ToString();
                msg = msg + ";" + FACTOR + ";" + FACTOR_CATEGORY + ";" + DP;
            }
            else
            {
                msg = "No WH";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
                    
    [WebMethod]
    public static string AddDistStock(string opt, string ocNo, string distId, string distName, string whId, string itemGroupId, string itemGroupName, string itemId, string itemName, string carton, string piece, string factor, string factorType, string price, string receivingDate, string remarks)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = "";

            if (opt == "Save")
            {
                query = @"INSERT INTO T_DIST_STOCK(DIST_CODE,DIST_NAME,WH_CODE,ITEM_GROUP,ITEM_GRP_NAME,ITEM_ID,ITEM_NAME,ITEM_CTN,ITEM_PCS,FACTOR,FACTOR_TYPE,DP_PRICE,ENTRY_DATE,RECEIVING_DATE,ENTRY_BY,REMARKS,OC_NUMBER)
                            VALUES('" + distId + "','" + distName + "','" + whId + "','" + itemGroupId + "','" + itemGroupName + "','" + itemId + "','" + itemName + "','" + carton + "','" + piece + "','" + factor + "','" + factorType + "','" + price + "',SYSDATE,TO_DATE('" + receivingDate + "','DD/MM/YYYY'),'" + HttpContext.Current.Session["userid"].ToString() + "','" + remarks + "','" + ocNo + "')";
            }
            else
            {
                query = @"UPDATE T_DIST_STOCK SET ITEM_CTN='" + carton + "',ITEM_PCS='" + piece + "',RECEIVING_DATE=TO_DATE('" + receivingDate + "','DD/MM/YYYY'),REMARKS='" + remarks + "' WHERE OC_NUMBER='" + ocNo + "' AND ITEM_ID='" + itemId + "'";
            }
            
            OracleCommand cmd = new OracleCommand(query, conn);
            int c = cmd.ExecuteNonQuery();
            if (c > 0)
            {
                msg = "Successful!";
            }
            else
            {
                msg = "Not Successful!";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
         
    [WebMethod]
    public static string AddDistStockFreeItem(string freeItem, string freeQty, string distId, string whId, string itemId, string carton, string piece, string receivingDate, string ocNos)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"INSERT INTO T_DIST_FREE_ITEM(DIST_CODE,WH_CODE,ITEM_CODE,ITEM_CTN,ITEM_PCS,FREE_ITEM,FREE_QTY,RECEIVING_DATE,ENTRY_DATE,ENTRY_BY,OC_NUMBER)
                            VALUES('" + distId + "','" + whId + "','" + itemId + "','" + carton + "','" + piece + "','" + freeItem + "','" + freeQty + "',TO_DATE('" + receivingDate + "','DD/MM/YYYY'),SYSDATE,'" + HttpContext.Current.Session["userid"].ToString() + "','" + ocNos + "')";
            OracleCommand cmd = new OracleCommand(query, conn);
            int c = cmd.ExecuteNonQuery();
            if (c > 0)
            {
                msg = "Successful!";
            }
            else
            {
                msg = "Not Successful!";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
}