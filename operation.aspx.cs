using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Script.Services;

public partial class operation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                //HttpContext.Current.Session["country"] = Request.QueryString["country"].ToString();
                HttpContext.Current.Session["company"] = Request.QueryString["company"].ToString();

                if (HttpContext.Current.Session["userid"].ToString() == "suman_pal")
                {

                }
                else if (HttpContext.Current.Session["userid"].ToString() == "33222")
                {
                    //Response.Redirect("qatarsetup.aspx?country=" + HttpContext.Current.Session["country"].ToString().Trim() + "&company=" + HttpContext.Current.Session["company"].ToString().Trim(), false);
                    Response.Redirect("qatarsetup.aspx", false);

                }
                else
                {
                    Response.Redirect("dashboard.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("login.aspx", false);
            }
        }        
    }

    
    [WebMethod(EnableSession = true)]
    public static string TransferSRRouteOutlet(string closedSRId, string newSRId)
    {
        string msg = "Not Successful";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
        
        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT ROUTE_ID FROM T_SR_ROUTE_DAY WHERE SR_ID='" + newSRId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["ROUTE_ID"].ToString() != "")
            {
                for (int k = 0; k < i; k++)
                {
                    string routeId = ds.Tables[0].Rows[k]["ROUTE_ID"].ToString();

                    string qrOlt = @"SELECT OUTLET_ID FROM T_OUTLET WHERE SR_ID='" + closedSRId + "' AND ROUTE_ID='" + routeId + "'";
                    OracleCommand cmdOlt = new OracleCommand(qrOlt, conn);
                    OracleDataAdapter daOlt = new OracleDataAdapter(cmdOlt);
                    DataSet dsOlt = new DataSet();
                    daOlt.Fill(dsOlt);
                    int q = dsOlt.Tables[0].Rows.Count;
                    if (q > 0 && dsOlt.Tables[0].Rows[0]["OUTLET_ID"].ToString() != "")
                    {
                        for (int p = 0; p < q; p++)
                        {
                            string outletID = dsOlt.Tables[0].Rows[p]["OUTLET_ID"].ToString();
                            string qrUp = @"UPDATE T_OUTLET SET SR_ID='" + newSRId + "' WHERE OUTLET_ID='" + outletID + "'";
                            OracleCommand cmdUp = new OracleCommand(qrUp, conn);
                            int u = cmdUp.ExecuteNonQuery();
                            if (u > 0)
                            {
                                msg = "Successful!";
                            }
                            else
                            {
                                msg = "Not Successful";
                            }
                        }
                    }
                    else
                    {
                        msg = "No Data for Update";
                    }
                }               
                
            }
            else
            {
                msg = "No Data for Update";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod(EnableSession = true)]
    public static string TransferSROutlet(string routeID, string closedSRId, string newSRId)
    {
        string msg = "Not Successful";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qrOlt = @"SELECT OUTLET_ID FROM T_OUTLET WHERE SR_ID='" + closedSRId.Trim().ToString() + "' AND ROUTE_ID='" + routeID.Trim().ToString() + "'";
            OracleCommand cmdOlt = new OracleCommand(qrOlt, conn);
            OracleDataAdapter daOlt = new OracleDataAdapter(cmdOlt);
            DataSet dsOlt = new DataSet();
            daOlt.Fill(dsOlt);
            int q = dsOlt.Tables[0].Rows.Count;
            if (q > 0 && dsOlt.Tables[0].Rows[0]["OUTLET_ID"].ToString() != "")
            {
                for (int p = 0; p < q; p++)
                {
                    string outletID = dsOlt.Tables[0].Rows[p]["OUTLET_ID"].ToString();
                    string qrUp = @"UPDATE T_OUTLET SET SR_ID='" + newSRId.Trim().ToString() + "' WHERE OUTLET_ID='" + outletID.Trim().ToString() + "'";
                    OracleCommand cmdUp = new OracleCommand(qrUp, conn);
                    int u = cmdUp.ExecuteNonQuery();
                    if (u > 0)
                    {
                        msg = "Successful!";
                    }
                    else
                    {
                        msg = "Not Successful";
                    }
                }
            }
            else
            {
                msg = "No Data for Update";
            }


            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }



    [WebMethod(EnableSession = true)]
    public static string CopySROutlet(string routeID, string txtFromSRId, string txtToSRId)
    {
        string msg = "Not Successful";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qrOlt = @"SELECT * FROM T_OUTLET WHERE SR_ID='" + txtFromSRId.Trim().ToString() + "' AND ROUTE_ID='" + routeID.Trim().ToString() + "'";
            OracleCommand cmdOlt = new OracleCommand(qrOlt, conn);
            OracleDataAdapter daOlt = new OracleDataAdapter(cmdOlt);
            DataSet dsOlt = new DataSet();
            daOlt.Fill(dsOlt);
            int q = dsOlt.Tables[0].Rows.Count;
            if (q > 0 && dsOlt.Tables[0].Rows[0]["OUTLET_ID"].ToString() != "")
            {
                for (int p = 0; p < q; p++)
                {
                    string outletID = dsOlt.Tables[0].Rows[p]["OUTLET_ID"].ToString();
                    string OUTLET_NAME = dsOlt.Tables[0].Rows[p]["OUTLET_NAME"].ToString();
                    string OUTLET_ADDRESS = dsOlt.Tables[0].Rows[p]["OUTLET_ADDRESS"].ToString();
                    string PROPRITOR_NAME = dsOlt.Tables[0].Rows[p]["PROPRITOR_NAME"].ToString();
                    string MOBILE_NUMBER = dsOlt.Tables[0].Rows[p]["MOBILE_NUMBER"].ToString();
                    string EMAIL_ADDRESS = dsOlt.Tables[0].Rows[p]["EMAIL_ADDRESS"].ToString();
                    string COUNTRY_NAME = dsOlt.Tables[0].Rows[p]["COUNTRY_NAME"].ToString();
                    string DIVISION_ID = dsOlt.Tables[0].Rows[p]["DIVISION_ID"].ToString();
                    string ZONE_ID = dsOlt.Tables[0].Rows[p]["ZONE_ID"].ToString();
                    string ROUTE_ID = dsOlt.Tables[0].Rows[p]["ROUTE_ID"].ToString();
                    string FRIDGE = dsOlt.Tables[0].Rows[p]["FRIDGE"].ToString();
                    string SIGNBOARD = dsOlt.Tables[0].Rows[p]["SIGNBOARD"].ToString();
                    string RACK = dsOlt.Tables[0].Rows[p]["RACK"].ToString();
                    string STATUS = dsOlt.Tables[0].Rows[p]["STATUS"].ToString();
                    string ENTRY_DATE = dsOlt.Tables[0].Rows[p]["ENTRY_DATE"].ToString();
                    string ENTRY_BY = txtToSRId.Trim().ToString();
                    string OUTLET_BL_NAME = dsOlt.Tables[0].Rows[p]["OUTLET_BL_NAME"].ToString();
                    string ADDRESS_BL = dsOlt.Tables[0].Rows[p]["ADDRESS_BL"].ToString();
                    string PROPRITOR_BL = dsOlt.Tables[0].Rows[p]["PROPRITOR_BL"].ToString();
                    string CATEGORY_NAME = dsOlt.Tables[0].Rows[p]["CATEGORY_NAME"].ToString();
                    string GRADE = dsOlt.Tables[0].Rows[p]["GRADE"].ToString();
                    string SR_ID = txtToSRId.Trim().ToString();
                    string PRODUCTS = dsOlt.Tables[0].Rows[p]["PRODUCTS"].ToString();


                    string qrUp = @"INSERT INTO T_OUTLET(OUTLET_ID,OUTLET_NAME,OUTLET_ADDRESS,PROPRITOR_NAME,MOBILE_NUMBER,EMAIL_ADDRESS,COUNTRY_NAME,DIVISION_ID,ZONE_ID,ROUTE_ID,FRIDGE,SIGNBOARD,RACK,STATUS,ENTRY_DATE,ENTRY_BY,OUTLET_BL_NAME,ADDRESS_BL,PROPRITOR_BL,CATEGORY_NAME,GRADE,SR_ID,PRODUCTS) " +
                                  @"VALUES('" + outletID + "','" + OUTLET_NAME + "','" + OUTLET_ADDRESS + "','" + PROPRITOR_NAME + "','" + MOBILE_NUMBER + "','" + EMAIL_ADDRESS + "','" + COUNTRY_NAME + "','" + DIVISION_ID + "','" + ZONE_ID + "','" + ROUTE_ID + "','" + FRIDGE + "','" + SIGNBOARD + "','" + RACK + "','" + STATUS + "',TO_DATE(SYSDATE,'DD/MM/YYYY'),'" + SR_ID + "','" + OUTLET_BL_NAME + "','" + ADDRESS_BL + "','" + PROPRITOR_BL + "','" + CATEGORY_NAME + "','" + GRADE + "','" + SR_ID + "','" + PRODUCTS + "')";
                    OracleCommand cmdUp = new OracleCommand(qrUp, conn);
                    int u = cmdUp.ExecuteNonQuery();
                    if (u > 0)
                    {
                        msg = "Successful!";
                    }
                    else
                    {
                        msg = "Not Successful";
                    }
                }
            }
            else
            {
                msg = "No Data for Update";
            }


            conn.Close();
        }
        catch (Exception ex) 
        {
            msg = ex.ToString();
        }

        return msg;
    }

    
   
                           
    [WebMethod(EnableSession = true)]
    public static string AddItemGroupInfo(string groupId, string groupName, string motherCompany, string ownCompany, string status)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT ITEM_GROUP_ID FROM T_ITEM_GROUP WHERE ITEM_GROUP_ID='" + groupId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["ITEM_GROUP_ID"].ToString() != "")
            {
                string query = @"UPDATE T_ITEM_GROUP SET ITEM_GROUP_NAME='" + groupName + "',COMPANY_ID='" + ownCompany + "',STATUS='" + status + "',MOTHER_COMPANY='" + motherCompany + "' WHERE ITEM_GROUP_ID='" + groupId + "'";
                            
                OracleCommand cmd = new OracleCommand(query, conn);
                int c = cmd.ExecuteNonQuery();
                if (c > 0)
                {
                    msg = "Successful!";
                }
                else
                {
                    msg = "Not Successful";
                }
            }
            else
            {

                string query = @"INSERT INTO T_ITEM_GROUP(ITEM_GROUP_ID,ITEM_GROUP_NAME,COMPANY_ID,ENTRY_DATE,ENTRY_BY,STATUS,MOTHER_COMPANY)
                            VALUES ('" + groupId + "','" + groupName + "','" + ownCompany + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + status + "','" + motherCompany + "')";
                OracleCommand cmd = new OracleCommand(query, conn);
                int c = cmd.ExecuteNonQuery();
                if (c > 0)
                {
                    msg = "Successful!";
                }
                else
                {
                    msg = "Not Successful";
                }
            }


            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
                  
    
    [WebMethod(EnableSession = true)]
    public static string AddItemClassInfo(string classId, string className, string motherCompany, string ownCompany, string itemGroup, string status)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT CLASS_ID FROM T_ITEM_CLASS WHERE CLASS_ID='" + classId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["CLASS_ID"].ToString() != "")
            {
                string qrS = @"DELETE FROM T_ITEM_CLASS WHERE CLASS_ID='" + classId + "'";
                OracleCommand cmdS = new OracleCommand(qrS, conn);
                int cS = cmdS.ExecuteNonQuery();
            }

            string query = @"INSERT INTO T_ITEM_CLASS(CLASS_ID,CLASS_NAME,MOTHER_COMPANY,COMPANY_ID,ITEM_GROUP,ENTRY_DATE,ENTRY_BY,STATUS)
                            VALUES ('" + classId + "','" + className + "','" + motherCompany + "','" + ownCompany + "','" + itemGroup + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + status + "')";
            OracleCommand cmd = new OracleCommand(query, conn);
            int c = cmd.ExecuteNonQuery();
            if (c > 0)
            {
                msg = "Successful!";
            }
            else
            {
                msg = "Not Successful";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod]
    public static string GetCompanyGroup(string ownCompany)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP WHERE COMPANY_ID='" + ownCompany.Trim() + "'";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0 && ds.Tables[0].Rows[0][0].ToString() != "")
            {
                for (int i = 0; i < c; i++)
                {
                    string comId = ds.Tables[0].Rows[i]["ITEM_GROUP_ID"].ToString();
                    string comName = ds.Tables[0].Rows[i]["ITEM_GROUP_NAME"].ToString();
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
    public static string GetSRByGroup(string orderDate, string groupId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT T2.SR_ID,T2.SR_NAME FROM
                            (SELECT DISTINCT SR_ID FROM T_ORDER_DETAIL WHERE ENTRY_DATE=TO_DATE('" + orderDate.Trim() + "','DD/MM/YYYY')) T1, ";
            query = query + @"(SELECT SR_ID,SR_NAME FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y') T2 WHERE T1.SR_ID=T2.SR_ID";

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0 && ds.Tables[0].Rows[0][0].ToString() != "")
            {
                for (int i = 0; i < c; i++)
                {
                    string SR_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string SR_NAME = ds.Tables[0].Rows[i]["SR_NAME"].ToString();
                    msg = msg + ";" + SR_ID + ";" + SR_NAME;
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
    public static string GetOutletBySR(string orderDate, string srId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT T2.OUTLET_ID,T2.OUTLET_NAME FROM
                            (SELECT DISTINCT OUTLET_ID FROM T_ORDER_DETAIL WHERE SR_ID='" + srId.Trim() + "' AND ENTRY_DATE=TO_DATE('" + orderDate.Trim() + "','DD/MM/YYYY')) T1, ";
            query = query + @"(SELECT OUTLET_ID,OUTLET_NAME FROM T_OUTLET WHERE SR_ID='" + srId + "' AND STATUS='Y') T2 WHERE T1.OUTLET_ID=T2.OUTLET_ID";

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0 && ds.Tables[0].Rows[0][0].ToString() != "")
            {
                for (int i = 0; i < c; i++)
                {
                    string OUTLET_ID = ds.Tables[0].Rows[i]["OUTLET_ID"].ToString();
                    string OUTLET_NAME = ds.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                    msg = msg + ";" + OUTLET_ID + ";" + OUTLET_NAME;
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
    public static string GetItemByOutlet(string orderDate, string srId, string groupId, string outletId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT T2.ITEM_ID,T2.ITEM_NAME FROM
                            (SELECT DISTINCT ITEM_ID FROM T_ORDER_DETAIL WHERE OUTLET_ID='" + outletId.Trim() + "' AND SR_ID='" + srId.Trim() + "' AND ENTRY_DATE=TO_DATE('" + orderDate.Trim() + "','DD/MM/YYYY')) T1, ";
            query = query + @"(SELECT ITEM_ID,ITEM_NAME FROM T_ITEM WHERE ACTIVENESS='Y') T2 WHERE T1.ITEM_ID=T2.ITEM_ID";

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0 && ds.Tables[0].Rows[0][0].ToString() != "")
            {
                for (int i = 0; i < c; i++)
                {
                    string ITEM_ID = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string ITEM_NAME = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    msg = msg + ";" + ITEM_ID + ";" + ITEM_NAME;
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
    public static string GetItemCtnByOutlet(string orderDate, string srId, string groupId, string outletId, string itemId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT ITEM_QTY,ITEM_CTN FROM T_ORDER_DETAIL WHERE OUTLET_ID='" + outletId.Trim() + "' AND ITEM_ID='" + itemId + "' AND SR_ID='" + srId.Trim() + "' AND ENTRY_DATE=TO_DATE('" + orderDate.Trim() + "','DD/MM/YYYY')";             

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0 && ds.Tables[0].Rows[0][0].ToString() !="")
            {
                for (int i = 0; i < c; i++)
                {
                    string ITEM_QTY = ds.Tables[0].Rows[i]["ITEM_QTY"].ToString();
                    string ITEM_CTN = ds.Tables[0].Rows[i]["ITEM_CTN"].ToString();
                    msg = msg + ";" + ITEM_QTY + ";" + ITEM_CTN;
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

    
    [WebMethod(EnableSession = true)]
    public static string AddNewItemInfo(string itemCode, string itemName, string itemShortName, string itemNameBangla, string motherCompany, string ownCompany, string itemGroup, string itemClass, string factorCat, string factor, string mrp, string ws, string dp, string tp, string vat, string activeness)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT ITEM_ID FROM T_ITEM WHERE ITEM_ID='" + itemCode + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["ITEM_ID"].ToString() != "")
            {
                string qrS = @"UPDATE T_ITEM SET ITEM_NAME='" + itemName + "',ITEM_SHORT_NAME='" + itemShortName + "',ITEM_BL_NAME='" + itemNameBangla + "',MOTHER_COMPANY='" + motherCompany + "',OWN_COMPANY='" + ownCompany + "',ITEM_GROUP='" + itemGroup + "',ITEM_CLASS='" + itemClass + "',FACTOR_CATEGORY='" + factorCat + "',FACTOR='" + factor + "',DP='" + dp + "',TP='" + tp + "',MRP='" + mrp + "',WS='" + ws + "',VAT='" + vat + "',ACTIVENESS='" + activeness + "' WHERE ITEM_ID='" + itemCode + "'";
                OracleCommand cmdS = new OracleCommand(qrS, conn);
                int cS = cmdS.ExecuteNonQuery();
                if (cS > 0)
                {
                    msg = "Successful!";
                }
                else
                {
                    msg = "Not Successful";
                }
            }
            else
            {
                if (ownCompany == "M1001")
                {
                    itemCode = "M" + itemCode;
                }
                else if (ownCompany == "M1002")
                {
                    itemCode = "M" + itemCode;
                }
                else if (ownCompany == "M1003")
                {
                    itemCode = "M" + itemCode;
                }

                string query = @"INSERT INTO T_ITEM(ITEM_ID,ITEM_NAME,ITEM_SHORT_NAME,ITEM_BL_NAME,MOTHER_COMPANY,OWN_COMPANY,ITEM_GROUP,ITEM_CLASS,FACTOR_CATEGORY,FACTOR,DP,TP,MRP,WS,VAT,ACTIVENESS,ENTRY_DATE,ENTRY_BY)
                            VALUES ('" + itemCode + "','" + itemName + "','" + itemShortName + "','" + itemNameBangla + "','" + motherCompany + "','" + ownCompany + "','" + itemGroup + "','" + itemClass + "','" + factorCat + "','" + factor + "','" + dp + "','" + tp + "','" + mrp + "','" + ws + "','" + vat + "','" + activeness + "', TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "')";
                OracleCommand cmd = new OracleCommand(query, conn);

                int c = cmd.ExecuteNonQuery();
                if (c > 0)
                {
                    msg = "Successful!";
                }
                else
                {
                    msg = "Not Successful";
                }
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod(EnableSession = true)]
    public static string AddCOOInfo(string cooid, string name, string designation, string pwd, string phone, string email, string startDate, string status)
    {
        string msg = "";
        string query = "";
        string currentUser = HttpContext.Current.Session["userid"].ToString();
        DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);
        try
        {
            DataSet ds = new DataSet();

            string qr = @"SELECT COO_ID FROM T_COO WHERE COO_ID='" + cooid + "'";
            ds = CommonDBSvc.GetDataSet(qr);

            if (ds.Tables[0].Rows.Count > 0)
            {
                query = @"UPDATE [dbo].[T_COO] SET NAME='" + name + "', DESIGNATION_ID='" + designation + "', PHONE_NO='" + phone + "', EMAIL='" + email + "', START_DATE='" + startDate + "', " +
                " STATUS='" + status + "',LAST_UPDATE=GETDATE(),ENTRY_BY='" + currentUser + "' WHERE COO_ID='" + cooid + "'";
            }

            else
            {
                query = @"INSERT INTO T_COO(COO_ID,NAME,PWD,DESIGNATION_ID,PHONE_NO,EMAIL,START_DATE,ENTRY_BY,LAST_UPDATE,STATUS)
                            VALUES ('" + cooid + "','" + name + "','" + pwd + "','" + designation + "','" + phone + "','" + email + "','" + startDate + "','" + currentUser + "',GETDATE(),'" + status + "')";
            }

            msg = CommonDBSvc.ExecuteQuery(query);

        }
        catch (Exception ex) 
        {
            msg = ex.ToString();
        }

        return msg;
    }        
    
                   
    [WebMethod(EnableSession = true)]
    public static string AddHOSInfo(string hosId, string hosName, string designation, string Pwd, string Phone, string emailAddress, string itemGroup, string startDate, string status)
    {
        string msg = "";
        string query = "";
        string currentUser = HttpContext.Current.Session["userid"].ToString();
        DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);
            try
            {
                DataSet ds = new DataSet();

                string qr = @"SELECT HOS_ID FROM T_HOS WHERE HOS_ID='" + hosId + "'";
                ds = CommonDBSvc.GetDataSet(qr);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    query = @"UPDATE [dbo].[T_HOS] SET HOS_NAME='" + hosName + "', DESIGNATION_ID='" + designation + "', HOS_PWD='" + Pwd + "', HOS_MOBILE='" + Phone + "', HOS_EMAIL='" + emailAddress + "', " +
                    " ITEM_GROUP_ID='" + itemGroup + "', START_DATE='" + start + "', STATUS='" + status + "',LAST_UPDATE=GETDATE(),ENTRY_BY='" + currentUser + "' WHERE HOS_ID='" + hosId + "'";
                }

                else
                {
                    query = @"INSERT INTO T_HOS(HOS_ID,HOS_NAME,DESIGNATION_ID,HOS_PWD,ITEM_GROUP_ID,HOS_MOBILE,HOS_EMAIL,STATUS,LAST_UPDATE,ENTRY_BY,START_DATE)
                            VALUES ('" + hosId + "','" + hosName + "','"+designation+"','" + Pwd + "','" + itemGroup + "','" + Phone + "','" + emailAddress + "','" + status + "',GETDATE(),'" + currentUser + "','" + start + "')";
                }

                msg = CommonDBSvc.ExecuteQuery(query);

            }
            catch (Exception ex) { }

        return msg;
    }     

    

    [WebMethod(EnableSession = true)]
    public static string AddAGMRMInfo(string rmId, string rmName, string designation, string Pwd, string Phone, string emailAddress, string itemGroup, string startDate,string status)
    {
        string msg = "";
        string query = "";
        string currentUser = HttpContext.Current.Session["userid"].ToString();
        DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);

         try
            {
                DataSet ds = new DataSet();

                string qr = @"SELECT AGM_RM_ID FROM T_AGM_RM WHERE AGM_RM_ID='" + rmId + "'";
                ds = CommonDBSvc.GetDataSet(qr);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    query = @"UPDATE [dbo].[T_AGM_RM] SET NAME='" + rmName + "', DESIGNATION_ID='" + designation + "', PWD='" + Pwd + "', MOBILE_NO='" + Phone + "', EMAIL='" + emailAddress + "', " +
                    " ITEM_GROUP_ID='" + itemGroup + "', START_DATE='" + start + "', STATUS='" + status + "',LAST_UPDATE=GETDATE(),ENTRY_BY='" + currentUser + "' WHERE AGM_RM_ID='" + rmId + "'";
                }

                else
                {
                    query = @"INSERT INTO T_AGM_RM(AGM_RM_ID,NAME,DESIGNATION_ID,PWD,ITEM_GROUP_ID,MOBILE_NO,EMAIL,STATUS,LAST_UPDATE,ENTRY_BY,START_DATE)
                            VALUES ('" + rmId + "','" + rmName + "','" + designation + "','" + Pwd + "','" + itemGroup + "','" + Phone + "','" + emailAddress + "','" + status + "',GETDATE(),'" + currentUser + "','" + start + "')";
                }

                msg = CommonDBSvc.ExecuteQuery(query);

            }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod(EnableSession = true)]
    public static string AddTSMZMInfo(string tsmId, string tsmName, string designation, string Pwd, string Phone, string emailAddress, string itemGroup, string startDate, string status)
    {
        string msg = "";
        string query = "";
        string currentUser = HttpContext.Current.Session["userid"].ToString();
        DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);

         try
            {
                DataSet ds = new DataSet();

                string qr = @"SELECT TSM_ZM_ID FROM T_TSM_ZM WHERE TSM_ZM_ID='" + tsmId + "'";
                ds = CommonDBSvc.GetDataSet(qr);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    query = @"UPDATE [dbo].[T_TSM_ZM] SET NAME='" + tsmName + "', DESIGNATION_ID='" + designation + "', PWD='" + Pwd + "', MOBILE_NO='" + Phone + "', EMAIL='" + emailAddress + "', " +
                    " ITEM_GROUP_ID='" + itemGroup + "', START_DATE='" + start + "', STATUS='" + status + "',LAST_UPDATE=GETDATE(),ENTRY_BY='" + currentUser + "' WHERE TSM_ZM_ID='" + tsmId + "'";
                }

                else
                {
                    query = @"INSERT INTO T_TSM_ZM(TSM_ZM_ID,NAME,DESIGNATION_ID,PWD,ITEM_GROUP_ID,MOBILE_NO,EMAIL,STATUS,LAST_UPDATE,ENTRY_BY,START_DATE)
                            VALUES ('" + tsmId + "','" +tsmName + "','" + designation + "','" + Pwd + "','" + itemGroup + "','" + Phone + "','" + emailAddress + "','" + status + "',GETDATE(),'" + currentUser + "','" + start + "')";
                }

                msg = CommonDBSvc.ExecuteQuery(query);

            }
        catch (Exception ex) { }

        return msg;
    }

    
    [WebMethod(EnableSession = true)]
    public static string AddSRInfo(string id, string name, string pwd, string phoneOffice, string phonePersonal, string emailAddress, string itemGroup, string preOrder, string directSales, string delivery, string startDate, string status)
    {
        string msg = "";
        string query = "";
        string currentUser = HttpContext.Current.Session["userid"].ToString();
        DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);

        try
        {
            DataSet ds = new DataSet();

            string qr = @"SELECT SR_ID FROM T_SR WHERE SR_ID='" + id + "'";
            ds = CommonDBSvc.GetDataSet(qr);

            if (ds.Tables[0].Rows.Count > 0)
            {
                query = @"UPDATE [dbo].[T_SR] SET NAME='" + name + "',PWD='" + pwd + "',  PHONE_OFFICE='" + phoneOffice + "', PHONE_PERSONAL='" + phonePersonal + "', EMAIL='" + emailAddress + "', " +
                " CURRENT_GROUP_ID='" + itemGroup + "', START_DATE='" + start + "', STATUS='" + status + "',LAST_UPDATE=GETDATE(),ENTRY_BY='" + currentUser + "' WHERE SR_ID='" + id + "'";
            }

            else
            {
                query = @"INSERT INTO T_SR(SR_ID,NAME,PHONE_OFFICE,PHONE_PERSONAL,PWD,CURRENT_GROUP_ID,EMAIL,PRE_ORDER,DIRECT_SALES,DELIVERY,START_DATE,STATUS,LAST_UPDATE,ENTRY_BY)
                            VALUES ('" + id + "','" + name + "','" + phoneOffice + "','" + phonePersonal + "','" + pwd + "','" + itemGroup + "','" + emailAddress + "','" + preOrder + "','" + directSales + "','" + delivery + "','" + start + "','" + status + "',GETDATE(),'" + currentUser + "')";
            }

            msg = CommonDBSvc.ExecuteQuery(query);

        }
        catch (Exception ex) 
        {
            msg = ex.ToString();
        }

        return msg;
    }

    
    [WebMethod(EnableSession = true)]
    public static string AddDistributorInfo(string rmId, string rmName, string bName, string address, string Phone, string emailAddress, string country, string division, string zone, string motherCompany, string ownCompany, string itemGroup, string status)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT DIST_ID FROM T_DISTRIBUTOR WHERE DIST_ID='" + rmId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["DIST_ID"].ToString() != "")
            {
                string qrS = @"UPDATE T_DISTRIBUTOR SET DIST_NAME='" + rmName + "', DIST_BUSINESS_NAME='" + bName + "',MOBILE_NO='" + Phone + "',DIST_ADDRESS='" + address + "',COUNTRY_NAME='" + country + "',DIVISION_NAME='" + division + "',DIST_ZONE='" + zone + "',ITEM_GROUP='" + itemGroup + "',STATUS='" + status + "',EMAIL_ADDRESS='" + emailAddress + "' WHERE DIST_ID='" + rmId + "'";
                OracleCommand cmdS = new OracleCommand(qrS, conn);
                int cS = cmdS.ExecuteNonQuery();
                if (cS > 0)
                {
                    msg = "Successful!";
                }
                else
                {
                    msg = "Not Successful";
                }

            }
            else
            {
                string query = @"INSERT INTO T_DISTRIBUTOR(DIST_ID,DIST_NAME,DIST_BUSINESS_NAME,MOBILE_NO,DIST_ADDRESS,COUNTRY_NAME,DIVISION_NAME,DIST_ZONE,ITEM_GROUP,STATUS,ENTRY_DATE,ENTRY_BY,EMAIL_ADDRESS)
                            VALUES ('" + rmId + "','" + rmName + "','" + bName + "','" + Phone + "','" + address + "','" + country + "','" + division + "','" + zone + "','" + itemGroup + "','" + status + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + emailAddress + "')";
                OracleCommand cmd = new OracleCommand(query, conn);

                int c = cmd.ExecuteNonQuery();
                if (c > 0)
                {
                    msg = "Successful!";
                }
                else
                {
                    msg = "Not Successful";
                }
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
                     
    
    [WebMethod(EnableSession = true)]
    public static string AddWHInfo(string distId, string distName, string WHCode, string whName, string contactName, string address, string Phone, string emailAddress, string country, string division, string zone, string status)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT WH_CODE FROM T_WARE_HOUSE WHERE WH_CODE='" + WHCode + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["WH_CODE"].ToString() != "")
            {
                string qrS = @"DELETE FROM T_WARE_HOUSE WHERE WH_CODE='" + WHCode + "'";
                OracleCommand cmdS = new OracleCommand(qrS, conn);
                int cS = cmdS.ExecuteNonQuery();

            }

            string query = @"INSERT INTO T_WARE_HOUSE(DIST_CODE,WH_CODE,WH_NAME,CONTACT_PERSON,MOBILE_NO,ADDRESS,EMAIL_ADDRESS,COUNTRY,DIVISION_ID,ZONE_ID,STATUS,ENTRY_DATE,ENTRY_DATETIME,ENTRY_BY)
                            VALUES ('" + distId + "','" + WHCode + "','" + whName + "','" + contactName + "','" + Phone + "','" + address + "','" + emailAddress + "','" + country + "','" + division + "','" + zone + "','" + status + "',TO_DATE(SYSDATE),SYSDATE,'" + HttpContext.Current.Session["userid"].ToString() + "')";
            OracleCommand cmd = new OracleCommand(query, conn);

            int c = cmd.ExecuteNonQuery();
            if (c > 0)
            {
                msg = "Successful!";
            }
            else
            {
                msg = "Not Successful";
            }


            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod(EnableSession = true)]
    public static string AddDSRInfo(string rmId, string rmName, string pwd, string distId, string Phone, string emailAddress, string status)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT DSR_ID FROM T_DSR WHERE DSR_ID='" + rmId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["DSR_ID"].ToString() != "")
            {
                string qrS = @"DELETE FROM T_DSR WHERE DSR_ID='" + rmId + "'";
                OracleCommand cmdS = new OracleCommand(qrS, conn);
                int cS = cmdS.ExecuteNonQuery();
            }

            string query = @"INSERT INTO T_DSR(DSR_ID,DSR_NAME,DSR_PWD,MOBILE_NO,EMAIL_ADDRESS,DIST_ID,STATUS,ENTRY_DATE,ENTRY_BY)
                            VALUES ('" + rmId + "','" + rmName + "','" + pwd + "','" + Phone + "','" + emailAddress + "','" + distId + "','" + status + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "')";
            OracleCommand cmd = new OracleCommand(query, conn);

            int c = cmd.ExecuteNonQuery();
            if (c > 0)
            {
                msg = "Successful!";
            }
            else
            {
                msg = "Not Successful";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod(EnableSession = true)]
    public static string AddDMInfo(string dmId, string dmName, string pwd, string distId, string Phone, string emailAddress, string status, string  country, string division, string zone, string divisionName, string zoneName)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT DM_ID FROM T_DM WHERE DM_ID='" + dmId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["DM_ID"].ToString() != "")
            {
                string qrS = @"UPDATE T_DM SET DM_NAME='" + dmName + "',DM_PWD='" + pwd + "',MOBILE_NO='" + Phone + "',EMAIL_ADDRESS='" + emailAddress + "',DIST_ID='" + distId + "',DIVISION_ID='" + division + "',DIVISION_NAME='" + divisionName + "',ZONE_ID='" + zone + "',ZONE_NAME='" + zoneName + "',STATUS='" + status + "' WHERE DM_ID='" + dmId + "'";
                OracleCommand cmdS = new OracleCommand(qrS, conn);
                int cS = cmdS.ExecuteNonQuery();
                if (cS > 0)
                {
                    msg = "Successful!";
                }
                else
                {
                    msg = "Not Successful";
                }
            }
            else
            {
                string query = @"INSERT INTO T_DM(DM_ID,DM_NAME,DM_PWD,MOBILE_NO,EMAIL_ADDRESS,DIST_ID,STATUS,ENTRY_DATE,ENTRY_BY,COUNTRY_NAME,DIVISION_ID,ZONE_ID,DIVISION_NAME,ZONE_NAME)
                            VALUES ('" + dmId + "','" + dmName + "','" + pwd + "','" + Phone + "','" + emailAddress + "','" + distId + "','" + status + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + country + "','" + division + "','" + zone + "','" + divisionName + "','" + zoneName + "')";
                OracleCommand cmd = new OracleCommand(query, conn);

                int c = cmd.ExecuteNonQuery();
                if (c > 0)
                {
                    msg = "Successful!";
                }
                else
                {
                    msg = "Not Successful";
                }
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }  
        
    
    [WebMethod(EnableSession = true)]
    public static string AddDMSRMapping(string dmId, string srId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qrUp = @"SELECT * FROM T_DM_SR WHERE DM_ID='" + dmId + "' AND SR_ID='" + srId + "'"; ;
            OracleCommand cmdUp = new OracleCommand(qrUp, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmdUp);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int u = ds.Tables[0].Rows.Count;
            if (u > 0 && ds.Tables[0].Rows[0]["SR_ID"].ToString() != "")
            {
                string queryC = @"UPDATE T_DM_SR SET DM_ID='" + dmId + "' WHERE SR_ID ='" + srId + "'";
                OracleCommand cmdC = new OracleCommand(queryC, conn);
                int cC = cmdC.ExecuteNonQuery();
                if (cC > 0)
                {
                    msg = "Successful!";
                }
                else
                {
                    msg = "Not Successful";
                }
            }
            else
            {

                string query = @"INSERT INTO T_DM_SR(DM_ID,SR_ID)
                            VALUES ('" + dmId + "','" + srId + "')";
                OracleCommand cmd = new OracleCommand(query, conn);

                int c = cmd.ExecuteNonQuery();
                if (c > 0)
                {
                    msg = "Successful!";
                }
                else
                {
                    msg = "Not Successful";
                }
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }  
    
    
    [WebMethod(EnableSession = true)]
    public static string AddTSMSRMapping(string tsmId, string srId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = "DELETE FROM T_TSM_SR WHERE SR_ID='" + srId.Trim() + "'";
            OracleCommand cmdT = new OracleCommand(qr, conn);
            int k = cmdT.ExecuteNonQuery();

            string query = @"INSERT INTO T_TSM_SR(SR_ID,TSM_ID)
                            VALUES ('" + srId + "','" + tsmId + "')";
            OracleCommand cmd = new OracleCommand(query, conn);

            int c = cmd.ExecuteNonQuery();
            if (c > 0)
            {
                msg = "Successful!";
            }
            else
            {
                msg = "Not Successful";
            }


            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }  

    
    [WebMethod(EnableSession = true)]
    public static string AddRouteInfo(string routeId, string rmName, string country, string division, string zone, string status)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (routeId == "")
            {
                query = @"INSERT INTO T_ROUTE(ROUTE_ID,ROUTE_NAME,ZONE_ID,DIVISION_ID,COUNTRY_NAME,STATUS,ENTRY_DATE,ENTRY_BY)
                            VALUES ((SALES.SEQ_ROUTE_ID.NEXTVAL),'" + rmName + "','" + zone + "','" + division + "','" + country + "','" + status + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "')";
            }
            else
            {
                query = @"UPDATE T_ROUTE SET ROUTE_NAME='" + rmName + "',ZONE_ID='" + zone + "',DIVISION_ID='" + division + "',COUNTRY_NAME='" + country + "',STATUS='" + status + "' WHERE ROUTE_ID='" + routeId + "'";
            }
            OracleCommand cmd = new OracleCommand(query, conn);

            int c = cmd.ExecuteNonQuery();
            if (c > 0)
            {
                msg = "Successful!";
            }
            else
            {
                msg = "Not Successful";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }  
    
   
    [WebMethod(EnableSession = true)]
    public static string AddOutletInfo(string outletId, string outletName, string outletAddress, string outletPropritorName, string outletNameBangla, string outletAddressBangla, string outletPropritorNameBangla, string outletPhone, string outletEmailAddress, string country, string division, string zone, string route, string fridge, string signboard, string rack, string category, string grade, string status, string srId, string product)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = "";
            if (outletId == "")
            {

                query = @"INSERT INTO T_OUTLET(OUTLET_ID,OUTLET_NAME,OUTLET_ADDRESS,PROPRITOR_NAME,MOBILE_NUMBER,EMAIL_ADDRESS,COUNTRY_NAME,DIVISION_ID,ZONE_ID,ROUTE_ID,FRIDGE,SIGNBOARD,RACK,STATUS,ENTRY_DATE,ENTRY_BY, OUTLET_BL_NAME,ADDRESS_BL,PROPRITOR_BL,CATEGORY_NAME,GRADE,SR_ID,PRODUCTS)
                            VALUES ((SALES.SEQ_OUTLET_ID.NEXTVAL),'" + outletName + "','" + outletAddress + "','" + outletPropritorName + "','" + outletPhone + "','" + outletEmailAddress + "','" + country + "','" + division + "','" + zone + "','" + route + "','" + fridge + "','" + signboard + "','" + rack + "','" + status + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + outletNameBangla + "','" + outletAddressBangla + "','" + outletPropritorNameBangla + "','" + category + "','" + grade + "','" + srId + "','" + product + "')";
            }
            else
            {
                query = @"UPDATE T_OUTLET SET OUTLET_NAME='" + outletName + "',OUTLET_ADDRESS='" + outletAddress + "',PROPRITOR_NAME='" + outletPropritorName + "',MOBILE_NUMBER='" + outletPhone + "',EMAIL_ADDRESS='" + outletEmailAddress + "',COUNTRY_NAME='" + country + "',DIVISION_ID='" + division + "',ZONE_ID='" + zone + "',ROUTE_ID='" + route + "',FRIDGE='" + fridge + "',SIGNBOARD='" + signboard + "',RACK='" + rack + "',STATUS='" + status + "',OUTLET_BL_NAME='" + outletNameBangla + "',ADDRESS_BL='" + outletAddressBangla + "',PROPRITOR_BL='" + outletPropritorNameBangla + "',CATEGORY_NAME='" + category + "',GRADE='" + grade + "',PRODUCTS='" + product + "' WHERE SR_ID='" + srId + "' AND OUTLET_ID='" + outletId + "'";
            }

            OracleCommand cmd = new OracleCommand(query, conn);

            int c = cmd.ExecuteNonQuery();
            if (c > 0)
            {
                msg = "Successful!";
            }
            else
            {
                msg = "Not Successful";
            }


            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }  
                         

    [WebMethod(EnableSession = true)]
    public static string AddDivisionInfo(string divisionID, string divisionName, string country, string status)
    {
        string msg = "";
        string query = "";
        string currentUser = HttpContext.Current.Session["userid"].ToString();

        try
        {
            DataSet ds = new DataSet();

            string qr = @"SELECT DIVISION_ID FROM T_DIVISION WHERE DIVISION_ID='" + divisionID + "'";
            ds = CommonDBSvc.GetDataSet(qr);

            if (ds.Tables[0].Rows.Count > 0)
            {
                query = @"UPDATE [dbo].[T_DIVISION] SET DIVISION_NAME='" + divisionName + "', COUNTRY_ID='" + country + "', " +
                " STATUS='" + status + "',LAST_UPDATE=GETDATE(),ENTRY_BY='" + currentUser + "' WHERE DIVISION_ID='" + divisionID + "'";
            }

            else
            {
                query = @"INSERT INTO T_DIVISION(DIVISION_NAME,COUNTRY_ID,STATUS,LAST_UPDATE,ENTRY_BY)
                VALUES ('" + divisionName + "','" + country + "','" + status + "',GETDATE(),'" +currentUser+ "')";
            }

            msg = CommonDBSvc.ExecuteQuery(query);

        }
        catch (Exception ex) { }

        return msg;
    }

   
    [WebMethod(EnableSession = true)]
    public static string AddZoneInfo(string zoneId, string zoneName, string division, string country, string status)
    {
        string msg = "";
        string currentUser = HttpContext.Current.Session["userid"].ToString();

        try
        {

            string qr = @"SELECT ZONE_ID FROM T_ZONE WHERE ZONE_ID='" + zoneId.Trim() + "'"; 
            DataSet ds = new DataSet();
            ds = CommonDBSvc.GetDataSet(qr);


            string query = @"INSERT INTO T_ZONE(ZONE_ID,ZONE_NAME,DIVISION_ID,COUNTRY_NAME,STATUS,ENTRY_DATE,ENTRY_BY)
                            VALUES ('" + zoneId + "','" + zoneName + "','" + division + "','" + country + "','" + status + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "')";
            if (ds.Tables[0].Rows.Count > 0)
            {
                query = @"UPDATE [dbo].[T_ZONE] SET ZONE_NAME='" + zoneName + "', DIVISION_ID='" + division + "', " +
                " STATUS='" + status + "',LAST_UPDATE=GETDATE(), ENTRY_BY='" + currentUser + "' WHERE ZONE_ID='" + zoneId + "'";
            }

            else
            {
                query = @"INSERT INTO T_ZONE (ZONE_ID,ZONE_NAME,DIVISION_ID,STATUS,LAST_UPDATE,ENTRY_BY)
                VALUES ('" + zoneId + "','" + zoneName + "','"+division+"','" + status + "',GETDATE(),'" + currentUser + "')";
            }

            msg = CommonDBSvc.ExecuteQuery(query);
        }
        catch (Exception ex) 
        {
            msg = ex.ToString();
        }

        return msg;
    }  
    
    
    
    [WebMethod(EnableSession = true)]
    public static string AddSRRouteDayInfo(string srId, string country, string division, string zone, string day1, string day2, string day3, string day4, string day5, string day6, string route1, string route2, string route3, string route4, string route5, string route6, string route1Id, string route2Id, string route3Id, string route4Id, string route5Id, string route6Id)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT SR_ID FROM T_SR_ROUTE_DAY WHERE SR_ID='" + srId.Trim() + "' AND STATUS='Y'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["SR_ID"].ToString() != "")
            {
                /*string query1 = @"UPDATE T_SR_ROUTE_DAY SET ROUTE_ID='" + route1Id + "',ROUTE_NAME='" + route1 + "' WHERE DAY_NAME='" + day1.Trim() + "' AND COUNTRY_NAME='" + country.Trim() + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "' AND SR_ID='" + srId.Trim() + "'";                            
                OracleCommand cmd1 = new OracleCommand(query1, conn);
                int c1 = cmd1.ExecuteNonQuery();

                string query2 = @"UPDATE T_SR_ROUTE_DAY SET ROUTE_ID='" + route2Id + "',ROUTE_NAME='" + route2 + "' WHERE DAY_NAME='" + day2.Trim() + "' AND COUNTRY_NAME='" + country.Trim() + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "' AND SR_ID='" + srId.Trim() + "'";                            
                OracleCommand cmd2 = new OracleCommand(query2, conn);
                int c2 = cmd2.ExecuteNonQuery();

                string query3 = @"UPDATE T_SR_ROUTE_DAY SET ROUTE_ID='" + route3Id + "',ROUTE_NAME='" + route3 + "' WHERE DAY_NAME='" + day3.Trim() + "' AND COUNTRY_NAME='" + country.Trim() + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "' AND SR_ID='" + srId.Trim() + "'";                            
                OracleCommand cmd3 = new OracleCommand(query3, conn);
                int c3 = cmd3.ExecuteNonQuery();

                string query4 = @"UPDATE T_SR_ROUTE_DAY SET ROUTE_ID='" + route4Id + "',ROUTE_NAME='" + route4 + "' WHERE DAY_NAME='" + day4.Trim() + "' AND COUNTRY_NAME='" + country.Trim() + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "' AND SR_ID='" + srId.Trim() + "'";                            
                OracleCommand cmd4 = new OracleCommand(query4, conn);

                int c4 = cmd4.ExecuteNonQuery();

                string query5 = @"UPDATE T_SR_ROUTE_DAY SET ROUTE_ID='" + route5Id + "',ROUTE_NAME='" + route5 + "' WHERE DAY_NAME='" + day5.Trim() + "' AND COUNTRY_NAME='" + country.Trim() + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "' AND SR_ID='" + srId.Trim() + "'";                            
                OracleCommand cmd5 = new OracleCommand(query5, conn);

                int c5 = cmd5.ExecuteNonQuery();

                string query6 = @"UPDATE T_SR_ROUTE_DAY SET ROUTE_ID='" + route6Id + "',ROUTE_NAME='" + route6 + "' WHERE DAY_NAME='" + day6.Trim() + "' AND COUNTRY_NAME='" + country.Trim() + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "' AND SR_ID='" + srId.Trim() + "'";                            
                OracleCommand cmd6 = new OracleCommand(query6, conn);

                int c6 = cmd6.ExecuteNonQuery();*/

                string queryDel = @"DELETE FROM T_SR_ROUTE_DAY WHERE SR_ID='" + srId.Trim() + "'";
                OracleCommand cmdDel = new OracleCommand(queryDel, conn);
                int del = cmdDel.ExecuteNonQuery();

                 
            }
            //else
            //{
                string query1 = @"INSERT INTO T_SR_ROUTE_DAY(SR_ID,DAY_NAME,ROUTE_NAME,COUNTRY_NAME,DIVISION_ID,ZONE_ID,STATUS,ENTRY_DATE,ENTRY_BY,ROUTE_ID)
                            VALUES ('" + srId.Trim() + "','" + day1 + "','" + route1 + "','" + country + "','" + division + "','" + zone + "','Y',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + route1Id + "')";
                OracleCommand cmd1 = new OracleCommand(query1, conn);

                int c1 = cmd1.ExecuteNonQuery();

                string query2 = @"INSERT INTO T_SR_ROUTE_DAY(SR_ID,DAY_NAME,ROUTE_NAME,COUNTRY_NAME,DIVISION_ID,ZONE_ID,STATUS,ENTRY_DATE,ENTRY_BY,ROUTE_ID)
                            VALUES ('" + srId.Trim() + "','" + day2 + "','" + route2 + "','" + country + "','" + division + "','" + zone + "','Y',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + route2Id + "')";
                OracleCommand cmd2 = new OracleCommand(query2, conn);

                int c2 = cmd2.ExecuteNonQuery();

                string query3 = @"INSERT INTO T_SR_ROUTE_DAY(SR_ID,DAY_NAME,ROUTE_NAME,COUNTRY_NAME,DIVISION_ID,ZONE_ID,STATUS,ENTRY_DATE,ENTRY_BY,ROUTE_ID)
                            VALUES ('" + srId.Trim() + "','" + day3 + "','" + route3 + "','" + country + "','" + division + "','" + zone + "','Y',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + route3Id + "')";
                OracleCommand cmd3 = new OracleCommand(query3, conn);

                int c3 = cmd3.ExecuteNonQuery();

                string query4 = @"INSERT INTO T_SR_ROUTE_DAY(SR_ID,DAY_NAME,ROUTE_NAME,COUNTRY_NAME,DIVISION_ID,ZONE_ID,STATUS,ENTRY_DATE,ENTRY_BY,ROUTE_ID)
                            VALUES ('" + srId.Trim() + "','" + day4 + "','" + route4 + "','" + country + "','" + division + "','" + zone + "','Y',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + route4Id + "')";
                OracleCommand cmd4 = new OracleCommand(query4, conn);

                int c4 = cmd4.ExecuteNonQuery();

                string query5 = @"INSERT INTO T_SR_ROUTE_DAY(SR_ID,DAY_NAME,ROUTE_NAME,COUNTRY_NAME,DIVISION_ID,ZONE_ID,STATUS,ENTRY_DATE,ENTRY_BY,ROUTE_ID)
                            VALUES ('" + srId.Trim() + "','" + day5 + "','" + route5 + "','" + country + "','" + division + "','" + zone + "','Y',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + route5Id + "')";
                OracleCommand cmd5 = new OracleCommand(query5, conn);

                int c5 = cmd5.ExecuteNonQuery();

                string query6 = @"INSERT INTO T_SR_ROUTE_DAY(SR_ID,DAY_NAME,ROUTE_NAME,COUNTRY_NAME,DIVISION_ID,ZONE_ID,STATUS,ENTRY_DATE,ENTRY_BY,ROUTE_ID)
                            VALUES ('" + srId.Trim() + "','" + day6 + "','" + route6 + "','" + country + "','" + division + "','" + zone + "','Y',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + route6Id + "')";
                OracleCommand cmd6 = new OracleCommand(query6, conn);

                int c6 = cmd6.ExecuteNonQuery();

                msg = "Successful!";
            //}

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    

    [WebMethod(EnableSession = true)]
    public static string AddCompanyInfo(string country, string comId, string comName, string comNickName, string motherCompany, string status)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT COMPANY_ID FROM T_COMPANY WHERE COMPANY_ID='" + comId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int K = ds.Tables[0].Rows.Count;
            if (K > 0 && ds.Tables[0].Rows[0]["COMPANY_ID"].ToString() != "")
            {
                string qrS = @"UPDATE T_COMPANY SET COMPANY_FULL_NAME='" + comName + "',COMPANY_NICK_NAME='" + comNickName + "',MOTHER_COMPANY='" + motherCompany + "',COUNTRY_NAME='" + country + "',STATUS='" + status + "' WHERE COMPANY_ID='" + comId + "'";
                OracleCommand cmdS = new OracleCommand(qrS, conn);
                int cS = cmdS.ExecuteNonQuery();
                if (cS > 0)
                {
                    msg = "Successful!";
                }
                else
                {
                    msg = "NotExist";
                }
            }
            else
            {
                string query = @"INSERT INTO T_COMPANY(COMPANY_ID,COMPANY_FULL_NAME,COMPANY_NICK_NAME,MOTHER_COMPANY,ENTRY_DATE,ENTRY_BY,STATUS,COUNTRY_NAME)
                           VALUES('" + comId + "','" + comName + "','" + comNickName + "','" + motherCompany + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + status + "','" + country + "')";
                OracleCommand cmd = new OracleCommand(query, conn);
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    msg = "Successful!";
                }
                else
                {
                    msg = "NotExist";
                }
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod(EnableSession = true)]
    public static string AddSRTarget(string srId, string targetAmt, string month, string year)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT SR_ID FROM T_SR_TARGET WHERE SR_ID='" + srId + "' AND MONTH_NAME='" + month + "' AND YEAR='" + year + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int K = ds.Tables[0].Rows.Count;
            if (K > 0 && ds.Tables[0].Rows[0]["SR_ID"].ToString() != "")
            {
                string qrS = @"DELETE FROM T_SR_TARGET WHERE SR_ID='" + srId + "' AND MONTH_NAME='" + month + "' AND YEAR='" + year + "'";
                OracleCommand cmdS = new OracleCommand(qrS, conn);
                int cS = cmdS.ExecuteNonQuery();
            }

            string query = @"INSERT INTO T_SR_TARGET(SR_ID,TARGET_AMT,MONTH_NAME,YEAR, ENTRY_DATE,ENTRY_BY)
                           VALUES('" + srId + "','" + targetAmt + "','" + month + "','" + year + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "')";    
            OracleCommand cmd = new OracleCommand(query, conn);
            int i = cmd.ExecuteNonQuery();
            if (i > 0) 
            {
                msg = "Successful!";
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


    [WebMethod(EnableSession = true)]
    public static string AddTSMTarget(string tsmId, string targetAmt, string month, string year)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT TSM_ID FROM T_TSM_TARGET WHERE TSM_ID='" + tsmId + "' AND MONTH_NAME='" + month + "' AND YEAR='" + year + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int K = ds.Tables[0].Rows.Count;
            if (K > 0 && ds.Tables[0].Rows[0]["TSM_ID"].ToString() != "")
            {
                string qrS = @"DELETE FROM T_TSM_TARGET WHERE TSM_ID='" + tsmId + "' AND MONTH_NAME='" + month + "' AND YEAR='" + year + "'";
                OracleCommand cmdS = new OracleCommand(qrS, conn);
                int cS = cmdS.ExecuteNonQuery();
            }

            string query = @"INSERT INTO T_TSM_TARGET(TSM_ID,TARGET_AMT,MONTH_NAME,YEAR,ENTRY_BY,ENTRY_DATE)
                           VALUES('" + tsmId + "','" + targetAmt + "','" + month + "','" + year + "','" + HttpContext.Current.Session["userid"].ToString() + "',TO_DATE(SYSDATE))";
            OracleCommand cmd = new OracleCommand(query, conn);
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                msg = "Successful!";
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


    [WebMethod(EnableSession = true)]
    public static string AddRMTarget(string rmId, string targetAmt, string month, string year)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT RM_ID FROM T_RM_TARGET WHERE RM_ID='" + rmId + "' AND MONTH_NAME='" + month + "' AND YEAR='" + year + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int K = ds.Tables[0].Rows.Count;
            if (K > 0 && ds.Tables[0].Rows[0]["RM_ID"].ToString() != "")
            {
                string qrS = @"DELETE FROM T_RM_TARGET WHERE RM_ID='" + rmId + "' AND MONTH_NAME='" + month + "' AND YEAR='" + year + "'";
                OracleCommand cmdS = new OracleCommand(qrS, conn);
                int cS = cmdS.ExecuteNonQuery();
            }

            string query = @"INSERT INTO T_RM_TARGET(RM_ID,TARGET_AMT,MONTH_NAME,YEAR,ENTRY_BY,ENTRY_DATE)
                           VALUES('" + rmId + "','" + targetAmt + "','" + month + "','" + year + "','" + HttpContext.Current.Session["userid"].ToString() + "',TO_DATE(SYSDATE))";
            OracleCommand cmd = new OracleCommand(query, conn);
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                msg = "Successful!";
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

    [WebMethod(EnableSession = true)]
    public static string AddHOSTarget(string hosId, string targetAmt, string month, string year)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT HOS_ID FROM T_HOS_TARGET WHERE HOS_ID='" + hosId + "' AND MONTH_NAME='" + month + "' AND YEAR='" + year + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int K = ds.Tables[0].Rows.Count;
            if (K > 0 && ds.Tables[0].Rows[0]["HOS_ID"].ToString() != "")
            {
                string qrS = @"DELETE FROM T_HOS_TARGET WHERE HOS_ID='" + hosId + "' AND MONTH_NAME='" + month + "' AND YEAR='" + year + "'";
                OracleCommand cmdS = new OracleCommand(qrS, conn);
                int cS = cmdS.ExecuteNonQuery();
            }

            string query = @"INSERT INTO T_HOS_TARGET(HOS_ID,TARGET_AMT,MONTH_NAME,YEAR,ENTRY_BY,ENTRY_DATE)
                           VALUES('" + hosId + "','" + targetAmt + "','" + month + "','" + year + "','" + HttpContext.Current.Session["userid"].ToString() + "',TO_DATE(SYSDATE))";
            OracleCommand cmd = new OracleCommand(query, conn);
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                msg = "Successful!";
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


    [WebMethod(EnableSession = true)]
    public static string GetOrderedItem(string srid, string routeId, string outletId, string orderDate, string orderType)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
//            string qrF = @"SELECT DISTINCT T3.*,T4.OUTLET_NAME,T4.OUTLET_ID FROM
//                        (SELECT T1.*,T2.ITEM_NAME,T2.FACTOR FROM
//                        (SELECT DISTINCT ITEM_ID,ITEM_CTN,ITEM_QTY,OUT_PRICE,ENTRY_DATE,OUTLET_ID FROM T_TRANSACTION
//                        WHERE SR_ID='" + srid + "' AND IS_PROCESS='S' AND ORDER_TYPE='" + orderType + "' AND OUTLET_ID='" + outletId + "' AND ROUTE_ID='" + routeId + "' AND ENTRY_DATE=TO_DATE('" + orderDate + "','DD/MM/YYYY')) T1, ";
//            qrF = qrF + @"(SELECT ITEM_ID,ITEM_NAME,FACTOR FROM T_ITEM) T2
//                        WHERE T1.ITEM_ID=T2.ITEM_ID) T3,
//                        (SELECT OUTLET_ID,OUTLET_NAME FROM T_OUTLET) T4
//                        WHERE T3.OUTLET_ID=T4.OUTLET_ID";

            string qrF = @"SELECT T3.TRAN_ID,T3.ITEM_ID,T3.ITEM_NAME,T3.ITEM_CTN,T3.ITEM_QTY,T3.OUT_PRICE,T3.FACTOR,T3.ENTRY_DATE,T3.OUTLET_ID,T4.OUTLET_NAME FROM
                         (SELECT T1.*,T2.FACTOR,T2.ITEM_NAME FROM
                         (SELECT TRAN_ID,ITEM_ID,ITEM_CTN,ITEM_QTY,OUT_PRICE,OUTLET_ID,ENTRY_DATE FROM T_ORDER_DETAIL 
                         WHERE SR_ID='" + srid + "' AND ORDER_TYPE='OC' AND OUTLET_ID='" + outletId + "' AND ROUTE_ID='" + routeId + "' AND ENTRY_DATE=TO_DATE('" + orderDate + "','DD/MM/YYYY')) T1, ";
            qrF = qrF + @"(SELECT ITEM_ID,ITEM_NAME,FACTOR FROM T_ITEM) T2
                         WHERE T1.ITEM_ID=T2.ITEM_ID) T3,  
                         (SELECT OUTLET_ID,OUTLET_NAME FROM T_OUTLET) T4
                         WHERE T3.OUTLET_ID=T4.OUTLET_ID";

            OracleCommand cmdF = new OracleCommand(qrF, conn);
            OracleDataAdapter daF = new OracleDataAdapter(cmdF);
            DataSet dsF = new DataSet();
            daF.Fill(dsF);
            
            int c = dsF.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {                     
                    string itemId = dsF.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string itemName = dsF.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    string qtyCtn = dsF.Tables[0].Rows[i]["ITEM_CTN"].ToString();
                    string qty = dsF.Tables[0].Rows[i]["ITEM_QTY"].ToString();
                    string factor = dsF.Tables[0].Rows[i]["FACTOR"].ToString();
                    string price = dsF.Tables[0].Rows[i]["OUT_PRICE"].ToString();
                    string outletName = dsF.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                    string oltId = dsF.Tables[0].Rows[i]["OUTLET_ID"].ToString();
                    string TRAN_ID = dsF.Tables[0].Rows[i]["TRAN_ID"].ToString();

                    double q = (double)(Convert.ToInt32(qty) / Convert.ToInt32(factor));


                    msg = msg + ";" + orderDate + ";" + itemId + ";" + itemName + ";" + qtyCtn + ";" + qty + ";" + factor + ";" + price + ";" + outletName + ";" + oltId + ";" + TRAN_ID;
                }
            }

        }
        catch(Exception ex)
        {
        }
            
        return msg;
    }


    [WebMethod(EnableSession = true)]
    public static string GetOrderedItemByOutletSR(string srid, string outletId, string orderDate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
              
            string qrF = @"SELECT T3.TRAN_ID,T3.ITEM_ID,T3.ITEM_NAME,T3.ITEM_CTN,T3.ITEM_QTY,T3.OUT_PRICE,T3.FACTOR,T3.ENTRY_DATE,T3.OUTLET_ID,T4.OUTLET_NAME FROM
                         (SELECT T1.*,T2.FACTOR,T2.ITEM_NAME FROM
                         (SELECT TRAN_ID,ITEM_ID,ITEM_CTN,ITEM_QTY,OUT_PRICE,OUTLET_ID,ENTRY_DATE FROM T_ORDER_DETAIL 
                         WHERE SR_ID='" + srid + "' AND OUTLET_ID='" + outletId + "' AND ENTRY_DATE=TO_DATE('" + orderDate + "','DD/MM/YYYY')) T1, ";
            qrF = qrF + @"(SELECT ITEM_ID,ITEM_NAME,FACTOR FROM T_ITEM) T2
                         WHERE T1.ITEM_ID=T2.ITEM_ID) T3,  
                         (SELECT OUTLET_ID,OUTLET_NAME FROM T_OUTLET) T4
                         WHERE T3.OUTLET_ID=T4.OUTLET_ID";

            OracleCommand cmdF = new OracleCommand(qrF, conn);
            OracleDataAdapter daF = new OracleDataAdapter(cmdF);
            DataSet dsF = new DataSet();
            daF.Fill(dsF);

            int c = dsF.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string itemId = dsF.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string itemName = dsF.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    string qtyCtn = dsF.Tables[0].Rows[i]["ITEM_CTN"].ToString();
                    string qty = dsF.Tables[0].Rows[i]["ITEM_QTY"].ToString();
                    string factor = dsF.Tables[0].Rows[i]["FACTOR"].ToString();
                    string price = dsF.Tables[0].Rows[i]["OUT_PRICE"].ToString();
                    string outletName = dsF.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                    string oltId = dsF.Tables[0].Rows[i]["OUTLET_ID"].ToString();
                    string TRAN_ID = dsF.Tables[0].Rows[i]["TRAN_ID"].ToString();

                    double q = (double)(Convert.ToInt32(qty) / Convert.ToInt32(factor));


                    msg = msg + ";" + orderDate + ";" + itemId + ";" + itemName + ";" + qtyCtn + ";" + qty + ";" + factor + ";" + price + ";" + outletName + ";" + oltId + ";" + TRAN_ID;
                }
            }

        }
        catch (Exception ex)
        {
        }

        return msg;
    }
    
    
    [WebMethod(EnableSession = true)]
    public static string GetOrderItemInfo(string itemId, string outletId, string orderDate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string qrF = @"SELECT DISTINCT T5.*,T6.SR_NAME FROM
                        (SELECT DISTINCT T3.*,T4.OUTLET_NAME FROM
                                                (SELECT T1.*,T2.ITEM_NAME,T2.FACTOR FROM
                                                (SELECT SR_ID,ITEM_ID,ITEM_CTN,ITEM_QTY,OUT_PRICE,ENTRY_DATE,OUTLET_ID FROM T_TRANSACTION
                                                WHERE ORDER_TYPE='OC' AND OUTLET_ID='" + outletId + "' AND ITEM_ID='" + itemId + "' AND ENTRY_DATE=TO_DATE('" + orderDate + "','DD/MM/YYYY')) T1, ";
            qrF = qrF + @"(SELECT ITEM_ID,ITEM_NAME,FACTOR FROM T_ITEM) T2
                          WHERE T1.ITEM_ID=T2.ITEM_ID) T3,
                          (SELECT OUTLET_ID,OUTLET_NAME FROM T_OUTLET) T4
                          WHERE T3.OUTLET_ID=T4.OUTLET_ID) T5,
                          (SELECT SR_ID,SR_NAME FROM T_SR_INFO) T6
                          WHERE T5.SR_ID=T6.SR_ID ";

            OracleCommand cmdF = new OracleCommand(qrF, conn);
            OracleDataAdapter daF = new OracleDataAdapter(cmdF);
            DataSet dsF = new DataSet();
            daF.Fill(dsF);
            
            int c = dsF.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string srId = dsF.Tables[0].Rows[i]["SR_ID"].ToString();
                    string srName = dsF.Tables[0].Rows[i]["SR_NAME"].ToString();
                    string itemIds = dsF.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string itemName = dsF.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    string qtyCtn = dsF.Tables[0].Rows[i]["ITEM_CTN"].ToString();
                    string qty = dsF.Tables[0].Rows[i]["ITEM_QTY"].ToString();
                    string factor = dsF.Tables[0].Rows[i]["FACTOR"].ToString();
                    string price = dsF.Tables[0].Rows[i]["OUT_PRICE"].ToString();
                    string outletName = dsF.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                    string oltId = dsF.Tables[0].Rows[i]["OUTLET_ID"].ToString();

                    double q = (double)(Convert.ToInt32(qty) / Convert.ToInt32(factor));


                    msg = msg + ";" + srId + ";" + srName + ";" + orderDate + ";" + itemId + ";" + itemName + ";" + qtyCtn + ";" + qty + ";" + factor + ";" + price + ";" + outletName + ";" + oltId;
                }
            }

        }
        catch(Exception ex)
        {
        }
            
        return msg;
    }


    [WebMethod(EnableSession = true)]
    public static string ActiveOrderedItem(string srid, string orderDate, string orderType)
    {
         string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
 
                //---- order detail-------------                

                string qrO = @"SELECT DISTINCT OUTLET_ID FROM T_TRANSACTION WHERE IS_PROCESS = 'P' AND SR_ID='" + srid + "' AND ENTRY_DATE=TO_DATE('" + orderDate.Trim() + "','DD/MM/YYYY')";
                OracleCommand cmdO = new OracleCommand(qrO, conn);
                OracleDataAdapter daO = new OracleDataAdapter(cmdO);
                DataSet dsO = new DataSet();
                daO.Fill(dsO);
                int c = dsO.Tables[0].Rows.Count;
                if (c > 0 && dsO.Tables[0].Rows[0]["OUTLET_ID"].ToString() != "")
                {
                    for (int k = 0; k < c; k++)
                    {
                        string outletId = dsO.Tables[0].Rows[k]["OUTLET_ID"].ToString();
                        string orderTypes = "";
                        string routeId = "";

                        string qrFs = @"SELECT * FROM T_TRANSACTION WHERE IS_PROCESS = 'P' AND OUTLET_ID ='" + outletId + "' AND SR_ID='" + srid + "' AND ENTRY_DATE=TO_DATE('" + orderDate.Trim() + "','DD/MM/YYYY')";
                        OracleCommand cmdFs = new OracleCommand(qrFs, conn);
                        OracleDataAdapter daFs = new OracleDataAdapter(cmdFs);
                        DataSet dsFs = new DataSet();
                        daFs.Fill(dsFs);
                        int f = dsFs.Tables[0].Rows.Count;
                        if (f > 0 && dsFs.Tables[0].Rows[0]["OUTLET_ID"].ToString() != "")
                        {
                            orderTypes = dsFs.Tables[0].Rows[0]["ORDER_TYPE"].ToString();
                            routeId = dsFs.Tables[0].Rows[0]["ROUTE_ID"].ToString();

                            string qrSeq = @"SELECT SEQ_ORDER_ID.NEXTVAL FROM DUAL";
                            OracleCommand cmdSeq = new OracleCommand(qrSeq, conn);
                            OracleDataAdapter daSeq = new OracleDataAdapter(cmdSeq);
                            DataSet dsSeq = new DataSet();
                            daSeq.Fill(dsSeq);
                            string seq = dsSeq.Tables[0].Rows[0][0].ToString();

                            string d = DateTime.Now.Day.ToString();
                            d = d.Length == 1 ? "0" + d : d;
                            string m = DateTime.Now.Month.ToString();
                            m = m.Length == 1 ? "0" + m : m;
                            string yr = DateTime.Now.Year.ToString();

                            string invoiceNo = orderTypes + "-" + d + m + yr + "-" + seq;

                            string qr = @"INSERT INTO T_ORDER_HEADER(TRAN_ID,SR_ID,OUTLET_ID,ROUTE_ID,ENTRY_DATE,ENTRY_DATETIME) 
                            VALUES('" + invoiceNo + "','" + srid + "','" + outletId + "','" + routeId + "',TO_DATE(SYSDATE),SYSDATE)";
                            OracleCommand cmdQr = new OracleCommand(qr, conn);
                            int q = cmdQr.ExecuteNonQuery();
                            if (q > 0)
                            {
                                for (int i = 0; i < f; i++)
                                {
                                    string itemIds = dsFs.Tables[0].Rows[i]["ITEM_ID"].ToString().Trim();
                                    string itemPcss = dsFs.Tables[0].Rows[i]["ITEM_QTY"].ToString().Trim();
                                    string itemCtns = dsFs.Tables[0].Rows[i]["ITEM_CTN"].ToString().Trim();
                                    string prices = dsFs.Tables[0].Rows[i]["OUT_PRICE"].ToString().Trim();

                                    string lat = dsFs.Tables[0].Rows[i]["LATITUDE"].ToString();
                                    string lon = dsFs.Tables[0].Rows[i]["LONGITUDE"].ToString();

                                    string qrDs = @"INSERT INTO T_ORDER_DETAIL(TRAN_ID,SR_ID,ITEM_ID,ITEM_QTY,ITEM_CTN,OUT_PRICE,OUTLET_ID,ROUTE_ID,ENTRY_DATE,ENTRY_DATETIME,ORDER_TYPE,STATUS,IS_CANCEL,IS_PROCESS,LATITUDE,LONGITUDE) 
                                        VALUES('" + invoiceNo + "','" + srid + "','" + itemIds + "','" + itemPcss + "','" + itemCtns + "','" + prices + "','" + outletId + "','" + routeId + "',TO_DATE(SYSDATE),SYSDATE,'" + orderTypes + "','Y','N','P','" + lat + "','" + lon + "')";
                                    OracleCommand cmdQrDs = new OracleCommand(qrDs, conn);
                                    int qDs = cmdQrDs.ExecuteNonQuery();
                                    if (qDs > 0)
                                    {
                                        msg = "Successful!";
                                    }
                                    else
                                    {
                                        msg = "Not Successful";
                                    }
                                }
                            }
                        }

                        if (msg == "Successful!")
                        {
                            string qrUpD = @"UPDATE T_TRANSACTION SET IS_PROCESS='S' WHERE SR_ID='" + srid + "' AND OUTLET_ID='" + outletId + "' AND ROUTE_ID='" + routeId + "' AND ORDER_TYPE='" + orderTypes + "' AND ENTRY_DATE=TO_DATE('" + orderDate.Trim() + "','DD/MM/YYYY')";
                            OracleCommand cmdUp = new OracleCommand(qrUpD, conn);
                            int up = cmdUp.ExecuteNonQuery();
                            if (up > 0)
                            {
                                msg = "Successful!";
                            }
                        }
                    }
                }
            
        }
        catch (Exception ex)
        {

        }

        return msg;
    }
    
    
    [WebMethod(EnableSession = true)]
    public static string UpdateOrder(string srid, string outeltId, string itemId, string itemCtn, string itemPcs, string orderDate)
    {
        string msg = "Not Successful!";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"UPDATE T_ORDER_DETAIL SET ITEM_CTN='" + itemCtn.Trim() + "',ITEM_QTY='" + itemPcs.Trim() + "' WHERE SR_ID='" + srid + "' AND OUTLET_ID='" + outeltId.Trim() + "' AND ITEM_ID='" + itemId + "' AND ENTRY_DATE=TO_DATE('" + orderDate.Trim() + "','DD/MM/YYYY')";
            OracleCommand cmd = new OracleCommand(qr, conn);
            int c = cmd.ExecuteNonQuery();
            if (c > 0)
            {
                msg = "Successful!";
            }
        }
        catch (Exception ex)
        {

        }

        return msg;
    }


    [WebMethod(EnableSession = true)]
    public static string AddOrder(string tranId, string opt, string srid, string routeId, string outeltId, string itemId, string itemCtn, string itemPcs, string orderType, string orderDate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qrF = @"SELECT FACTOR,TP FROM T_ITEM WHERE ITEM_ID='" + itemId + "'";
            OracleCommand cmdF = new OracleCommand(qrF, conn);
            OracleDataAdapter daF = new OracleDataAdapter(cmdF);
            DataSet dsF = new DataSet();
            daF.Fill(dsF);
            string factor = "1";
            string tp = "0";
            if (dsF.Tables[0].Rows.Count > 0)
            {
                factor = dsF.Tables[0].Rows[0]["FACTOR"].ToString();
                tp = dsF.Tables[0].Rows[0]["TP"].ToString();
            }

            //int qty = (Convert.ToInt32(itemCtn) * Convert.ToInt32(factor)) + Convert.ToInt32(itemPcs);
            string qrD = "";
            string qrUp = "";
            string qrInsert = "";
            if (opt == "Add")
            {
                //--app query-------------
                //INSERT INTO SALES.T_TRANSACTION(TRAN_ID,SR_ID,ITEM_ID,ITEM_CTN,ITEM_QTY,OUT_PRICE,OUTLET_ID,ROUTE_ID,ENTRY_DATE,ENTRY_DATETIME,ORDER_TYPE,STATUS,IS_CANCEL,IS_PROCESS,LATITUDE,LONGITUDE) 
                //          "VALUES((SALES.SEQ_ORDER_ID.NEXTVAL),'" + srId + "','" + ItemCode + "','" + carton + "','" + pcs + "','" + itemPrice + "','" + outletId + "','" + PublicModel.getGlbRoute() + "',TO_DATE(SYSDATE),SYSDATE,'" + order_type + "','Y','N','P','"+PublicModel.getLatitute()+"','"+PublicModel.getLongitude()+"');";

                qrInsert = @"INSERT INTO T_TRANSACTION(TRAN_ID,SR_ID,ITEM_ID,ITEM_CTN,ITEM_QTY,OUT_PRICE,OUTLET_ID,ROUTE_ID,ENTRY_DATE,ENTRY_DATETIME,ORDER_TYPE,STATUS,IS_CANCEL,IS_PROCESS,LATITUDE,LONGITUDE) 
                            VALUES((SALES.SEQ_ORDER_ID.NEXTVAL),'" + srid.Trim() + "','" + itemId.Trim() + "','" + itemCtn.ToString().Trim() + "','" + itemPcs.ToString().Trim() + "','" + tp.Trim() + "','" + outeltId.Trim() + "','" + routeId.Trim() + "',TO_DATE('" + orderDate.Trim() + "','DD/MM/YYYY'),SYSDATE,'" + orderType.Trim() + "','Y','N','P','90.32545468751','43.245479874545')";
                OracleCommand cmdIn = new OracleCommand(qrInsert, conn);
                int cnt = cmdIn.ExecuteNonQuery();
                if (cnt > 0)
                {
                    msg = "Successful!";
                }
                else
                {
                    msg = "Not Successful!";
                }
            }
            else if (opt == "Edit")
            {
                //qrD = @"UPDATE T_TRANSACTION SET ITEM_CTN='" + itemCtn.ToString().Trim() + "', ITEM_QTY='" + itemPcs.ToString().Trim() + "',ORDER_TYPE='" + orderType.Trim() + "' WHERE SR_ID='" + srid.Trim() + "' AND ITEM_ID='" + itemId.Trim() + "' AND OUTLET_ID='" + outeltId.Trim() + "' AND ROUTE_ID='" + routeId.Trim() + "' AND ENTRY_DATE=TO_DATE('" + orderDate.Trim() + "','DD/MM/YYYY')";
                qrUp = @"UPDATE T_ORDER_DETAIL SET ITEM_CTN='" + itemCtn.ToString().Trim() + "', ITEM_QTY='" + itemPcs.ToString().Trim() + "',ORDER_TYPE='" + orderType.Trim() + "' WHERE TRAN_ID='" + tranId + "' AND SR_ID='" + srid.Trim() + "' AND ITEM_ID='" + itemId.Trim() + "' AND OUTLET_ID='" + outeltId.Trim() + "' AND ROUTE_ID='" + routeId.Trim() + "' AND ENTRY_DATE=TO_DATE('" + orderDate.Trim() + "','DD/MM/YYYY')";
                OracleCommand cmdQrD = new OracleCommand(qrUp, conn);
                int qD = cmdQrD.ExecuteNonQuery();
                if (qD > 0)
                {
                        msg = "Successful!";                  
                }
                else
                {
                    msg = "Not Successful!";
                }
            }            
        }
        catch (Exception ex)
        {
            msg = ex.ToString();
        }

        return msg;
    }
    
  
    [WebMethod(EnableSession = true)]
    public static string AddTradeProgram(string programID, string programName, string motherCompany, string ownCompany, string country, string itemGroupId, string itemGroupName, string classId, string className, string itemId, string itemName, string freeItems, string minQty, string maxQty, string freeCtn, string freePcs, string discount, string discountType, string programFromDate, string programToDate, string freeFromDate, string freeToDate, string status, string divisionId, string divisionName, string zoneId, string zoneName, string outletGrade, string freeType, string minQtyForDiscount)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            if (programID != "")
            {
                string qrF = @"SELECT * FROM T_TRADE_PROGRAM WHERE PROGRAM_ID='" + programID.Trim() + "'";
                OracleCommand cmdF = new OracleCommand(qrF, conn);
                OracleDataAdapter daF = new OracleDataAdapter(cmdF);
                DataSet dsF = new DataSet();
                daF.Fill(dsF);
                int ff = dsF.Tables[0].Rows.Count;
                if (ff > 0 && dsF.Tables[0].Rows[0]["ITEM_ID"].ToString() != "")
                {
                    string PROGRAM_ID = dsF.Tables[0].Rows[0]["PROGRAM_ID"].ToString();
                    //string qrU = @"UPDATE T_TRADE_PROGRAM SET PROGRAM_NAME='" + programName + "',FREE_ITEM_ID='" + freeItems.Trim() + "',FREE_ITEM_NAME='" + freeItems.Trim() + "',MIN_QTY='" + Convert.ToInt32(minQty.Trim()) + "',MAX_QTY='" + Convert.ToInt32(maxQty.Trim()) + "',FREE_CTN='" + Convert.ToInt32(freeCtn.Trim()) + "',FREE_PCS='" + Convert.ToInt32(freePcs.Trim()) + "',DISCOUNT='" + Convert.ToDouble(discount.Trim()) + "',DISCOUNT_TYPE='" + discountType.Trim() + "',MIN_QTY_DISCOUNT='" + minQtyForDiscount.Trim() + "',P_FROM_DATE=TO_DATE('" + programFromDate.Trim() + "','DD/MM/YYYY'),P_TO_DATE=TO_DATE('" + programToDate.Trim() + "','DD/MM/YYYY'),FREE_FROM_DATE=TO_DATE('" + freeFromDate.Trim() + "','DD/MM/YYYY'),FREE_TO_DATE=TO_DATE('" + freeToDate.Trim() + "','DD/MM/YYYY'),STATUS='" + status.Trim() + "',DIVISION_ID='" + divisionId.Trim() + "',DIVISION_NAME='" + divisionName.Trim() + "',ZONE_ID='" + zoneId.Trim() + "',ZONE_NAME='" + zoneName.Trim() + "',OUTLET_GRADE='" + outletGrade.Trim() + "',FREE_TYPE='" + freeType.Trim() + "' WHERE MOTHER_COMPANY='" + motherCompany.Trim() + "' AND OWN_COMPANY='" + ownCompany.Trim() + "' AND COUNTRY_NAME LIKE '%" + country + "%' AND ITEM_GROUP_ID='" + itemGroupId.Trim() + "' AND CLASS_ID='" + classId.Trim() + "' AND ITEM_ID='" + itemId.Trim() + "'";
                    string qrU = @"UPDATE T_TRADE_PROGRAM SET PROGRAM_NAME='" + programName + "',FREE_ITEM_ID='" + freeItems.Trim() + "',FREE_ITEM_NAME='" + freeItems.Trim() + "',MIN_QTY='" + Convert.ToInt32(minQty.Trim()) + "',MAX_QTY='" + Convert.ToInt32(maxQty.Trim()) + "',FREE_CTN='" + Convert.ToInt32(freeCtn.Trim()) + "',FREE_PCS='" + Convert.ToInt32(freePcs.Trim()) + "',DISCOUNT='" + Convert.ToDouble(discount.Trim()) + "',DISCOUNT_TYPE='" + discountType.Trim() + "',MIN_QTY_DISCOUNT='" + minQtyForDiscount.Trim() + "',P_FROM_DATE=TO_DATE('" + programFromDate.Trim() + "','DD/MM/YYYY'),P_TO_DATE=TO_DATE('" + programToDate.Trim() + "','DD/MM/YYYY'),FREE_FROM_DATE=TO_DATE('" + freeFromDate.Trim() + "','DD/MM/YYYY'),FREE_TO_DATE=TO_DATE('" + freeToDate.Trim() + "','DD/MM/YYYY'),STATUS='" + status.Trim() + "',DIVISION_ID='" + divisionId.Trim() + "',DIVISION_NAME='" + divisionName.Trim() + "',ZONE_ID='" + zoneId.Trim() + "',ZONE_NAME='" + zoneName.Trim() + "',OUTLET_GRADE='" + outletGrade.Trim() + "',FREE_TYPE='" + freeType.Trim() + "' WHERE PROGRAM_ID='" + PROGRAM_ID.Trim() + "'";
                    OracleCommand cmdQrD = new OracleCommand(qrU, conn);
                    int qD = cmdQrD.ExecuteNonQuery();
                    if (qD > 0)
                    {
                        msg = "Successful!";
                    }
                }
            }
            else
            {
                string qrD = @"INSERT INTO T_TRADE_PROGRAM(PROGRAM_NAME,MOTHER_COMPANY,OWN_COMPANY,COUNTRY_NAME,ITEM_GROUP_ID,GROUP_NAME,CLASS_ID,CLASS_NAME,ITEM_ID,ITEM_NAME,FREE_ITEM_ID,FREE_ITEM_NAME,MIN_QTY,MAX_QTY,FREE_CTN,FREE_PCS,DISCOUNT,DISCOUNT_TYPE,P_FROM_DATE,P_TO_DATE,FREE_FROM_DATE,FREE_TO_DATE,ENTRY_BY,DIVISION_ID,DIVISION_NAME,ZONE_ID,ZONE_NAME,OUTLET_GRADE,STATUS,FREE_TYPE,MIN_QTY_DISCOUNT,PROGRAM_ID) 
                            VALUES('" + programName + "','" + motherCompany.Trim() + "','" + ownCompany.Trim() + "','" + country.Trim() + "','" + itemGroupId.Trim() + "','" + itemGroupName.Trim() + "','" + classId.Trim() + "','" + className + "','" + itemId.Trim() + "','" + itemName + "','" + freeItems.Trim() + "','" + freeItems.Trim() + "','" + Convert.ToInt32(minQty.Trim()) + "','" + Convert.ToInt32(maxQty.Trim()) + "','" + Convert.ToInt32(freeCtn.Trim()) + "','" + Convert.ToInt32(freePcs.Trim()) + "','" + Convert.ToDouble(discount.Trim()) + "','" + discountType.Trim() + "',TO_DATE('" + programFromDate.Trim() + "','DD/MM/YYYY'),TO_DATE('" + programToDate.Trim() + "','DD/MM/YYYY'),TO_DATE('" + freeFromDate.Trim() + "','DD/MM/YYYY'),TO_DATE('" + freeToDate.Trim() + "','DD/MM/YYYY'),'','" + divisionId.Trim() + "','" + divisionName.Trim() + "','" + zoneId.Trim() + "','" + zoneName.Trim() + "','" + outletGrade.Trim() + "','" + status.Trim() + "','" + freeType.Trim() + "','" + minQtyForDiscount.Trim() + "',(SALES.SEQ_TRADE_PROGRAM.NEXTVAL))";
                OracleCommand cmdQrD = new OracleCommand(qrD, conn);
                int qD = cmdQrD.ExecuteNonQuery();
                if (qD > 0)
                {
                    msg = "Successful";
                    /*string[] fItems = freeItems.Split(',');

                    if (fItems.Length > 1)
                    {
                        for (int i = 0; i < fItems.Length; i++)
                        {
                            string freeItemId = fItems[i].ToString();

                            string qrDs = @"INSERT INTO T_TRADE_PROGRAM_FREE_ITEM(PROGRAM_NAME,ITEM_ID,FREE_ITEM_ID,FREE_ITEM_NAME,FREE_QTY) 
                            VALUES('" + programName + "','" + itemId + "','" + freeItemId + "',(SELECT ITEM_NAME FROM T_ITEM WHERE ITEM_ID='" + freeItemId + "'),'" + freeQty + "')";
                            OracleCommand cmdQrDs = new OracleCommand(qrDs, conn);
                            int qDs = cmdQrDs.ExecuteNonQuery();
                            if (qDs > 0)
                            {
                                msg = "Successful!";
                            }
                        }
                    }
                    else
                    {
                        string qrDs = @"INSERT INTO T_TRADE_PROGRAM_FREE_ITEM(PROGRAM_NAME,ITEM_ID,FREE_ITEM_ID,FREE_ITEM_NAME,FREE_QTY) 
                            VALUES('" + programName + "','" + itemId + "','" + fItems[0].ToString().Trim() + "',(SELECT ITEM_NAME FROM T_ITEM WHERE ITEM_ID='" + fItems[0].ToString().Trim() + "'),'" + freeQty + "')";
                        OracleCommand cmdQrDs = new OracleCommand(qrDs, conn);
                        int qDs = cmdQrDs.ExecuteNonQuery();
                        if (qDs > 0)
                        {
                            msg = "Successful!";
                        }
                    }*/
                }
            }  
        }
        catch (Exception ex)
        {
            msg = ex.ToString();
        }

        return msg;
    }
   
   
    [WebMethod(EnableSession = true)]
    public static string AddDepotProductBalance(string ocNumber, string country, string depotId, string depotName, string itemGroupId, string groupName, string itemId, string itemName, string carton, string piece, string arrivalDate, string status)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qrS = @"SELECT * FROM T_DEPOT_BULK_ITEM_BALANCE WHERE OC_NUMBER='" + ocNumber + "' AND ITEM_ID='" + itemId + "'";
            OracleCommand cmdS = new OracleCommand(qrS, conn);
            OracleDataAdapter daS = new OracleDataAdapter(cmdS);
            DataSet dsS = new DataSet();
            daS.Fill(dsS);
            int i = dsS.Tables[0].Rows.Count;
            if (i > 0 && dsS.Tables[0].Rows[0][0].ToString() != "")
            {
                string qrUp = @"UPDATE SALES.T_DEPOT_BULK_ITEM_BALANCE SET ITEM_GROUP_ID='" + itemGroupId + "', GROUP_NAME='" + groupName + "', CARTON_QTY='" + carton + "', PCS_QTY='" + piece + "', DEPOT_ID='" + depotId + "', DEPOT_NAME='" + depotName + "',COUNTRY_NAME='" + country + "',ARRIVAL_DATE=TO_DATE('" + arrivalDate + "','DD/MM/YYYY'),STATUS='" + status + "' WHERE OC_NUMBER='" + ocNumber + "' AND ITEM_ID='" + itemId + "'";
                OracleCommand cmdUp = new OracleCommand(qrUp, conn);
                int qU = cmdUp.ExecuteNonQuery();
                if (qU > 0)
                {
                    string qrSSA = @"SELECT * FROM T_DEPOT_STOCK WHERE ITEM_ID='" + itemId + "' AND DEPOT_ID='" + depotId + "' AND COUNTRY_NAME LIKE '%" + country + "%'";
                    OracleCommand cmdSSA = new OracleCommand(qrSSA, conn);
                    OracleDataAdapter daSSA = new OracleDataAdapter(cmdSSA);
                    DataSet dsSSA = new DataSet();
                    daSSA.Fill(dsSSA);
                    int iSA = dsSSA.Tables[0].Rows.Count;
                    if (iSA > 0 && dsSSA.Tables[0].Rows[0][0].ToString() != "")
                    {                        
                        string upStockA = @"UPDATE T_DEPOT_STOCK SET CARTON_QTY=(SELECT SUM(CARTON_QTY) FROM T_DEPOT_BULK_ITEM_BALANCE WHERE ITEM_ID='" + itemId + "'), PCS_QTY=(SELECT SUM(PCS_QTY) FROM T_DEPOT_BULK_ITEM_BALANCE WHERE ITEM_ID='" + itemId + "') WHERE ITEM_ID='" + itemId + "' AND DEPOT_ID='" + depotId + "' AND COUNTRY_NAME LIKE '%" + country + "%'";
                        OracleCommand cmdUpStockA = new OracleCommand(upStockA, conn);
                        int qUstockA = cmdUpStockA.ExecuteNonQuery();
                        if (qUstockA > 0)
                        {
                            msg = "Successful!";
                        }
                    }
                    else
                    {
                        string qrStockInsertA = @"INSERT INTO SALES.T_DEPOT_STOCK(DEPOT_ID,DEPOT_NAME,COUNTRY_NAME,ITEM_ID,ITEM_NAME,ITEM_GROUP_ID,GROUP_NAME,CARTON_QTY,PCS_QTY,ENTRY_DATE) 
                            VALUES('" + depotId + "','" + depotName + "','" + country + "','" + itemId + "','" + itemName + "','" + itemGroupId + "','" + groupName + "','" + carton + "','" + piece + "',TO_DATE(SYSDATE))";

                        OracleCommand cmdStockInsertA = new OracleCommand(qrStockInsertA, conn);
                        int stkA = cmdStockInsertA.ExecuteNonQuery();
                        if (stkA > 0)
                        {
                            msg = "Successful!";
                        }
                    }
                }
            }
            else
            {
                string qrD = @"INSERT INTO SALES.T_DEPOT_BULK_ITEM_BALANCE(OC_NUMBER,ITEM_ID,ITEM_NAME,ITEM_GROUP_ID,GROUP_NAME,CARTON_QTY,PCS_QTY,DEPOT_ID,DEPOT_NAME,COUNTRY_NAME,ARRIVAL_DATE,STATUS,ENTRY_BY) 
                            VALUES('" + ocNumber + "','" + itemId + "','" + itemName + "','" + itemGroupId + "','" + groupName + "','" + carton + "','" + piece + "','" + depotId + "','" + depotName + "','" + country + "',TO_DATE('" + arrivalDate + "','DD/MM/YYYY'),'" + status + "','" + HttpContext.Current.Session["userid"].ToString() + "')";

                OracleCommand cmdQrD = new OracleCommand(qrD, conn);
                int qD = cmdQrD.ExecuteNonQuery();
                if (qD > 0)
                {
                    string qrSS = @"SELECT * FROM T_DEPOT_STOCK WHERE ITEM_ID='" + itemId + "' AND DEPOT_ID='" + depotId + "' AND COUNTRY_NAME LIKE '%" + country + "%'";
                    OracleCommand cmdSS = new OracleCommand(qrSS, conn);
                    OracleDataAdapter daSS = new OracleDataAdapter(cmdSS);
                    DataSet dsSS = new DataSet();
                    daSS.Fill(dsSS);
                    int iS = dsSS.Tables[0].Rows.Count;
                    if (iS > 0 && dsSS.Tables[0].Rows[0][0].ToString() != "")
                    {
                        string ctn = dsSS.Tables[0].Rows[0]["CARTON_QTY"].ToString();
                        string pcs = dsSS.Tables[0].Rows[0]["PCS_QTY"].ToString();

                        long totalCtn = Convert.ToInt64(ctn) + Convert.ToInt64(carton);
                        long totalPcs = Convert.ToInt64(pcs) + Convert.ToInt64(piece);

                        string upStock = @"UPDATE T_DEPOT_STOCK SET CARTON_QTY='" + totalCtn + "', PCS_QTY='" + totalPcs + "' WHERE ITEM_ID='" + itemId + "' AND DEPOT_ID='" + depotId + "' AND COUNTRY_NAME LIKE '%" + country + "%'";
                        OracleCommand cmdUpStock = new OracleCommand(upStock, conn);
                        int qUstock = cmdUpStock.ExecuteNonQuery();
                        if (qUstock > 0)
                        {
                            msg = "Successful!";
                        }
                    }
                    else
                    {
                        string qrStockInsert = @"INSERT INTO SALES.T_DEPOT_STOCK(DEPOT_ID,DEPOT_NAME,COUNTRY_NAME,ITEM_ID,ITEM_NAME,ITEM_GROUP_ID,GROUP_NAME,CARTON_QTY,PCS_QTY,ENTRY_DATE) 
                            VALUES('" + depotId + "','" + depotName + "','" + country + "','" + itemId + "','" + itemName + "','" + itemGroupId + "','" + groupName + "','" + carton + "','" + piece + "',TO_DATE(SYSDATE))";

                        OracleCommand cmdStockInsert = new OracleCommand(qrStockInsert, conn);
                        int stk = cmdStockInsert.ExecuteNonQuery();
                        if (stk > 0)
                        {
                            msg = "Successful!";
                        }
                    }

                }
            }
        }
        catch (Exception ex)
        {
            msg = ex.ToString();
        }

        return msg;
    }


    [WebMethod(EnableSession = true)]
    public static string ActiveOrder(string srid, string routeId, string outeltId, string orderType, string orderDate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qrFs = @"SELECT * FROM T_TRANSACTION WHERE IS_PROCESS = 'P' AND SR_ID='" + srid + "' AND OUTLET_ID='" + outeltId + "' AND ROUTE_ID='" + routeId + "' AND ORDER_TYPE='" + orderType + "' AND ENTRY_DATE=TO_DATE('" + orderDate + "','DD/MM/YYYY')";
            OracleCommand cmdFs = new OracleCommand(qrFs, conn);
            OracleDataAdapter daFs = new OracleDataAdapter(cmdFs);
            DataSet dsFs = new DataSet();
            daFs.Fill(dsFs);
            int c = dsFs.Tables[0].Rows.Count;
            if (c > 0 && dsFs.Tables[0].Rows[0]["SR_ID"].ToString() != "") 
            {
                string qrSeq = @"SELECT SEQ_ORDER_ID.NEXTVAL FROM DUAL";
                OracleCommand cmdSeq = new OracleCommand(qrSeq, conn);
                OracleDataAdapter daSeq = new OracleDataAdapter(cmdSeq);
                DataSet dsSeq = new DataSet();
                daSeq.Fill(dsSeq);
                string seq = dsSeq.Tables[0].Rows[0][0].ToString();

                string d = DateTime.Now.Day.ToString();
                d = d.Length == 1 ? "0" + d : d;
                string m = DateTime.Now.Month.ToString();
                m = m.Length == 1 ? "0" + m : m;
                string yr = DateTime.Now.Year.ToString();

                string invoiceNo = orderType + "-" + d + m + yr + "-" + seq;

                string qr = @"INSERT INTO T_ORDER_HEADER(TRAN_ID,SR_ID,OUTLET_ID,ROUTE_ID,ENTRY_DATE,ENTRY_DATETIME) 
                            VALUES('" + invoiceNo + "','" + srid + "','" + outeltId + "','" + routeId + "',TO_DATE(SYSDATE),SYSDATE)";
                OracleCommand cmdQr = new OracleCommand(qr, conn);
                int q = cmdQr.ExecuteNonQuery();
                if (q > 0)
                {
                    for (int i = 0; i < c; i++)
                    {
                        string itemId = dsFs.Tables[0].Rows[i]["ITEM_ID"].ToString();
                        string itemPcs = dsFs.Tables[0].Rows[i]["ITEM_QTY"].ToString();
                        string itemCtn = dsFs.Tables[0].Rows[i]["ITEM_CTN"].ToString();
                        string price = dsFs.Tables[0].Rows[i]["OUT_PRICE"].ToString();

                        string lat = dsFs.Tables[0].Rows[i]["LATITUDE"].ToString();
                        string lon = dsFs.Tables[0].Rows[i]["LONGITUDE"].ToString();

                        string qrD = @"INSERT INTO T_ORDER_DETAIL(TRAN_ID,SR_ID,ITEM_ID,ITEM_QTY,ITEM_CTN,OUT_PRICE,OUTLET_ID,ROUTE_ID,ENTRY_DATE,ENTRY_DATETIME,ORDER_TYPE,STATUS,IS_CANCEL,IS_PROCESS,LATITUDE,LONGITUDE) 
                                        VALUES('" + invoiceNo + "','" + srid + "','" + itemId + "','" + itemPcs + "','" + itemCtn + "','" + price + "','" + outeltId + "','" + routeId + "',TO_DATE(SYSDATE),SYSDATE,'" + orderType + "','Y','N','P','" + lat + "','" + lon + "')";
                        OracleCommand cmdQrD = new OracleCommand(qrD, conn);
                        int qD = cmdQrD.ExecuteNonQuery();
                        if (qD > 0)
                        {
                            msg = "Successful!";
                        }
                    }
                }                 
            }

            if (msg == "Successful!") 
            {
                string qrUp = @"UPDATE T_TRANSACTION SET IS_PROCESS='S' WHERE SR_ID='" + srid + "' AND OUTLET_ID='" + outeltId + "' AND ROUTE_ID='" + routeId + "' AND ORDER_TYPE='" + orderType + "' AND ENTRY_DATE=TO_DATE('" + orderDate + "','DD/MM/YYYY')";
                OracleCommand cmdUp = new OracleCommand(qrUp, conn);
                int up = cmdUp.ExecuteNonQuery();
                if (up > 0)
                {
                    msg = "Successful!";
                }
            }

        }
        catch (Exception ex)
        {
            msg = ex.ToString();
        }

        return msg;
    }
    


    [WebMethod]
    public static string GetSRName(string srId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT SR_ID,SR_NAME FROM T_SR_INFO WHERE SR_ID='" + srId + "'";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                string srid = ds.Tables[0].Rows[0]["SR_ID"].ToString();
                string srName = ds.Tables[0].Rows[0]["SR_NAME"].ToString();
                msg = srName;
            }
            else
            {
                msg = "No SR";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
    
    [WebMethod]
    public static string GetItemStock(string itemId, string ocNum)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = "";
            if (ocNum == "ocNum")
            {
                query = @"SELECT OC_NUMBER,ITEM_ID,ITEM_NAME,ITEM_GROUP_ID,GROUP_NAME,CARTON_QTY,PCS_QTY,DEPOT_ID,DEPOT_NAME,COUNTRY_NAME,ARRIVAL_DATE,STATUS FROM T_DEPOT_BULK_ITEM_BALANCE WHERE ITEM_ID='" + itemId + "'";
            }
            else if (itemId == "itemId")
            {
                query = @"SELECT OC_NUMBER,ITEM_ID,ITEM_NAME,ITEM_GROUP_ID,GROUP_NAME,CARTON_QTY,PCS_QTY,DEPOT_ID,DEPOT_NAME,COUNTRY_NAME,ARRIVAL_DATE,STATUS FROM T_DEPOT_BULK_ITEM_BALANCE WHERE OC_NUMBER='" + ocNum + "'";
            }
            else
            {
                query = @"SELECT OC_NUMBER,ITEM_ID,ITEM_NAME,ITEM_GROUP_ID,GROUP_NAME,CARTON_QTY,PCS_QTY,DEPOT_ID,DEPOT_NAME,COUNTRY_NAME,ARRIVAL_DATE,STATUS FROM T_DEPOT_BULK_ITEM_BALANCE WHERE ITEM_ID='" + itemId + "' AND OC_NUMBER='" + ocNum + "'";
            }
            
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0 && ds.Tables[0].Rows[0]["ITEM_ID"].ToString() != "")
            {
                for (int i = 0; i < c; i++)
                {
                    string OC_NUMBER = ds.Tables[0].Rows[i]["OC_NUMBER"].ToString();
                    string ITEM_ID = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string ITEM_NAME = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    string ITEM_GROUP_ID = ds.Tables[0].Rows[i]["ITEM_GROUP_ID"].ToString();
                    string GROUP_NAME = ds.Tables[0].Rows[i]["GROUP_NAME"].ToString();
                    string CARTON_QTY = ds.Tables[0].Rows[i]["CARTON_QTY"].ToString();
                    string PCS_QTY = ds.Tables[0].Rows[i]["PCS_QTY"].ToString();
                    string DEPOT_ID = ds.Tables[0].Rows[i]["DEPOT_ID"].ToString();
                    string DEPOT_NAME = ds.Tables[0].Rows[i]["DEPOT_NAME"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string ARRIVAL_DATE = ds.Tables[0].Rows[i]["ARRIVAL_DATE"].ToString();
                    ARRIVAL_DATE = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(ARRIVAL_DATE));
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString().Trim();

                    msg = msg + ";" + OC_NUMBER + ";" + ITEM_ID + ";" + ITEM_NAME + ";" + ITEM_GROUP_ID + ";" + GROUP_NAME + ";" + CARTON_QTY + ";" + PCS_QTY + ";" + DEPOT_ID + ";" + DEPOT_NAME + ";" + COUNTRY_NAME + ";" + ARRIVAL_DATE + ";" + STATUS;
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
    public static string GetItem(string items)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT ITEM_ID,ITEM_NAME FROM T_ITEM WHERE ACTIVENESS='Y'";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string itemId = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string itemName = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    msg = msg + ";" + itemId + ";" + itemName;
                }
            }
            else
            {
                msg = ";0000;No Item";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }

    [WebMethod]
    public static string GetItemByCompany(string companyId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT ITEM_ID,ITEM_NAME FROM T_ITEM WHERE OWN_COMPANY='" + companyId + "' AND ACTIVENESS='Y' ORDER BY ITEM_NAME";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string itemId = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string itemName = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    msg = msg + ";" + itemId + ";" + itemName;
                }
            }
            else
            {
                msg = ";0000;No Item";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
    [WebMethod]
    public static string GetItemPrice(string itemId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT ITEM_ID,TP FROM T_ITEM WHERE ITEM_ID='" + itemId + "' AND ACTIVENESS='Y'";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string tp = ds.Tables[0].Rows[i]["TP"].ToString();

                    msg = tp;
                }
            }
            else
            {
                msg = "No Price";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod]
    public static string GetCompany(string motherCompany)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT COMPANY_ID,COMPANY_FULL_NAME FROM T_COMPANY WHERE MOTHER_COMPANY='" + motherCompany + "'";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string comId = ds.Tables[0].Rows[i]["COMPANY_ID"].ToString();
                    string comName = ds.Tables[0].Rows[i]["COMPANY_FULL_NAME"].ToString();
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
    public static string GetCompanyByCountry(string countryName)
    {
        string msg = "";
        string ConStr = CommonDBSvc.GetConnectionString();

        try
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(ConStr);
            conn.Open();

            string queryCompany = @"SELECT COMPANY_ID,COMPANY_FULL_NAME FROM T_COMPANY";
            SqlDataAdapter daGlobalClass = new SqlDataAdapter(queryCompany, conn);

            daGlobalClass.SelectCommand.CommandTimeout = 0;
            daGlobalClass.Fill(ds, "CompanyTable");
            conn.Close();

            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string comId = ds.Tables[0].Rows[i]["COMPANY_ID"].ToString();
                    string comName = ds.Tables[0].Rows[i]["COMPANY_FULL_NAME"].ToString();
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
    public static string GetDMByCountry(string countryName)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT DM_ID,DM_NAME FROM T_DM WHERE COUNTRY_NAME LIKE '%" + countryName + "%' ORDER BY DM_NAME";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0 && ds.Tables[0].Rows[0]["DM_ID"].ToString() !="")
            {
                for (int i = 0; i < c; i++)
                {
                    string dmId = ds.Tables[0].Rows[i]["DM_ID"].ToString();
                    string dmName = ds.Tables[0].Rows[i]["DM_NAME"].ToString();
                    msg = msg + ";" + dmId + ";" + dmName;
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
    public static string GetSRByDM(string dmId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT SR_ID,SR_NAME FROM T_SR_INFO WHERE SR_ID IN(
                            (SELECT DISTINCT SR_ID FROM T_DM_SR WHERE DM_ID='" + dmId + "'))";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0 && ds.Tables[0].Rows[0]["SR_ID"].ToString() != "")
            {
                for (int i = 0; i < c; i++)
                {
                    string SR_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string SR_NAME = ds.Tables[0].Rows[i]["SR_NAME"].ToString();
                    msg = msg + ";" + SR_ID + ";" + SR_NAME;
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
    public static string GetItemGroup(string ownCompany)
    {
        string msg = "";

        if (string.IsNullOrEmpty(ownCompany))
        {
            ownCompany = HttpContext.Current.Session["Company"].ToString().Trim();
        }

        try
        {       
            string query = @"SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP
                            WHERE COMPANY_ID='" + ownCompany + "'";
            DataSet ds = new DataSet();
            ds = CommonDBSvc.GetDataSet(query);

            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
               return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                msg = "NotExist";
            }
        }
        catch (Exception ex) 
        {
            msg = ex.ToString();
        }

        return msg;
    }

    [WebMethod]
    public static string GetAllDivisionByCompany()
    {
        string msg = "";
        string ownCompany = "";

        if (string.IsNullOrEmpty(ownCompany))
        {
            ownCompany = HttpContext.Current.Session["Company"].ToString().Trim();
        }

        try
        {
            string query = @"SELECT T3.DIVISION_ID,T3.DIVISION_NAME FROM T_COMPANY_COUNTRY T1
                            INNER JOIN T_COUNTRY T2 ON T1.COUNTRY_ID=T2.COUNTRY_ID
                            INNER JOIN T_DIVISION T3 ON T2.COUNTRY_ID=T3.COUNTRY_ID
                            WHERE T1.COMPANY_ID='" + ownCompany + "'";
                            
            DataSet ds = new DataSet();
            ds = CommonDBSvc.GetDataSet(query);

            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                msg = "NotExist";
            }
        }
        catch (Exception ex)
        {
            msg = ex.ToString();
        }

        return msg;
    }

    [WebMethod]
    public static string GetAllDesignation(string designation)
    {
        string msg = "";

        try
        {
            string query = "";

            if (string.IsNullOrEmpty(designation))
            {
                query = @"SELECT DESIGNATION_ID,DESIGNATION_NAME FROM T_DESIGNATION ORDER BY DESIGNATION_NAME";
            }
            else
            {
                query = @"SELECT DESIGNATION_ID,DESIGNATION_NAME FROM T_DESIGNATION WHERE ID='" + designation + "'";
                                
            }

            DataSet ds = new DataSet();

            ds = CommonDBSvc.GetDataSet(query);

            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                msg = "NotExist";
            }
        }
        catch (Exception ex)
        {
            msg = ex.ToString();
        }

        return msg;
    }

    [WebMethod]
    public static string GetItemGroupByCountry(string country)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP
                            WHERE STATUS='Y' AND COMPANY_ID IN(
                            SELECT COMPANY_ID FROM T_COMPANY WHERE COUNTRY_NAME LIKE '%" + country + "%' AND STATUS='Y')";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string comId = ds.Tables[0].Rows[i]["ITEM_GROUP_ID"].ToString();
                    string comName = ds.Tables[0].Rows[i]["ITEM_GROUP_NAME"].ToString();
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
    public static string GetItemClass(string itemGroup)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT CLASS_ID,CLASS_NAME FROM T_ITEM_CLASS
                            WHERE ITEM_GROUP='" + itemGroup + "'";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string comId = ds.Tables[0].Rows[i]["CLASS_ID"].ToString();
                    string comName = ds.Tables[0].Rows[i]["CLASS_NAME"].ToString();
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
    public static string GetItemByClass(string itemClass)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT ITEM_ID,ITEM_NAME FROM T_ITEM
                            WHERE ITEM_CLASS='" + itemClass + "'";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string comId = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string comName = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
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
    public static string GetDivision(string countryID)
    {
        string msg = "";
        string ConStr = CommonDBSvc.GetConnectionString(); 

        try
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(ConStr);

            string query = @"SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION
                            WHERE COUNTRY_ID='" + countryID + "' ORDER BY DIVISION_NAME";
            SqlDataAdapter daGlobalClass = new SqlDataAdapter(query, conn);

            daGlobalClass.SelectCommand.CommandTimeout = 0;
            conn.Open();
            daGlobalClass.Fill(ds, "DivisionTable");

            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    msg = msg + ";" + DIVISION_ID + ";" + DIVISION_NAME;
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
    public static string GetCountry(string countryName)
    {
        string msg = "";
        string ConStr = CommonDBSvc.GetConnectionString(); 

        try
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(ConStr);

            string query = @"SELECT COUNTRY_ID,COUNTRY_NAME FROM T_COUNTRY ORDER BY COUNTRY_NAME";
            SqlDataAdapter daGlobalClass = new SqlDataAdapter(query, conn);

            daGlobalClass.SelectCommand.CommandTimeout = 0;

            conn.Open();
            daGlobalClass.Fill(ds, "CountryTable");

            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string COUNTRY_ID = ds.Tables[0].Rows[i]["COUNTRY_ID"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    msg = msg + ";" + COUNTRY_ID + ";" + COUNTRY_NAME;
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
    public static string GetRoute(string zoneId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT ROUTE_ID,ROUTE_NAME FROM T_ROUTE
                            WHERE ZONE_ID='" + zoneId + "'";
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
    public static string GetSRRoute(string country, string division, string zone)
    {
        string msg = "";
        string days = "";

        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            //string qr = "SELECT DAY_NAME,ROUTE_NAME FROM T_SR_ROUTE_DAY WHERE SR_ID='" + srId + "' AND STATUS='Y'";
            //OracleCommand cmdR = new OracleCommand(qr, conn);
            //OracleDataAdapter daR = new OracleDataAdapter(cmdR);  
            //DataSet dsR = new DataSet();
            //daR.Fill(dsR);
            //int r = dsR.Tables[0].Rows.Count;
            //if (r > 0)
            //{
            //    for (int t = 0; t < 6; t++)
            //    {
            //        string comId = dsR.Tables[0].Rows[t]["ROUTE_NAME"].ToString();
            //        string comName = dsR.Tables[0].Rows[t]["DAY_NAME"].ToString();
            //        days = days + ";" + comId + ";" + comName;
            //    }

            //}


            string query = @"SELECT ROUTE_ID,ROUTE_NAME FROM T_ROUTE
                            WHERE COUNTRY_NAME='" + country + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "' ORDER BY ROUTE_NAME";
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
    public static string GetSRAssingedRoute(string srId)
    {
        
        string days = "";

        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = "SELECT DAY_NAME,ROUTE_ID FROM T_SR_ROUTE_DAY WHERE SR_ID='" + srId + "' AND STATUS='Y'";
            OracleCommand cmdR = new OracleCommand(qr, conn);
            OracleDataAdapter daR = new OracleDataAdapter(cmdR);
            DataSet dsR = new DataSet();
            daR.Fill(dsR);
            int r = dsR.Tables[0].Rows.Count;
            if (r > 0)
            {
                for (int t = 0; t < 6; t++)
                {
                    string dayName = dsR.Tables[0].Rows[t]["DAY_NAME"].ToString();
                    string routeName = dsR.Tables[0].Rows[t]["ROUTE_ID"].ToString();
                    days = days + ";" + dayName + ";" + routeName;
                }

            }            

            conn.Close();
        }
        catch (Exception ex) { }

        return days;
    }


    [WebMethod]
    public static string GetZone(string division)
    {
        string msg = "";

        try
        {
            string query = @"SELECT * FROM T_ZONE WHERE DIVISION_ID='"+division+"'";
            DataSet ds = new DataSet();
            ds = CommonDBSvc.GetDataSet(query);

            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                msg = "NotExist";
            }
        }
        catch (Exception ex)
        {
            msg = ex.ToString();
        }

        return msg;
        
    }


    [WebMethod]
    public static string GetTradeProgram(string itemId, string zoneId, string divisionId, string country, string fromDate, string toDate, string programid)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = "";
            string div = "";// divisionId.Substring(2, 3).ToString();
            string zn = "";// zoneId.Substring(2, 3).ToString();

            if (divisionId.ToLower().Contains("ALL".ToLower()))
            {
                div = "ALL";
            }

            if (zoneId.ToLower().Contains("ALL".ToLower()))
            {
                zn = "ALL";
            }

            if (itemId == "itemId")
            {
                if (div == "ALL")
                {
                    query = @"SELECT * FROM T_TRADE_PROGRAM WHERE COUNTRY_NAME LIKE '%" + country + "%' ORDER BY FREE_FROM_DATE DESC";
                }
                else if (zn == "ALL")
                {
                    query = @"SELECT * FROM T_TRADE_PROGRAM WHERE COUNTRY_NAME LIKE '%" + country + "%' AND DIVISION_ID='" + divisionId + "' ORDER BY FREE_FROM_DATE DESC";
                }
                else
                {
                    query = @"SELECT * FROM T_TRADE_PROGRAM WHERE ZONE_ID='" + zoneId + "' AND DIVISION_ID='" + divisionId + "' AND COUNTRY_NAME LIKE '%" + country + "%' ORDER BY FREE_FROM_DATE DESC";
                }
            }
            else
            {
                if (div == "ALL")
                {
                    query = @"SELECT * FROM T_TRADE_PROGRAM WHERE PROGRAM_ID='" + programid + "' AND ITEM_ID='" + itemId + "' AND FREE_FROM_DATE>=TO_DATE('" + fromDate + "','DD/MM/YYYY') AND FREE_TO_DATE<=TO_DATE('" + toDate + "','DD/MM/YYYY') AND COUNTRY_NAME LIKE '%" + country + "%' ORDER BY FREE_FROM_DATE DESC";
                }
                else if (zn == "ALL")
                {
                    query = @"SELECT * FROM T_TRADE_PROGRAM WHERE PROGRAM_ID='" + programid + "' AND ITEM_ID='" + itemId + "' AND FREE_FROM_DATE>=TO_DATE('" + fromDate + "','DD/MM/YYYY') AND FREE_TO_DATE<=TO_DATE('" + toDate + "','DD/MM/YYYY') AND COUNTRY_NAME LIKE '%" + country + "%' AND DIVISION_ID='" + divisionId + "' ORDER BY FREE_FROM_DATE DESC";
                }
                else
                {
                    query = @"SELECT * FROM T_TRADE_PROGRAM WHERE PROGRAM_ID='" + programid + "' AND ITEM_ID='" + itemId + "' AND FREE_FROM_DATE>=TO_DATE('" + fromDate + "','DD/MM/YYYY') AND FREE_TO_DATE<=TO_DATE('" + toDate + "','DD/MM/YYYY') AND ZONE_ID='" + zoneId + "' AND DIVISION_ID='" + divisionId + "' AND COUNTRY_NAME LIKE '%" + country + "%' ORDER BY FREE_FROM_DATE DESC";
                }
            }

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string PROGRAM_ID = ds.Tables[0].Rows[i]["PROGRAM_ID"].ToString();
                    string PROGRAM_NAME = ds.Tables[0].Rows[i]["PROGRAM_NAME"].ToString();
                    string MOTHER_COMPANY = ds.Tables[0].Rows[i]["MOTHER_COMPANY"].ToString();
                    string OWN_COMPANY = ds.Tables[0].Rows[i]["OWN_COMPANY"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString().Trim();
                    string ITEM_GROUP_ID = ds.Tables[0].Rows[i]["ITEM_GROUP_ID"].ToString();
                    string GROUP_NAME = ds.Tables[0].Rows[i]["GROUP_NAME"].ToString();
                    string CLASS_ID = ds.Tables[0].Rows[i]["CLASS_ID"].ToString();
                    string CLASS_NAME = ds.Tables[0].Rows[i]["CLASS_NAME"].ToString();
                    string ITEM_ID = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string ITEM_NAME = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    string FREE_ITEM_ID = ds.Tables[0].Rows[i]["FREE_ITEM_ID"].ToString();
                    string FREE_ITEM_NAME = ds.Tables[0].Rows[i]["FREE_ITEM_NAME"].ToString();
                    
                    string MIN_QTY = ds.Tables[0].Rows[i]["MIN_QTY"].ToString();
                    string MAX_QTY = ds.Tables[0].Rows[i]["MAX_QTY"].ToString();
                    string FREE_CTN = ds.Tables[0].Rows[i]["FREE_CTN"].ToString();
                    string FREE_PCS = ds.Tables[0].Rows[i]["FREE_PCS"].ToString();
                    
                    string DISCOUNT = ds.Tables[0].Rows[i]["DISCOUNT"].ToString();
                    string DISCOUNT_TYPE = ds.Tables[0].Rows[i]["DISCOUNT_TYPE"].ToString();
                    string P_FROM_DATE = ds.Tables[0].Rows[i]["P_FROM_DATE"].ToString();
                    P_FROM_DATE = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(P_FROM_DATE));
                    string P_TO_DATE = ds.Tables[0].Rows[i]["P_TO_DATE"].ToString();
                    P_TO_DATE = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(P_TO_DATE));
                    string FREE_FROM_DATE = ds.Tables[0].Rows[i]["FREE_FROM_DATE"].ToString();
                    FREE_FROM_DATE = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(FREE_FROM_DATE));
                    string FREE_TO_DATE = ds.Tables[0].Rows[i]["FREE_TO_DATE"].ToString();
                    FREE_TO_DATE = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(FREE_TO_DATE));
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string ZONE_ID = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                    string OUTLET_GRADE = ds.Tables[0].Rows[i]["OUTLET_GRADE"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString().Trim();
                    string FREETYPE = ds.Tables[0].Rows[i]["FREE_TYPE"].ToString().Trim();
                    string MIN_QTY_DISCOUNT = ds.Tables[0].Rows[i]["MIN_QTY_DISCOUNT"].ToString().Trim();


                    msg = msg + ";" + PROGRAM_NAME + ";" + MOTHER_COMPANY + ";" + OWN_COMPANY + ";" + COUNTRY_NAME + ";" + ITEM_GROUP_ID + ";" + GROUP_NAME + ";" + CLASS_ID + ";" + CLASS_NAME + ";" + ITEM_ID + ";" + ITEM_NAME + ";" + FREE_ITEM_ID + ";" + FREE_ITEM_NAME + ";" + MIN_QTY + ";" + MAX_QTY + ";" + FREE_CTN + ";" + DISCOUNT + ";" + DISCOUNT_TYPE + ";" + P_FROM_DATE + ";" + P_TO_DATE + ";" + FREE_FROM_DATE + ";" + FREE_TO_DATE + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + ZONE_ID + ";" + ZONE_NAME + ";" + OUTLET_GRADE + ";" + STATUS + ";" + FREE_PCS + ";" + FREETYPE + ";" + MIN_QTY_DISCOUNT + ";" + PROGRAM_ID;
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
    public static string GetCOOInfo(string coo)
    {
        string msg = "";


        try
        {

            string query = "";
            if (coo == "coo")
            {
                query = @"SELECT T1.*,T2.DESIGNATION_NAME FROM T_COO T1
                        INNER JOIN T_DESIGNATION T2 ON T1.DESIGNATION_ID=T2.DESIGNATION_ID";

            }
            else
            {
                query = @"SELECT T1.*,T2.DESIGNATION_NAME FROM T_COO T1
                        INNER JOIN T_DESIGNATION T2 ON T1.DESIGNATION_ID=T2.DESIGNATION_ID 
                        WHERE T1.COO_ID='" + coo + "'";
            }

            DataSet ds = new DataSet();
            ds = CommonDBSvc.GetDataSet(query);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                msg = "NotExist";
            }

        }
        catch (Exception ex)
        {
            msg = ex.ToString();
        }

        return msg;
    }
    
    [WebMethod]
    public static string GetHOSInfo(string hos)
    {
        string msg = "";
      

        try
        {
         
            string query ="";
            if (hos == "hos")
            {
                query = @"SELECT T1.*,T2.ITEM_GROUP_NAME,T3.DESIGNATION_NAME FROM T_HOS T1
                        INNER JOIN T_ITEM_GROUP T2 ON T1.ITEM_GROUP_ID=T2.ITEM_GROUP_ID
                        INNER JOIN T_DESIGNATION T3 ON T1.DESIGNATION_ID=T3.DESIGNATION_ID";
                
            }
            else
            {
                query = @"SELECT T1.*,T2.ITEM_GROUP_NAME,T3.DESIGNATION_NAME FROM T_HOS T1
                        INNER JOIN T_ITEM_GROUP T2 ON T1.ITEM_GROUP_ID=T2.ITEM_GROUP_ID
                        INNER JOIN T_DESIGNATION T3 ON T1.DESIGNATION_ID=T3.DESIGNATION_ID 
                        WHERE T1.HOS_ID='" + hos+"'";             
            }

            DataSet ds = new DataSet();
            ds = CommonDBSvc.GetDataSet(query);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                msg = "NotExist";
            }

        }
        catch (Exception ex) 
        {
            msg = ex.ToString();
        }

        return msg;
    }


    [WebMethod]
    public static string GetAGMRMInfo(string rm)
    {

        string msg = "";
        try
        {
            string query = "";
            if (rm == "rm")
            {
                query = @"SELECT T1.*,T2.ITEM_GROUP_NAME,T3.DESIGNATION_NAME FROM T_AGM_RM T1
                        INNER JOIN T_ITEM_GROUP T2 ON T1.ITEM_GROUP_ID=T2.ITEM_GROUP_ID
                        INNER JOIN T_DESIGNATION T3 ON T1.DESIGNATION_ID=T3.DESIGNATION_ID";

            }
            else
            {
                query = @"SELECT T1.*,T2.ITEM_GROUP_NAME,T3.DESIGNATION_NAME FROM T_AGM_RM T1
                        INNER JOIN T_ITEM_GROUP T2 ON T1.ITEM_GROUP_ID=T2.ITEM_GROUP_ID
                        INNER JOIN T_DESIGNATION T3 ON T1.DESIGNATION_ID=T3.DESIGNATION_ID 
                        WHERE T1.AGM_RM_ID='" + rm + "'";
            }

            DataSet ds = new DataSet();
            ds = CommonDBSvc.GetDataSet(query);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                msg = "NotExist";
            }

        }
        catch (Exception ex)
        {
            msg = ex.ToString();
        }

        return msg;
    }

    [WebMethod]
    public static string GetTSMZMInfo(string tsm)
    {
        string msg = "";
        try
        {

            string query = "";
            if (tsm == "tsm")
            {
                query = @"SELECT T1.*,T2.ITEM_GROUP_NAME,T3.DESIGNATION_NAME FROM T_TSM_ZM T1
                        INNER JOIN T_ITEM_GROUP T2 ON T1.ITEM_GROUP_ID=T2.ITEM_GROUP_ID
                        INNER JOIN T_DESIGNATION T3 ON T1.DESIGNATION_ID=T3.DESIGNATION_ID";

            }
            else
            {
                query = @"SELECT T1.*,T2.ITEM_GROUP_NAME,T3.DESIGNATION_NAME FROM T_TSM_ZM T1
                        INNER JOIN T_ITEM_GROUP T2 ON T1.ITEM_GROUP_ID=T2.ITEM_GROUP_ID
                        INNER JOIN T_DESIGNATION T3 ON T1.DESIGNATION_ID=T3.DESIGNATION_ID 
                        WHERE T1.TSM_ZM_ID='" + tsm + "'";
            }

            DataSet ds = new DataSet();
            ds = CommonDBSvc.GetDataSet(query);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                msg = "NotExist";
            }

        }
        catch (Exception ex)
        {
            msg = ex.ToString();
        }

        return msg;
    }


    [WebMethod]
    public static string GetSRInfo(string srId)
    {
    string msg = "";
        try
        {

            string query = "";
            if (srId == "sr")
            {
                query = @"SELECT T1.*,T2.ITEM_GROUP_NAME FROM T_SR T1
                        INNER JOIN T_ITEM_GROUP T2 ON T1.CURRENT_GROUP_ID=T2.ITEM_GROUP_ID ";
            }
            else
            {
                query = @"SELECT T1.*,T2.ITEM_GROUP_NAME FROM T_SR T1
                        INNER JOIN T_ITEM_GROUP T2 ON T1.CURRENT_GROUP_ID=T2.ITEM_GROUP_ID 
                        WHERE T1.SR_ID='" + srId + "'";
            }

            DataSet ds = new DataSet();
            ds = CommonDBSvc.GetDataSet(query);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                msg = "NotExist";
            }

        }
        catch (Exception ex)
        {
            msg = ex.ToString();
        }

        return msg;
}

    [WebMethod]
    public static string GetDistInfo(string dist)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (dist == "dist")
            {
                query = @"SELECT T9.*,T10.ITEM_GROUP_NAME FROM
                        (SELECT T7.*,T8.COMPANY_FULL_NAME,T8.MOTHER_COMPANY FROM
                        (SELECT T5.*,T6.ZONE_NAME FROM
                        (SELECT T3.*,T4.COMPANY_ID FROM
                        (SELECT T1.*,T2.DIVISION_NAME DIVISION FROM
                         (SELECT * FROM T_DISTRIBUTOR) T1,
                         (SELECT * FROM T_DIVISION) T2
                         WHERE T1.DIVISION_NAME=T2.DIVISION_ID) T3,
                        (SELECT * FROM T_ITEM_GROUP) T4
                        WHERE T3.ITEM_GROUP=T4.ITEM_GROUP_ID) T5,
                        (SELECT * FROM T_ZONE) T6
                        WHERE T5.DIST_ZONE=T6.ZONE_ID) T7,
                        (SELECT COMPANY_ID,COMPANY_FULL_NAME,MOTHER_COMPANY FROM T_COMPANY WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString().Trim() + "') T8 ";
                query = query + @"WHERE T7.COMPANY_ID=T8.COMPANY_ID) T9,
                        (SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP) T10
                        WHERE T9.ITEM_GROUP=T10.ITEM_GROUP_ID";
            }
            else
            {
                query = @"SELECT T9.*,T10.ITEM_GROUP_NAME FROM
                        (SELECT T7.*,T8.COMPANY_FULL_NAME,T8.MOTHER_COMPANY FROM
                        (SELECT T5.*,T6.ZONE_NAME FROM
                        (SELECT T3.*,T4.COMPANY_ID FROM
                        (SELECT T1.*,T2.DIVISION_NAME DIVISION FROM
                         (SELECT * FROM T_DISTRIBUTOR WHERE DIST_ID='" + dist + "') T1, ";
                query = query + @"(SELECT * FROM T_DIVISION) T2
                         WHERE T1.DIVISION_NAME=T2.DIVISION_ID) T3,
                        (SELECT * FROM T_ITEM_GROUP) T4
                        WHERE T3.ITEM_GROUP=T4.ITEM_GROUP_ID) T5,
                        (SELECT * FROM T_ZONE) T6
                        WHERE T5.DIST_ZONE=T6.ZONE_ID) T7,
                        (SELECT COMPANY_ID,COMPANY_FULL_NAME,MOTHER_COMPANY FROM T_COMPANY) T8
                        WHERE T7.COMPANY_ID=T8.COMPANY_ID) T9,
                        (SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP) T10
                        WHERE T9.ITEM_GROUP=T10.ITEM_GROUP_ID";
            }

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string DSIT_ID = ds.Tables[0].Rows[i]["DIST_ID"].ToString();
                    string DIST_NAME = ds.Tables[0].Rows[i]["DIST_NAME"].ToString();
                    string DIST_BUSINESS_NAME = ds.Tables[0].Rows[i]["DIST_BUSINESS_NAME"].ToString();
                    string MOBILE_NO = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string DIST_ADDRESS = ds.Tables[0].Rows[i]["DIST_ADDRESS"].ToString();
                    string EMAIL_ADDRESS = ds.Tables[0].Rows[i]["EMAIL_ADDRESS"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION"].ToString();
                    string ZONE_ID = ds.Tables[0].Rows[i]["DIST_ZONE"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();

                    string MOTHER_COMPANY = ds.Tables[0].Rows[i]["MOTHER_COMPANY"].ToString();

                    string OWN_COMPANY = ds.Tables[0].Rows[i]["COMPANY_FULL_NAME"].ToString();
                    string OWN_COMPANY_ID = ds.Tables[0].Rows[i]["COMPANY_ID"].ToString();
                    string ITEM_GROUP = ds.Tables[0].Rows[i]["ITEM_GROUP_NAME"].ToString();
                    string ITEM_GROUP_ID = ds.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + DSIT_ID + ";" + DIST_NAME + ";" + DIST_BUSINESS_NAME + ";" + MOBILE_NO + ";" + DIST_ADDRESS + ";" + EMAIL_ADDRESS + ";" + COUNTRY_NAME + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + ZONE_ID + ";" + ZONE_NAME + ";" + MOTHER_COMPANY + ";" + OWN_COMPANY + ";" + OWN_COMPANY_ID + ";" + ITEM_GROUP + ";" + ITEM_GROUP_ID + ";" + STATUS;
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
    public static string GetWarehouseInfo(string whCode)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (whCode == "whCode")
            {
                query = @"SELECT T5.*,T6.DIST_NAME FROM
                        (SELECT T3.*,T4.ZONE_NAME FROM
                        (SELECT T1.*,T2.DIVISION_NAME DIVISION FROM
                         (SELECT * FROM T_WARE_HOUSE WHERE COUNTRY LIKE '%" + HttpContext.Current.Session["country"].ToString().Trim() + "%') T1, ";
                query = query + @"(SELECT * FROM T_DIVISION) T2
                         WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,                         
                        (SELECT * FROM T_ZONE) T4
                        WHERE T3.ZONE_ID=T4.ZONE_ID) T5,
                        (SELECT * FROM T_DISTRIBUTOR) T6
                        WHERE T5.DIST_CODE=T6.DIST_ID";
            }
            else
            {
                query = @"SELECT T5.*,T6.DIST_NAME FROM
                        (SELECT T3.*,T4.ZONE_NAME FROM
                        (SELECT T1.*,T2.DIVISION_NAME DIVISION FROM
                        (SELECT * FROM T_WARE_HOUSE WHERE WH_CODE='" + whCode + "') T1, ";
                query = query + @"(SELECT * FROM T_DIVISION) T2
                        WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,                         
                        (SELECT * FROM T_ZONE) T4
                        WHERE T3.ZONE_ID=T4.ZONE_ID) T5,
                        (SELECT * FROM T_DISTRIBUTOR) T6
                        WHERE T5.DIST_CODE=T6.DIST_ID 
                        ";
            }

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string DSIT_ID = ds.Tables[0].Rows[i]["DIST_CODE"].ToString();
                    string DIST_NAME = ds.Tables[0].Rows[i]["DIST_NAME"].ToString();
                    string WH_CODE = ds.Tables[0].Rows[i]["WH_CODE"].ToString();
                    string WH_NAME = ds.Tables[0].Rows[i]["WH_NAME"].ToString();
                    string CONTACT_PERSON = ds.Tables[0].Rows[i]["CONTACT_PERSON"].ToString();
                    string MOBILE_NO = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string ADDRESS = ds.Tables[0].Rows[i]["ADDRESS"].ToString();
                    string EMAIL_ADDRESS = ds.Tables[0].Rows[i]["EMAIL_ADDRESS"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY"].ToString();
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION"].ToString();
                    string ZONE_ID = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();                    
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + DSIT_ID + ";" + DIST_NAME + ";" + WH_CODE + ";" + WH_NAME + ";" + CONTACT_PERSON + ";" + MOBILE_NO + ";" + ADDRESS + ";" + EMAIL_ADDRESS + ";" + COUNTRY_NAME + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + ZONE_ID + ";" + ZONE_NAME + ";" + STATUS;
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
    public static string GetDMInfo(string dm)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (dm == "dm")
            {
                query = @"SELECT * FROM T_DM WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString().Trim() + "%'";
            }
            else
            {
                query = @"SELECT * FROM T_DM WHERE DM_ID='" + dm + "' ";
            }

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string DM_ID = ds.Tables[0].Rows[i]["DM_ID"].ToString();
                    string DM_NAME = ds.Tables[0].Rows[i]["DM_NAME"].ToString();
                    string DM_PWD = ds.Tables[0].Rows[i]["DM_PWD"].ToString();
                    string MOBILE_NO = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string EMAIL_ADDRESS = ds.Tables[0].Rows[i]["EMAIL_ADDRESS"].ToString();
                    string DIST_ID = ds.Tables[0].Rows[i]["DIST_ID"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string ZONE_ID = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();

                    msg = msg + ";" + DM_ID + ";" + DM_NAME + ";" + DM_PWD + ";" + MOBILE_NO + ";" + EMAIL_ADDRESS + ";" + DIST_ID + ";" + STATUS + ";" + COUNTRY_NAME + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + ZONE_ID + ";" + ZONE_NAME;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod]
    public static string GetDMSRInfo(string srId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (srId == "sr")
            {
                query = @"SELECT T3.*,T4.SR_NAME FROM
                        (SELECT T2.DM_ID,T1.DM_NAME,T2.SR_ID FROM
                        (SELECT DM_ID,DM_NAME FROM T_DM WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString().Trim() + "%') T1, ";
                query = query + @"(SELECT DM_ID,SR_ID FROM T_DM_SR) T2
                        WHERE T1.DM_ID=T2.DM_ID) T3,
                        (SELECT SR_ID,SR_NAME FROM T_SR_INFO) T4
                        WHERE T3.SR_ID=T4.SR_ID ORDER BY T3.DM_ID";
            }
            else
            {             
                query = @"SELECT T3.*,T4.SR_NAME FROM
                        (SELECT T2.DM_ID,T1.DM_NAME,T2.SR_ID FROM
                        (SELECT DM_ID,DM_NAME FROM T_DM) T1,
                        (SELECT DM_ID,SR_ID FROM T_DM_SR) T2
                        WHERE T1.DM_ID=T2.DM_ID) T3,
                        (SELECT SR_ID,SR_NAME FROM T_SR_INFO WHERE SR_ID='" + srId + "') T4 WHERE T3.SR_ID=T4.SR_ID ORDER BY T3.DM_ID";
            }

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string DM_ID = ds.Tables[0].Rows[i]["DM_ID"].ToString();
                    string DM_NAME = ds.Tables[0].Rows[i]["DM_NAME"].ToString();
                    string SR_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string SR_NAME = ds.Tables[0].Rows[i]["SR_NAME"].ToString();


                    msg = msg + ";" + DM_ID + ";" + DM_NAME + ";" + SR_ID + ";" + SR_NAME;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod]
    public static string GetTSMSRInfo(string srId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (srId == "sr")
            {
                query = @"SELECT T3.*,T4.SR_NAME,T4.SR_NUMBER FROM
                        (SELECT DISTINCT T1.*,T2.SR_ID FROM
                        (SELECT TSM_ID,TSM_NAME,MOBILE_NO TSM_NUMBER FROM T_TSM_ZM WHERE STATUS='Y' AND COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString().Trim() + "%') T1, ";
                query = query + @"(SELECT TSM_ID,SR_ID FROM T_TSM_SR) T2
                        WHERE T1.TSM_ID=T2.TSM_ID) T3,
                        (SELECT SR_ID,SR_NAME,MOBILE_NO SR_NUMBER FROM T_SR_INFO WHERE STATUS='Y' AND ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString().Trim() + "') T4 WHERE T3.SR_ID=T4.SR_ID";
            }
            else
            {
                query = @"SELECT T3.*,T4.SR_NAME,T4.SR_NUMBER FROM
                        (SELECT DISTINCT T1.*,T2.SR_ID FROM
                        (SELECT TSM_ID,TSM_NAME,MOBILE_NO TSM_NUMBER FROM T_TSM_ZM WHERE STATUS='Y' AND COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString().Trim() + "%') T1, ";
                query = query + @"(SELECT TSM_ID,SR_ID FROM T_TSM_SR) T2
                        WHERE T1.TSM_ID=T2.TSM_ID) T3,
                        (SELECT SR_ID,SR_NAME,MOBILE_NO SR_NUMBER FROM T_SR_INFO WHERE STATUS='Y' AND SR_ID='" + srId + "' AND ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString().Trim() + "') T4 WHERE T3.SR_ID=T4.SR_ID";
            }

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string TSM_ID = ds.Tables[0].Rows[i]["TSM_ID"].ToString();
                    string TSM_NAME = ds.Tables[0].Rows[i]["TSM_NAME"].ToString();
                    string TSM_NUMBER = ds.Tables[0].Rows[i]["TSM_NUMBER"].ToString();
                    string SR_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string SR_NAME = ds.Tables[0].Rows[i]["SR_NAME"].ToString();
                    string SR_NUMBER = ds.Tables[0].Rows[i]["SR_NUMBER"].ToString();

                    msg = msg + ";" + TSM_ID + ";" + TSM_NAME + ";" + TSM_NUMBER + ";" + SR_ID + ";" + SR_NAME + ";" + SR_NUMBER;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
        
    
    [WebMethod]
    public static string DeleteDMSRInfo(string srId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"DELETE FROM T_DM_SR WHERE SR_ID='" + srId + "'";
           
            OracleCommand cmd = new OracleCommand(query, conn);
            int c = cmd.ExecuteNonQuery();
            if (c > 0)
            {
                msg = "Successful!";
            }
            else
            {
                msg = "Not Successful";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }

    
    [WebMethod]
    public static string GetDivisionInfo(string division)
    {
        string msg = "";


        try
        {
            DataSet ds = new DataSet();

            string query = "";
            if (division == "division")
            {
                //query = @"SELECT * FROM T_DIVISION WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString().Trim() + "%' ORDER BY DIVISION_NAME";
                query = @"SELECT T1.DIVISION_ID,T1.DIVISION_NAME,T2.COUNTRY_ID,T2.COUNTRY_NAME,T1.STATUS FROM T_DIVISION T1 INNER JOIN T_COUNTRY T2 ON T1.COUNTRY_ID=T2.COUNTRY_ID 
                ORDER BY DIVISION_NAME";

            }
            else
            {
                query = @"SELECT T1.DIVISION_ID,T1.DIVISION_NAME,T2.COUNTRY_ID,T2.COUNTRY_NAME,T1.STATUS FROM T_DIVISION T1 
                          INNER JOIN T_COUNTRY T2 ON T1.COUNTRY_ID=T2.COUNTRY_ID 
                          WHERE DIVISION_ID='" + division + "' ORDER BY DIVISION_NAME";
            }

            ds = CommonDBSvc.GetDataSet(query);

            int c = ds.Tables[0].Rows.Count;

            if (c > 0)
            {
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                msg = "Not Exist";
            }

        }

        catch (Exception ex) 
        {
            msg = ex.ToString();
        }

        return msg;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetZoneInfo(string zone)
    {
        string msg = "";
        //string currentUser = HttpContext.Current.Session["country"].ToString().Trim();

        try
        {
            string query = "";
            if (zone == "zone")
            {
                query = @"SELECT T1.ZONE_ID,T1.ZONE_NAME,T2.DIVISION_ID,T2.DIVISION_NAME,T3.COUNTRY_NAME,T1.STATUS FROM T_ZONE T1 
                        INNER JOIN T_DIVISION T2 ON T1.DIVISION_ID=T2.DIVISION_ID
                        INNER JOIN T_COUNTRY T3 ON T2.COUNTRY_ID=T3.COUNTRY_ID";
             
            }
            else
            {
                query = @"SELECT T1.ZONE_ID,T1.ZONE_NAME,T2.DIVISION_ID,T2.DIVISION_NAME,T3.COUNTRY_NAME,T3.COUNTRY_ID,T1.STATUS FROM T_ZONE T1 
                INNER JOIN T_DIVISION T2 ON T1.DIVISION_ID=T2.DIVISION_ID
                INNER JOIN T_COUNTRY T3 ON T2.COUNTRY_ID=T3.COUNTRY_ID
                Where T1.ZONE_ID='" +zone+"'";
            }

            DataSet ds = new DataSet();
            ds = CommonDBSvc.GetDataSet(query);

            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                return JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                msg = "Not Exist";
            }

        }
        catch (Exception ex) { }

        return msg;
    }

    [WebMethod]
    public static string GetRouteInfo(string route)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (route == "route")
            {
                query = @"SELECT T3.*,T4.ZONE_NAME FROM
                        (SELECT T1.*,T2.DIVISION_NAME FROM
                        (SELECT ROUTE_ID,ROUTE_NAME,ZONE_ID,DIVISION_ID,COUNTRY_NAME,STATUS FROM T_ROUTE) T1,
                        (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString().Trim() + "%') T2 WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3, ";
                query = query + @"(SELECT * FROM T_ZONE) T4
                        WHERE T3.ZONE_ID=T4.ZONE_ID";
            }
            else
            {
                query = @"SELECT T3.*,T4.ZONE_NAME FROM
                        (SELECT T1.*,T2.DIVISION_NAME FROM
                        (SELECT ROUTE_ID,ROUTE_NAME,ZONE_ID,DIVISION_ID,COUNTRY_NAME,STATUS FROM T_ROUTE WHERE ROUTE_ID='" + route + "') T1, ";
                query = query + @" (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T2 WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                        (SELECT * FROM T_ZONE) T4
                        WHERE T3.ZONE_ID=T4.ZONE_ID";
            }

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string ROUTE_ID = ds.Tables[0].Rows[i]["ROUTE_ID"].ToString();
                    string ROUTE_NAME = ds.Tables[0].Rows[i]["ROUTE_NAME"].ToString();
                    string ZONE_ID = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + ZONE_ID + ";" + ZONE_NAME + ";" + COUNTRY_NAME + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + STATUS + ";" + ROUTE_ID + ";" + ROUTE_NAME;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }

    
    [WebMethod]
    public static string GetOutletInfo(string outlet)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (outlet == "outlet")
            {
                query = @"SELECT T5.*,T6.ROUTE_NAME FROM
                        (SELECT T3.*,T4.ZONE_NAME FROM
                        (SELECT T1.*,T2.DIVISION_NAME FROM
                        (SELECT * FROM T_OUTLET) T1,
                        (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T2 WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                        (SELECT * FROM T_ZONE) T4
                        WHERE T3.ZONE_ID=T4.ZONE_ID) T5,
                        (SELECT ROUTE_ID,ROUTE_NAME FROM T_ROUTE) T6
                        WHERE T5.ROUTE_ID=T6.ROUTE_ID";
            }
            else
            {
                query = @"SELECT T5.*,T6.ROUTE_NAME FROM
                        (SELECT T3.*,T4.ZONE_NAME FROM
                        (SELECT T1.*,T2.DIVISION_NAME FROM
                        (SELECT * FROM T_OUTLET WHERE OUTLET_ID='" + outlet + "') T1, ";
                query = query + @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T2 WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                        (SELECT * FROM T_ZONE) T4
                        WHERE T3.ZONE_ID=T4.ZONE_ID) T5,
                        (SELECT ROUTE_ID,ROUTE_NAME FROM T_ROUTE) T6
                        WHERE T5.ROUTE_ID=T6.ROUTE_ID";
            }

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string OUTLET_ID = ds.Tables[0].Rows[i]["OUTLET_ID"].ToString();
                    string OUTLET_NAME = ds.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                    string OUTLET_ADDRESS = ds.Tables[0].Rows[i]["OUTLET_ADDRESS"].ToString();
                    string PROPRITOR_NAME = ds.Tables[0].Rows[i]["PROPRITOR_NAME"].ToString();
                    string MOBILE_NUMBER = ds.Tables[0].Rows[i]["MOBILE_NUMBER"].ToString();
                    string EMAIL_ADDRESS = ds.Tables[0].Rows[i]["EMAIL_ADDRESS"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string ZONE_ID = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                    string ROUTE_ID = ds.Tables[0].Rows[i]["ROUTE_ID"].ToString();
                    string ROUTE_NAME = ds.Tables[0].Rows[i]["ROUTE_NAME"].ToString();
                    string FRIDGE = ds.Tables[0].Rows[i]["FRIDGE"].ToString();                    
                    string SIGNBOARD = ds.Tables[0].Rows[i]["SIGNBOARD"].ToString();
                    string RACK = ds.Tables[0].Rows[i]["RACK"].ToString();
                    string CATEGORY_NAME = ds.Tables[0].Rows[i]["CATEGORY_NAME"].ToString();
                    string GRADE = ds.Tables[0].Rows[i]["GRADE"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();
                    string PRODUCTS = ds.Tables[0].Rows[i]["PRODUCTS"].ToString();
                    string SR_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString();

                    msg = msg + ";" + OUTLET_ID + ";" + OUTLET_NAME + ";" + OUTLET_ADDRESS + ";" + PROPRITOR_NAME + ";" + MOBILE_NUMBER + ";" + EMAIL_ADDRESS + ";" + COUNTRY_NAME + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + ZONE_ID + ";" + ZONE_NAME + ";" + ROUTE_ID + ";" + ROUTE_NAME + ";" + FRIDGE + ";" + SIGNBOARD + ";" + RACK + ";" + CATEGORY_NAME + ";" + GRADE + ";" + STATUS + ";" + PRODUCTS + ";" + SR_ID;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod]
    public static string GetRouteWiseOutlet(string routeId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
             
            string query = @"SELECT T5.*,T6.ROUTE_NAME FROM
                        (SELECT T3.*,T4.ZONE_NAME FROM
                        (SELECT T1.*,T2.DIVISION_NAME FROM
                        (SELECT * FROM T_OUTLET WHERE ROUTE_ID='" + routeId + "') T1, ";
                query = query + @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T2 WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                        (SELECT * FROM T_ZONE) T4
                        WHERE T3.ZONE_ID=T4.ZONE_ID) T5,
                        (SELECT ROUTE_ID,ROUTE_NAME FROM T_ROUTE) T6
                        WHERE T5.ROUTE_ID=T6.ROUTE_ID ORDER BY T5.OUTLET_ID";
            

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string OUTLET_ID = ds.Tables[0].Rows[i]["OUTLET_ID"].ToString();
                    string OUTLET_NAME = ds.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                    string OUTLET_ADDRESS = ds.Tables[0].Rows[i]["OUTLET_ADDRESS"].ToString();
                    string PROPRITOR_NAME = ds.Tables[0].Rows[i]["PROPRITOR_NAME"].ToString();
                    string MOBILE_NUMBER = ds.Tables[0].Rows[i]["MOBILE_NUMBER"].ToString();
                    string EMAIL_ADDRESS = ds.Tables[0].Rows[i]["EMAIL_ADDRESS"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string ZONE_ID = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                    string ROUTE_ID = ds.Tables[0].Rows[i]["ROUTE_ID"].ToString();
                    string ROUTE_NAME = ds.Tables[0].Rows[i]["ROUTE_NAME"].ToString();
                    string FRIDGE = ds.Tables[0].Rows[i]["FRIDGE"].ToString();
                    string SIGNBOARD = ds.Tables[0].Rows[i]["SIGNBOARD"].ToString();
                    string RACK = ds.Tables[0].Rows[i]["RACK"].ToString();
                    string CATEGORY_NAME = ds.Tables[0].Rows[i]["CATEGORY_NAME"].ToString();
                    string GRADE = ds.Tables[0].Rows[i]["GRADE"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();
                    string PRODUCTS = ds.Tables[0].Rows[i]["PRODUCTS"].ToString();
                    string SR_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString();

                    msg = msg + ";" + OUTLET_ID + ";" + OUTLET_NAME + ";" + OUTLET_ADDRESS + ";" + PROPRITOR_NAME + ";" + MOBILE_NUMBER + ";" + EMAIL_ADDRESS + ";" + COUNTRY_NAME + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + ZONE_ID + ";" + ZONE_NAME + ";" + ROUTE_ID + ";" + ROUTE_NAME + ";" + FRIDGE + ";" + SIGNBOARD + ";" + RACK + ";" + CATEGORY_NAME + ";" + GRADE + ";" + STATUS + ";" + PRODUCTS + ";" + SR_ID;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod]
    public static string GetRouteWiseForeignOutlet(string routeId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT T5.*,T6.ROUTE_NAME FROM
                        (SELECT T3.*,T4.ZONE_NAME FROM
                        (SELECT T1.*,T2.DIVISION_NAME FROM
                        (SELECT * FROM T_OUTLET WHERE ROUTE_ID='" + routeId + "') T1, ";
            query = query + @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T2 WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                        (SELECT * FROM T_ZONE) T4
                        WHERE T3.ZONE_ID=T4.ZONE_ID) T5,
                        (SELECT ROUTE_ID,ROUTE_NAME FROM T_ROUTE) T6
                        WHERE T5.ROUTE_ID=T6.ROUTE_ID ORDER BY T5.OUTLET_ID";


            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string OUTLET_ID = ds.Tables[0].Rows[i]["OUTLET_ID"].ToString();
                    string OUTLET_NAME = ds.Tables[0].Rows[i]["OUTLET_NAME"].ToString();


                    msg = msg + ";" + OUTLET_ID + ";" + OUTLET_NAME;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod]
    public static string SearchOutletInfo(string country, string division, string zone, string route)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";

            query = @"SELECT T5.*,T6.ROUTE_NAME FROM
                        (SELECT T3.*,T4.ZONE_NAME FROM
                        (SELECT T1.*,T2.DIVISION_NAME FROM
                        (SELECT * FROM T_OUTLET WHERE COUNTRY_NAME = '" + country + "' AND DIVISION_ID = '" + division + "' AND ZONE_ID = '" + zone + "' AND ROUTE_ID = '" + route + "') T1, ";
                query = query + @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T2 WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                        (SELECT * FROM T_ZONE) T4
                        WHERE T3.ZONE_ID=T4.ZONE_ID) T5,
                        (SELECT ROUTE_ID,ROUTE_NAME FROM T_ROUTE) T6
                        WHERE T5.ROUTE_ID=T6.ROUTE_ID";
           

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string OUTLET_ID = ds.Tables[0].Rows[i]["OUTLET_ID"].ToString();
                    string OUTLET_NAME = ds.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                    string OUTLET_ADDRESS = ds.Tables[0].Rows[i]["OUTLET_ADDRESS"].ToString();
                    string PROPRITOR_NAME = ds.Tables[0].Rows[i]["PROPRITOR_NAME"].ToString();
                    string MOBILE_NUMBER = ds.Tables[0].Rows[i]["MOBILE_NUMBER"].ToString();
                    string EMAIL_ADDRESS = ds.Tables[0].Rows[i]["EMAIL_ADDRESS"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string ZONE_ID = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                    string ROUTE_ID = ds.Tables[0].Rows[i]["ROUTE_ID"].ToString();
                    string ROUTE_NAME = ds.Tables[0].Rows[i]["ROUTE_NAME"].ToString();
                    string FRIDGE = ds.Tables[0].Rows[i]["FRIDGE"].ToString();
                    string SIGNBOARD = ds.Tables[0].Rows[i]["SIGNBOARD"].ToString();
                    string RACK = ds.Tables[0].Rows[i]["RACK"].ToString();
                    string CATEGORY_NAME = ds.Tables[0].Rows[i]["CATEGORY_NAME"].ToString();
                    string GRADE = ds.Tables[0].Rows[i]["GRADE"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + OUTLET_ID + ";" + OUTLET_NAME + ";" + OUTLET_ADDRESS + ";" + PROPRITOR_NAME + ";" + MOBILE_NUMBER + ";" + EMAIL_ADDRESS + ";" + COUNTRY_NAME + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + ZONE_ID + ";" + ZONE_NAME + ";" + ROUTE_ID + ";" + ROUTE_NAME + ";" + FRIDGE + ";" + SIGNBOARD + ";" + RACK + ";" + CATEGORY_NAME + ";" + GRADE + ";" + STATUS;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod]
    public static string GetCompanyInfo(string comId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (comId == "comId")
            {
                query = @"SELECT * FROM T_COMPANY";
            }
            else
            {
                query = @"SELECT * FROM T_COMPANY WHERE COMPANY_ID='" + comId + "'";
            }

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string COMPANY_ID = ds.Tables[0].Rows[i]["COMPANY_ID"].ToString();
                    string COMPANY_FULL_NAME = ds.Tables[0].Rows[i]["COMPANY_FULL_NAME"].ToString();
                    string COMPANY_NICK_NAME = ds.Tables[0].Rows[i]["COMPANY_NICK_NAME"].ToString();
                    string MOTHER_COMPANY = ds.Tables[0].Rows[i]["MOTHER_COMPANY"].ToString();                    
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();

                    msg = msg + ";" + COMPANY_ID + ";" + COMPANY_FULL_NAME + ";" + COMPANY_NICK_NAME + ";" + MOTHER_COMPANY + ";" + STATUS + ";" + COUNTRY_NAME;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
    
    [WebMethod]
    public static string GetDepot(string country)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();


            string query = @"SELECT DIST_ID,DIST_BUSINESS_NAME FROM T_DISTRIBUTOR WHERE STATUS='Y' AND COUNTRY_NAME LIKE '%" + country + "%'";
           
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string DIST_ID = ds.Tables[0].Rows[i]["DIST_ID"].ToString();
                    string DIST_BUSINESS_NAME = ds.Tables[0].Rows[i]["DIST_BUSINESS_NAME"].ToString();

                    msg = msg + ";" + DIST_ID + ";" + DIST_BUSINESS_NAME;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
    
    [WebMethod]
    public static string GetGroupInfo(string groupId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (groupId == "groupId")
            {
                query = @"SELECT T1.*,T2.COMPANY_FULL_NAME FROM
                        (SELECT * FROM T_ITEM_GROUP) T1,
                        (SELECT COMPANY_ID,COMPANY_FULL_NAME FROM T_COMPANY WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString().Trim() + "') T2 WHERE T1.COMPANY_ID=T2.COMPANY_ID";
            }
            else
            {
                query = @"SELECT T1.*,T2.COMPANY_FULL_NAME FROM
                        (SELECT * FROM T_ITEM_GROUP  WHERE ITEM_GROUP_ID='" + groupId + "') T1,(SELECT COMPANY_ID,COMPANY_FULL_NAME FROM T_COMPANY) T2 WHERE T1.COMPANY_ID=T2.COMPANY_ID";
            }

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string ITEM_GROUP_ID = ds.Tables[0].Rows[i]["ITEM_GROUP_ID"].ToString();
                    string ITEM_GROUP_NAME = ds.Tables[0].Rows[i]["ITEM_GROUP_NAME"].ToString();
                    string COMPANY_ID = ds.Tables[0].Rows[i]["COMPANY_ID"].ToString();
                    string COMPANY_NAME = ds.Tables[0].Rows[i]["COMPANY_FULL_NAME"].ToString();
                    string MOTHER_COMPANY = ds.Tables[0].Rows[i]["MOTHER_COMPANY"].ToString();                    
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + ITEM_GROUP_ID + ";" + ITEM_GROUP_NAME + ";" + COMPANY_ID + ";" + COMPANY_NAME + ";" + MOTHER_COMPANY + ";" + STATUS;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod]
    public static string GetClassInfo(string classId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (classId == "classId")
            {
                query = @"SELECT T3.*,T4.ITEM_GROUP_NAME FROM
                        (SELECT T1.*,T2.COMPANY_FULL_NAME FROM
                        (SELECT * FROM T_ITEM_CLASS) T1,
                        (SELECT COMPANY_ID,COMPANY_FULL_NAME FROM T_COMPANY WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString().Trim() + "') T2 ";
                query = query + @"WHERE T1.COMPANY_ID=T2.COMPANY_ID) T3,
                        (SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP) T4
                        WHERE T3.ITEM_GROUP=T4.ITEM_GROUP_ID";
            }
            else
            {
                query = @"SELECT T3.*,T4.ITEM_GROUP_NAME FROM
                        (SELECT T1.*,T2.COMPANY_FULL_NAME FROM
                        (SELECT * FROM T_ITEM_CLASS WHERE CLASS_ID='" + classId + "') T1, ";
                query = query + @" (SELECT COMPANY_ID,COMPANY_FULL_NAME FROM T_COMPANY) T2
                        WHERE T1.COMPANY_ID=T2.COMPANY_ID) T3,
                        (SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP) T4
                        WHERE T3.ITEM_GROUP=T4.ITEM_GROUP_ID";

            }

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string CLASS_ID = ds.Tables[0].Rows[i]["CLASS_ID"].ToString();
                    string CLASS_NAME = ds.Tables[0].Rows[i]["CLASS_NAME"].ToString();
                    string ITEM_GROUP_ID = ds.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                    string ITEM_GROUP_NAME = ds.Tables[0].Rows[i]["ITEM_GROUP_NAME"].ToString();
                    string COMPANY_ID = ds.Tables[0].Rows[i]["COMPANY_ID"].ToString();
                    string COMPANY_NAME = ds.Tables[0].Rows[i]["COMPANY_FULL_NAME"].ToString();
                    string MOTHER_COMPANY = ds.Tables[0].Rows[i]["MOTHER_COMPANY"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + CLASS_ID + ";" + CLASS_NAME + ";" + ITEM_GROUP_ID + ";" + ITEM_GROUP_NAME + ";" + COMPANY_ID + ";" + COMPANY_NAME + ";" + MOTHER_COMPANY + ";" + STATUS;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
    
    [WebMethod]
    public static string GetItemInfo(string itemId, string ownCompany)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (itemId == "itemId")
            {
                query = @"SELECT T5.*,T6.CLASS_NAME FROM
                         (SELECT T3.*,T4.ITEM_GROUP_NAME FROM
                         (SELECT T1.*,T2.COMPANY_FULL_NAME FROM
                         (SELECT * FROM T_ITEM WHERE OWN_COMPANY='" + ownCompany + "') T1, ";
                query = query + @" (SELECT COMPANY_ID,COMPANY_FULL_NAME FROM T_COMPANY) T2
                         WHERE T1.OWN_COMPANY=T2.COMPANY_ID) T3,
                         (SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP) T4
                         WHERE T3.ITEM_GROUP=T4.ITEM_GROUP_ID) T5,
                         (SELECT CLASS_ID,CLASS_NAME FROM T_ITEM_CLASS) T6 
                         WHERE T5.ITEM_CLASS=T6.CLASS_ID";
            }
            else
            {
                query = @"SELECT T5.*,T6.CLASS_NAME FROM
                         (SELECT T3.*,T4.ITEM_GROUP_NAME FROM
                         (SELECT T1.*,T2.COMPANY_FULL_NAME FROM
                         (SELECT * FROM T_ITEM WHERE ITEM_ID = '" + itemId + "') T1, ";
                query = query + @" (SELECT COMPANY_ID,COMPANY_FULL_NAME FROM T_COMPANY) T2
                         WHERE T1.OWN_COMPANY=T2.COMPANY_ID) T3,
                         (SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP) T4
                         WHERE T3.ITEM_GROUP=T4.ITEM_GROUP_ID) T5,
                         (SELECT CLASS_ID,CLASS_NAME FROM T_ITEM_CLASS) T6 
                         WHERE T5.ITEM_CLASS=T6.CLASS_ID";

            }

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string ITEM_ID = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string ITEM_NAME = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    string ITEM_SHORT_NAME = ds.Tables[0].Rows[i]["ITEM_SHORT_NAME"].ToString();
                    string ITEM_BL_NAME = ds.Tables[0].Rows[i]["ITEM_BL_NAME"].ToString();
                    string CLASS_ID = ds.Tables[0].Rows[i]["ITEM_CLASS"].ToString();
                    string CLASS_NAME = ds.Tables[0].Rows[i]["CLASS_NAME"].ToString();
                    string ITEM_GROUP_ID = ds.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                    string ITEM_GROUP_NAME = ds.Tables[0].Rows[i]["ITEM_GROUP_NAME"].ToString();
                    string COMPANY_ID = ds.Tables[0].Rows[i]["OWN_COMPANY"].ToString();
                    string COMPANY_NAME = ds.Tables[0].Rows[i]["COMPANY_FULL_NAME"].ToString();
                    string MOTHER_COMPANY = ds.Tables[0].Rows[i]["MOTHER_COMPANY"].ToString();
                    string FACTOR_CATEGORY = ds.Tables[0].Rows[i]["FACTOR_CATEGORY"].ToString();
                    string FACTOR = ds.Tables[0].Rows[i]["FACTOR"].ToString();

                    string DP = ds.Tables[0].Rows[i]["DP"].ToString();
                    string TP = ds.Tables[0].Rows[i]["TP"].ToString();
                    string MRP = ds.Tables[0].Rows[i]["MRP"].ToString();
                    string WS = ds.Tables[0].Rows[i]["WS"].ToString();
                    string VAT = ds.Tables[0].Rows[i]["VAT"].ToString();

                    string STATUS = ds.Tables[0].Rows[i]["ACTIVENESS"].ToString();

                    msg = msg + ";" + ITEM_ID + ";" + ITEM_NAME + ";" + ITEM_SHORT_NAME + ";" + ITEM_BL_NAME + ";" + CLASS_ID + ";" + CLASS_NAME + ";" + ITEM_GROUP_ID + ";" + ITEM_GROUP_NAME + ";" + COMPANY_ID + ";" + COMPANY_NAME + ";" + MOTHER_COMPANY + ";" + FACTOR_CATEGORY + ";" + FACTOR + ";" + DP + ";" + TP + ";" + MRP + ";" + WS + ";" + VAT + ";" + STATUS;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod]
    public static string GetItemByGroup(string groupId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = @"SELECT ITEM_ID,ITEM_NAME FROM T_ITEM
                            WHERE ITEM_GROUP='" + groupId + "' AND ACTIVENESS='Y' ORDER BY ITEM_NAME";            

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string ITEM_ID = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string ITEM_NAME = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();

                    msg = msg + ";" + ITEM_ID + ";" + ITEM_NAME;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }



    [WebMethod]
    public static string GetHosTargetInfo(string hosId, string month, string year)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (hosId == "hosId")
            {
                query = @"SELECT * FROM T_HOS_TARGET";
            }
            else
            {
                query = @"SELECT * FROM T_HOS_TARGET WHERE HOS_ID='" + hosId + "' AND MONTH_NAME='" + month + "' AND YEAR='" + year + "'";

            }

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string HOS_ID = ds.Tables[0].Rows[i]["HOS_ID"].ToString();
                    string TARGET_AMT = ds.Tables[0].Rows[i]["TARGET_AMT"].ToString();
                    string MONTH_NAME = ds.Tables[0].Rows[i]["MONTH_NAME"].ToString();
                    string YEAR = ds.Tables[0].Rows[i]["YEAR"].ToString();


                    msg = msg + ";" + HOS_ID + ";" + TARGET_AMT + ";" + MONTH_NAME + ";" + YEAR;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
    
    [WebMethod]
    public static string GetRMTargetInfo(string hosId, string month, string year)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (hosId == "hosId")
            {
                query = @"SELECT * FROM T_RM_TARGET";
            }
            else
            {
                query = @"SELECT * FROM T_RM_TARGET WHERE RM_ID='" + hosId + "' AND MONTH_NAME='" + month + "' AND YEAR='" + year + "'";

            }

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string HOS_ID = ds.Tables[0].Rows[i]["RM_ID"].ToString();
                    string TARGET_AMT = ds.Tables[0].Rows[i]["TARGET_AMT"].ToString();
                    string MONTH_NAME = ds.Tables[0].Rows[i]["MONTH_NAME"].ToString();
                    string YEAR = ds.Tables[0].Rows[i]["YEAR"].ToString();


                    msg = msg + ";" + HOS_ID + ";" + TARGET_AMT + ";" + MONTH_NAME + ";" + YEAR;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
    [WebMethod]
    public static string GetTSMTargetInfo(string hosId, string month, string year)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (hosId == "hosId")
            {
                query = @"SELECT * FROM T_TSM_TARGET";
            }
            else
            {
                query = @"SELECT * FROM T_TSM_TARGET WHERE TSM_ID='" + hosId + "' AND MONTH_NAME='" + month + "' AND YEAR='" + year + "'";

            }

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string HOS_ID = ds.Tables[0].Rows[i]["TSM_ID"].ToString();
                    string TARGET_AMT = ds.Tables[0].Rows[i]["TARGET_AMT"].ToString();
                    string MONTH_NAME = ds.Tables[0].Rows[i]["MONTH_NAME"].ToString();
                    string YEAR = ds.Tables[0].Rows[i]["YEAR"].ToString();


                    msg = msg + ";" + HOS_ID + ";" + TARGET_AMT + ";" + MONTH_NAME + ";" + YEAR;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }

    [WebMethod]
    public static string GetSRTargetInfo(string hosId, string month, string year)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (hosId == "hosId")
            {
                query = @"SELECT * FROM T_SR_TARGET";
            }
            else
            {
                query = @"SELECT * FROM T_SR_TARGET WHERE SR_ID='" + hosId + "' AND MONTH_NAME='" + month + "' AND YEAR='" + year + "'";

            }

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string HOS_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string TARGET_AMT = ds.Tables[0].Rows[i]["TARGET_AMT"].ToString();
                    string MONTH_NAME = ds.Tables[0].Rows[i]["MONTH_NAME"].ToString();
                    string YEAR = ds.Tables[0].Rows[i]["YEAR"].ToString();


                    msg = msg + ";" + HOS_ID + ";" + TARGET_AMT + ";" + MONTH_NAME + ";" + YEAR;
                }

            }
            else
            {
                msg = "Not Exist";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }

}