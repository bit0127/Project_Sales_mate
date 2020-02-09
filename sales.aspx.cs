using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class sales : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HttpContext.Current.Session["todaysDate"] = Request.QueryString["todaysDate"].ToString();
            HttpContext.Current.Session["country"] = Request.QueryString["country"].ToString();
            HttpContext.Current.Session["company"] = Request.QueryString["company"].ToString();

            try
            {
                if (HttpContext.Current.Session["userid"].ToString() == "1122")
                {
                    Response.Redirect("dashboardcom.aspx", false);
                }
                else if (HttpContext.Current.Session["user_type"].ToString() == "pm")
                {
                    Response.Redirect("pmdashboard.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("login.aspx", false);
            }
        }
    }


    [WebMethod(EnableSession = true)]
    public static string CheckUserData(string uid, string pwd)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
        
        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT USER_ID,USER_TYPE FROM T_USER WHERE USER_ID='" + uid + "' AND USER_PWD='" + pwd + "'";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0)
            {
                msg = ds.Tables[0].Rows[0]["USER_TYPE"].ToString();
                HttpContext.Current.Session["user_type"] = msg;
                HttpContext.Current.Session["userid"] = ds.Tables[0].Rows[0]["USER_ID"].ToString();
            }
            else
            {
                msg = "You are not permitted";
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
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP
                            WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString().Trim() + "'";
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
    public static string GetOutletInfo(string division, string zone, string group)
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
                            (SELECT OUTLET_ID,OUTLET_NAME,OUTLET_ADDRESS,PROPRITOR_NAME,MOBILE_NUMBER,DIVISION_ID,ZONE_ID,ROUTE_ID FROM T_OUTLET WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='"+group.Trim()+"') AND ZONE_ID='"+zone.Trim()+"') T1, ";
            query = query + @"(SELECT * FROM T_DIVISION) T2
                            WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                            (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE) T4
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
                    string OUTLET_NAME = ds.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                    string OUTLET_ADDRESS = ds.Tables[0].Rows[i]["OUTLET_ADDRESS"].ToString();
                    string PROPRITOR_NAME = ds.Tables[0].Rows[i]["PROPRITOR_NAME"].ToString();
                    string MOBILE_NUMBER = ds.Tables[0].Rows[i]["MOBILE_NUMBER"].ToString();
                    string ROUTE_NAME = ds.Tables[0].Rows[i]["ROUTE_NAME"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();



                    msg = msg + ";" + OUTLET_NAME + ";" + OUTLET_ADDRESS + ";" + PROPRITOR_NAME + ";" + MOBILE_NUMBER + ";" + ROUTE_NAME + ";" + ZONE_NAME + ";" + DIVISION_NAME;
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
    public static string GetDivision(string countryName)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION
                            WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString() + "%'";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string comId = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string comName = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
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
    public static string GetSRInfo(string company)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT T3.ITEM_GROUP,T3.SR_ID,T3.SR_NAME,T3.MOBILE_NO,T3.DIST_ID,T3.DIVISION_NAME,T4.ZONE_NAME FROM
                            (SELECT T1.*,T2.DIVISION_NAME FROM
                            (SELECT SR_ID,SR_NAME,MOBILE_NO,DIST_ID,DIVISION_NAME DIVISION_ID,DIST_ZONE,ITEM_GROUP FROM T_SR_INFO  
                            WHERE ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "' AND COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString() + "%' AND STATUS='Y') T1, ";
            query = query + @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T2
                            WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                            (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE) T4
                            WHERE T3.DIST_ZONE=T4.ZONE_ID";

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string ITEM_GROUP = ds.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                    string SR_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string SR_NAME = ds.Tables[0].Rows[i]["SR_NAME"].ToString();
                    string MOBILE_NO = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string DIST_ID = ds.Tables[0].Rows[i]["DIST_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();

                    msg = msg + ";" + ITEM_GROUP + ";" + SR_ID + ";" + SR_NAME + ";" + MOBILE_NO + ";" + DIST_ID + ";" + ZONE_NAME + ";" + DIVISION_NAME;
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
    public static string GetActiveSRInfo(string company, string division, string zone, string txtDate, string country)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
        
        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";

            if (division != "All" && zone != "All")
            {
                query = @"SELECT T5.* FROM  
                         (SELECT T3.*,T4.ZONE_NAME FROM
                          (SELECT T1.SR_ID,T1.SR_NAME,T1.MOBILE_NO,T1.ITEM_GROUP,T1.DIST_ZONE,T2.DIVISION_NAME FROM                                          
                         (SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP,DIVISION_NAME,DIST_ZONE FROM T_SR_INFO  
                         WHERE STATUS='Y' AND COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "' AND DIVISION_NAME = '" + division + "' AND DIST_ZONE='" + zone + "' AND SR_ID IN(SELECT DISTINCT SR_ID FROM T_ORDER_DETAIL WHERE ENTRY_DATE = TO_DATE('" + txtDate + "','dd/mm/yyyy') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID IN (SELECT DISTINCT ITEM_GROUP FROM T_ITEM WHERE OWN_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "')))) T1, ";
                query = query + @" (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T2 ";
                query = query + @" WHERE T1.DIVISION_NAME=T2.DIVISION_ID) T3,
                                   (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T4 ";
                query = query + @" WHERE T3.DIST_ZONE=T4.ZONE_ID) T5";
            }
            if (division == "All" && zone == "All")
            {
                /*query = @"SELECT T5.*,T6.TSM FROM  
                         (SELECT T3.*,T4.ZONE_NAME FROM
                          (SELECT T1.SR_ID,T1.SR_NAME,T1.MOBILE_NO,T1.ITEM_GROUP,T1.DIST_ZONE,T2.DIVISION_NAME FROM                                          
                         (SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP,DIVISION_NAME,DIST_ZONE FROM T_SR_INFO  
                         WHERE ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "' AND COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "' AND SR_ID IN(SELECT DISTINCT SR_ID FROM T_ORDER_DETAIL WHERE ENTRY_DATE = TO_DATE('" + txtDate + "','dd/mm/yyyy'))) T1, ";
                query = query + @" (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T2 ";
                query = query + @" WHERE T1.DIVISION_NAME=T2.DIVISION_ID) T3,
                         (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T4 ";
                query = query + @" WHERE T3.DIST_ZONE=T4.ZONE_ID)T5,
                                                    (SELECT T1.*,(T2.TSM_NAME || '-' || T2.MOBILE_NO) TSM FROM
                                                    (SELECT SR_ID,TSM_ID FROM T_TSM_SR) T1,
                                                    (SELECT * FROM T_TSM_ZM) T2
                                                    WHERE T1.TSM_ID=T2.TSM_ID) T6
                                                    WHERE T5.SR_ID=T6.SR_ID";*/
                query = @"SELECT T5.* FROM  
                         (SELECT T3.*,T4.ZONE_NAME FROM
                          (SELECT T1.SR_ID,T1.SR_NAME,T1.MOBILE_NO,T1.ITEM_GROUP,T1.DIST_ZONE,T2.DIVISION_NAME FROM                                          
                         (SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP,DIVISION_NAME,DIST_ZONE FROM T_SR_INFO  
                         WHERE STATUS='Y' AND ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "' AND COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "' AND SR_ID IN(SELECT DISTINCT SR_ID FROM T_ORDER_DETAIL WHERE ENTRY_DATE = TO_DATE('" + txtDate + "','dd/mm/yyyy'))) T1, ";
                query = query + @" (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T2 ";
                query = query + @" WHERE T1.DIVISION_NAME=T2.DIVISION_ID) T3,
                         (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T4 ";
                query = query + @" WHERE T3.DIST_ZONE=T4.ZONE_ID) T5";
            }

            else if (division != "All" && zone == "All")
            {
                query = @"SELECT T5.* FROM  
                         (SELECT T3.*,T4.ZONE_NAME FROM
                          (SELECT T1.SR_ID,T1.SR_NAME,T1.MOBILE_NO,T1.ITEM_GROUP,T1.DIST_ZONE,T2.DIVISION_NAME FROM                                          
                         (SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP,DIVISION_NAME,DIST_ZONE FROM T_SR_INFO  
                         WHERE STATUS='Y' AND ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "' AND COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "' AND DIVISION_NAME = '" + division + "' AND SR_ID IN(SELECT DISTINCT SR_ID FROM T_ORDER_DETAIL WHERE ENTRY_DATE = TO_DATE('" + txtDate + "','dd/mm/yyyy'))) T1, ";
                query = query + @" (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "' AND DIVISION_ID = '" + division + "') T2 ";
                query = query + @" WHERE T1.DIVISION_NAME=T2.DIVISION_ID) T3,
                         (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T4 ";
                query = query + @" WHERE T3.DIST_ZONE=T4.ZONE_ID) T5";
            }
            else if (division == "All" && zone != "All")
            {
                query = @"SELECT T5.* FROM  
                         (SELECT T3.*,T4.ZONE_NAME FROM
                          (SELECT T1.SR_ID,T1.SR_NAME,T1.MOBILE_NO,T1.ITEM_GROUP,T1.DIST_ZONE,T2.DIVISION_NAME FROM                                          
                         (SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP,DIVISION_NAME,DIST_ZONE FROM T_SR_INFO  
                         WHERE STATUS='Y' AND ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "' AND COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "' AND DIST_ZONE='" + zone + "' AND SR_ID IN(SELECT DISTINCT SR_ID FROM T_ORDER_DETAIL WHERE ENTRY_DATE = TO_DATE('" + txtDate + "','dd/mm/yyyy'))) T1, ";
                query = query + @" (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T2 ";
                query = query + @" WHERE T1.DIVISION_NAME=T2.DIVISION_ID) T3,
                         (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "' AND ZONE_ID='" + zone + "') T4 ";
                query = query + @" WHERE T3.DIST_ZONE=T4.ZONE_ID) T5";
            }
            else if (division == "All" && zone == "All")
            {
                query = @"SELECT T5.* FROM  
                         (SELECT T3.*,T4.ZONE_NAME FROM
                          (SELECT T1.SR_ID,T1.SR_NAME,T1.MOBILE_NO,T1.ITEM_GROUP,T1.DIST_ZONE,T2.DIVISION_NAME FROM                                          
                         (SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP,DIVISION_NAME,DIST_ZONE FROM T_SR_INFO  
                         WHERE STATUS='Y' AND ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "' AND COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "' AND SR_ID IN(SELECT DISTINCT SR_ID FROM T_ORDER_DETAIL WHERE ENTRY_DATE = TO_DATE('" + txtDate + "','dd/mm/yyyy'))) T1, ";
                query = query + @" (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T2 ";
                query = query + @" WHERE T1.DIVISION_NAME=T2.DIVISION_ID) T3,
                         (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T4 ";
                query = query + @" WHERE T3.DIST_ZONE=T4.ZONE_ID) T5";
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
                    string srId = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string srName = ds.Tables[0].Rows[i]["SR_NAME"].ToString();
                    string phone = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string group = ds.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                    string zoneName = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                    string divisionName = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string tsm = "No TSM Assigned";// ds.Tables[0].Rows[i]["TSM"].ToString();

                    string qrTsm = @"SELECT T1.*,(T2.TSM_NAME || '-' || T2.MOBILE_NO) TSM FROM
                                   (SELECT SR_ID,TSM_ID FROM T_TSM_SR WHERE SR_ID='" + srId + "') T1, ";
                    qrTsm = qrTsm + @"(SELECT * FROM T_TSM_ZM WHERE STATUS='Y') T2
                                    WHERE T1.TSM_ID=T2.TSM_ID";
                    OracleCommand cmdTSM = new OracleCommand(qrTsm, conn);
                    OracleDataAdapter daTSM = new OracleDataAdapter(cmdTSM);
                    DataSet dsTSM = new DataSet();
                    daTSM.Fill(dsTSM);
                    int t = dsTSM.Tables[0].Rows.Count;
                    if (t > 0 && dsTSM.Tables[0].Rows[0]["SR_ID"].ToString() != "")
                    {
                        tsm = dsTSM.Tables[0].Rows[0]["TSM"].ToString();
                    }

                    msg = msg + ";" + group + ";" + srId + ";" + srName + ";" + phone + ";" + tsm + ";" + zoneName + ";" + divisionName;
                }
                 
            }
            else
            {
                msg = "No active SR";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod(EnableSession = true)]
    public static string GetInActiveSRInfo(string company, string division, string zone, string txtDate, string country)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (division != "All" && zone != "All")
            {
                query = @"SELECT T5.* FROM  
                         (SELECT T3.*,T4.ZONE_NAME FROM
                          (SELECT T1.SR_ID,T1.SR_NAME,T1.MOBILE_NO,T1.ITEM_GROUP,T1.DIST_ZONE,T2.DIVISION_NAME FROM                                          
                         (SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP,DIVISION_NAME,DIST_ZONE FROM T_SR_INFO  
                         WHERE STATUS='Y' AND COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "' AND DIVISION_NAME = '" + division + "' AND DIST_ZONE='" + zone + "' AND SR_ID NOT IN(SELECT DISTINCT SR_ID FROM T_ORDER_DETAIL WHERE ENTRY_DATE = TO_DATE('" + txtDate + "','dd/mm/yyyy') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID IN (SELECT DISTINCT ITEM_GROUP FROM T_ITEM WHERE OWN_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "')))) T1, ";
                query = query + @" (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T2 ";
                query = query + @" WHERE T1.DIVISION_NAME=T2.DIVISION_ID) T3,
                                   (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T4 ";
                query = query + @" WHERE T3.DIST_ZONE=T4.ZONE_ID) T5";
            }
            if (division == "All" && zone == "All")
            {
                query = @"SELECT T5.* FROM  
                         (SELECT T3.*,T4.ZONE_NAME FROM
                          (SELECT T1.SR_ID,T1.SR_NAME,T1.MOBILE_NO,T1.ITEM_GROUP,T1.DIST_ZONE,T2.DIVISION_NAME FROM                                          
                         (SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP,DIVISION_NAME,DIST_ZONE FROM T_SR_INFO  
                         WHERE STATUS='Y' AND ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "' AND COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "' AND SR_ID NOT IN(SELECT DISTINCT SR_ID FROM T_ORDER_DETAIL WHERE ENTRY_DATE = TO_DATE('" + txtDate + "','dd/mm/yyyy'))) T1, ";
                query = query + @" (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T2 ";
                query = query + @" WHERE T1.DIVISION_NAME=T2.DIVISION_ID) T3,
                         (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T4 ";
                query = query + @" WHERE T3.DIST_ZONE=T4.ZONE_ID) T5";
            }
           
            else if (division != "All" && zone == "All")
            {
                query = @"SELECT T5.* FROM  
                         (SELECT T3.*,T4.ZONE_NAME FROM
                          (SELECT T1.SR_ID,T1.SR_NAME,T1.MOBILE_NO,T1.ITEM_GROUP,T1.DIST_ZONE,T2.DIVISION_NAME FROM                                          
                         (SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP,DIVISION_NAME,DIST_ZONE FROM T_SR_INFO  
                         WHERE STATUS='Y' AND ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "' AND COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "' AND DIVISION_NAME = '" + division + "' AND SR_ID NOT IN(SELECT DISTINCT SR_ID FROM T_ORDER_DETAIL WHERE ENTRY_DATE = TO_DATE('" + txtDate + "','dd/mm/yyyy'))) T1, ";
                query = query + @" (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "' AND DIVISION_ID = '" + division + "') T2 ";
                query = query + @" WHERE T1.DIVISION_NAME=T2.DIVISION_ID) T3,
                         (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T4 ";
                query = query + @" WHERE T3.DIST_ZONE=T4.ZONE_ID) T5";
            }
            else if (division == "All" && zone != "All")
            {
                query = @"SELECT T5.* FROM  
                         (SELECT T3.*,T4.ZONE_NAME FROM
                          (SELECT T1.SR_ID,T1.SR_NAME,T1.MOBILE_NO,T1.ITEM_GROUP,T1.DIST_ZONE,T2.DIVISION_NAME FROM                                          
                         (SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP,DIVISION_NAME,DIST_ZONE FROM T_SR_INFO  
                         WHERE STATUS='Y' AND ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "' AND COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "' AND DIST_ZONE='" + zone + "' AND SR_ID NOT IN(SELECT DISTINCT SR_ID FROM T_ORDER_DETAIL WHERE ENTRY_DATE = TO_DATE('" + txtDate + "','dd/mm/yyyy'))) T1, ";
                query = query + @" (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T2 ";
                query = query + @" WHERE T1.DIVISION_NAME=T2.DIVISION_ID) T3,
                         (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "' AND ZONE_ID='" + zone + "') T4 ";
                query = query + @" WHERE T3.DIST_ZONE=T4.ZONE_ID) T5";
            }
            else if (division == "All" && zone == "All")
            {
                query = @"SELECT T5.* FROM  
                         (SELECT T3.*,T4.ZONE_NAME FROM
                          (SELECT T1.SR_ID,T1.SR_NAME,T1.MOBILE_NO,T1.ITEM_GROUP,T1.DIST_ZONE,T2.DIVISION_NAME FROM                                          
                         (SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP,DIVISION_NAME,DIST_ZONE FROM T_SR_INFO  
                         WHERE STATUS='Y' AND ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "' AND COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "' AND SR_ID NOT IN(SELECT DISTINCT SR_ID FROM T_ORDER_DETAIL WHERE ENTRY_DATE = TO_DATE('" + txtDate + "','dd/mm/yyyy'))) T1, ";
                query = query + @" (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T2 ";
                query = query + @" WHERE T1.DIVISION_NAME=T2.DIVISION_ID) T3,
                         (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE WHERE COUNTRY_NAME='" + HttpContext.Current.Session["country"].ToString() + "') T4 ";
                query = query + @" WHERE T3.DIST_ZONE=T4.ZONE_ID) T5";
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
                    string srId = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string srName = ds.Tables[0].Rows[i]["SR_NAME"].ToString();
                    string phone = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string group = ds.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                    string zoneName = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                    string divisionName = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string tsm = "No TSM Assigned";

                    string qrTsm = @"SELECT T1.*,(T2.TSM_NAME || '-' || T2.MOBILE_NO) TSM FROM
                                   (SELECT SR_ID,TSM_ID FROM T_TSM_SR WHERE SR_ID='" + srId + "') T1, ";
                    qrTsm = qrTsm + @"(SELECT * FROM T_TSM_ZM) T2
                                    WHERE T1.TSM_ID=T2.TSM_ID";
                    OracleCommand cmdTSM = new OracleCommand(qrTsm, conn);
                    OracleDataAdapter daTSM = new OracleDataAdapter(cmdTSM);
                    DataSet dsTSM = new DataSet();
                    daTSM.Fill(dsTSM);
                    int t = dsTSM.Tables[0].Rows.Count;
                    if (t > 0 && dsTSM.Tables[0].Rows[0]["SR_ID"].ToString() != "")
                    {
                        tsm = dsTSM.Tables[0].Rows[0]["TSM"].ToString();
                    }

                    string remarks = "Active but not working";

                    string qrLeave = @"SELECT * FROM T_LEAVE WHERE T_DATE=TO_DATE('" + txtDate.Trim() + "','DD/MM/YYYY') AND SR_ID='" + srId + "'";
                    OracleCommand cmdL = new OracleCommand(qrLeave, conn);
                    OracleDataAdapter daL = new OracleDataAdapter(cmdL);
                    DataSet dsL = new DataSet();
                    daL.Fill(dsL);
                    int l = dsL.Tables[0].Rows.Count;
                    if (l > 0 && dsL.Tables[0].Rows[0][0].ToString() != "") 
                    {
                        remarks = "Leave";
                    }
                    else 
                    {
                        string qrLWP = @"SELECT * FROM T_LWP WHERE LWP_DATE=TO_DATE('" + txtDate.Trim() + "','DD/MM/YYYY') AND SR_ID='" + srId + "'";
                        OracleCommand cmdLWP = new OracleCommand(qrLWP, conn);
                        OracleDataAdapter daLWP = new OracleDataAdapter(cmdLWP);
                        DataSet dsLWP = new DataSet();
                        daLWP.Fill(dsLWP);
                        int lwp = dsLWP.Tables[0].Rows.Count;
                        if (lwp > 0 && dsLWP.Tables[0].Rows[0][0].ToString() != "")
                        {
                            remarks = "LWP";
                        }
                    }

                    msg = msg + ";" + group + ";" + srId + ";" + srName + ";" + phone + ";" + tsm + ";" + zoneName + ";" + divisionName + ";" + remarks;
                }

            }
            else
            {
                msg = "No inactive SR";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
    [WebMethod(EnableSession = true)]
    public static string GetSRLeaveInfo(string company, string division, string zone, string txtDate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (division != "All" && zone != "All")
            {
                query = @"SELECT T8.*,T7.REASON FROM
                            (SELECT DISTINCT SR_ID,REASON FROM T_LEAVE
                            WHERE FROM_DATE BETWEEN TO_DATE('" + txtDate.Trim() + "','DD/MM/YYYY') AND TO_DATE('" + txtDate.Trim() + "','DD/MM/YYYY')) T7, ";
                query = query + @" (SELECT T5.*,T6.TSM FROM
                            (SELECT T3.*,T4.ZONE_NAME FROM
                            (SELECT T1.SR_ID,T1.SR_NAME,T1.MOBILE_NO,T1.ITEM_GROUP,T1.DIST_ZONE,T2.DIVISION_NAME FROM
                            (SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP,DIVISION_NAME,DIST_ZONE FROM T_SR_INFO
                            WHERE STATUS='Y' AND ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "' AND DIVISION_NAME='" + division + "' AND DIST_ZONE='" + zone + "') T1, ";
                query = query + @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T2
                            WHERE T1.DIVISION_NAME=T2.DIVISION_ID) T3,
                            (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE) T4
                            WHERE T3.DIST_ZONE=T4.ZONE_ID) T5,
                            (SELECT T1.*,(T2.TSM_NAME || '-' || T2.MOBILE_NO) TSM FROM
                            (SELECT SR_ID,TSM_ID FROM T_TSM_SR WHERE STATUS='Y') T1,
                            (SELECT * FROM T_TSM_ZM) T2
                            WHERE T1.TSM_ID=T2.TSM_ID) T6
                            WHERE T5.SR_ID=T6.SR_ID) T8
                            WHERE T7.SR_ID=T8.SR_ID";
            }
            else if (division == "All")
            {
                query = @"SELECT T8.*,T7.REASON FROM
                            (SELECT DISTINCT SR_ID,REASON FROM T_LEAVE
                            WHERE FROM_DATE BETWEEN TO_DATE('" + txtDate.Trim() + "','DD/MM/YYYY') AND TO_DATE('" + txtDate.Trim() + "','DD/MM/YYYY')) T7, ";
                query = query + @" (SELECT T5.*,T6.TSM FROM
                            (SELECT T3.*,T4.ZONE_NAME FROM
                            (SELECT T1.SR_ID,T1.SR_NAME,T1.MOBILE_NO,T1.ITEM_GROUP,T1.DIST_ZONE,T2.DIVISION_NAME FROM
                            (SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP,DIVISION_NAME,DIST_ZONE FROM T_SR_INFO
                            WHERE STATUS='Y' AND ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "') T1, ";
                query = query + @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T2
                            WHERE T1.DIVISION_NAME=T2.DIVISION_ID) T3,
                            (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE) T4
                            WHERE T3.DIST_ZONE=T4.ZONE_ID) T5,
                            (SELECT T1.*,(T2.TSM_NAME || '-' || T2.MOBILE_NO) TSM FROM
                            (SELECT SR_ID,TSM_ID FROM T_TSM_SR WHERE STATUS='Y') T1,
                            (SELECT * FROM T_TSM_ZM) T2
                            WHERE T1.TSM_ID=T2.TSM_ID) T6
                            WHERE T5.SR_ID=T6.SR_ID) T8
                            WHERE T7.SR_ID=T8.SR_ID";
            }
            else if (division != "All" && zone == "All")
            {
                query = @"SELECT T8.*,T7.REASON FROM
                            (SELECT DISTINCT SR_ID,REASON FROM T_LEAVE
                            WHERE FROM_DATE BETWEEN TO_DATE('" + txtDate.Trim() + "','DD/MM/YYYY') AND TO_DATE('" + txtDate.Trim() + "','DD/MM/YYYY')) T7, ";
                query = query + @" (SELECT T5.*,T6.TSM FROM
                            (SELECT T3.*,T4.ZONE_NAME FROM
                            (SELECT T1.SR_ID,T1.SR_NAME,T1.MOBILE_NO,T1.ITEM_GROUP,T1.DIST_ZONE,T2.DIVISION_NAME FROM
                            (SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP,DIVISION_NAME,DIST_ZONE FROM T_SR_INFO
                            WHERE STATUS='Y' AND ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "' AND DIVISION_NAME='" + division + "') T1, ";
                query = query + @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T2
                            WHERE T1.DIVISION_NAME=T2.DIVISION_ID) T3,
                            (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE) T4
                            WHERE T3.DIST_ZONE=T4.ZONE_ID) T5,
                            (SELECT T1.*,(T2.TSM_NAME || '-' || T2.MOBILE_NO) TSM FROM
                            (SELECT SR_ID,TSM_ID FROM T_TSM_SR WHERE STATUS='Y') T1,
                            (SELECT * FROM T_TSM_ZM) T2
                            WHERE T1.TSM_ID=T2.TSM_ID) T6
                            WHERE T5.SR_ID=T6.SR_ID) T8
                            WHERE T7.SR_ID=T8.SR_ID";
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
                    string srId = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string srName = ds.Tables[0].Rows[i]["SR_NAME"].ToString();
                    string phone = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string group = ds.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                    string zoneName = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                    string divisionName = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string tsm = ds.Tables[0].Rows[i]["TSM"].ToString();
                    string reason = ds.Tables[0].Rows[i]["REASON"].ToString();

                    msg = msg + ";" + group + ";" + srId + ";" + srName + ";" + phone + ";" + tsm + ";" + zoneName + ";" + divisionName + ";" + reason;
                }

            }
            else
            {
                msg = "No SR on Leave";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }

   

    [WebMethod]
    public static string GetTodaysSRAttendance(string groupId, string fdate, string tdate)
    {
        string msg = "";        

        try
        {
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());
             
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            try
            {
                /*string q = @"SELECT * FROM T_TRADE_PROGRAM";
                OracleCommand cmdPs = new OracleCommand(q, con);
                OracleDataAdapter daPs = new OracleDataAdapter(cmdPs);
                DataSet dsPs = new DataSet();
                daPs.Fill(dsPs);
                int cs = dsPs.Tables[0].Rows.Count;
                if (cs > 0 && dsPs.Tables[0].Rows[0][0].ToString() != "")
                {
                    for (int k = 0; k < cs; k++)
                    {
                        string ITEM_ID = dsPs.Tables[0].Rows[k]["ITEM_ID"].ToString();
                        string qd = @"UPDATE T_TRADE_PROGRAM SET PROGRAM_ID=(SALES.SEQ_TRADE_PROGRAM.NEXTVAL) WHERE ITEM_ID='" + ITEM_ID + "' AND PROGRAM_ID IS NULL";
                        OracleCommand cmd = new OracleCommand(qd, con);
                        int ik = cmd.ExecuteNonQuery();
                    }
                }*/

                string qrSr = @"SELECT TBL4.ITEM_GROUP,TBL4.SR_NAME,TBL4.MOBILE_NO,TBL3.* FROM                            
                                (SELECT TBL1.ROUTE_ID,TBL1.SR_ID,TO_CHAR(SUM(TBL1.AMT)/1000,'999999999.999') TOTAL_AMT, TO_CHAR(ROUND(SUM(TBL1.AMT)/26)/1000,'999999999.999') AVG_AMT ,TBL2.TOTAL_MEMO,TBL2.AVG_MEMO FROM
                                (SELECT DISTINCT T3.ROUTE_ID,T3.SR_ID,SUM((T3.ITEM_QTY+(T3.ITEM_CTN*T3.FACTOR))*T3.OUT_PRICE) AMT,T3.ENTRY_DATE FROM                  
                                (SELECT T1.*,T2.FACTOR FROM         
                                (SELECT * FROM T_ORDER_DETAIL    
                                WHERE ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y')) T1, ";
                qrSr = qrSr + @" (SELECT ITEM_ID,FACTOR FROM T_ITEM) T2     
                                WHERE T1.ITEM_ID=T2.ITEM_ID) T3  
                                GROUP BY T3.ROUTE_ID,T3.SR_ID,T3.OUTLET_ID,T3.ENTRY_DATE ORDER BY T3.ENTRY_DATE) TBL1, 

                                (SELECT SR_ID,COUNT(OUTLET_ID)TOTAL_MEMO,ROUND(COUNT(OUTLET_ID)/25.75)AVG_MEMO FROM
                                 (SELECT DISTINCT SR_ID,OUTLET_ID,ENTRY_DATE FROM T_ORDER_DETAIL
                                 WHERE ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y') ORDER BY ENTRY_DATE) TBL1 GROUP BY SR_ID ) TBL2  ";
                qrSr = qrSr + @" WHERE TBL1.SR_ID=TBL2.SR_ID  
                                 GROUP BY TBL1.ROUTE_ID,TBL1.SR_ID,TBL2.TOTAL_MEMO,TBL2.AVG_MEMO) TBL3,   
                                 (SELECT ITEM_GROUP,SR_ID,SR_NAME,MOBILE_NO FROM T_SR_INFO) TBL4
                                 WHERE TBL3.SR_ID=TBL4.SR_ID ";

                OracleCommand cmdP = new OracleCommand(qrSr, con);
                OracleDataAdapter daP = new OracleDataAdapter(cmdP);
                DataSet dsP = new DataSet();
                daP.Fill(dsP);
                int c = dsP.Tables[0].Rows.Count;
                if (c > 0 && dsP.Tables[0].Rows[0][0].ToString() != "")
                {
                    for (int i = 0; i < c; i++)
                    {
                        string srId = dsP.Tables[0].Rows[i]["SR_ID"].ToString();
                        string group = dsP.Tables[0].Rows[i]["ITEM_GROUP"].ToString();                        
                        string totalAmt = dsP.Tables[0].Rows[i]["TOTAL_AMT"].ToString();
                        string avgAmt = dsP.Tables[0].Rows[i]["AVG_AMT"].ToString();
                        string totalMemo = dsP.Tables[0].Rows[i]["TOTAL_MEMO"].ToString();
                        string avgMemo = dsP.Tables[0].Rows[i]["AVG_MEMO"].ToString();
                        string srName = dsP.Tables[0].Rows[i]["SR_NAME"].ToString() + "(" + srId + ")";
                        string srMobileNo = dsP.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                        string routeId = dsP.Tables[0].Rows[i]["ROUTE_ID"].ToString();

                        string intime = "";
                        string qrT = @"SELECT MIN(to_char(ENTRY_DATETIME, 'HH24:MI:SS' ))INTIME FROM T_ORDER_DETAIL
                                     WHERE SR_ID='" + srId + "' AND ENTRY_DATE=TO_DATE('" + fdate + "','dd/mm/yyyy') ORDER BY ENTRY_DATETIME ASC";
                        OracleCommand cmdT = new OracleCommand(qrT, con);
                        OracleDataAdapter daT = new OracleDataAdapter(cmdT);
                        DataSet dsT = new DataSet();
                        daT.Fill(dsT);
                        int t = dsT.Tables[0].Rows.Count;
                        if (t > 0 && dsT.Tables[0].Rows[0][0].ToString() != "")
                        {
                            intime = dsT.Tables[0].Rows[0][0].ToString();
                        }

                        string leave = "0";
                        string lwp = "0";


                        string qrLv = @"SELECT SR_ID FROM T_LEAVE
                                       WHERE E_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy') AND SR_ID='" + srId + "'";

                        OracleCommand cmdLv = new OracleCommand(qrLv, con);
                        OracleDataAdapter daLv = new OracleDataAdapter(cmdLv);
                        DataSet dsLv = new DataSet();
                        daLv.Fill(dsLv);
                        int lv = dsLv.Tables[0].Rows.Count;
                        if (lv > 0 && dsLv.Tables[0].Rows[0][0].ToString() != "")
                        {
                            leave = lv.ToString();
                        }

                        string qrLwp = @"SELECT SR_ID,COUNT(SR_ID)LWP FROM T_LWP
                                        WHERE ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy') AND SR_ID='" + srId + "' GROUP BY SR_ID";

                        OracleCommand cmdLwp = new OracleCommand(qrLwp, con);
                        OracleDataAdapter daLwp = new OracleDataAdapter(cmdLwp);
                        DataSet dsLwp = new DataSet();
                        daLwp.Fill(dsLwp);
                        int lp = dsLwp.Tables[0].Rows.Count;
                        if (lp > 0 && dsLwp.Tables[0].Rows[0][0].ToString() != "")
                        {
                            lwp = dsLwp.Tables[0].Rows[0][1].ToString();
                        }

                        string route = "";
                        string qrRoute = @"SELECT ROUTE_NAME FROM T_ROUTE   
                                            WHERE ROUTE_ID IN (SELECT DISTINCT ROUTE_ID FROM T_ORDER_DETAIL
                                            WHERE SR_ID='" + srId + "' AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy') AND ROWNUM<2 )";
                        OracleCommand cmdR = new OracleCommand(qrRoute, con);
                        OracleDataAdapter daR = new OracleDataAdapter(cmdR);
                        DataSet dsR = new DataSet();
                        daR.Fill(dsR);
                        int r = dsR.Tables[0].Rows.Count;
                        if (r > 0 && dsR.Tables[0].Rows[0][0].ToString() != "")
                        {
                            route = dsR.Tables[0].Rows[0]["ROUTE_NAME"].ToString();
                        }

                        string srZone = "No Zone";
                        string qrZone = @"SELECT ZONE_NAME FROM T_ZONE WHERE ZONE_ID IN(SELECT DIST_ZONE FROM T_SR_INFO WHERE SR_ID='" + srId + "')";

                        OracleCommand cmdZ = new OracleCommand(qrZone, con);
                        OracleDataAdapter daZ = new OracleDataAdapter(cmdZ);
                        DataSet dsZ = new DataSet();
                        daZ.Fill(dsZ);
                        int z = dsZ.Tables[0].Rows.Count;
                        if (z > 0 && dsZ.Tables[0].Rows[0][0].ToString() != "")
                        {
                            srZone = dsZ.Tables[0].Rows[0]["ZONE_NAME"].ToString();
                        }
                         

                        string visitedOutlet = "0";

                        string qrVolt = @"SELECT DISTINCT OUTLET_ID FROM T_ORDER_DETAIL
                                        WHERE SR_ID='" + srId + "' AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy') ";
                        qrVolt = qrVolt + @" UNION
                                        SELECT DISTINCT OUTLET_ID FROM T_NON_PRODUCTIVE_SALES
                                        WHERE SR_ID='" + srId + "' AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy')";
   
                        OracleCommand cmdV = new OracleCommand(qrVolt, con);
                        OracleDataAdapter daV = new OracleDataAdapter(cmdV);
                        DataSet dsV = new DataSet();
                        daV.Fill(dsV);
                        int v = dsV.Tables[0].Rows.Count;
                        if (v > 0 && dsV.Tables[0].Rows[0][0].ToString() != "")
                        {
                            visitedOutlet = v.ToString();
                        }

                        string totalOutlet = "0";
                        string qrTolt = @"SELECT DISTINCT OUTLET_ID,OUTLET_NAME FROM T_OUTLET
                                        WHERE STATUS='Y' AND  ROUTE_ID='" + routeId + "' AND SR_ID='" + srId + "'";


                        OracleCommand cmdTt = new OracleCommand(qrTolt, con);
                        OracleDataAdapter daTt = new OracleDataAdapter(cmdTt);
                        DataSet dsTt = new DataSet();
                        daTt.Fill(dsTt);
                        int tt = dsTt.Tables[0].Rows.Count;
                        if (tt > 0 && dsTt.Tables[0].Rows[0][0].ToString() != "")
                        {
                            totalOutlet = tt.ToString();
                        }

                        //------lpc---------
                        string lpcLine = "0";
                        string qrLpc = @"SELECT ITEM_ID FROM T_ORDER_DETAIL
                                        WHERE ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy') AND SR_ID='" + srId + "'";

                        OracleCommand cmdLpc = new OracleCommand(qrLpc, con);
                        OracleDataAdapter daLpc = new OracleDataAdapter(cmdLpc);
                        DataSet dsLpc = new DataSet();
                        daLpc.Fill(dsLpc);
                        int lpn = dsLpc.Tables[0].Rows.Count;
                        if (lpn > 0 && dsLpc.Tables[0].Rows[0][0].ToString() != "")
                        {
                            lpcLine = lpn.ToString();
                        }

                        double lpcL = Convert.ToDouble(lpcLine);
                        double tMo = Convert.ToDouble(totalMemo);
                        string lpc = TotalAmount(lpcL / tMo);

                        int tOlt = Convert.ToInt32(totalOutlet);
                        int tVolt = Convert.ToInt32(visitedOutlet);
                        if (tVolt > tOlt)
                        {
                            tVolt = tOlt;
                            visitedOutlet = tOlt.ToString();
                        }

                        double avgOlt = ((double)tVolt / (double)tOlt);
                        //string vOltPcnt = visitedOutlet + " of " + totalOutlet + "(" + DisplayPercentage(avgOlt) + ")";
                        string vOltPcnt = DisplayPercentage(avgOlt);
                        int pendingOlt = (Convert.ToInt32(totalOutlet) - Convert.ToInt32(visitedOutlet));

                        //msg = msg + ";" + srName + ";" + srMobileNo + ";" + totalAmt + ";" + avgAmt + ";" + totalMemo + ";" + avgMemo + ";" + leave + ";" + lwp + ";" + company + ";" + srZone + ";" + vOltPcnt + ";" + intime;
                        msg = msg + ";" + srName + ";" + srMobileNo + ";" + totalAmt + ";" + avgAmt + ";" + totalMemo + ";" + route + ";" + leave + ";" + lwp + ";" + group + ";" + srZone + ";" + vOltPcnt + ";" + intime + ";" + pendingOlt.ToString() + ";" + lpc + ";" + totalOutlet + ";" + visitedOutlet;
                    }
                }
            }
            catch (Exception ex) { }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        catch (Exception ex) { }

        return msg;
    }



    [WebMethod]
    public static string GetProductivityReport(string group, string fdate, string tdate)
    {
        string msg = "";

        try
        {
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            try
            {
                string qrGroup = "";

                if (group == "All")
                {
                    qrGroup = @"SELECT T1.*,T2.COMPANY_NICK_NAME FROM
                                (SELECT COMPANY_ID,ITEM_GROUP_ID,ITEM_GROUP_NAME,MOTHER_COMPANY FROM T_ITEM_GROUP WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString() + "') T1, ";
                    qrGroup = qrGroup + @" (SELECT COMPANY_ID,COMPANY_NICK_NAME FROM T_COMPANY) T2
                                WHERE T1.COMPANY_ID=T2.COMPANY_ID";
                }
                else
                {
                    qrGroup = @"SELECT T1.*,T2.COMPANY_NICK_NAME FROM
                                (SELECT COMPANY_ID,ITEM_GROUP_ID,ITEM_GROUP_NAME,MOTHER_COMPANY FROM T_ITEM_GROUP WHERE ITEM_GROUP_ID='" + group + "') T1, (SELECT COMPANY_ID,COMPANY_NICK_NAME FROM T_COMPANY) T2 WHERE T1.COMPANY_ID=T2.COMPANY_ID";
                }

                OracleCommand cmdP = new OracleCommand(qrGroup, con);
                OracleDataAdapter daP = new OracleDataAdapter(cmdP);
                DataSet dsP = new DataSet();
                daP.Fill(dsP);
                int c = dsP.Tables[0].Rows.Count;
                if (c > 0 && dsP.Tables[0].Rows[0][0].ToString() != "")
                {
                    for (int i = 0; i < c; i++)
                    {
                        string companyName = dsP.Tables[0].Rows[i]["COMPANY_NICK_NAME"].ToString();
                        string groupId = dsP.Tables[0].Rows[i]["ITEM_GROUP_ID"].ToString();
                        string groupName = dsP.Tables[0].Rows[i]["ITEM_GROUP_NAME"].ToString();

                        string totalSR = "0";
                        string activeSR = "0";
                        string activeSRpercent = "0";
                        string inActiveSR = "0";
                        string inActiveSRpercent = "0";
                        string totalOutlet = "0";
                        string totalVisitedOutlet = "0";
                        string totalVisitedOutletPercent = "0";
                        string totalPendingOutlet = "0";
                        string totalPendingOutletPercent = "0";
                        string avgVisitedOutlet = "0";
                        string successfulCall = "0";
                        string nonSuccessfulCall = "0";
                        string memoPercent = "0";
                        string LPC = "0";
                        string totalAmount = "0";
                        string avgAmount = "0";


                        string qrSR = @"SELECT COUNT(*) TOTAL_SR FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "'";
                        OracleCommand cmdsr = new OracleCommand(qrSR, con);
                        OracleDataAdapter dasr = new OracleDataAdapter(cmdsr);
                        DataSet dssr = new DataSet();
                        dasr.Fill(dssr);
                        int sr = dssr.Tables[0].Rows.Count;
                        if (sr > 0 && dssr.Tables[0].Rows[0][0].ToString() != "0")
                        {
                            totalSR = dssr.Tables[0].Rows[0][0].ToString();


                            string qrActiveSR = @"SELECT DISTINCT SR_ID FROM T_ORDER_DETAIL
                                                WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy')";
                            OracleCommand cmdASR = new OracleCommand(qrActiveSR, con);
                            OracleDataAdapter daASR = new OracleDataAdapter(cmdASR);
                            DataSet dsASR = new DataSet();
                            daASR.Fill(dsASR);
                            int asr = dsASR.Tables[0].Rows.Count;
                            if (asr > 0 && dsASR.Tables[0].Rows[0][0].ToString() != "")
                            {
                                activeSR = asr.ToString();
                                activeSRpercent = DisplayPercentage((double)Convert.ToDouble(activeSR) / Convert.ToDouble(totalSR)).ToString();

                                inActiveSR = (Convert.ToInt32(totalSR) - Convert.ToInt32(activeSR)).ToString();
                                inActiveSRpercent = DisplayPercentage((double)(Convert.ToDouble(inActiveSR) / Convert.ToDouble(totalSR))).ToString();

                                string qrTotalOlt = @"SELECT COUNT(*) TOTAL_OUTLET FROM T_OUTLET
                                                    WHERE STATUS='Y' AND SR_ID IN(SELECT SR_ID TOTAL_SR FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "')";
                                OracleCommand cmdOlt = new OracleCommand(qrTotalOlt, con);
                                OracleDataAdapter daOlt = new OracleDataAdapter(cmdOlt);
                                DataSet dsOlt = new DataSet();
                                daOlt.Fill(dsOlt);
                                int olt = dsOlt.Tables[0].Rows.Count;
                                if (olt > 0 && dsOlt.Tables[0].Rows[0][0].ToString() != "")
                                {
                                    totalOutlet = dsOlt.Tables[0].Rows[0][0].ToString();
                                }

                                string qrVstOlt = @"SELECT DISTINCT OUTLET_ID FROM T_ORDER_DETAIL
                                                    WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy') ";
                                qrVstOlt = qrVstOlt + @" UNION
                                                    SELECT DISTINCT OUTLET_ID FROM T_NON_PRODUCTIVE_SALES
                                                    WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy') ";

                                OracleCommand cmdvstOlt = new OracleCommand(qrVstOlt, con);
                                OracleDataAdapter davstOlt = new OracleDataAdapter(cmdvstOlt);
                                DataSet dsvstOlt = new DataSet();
                                davstOlt.Fill(dsvstOlt);
                                int vstolt = dsvstOlt.Tables[0].Rows.Count;
                                if (vstolt > 0 && dsvstOlt.Tables[0].Rows[0][0].ToString() != "")
                                {
                                    totalVisitedOutlet = vstolt.ToString();
                                    totalVisitedOutletPercent = DisplayPercentage((double)(Convert.ToDouble(totalVisitedOutlet) / Convert.ToDouble(totalOutlet))).ToString();

                                    totalPendingOutlet = (Convert.ToInt32(totalOutlet) - Convert.ToInt32(totalVisitedOutlet)).ToString();
                                    totalPendingOutletPercent = DisplayPercentage((double)(Convert.ToDouble(totalPendingOutlet) / Convert.ToDouble(totalOutlet))).ToString();
                                }

                                avgVisitedOutlet = (Convert.ToInt32(totalVisitedOutlet) / Convert.ToInt32(activeSR)).ToString();


                                string qrSuccessCall = @"SELECT COUNT(DISTINCT OUTLET_ID) VISITED_OLT FROM T_ORDER_DETAIL
                                                    WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy')";

                                OracleCommand cmdSc = new OracleCommand(qrSuccessCall, con);
                                OracleDataAdapter daSc = new OracleDataAdapter(cmdSc);
                                DataSet dsSc = new DataSet();
                                daSc.Fill(dsSc);
                                int vsSc = dsSc.Tables[0].Rows.Count;
                                if (vsSc > 0 && dsSc.Tables[0].Rows[0][0].ToString() != "")
                                {
                                    successfulCall = dsSc.Tables[0].Rows[0][0].ToString();
                                    nonSuccessfulCall = (Convert.ToInt32(totalVisitedOutlet) - Convert.ToInt32(successfulCall)).ToString();

                                    memoPercent = DisplayPercentage((double)(Convert.ToDouble(successfulCall) / Convert.ToDouble(totalVisitedOutlet))).ToString();

                                }

                                string qrLPC = @"SELECT TO_CHAR(COUNT(ITEM_ID)/COUNT(DISTINCT OUTLET_ID),'999999999.99') LPC FROM T_ORDER_DETAIL
                                                WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy')";
                                OracleCommand cmdLPC = new OracleCommand(qrLPC, con);
                                OracleDataAdapter daLPC = new OracleDataAdapter(cmdLPC);
                                DataSet dsLPC = new DataSet();
                                daLPC.Fill(dsLPC);
                                int lpc = dsLPC.Tables[0].Rows.Count;
                                if (lpc > 0 && dsLPC.Tables[0].Rows[0][0].ToString() != "")
                                {
                                    LPC = dsLPC.Tables[0].Rows[0]["LPC"].ToString();

                                }

                                string qrAmt = @"SELECT TO_CHAR(SUM((T1.ITEM_QTY+(T1.ITEM_CTN*T2.FACTOR))*T1.OUT_PRICE),'999999999.99') AMT FROM         
                                                (SELECT * FROM T_ORDER_DETAIL    
                                                WHERE ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y')) T1, (SELECT ITEM_ID,FACTOR FROM T_ITEM) T2 WHERE T1.ITEM_ID=T2.ITEM_ID";

                                OracleCommand cmdAmt = new OracleCommand(qrAmt, con);
                                OracleDataAdapter daAmt = new OracleDataAdapter(cmdAmt);
                                DataSet dsAmt = new DataSet();
                                daAmt.Fill(dsAmt);
                                int amt = dsAmt.Tables[0].Rows.Count;
                                if (amt > 0 && dsAmt.Tables[0].Rows[0][0].ToString() != "")
                                {
                                    totalAmount = dsAmt.Tables[0].Rows[0]["AMT"].ToString();
                                    avgAmount = (Convert.ToDouble(totalAmount) / Convert.ToInt32(activeSR)).ToString();
                                    totalAmount = String.Format("{0:0.00}", (Convert.ToDouble(totalAmount)/1000));  
                                }
                                
                                avgAmount = String.Format("{0:0.00}", (Convert.ToDouble(avgAmount)/1000));  

                                msg = msg + ";" + groupName + ";" + totalOutlet + ";" + totalVisitedOutlet + ";" + totalPendingOutlet + ";" + successfulCall + ";" + nonSuccessfulCall + ";" + companyName + ";" + totalSR + ";" + activeSR + ";" + activeSRpercent + ";" + inActiveSR + ";" + inActiveSRpercent + ";" + totalVisitedOutletPercent + ";" + memoPercent + ";" + LPC + ";" + totalAmount + ";" + avgAmount + ";" + avgVisitedOutlet + ";" + totalPendingOutletPercent;

                            }

                        }
                    }
                }
            }
            catch (Exception ex) { }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod]
    public static string GetNonProductivityReport(string group, string fdate, string tdate)
    {
        string msg = "";

        try
        {
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            try
            {
                string qrGroup = "";

                if (group == "All")
                {
                    qrGroup = @"SELECT T5.*,T6.* FROM
                                (SELECT T3.*,T4.SR_NAME,T4.MOBILE_NO FROM
                                (SELECT T1.*,T2.OUTLET_NAME,T2.OUTLET_ADDRESS,T2.MOBILE_NUMBER FROM
                                (SELECT * FROM T_NON_PRODUCTIVE_SALES WHERE ENTRY_DATE BETWEEN TO_DATE('" + fdate.Trim() + "','DD/MM/YYYY') AND TO_DATE('" + tdate.Trim() + "','DD/MM/YYYY')) T1, ";
                    qrGroup = qrGroup + @"(SELECT OUTLET_ID,OUTLET_NAME,OUTLET_ADDRESS,MOBILE_NUMBER FROM T_OUTLET WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString() + "%') T2 ";
                    qrGroup = qrGroup + @"WHERE T1.OUTLET_ID=T2.OUTLET_ID) T3,
                                (SELECT SR_ID,SR_NAME,MOBILE_NO FROM T_SR_INFO) T4
                                WHERE T3.SR_ID=T4.SR_ID) T5,
                                (SELECT TT3.*,TT4.DIVISION_NAME FROM
                                (SELECT TT1.ROUTE_ID,TT1.ROUTE_NAME,TT1.DIVISION_ID,TT2.ZONE_NAME FROM ";
                    qrGroup = qrGroup + @"(SELECT * FROM T_ROUTE WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString() + "%') TT1, ";
                    qrGroup = qrGroup + @"(SELECT * FROM T_ZONE WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString() + "%') TT2 ";
                    qrGroup = qrGroup + @"WHERE TT1.ZONE_ID=TT2.ZONE_ID) TT3,  
                                SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString() + "%') TT4 WHERE TT3.DIVISION_ID=TT4.DIVISION_ID) T6 WHERE T5.ROUTE_ID=T6.ROUTE_ID";
                }
                else
                {
                    qrGroup = @"SELECT T5.*,T6.* FROM
                                                    (SELECT T3.*,T4.SR_NAME,T4.MOBILE_NO FROM
                                                    (SELECT T1.*,T2.OUTLET_NAME,T2.OUTLET_ADDRESS,T2.MOBILE_NUMBER FROM
                                                    (SELECT * FROM T_NON_PRODUCTIVE_SALES WHERE ENTRY_DATE BETWEEN TO_DATE('" + fdate.Trim() + "','DD/MM/YYYY') AND TO_DATE('" + tdate.Trim() + "','DD/MM/YYYY')) T1, ";
                                        qrGroup = qrGroup + @"(SELECT OUTLET_ID,OUTLET_NAME,OUTLET_ADDRESS,MOBILE_NUMBER FROM T_OUTLET WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString() + "%') T2 ";
                                        qrGroup = qrGroup + @"WHERE T1.OUTLET_ID=T2.OUTLET_ID) T3, 
                                                    (SELECT SR_ID,SR_NAME,MOBILE_NO FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + group + "') T4 ";
                                        qrGroup = qrGroup + @"WHERE T3.SR_ID=T4.SR_ID) T5,
                                                    (SELECT TT3.*,TT4.DIVISION_NAME FROM
                                                    (SELECT TT1.ROUTE_ID,TT1.ROUTE_NAME,TT1.DIVISION_ID,TT2.ZONE_NAME FROM ";
                                        qrGroup = qrGroup + @"(SELECT * FROM T_ROUTE WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString() + "%') TT1, ";
                                        qrGroup = qrGroup + @"(SELECT * FROM T_ZONE WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString() + "%') TT2 ";
                                        qrGroup = qrGroup + @"WHERE TT1.ZONE_ID=TT2.ZONE_ID) TT3,
                                                    (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString() + "%') TT4 WHERE TT3.DIVISION_ID=TT4.DIVISION_ID) T6 WHERE T5.ROUTE_ID=T6.ROUTE_ID";
                }

                OracleCommand cmdP = new OracleCommand(qrGroup, con);
                OracleDataAdapter daP = new OracleDataAdapter(cmdP);
                DataSet dsP = new DataSet();
                daP.Fill(dsP);
                int c = dsP.Tables[0].Rows.Count;
                if (c > 0 && dsP.Tables[0].Rows[0][0].ToString() != "")
                {
                    for (int i = 0; i < c; i++)
                    {
                        string ENTRY_DATE = Convert.ToDateTime(dsP.Tables[0].Rows[i]["ENTRY_DATE"].ToString()).ToShortDateString();
                        string SR_ID = dsP.Tables[0].Rows[i]["SR_ID"].ToString();
                        string SR_NAME = dsP.Tables[0].Rows[i]["SR_NAME"].ToString();
                        string SR_PHONE = dsP.Tables[0].Rows[i]["MOBILE_NO"].ToString();

                        string OUTLET_NAME = dsP.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                        string OLT_PHONE = dsP.Tables[0].Rows[i]["MOBILE_NUMBER"].ToString();
                        string OUTLET_ADDRESS = dsP.Tables[0].Rows[i]["OUTLET_ADDRESS"].ToString();
                        string REASON = dsP.Tables[0].Rows[i]["REASON"].ToString();
                        string ROUTE_NAME = dsP.Tables[0].Rows[i]["ROUTE_NAME"].ToString();
                        string ZONE_NAME = dsP.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                        string DIVISION_NAME = dsP.Tables[0].Rows[i]["DIVISION_NAME"].ToString();

                        msg = msg + ";" + ENTRY_DATE + ";" + SR_ID + ";" + SR_NAME + ";" + SR_PHONE + ";" + OUTLET_NAME + ";" + OLT_PHONE + ";" + OUTLET_ADDRESS + ";" + REASON + ";" + ROUTE_NAME + ";" + ZONE_NAME + ";" + DIVISION_NAME;
                    }
                }
            }
            catch (Exception ex) { }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        catch (Exception ex) { }

        return msg;
    }

    
    [WebMethod]
    public static string GetDashboard(string companyName)
    {
        string marqueValue = "";
        string marque = "<div style='width:98%;height:36px;margin-top: 5px;background-color:#2196F3;text-align:center;color:#fff;font-size:25px;padding-top:2px;line-height:33px;margin-top:10px;margin-left:1%;'><marquee direction='left'>";
        string msg = "<div style='width:98%;height:36px;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:25px;padding-top:10px;margin-top:10px;margin-left:1%;'><span>Sales Information</span>&nbsp;&nbsp;<span><button type='button' id='btnRefresh' style='background-color: #ff9800;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 2px 5px;text-align: center;vertical-align: middle;white-space: nowrap;'>Refresh</button></span></div>";
        string msg2 = "<div style='width:98%;height:auto;margin-top: 2px;border:1px solid #eceeef;padding-top:0px;margin-left:1%;'>" +
                      "<table style='width:99.5%;padding-top:10px;padding-bottom: 10px;padding-left:0%'>";

        string tblHos = "<table class='table-striped table-bordered table-hover' style='border-collapse: collapse;width:100%;'>";
        string rowHos = "<tr style='background-color:#ff9800;'><td style='border: 1px solid orange;padding: 5px;text-align: left;'>HOS ID</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>HOS NAME</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>MOBILE NO</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>GROUP NAME</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>TOTAL AMOUNT</td></tr>";

        string tblRM = "<table class='table-striped table-bordered table-hover' style='border-collapse: collapse;width:100%;'>";
        string rowRM = "<tr style='background-color:#ff9800;'><td style='border: 1px solid orange;padding: 5px;text-align: left;'>AGM/RM ID</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>AGM/RM NAME</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>MOBILE NO</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>GROUP NAME</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>Division Name</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>TOTAL AMOUNT</td></tr>";

        string tblTSM = "<table class='table-striped table-bordered table-hover' style='border-collapse: collapse;width:100%;'>";
        string rowTSM = "<tr style='background-color:#ff9800;'><td style='border: 1px solid orange;padding: 5px;text-align: left;'>TSM ID</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>TSM NAME</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>MOBILE NO</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>GROUP NAME</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>Zone Name</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>Division Name</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>TOTAL AMOUNT</td></tr>";
   

        DataTable dt = new DataTable();
        dt.Columns.Add("STAFF_ID", typeof(string));
        dt.Columns.Add("HOS_NAME", typeof(string));
        dt.Columns.Add("HOS_MOBILE", typeof(string));
        dt.Columns.Add("GROUP_NAME", typeof(string));
        dt.Columns.Add("TOTAL_AMOUNT", typeof(double));

        DataTable dt2 = new DataTable();
        dt2.Columns.Add("RM_ID", typeof(string));
        dt2.Columns.Add("RM_NAME", typeof(string));
        dt2.Columns.Add("MOBILE_NO", typeof(string));
        dt2.Columns.Add("GROUP_NAME", typeof(string));
        dt2.Columns.Add("DIVISION_NAME", typeof(string));
        dt2.Columns.Add("TOTAL_AMOUNT", typeof(double));

                
        DataTable dt3 = new DataTable();
        dt3.Columns.Add("TSM_ID", typeof(string));
        dt3.Columns.Add("TSM_NAME", typeof(string));
        dt3.Columns.Add("MOBILE_NO", typeof(string));
        dt3.Columns.Add("GROUP_NAME", typeof(string));
        dt3.Columns.Add("ZONE_NAME", typeof(string));
        dt3.Columns.Add("DIVISION_NAME", typeof(string));
        dt3.Columns.Add("TOTAL_AMOUNT", typeof(double));
        
        try
        {
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            int netTotalSR = 0;
            int netTotalActiveSR = 0;
            int netTotalInactiveSR = 0;
            int netTotalSRonLeave = 0;

            double netTotalAmount = 0;
            double netTotalAvgAmount = 0;

            int netTotalLine = 0;
            double netTotalStrikeRate = 0;

            int netTotalOutlet = 0;
            int netTotalVisitedOutlet = 0;
            int netTotalMemo = 0;
            int netSRonLeave = 0;
            string currentDate = "";

            try
            {
                string qrGroup = "";
 
                    qrGroup = @"SELECT T1.*,T2.COMPANY_NICK_NAME FROM
                                (SELECT COMPANY_ID,ITEM_GROUP_ID,ITEM_GROUP_NAME,MOTHER_COMPANY FROM T_ITEM_GROUP WHERE STATUS='Y' AND COMPANY_ID='" + HttpContext.Current.Session["company"].ToString() + "') T1, ";
                    qrGroup = qrGroup + @" (SELECT COMPANY_ID,COMPANY_NICK_NAME FROM T_COMPANY) T2
                                WHERE T1.COMPANY_ID=T2.COMPANY_ID";               

                OracleCommand cmdP = new OracleCommand(qrGroup, con);
                OracleDataAdapter daP = new OracleDataAdapter(cmdP);
                DataSet dsP = new DataSet();
                daP.Fill(dsP);
                int c = dsP.Tables[0].Rows.Count;
                if (c > 0 && dsP.Tables[0].Rows[0][0].ToString() != "")
                {
                    //string dt = DateTime.Now.ToShortDateString();
                    currentDate = HttpContext.Current.Session["todaysDate"].ToString();

                    string trRow1 = "<td><div style='width:100%;height:auto;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:22px;padding-top:7px;padding-bottom: 7px; margin-left:1%;'><span><button type='button' id='btnAllGroup' style='background-color: #ff9800;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 2px 5px;text-align: center;vertical-align: middle;white-space: nowrap;'>All Group</button></span></div></td>";
                    string trRow2 = "";

                    for (int i = 0; i < c; i++)
                    {                        
                        string groupId = dsP.Tables[0].Rows[i]["ITEM_GROUP_ID"].ToString();
                        string groupName = dsP.Tables[0].Rows[i]["ITEM_GROUP_NAME"].ToString();
                                                 
                        marqueValue = marqueValue + "&nbsp;" + groupName;

                        trRow1 = trRow1 + "<td><div style='width:100%;height:auto;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:22px;padding-top:7px;padding-bottom: 7px; margin-left:1%;'><span><button type='button' class='btn btn-xs btn-success groupid' id='" + groupId + "' style='background-color: #ff9800;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 2px 5px;text-align: center;vertical-align: middle;white-space: nowrap;'>" + groupName + "</button></span></div></td>";

                        string totalSR = "0";
                        string activeSR = "0";
                        string activeSRpercent = "0";
                        string inActiveSR = "0";
                        string inActiveSRpercent = "0";
                        string totalOutlet = "0";
                        string totalVisitedOutlet = "0";
                        string totalVisitedOutletPercent = "0";
                        string totalPendingOutlet = "0";
                        string totalPendingOutletPercent = "0";
                        string avgVisitedOutlet = "0";
                        string successfulCall = "0";
                        string nonSuccessfulCall = "0";
                        string memoPercent = "0";
                        string LPC = "0";
                        string totalAmount = "0";
                        string avgAmount = "0";
                        string avgOltOrd = "0";
                        string srOnLeave = "0";
                        string strk = "0%";


                        string ordRow = "";
//                        string qrOrd = @"SELECT T2.ITEM_SHORT_NAME FROM
//                                        (SELECT ITEM_ID,SUM((ITEM_QTY*OUT_PRICE) + (ITEM_CTN*OUT_PRICE)) TOTAL_AMT FROM T_ORDER_DETAIL
//                                        WHERE ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') AND SR_ID IN(SELECT DISTINCT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y') GROUP BY ITEM_ID ORDER BY TOTAL_AMT ASC) T1,";
//                        qrOrd = qrOrd + @"(SELECT ITEM_ID,ITEM_SHORT_NAME FROM T_ITEM) T2
//                                        WHERE T1.ITEM_ID=T2.ITEM_ID";

                        string qrOrd = @"SELECT T2.CLASS_ID,T2.CLASS_NAME,SUM(T1.TOTAL_AMT) TOTAL_AMOUNT FROM
                                     (SELECT ITEM_ID,SUM((ITEM_QTY*OUT_PRICE) + (ITEM_CTN*OUT_PRICE)) TOTAL_AMT FROM T_ORDER_DETAIL
                                     WHERE ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') AND SR_ID IN(SELECT DISTINCT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y') " +
                                     @"GROUP BY ITEM_ID ORDER BY TOTAL_AMT ASC) T1, 
                                     (SELECT tt1.ITEM_ID,tt1.ITEM_SHORT_NAME,tt2.CLASS_ID,tt2.CLASS_NAME FROM                                        
                                     (SELECT ITEM_ID,ITEM_SHORT_NAME,ITEM_CLASS FROM T_ITEM) tt1,
                                     (SELECT CLASS_ID,CLASS_NAME FROM T_ITEM_CLASS) tt2 
                                     WHERE tt1.ITEM_CLASS=tt2.CLASS_ID) T2
                                     WHERE T1.ITEM_ID=T2.ITEM_ID
                                     GROUP BY CLASS_ID,CLASS_NAME
                                     ORDER BY TOTAL_AMOUNT ASC";

                        OracleCommand cmdOrd = new OracleCommand(qrOrd, con);
                        OracleDataAdapter daOrd = new OracleDataAdapter(cmdOrd);
                        DataSet dsOrd = new DataSet();
                        daOrd.Fill(dsOrd);
                        int ord = dsOrd.Tables[0].Rows.Count;
                        if (ord > 0 && dsOrd.Tables[0].Rows[0][0].ToString() != "")
                        {
                            int slno = 1;
                            for (int r = 0; r < ord; r++)
                            {
                                string className = dsOrd.Tables[0].Rows[r]["CLASS_NAME"].ToString();
                                ordRow = ordRow + "<tr><td>" + slno.ToString() + ". <a href='' style='text-decoration:none;'>" + className + "</a></td></tr>";
                                slno++;
                            }
                        }

                        string tblOrd = "<table class='table-striped table-bordered table-hover'>" + ordRow + "</table>";


                        string qrSR = @"SELECT COUNT(*) TOTAL_SR FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y'";
                        OracleCommand cmdsr = new OracleCommand(qrSR, con);
                        OracleDataAdapter dasr = new OracleDataAdapter(cmdsr);
                        DataSet dssr = new DataSet();
                        dasr.Fill(dssr);
                        int sr = dssr.Tables[0].Rows.Count;
                        if (sr > 0 && dssr.Tables[0].Rows[0][0].ToString() != "0")
                        {
                            totalSR = dssr.Tables[0].Rows[0][0].ToString();

                            netTotalSR = netTotalSR + Convert.ToInt32(totalSR);

                            string qrActiveSR = @"SELECT DISTINCT SR_ID FROM T_ORDER_DETAIL
                                                WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y') AND ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";
                            OracleCommand cmdASR = new OracleCommand(qrActiveSR, con);
                            OracleDataAdapter daASR = new OracleDataAdapter(cmdASR);
                            DataSet dsASR = new DataSet();
                            daASR.Fill(dsASR);
                            int asr = dsASR.Tables[0].Rows.Count;
                            if (asr > 0 && dsASR.Tables[0].Rows[0][0].ToString() != "")
                            {
                                netTotalActiveSR = netTotalActiveSR + asr;

                                activeSR = asr.ToString();
                                activeSRpercent = DisplayPercentage((double)Convert.ToDouble(activeSR) / Convert.ToDouble(totalSR)).ToString();

                                inActiveSR = (Convert.ToInt32(totalSR) - Convert.ToInt32(activeSR)).ToString();
                                inActiveSRpercent = DisplayPercentage((double)(Convert.ToDouble(inActiveSR) / Convert.ToDouble(totalSR))).ToString();
                                netTotalInactiveSR = netTotalInactiveSR + Convert.ToInt32(inActiveSR);
                            }
                            else
                            {
                                inActiveSR = totalSR.ToString();
                                inActiveSRpercent = "100%";                                
                            }

                            string qrTotalOlt = @"SELECT COUNT(*) TOTAL_OUTLET FROM T_OUTLET
                                                    WHERE STATUS='Y' AND SR_ID IN(SELECT SR_ID TOTAL_SR FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y')";
                            OracleCommand cmdOlt = new OracleCommand(qrTotalOlt, con);
                            OracleDataAdapter daOlt = new OracleDataAdapter(cmdOlt);
                            DataSet dsOlt = new DataSet();
                            daOlt.Fill(dsOlt);
                            int olt = dsOlt.Tables[0].Rows.Count;
                            if (olt > 0 && dsOlt.Tables[0].Rows[0][0].ToString() != "0")
                            {
                                totalOutlet = dsOlt.Tables[0].Rows[0][0].ToString();

                                netTotalOutlet = netTotalOutlet + Convert.ToInt32(totalOutlet);
                            }

                            string qrVstOlt = @"SELECT DISTINCT OUTLET_ID FROM T_ORDER_DETAIL
                                                    WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y') AND ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";
                            qrVstOlt = qrVstOlt + @" UNION
                                                    SELECT DISTINCT OUTLET_ID FROM T_NON_PRODUCTIVE_SALES
                                                    WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y') AND ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";

                            OracleCommand cmdvstOlt = new OracleCommand(qrVstOlt, con);
                            OracleDataAdapter davstOlt = new OracleDataAdapter(cmdvstOlt);
                            DataSet dsvstOlt = new DataSet();
                            davstOlt.Fill(dsvstOlt);
                            int vstolt = dsvstOlt.Tables[0].Rows.Count;
                            if (vstolt > 0 && dsvstOlt.Tables[0].Rows[0][0].ToString() != "")
                            {
                                netTotalVisitedOutlet = netTotalVisitedOutlet + vstolt;

                                totalVisitedOutlet = vstolt.ToString();
                                totalVisitedOutletPercent = DisplayPercentage((double)(Convert.ToDouble(totalVisitedOutlet) / Convert.ToDouble(totalOutlet))).ToString();

                                totalPendingOutlet = (Convert.ToInt32(totalOutlet) - Convert.ToInt32(totalVisitedOutlet)).ToString();
                                totalPendingOutletPercent = DisplayPercentage((double)(Convert.ToDouble(totalPendingOutlet) / Convert.ToDouble(totalOutlet))).ToString();

                                avgVisitedOutlet = (Convert.ToInt32(totalVisitedOutlet) / Convert.ToInt32(activeSR)).ToString();
                            }


                            string qrSuccessCall = @"SELECT COUNT(DISTINCT OUTLET_ID) VISITED_OLT FROM T_ORDER_DETAIL
                                                    WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y') AND ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";

                            OracleCommand cmdSc = new OracleCommand(qrSuccessCall, con);
                            OracleDataAdapter daSc = new OracleDataAdapter(cmdSc);
                            DataSet dsSc = new DataSet();
                            daSc.Fill(dsSc);
                            int vsSc = dsSc.Tables[0].Rows.Count;
                            if (vsSc > 0 && dsSc.Tables[0].Rows[0][0].ToString() != "0")
                            {
                                successfulCall = dsSc.Tables[0].Rows[0][0].ToString();
                                nonSuccessfulCall = (Convert.ToInt32(totalVisitedOutlet) - Convert.ToInt32(successfulCall)).ToString();

                                memoPercent = DisplayPercentage((double)(Convert.ToDouble(successfulCall) / Convert.ToDouble(totalVisitedOutlet))).ToString();

                                netTotalMemo = netTotalMemo + Convert.ToInt32(successfulCall);
                            }

                            string qrLPC = @"SELECT COUNT(ITEM_ID) LPC FROM T_ORDER_DETAIL
                                                WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y') AND ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";
                            OracleCommand cmdLPC = new OracleCommand(qrLPC, con);
                            OracleDataAdapter daLPC = new OracleDataAdapter(cmdLPC);
                            DataSet dsLPC = new DataSet();
                            daLPC.Fill(dsLPC);
                            int lpc = dsLPC.Tables[0].Rows.Count;
                            if (lpc > 0 && dsLPC.Tables[0].Rows[0][0].ToString() != "0")
                            {
                                string totalLine = dsLPC.Tables[0].Rows[0]["LPC"].ToString();

                                string slpc = (Convert.ToDouble(totalLine) / Convert.ToInt32(successfulCall)).ToString();
                                LPC = String.Format("{0:0.00}", (Convert.ToDouble(slpc)));

                                netTotalLine = netTotalLine + Convert.ToInt32(totalLine);
                            }

                           

                            string qrAmt = @"SELECT TO_CHAR(SUM((T1.ITEM_QTY+(T1.ITEM_CTN*T2.FACTOR))*T1.OUT_PRICE),'999999999.99') AMT FROM         
                                                (SELECT * FROM T_ORDER_DETAIL    
                                                WHERE ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y')) T1, (SELECT ITEM_ID,FACTOR FROM T_ITEM) T2 WHERE T1.ITEM_ID=T2.ITEM_ID";

                            OracleCommand cmdAmt = new OracleCommand(qrAmt, con);
                            OracleDataAdapter daAmt = new OracleDataAdapter(cmdAmt);
                            DataSet dsAmt = new DataSet();
                            daAmt.Fill(dsAmt);
                            int amt = dsAmt.Tables[0].Rows.Count;
                            if (amt > 0 && dsAmt.Tables[0].Rows[0][0].ToString() != "")
                            {
                                totalAmount = dsAmt.Tables[0].Rows[0]["AMT"].ToString();
                                totalAmount = String.Format("{0:0.00}", (Convert.ToDouble(totalAmount)/1000));

                                marqueValue = marqueValue + ":&nbsp;" + totalAmount + "/-&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

                                netTotalAmount = netTotalAmount + (Convert.ToDouble(totalAmount));

                                avgAmount = (Convert.ToDouble(totalAmount) / Convert.ToInt32(activeSR)).ToString();
                                avgAmount = String.Format("{0:0.00}", (Convert.ToDouble(avgAmount)));



                                //boss

                                string GROUP_NAME = "";
                                string HOS_NAME = "";
                                string TOTAL_AMT = "0";
                                string STAFF_ID = "";
                                string HOS_MOBILE = "";                               
                                

                                string qrHos = @"SELECT T1.STAFF_ID,T1.HOS_NAME,T1.MOBILE_NO,T2.ITEM_GROUP_ID,T1.ITEM_GROUP FROM
                                                (SELECT STAFF_ID,HOS_NAME,MOBILE_NO,ITEM_GROUP FROM T_HOS)T1,
                                                (SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y') T2 WHERE T1.ITEM_GROUP=T2.ITEM_GROUP_NAME";
                                OracleCommand cmdd = new OracleCommand(qrHos, con);
                                OracleDataAdapter daa = new OracleDataAdapter(cmdd);
                                DataSet dss = new DataSet();
                                daa.Fill(dss);
                                int s = dss.Tables[0].Rows.Count;

                                if (s > 0)
                                {                                  
                                    GROUP_NAME = dss.Tables[0].Rows[0]["ITEM_GROUP"].ToString();
                                    HOS_NAME = dss.Tables[0].Rows[0]["HOS_NAME"].ToString();
                                    TOTAL_AMT = totalAmount; 
                                    STAFF_ID = dss.Tables[0].Rows[0]["STAFF_ID"].ToString();
                                    HOS_MOBILE = dss.Tables[0].Rows[0]["MOBILE_NO"].ToString();

                                    dt.Rows.Add(STAFF_ID, HOS_NAME, HOS_MOBILE, GROUP_NAME, Convert.ToDouble(TOTAL_AMT));
                                    
                                }


                                string qrRM = @"SELECT T1.*,T2.DIVISION_NAME FROM
                                              (SELECT RM_ID,RM_NAME,MOBILE_NO,DIVISION_ID  FROM T_AGM_RM WHERE HOS_ID='" + STAFF_ID + "') T1," +
                                              @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T2
                                              WHERE T1.DIVISION_ID=T2.DIVISION_ID";
                                OracleCommand cmdRM = new OracleCommand(qrRM, con);
                                OracleDataAdapter daRM = new OracleDataAdapter(cmdRM);
                                DataSet dsRM = new DataSet();
                                daRM.Fill(dsRM);
                                int rm = dsRM.Tables[0].Rows.Count;
                                if (rm > 0)
                                {
                                    for (int q = 0; q < rm; q++)
                                    {
                                        string RM_ID = dsRM.Tables[0].Rows[q]["RM_ID"].ToString();
                                        string RM_NAME = dsRM.Tables[0].Rows[q]["RM_NAME"].ToString();
                                        string MOBILE_NO = dsRM.Tables[0].Rows[q]["MOBILE_NO"].ToString();
                                        string DIVISION_NAME = dsRM.Tables[0].Rows[q]["DIVISION_NAME"].ToString();
                                        string T_AMOUNT = "0";

                                        string qrAmnt = @"SELECT SUM(T3.TOTAL_AMT) TOTAL_AMOUNT FROM
                                            (SELECT T1.SR_ID, (SUM(((T1.ITEM_CTN*T2.FACTOR)+T1.ITEM_QTY)*T1.OUT_PRICE)/1000) TOTAL_AMT FROM
                                            (SELECT SR_ID,ITEM_ID,ITEM_CTN,ITEM_QTY,OUT_PRICE FROM T_ORDER_DETAIL WHERE ENTRY_DATE=TO_DATE('" + currentDate.Trim() + "','DD/MM/YYYY')) T1, " +
                                                        @"(SELECT ITEM_ID,FACTOR FROM T_ITEM WHERE ITEM_GROUP='" + groupId + "') T2 " +
                                                        @"WHERE T1.ITEM_ID=T2.ITEM_ID GROUP BY T1.SR_ID) T3,
                                            (SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y' " +
                                                           @"AND SR_ID IN(SELECT SR_ID FROM T_TSM_SR WHERE TSM_ID IN(SELECT TSM_ID FROM T_TSM_ZM WHERE AGM_RM_ID='" + RM_ID + "' AND STATUS='Y')) " +
                                                        @") T4
                                            WHERE T3.SR_ID=T4.SR_ID";

                                        OracleCommand cmddd = new OracleCommand(qrAmnt, con);
                                        OracleDataAdapter daaa = new OracleDataAdapter(cmddd);
                                        DataSet dsss = new DataSet();
                                        daaa.Fill(dsss);
                                        int ss = dsss.Tables[0].Rows.Count;

                                        if (ss > 0 && dsss.Tables[0].Rows[0]["TOTAL_AMOUNT"].ToString().Length > 0)
                                        {
                                            T_AMOUNT = String.Format("{0:0.00}", Convert.ToDouble(dsss.Tables[0].Rows[0]["TOTAL_AMOUNT"].ToString()));
                                            dt2.Rows.Add(RM_ID, RM_NAME, MOBILE_NO, GROUP_NAME, DIVISION_NAME, Convert.ToDouble(T_AMOUNT));                                                                                        
                                        }                                       
                                         
                                    }
                                }

                                //tsm
                                string qrTSM = @"SELECT T3.*,T4.ZONE_NAME FROM
                                                (SELECT T1.*,T2.DIVISION_NAME FROM
                                                (SELECT TSM_ID,TSM_NAME,MOBILE_NO,DIVISION_ID,ZONE_ID,ITEM_GROUP FROM T_TSM_ZM WHERE ITEM_GROUP LIKE '%" + GROUP_NAME + "%') T1, " +
                                                @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T2
                                                WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                                                (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE) T4
                                                WHERE T3.ZONE_ID=T4.ZONE_ID";

                                OracleCommand cmdTSM = new OracleCommand(qrTSM, con);
                                OracleDataAdapter daTSM = new OracleDataAdapter(cmdTSM);
                                DataSet dsTSM = new DataSet();
                                daTSM.Fill(dsTSM);
                                int tsm = dsTSM.Tables[0].Rows.Count;
                                if (tsm > 0)
                                {
                                    for (int q = 0; q < tsm; q++)
                                    {
                                        string TSM_ID = dsTSM.Tables[0].Rows[q]["TSM_ID"].ToString();
                                        string TSM_NAME = dsTSM.Tables[0].Rows[q]["TSM_NAME"].ToString();
                                        string MOBILE_NO = dsTSM.Tables[0].Rows[q]["MOBILE_NO"].ToString();
                                        string DIVISION_NAME = dsTSM.Tables[0].Rows[q]["DIVISION_NAME"].ToString();
                                        string ZONE_NAME = dsTSM.Tables[0].Rows[q]["ZONE_NAME"].ToString();
                                        string T_AMOUNT = "0";

                                        string qrAmntS = @"SELECT SUM(T3.TOTAL_AMT) TOTAL_AMOUNT FROM
                                            (SELECT T1.SR_ID, (SUM(((T1.ITEM_CTN*T2.FACTOR)+T1.ITEM_QTY)*T1.OUT_PRICE)/1000) TOTAL_AMT FROM
                                            (SELECT SR_ID,ITEM_ID,ITEM_CTN,ITEM_QTY,OUT_PRICE FROM T_ORDER_DETAIL WHERE ENTRY_DATE=TO_DATE('" + currentDate.Trim() + "','DD/MM/YYYY')) T1, " +
                                                        @"(SELECT ITEM_ID,FACTOR FROM T_ITEM WHERE ITEM_GROUP='" + groupId + "') T2 " +
                                                        @"WHERE T1.ITEM_ID=T2.ITEM_ID GROUP BY T1.SR_ID) T3,
                                            (SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y' " +
                                                           @"AND SR_ID IN(SELECT SR_ID FROM T_TSM_SR WHERE TSM_ID = '" + TSM_ID + "')) T4 WHERE T3.SR_ID=T4.SR_ID";

                                        OracleCommand cmdT = new OracleCommand(qrAmntS, con);
                                        OracleDataAdapter daT = new OracleDataAdapter(cmdT);
                                        DataSet dsT = new DataSet();
                                        daT.Fill(dsT);
                                        int sT = dsT.Tables[0].Rows.Count;

                                        if (sT > 0 && dsT.Tables[0].Rows[0]["TOTAL_AMOUNT"].ToString().Length > 0)
                                        {
                                            T_AMOUNT = String.Format("{0:0.00}", Convert.ToDouble(dsT.Tables[0].Rows[0]["TOTAL_AMOUNT"].ToString()));
                                            dt3.Rows.Add(TSM_ID, TSM_NAME, MOBILE_NO, GROUP_NAME, ZONE_NAME, DIVISION_NAME, Convert.ToDouble(T_AMOUNT));
                                        }

                                    }
                                }

                            }

                            string qrLeave = "SELECT DISTINCT SR_ID FROM T_LEAVE WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y') AND FROM_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";
                            OracleCommand cmdL = new OracleCommand(qrLeave, con);
                            OracleDataAdapter daL = new OracleDataAdapter(cmdL);
                            DataSet dsL = new DataSet();
                            daL.Fill(dsL);
                            int L = dsL.Tables[0].Rows.Count;
                            if (L > 0 && dsL.Tables[0].Rows[0][0].ToString() != "")
                            {
                                srOnLeave = L.ToString();
                                netSRonLeave = netSRonLeave + Convert.ToInt32(srOnLeave);
                            }
                            
                            //--strike rate---------------------------
                            strk = "0%";
                            if (Convert.ToInt32(totalVisitedOutlet) > 0)
                            {
                                double sk = Convert.ToDouble(successfulCall) / Convert.ToDouble(totalVisitedOutlet);
                                strk = DisplayPercentage(sk);

                                if (Convert.ToDouble(netTotalAmount) > 0)
                                {
                                    avgOltOrd = (Convert.ToDouble(totalAmount) / Convert.ToDouble(successfulCall)).ToString();
                                    avgOltOrd = String.Format("{0:0.00}", (Convert.ToDouble(avgOltOrd)));
                                }
                            }
                            

                            if (Convert.ToInt32(activeSR) > 0)
                            {
                                inActiveSR = (Convert.ToInt32(inActiveSR) - Convert.ToInt32(srOnLeave)).ToString();

                                trRow2 = trRow2 + "<td><div style='width:100%;height:auto;margin-top: 5px;border:1px solid #8dc060;text-align:center;font-size:14px;padding-top:7px;padding-bottom: 7px; margin-left:1%;'>" +
                                                "<div style='background-color:#f06292;color:#fff;margin-left:2px;margin-right:2px;'><span>Act SR: " + activeSR + " of " + totalSR + "</span><br/>" +
                                                "<span>Inact SR: " + inActiveSR + " of " + totalSR + "</span><br/>" +
                                                "<span>SR-Leave: " + srOnLeave + " of " + totalSR + "</span></div><br/>" +

                                                "<div style='background-color:#2196f3;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Amt: " + totalAmount + "</span><br/>" +
                                                "<span>SR Avg.Ord: " + avgAmount + "</span><br/>" +
                                                "<span>Olt Avg.Ord: " + avgOltOrd + "</span></div><br/>" +

                                                "<div style='background-color:#ff9800;color:#fff;margin-left:2px;margin-right:2px;'><span>LPC: " + LPC + "</span><br/>" +
                                                "<span>Strk Rate: " + strk + "</span></span></div><br/>" +

                                                "<div style='background-color:#009688;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Outlet: " + totalOutlet + "</span><br/>" +
                                                "<span>Vstd Olt: " + totalVisitedOutlet + "</span><br/>" +
                                                "<span>Total memo: " + successfulCall.ToString() + "</span></div><br/>" +

                                                "<div style='background-color:#ff9800;color:#fff;margin-left:2px;margin-right:2px;'>"+ 
                                                "<span>Low to Top Ord Class</span></span></div>" +
                                                "<div style='color:black;font-size:11px;margin-left:2px;margin-right:2px;height:210px;overflow-y: scroll;min-height:210px;'>" +
                                                "<span>" + tblOrd + "</span><br/>" +                                                 

                                            "</div></td>";
                            }
                            else
                            {
                                netTotalInactiveSR = netTotalInactiveSR + Convert.ToInt32(totalSR);
                                inActiveSR = totalSR.ToString();
                                inActiveSRpercent = "100%";

                                trRow2 = trRow2 + "<td><div style='width:100%;height:auto;margin-top: 5px;border:1px solid #8dc060;text-align:center;font-size:14px;padding-top:7px;padding-bottom: 7px; margin-left:1%;'>" +
                                                "<div style='background-color:#f06292;color:#fff;margin-left:2px;margin-right:2px;'><span>Act SR: " + activeSR + " of " + totalSR + "</span><br/>" +
                                                "<span>Inact SR: " + inActiveSR + " of " + totalSR + "</span><br/>" +
                                                "<span>SR-Leave: " + srOnLeave + " of " + totalSR + "</span></div><br/>" +

                                                 "<div style='background-color:#2196f3;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Amt: " + totalAmount + "</span><br/>" +
                                                "<span>SR Avg.Ord: " + avgAmount + "</span><br/>" +
                                                "<span>Olt Avg.Ord: " + avgOltOrd + "</span></div><br/>" +

                                                 "<div style='background-color:#ff9800;color:#fff;margin-left:2px;margin-right:2px;'><span>LPC: " + LPC + "</span><br/>" +
                                                "<span>Strk Rate: " + strk + "</span></span></div><br/>" +

                                                 "<div style='background-color:#009688;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Outlet: " + totalOutlet + "</span><br/>" +
                                                "<span>Vstd Olt: " + totalVisitedOutlet + "</span><br/>" +
                                                "<span>Total memo: " + successfulCall.ToString() + "</span></div><br/>" +

                                                "<div style='background-color:#ff9800;color:#fff;margin-left:2px;margin-right:2px;'>" +
                                                "<span>Low to Top Ord Class</span></span></div>" +
                                                "<div style='color:black;font-size:11px;margin-left:2px;margin-right:2px;overflow-y: scroll;min-height:210px;'>" +
                                                "<span>" + tblOrd + "</span><br/>" +    

                                            "</div></td>";
                            }

                        }
                        else
                        {
                            trRow2 = trRow2 + "<td><div style='width:100%;height:auto;margin-top: 5px;border:1px solid #8dc060;text-align:center;font-size:14px;padding-top:7px;padding-bottom: 7px; margin-left:1%;'>" +
                                                "<div style='background-color:#f06292;color:#fff;margin-left:2px;margin-right:2px;'><span>Act SR: " + activeSR + " of " + totalSR + "</span><br/>" +
                                                "<span>Inact SR: " + inActiveSR + " of " + totalSR + "</span><br/>" +
                                                "<span>SR-Leave: " + srOnLeave + " of " + totalSR + "</span></div><br/>" +

                                                 "<div style='background-color:#2196f3;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Amt: " + totalAmount + "</span><br/>" +
                                                "<span>SR Avg.Ord: " + avgAmount + "</span><br/>" +
                                                "<span>Olt Avg.Ord: " + avgOltOrd + "</span></div><br/>" +

                                                 "<div style='background-color:#ff9800;color:#fff;margin-left:2px;margin-right:2px;'><span>LPC: " + LPC + "</span><br/>" +
                                                "<span>Strk Rate: " + strk + "</span></span></div><br/>" +

                                                "<div style='background-color:#009688;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Outlet: " + totalOutlet + "</span><br/>" +
                                                "<span>Vstd Olt: " + totalVisitedOutlet + "</span><br/>" +
                                                "<span>Total memo: " + successfulCall.ToString() + "</span></div><br/>" +

                                                "<div style='background-color:#ff9800;color:#fff;margin-left:2px;margin-right:2px;'>" +
                                                "<span>Low to Top Ord Class</span></span></div>" +
                                                "<div style='color:black;font-size:11px;margin-left:2px;margin-right:2px;height:173px;overflow-y: scroll;min-height:210px;'>" +
                                                "<span></span><br/>" +  
                                            "</div></td>";
                        }
                    }

                    //--LOW TO TOP ORDERED PRODUCTS


                    string ordRows = "";
//                    string qrOrds = @"SELECT T2.ITEM_SHORT_NAME FROM
//                                        (SELECT ITEM_ID,SUM((ITEM_QTY*OUT_PRICE) + (ITEM_CTN*OUT_PRICE)) TOTAL_AMT FROM T_ORDER_DETAIL
//                                        WHERE ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') AND SR_ID IN(SELECT DISTINCT SR_ID FROM T_SR_INFO WHERE ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString().Trim() + "' AND STATUS='Y') GROUP BY ITEM_ID ORDER BY TOTAL_AMT ASC) T1,";
//                    qrOrds = qrOrds + @"(SELECT ITEM_ID,ITEM_SHORT_NAME FROM T_ITEM) T2
//                                        WHERE T1.ITEM_ID=T2.ITEM_ID";

                    string qrOrds = @"SELECT T2.CLASS_ID,T2.CLASS_NAME,SUM(T1.TOTAL_AMT) TOTAL_AMOUNT FROM
                                     (SELECT ITEM_ID,SUM((ITEM_QTY*OUT_PRICE) + (ITEM_CTN*OUT_PRICE)) TOTAL_AMT FROM T_ORDER_DETAIL
                                     WHERE ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') AND SR_ID IN(SELECT DISTINCT SR_ID FROM T_SR_INFO WHERE ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString().Trim() + "' AND STATUS='Y') " +
                                     @"GROUP BY ITEM_ID ORDER BY TOTAL_AMT ASC) T1, 
                                     (SELECT tt1.ITEM_ID,tt1.ITEM_SHORT_NAME,tt2.CLASS_ID,tt2.CLASS_NAME FROM                                        
                                     (SELECT ITEM_ID,ITEM_SHORT_NAME,ITEM_CLASS FROM T_ITEM) tt1,
                                     (SELECT CLASS_ID,CLASS_NAME FROM T_ITEM_CLASS) tt2 
                                     WHERE tt1.ITEM_CLASS=tt2.CLASS_ID) T2
                                     WHERE T1.ITEM_ID=T2.ITEM_ID
                                     GROUP BY CLASS_ID,CLASS_NAME
                                     ORDER BY TOTAL_AMOUNT ASC";

                    OracleCommand cmdOrds = new OracleCommand(qrOrds, con);
                    OracleDataAdapter daOrds = new OracleDataAdapter(cmdOrds);
                    DataSet dsOrds = new DataSet();
                    daOrds.Fill(dsOrds);
                    int ords = dsOrds.Tables[0].Rows.Count;
                    if (ords > 0 && dsOrds.Tables[0].Rows[0][0].ToString() != "")
                    {
                        int slno = 1;
                        for (int r = 0; r < ords; r++)
                        {
                            string itemName = dsOrds.Tables[0].Rows[r]["CLASS_NAME"].ToString();
                            ordRows = ordRows + "<tr><td>" + slno.ToString() + ". <a href='' style='text-decoration:none;'>" + itemName + "</td></tr>";
                            slno++;
                        }
                    }

                    string tblOrds = "<table class='table-striped table-bordered table-hover'>" + ordRows + "</table>";

                    //---main khela here----------------------
                    string avgNetOltOrd = "0";
                    netTotalInactiveSR = Convert.ToInt32(netTotalSR) - Convert.ToInt32(netTotalActiveSR);

                    netTotalAvgAmount = (Convert.ToDouble(netTotalAmount) / Convert.ToInt32(netTotalActiveSR));
                    string netAvgAmt = String.Format("{0:0.00}", netTotalAvgAmount);

                    string netLPC = String.Format("{0:0.00}", (Convert.ToDouble((Convert.ToDouble(netTotalLine) / Convert.ToDouble(netTotalMemo)))));

                    string netStrk = "0%";
                    if (Convert.ToInt32(netTotalVisitedOutlet) > 0)
                    {
                        double sk = Convert.ToDouble(netTotalMemo) / Convert.ToDouble(netTotalVisitedOutlet);
                        netStrk = DisplayPercentage(sk);

                        if (Convert.ToDouble(netTotalAmount) > 0)
                        {
                            avgNetOltOrd = (Convert.ToDouble(netTotalAmount) / Convert.ToDouble(netTotalMemo)).ToString();
                            avgNetOltOrd = String.Format("{0:0.00}", (Convert.ToDouble(avgNetOltOrd)));
                        }
                    }

                    netTotalInactiveSR = netTotalInactiveSR - netSRonLeave;

                    string totalRow = "<td><div style='width:100%;height:auto;margin-top: 5px;border:1px solid #8dc060;text-align:center;font-size:14px;padding-top:7px;padding-bottom: 7px; margin-left:1%;'>" +
                                                "<div style='background-color:#f06292;color:#fff;margin-left:2px;margin-right:2px;'><span>Active SR: " + netTotalActiveSR + " of " + netTotalSR + "</span><br/>" +
                                                "<span>Inactive SR: " + netTotalInactiveSR + " of " + netTotalSR + "</span><br/>" +
                                                "<span>SR on Leave: " + netSRonLeave + " of " + netTotalSR + "</span></div><br/>" +

                                                "<div style='background-color:#2196f3;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Amt: " + netTotalAmount + "</span><br/>" +
                                                "<span>SR Avg. Ord: " + netAvgAmt + "</span><br/>" +
                                                "<span>Olt Avg. Ord: " + avgNetOltOrd + "</span></div><br/>" +

                                                 "<div style='background-color:#ff9800;color:#fff;margin-left:2px;margin-right:2px;'><span>LPC: " + netLPC + "</span><br/>" +
                                                "<span>Strike Rate: " + netStrk + "</span></span></div><br/>" +

                                                 "<div style='background-color:#009688;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Outlet: " + netTotalOutlet + "</span><br/>" +
                                                "<span>Visited Outlet: " + netTotalVisitedOutlet + "</span><br/>" +
                                                "<span>Total memo: " + netTotalMemo + "</span></div><br/>" +

                                                "<div style='background-color:#ff9800;color:#fff;margin-left:2px;margin-right:2px;'>" +
                                                "<span>Low to Top Ord Class</span></span></div>" +
                                                "<div style='color:black;font-size:11px;margin-left:2px;margin-right:2px;height:210px;overflow-y: scroll;'>" +
                                                "<span>" + tblOrds + "</span><br/>" +    

                                            "</div></td>";
                    trRow2 = totalRow + trRow2;
                    marque = marque + marqueValue + "</marquee></div>";
                    msg = msg + marque + msg2 + "<tr>" + trRow1 + "</tr><tr>" + trRow2 + "</tr></table></div>"; 


                }
            }
            catch (Exception ex) { }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        catch (Exception ex) { }

        if (dt.Rows.Count > 0)
        {             
            DataTable sortedTable = dt.AsEnumerable().OrderByDescending(r => r.Field<double>("TOTAL_AMOUNT")).CopyToDataTable();
            for (int h = 0; h < sortedTable.Rows.Count; h++)
            {
                string STAFF_ID = sortedTable.Rows[h]["STAFF_ID"].ToString();
                string HOS_NAME = sortedTable.Rows[h]["HOS_NAME"].ToString();
                string HOS_MOBILE = sortedTable.Rows[h]["HOS_MOBILE"].ToString();
                string GROUP_NAME = sortedTable.Rows[h]["GROUP_NAME"].ToString();
                string TOTAL_AMT = sortedTable.Rows[h]["TOTAL_AMOUNT"].ToString();
                rowHos = rowHos + "<tr><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + STAFF_ID + "</td><td style='border:1px solid #eceeef;padding: 5px;text-align: left;'>" + HOS_NAME + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + HOS_MOBILE + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + GROUP_NAME + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + TOTAL_AMT + "</td></tr>";
            }
        }
            
        tblHos = tblHos + rowHos + "</table>";

        if (dt2.Rows.Count > 0)
        {
            DataTable sortedTable2 = dt2.AsEnumerable().OrderByDescending(r => r.Field<double>("TOTAL_AMOUNT")).CopyToDataTable();
            for (int q = 0; q < sortedTable2.Rows.Count; q++)
            {
                string RM_ID = sortedTable2.Rows[q]["RM_ID"].ToString();
                string RM_NAME = sortedTable2.Rows[q]["RM_NAME"].ToString();
                string MOBILE_NO = sortedTable2.Rows[q]["MOBILE_NO"].ToString();
                string GROUP_NAME = sortedTable2.Rows[q]["GROUP_NAME"].ToString();
                string TOTAL_AMT = sortedTable2.Rows[q]["TOTAL_AMOUNT"].ToString();
                string DIVISION_NAME = sortedTable2.Rows[q]["DIVISION_NAME"].ToString();
                rowRM = rowRM + "<tr><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + RM_ID + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + RM_NAME + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + MOBILE_NO + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + GROUP_NAME + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + DIVISION_NAME + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + TOTAL_AMT + "</td></tr>";
            }
        }
        tblRM = tblRM + rowRM + "</table>";

        if (dt3.Rows.Count > 0)
        {
            DataTable sortedTable3 = dt3.AsEnumerable().OrderByDescending(r => r.Field<double>("TOTAL_AMOUNT")).CopyToDataTable();
            for (int q = 0; q < sortedTable3.Rows.Count; q++)
            {
                string TSM_ID = sortedTable3.Rows[q]["TSM_ID"].ToString();
                string TSM_NAME = sortedTable3.Rows[q]["TSM_NAME"].ToString();
                string MOBILE_NO = sortedTable3.Rows[q]["MOBILE_NO"].ToString();
                string GROUP_NAME = sortedTable3.Rows[q]["GROUP_NAME"].ToString();
                string TOTAL_AMT = sortedTable3.Rows[q]["TOTAL_AMOUNT"].ToString();
                string DIVISION_NAME = sortedTable3.Rows[q]["DIVISION_NAME"].ToString();
                string ZONE_NAME = sortedTable3.Rows[q]["ZONE_NAME"].ToString();
                rowTSM = rowTSM + "<tr><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + TSM_ID + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + TSM_NAME + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + MOBILE_NO + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + GROUP_NAME + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + ZONE_NAME + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + DIVISION_NAME + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + TOTAL_AMT + "</td></tr>";
            }
        }
        tblTSM = tblTSM + rowTSM + "</table>";     


        HttpContext.Current.Session["tblHos"] = tblHos;
        HttpContext.Current.Session["tblRM"] = tblRM;
        HttpContext.Current.Session["tblTSM"] = tblTSM;

        return msg;
    }


    [WebMethod]
    public static string GetDivisionWiseDashboard(string groupIds)
    {
        string msg = "";
        try
        {
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            int netTotalSR = 0;
            int netTotalActiveSR = 0;
            int netTotalInactiveSR = 0;
            int netTotalSRonLeave = 0;

            double netTotalAmount = 0;
            double netTotalAvgAmount = 0;

            int netTotalLine = 0;
            double netTotalStrikeRate = 0;

            int netTotalOutlet = 0;
            int netTotalVisitedOutlet = 0;
            int netTotalMemo = 0;
            int netSRonLeave = 0;
            string currentDate = "";

            try
            {
                string qrGroup = "";

                qrGroup = @"SELECT T1.ITEM_GROUP,T2.* FROM
                            (SELECT DISTINCT DIVISION_NAME,ITEM_GROUP FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupIds + "' AND STATUS='Y') T1, ";
                qrGroup = qrGroup + @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T2
                            WHERE T1.DIVISION_NAME=T2.DIVISION_ID";

                OracleCommand cmdP = new OracleCommand(qrGroup, con);
                OracleDataAdapter daP = new OracleDataAdapter(cmdP);
                DataSet dsP = new DataSet();
                daP.Fill(dsP);
                int c = dsP.Tables[0].Rows.Count;
                if (c > 0 && dsP.Tables[0].Rows[0][0].ToString() != "")
                {
                    currentDate = HttpContext.Current.Session["todaysDate"].ToString();

                    string groupName = dsP.Tables[0].Rows[0]["ITEM_GROUP"].ToString();
                    msg = "<div style='width:98%;height:36px;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:25px;padding-top:10px;margin-top:10px;margin-left:1%;'><span><button type='button' id='btnBack' style='background-color: #ff9800;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 2px 5px;text-align: center;vertical-align: middle;white-space: nowrap;'>Back</button></span>&nbsp;&nbsp;<span>" + groupName + " Group Sales Information</span>&nbsp;&nbsp;<span><button type='button' id='btnRefresh' style='background-color: #ff9800;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 2px 5px;text-align: center;vertical-align: middle;white-space: nowrap;'>Refresh</button></span></div>" +
                               "<div style='width:98%;height:auto;margin-top: 2px;border:1px solid #8dc060;padding-top:10px;margin-left:1%;'>" +
                                  "<table style='width:99.5%;padding-top:10px;padding-bottom: 10px;padding-left:0%'>";


                    string trRow1 = "<td><div style='width:100%;height:auto;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:22px;padding-top:7px;padding-bottom: 7px; margin-left:1%;'><span><button type='button' id='btnAllDivision' style='background-color: #ff9800;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 2px 5px;text-align: center;vertical-align: middle;white-space: nowrap;'>All Division</button></span></div></td>";
                    string trRow2 = "";

                    for (int i = 0; i < c; i++)
                    {
                        string DIVISION_ID = dsP.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                        string DIVISION_NAME = dsP.Tables[0].Rows[i]["DIVISION_NAME"].ToString();

                        trRow1 = trRow1 + "<td><div style='width:100%;height:auto;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:22px;padding-top:7px;padding-bottom: 7px; margin-left:1%;'><span><button type='button' class='btn btn-xs btn-success divisionid' id='" + groupIds + ";" + DIVISION_ID + "' style='background-color: #ff9800;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 2px 5px;text-align: center;vertical-align: middle;white-space: nowrap;'>" + DIVISION_NAME + "</button></span></div></td>";

                        string totalSR = "0";
                        string activeSR = "0";
                        string activeSRpercent = "0";
                        string inActiveSR = "0";
                        string inActiveSRpercent = "0";
                        string totalOutlet = "0";
                        string totalVisitedOutlet = "0";
                        string totalVisitedOutletPercent = "0";
                        string totalPendingOutlet = "0";
                        string totalPendingOutletPercent = "0";
                        string avgVisitedOutlet = "0";
                        string successfulCall = "0";
                        string nonSuccessfulCall = "0";
                        string memoPercent = "0";
                        string LPC = "0";
                        string totalAmount = "0";
                        string avgAmount = "0";
                        string avgOltAmt = "0";
                        string srOnLeave = "0";


                        string qrSR = @"SELECT COUNT(*) TOTAL_SR FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupIds + "' AND DIVISION_NAME='" + DIVISION_ID + "' AND STATUS='Y'";
                        OracleCommand cmdsr = new OracleCommand(qrSR, con);
                        OracleDataAdapter dasr = new OracleDataAdapter(cmdsr);
                        DataSet dssr = new DataSet();
                        dasr.Fill(dssr);
                        int sr = dssr.Tables[0].Rows.Count;
                        if (sr > 0 && dssr.Tables[0].Rows[0][0].ToString() != "0")
                        {
                            totalSR = dssr.Tables[0].Rows[0][0].ToString();

                            netTotalSR = netTotalSR + Convert.ToInt32(totalSR);

                            string qrActiveSR = @"SELECT DISTINCT SR_ID FROM T_ORDER_DETAIL
                                                WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE DIVISION_NAME='" + DIVISION_ID + "' AND ITEM_GROUP_ID='" + groupIds + "' AND STATUS='Y') AND ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";
                            OracleCommand cmdASR = new OracleCommand(qrActiveSR, con);
                            OracleDataAdapter daASR = new OracleDataAdapter(cmdASR);
                            DataSet dsASR = new DataSet();
                            daASR.Fill(dsASR);
                            int asr = dsASR.Tables[0].Rows.Count;
                            if (asr > 0 && dsASR.Tables[0].Rows[0][0].ToString() != "")
                            {
                                netTotalActiveSR = netTotalActiveSR + asr;

                                activeSR = asr.ToString();
                                activeSRpercent = DisplayPercentage((double)Convert.ToDouble(activeSR) / Convert.ToDouble(totalSR)).ToString();

                                inActiveSR = (Convert.ToInt32(totalSR) - Convert.ToInt32(activeSR)).ToString();
                                inActiveSRpercent = DisplayPercentage((double)(Convert.ToDouble(inActiveSR) / Convert.ToDouble(totalSR))).ToString();
                                netTotalInactiveSR = netTotalInactiveSR + Convert.ToInt32(inActiveSR);
                            }
                            else
                            {
                                inActiveSR = totalSR.ToString();
                                inActiveSRpercent = "100%";
                            }

                            string qrTotalOlt = @"SELECT COUNT(*) TOTAL_OUTLET FROM T_OUTLET
                                                    WHERE STATUS='Y' AND SR_ID IN(SELECT SR_ID TOTAL_SR FROM T_SR_INFO WHERE DIVISION_NAME='" + DIVISION_ID + "' AND ITEM_GROUP_ID='" + groupIds + "' AND STATUS='Y')";
                            OracleCommand cmdOlt = new OracleCommand(qrTotalOlt, con);
                            OracleDataAdapter daOlt = new OracleDataAdapter(cmdOlt);
                            DataSet dsOlt = new DataSet();
                            daOlt.Fill(dsOlt);
                            int olt = dsOlt.Tables[0].Rows.Count;
                            if (olt > 0 && dsOlt.Tables[0].Rows[0][0].ToString() != "0")
                            {
                                totalOutlet = dsOlt.Tables[0].Rows[0][0].ToString();

                                netTotalOutlet = netTotalOutlet + Convert.ToInt32(totalOutlet);
                            }

                            string qrVstOlt = @"SELECT DISTINCT OUTLET_ID FROM T_ORDER_DETAIL
                                                    WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE DIVISION_NAME='" + DIVISION_ID + "' AND ITEM_GROUP_ID='" + groupIds + "' AND STATUS='Y') AND ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";
                            qrVstOlt = qrVstOlt + @" UNION
                                                    SELECT DISTINCT OUTLET_ID FROM T_NON_PRODUCTIVE_SALES
                                                    WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE DIVISION_NAME='" + DIVISION_ID + "' AND ITEM_GROUP_ID='" + groupIds + "' AND STATUS='Y') AND ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";

                            OracleCommand cmdvstOlt = new OracleCommand(qrVstOlt, con);
                            OracleDataAdapter davstOlt = new OracleDataAdapter(cmdvstOlt);
                            DataSet dsvstOlt = new DataSet();
                            davstOlt.Fill(dsvstOlt);
                            int vstolt = dsvstOlt.Tables[0].Rows.Count;
                            if (vstolt > 0 && dsvstOlt.Tables[0].Rows[0][0].ToString() != "")
                            {
                                netTotalVisitedOutlet = netTotalVisitedOutlet + vstolt;

                                totalVisitedOutlet = vstolt.ToString();
                                totalVisitedOutletPercent = DisplayPercentage((double)(Convert.ToDouble(totalVisitedOutlet) / Convert.ToDouble(totalOutlet))).ToString();

                                totalPendingOutlet = (Convert.ToInt32(totalOutlet) - Convert.ToInt32(totalVisitedOutlet)).ToString();
                                totalPendingOutletPercent = DisplayPercentage((double)(Convert.ToDouble(totalPendingOutlet) / Convert.ToDouble(totalOutlet))).ToString();

                                avgVisitedOutlet = (Convert.ToInt32(totalVisitedOutlet) / Convert.ToInt32(activeSR)).ToString();
                            }




                            string qrSuccessCall = @"SELECT COUNT(DISTINCT OUTLET_ID) VISITED_OLT FROM T_ORDER_DETAIL
                                                    WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE DIVISION_NAME='" + DIVISION_ID + "' AND ITEM_GROUP_ID='" + groupIds + "' AND STATUS='Y') AND ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";

                            OracleCommand cmdSc = new OracleCommand(qrSuccessCall, con);
                            OracleDataAdapter daSc = new OracleDataAdapter(cmdSc);
                            DataSet dsSc = new DataSet();
                            daSc.Fill(dsSc);
                            int vsSc = dsSc.Tables[0].Rows.Count;
                            if (vsSc > 0 && dsSc.Tables[0].Rows[0][0].ToString() != "0")
                            {
                                successfulCall = dsSc.Tables[0].Rows[0][0].ToString();
                                nonSuccessfulCall = (Convert.ToInt32(totalVisitedOutlet) - Convert.ToInt32(successfulCall)).ToString();

                                memoPercent = DisplayPercentage((double)(Convert.ToDouble(successfulCall) / Convert.ToDouble(totalVisitedOutlet))).ToString();

                                netTotalMemo = netTotalMemo + Convert.ToInt32(successfulCall);
                            }

                            string qrLPC = @"SELECT COUNT(ITEM_ID) LPC FROM T_ORDER_DETAIL
                                                WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE DIVISION_NAME='" + DIVISION_ID + "' AND ITEM_GROUP_ID='" + groupIds + "' AND STATUS='Y') AND ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";
                            OracleCommand cmdLPC = new OracleCommand(qrLPC, con);
                            OracleDataAdapter daLPC = new OracleDataAdapter(cmdLPC);
                            DataSet dsLPC = new DataSet();
                            daLPC.Fill(dsLPC);
                            int lpc = dsLPC.Tables[0].Rows.Count;
                            if (lpc > 0 && dsLPC.Tables[0].Rows[0][0].ToString() != "0")
                            {
                                string totalLine = dsLPC.Tables[0].Rows[0]["LPC"].ToString();

                                string slpc = (Convert.ToDouble(totalLine) / Convert.ToInt32(successfulCall)).ToString();
                                LPC = String.Format("{0:0.00}", (Convert.ToDouble(slpc)));

                                netTotalLine = netTotalLine + Convert.ToInt32(totalLine);
                            }



                            string qrAmt = @"SELECT TO_CHAR(SUM((T1.ITEM_QTY+(T1.ITEM_CTN*T2.FACTOR))*T1.OUT_PRICE),'999999999.99') AMT FROM         
                                                (SELECT * FROM T_ORDER_DETAIL    
                                                WHERE ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE DIVISION_NAME='" + DIVISION_ID + "' AND ITEM_GROUP_ID='" + groupIds + "' AND STATUS='Y')) T1, (SELECT ITEM_ID,FACTOR FROM T_ITEM) T2 WHERE T1.ITEM_ID=T2.ITEM_ID";

                            OracleCommand cmdAmt = new OracleCommand(qrAmt, con);
                            OracleDataAdapter daAmt = new OracleDataAdapter(cmdAmt);
                            DataSet dsAmt = new DataSet();
                            daAmt.Fill(dsAmt);
                            int amt = dsAmt.Tables[0].Rows.Count;
                            if (amt > 0 && dsAmt.Tables[0].Rows[0][0].ToString() != "")
                            {
                                totalAmount = dsAmt.Tables[0].Rows[0]["AMT"].ToString();
                                totalAmount = String.Format("{0:0.00}", (Convert.ToDouble(totalAmount) / 1000));

                                netTotalAmount = netTotalAmount + (Convert.ToDouble(totalAmount));

                                avgAmount = (Convert.ToDouble(totalAmount) / Convert.ToInt32(activeSR)).ToString();
                                avgAmount = String.Format("{0:0.00}", (Convert.ToDouble(avgAmount)));

                            }

                            string qrLeave = "SELECT DISTINCT SR_ID FROM T_LEAVE WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE DIVISION_NAME='" + DIVISION_ID + "' AND ITEM_GROUP_ID='" + groupIds + "' AND STATUS='Y') AND FROM_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";
                            OracleCommand cmdL = new OracleCommand(qrLeave, con);
                            OracleDataAdapter daL = new OracleDataAdapter(cmdL);
                            DataSet dsL = new DataSet();
                            daL.Fill(dsL);
                            int L = dsL.Tables[0].Rows.Count;
                            if (L > 0 && dsL.Tables[0].Rows[0][0].ToString() != "")
                            {
                                srOnLeave = L.ToString();
                                netSRonLeave = netSRonLeave + Convert.ToInt32(srOnLeave);
                            }

                            
                            //--strike rate---------------------------
                            string strk = "0%";
                            if (Convert.ToInt32(totalVisitedOutlet) > 0)
                            {
                                double sk = Convert.ToDouble(successfulCall) / Convert.ToDouble(totalVisitedOutlet);
                                strk = DisplayPercentage(sk);

                                if (Convert.ToDouble(totalAmount) > 0)
                                {
                                    avgOltAmt = (Convert.ToDouble(totalAmount) / Convert.ToDouble(successfulCall)).ToString();
                                    avgOltAmt = String.Format("{0:0.00}", (Convert.ToDouble(avgOltAmt)));
                                }
                            }

                            if (Convert.ToInt32(activeSR) > 0)
                            {
                                trRow2 = trRow2 + "<td><div style='width:100%;height:auto;margin-top: 5px;border:1px solid #8dc060;text-align:center;font-size:14px;padding-top:7px;padding-bottom: 7px; margin-left:1%;'>" +
                                                "<div style='background-color:#f06292;color:#fff;margin-left:2px;margin-right:2px;'><span>Act SR: " + activeSR + " of " + totalSR + "</span><br/>" +
                                                "<span>Inact SR: " + inActiveSR + " of " + totalSR + "</span><br/>" +
                                                "<span>SR on Leave: " + srOnLeave + " of " + totalSR + "</span></div><br/>" +

                                                "<div style='background-color:#2196f3;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Amt: " + totalAmount + "</span><br/>" +
                                                "<span>SR Avg.Ord: " + avgAmount + "</span><br/>" +
                                                "<span>Olt Avg.Ord: " + avgOltAmt + "</span></div><br/>" +

                                                "<div style='background-color:#ff9800;color:#fff;margin-left:2px;margin-right:2px;'><span>LPC: " + LPC + "</span><br/>" +
                                                "<span>Strk Rate: " + strk + "</span></span></div><br/>" +

                                                 "<div style='background-color:#009688;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Outlet: " + totalOutlet + "</span><br/>" +
                                                "<span>Vstd Olt: " + totalVisitedOutlet + "</span><br/>" +
                                                "<span>Total memo: " + successfulCall.ToString() + "</span></div><br/>" +

                                            "</div></td>";
                            }
                            else
                            {
                                netTotalInactiveSR = netTotalInactiveSR + Convert.ToInt32(totalSR);
                                inActiveSR = totalSR.ToString();
                                inActiveSRpercent = "100%";

                                trRow2 = trRow2 + "<td><div style='width:100%;height:auto;margin-top: 5px;border:1px solid #8dc060;text-align:center;font-size:14px;padding-top:7px;padding-bottom: 7px; margin-left:1%;'>" +
                                                "<div style='background-color:#f06292;color:#fff;margin-left:2px;margin-right:2px;'><span>Act SR: " + activeSR + " of " + totalSR + "</span><br/>" +
                                                "<span>Inact SR: " + inActiveSR + " of " + totalSR + "</span><br/>" +
                                                "<span>SR on Leave: " + srOnLeave + " of " + totalSR + "</span></div><br/>" +

                                                 "<div style='background-color:#2196f3;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Amt: " + totalAmount + "</span><br/>" +
                                                "<span>SR Avg.Ord: " + avgAmount + "</span><br/>" +
                                                "<span>Olt Avg.Ord: " + avgOltAmt + "</span></div><br/>" +

                                                "<div style='background-color:#ff9800;color:#fff;margin-left:2px;margin-right:2px;'><span>LPC: " + LPC + "</span><br/>" +
                                                "<span>Strk Rate: " + strk + "</span></span></div><br/>" +

                                                 "<div style='background-color:#009688;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Outlet: " + totalOutlet + "</span><br/>" +
                                                "<span>Vst Olt: " + totalVisitedOutlet + "</span><br/>" +
                                                "<span>Total memo: " + successfulCall.ToString() + "</span></div><br/>" +

                                            "</div></td>";
                            }

                        }
                    }

                    //---main khela here----------------------
                    netTotalInactiveSR = Convert.ToInt32(netTotalSR) - Convert.ToInt32(netTotalActiveSR);

                    netTotalAvgAmount = (Convert.ToDouble(netTotalAmount) / Convert.ToInt32(netTotalActiveSR));
                    string netAvgAmt = String.Format("{0:0.00}", netTotalAvgAmount);

                    string netLPC = String.Format("{0:0.00}", (Convert.ToDouble((Convert.ToDouble(netTotalLine) / Convert.ToDouble(netTotalMemo)))));

                    string avgNetOltOrd = "0";

                    string netStrk = "0%";
                    if (Convert.ToInt32(netTotalVisitedOutlet) > 0)
                    {
                        double sk = Convert.ToDouble(netTotalMemo) / Convert.ToDouble(netTotalVisitedOutlet);
                        netStrk = DisplayPercentage(sk);

                        if (Convert.ToDouble(netTotalAmount) > 0)
                        {
                            avgNetOltOrd = (Convert.ToDouble(netTotalAmount) / Convert.ToDouble(netTotalMemo)).ToString();
                            avgNetOltOrd = String.Format("{0:0.00}", (Convert.ToDouble(avgNetOltOrd)));
                        }

                    }

                    string totalRow = "<td><div style='width:100%;height:auto;margin-top: 5px;border:1px solid #8dc060;text-align:center;font-size:14px;padding-top:7px;padding-bottom: 7px; margin-left:1%;'>" +
                                                "<div style='background-color:#f06292;color:#fff;margin-left:2px;margin-right:2px;'><span>Active SR: " + netTotalActiveSR + " of " + netTotalSR + "</span><br/>" +
                                                "<span>Inactive SR: " + netTotalInactiveSR + " of " + netTotalSR + "</span><br/>" +
                                                "<span>SR on Leave: " + netSRonLeave + " of " + netTotalSR + "</span></div><br/>" +

                                                "<div style='background-color:#2196f3;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Amt: " + netTotalAmount + "</span><br/>" +
                                                "<span>SR Avg.Ord: " + netAvgAmt + "</span><br/>" +
                                                "<span>Outlet Avg.Ord: " + avgNetOltOrd + "</span></div><br/>" +

                                                "<div style='background-color:#ff9800;color:#fff;margin-left:2px;margin-right:2px;'><span>LPC: " + netLPC + "</span><br/>" +
                                                "<span>Strike Rate: " + netStrk + "</span></span></div><br/>" +

                                                 "<div style='background-color:#009688;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Outlet: " + netTotalOutlet + "</span><br/>" +
                                                "<span>Visited Outlet: " + netTotalVisitedOutlet + "</span><br/>" +
                                                "<span>Total memo: " + netTotalMemo + "</span></div><br/>" +

                                            "</div></td>";
                    trRow2 = totalRow + trRow2;

                    msg = msg + "<tr>" + trRow1 + "</tr><tr>" + trRow2 + "</tr></table></div>";


                }
            }
            catch (Exception ex) { }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        catch (Exception ex) { }

        return msg;
    }
    
    
    [WebMethod]
    public static string GetDivisionAndZoneWiseDashboard(string group, string division)
    {
        string msg = "";
        try
        {
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            int netTotalSR = 0;
            int netTotalActiveSR = 0;
            int netTotalInactiveSR = 0;
            int netTotalSRonLeave = 0;

            double netTotalAmount = 0;
            double netTotalAvgAmount = 0;

            int netTotalLine = 0;
            double netTotalStrikeRate = 0;

            int netTotalOutlet = 0;
            int netTotalVisitedOutlet = 0;
            int netTotalMemo = 0;
            int netSRonLeave = 0;
            string currentDate = "";

            try
            {
                string qrGroup = "";

                qrGroup = @"SELECT T3.*,T4.ZONE_NAME FROM
                            (SELECT T1.ITEM_GROUP,T1.DIST_ZONE,T2.* FROM
                            (SELECT DISTINCT DIVISION_NAME,ITEM_GROUP,DIST_ZONE FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + group + "' AND DIVISION_NAME='" + division + "' AND STATUS='Y') T1, ";
                qrGroup = qrGroup + @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T2
                            WHERE T1.DIVISION_NAME=T2.DIVISION_ID) T3, (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE WHERE DIVISION_ID='" + division + "') T4 WHERE T3.DIST_ZONE=T4.ZONE_ID";

                OracleCommand cmdP = new OracleCommand(qrGroup, con);
                OracleDataAdapter daP = new OracleDataAdapter(cmdP);
                DataSet dsP = new DataSet();
                daP.Fill(dsP);
                int c = dsP.Tables[0].Rows.Count;
                if (c > 0 && dsP.Tables[0].Rows[0][0].ToString() != "")
                {
                    currentDate = HttpContext.Current.Session["todaysDate"].ToString();

                    string groupName = dsP.Tables[0].Rows[0]["ITEM_GROUP"].ToString();

                    msg = "<div style='width:98%;height:36px;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:25px;padding-top:10px;margin-top:10px;margin-left:1%;'><span><button type='button' id='btnBack' style='background-color: #ff9800;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 2px 5px;text-align: center;vertical-align: middle;white-space: nowrap;'>Back</button></span>&nbsp;&nbsp;<span>" + groupName + " Group Sales Information</span>&nbsp;&nbsp;<span><button type='button' id='btnRefresh' style='background-color: #ff9800;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 2px 5px;text-align: center;vertical-align: middle;white-space: nowrap;'>Refresh</button></span></div>" +
                               "<div style='width:98%;height:auto;margin-top: 2px;border:1px solid #8dc060;padding-top:10px;margin-left:1%;'>" +
                                  "<table style='width:99.5%;padding-top:10px;padding-bottom: 10px;padding-left:0%'>";


                    string trRow1 = "<td><div style='width:100%;height:auto;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:22px;padding-top:7px;padding-bottom: 7px; margin-left:1%;'><span><button type='button' id='btnAllDivision' style='background-color: #ff9800;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 2px 5px;text-align: center;vertical-align: middle;white-space: nowrap;'>All Zone</button></span></div></td>";
                    string trRow2 = "";

                    for (int i = 0; i < c; i++)
                    {
                        string ZONE_ID = dsP.Tables[0].Rows[i]["DIST_ZONE"].ToString();
                        string ZONE_NAME = dsP.Tables[0].Rows[i]["ZONE_NAME"].ToString();

                        trRow1 = trRow1 + "<td><div style='width:100%;height:auto;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:22px;padding-top:7px;padding-bottom: 7px; margin-left:1%;'><span><button type='button' class='btn btn-xs btn-success zoneid' id='" + group + ";" + division + ";" + ZONE_ID + "' style='background-color: #ff9800;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 2px 5px;text-align: center;vertical-align: middle;white-space: nowrap;'>" + ZONE_NAME + "</button></span></div></td>";

                        string totalSR = "0";
                        string activeSR = "0";
                        string activeSRpercent = "0";
                        string inActiveSR = "0";
                        string inActiveSRpercent = "0";
                        string totalOutlet = "0";
                        string totalVisitedOutlet = "0";
                        string totalVisitedOutletPercent = "0";
                        string totalPendingOutlet = "0";
                        string totalPendingOutletPercent = "0";
                        string avgVisitedOutlet = "0";
                        string successfulCall = "0";
                        string nonSuccessfulCall = "0";
                        string memoPercent = "0";
                        string LPC = "0";
                        string totalAmount = "0";
                        string avgAmount = "0";
                        string avgOltAmt = "0";
                        string srOnLeave = "0";


                        string qrSR = @"SELECT COUNT(*) TOTAL_SR FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + group + "' AND DIVISION_NAME='" + division + "' AND DIST_ZONE='" + ZONE_ID + "' AND STATUS='Y'";
                        OracleCommand cmdsr = new OracleCommand(qrSR, con);
                        OracleDataAdapter dasr = new OracleDataAdapter(cmdsr);
                        DataSet dssr = new DataSet();
                        dasr.Fill(dssr);
                        int sr = dssr.Tables[0].Rows.Count;
                        if (sr > 0 && dssr.Tables[0].Rows[0][0].ToString() != "0")
                        {
                            totalSR = dssr.Tables[0].Rows[0][0].ToString();

                            netTotalSR = netTotalSR + Convert.ToInt32(totalSR);

                            string qrActiveSR = @"SELECT DISTINCT SR_ID FROM T_ORDER_DETAIL
                                                WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE DIVISION_NAME='" + division + "' AND DIST_ZONE='" + ZONE_ID + "' AND ITEM_GROUP_ID='" + group + "' AND STATUS='Y') AND ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";
                            OracleCommand cmdASR = new OracleCommand(qrActiveSR, con);
                            OracleDataAdapter daASR = new OracleDataAdapter(cmdASR);
                            DataSet dsASR = new DataSet();
                            daASR.Fill(dsASR);
                            int asr = dsASR.Tables[0].Rows.Count;
                            if (asr > 0 && dsASR.Tables[0].Rows[0][0].ToString() != "")
                            {
                                netTotalActiveSR = netTotalActiveSR + asr;

                                activeSR = asr.ToString();
                                activeSRpercent = DisplayPercentage((double)Convert.ToDouble(activeSR) / Convert.ToDouble(totalSR)).ToString();

                                inActiveSR = (Convert.ToInt32(totalSR) - Convert.ToInt32(activeSR)).ToString();
                                inActiveSRpercent = DisplayPercentage((double)(Convert.ToDouble(inActiveSR) / Convert.ToDouble(totalSR))).ToString();
                                netTotalInactiveSR = netTotalInactiveSR + Convert.ToInt32(inActiveSR);
                            }
                            else
                            {
                                inActiveSR = totalSR.ToString();
                                inActiveSRpercent = "100%";
                            }

                            string qrTotalOlt = @"SELECT COUNT(*) TOTAL_OUTLET FROM T_OUTLET
                                                    WHERE STATUS='Y' AND SR_ID IN(SELECT SR_ID TOTAL_SR FROM T_SR_INFO WHERE DIVISION_NAME='" + division + "' AND DIST_ZONE='" + ZONE_ID + "' AND ITEM_GROUP_ID='" + group + "' AND STATUS='Y')";
                            OracleCommand cmdOlt = new OracleCommand(qrTotalOlt, con);
                            OracleDataAdapter daOlt = new OracleDataAdapter(cmdOlt);
                            DataSet dsOlt = new DataSet();
                            daOlt.Fill(dsOlt);
                            int olt = dsOlt.Tables[0].Rows.Count;
                            if (olt > 0 && dsOlt.Tables[0].Rows[0][0].ToString() != "0")
                            {
                                totalOutlet = dsOlt.Tables[0].Rows[0][0].ToString();

                                netTotalOutlet = netTotalOutlet + Convert.ToInt32(totalOutlet);
                            }

                            string qrVstOlt = @"SELECT DISTINCT OUTLET_ID FROM T_ORDER_DETAIL
                                                    WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE DIVISION_NAME='" + division + "' AND DIST_ZONE='" + ZONE_ID + "' AND ITEM_GROUP_ID='" + group + "' AND STATUS='Y') AND ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";
                            qrVstOlt = qrVstOlt + @" UNION
                                                    SELECT DISTINCT OUTLET_ID FROM T_NON_PRODUCTIVE_SALES
                                                    WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE DIVISION_NAME='" + division + "' AND DIST_ZONE='" + ZONE_ID + "' AND ITEM_GROUP_ID='" + group + "' AND STATUS='Y') AND ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";

                            OracleCommand cmdvstOlt = new OracleCommand(qrVstOlt, con);
                            OracleDataAdapter davstOlt = new OracleDataAdapter(cmdvstOlt);
                            DataSet dsvstOlt = new DataSet();
                            davstOlt.Fill(dsvstOlt);
                            int vstolt = dsvstOlt.Tables[0].Rows.Count;
                            if (vstolt > 0 && dsvstOlt.Tables[0].Rows[0][0].ToString() != "")
                            {
                                netTotalVisitedOutlet = netTotalVisitedOutlet + vstolt;

                                totalVisitedOutlet = vstolt.ToString();
                                totalVisitedOutletPercent = DisplayPercentage((double)(Convert.ToDouble(totalVisitedOutlet) / Convert.ToDouble(totalOutlet))).ToString();

                                totalPendingOutlet = (Convert.ToInt32(totalOutlet) - Convert.ToInt32(totalVisitedOutlet)).ToString();
                                totalPendingOutletPercent = DisplayPercentage((double)(Convert.ToDouble(totalPendingOutlet) / Convert.ToDouble(totalOutlet))).ToString();

                                avgVisitedOutlet = (Convert.ToInt32(totalVisitedOutlet) / Convert.ToInt32(activeSR)).ToString();
                            }




                            string qrSuccessCall = @"SELECT COUNT(DISTINCT OUTLET_ID) VISITED_OLT FROM T_ORDER_DETAIL
                                                    WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE DIVISION_NAME='" + division + "' AND DIST_ZONE='" + ZONE_ID + "' AND ITEM_GROUP_ID='" + group + "' AND STATUS='Y') AND ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";

                            OracleCommand cmdSc = new OracleCommand(qrSuccessCall, con);
                            OracleDataAdapter daSc = new OracleDataAdapter(cmdSc);
                            DataSet dsSc = new DataSet();
                            daSc.Fill(dsSc);
                            int vsSc = dsSc.Tables[0].Rows.Count;
                            if (vsSc > 0 && dsSc.Tables[0].Rows[0][0].ToString() != "0")
                            {
                                successfulCall = dsSc.Tables[0].Rows[0][0].ToString();
                                nonSuccessfulCall = (Convert.ToInt32(totalVisitedOutlet) - Convert.ToInt32(successfulCall)).ToString();

                                memoPercent = DisplayPercentage((double)(Convert.ToDouble(successfulCall) / Convert.ToDouble(totalVisitedOutlet))).ToString();

                                netTotalMemo = netTotalMemo + Convert.ToInt32(successfulCall);
                            }

                            string qrLPC = @"SELECT COUNT(ITEM_ID) LPC FROM T_ORDER_DETAIL
                                                WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE DIVISION_NAME='" + division + "' AND DIST_ZONE='" + ZONE_ID + "' AND ITEM_GROUP_ID='" + group + "' AND STATUS='Y') AND ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";
                            OracleCommand cmdLPC = new OracleCommand(qrLPC, con);
                            OracleDataAdapter daLPC = new OracleDataAdapter(cmdLPC);
                            DataSet dsLPC = new DataSet();
                            daLPC.Fill(dsLPC);
                            int lpc = dsLPC.Tables[0].Rows.Count;
                            if (lpc > 0 && dsLPC.Tables[0].Rows[0][0].ToString() != "0")
                            {
                                string totalLine = dsLPC.Tables[0].Rows[0]["LPC"].ToString();

                                string slpc = (Convert.ToDouble(totalLine) / Convert.ToInt32(successfulCall)).ToString();
                                LPC = String.Format("{0:0.00}", (Convert.ToDouble(slpc)));

                                netTotalLine = netTotalLine + Convert.ToInt32(totalLine);
                            }



                            string qrAmt = @"SELECT TO_CHAR(SUM((T1.ITEM_QTY+(T1.ITEM_CTN*T2.FACTOR))*T1.OUT_PRICE),'999999999.99') AMT FROM         
                                                (SELECT * FROM T_ORDER_DETAIL    
                                                WHERE ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE DIVISION_NAME='" + division + "' AND DIST_ZONE='" + ZONE_ID + "' AND ITEM_GROUP_ID='" + group + "' AND STATUS='Y')) T1, (SELECT ITEM_ID,FACTOR FROM T_ITEM) T2 WHERE T1.ITEM_ID=T2.ITEM_ID";

                            OracleCommand cmdAmt = new OracleCommand(qrAmt, con);
                            OracleDataAdapter daAmt = new OracleDataAdapter(cmdAmt);
                            DataSet dsAmt = new DataSet();
                            daAmt.Fill(dsAmt);
                            int amt = dsAmt.Tables[0].Rows.Count;
                            if (amt > 0 && dsAmt.Tables[0].Rows[0][0].ToString() != "")
                            {
                                totalAmount = dsAmt.Tables[0].Rows[0]["AMT"].ToString();
                                totalAmount = String.Format("{0:0.00}", (Convert.ToDouble(totalAmount) / 1000));

                                netTotalAmount = netTotalAmount + (Convert.ToDouble(totalAmount));

                                avgAmount = (Convert.ToDouble(totalAmount) / Convert.ToInt32(activeSR)).ToString();
                                avgAmount = String.Format("{0:0.00}", (Convert.ToDouble(avgAmount)));

                            }

                            string qrLeave = "SELECT DISTINCT SR_ID FROM T_LEAVE WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE DIVISION_NAME='" + division + "' AND DIST_ZONE='" + ZONE_ID + "' AND ITEM_GROUP_ID='" + group + "' AND STATUS='Y') AND FROM_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') ";
                            OracleCommand cmdL = new OracleCommand(qrLeave, con);
                            OracleDataAdapter daL = new OracleDataAdapter(cmdL);
                            DataSet dsL = new DataSet();
                            daL.Fill(dsL);
                            int L = dsL.Tables[0].Rows.Count;
                            if (L > 0 && dsL.Tables[0].Rows[0][0].ToString() != "")
                            {
                                srOnLeave = L.ToString();
                                netSRonLeave = netSRonLeave + Convert.ToInt32(srOnLeave);
                            }

                            
                            //--strike rate---------------------------
                            string strk = "0%";
                            if (Convert.ToInt32(totalVisitedOutlet) > 0)
                            {
                                double sk = Convert.ToDouble(successfulCall) / Convert.ToDouble(totalVisitedOutlet);
                                strk = DisplayPercentage(sk);

                                if (Convert.ToDouble(netTotalAmount) > 0)
                                {
                                    avgOltAmt = (Convert.ToDouble(totalAmount) / Convert.ToDouble(successfulCall)).ToString();
                                    avgOltAmt = String.Format("{0:0.00}", (Convert.ToDouble(avgOltAmt)));
                                }
                            }

                            if (Convert.ToInt32(activeSR) > 0)
                            {
                                trRow2 = trRow2 + "<td><div style='width:100%;height:auto;margin-top: 5px;border:1px solid #8dc060;text-align:center;font-size:14px;padding-top:7px;padding-bottom: 7px; margin-left:1%;'>" +
                                                "<div style='background-color:#f06292;color:#fff;margin-left:2px;margin-right:2px;'><span>Act SR: " + activeSR + " of " + totalSR + "</span><br/>" +
                                                "<span>Inact SR: " + inActiveSR + " of " + totalSR + "</span><br/>" +
                                                "<span>SR on Leave: " + srOnLeave + " of " + totalSR + "</span></div><br/>" +

                                                "<div style='background-color:#2196f3;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Amt: " + totalAmount + "</span><br/>" +
                                                "<span>SR Avg.Ord: " + avgAmount + "</span><br/>" +
                                                "<span>Olt Avg.Ord: " + avgOltAmt + "</span></div><br/>" +

                                                 "<div style='background-color:#ff9800;color:#fff;margin-left:2px;margin-right:2px;'><span>LPC: " + LPC + "</span><br/>" +
                                                "<span>Strk Rate: " + strk + "</span></span></div><br/>" +

                                                 "<div style='background-color:#009688;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Outlet: " + totalOutlet + "</span><br/>" +
                                                "<span>Vstd Olt: " + totalVisitedOutlet + "</span><br/>" +
                                                "<span>Total memo: " + successfulCall.ToString() + "</span></div><br/>" +

                                            "</div></td>";
                            }
                            else
                            {
                                netTotalInactiveSR = netTotalInactiveSR + Convert.ToInt32(totalSR);
                                inActiveSR = totalSR.ToString();
                                inActiveSRpercent = "100%";

                                trRow2 = trRow2 + "<td><div style='width:100%;height:auto;margin-top: 5px;border:1px solid #8dc060;text-align:center;font-size:14px;padding-top:7px;padding-bottom: 7px; margin-left:1%;'>" +
                                                "<div style='background-color:#f06292;color:#fff;margin-left:2px;margin-right:2px;'><span>Act SR: " + activeSR + " of " + totalSR + "</span><br/>" +
                                                "<span>Inact SR: " + inActiveSR + " of " + totalSR + "</span><br/>" +
                                                "<span>SR on Leave: " + srOnLeave + " of " + totalSR + "</span></div><br/>" +

                                                 "<div style='background-color:#2196f3;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Amt: " + totalAmount + "</span><br/>" +
                                                "<span>SR Avg.Ord: " + avgAmount + "</span><br/>" +
                                                "<span>Olt Avg.Ord: " + avgOltAmt + "</span></div><br/>" +

                                                "<div style='background-color:#ff9800;color:#fff;margin-left:2px;margin-right:2px;'><span>LPC: " + LPC + "</span><br/>" +
                                                "<span>Strk Rate: " + strk + "</span></span></div><br/>" +

                                                 "<div style='background-color:#009688;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Outlet: " + totalOutlet + "</span><br/>" +
                                                "<span>Vstd Olt: " + totalVisitedOutlet + "</span><br/>" +
                                                "<span>Total memo: " + successfulCall.ToString() + "</span></div><br/>" +

                                            "</div></td>";
                            }

                        }
                    }

                    string avgNetOltOrd = "0";
                    //---main khela here----------------------
                    netTotalInactiveSR = Convert.ToInt32(netTotalSR) - Convert.ToInt32(netTotalActiveSR);

                    netTotalAvgAmount = (Convert.ToDouble(netTotalAmount) / Convert.ToInt32(netTotalActiveSR));
                    string netAvgAmt = String.Format("{0:0.00}", netTotalAvgAmount);

                    string netLPC = String.Format("{0:0.00}", (Convert.ToDouble((Convert.ToDouble(netTotalLine) / Convert.ToDouble(netTotalMemo)))));

                    string netStrk = "0%";
                    if (Convert.ToInt32(netTotalVisitedOutlet) > 0)
                    {
                        double sk = Convert.ToDouble(netTotalMemo) / Convert.ToDouble(netTotalVisitedOutlet);
                        netStrk = DisplayPercentage(sk);

                        if (Convert.ToDouble(netTotalAmount) > 0)
                        {
                            avgNetOltOrd = (Convert.ToDouble(netTotalAmount) / Convert.ToDouble(netTotalMemo)).ToString();
                            avgNetOltOrd = String.Format("{0:0.00}", (Convert.ToDouble(avgNetOltOrd)));
                        }
                    }

                    string totalRow = "<td><div style='width:100%;height:auto;margin-top: 5px;border:1px solid #8dc060;text-align:center;font-size:14px;padding-top:7px;padding-bottom: 7px; margin-left:1%;'>" +
                                                "<div style='background-color:#f06292;color:#fff;margin-left:2px;margin-right:2px;'><span>Active SR: " + netTotalActiveSR + " of " + netTotalSR + "</span><br/>" +
                                                "<span>Inactive SR: " + netTotalInactiveSR + " of " + netTotalSR + "</span><br/>" +
                                                "<span>SR on Leave: " + netSRonLeave + " of " + netTotalSR + "</span></div><br/>" +

                                                "<div style='background-color:#2196f3;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Amt: " + netTotalAmount + "</span><br/>" +
                                                "<span>SR Avg.Ord: " + netAvgAmt + "</span><br/>" +
                                                "<span>Outlet Avg.Ord: " + avgNetOltOrd + "</span></div><br/>" +

                                                "<div style='background-color:#ff9800;color:#fff;margin-left:2px;margin-right:2px;'><span>LPC: " + netLPC + "</span><br/>" +
                                                "<span>Strike Rate: " + netStrk + "</span></span></div><br/>" +

                                                 "<div style='background-color:#009688;color:#fff;margin-left:2px;margin-right:2px;'><span>Total Outlet: " + netTotalOutlet + "</span><br/>" +
                                                "<span>Visited Outlet: " + netTotalVisitedOutlet + "</span><br/>" +
                                                "<span>Total memo: " + netTotalMemo + "</span></div><br/>" +

                                            "</div></td>";
                    trRow2 = totalRow + trRow2;

                    msg = msg + "<tr>" + trRow1 + "</tr><tr>" + trRow2 + "</tr></table></div>";


                }
            }
            catch (Exception ex) { }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod]
    public static string GetCOOWiseReport(string company, string fdate, string tdate)
    {
        string msg = "";

        try
        {
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            try
            {
                string qrGroup = "";

                if (company == "All")
                {
                    qrGroup = @"SELECT COMPANY_ID,COMPANY_NICK_NAME FROM T_COMPANY";
                }
                else
                {
                    qrGroup = @" SELECT COMPANY_ID,COMPANY_NICK_NAME FROM T_COMPANY WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString() + "' ";
                }

                OracleCommand cmdP = new OracleCommand(qrGroup, con);
                OracleDataAdapter daP = new OracleDataAdapter(cmdP);
                DataSet dsP = new DataSet();
                daP.Fill(dsP);
                int c = dsP.Tables[0].Rows.Count;
                if (c > 0 && dsP.Tables[0].Rows[0][0].ToString() != "")
                {
                    for (int i = 0; i < c; i++)
                    {
                        string companyName = dsP.Tables[0].Rows[i]["COMPANY_NICK_NAME"].ToString();
                        string companyId = dsP.Tables[0].Rows[i]["COMPANY_ID"].ToString();
                         

                        string totalSR = "0";
                        string activeSR = "0";
                        string activeSRpercent = "0";
                        string inActiveSR = "0";
                        string inActiveSRpercent = "0";
                        string totalOutlet = "0";
                        string totalVisitedOutlet = "0";
                        string totalVisitedOutletPercent = "0";
                        string totalPendingOutlet = "0";
                        string totalPendingOutletPercent = "0";
                        string avgVisitedOutlet = "0";
                        string successfulCall = "0";
                        string nonSuccessfulCall = "0";
                        string memoPercent = "0";
                        string LPC = "0";
                        string totalAmount = "0";
                        string avgAmount = "0";


                        string qrSR = @"SELECT COUNT(*) TOTAL_SR FROM T_SR_INFO WHERE ITEM_GROUP_ID IN(SELECT ITEM_GROUP_ID FROM T_ITEM_GROUP WHERE COMPANY_ID='" + companyId + "' AND STATUS='Y')";
                        OracleCommand cmdsr = new OracleCommand(qrSR, con);
                        OracleDataAdapter dasr = new OracleDataAdapter(cmdsr);
                        DataSet dssr = new DataSet();
                        dasr.Fill(dssr);
                        int sr = dssr.Tables[0].Rows.Count;
                        if (sr > 0 && dssr.Tables[0].Rows[0][0].ToString() != "0")
                        {
                            totalSR = dssr.Tables[0].Rows[0][0].ToString();


                            string qrActiveSR = @"SELECT DISTINCT SR_ID FROM T_ORDER_DETAIL
                                                WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID IN(SELECT ITEM_GROUP_ID FROM T_ITEM_GROUP WHERE COMPANY_ID='" + companyId + "' AND STATUS='Y')) AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy')";
                            OracleCommand cmdASR = new OracleCommand(qrActiveSR, con);
                            OracleDataAdapter daASR = new OracleDataAdapter(cmdASR);
                            DataSet dsASR = new DataSet();
                            daASR.Fill(dsASR);
                            int asr = dsASR.Tables[0].Rows.Count;
                            if (asr > 0 && dsASR.Tables[0].Rows[0][0].ToString() != "")
                            {
                                activeSR = asr.ToString();
                                activeSRpercent = DisplayPercentage((double)Convert.ToDouble(activeSR) / Convert.ToDouble(totalSR)).ToString();

                                inActiveSR = (Convert.ToInt32(totalSR) - Convert.ToInt32(activeSR)).ToString();
                                inActiveSRpercent = DisplayPercentage((double)(Convert.ToDouble(inActiveSR) / Convert.ToDouble(totalSR))).ToString();

                                string qrTotalOlt = @"SELECT COUNT(*) TOTAL_OUTLET FROM T_OUTLET
                                                    WHERE STATUS='Y' AND SR_ID IN(SELECT SR_ID TOTAL_SR FROM T_SR_INFO WHERE ITEM_GROUP_ID IN(SELECT ITEM_GROUP_ID FROM T_ITEM_GROUP WHERE COMPANY_ID='" + companyId + "' AND STATUS='Y'))";
                                OracleCommand cmdOlt = new OracleCommand(qrTotalOlt, con);
                                OracleDataAdapter daOlt = new OracleDataAdapter(cmdOlt);
                                DataSet dsOlt = new DataSet();
                                daOlt.Fill(dsOlt);
                                int olt = dsOlt.Tables[0].Rows.Count;
                                if (olt > 0 && dsOlt.Tables[0].Rows[0][0].ToString() != "")
                                {
                                    totalOutlet = dsOlt.Tables[0].Rows[0][0].ToString();
                                }

                                string qrVstOlt = @"SELECT DISTINCT OUTLET_ID FROM T_ORDER_DETAIL
                                                    WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID IN(SELECT ITEM_GROUP_ID FROM T_ITEM_GROUP WHERE COMPANY_ID='" + companyId + "' AND STATUS='Y')) AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy') ";
                                qrVstOlt = qrVstOlt + @" UNION
                                                    SELECT DISTINCT OUTLET_ID FROM T_NON_PRODUCTIVE_SALES
                                                    WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID IN(SELECT ITEM_GROUP_ID FROM T_ITEM_GROUP WHERE COMPANY_ID='" + companyId + "' AND STATUS='Y')) AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy') ";

                                OracleCommand cmdvstOlt = new OracleCommand(qrVstOlt, con);
                                OracleDataAdapter davstOlt = new OracleDataAdapter(cmdvstOlt);
                                DataSet dsvstOlt = new DataSet();
                                davstOlt.Fill(dsvstOlt);
                                int vstolt = dsvstOlt.Tables[0].Rows.Count;
                                if (vstolt > 0 && dsvstOlt.Tables[0].Rows[0][0].ToString() != "")
                                {
                                    totalVisitedOutlet = vstolt.ToString();
                                    totalVisitedOutletPercent = DisplayPercentage((double)(Convert.ToDouble(totalVisitedOutlet) / Convert.ToDouble(totalOutlet))).ToString();

                                    totalPendingOutlet = (Convert.ToInt32(totalOutlet) - Convert.ToInt32(totalVisitedOutlet)).ToString();
                                    totalPendingOutletPercent = DisplayPercentage((double)(Convert.ToDouble(totalPendingOutlet) / Convert.ToDouble(totalOutlet))).ToString();
                                }

                                avgVisitedOutlet = (Convert.ToInt32(totalVisitedOutlet) / Convert.ToInt32(activeSR)).ToString();


                                string qrSuccessCall = @"SELECT COUNT(DISTINCT OUTLET_ID) VISITED_OLT FROM T_ORDER_DETAIL
                                                    WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID IN(SELECT ITEM_GROUP_ID FROM T_ITEM_GROUP WHERE COMPANY_ID='" + companyId + "' AND STATUS='Y')) AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy')";

                                OracleCommand cmdSc = new OracleCommand(qrSuccessCall, con);
                                OracleDataAdapter daSc = new OracleDataAdapter(cmdSc);
                                DataSet dsSc = new DataSet();
                                daSc.Fill(dsSc);
                                int vsSc = dsSc.Tables[0].Rows.Count;
                                if (vsSc > 0 && dsSc.Tables[0].Rows[0][0].ToString() != "")
                                {
                                    successfulCall = dsSc.Tables[0].Rows[0][0].ToString();
                                    nonSuccessfulCall = (Convert.ToInt32(totalVisitedOutlet) - Convert.ToInt32(successfulCall)).ToString();

                                    memoPercent = DisplayPercentage((double)(Convert.ToDouble(successfulCall) / Convert.ToDouble(totalVisitedOutlet))).ToString();

                                }

                                string qrLPC = @"SELECT TO_CHAR(COUNT(ITEM_ID)/COUNT(DISTINCT OUTLET_ID),'999999999.99') LPC FROM T_ORDER_DETAIL
                                                WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID IN(SELECT ITEM_GROUP_ID FROM T_ITEM_GROUP WHERE COMPANY_ID='" + companyId + "' AND STATUS='Y')) AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy')";
                                OracleCommand cmdLPC = new OracleCommand(qrLPC, con);
                                OracleDataAdapter daLPC = new OracleDataAdapter(cmdLPC);
                                DataSet dsLPC = new DataSet();
                                daLPC.Fill(dsLPC);
                                int lpc = dsLPC.Tables[0].Rows.Count;
                                if (lpc > 0 && dsLPC.Tables[0].Rows[0][0].ToString() != "")
                                {
                                    LPC = dsLPC.Tables[0].Rows[0]["LPC"].ToString();

                                }

                                string qrAmt = @"SELECT TO_CHAR(SUM((T1.ITEM_QTY+(T1.ITEM_CTN*T2.FACTOR))*T1.OUT_PRICE),'999999999.99') AMT FROM         
                                                (SELECT * FROM T_ORDER_DETAIL    
                                                WHERE ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','dd/mm/yyyy') AND TO_DATE('" + tdate + "','dd/mm/yyyy') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID IN(SELECT ITEM_GROUP_ID FROM T_ITEM_GROUP WHERE COMPANY_ID='" + companyId + "' AND STATUS='Y') AND STATUS='Y')) T1, (SELECT ITEM_ID,FACTOR FROM T_ITEM) T2 WHERE T1.ITEM_ID=T2.ITEM_ID";

                                OracleCommand cmdAmt = new OracleCommand(qrAmt, con);
                                OracleDataAdapter daAmt = new OracleDataAdapter(cmdAmt);
                                DataSet dsAmt = new DataSet();
                                daAmt.Fill(dsAmt);
                                int amt = dsAmt.Tables[0].Rows.Count;
                                if (amt > 0 && dsAmt.Tables[0].Rows[0][0].ToString() != "")
                                {
                                    totalAmount = dsAmt.Tables[0].Rows[0]["AMT"].ToString();
                                    avgAmount = String.Format("{0:0.00}", ((Convert.ToDouble(totalAmount) / Convert.ToInt32(activeSR)) / 1000));  
                                    totalAmount = String.Format("{0:0.00}", (Convert.ToDouble(totalAmount)/1000));
                                }                                

                                msg = msg + ";" + companyName + ";" + totalOutlet + ";" + totalVisitedOutlet + ";" + totalPendingOutlet + ";" + successfulCall + ";" + nonSuccessfulCall + ";" + companyName + ";" + totalSR + ";" + activeSR + ";" + activeSRpercent + ";" + inActiveSR + ";" + inActiveSRpercent + ";" + totalVisitedOutletPercent + ";" + memoPercent + ";" + LPC + ";" + totalAmount + ";" + avgAmount + ";" + avgVisitedOutlet + ";" + totalPendingOutletPercent;

                            }

                        }
                    }
                }
            }
            catch (Exception ex) { }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod]
    public static string GetTopRatedSR(string topsr)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string currentDate = HttpContext.Current.Session["todaysDate"].ToString();

            string query = @"SELECT DISTINCT T4.*,T3.TOTAL_AMT FROM
                (SELECT T1.SR_ID,(SUM(((T1.ITEM_CTN*T2.FACTOR)+T1.ITEM_QTY)*T1.OUT_PRICE)/1000) TOTAL_AMT FROM
                (SELECT SR_ID,ITEM_ID,ITEM_QTY,ITEM_CTN,OUT_PRICE FROM T_ORDER_DETAIL WHERE ENTRY_DATE=TO_DATE('" + currentDate.Trim() + "','DD/MM/YYYY') " +
                @"AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "')) T1, " +
                @"(SELECT ITEM_ID,FACTOR FROM T_ITEM) T2
                WHERE T1.ITEM_ID=T2.ITEM_ID GROUP BY T1.SR_ID) T3,
                (
                  SELECT t1.*,t2.tsm_name,t2.tsm_mobile from
                  (SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP FROM T_SR_INFO WHERE ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "') t1, " +
                  @"(SELECT T_TSM_SR.SR_ID,T_TSM_ZM.TSM_NAME,T_TSM_ZM.MOBILE_NO tsm_mobile FROM T_TSM_SR,T_TSM_ZM
                   WHERE T_TSM_SR.TSM_ID=T_TSM_ZM.TSM_ID) t2 where t1.sr_id=t2.sr_id 
                
                ) T4 
                WHERE T3.SR_ID=T4.SR_ID 
                ORDER BY T4.ITEM_GROUP,T3.TOTAL_AMT ASC";
        
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            int k = 1;
            string tbl = "<table class='table-striped table-bordered table-hover' style='border-collapse: collapse;width:100%;font-size:13px;'>";
            string row = "";
            row = row + "<tr style='background-color:#ff9800;'><td style='border: 1px solid orange;padding: 5px;text-align: left;'>SR ID</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>SR NAME</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>SR MOBILE NO</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>TOTAL AMOUNT</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>GROUP NAME</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>TSM NAME</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>TSM MOBILE NO</td></tr>";
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string ITEM_GROUP_NAME = ds.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                    string SR_NAME = ds.Tables[0].Rows[i]["SR_NAME"].ToString();
                    string TOTAL_AMT = String.Format("{0:0.00}", Convert.ToDouble(ds.Tables[0].Rows[i]["TOTAL_AMT"].ToString()));
                    string SR_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string SR_MOBILE = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string TSM_MOBILE = ds.Tables[0].Rows[i]["TSM_MOBILE"].ToString();
                    string TSM_NAME = ds.Tables[0].Rows[i]["TSM_NAME"].ToString();
                    row = row + "<tr><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + SR_ID + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + SR_NAME + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + SR_MOBILE + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + TOTAL_AMT + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + ITEM_GROUP_NAME + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + TSM_NAME + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + TSM_MOBILE + "</td></tr>";
                    //row = row + "<tr><td style='padding: 5px;text-align: left;'>" + SR_ID + "</td><td style='padding: 5px;text-align: left;'>" + SR_NAME + "</td><td style='padding: 5px;text-align: left;'>" + SR_MOBILE + "</td><td style='padding: 5px;text-align: left;'>" + TOTAL_AMT + "</td><td style='padding: 5px;text-align: left;'>" + ITEM_GROUP_NAME + "</td><td style='padding: 5px;text-align: left;'>" + TSM_NAME + "</td><td style='padding: 5px;text-align: left;'>" + TSM_MOBILE + "</td></tr>";
                    //msg = msg + "&nbsp;" + SR_ID + "</br>";
                    k++;
                }

                msg = msg + tbl + row + "</table>";
            }            

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
    
    [WebMethod]
    public static string GetTopRatedHOS(string topsr)
    {
        string msg = "";
        msg = HttpContext.Current.Session["tblHos"].ToString();
        /*OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string currentDate = HttpContext.Current.Session["todaysDate"].ToString();

            string qrHos = @"SELECT T1.STAFF_ID,T1.HOS_NAME,T1.MOBILE_NO,T2.ITEM_GROUP_ID,T1.ITEM_GROUP FROM
                            (SELECT STAFF_ID,HOS_NAME,MOBILE_NO,ITEM_GROUP FROM T_HOS)T1,
                            (SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString() + "' AND STATUS='Y') T2 WHERE T1.ITEM_GROUP=T2.ITEM_GROUP_NAME";
            OracleCommand cmdd = new OracleCommand(qrHos, conn);
            OracleDataAdapter daa = new OracleDataAdapter(cmdd);
            DataSet dss = new DataSet();
            daa.Fill(dss);
            int s = dss.Tables[0].Rows.Count;

            if (s > 0)
            {
                int k = 1;
                string tbl = "<table style='border-collapse: collapse;width:100%;'>";
                string row = "";
                row = row + "<tr style='background-color:#ff9800;'><td style='border: 1px solid orange;padding: 5px;text-align: left;'>HOS ID</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>HOS NAME</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>MOBILE NO</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>GROUP NAME</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>TOTAL AMOUNT</td></tr>";
 
                for (int i = 0; i < s; i++)
                {
                    string ITEM_GROUP_ID = dss.Tables[0].Rows[i]["ITEM_GROUP_ID"].ToString();

                    string ITEM_GROUP_NAME = dss.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                    string HOS_NAME = dss.Tables[0].Rows[i]["HOS_NAME"].ToString();
                    string TOTAL_AMT = "0";//String.Format("{0:0.00}", Convert.ToDouble(dss.Tables[0].Rows[i]["TOTAL_AMT"].ToString()));
                    string STAFF_ID = dss.Tables[0].Rows[i]["STAFF_ID"].ToString();
                    string HOS_MOBILE = dss.Tables[0].Rows[i]["MOBILE_NO"].ToString();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("STAFF_ID", typeof(string));
                    dt.Columns.Add("HOS_NAME", typeof(string));
                    dt.Columns.Add("HOS_MOBILE", typeof(string));
                    dt.Columns.Add("ITEM_GROUP_NAME", typeof(string));
                    dt.Columns.Add("TOTAL_AMOUNT", typeof(double));
                    dt.Rows.Add(STAFF_ID, HOS_NAME, HOS_MOBILE, ITEM_GROUP_NAME, Convert.ToDouble(TOTAL_AMT));
                    

                    string qrAmt = @"SELECT SUM(T3.TOTAL_AMT) TOTAL_AMOUNT FROM
                                    (SELECT T1.SR_ID, (SUM(((T1.ITEM_CTN*T2.FACTOR)+T1.ITEM_QTY)*T1.OUT_PRICE)/1000) TOTAL_AMT FROM
                                    (SELECT SR_ID,ITEM_ID,ITEM_CTN,ITEM_QTY,OUT_PRICE FROM T_ORDER_DETAIL WHERE ENTRY_DATE=TO_DATE('" + currentDate.Trim() + "','DD/MM/YYYY')) T1, " +
                                    @"(SELECT ITEM_ID,FACTOR FROM T_ITEM) T2
                                    WHERE T1.ITEM_ID=T2.ITEM_ID
                                    GROUP BY T1.SR_ID) T3,
                                    (SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + ITEM_GROUP_ID.Trim() + "' AND STATUS='Y') T4 WHERE T3.SR_ID=T4.SR_ID";

                    qrAmt = @"SELECT TO_CHAR(SUM((T1.ITEM_QTY+(T1.ITEM_CTN*T2.FACTOR))*T1.OUT_PRICE),'999999999.99') TOTAL_AMOUNT FROM         
                                                (SELECT * FROM T_ORDER_DETAIL    
                                                WHERE ENTRY_DATE=TO_DATE('" + currentDate + "','DD/MM/YYYY') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + ITEM_GROUP_ID.Trim() + "' AND STATUS='Y')) T1, (SELECT ITEM_ID,FACTOR FROM T_ITEM) T2 WHERE T1.ITEM_ID=T2.ITEM_ID";

                    OracleCommand cmddd = new OracleCommand(qrAmt, conn);
                    OracleDataAdapter daaa = new OracleDataAdapter(cmddd);
                    DataSet dsss = new DataSet();
                    daaa.Fill(dsss);
                    int ss = dsss.Tables[0].Rows.Count;

                    if (ss > 0 && dsss.Tables[0].Rows[0]["TOTAL_AMOUNT"].ToString().Length > 0)
                    {
                        TOTAL_AMT = String.Format("{0:0.00}", (Convert.ToDouble(dsss.Tables[0].Rows[0]["TOTAL_AMOUNT"].ToString()) / 1000));
                        dt.Rows.Add(STAFF_ID, HOS_NAME, HOS_MOBILE, ITEM_GROUP_NAME, Convert.ToDouble(TOTAL_AMT));
                         
                        
                        row = row + "<tr><td style='border: 1px solid orange;padding: 5px;text-align: left;'>" + STAFF_ID + "</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>" + HOS_NAME + "</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>" + HOS_MOBILE + "</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>" + ITEM_GROUP_NAME + "</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>" + TOTAL_AMT + "</td></tr>";
                    }

                                        //msg = msg + "&nbsp;" + SR_ID + "</br>";
                    k++;
                }

                msg = msg + tbl + row + "</table>";
            }
                   

            conn.Close();
        }
        catch (Exception ex) { } */

        return msg;
    }

    [WebMethod]
    public static string GetTopRatedTSM(string topsr)
    {
        string msg = HttpContext.Current.Session["tblTSM"].ToString();
        return msg;
    }
    
    [WebMethod]
    public static string GetTopRatedRM(string topsr)
    {
        string msg = HttpContext.Current.Session["tblRM"].ToString();
        
        /*OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string currentDate = HttpContext.Current.Session["todaysDate"].ToString();


            string qrHos = @"SELECT T1.STAFF_ID,T1.HOS_NAME,T1.MOBILE_NO,T2.ITEM_GROUP_ID,T1.ITEM_GROUP FROM
                            (SELECT STAFF_ID,HOS_NAME,MOBILE_NO,ITEM_GROUP FROM T_HOS)T1,
                            (SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString() + "' AND STATUS='Y') T2 WHERE T1.ITEM_GROUP=T2.ITEM_GROUP_NAME";
            OracleCommand cmdd = new OracleCommand(qrHos, conn);
            OracleDataAdapter daa = new OracleDataAdapter(cmdd);
            DataSet dss = new DataSet();
            daa.Fill(dss);
            int s = dss.Tables[0].Rows.Count;

            if (s > 0)
            {
                int k = 1;
                string tbl = "<table style='border-collapse: collapse;width:100%;'>";
                string row = "";
                row = row + "<tr style='background-color:#ff9800;'><td style='border: 1px solid orange;padding: 5px;text-align: left;'>AGM/RM ID</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>AGM/RM NAME</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>MOBILE NO</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>GROUP NAME</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>TOTAL AMOUNT</td></tr>";

                for (int i = 0; i < s; i++)
                {
                    string ITEM_GROUP_ID = dss.Tables[0].Rows[i]["ITEM_GROUP_ID"].ToString();
                    string ITEM_GROUP_NAME = dss.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                    string STAFF_ID = dss.Tables[0].Rows[i]["STAFF_ID"].ToString();

                    string qrRM = @"SELECT RM_ID,RM_NAME,MOBILE_NO FROM T_AGM_RM WHERE HOS_ID='" + STAFF_ID + "'";
                    OracleCommand cmdRM = new OracleCommand(qrRM, conn);
                    OracleDataAdapter daRM = new OracleDataAdapter(cmdRM);
                    DataSet dsRM = new DataSet();
                    daRM.Fill(dsRM);
                    int rm = dsRM.Tables[0].Rows.Count;
                    if (rm > 0)
                    {
                        for (int q = 0; q < rm; q++)
                        {
                            string RM_ID = dsRM.Tables[0].Rows[q]["RM_ID"].ToString();
                            string RM_NAME = dsRM.Tables[0].Rows[q]["RM_NAME"].ToString();
                            string MOBILE_NO = dsRM.Tables[0].Rows[q]["MOBILE_NO"].ToString();                             
                            string TOTAL_AMT = "0";

                            string qrAmt = @"SELECT SUM(T3.TOTAL_AMT) TOTAL_AMOUNT FROM
                                            (SELECT T1.SR_ID, (SUM(((T1.ITEM_CTN*T2.FACTOR)+T1.ITEM_QTY)*T1.OUT_PRICE)/1000) TOTAL_AMT FROM
                                            (SELECT SR_ID,ITEM_ID,ITEM_CTN,ITEM_QTY,OUT_PRICE FROM T_ORDER_DETAIL WHERE ENTRY_DATE=TO_DATE('" + currentDate.Trim() + "','DD/MM/YYYY')) T1, " +
                                            @"(SELECT ITEM_ID,FACTOR FROM T_ITEM WHERE ITEM_GROUP='" + ITEM_GROUP_ID + "') T2 " +
                                            @"WHERE T1.ITEM_ID=T2.ITEM_ID GROUP BY T1.SR_ID) T3,
                                            (SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + ITEM_GROUP_ID + "' AND STATUS='Y' " +
                                               @"AND SR_ID IN(SELECT SR_ID FROM T_TSM_SR WHERE TSM_ID IN(SELECT TSM_ID FROM T_TSM_ZM WHERE AGM_RM_ID='" + RM_ID + "' AND STATUS='Y')) " +
                                            @") T4
                                            WHERE T3.SR_ID=T4.SR_ID";

                            OracleCommand cmddd = new OracleCommand(qrAmt, conn);
                            OracleDataAdapter daaa = new OracleDataAdapter(cmddd);
                            DataSet dsss = new DataSet();
                            daaa.Fill(dsss);
                            int ss = dsss.Tables[0].Rows.Count;

                            if (ss > 0 && dsss.Tables[0].Rows[0]["TOTAL_AMOUNT"].ToString().Length > 0)
                            {
                                TOTAL_AMT = String.Format("{0:0.00}", Convert.ToDouble(dsss.Tables[0].Rows[0]["TOTAL_AMOUNT"].ToString()));
                                row = row + "<tr><td style='border: 1px solid orange;padding: 5px;text-align: left;'>" + RM_ID + "</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>" + RM_NAME + "</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>" + MOBILE_NO + "</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>" + ITEM_GROUP_NAME + "</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>" + TOTAL_AMT + "</td></tr>";                            
                            }

                            //msg = msg + "&nbsp;" + SR_ID + "</br>";
                            k++;
                        }
                    }

                    
                }

                msg = msg + tbl + row + "</table>";
            }


                   

            conn.Close();
        }
        catch (Exception ex) { }*/

        return msg;
    }
    
    
    [WebMethod]
    public static string GetTopSoldProduct(string topsr)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string currentDate = HttpContext.Current.Session["todaysDate"].ToString();

            string query = @"SELECT T1.ITEM_ID,T2.ITEM_NAME,T2.ITEM_GROUP_NAME, (SUM(((T1.ITEM_CTN*T2.FACTOR)+T1.ITEM_QTY)*T1.OUT_PRICE)/1000) TOTAL_AMT FROM
                (SELECT SR_ID,ITEM_ID,ITEM_QTY,ITEM_CTN,OUT_PRICE FROM T_ORDER_DETAIL WHERE ENTRY_DATE=TO_DATE('" + currentDate.Trim() + "','DD/MM/YYYY') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "')) T1, " +
                @"(
                    SELECT TT1.*,TT2.ITEM_GROUP_NAME FROM
                    (SELECT ITEM_ID,ITEM_NAME,FACTOR,ITEM_GROUP FROM T_ITEM WHERE OWN_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "') TT1, "+
                    @"(SELECT * FROM T_ITEM_GROUP) TT2
                    WHERE TT1.ITEM_GROUP=TT2.ITEM_GROUP_ID) T2
                WHERE T1.ITEM_ID=T2.ITEM_ID GROUP BY T1.ITEM_ID,T2.ITEM_NAME,T2.ITEM_GROUP_NAME
                ORDER BY TOTAL_AMT ASC";
        
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            int k = 1;
            string tbl = "<table class='table-striped table-bordered table-hover' style='border-collapse: collapse;width:100%;'>";
            string row = "<tr style='background-color:#ff9800;'><td style='border: 1px solid orange;padding: 5px;text-align: left;'>SL No.</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>Item ID</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>Item Name</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>Group Name</td><td style='border: 1px solid orange;padding: 5px;text-align: left;'>Total Amount</td></tr>";
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string ITEM_ID = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string ITEM_NAME = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    string ITEM_GROUP_NAME = ds.Tables[0].Rows[i]["ITEM_GROUP_NAME"].ToString();
                    string TOTAL_AMT = String.Format("{0:0.00}", Convert.ToDouble(ds.Tables[0].Rows[i]["TOTAL_AMT"].ToString()));

                    row = row + "<tr><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + k.ToString() + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + ITEM_ID + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + ITEM_NAME + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + ITEM_GROUP_NAME + "</td><td style='border: 1px solid #eceeef;padding: 5px;text-align: left;'>" + TOTAL_AMT + "</td></tr>";
                     
                    k++;
                }

                msg = msg + tbl + row + "</table>";
            }            

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
    
    [WebMethod]
    public static string GetTopLowOrderedProduct(string group, string fdate, string tdate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = "";
            if (group == "All")
            {                 
                query = @"SELECT T4.ITEM_GROUP_NAME,T3.* FROM
                        (SELECT T2.*,T1.OUT_PRICE,T1.CARTON,T1.PCS FROM
                        (SELECT ITEM_ID,OUT_PRICE, SUM(ITEM_QTY) PCS,SUM(ITEM_CTN) CARTON FROM T_ORDER_DETAIL 
                        WHERE ENTRY_DATE BETWEEN TO_DATE('" + tdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY')  ";
                query = query + @"AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID IN(SELECT ITEM_GROUP_ID FROM T_ITEM_GROUP WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString() + "' AND STATUS='Y'))  GROUP BY ITEM_ID,OUT_PRICE) T1,  ";
                query = query + @" (SELECT ITEM_ID,ITEM_NAME,FACTOR,ITEM_GROUP  FROM T_ITEM) T2  
                        WHERE T1.ITEM_ID=T2.ITEM_ID
                        ORDER BY CARTON DESC) T3, ";
                query = query + @"(SELECT ITEM_GROUP_NAME,ITEM_GROUP_ID FROM T_ITEM_GROUP WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString() + "' AND STATUS='Y') T4 WHERE T3.ITEM_GROUP=T4.ITEM_GROUP_ID";

            }
            else
            {
                query = @"SELECT T4.ITEM_GROUP_NAME,T3.* FROM
                        (SELECT T2.*,T1.OUT_PRICE,T1.CARTON,T1.PCS FROM
                        (SELECT ITEM_ID,OUT_PRICE, SUM(ITEM_QTY) PCS,SUM(ITEM_CTN) CARTON FROM T_ORDER_DETAIL 
                        WHERE ENTRY_DATE BETWEEN TO_DATE('" + tdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY')  AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID ='" + group + "' AND STATUS='Y')  GROUP BY ITEM_ID,OUT_PRICE) T1, ";
                query = query + @"(SELECT ITEM_ID,ITEM_NAME,FACTOR,ITEM_GROUP  FROM T_ITEM) T2 ";
                query = query + @" WHERE T1.ITEM_ID=T2.ITEM_ID
                        ORDER BY CARTON DESC) T3, ";
                query = query + @"(SELECT ITEM_GROUP_NAME,ITEM_GROUP_ID FROM T_ITEM_GROUP WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString() + "' AND STATUS='Y') T4 WHERE T3.ITEM_GROUP=T4.ITEM_GROUP_ID";
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
                    string ITEM_GROUP_NAME = ds.Tables[0].Rows[i]["ITEM_GROUP_NAME"].ToString();
                    string ITEM_ID = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string ITEM_NAME = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    string OUT_PRICE = ds.Tables[0].Rows[i]["OUT_PRICE"].ToString();
                    string CARTON = ds.Tables[0].Rows[i]["CARTON"].ToString();
                    string PCS = ds.Tables[0].Rows[i]["PCS"].ToString();
                    msg = msg + ";" + ITEM_ID + ";" + ITEM_NAME + ";" + OUT_PRICE + ";" + CARTON + ";" + PCS + ";" + ITEM_GROUP_NAME;
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
    public static string GetNoOrderedProduct(string group, string fdate, string tdate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = "";
            if (group == "All")
            { 
                query = @"SELECT T3.*,T4.CLASS_NAME FROM              
                        (SELECT T2.ITEM_GROUP_NAME,T1.* FROM               
                        (SELECT ITEM_ID,ITEM_NAME,ITEM_GROUP,ITEM_CLASS,FACTOR,FACTOR_CATEGORY FROM T_ITEM 
                        WHERE ITEM_GROUP IN(SELECT ITEM_GROUP_ID FROM T_ITEM_GROUP WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString() + "' AND STATUS='Y') AND ACTIVENESS='Y' ";
                query = query + @"AND ITEM_ID NOT IN(
                        (SELECT DISTINCT ITEM_ID FROM T_ORDER_DETAIL 
                         WHERE ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID IN(SELECT ITEM_GROUP_ID FROM T_ITEM_GROUP WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString() + "' AND STATUS='Y') AND STATUS='Y')))) T1, ";
                query = query + @"(SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME  FROM T_ITEM_GROUP) T2
                         WHERE T1.ITEM_GROUP=T2.ITEM_GROUP_ID) T3,
                         (SELECT CLASS_ID,CLASS_NAME FROM T_ITEM_CLASS) T4
                         WHERE T3.ITEM_CLASS=T4.CLASS_ID ";
            }
            else
            {
                query = @"SELECT T3.*,T4.CLASS_NAME FROM              
                        (SELECT T2.ITEM_GROUP_NAME,T1.* FROM               
                        (SELECT ITEM_ID,ITEM_NAME,ITEM_GROUP,ITEM_CLASS,FACTOR,FACTOR_CATEGORY FROM T_ITEM 
                        WHERE ITEM_GROUP ='" + group + "' AND ACTIVENESS='Y' ";
                query = query + @"AND ITEM_ID NOT IN(
                        (SELECT DISTINCT ITEM_ID FROM T_ORDER_DETAIL 
                         WHERE ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID ='" + group + "' AND STATUS='Y')))) T1, ";
                query = query + @"(SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME  FROM T_ITEM_GROUP) T2
                         WHERE T1.ITEM_GROUP=T2.ITEM_GROUP_ID) T3,
                         (SELECT CLASS_ID,CLASS_NAME FROM T_ITEM_CLASS) T4
                         WHERE T3.ITEM_CLASS=T4.CLASS_ID ";
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
                    string ITEM_GROUP_NAME = ds.Tables[0].Rows[i]["ITEM_GROUP_NAME"].ToString();
                    string ITEM_ID = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string ITEM_NAME = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    string CLASS_NAME = ds.Tables[0].Rows[i]["CLASS_NAME"].ToString();
                    string FACTOR = ds.Tables[0].Rows[i]["FACTOR"].ToString();
                    string FACTOR_CATEGORY = ds.Tables[0].Rows[i]["FACTOR_CATEGORY"].ToString();
                    msg = msg + ";" + ITEM_ID + ";" + ITEM_NAME + ";" + CLASS_NAME + ";" + FACTOR + ";" + FACTOR_CATEGORY + ";" + ITEM_GROUP_NAME;
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
    public static string GetDamageSummaryReport(string fdate, string tdate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
                        
            string qrDamage = @"SELECT T1.*,T2.ITEM_GROUP_NAME FROM
                                (SELECT ITEM_CODE,ITEM_NAME,SUM(CARTON) CTN,SUM(PCS) PIECE,REASON,OUTLET_ID FROM T_DAMAGE 
                                WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate.ToString() + "','DD/MM/YYYY') AND TO_DATE('" + tdate.ToString() + "','DD/MM/YYYY') GROUP BY ITEM_CODE,ITEM_NAME,REASON,OUTLET_ID) T1, " +
                                @"(SELECT T_ITEM.ITEM_ID,T_ITEM_GROUP.ITEM_GROUP_NAME FROM T_ITEM,T_ITEM_GROUP WHERE T_ITEM.ITEM_GROUP=T_ITEM_GROUP.ITEM_GROUP_ID) T2
                                WHERE T1.ITEM_CODE=T2.ITEM_ID";

            OracleCommand cmd = new OracleCommand(qrDamage, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string ITEM_CODE = ds.Tables[0].Rows[i]["ITEM_CODE"].ToString();
                    if (ITEM_CODE.Substring(0, 1) == "M")
                    {
                        ITEM_CODE = ITEM_CODE.Substring(1);
                    }
                    string ITEM_NAME = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    string ITEM_GROUP_NAME = ds.Tables[0].Rows[i]["ITEM_GROUP_NAME"].ToString();
                    string CTN = ds.Tables[0].Rows[i]["CTN"].ToString();
                    string PIECE = ds.Tables[0].Rows[i]["PIECE"].ToString();
                    string REASON = ds.Tables[0].Rows[i]["REASON"].ToString();

                    msg = msg + ";" + ITEM_CODE + ";" + ITEM_NAME + ";" + CTN + ";" + PIECE + ";" + REASON + ";" + ITEM_GROUP_NAME;
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
    public static string GetDamageDetailsReport(string fdate, string tdate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT T9.*,T10.ITEM_GROUP_NAME FROM
                            (SELECT T7.*,T8.DIVISION_NAME FROM
                            (SELECT T5.*,T6.ZONE_NAME,T6.DIVISION_ID FROM
                            (SELECT T3.*,T4.ROUTE_NAME,T4.ZONE_ID FROM
                            (SELECT T1.*,T2.OUTLET_NAME,T2.ROUTE_ID FROM
                            (SELECT ITEM_CODE,ITEM_NAME,SUM(CARTON) CTN,SUM(PCS) PIECE,REASON,OUTLET_ID FROM T_DAMAGE 
                            WHERE SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate.ToString() + "','DD/MM/YYYY') AND TO_DATE('" + tdate.ToString() + "','DD/MM/YYYY') " +
                            @" GROUP BY ITEM_CODE,ITEM_NAME,REASON,OUTLET_ID) T1,
                            (SELECT OUTLET_ID,ROUTE_ID,OUTLET_NAME FROM T_OUTLET) T2
                            WHERE T1.OUTLET_ID=T2.OUTLET_ID) T3,
                            (SELECT ROUTE_ID,ROUTE_NAME,ZONE_ID FROM T_ROUTE) T4
                            WHERE T3.ROUTE_ID=T4.ROUTE_ID) T5,
                            (SELECT ZONE_ID,ZONE_NAME,DIVISION_ID FROM T_ZONE) T6
                            WHERE T5.ZONE_ID=T6.ZONE_ID) T7,
                            (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T8
                            WHERE T7.DIVISION_ID=T8.DIVISION_ID) T9,
                            (SELECT T_ITEM.ITEM_ID,T_ITEM_GROUP.ITEM_GROUP_NAME FROM T_ITEM,T_ITEM_GROUP WHERE T_ITEM.ITEM_GROUP=T_ITEM_GROUP.ITEM_GROUP_ID) T10 
                            WHERE T9.ITEM_CODE=T10.ITEM_ID";

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string ITEM_CODE = ds.Tables[0].Rows[i]["ITEM_CODE"].ToString();
                    if (ITEM_CODE.Substring(0, 1) == "M")
                    {
                        ITEM_CODE = ITEM_CODE.Substring(1);
                    }
                    string ITEM_NAME = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    string ITEM_GROUP_NAME = ds.Tables[0].Rows[i]["ITEM_GROUP_NAME"].ToString();
                    string CTN = ds.Tables[0].Rows[i]["CTN"].ToString();
                    string PIECE = ds.Tables[0].Rows[i]["PIECE"].ToString();
                    string REASON = ds.Tables[0].Rows[i]["REASON"].ToString();
                    string OUTLET_NAME = ds.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                    string ROUTE_NAME = ds.Tables[0].Rows[i]["ROUTE_NAME"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();

                    msg = msg + ";" + ITEM_CODE + ";" + ITEM_NAME + ";" + CTN + ";" + PIECE + ";" + REASON + ";" + OUTLET_NAME + ";" + ROUTE_NAME + ";" + ZONE_NAME + ";" + DIVISION_NAME + ";" + ITEM_GROUP_NAME;
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
    public static string GetOrderedProduct(string fdate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT ITEM_ID,SUM(ITEM_CTN) CARTON,SUM(ITEM_QTY) PIECE FROM T_ORDER_DETAIL 
                            WHERE ENTRY_DATE=TO_DATE('" + fdate.Trim() + "','DD/MM/YYYY') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString().Trim() + "') GROUP BY ITEM_ID";
             

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                     
                    string ITEM_ID = ds.Tables[0].Rows[i]["ITEM_ID"].ToString().Substring(1);
                    string CARTON = ds.Tables[0].Rows[i]["CARTON"].ToString();
                    string PIECE = ds.Tables[0].Rows[i]["PIECE"].ToString();

                    msg = msg + ";" + ITEM_ID + ";" + CARTON + ";" + PIECE;
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
    public static string GetOrderedProductDetails(string fdate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT T3.*,T4.OUTLET_NAME FROM
                            (SELECT T1.*,T2.SR_NAME FROM
                            (SELECT ITEM_ID,SR_ID,OUTLET_ID,ITEM_CTN,ITEM_QTY FROM T_ORDER_DETAIL 
                            WHERE ENTRY_DATE=TO_DATE('" + fdate.Trim() + "','DD/MM/YYYY') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString().Trim() + "')) T1, ";
            query = query + @"(SELECT SR_ID,SR_NAME FROM T_SR_INFO) T2
                            WHERE T1.SR_ID=T2.SR_ID) T3,
                            (SELECT OUTLET_ID,OUTLET_NAME FROM T_OUTLET) T4
                            WHERE T3.OUTLET_ID=T4.OUTLET_ID";

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                     
                    string ITEM_ID = ds.Tables[0].Rows[i]["ITEM_ID"].ToString().Substring(1);
                    string CARTON = ds.Tables[0].Rows[i]["ITEM_CTN"].ToString();
                    string PIECE = ds.Tables[0].Rows[i]["ITEM_QTY"].ToString();

                    string SR_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string SR_NAME = ds.Tables[0].Rows[i]["SR_NAME"].ToString();
                    string OUTLET_NAME = ds.Tables[0].Rows[i]["OUTLET_NAME"].ToString();

                    msg = msg + ";" + ITEM_ID + ";" + CARTON + ";" + PIECE + ";" + SR_ID + ";" + SR_NAME + ";" + OUTLET_NAME;
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
    public static string GetCompanyWiseGroup(string motherCompany)
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


    [WebMethod(EnableSession = true)]
    public static string GetSRWiseOutletInfo(string company)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qrSR = @"SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP FROM T_SR_INFO WHERE SR_ID!='10169' AND STATUS='Y' AND ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "' ORDER BY SR_ID";
            OracleCommand cmd = new OracleCommand(qrSR, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0 && ds.Tables[0].Rows[0]["SR_ID"].ToString() != "") 
            {
                for (int i = 0; i < c; i++)
                {
                    string SR_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string MOBILE_NO = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string SR_NAME = ds.Tables[0].Rows[i]["SR_NAME"].ToString() + " - " + MOBILE_NO;
                    string ITEM_GROUP = ds.Tables[0].Rows[i]["ITEM_GROUP"].ToString();

                    string qR = @"SELECT T3.*,T4.ZONE_NAME FROM
                                (SELECT T2.ZONE_ID,T1.ROUTE_ID,T1.ROUTE_NAME,T2.TOTAL FROM
                                (SELECT ROUTE_ID,ROUTE_NAME FROM T_ROUTE) T1,
                                (SELECT SR_ID,ZONE_ID,ROUTE_ID,COUNT(OUTLET_ID) TOTAL FROM T_OUTLET
                                WHERE SR_ID='" + SR_ID.Trim() + "' AND ROUTE_ID IN(SELECT ROUTE_ID FROM T_SR_ROUTE_DAY WHERE SR_ID='" + SR_ID.Trim() + "') ";
                    qR = qR + @"GROUP BY SR_ID,ZONE_ID,ROUTE_ID ORDER BY TOTAL) T2 WHERE T1.ROUTE_ID=T2.ROUTE_ID) T3,
                                (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE) T4
                                WHERE T3.ZONE_ID=T4.ZONE_ID";
                    
                    OracleCommand cmdR = new OracleCommand(qR, conn);
                    OracleDataAdapter daR = new OracleDataAdapter(cmdR);
                    DataSet dsR = new DataSet();
                    daR.Fill(dsR);
                    int cR = dsR.Tables[0].Rows.Count;
                    if (cR > 0 && dsR.Tables[0].Rows[0][0].ToString() !="")
                    {
                        for (int k = 0; k < cR; k++)
                        {
                            string routeName = dsR.Tables[0].Rows[k]["ROUTE_NAME"].ToString();
                            string zoneName = dsR.Tables[0].Rows[k]["ZONE_NAME"].ToString();
                            string totalOutlet = dsR.Tables[0].Rows[k]["TOTAL"].ToString();

                            msg = msg + ";" + SR_ID + ";" + SR_NAME + ";" + routeName + ";" + totalOutlet + ";" + ITEM_GROUP + ";" + zoneName;
                        }
                    }
                }
            }

            

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod(EnableSession = true)]
    public static string GetUserLogInfo(string currentDate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qrSR = @"SELECT USER_ID,USER_NAME,LOGIN_TIME FROM T_USER_LOG WHERE LOGIN_DATE=TO_DATE('" + currentDate.Trim() + "','DD/MM/YYYY') ORDER BY USER_ID";
            OracleCommand cmd = new OracleCommand(qrSR, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0 && ds.Tables[0].Rows[0]["USER_ID"].ToString() != "")
            {
                string staffID = "";
                string name = "";
                string phone = "";
                string group = "";

                for (int i = 0; i < c; i++)
                {
                    string staffIds = ds.Tables[0].Rows[i]["USER_ID"].ToString();
                    string loginTime = ds.Tables[0].Rows[i]["LOGIN_TIME"].ToString();

                    string qrS = @"SELECT STAFF_ID,HOS_NAME,MOBILE_NO,ITEM_GROUP FROM T_HOS WHERE STAFF_ID='" + staffIds.Trim() + "'";
                    OracleCommand cmdS = new OracleCommand(qrS, conn);
                    OracleDataAdapter daS = new OracleDataAdapter(cmdS);
                    DataSet dsS = new DataSet();
                    daS.Fill(dsS);
                    int cS = dsS.Tables[0].Rows.Count;
                    if (cS > 0 && dsS.Tables[0].Rows[0]["STAFF_ID"].ToString() != "")
                    {
                        staffID = dsS.Tables[0].Rows[0]["STAFF_ID"].ToString();
                        name = dsS.Tables[0].Rows[0]["HOS_NAME"].ToString();
                        phone = dsS.Tables[0].Rows[0]["MOBILE_NO"].ToString();
                        group = "HOS - " + dsS.Tables[0].Rows[0]["ITEM_GROUP"].ToString();

                        msg = msg + ";" + staffID + ";" + name + ";" + phone + ";" + group + ";" + loginTime;
                    }
                    else
                    {
                        string qrSs = @"SELECT RM_ID,RM_NAME,MOBILE_NO,ITEM_GROUP FROM T_AGM_RM WHERE RM_ID='" + staffIds + "'";
                        OracleCommand cmdSs = new OracleCommand(qrSs, conn);
                        OracleDataAdapter daSs = new OracleDataAdapter(cmdSs);
                        DataSet dsSs = new DataSet();
                        daSs.Fill(dsSs);
                        int cSs = dsSs.Tables[0].Rows.Count;
                        if (cSs > 0 && dsSs.Tables[0].Rows[0]["RM_ID"].ToString() != "")
                        {
                            staffID = dsSs.Tables[0].Rows[0]["RM_ID"].ToString();
                            name = dsSs.Tables[0].Rows[0]["RM_NAME"].ToString();
                            phone = dsSs.Tables[0].Rows[0]["MOBILE_NO"].ToString();
                            group = "RM - " + dsSs.Tables[0].Rows[0]["ITEM_GROUP"].ToString();

                            msg = msg + ";" + staffID + ";" + name + ";" + phone + ";" + group + ";" + loginTime;
                        }
                        else
                        {
                            string qrSsr = @"SELECT T1.*,T2.ZONE_NAME FROM
                                            (SELECT TSM_ID,TSM_NAME,MOBILE_NO,ITEM_GROUP,ZONE_ID FROM T_TSM_ZM WHERE TSM_ID='" + staffIds + "') T1, ";
                            qrSsr = qrSsr + @"(SELECT ZONE_ID,ZONE_NAME FROM T_ZONE) T2
                                            WHERE T1.ZONE_ID=T2.ZONE_ID";

                            OracleCommand cmdSsr = new OracleCommand(qrSsr, conn);
                            OracleDataAdapter daSsr = new OracleDataAdapter(cmdSsr);
                            DataSet dsSsr = new DataSet();
                            daSsr.Fill(dsSsr);
                            int cSsr = dsSsr.Tables[0].Rows.Count;
                            if (cSsr > 0 && dsSsr.Tables[0].Rows[0]["TSM_ID"].ToString() != "")
                            {
                                staffID = dsSsr.Tables[0].Rows[0]["TSM_ID"].ToString();
                                name = dsSsr.Tables[0].Rows[0]["TSM_NAME"].ToString();
                                phone = dsSsr.Tables[0].Rows[0]["MOBILE_NO"].ToString();
                                group = "TSM - " + dsSsr.Tables[0].Rows[0]["ITEM_GROUP"].ToString() + " - " + dsSsr.Tables[0].Rows[0]["ZONE_NAME"].ToString();

                                msg = msg + ";" + staffID + ";" + name + ";" + phone + ";" + group + ";" + loginTime;
                            }
                            else
                            {
                                string qrSr = @"SELECT T1.*,T2.ZONE_NAME FROM
                                                (SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP,DIST_ZONE FROM T_SR_INFO WHERE SR_ID='" + staffIds + "') T1, ";
                                qrSr = qrSr + @"(SELECT ZONE_ID,ZONE_NAME FROM T_ZONE) T2
                                                WHERE T1.DIST_ZONE=T2.ZONE_ID";

                                OracleCommand cmdSr = new OracleCommand(qrSr, conn);
                                OracleDataAdapter daSr = new OracleDataAdapter(cmdSr);
                                DataSet dsSr = new DataSet();
                                daSr.Fill(dsSr);
                                int cSr = dsSr.Tables[0].Rows.Count;
                                if (cSr > 0 && dsSr.Tables[0].Rows[0]["SR_ID"].ToString() != "")
                                {
                                    staffID = dsSr.Tables[0].Rows[0]["SR_ID"].ToString();
                                    name = dsSr.Tables[0].Rows[0]["SR_NAME"].ToString();
                                    phone = dsSr.Tables[0].Rows[0]["MOBILE_NO"].ToString();
                                    group = "SR - " + dsSr.Tables[0].Rows[0]["ITEM_GROUP"].ToString() + " - " + dsSsr.Tables[0].Rows[0]["ZONE_NAME"].ToString();

                                    msg = msg + ";" + staffID + ";" + name + ";" + phone + ";" + group + ";" + loginTime;
                                }
                                else
                                {
                                    name = ds.Tables[0].Rows[i]["USER_NAME"].ToString();
                                    phone = "019";
                                    group = "Admin Level";
                                    msg = msg + ";" + staffIds + ";" + name + ";" + phone + ";" + group + ";" + loginTime;
                                }
                            }
                        }
                    } 
                   
                }
            }
            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }

    [WebMethod(EnableSession = true)]
    public static string GetSampleInfo(string currentDate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qrSR = @"SELECT T9.*,T10.ZONE_NAME FROM
                            (SELECT T7.*,T8.ROUTE_NAME,T8.ZONE_ID FROM
                            (SELECT T5.*,T6.OUTLET_NAME FROM
                            (SELECT T3.*,T4.ITEM_NAME,(((T3.ITEM_CTN*T4.FACTOR) + T3.ITEM_PCS) * T3.ITEM_PRICE) TOTAL_AMT FROM
                            (SELECT T1.*,T2.SR_INFO FROM
                            (SELECT SR_ID,OUTLET_ID,ROUTE_ID,ITEM_ID,ITEM_CTN,ITEM_PCS,ITEM_PRICE FROM T_SAMPLE WHERE ENTRY_DATE=TO_DATE('" + currentDate.Trim().ToString() + "','DD/MM/YYYY')) T1, (SELECT SR_ID,SR_NAME ||'-'||MOBILE_NO SR_INFO FROM T_SR_INFO WHERE ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "') T2 ";
            qrSR = qrSR + @"WHERE T1.SR_ID=T2.SR_ID) T3,
                            (SELECT ITEM_ID,FACTOR,ITEM_NAME FROM T_ITEM) T4
                            WHERE T3.ITEM_ID=T4.ITEM_ID) T5,
                            (SELECT OUTLET_ID,OUTLET_NAME FROM T_OUTLET) T6
                            WHERE T5.OUTLET_ID=T6.OUTLET_ID) T7,
                            (SELECT ROUTE_ID,ROUTE_NAME,ZONE_ID FROM T_ROUTE) T8
                            WHERE T7.ROUTE_ID=T8.ROUTE_ID) T9,
                            (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE) T10
                            WHERE T9.ZONE_ID=T10.ZONE_ID";

            OracleCommand cmd = new OracleCommand(qrSR, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0 && ds.Tables[0].Rows[0]["SR_ID"].ToString() != "")
            {
                for (int i = 0; i < c; i++)
                {
                    string SR_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string SR_INFO = ds.Tables[0].Rows[i]["SR_INFO"].ToString();
                    string ITEM_ID = ds.Tables[0].Rows[i]["ITEM_ID"].ToString().Substring(1);
                    string ITEM_NAME = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    string ITEM_CTN = ds.Tables[0].Rows[i]["ITEM_CTN"].ToString();
                    string ITEM_PCS = ds.Tables[0].Rows[i]["ITEM_PCS"].ToString();
                    string ITEM_PRICE = ds.Tables[0].Rows[i]["ITEM_PRICE"].ToString();
                    string TOTAL_AMT = ds.Tables[0].Rows[i]["TOTAL_AMT"].ToString();
                    string OUTLET_NAME = ds.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                    string ROUTE_NAME = ds.Tables[0].Rows[i]["ROUTE_NAME"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                    
                    msg = msg + ";" + SR_ID + ";" + SR_INFO + ";" + ITEM_ID + ";" + ITEM_NAME + ";" + ITEM_CTN + ";" + ITEM_PCS + ";" + ITEM_PRICE + ";" + TOTAL_AMT + ";" + OUTLET_NAME + ";" + ROUTE_NAME + ";" + ZONE_NAME;
                }
            }
            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
    
    [WebMethod]
    public static string GetDeliveryReport(string company, string fdate, string tdate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT T9.*,(((T9.ITEM_CTN*T9.FACTOR)+T9.ITEM_QTY)*T9.OUT_PRICE) TOTAL_ORDER_AMT, (((T9.DELIVERY_CTN*T9.FACTOR)+T9.DELIVERY_PCS)*T9.OUT_PRICE) TOTAL_DELIVERY_AMT FROM
                            (SELECT T7.*,T8.ROUTE_NAME FROM
                            (SELECT T5.*,T6.ITEM_NAME,T6.FACTOR FROM  
                            (SELECT T3.*,T4.OUTLET_NAME FROM
                             (SELECT T1.*,T2.SR_NAME,T2.ITEM_GROUP FROM 
                             (SELECT TRAN_ID,SR_ID,ITEM_ID,OUT_PRICE,OUTLET_ID,ROUTE_ID,DELIVERY_CTN,DELIVERY_PCS,ITEM_CTN,ITEM_QTY FROM T_ORDER_DETAIL
                             WHERE IS_PROCESS='S' AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_COMPANY='" + HttpContext.Current.Session["company"].ToString() + "' AND STATUS='Y') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') ";
            query = query + @" ORDER BY OUTLET_ID) T1,
                             (SELECT SR_ID,SR_NAME,ITEM_GROUP FROM T_SR_INFO) T2
                             WHERE T1.SR_ID=T2.SR_ID) T3,
                             (SELECT OUTLET_ID,OUTLET_NAME FROM T_OUTLET) T4
                             WHERE T3.OUTLET_ID=T4.OUTLET_ID) T5,
                             (SELECT ITEM_ID,ITEM_NAME,FACTOR FROM T_ITEM) T6
                             WHERE T5.ITEM_ID=T6.ITEM_ID) T7,
                             (SELECT ROUTE_ID,ROUTE_NAME FROM T_ROUTE) T8
                             WHERE T7.ROUTE_ID=T8.ROUTE_ID) T9";

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string TRAN_ID = ds.Tables[0].Rows[i]["TRAN_ID"].ToString();
                    string SR_NAME = ds.Tables[0].Rows[i]["SR_NAME"].ToString();
                    string OUTLET_NAME = ds.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                    string ROUTE_NAME = ds.Tables[0].Rows[i]["ROUTE_NAME"].ToString();
                    string ITEM_NAME = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    string DELIVERY_CTN = ds.Tables[0].Rows[i]["DELIVERY_CTN"].ToString();
                    string DELIVERY_PCS = ds.Tables[0].Rows[i]["DELIVERY_PCS"].ToString();
                    string OUT_PRICE = ds.Tables[0].Rows[i]["OUT_PRICE"].ToString();
                    string TOTAL_DELIVERY_AMT = ds.Tables[0].Rows[i]["TOTAL_DELIVERY_AMT"].ToString();
                    string TOTAL_ORDER_AMT = ds.Tables[0].Rows[i]["TOTAL_ORDER_AMT"].ToString();

                    msg = msg + ";" + TRAN_ID + ";" + SR_NAME + ";" + ROUTE_NAME + ";" + OUTLET_NAME + ";" + ITEM_NAME + ";" + DELIVERY_CTN + ";" + DELIVERY_PCS + ";" + OUT_PRICE + ";" + TOTAL_DELIVERY_AMT + ";" + TOTAL_ORDER_AMT;
                    
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
    public static string GetOrderVSDeliveryReport(string fdate, string tdate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qrGrp = @"SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString() + "' AND STATUS='Y'";
            OracleCommand cmdG = new OracleCommand(qrGrp, conn);
            OracleDataAdapter daG = new OracleDataAdapter(cmdG);
            DataSet dsG = new DataSet();
            daG.Fill(dsG);
            int cG = dsG.Tables[0].Rows.Count;
            if (cG > 0 && dsG.Tables[0].Rows[0]["ITEM_GROUP_ID"].ToString() != "")
            {
                for (int g = 0; g < cG; g++)
                {
                    string ITEM_GROUP_ID = dsG.Tables[0].Rows[g]["ITEM_GROUP_ID"].ToString();
                    string ITEM_GROUP_NAME = dsG.Tables[0].Rows[g]["ITEM_GROUP_NAME"].ToString();

                    string query = @"SELECT SUM(T10.TOTAL_ORDER_AMT) ORDER_AMOUNT,SUM(T10.TOTAL_DELIVERY_AMT) DELIVERY_AMOUNT FROM
                            (SELECT T9.TRAN_ID, SUM(((T9.ITEM_CTN*T9.FACTOR)+T9.ITEM_QTY)*T9.OUT_PRICE) TOTAL_ORDER_AMT, SUM(((T9.DELIVERY_CTN*T9.FACTOR)+T9.DELIVERY_PCS)*T9.OUT_PRICE) TOTAL_DELIVERY_AMT FROM
                            (SELECT T7.*,T8.ROUTE_NAME FROM
                            (SELECT T5.*,T6.ITEM_NAME,T6.FACTOR FROM  
                            (SELECT T3.*,T4.OUTLET_NAME FROM
                             (SELECT T1.*,T2.SR_NAME,T2.ITEM_GROUP FROM 
                             (SELECT TRAN_ID,SR_ID,ITEM_ID,OUT_PRICE,OUTLET_ID,ROUTE_ID,DELIVERY_CTN,DELIVERY_PCS,ITEM_CTN,ITEM_QTY FROM T_ORDER_DETAIL
                             WHERE IS_PROCESS='S' AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + ITEM_GROUP_ID + "' AND STATUS='Y') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY')  ";
                    query = query + @"ORDER BY OUTLET_ID) T1,
                             (SELECT SR_ID,SR_NAME,ITEM_GROUP FROM T_SR_INFO) T2
                             WHERE T1.SR_ID=T2.SR_ID) T3,
                             (SELECT OUTLET_ID,OUTLET_NAME FROM T_OUTLET) T4
                             WHERE T3.OUTLET_ID=T4.OUTLET_ID) T5,
                             (SELECT ITEM_ID,ITEM_NAME,FACTOR FROM T_ITEM) T6
                             WHERE T5.ITEM_ID=T6.ITEM_ID) T7,
                             (SELECT ROUTE_ID,ROUTE_NAME FROM T_ROUTE) T8
                             WHERE T7.ROUTE_ID=T8.ROUTE_ID) T9                             
                             GROUP BY T9.TRAN_ID) T10";

                    string TOTAL_DELIVERY_AMT = "";
                    string TOTAL_ORDER_AMT = "";

                    OracleCommand cmd = new OracleCommand(query, conn);
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    int c = ds.Tables[0].Rows.Count;
                    if (c > 0)
                    {
                        for (int i = 0; i < c; i++)
                        {
                            TOTAL_DELIVERY_AMT = ds.Tables[0].Rows[i]["DELIVERY_AMOUNT"].ToString();
                            TOTAL_ORDER_AMT = ds.Tables[0].Rows[i]["ORDER_AMOUNT"].ToString();

                            msg = msg + ";" + ITEM_GROUP_NAME + ";" +TOTAL_DELIVERY_AMT + ";" + TOTAL_ORDER_AMT;

                        }

                    }
                    else
                    {
                        TOTAL_DELIVERY_AMT = "0.00";
                        TOTAL_ORDER_AMT = "0.00";
                        msg = msg + ";" + ITEM_GROUP_NAME + ";" + TOTAL_DELIVERY_AMT + ";" + TOTAL_ORDER_AMT;
                    }
                }
            }

            

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }

    [WebMethod]
    public static string GetStockReport(string depotId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = "";
            if (depotId == "All")
            {
                query = @"SELECT DEPOT_NAME,ITEM_ID,ITEM_NAME,GROUP_NAME,CARTON_QTY,PCS_QTY FROM T_DEPOT_STOCK ORDER BY DEPOT_NAME";
            }
            else
            {
                query = @"SELECT DEPOT_NAME,ITEM_ID,ITEM_NAME,GROUP_NAME,CARTON_QTY,PCS_QTY FROM T_DEPOT_STOCK WHERE DEPOT_ID='" + depotId + "' ORDER BY ITEM_NAME";
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
                    string DEPOT_NAME = ds.Tables[0].Rows[i]["DEPOT_NAME"].ToString();
                    string ITEM_ID = ds.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string ITEM_NAME = ds.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    string GROUP_NAME = ds.Tables[0].Rows[i]["GROUP_NAME"].ToString();
                    string CARTON_QTY = ds.Tables[0].Rows[i]["CARTON_QTY"].ToString();
                    string PCS_QTY = ds.Tables[0].Rows[i]["PCS_QTY"].ToString();


                    msg = msg + ";" + DEPOT_NAME + ";" + ITEM_ID + ";" + ITEM_NAME + ";" + GROUP_NAME + ";" + CARTON_QTY + ";" + PCS_QTY;

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
    public static string GetCountrywiseDepot(string motherCompany)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT DIST_ID,DIST_NAME FROM T_DISTRIBUTOR WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString() + "%'";

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
                    string DIST_NAME = ds.Tables[0].Rows[i]["DIST_NAME"].ToString();
                    
                    msg = msg + ";" + DIST_ID + ";" + DIST_NAME;
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
    public static string GetOutletSKU(string groupId, string fdate, string tdate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT T5.SR_ID,T6.SR_NAME,T6.MOBILE_NO,T5.OUTLET_NAME,T5.SKU,T5.ROUTE_NAME,T5.ZONE_NAME FROM
                            (SELECT T3.*,T4.OUTLET_NAME FROM
                            (SELECT T1.*,T2.ROUTE_NAME,T2.ZONE_NAME FROM
                            (SELECT SR_ID,OUTLET_ID,ROUTE_ID,COUNT(ITEM_ID) SKU FROM T_ORDER_DETAIL
                            WHERE ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY SR_ID,OUTLET_ID,ROUTE_ID) T1, ";
            query = query + @"(SELECT TBL1.ROUTE_ID,TBL1.ROUTE_NAME,TBL2.ZONE_NAME FROM
                            (SELECT ROUTE_ID,ROUTE_NAME,ZONE_ID FROM T_ROUTE WHERE STATUS='Y') TBL1,
                            (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE WHERE STATUS='Y') TBL2
                            WHERE TBL1.ZONE_ID=TBL2.ZONE_ID) T2
                            WHERE T1.ROUTE_ID=T2.ROUTE_ID) T3,
                            (SELECT OUTLET_ID,OUTLET_NAME FROM T_OUTLET) T4
                            WHERE T3.OUTLET_ID=T4.OUTLET_ID) T5,
                            (SELECT SR_ID,SR_NAME,MOBILE_NO FROM T_SR_INFO WHERE STATUS='Y' AND ITEM_GROUP_ID='" + groupId + "') T6 WHERE T5.SR_ID=T6.SR_ID";

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string SR_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string SR_NAME = ds.Tables[0].Rows[i]["SR_NAME"].ToString();
                    string MOBILE_NO = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string OUTLET_NAME = ds.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                    string SKU = ds.Tables[0].Rows[i]["SKU"].ToString();
                    string ROUTE_NAME = ds.Tables[0].Rows[i]["ROUTE_NAME"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();

                    msg = msg + ";" + SR_ID + ";" + SR_NAME + ";" + MOBILE_NO + ";" + OUTLET_NAME + ";" + SKU + ";" + ROUTE_NAME + ";" + ZONE_NAME;
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
    public static string GetSRAttendance(string groupId, string fdate, string tdate)
    {
        string msg = "";
         

        try
        {
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());
            
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            try
            {
                int m = DateTime.Now.Month;
                int fm = m - 1;
                string tm = m.ToString().Length == 1 ? "0" + m.ToString() : m.ToString();
                //string fromDate = "21/" + fm.ToString() + "/" + DateTime.Now.Year.ToString();
                //string toDate = "20/" + tm + "/" + DateTime.Now.Year.ToString();

                string fromDate = fdate;
                string toDate = tdate;              
                                
                string qrSr = @"SELECT T9.*, T11.TOTAL_AMT,T11.AVG_AMT FROM                            
                                (SELECT T7.*,T8.TOTAL_MEMO,T8.AVG_MEMO FROM
                                (SELECT T5.*,T6.AVG_TIME FROM
                                (SELECT T3.*,T4.ZONE_NAME,T4.DIVISION_NAME FROM
                                ((SELECT T1.SR_ID,T1.SR_NAME,T1.MOBILE_NO,T1.DIST_ZONE,(T2.COMPANY_NICK_NAME||'('||T1.ITEM_GROUP||')') SR_GROUP FROM (SELECT SR_ID,SR_NAME,MOBILE_NO,DIST_ZONE,ITEM_GROUP,ITEM_COMPANY,ITEM_GROUP_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y')T1, ";
                qrSr = qrSr + @" (SELECT COMPANY_NICK_NAME,COMPANY_ID FROM T_COMPANY)T2 WHERE T1.ITEM_COMPANY=T2.COMPANY_ID)) T3,
                                (SELECT t.ZONE_ID,t.ZONE_NAME,tt.DIVISION_NAME FROM
                                (SELECT ZONE_ID,ZONE_NAME,DIVISION_ID FROM T_ZONE) t,
                                (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) tt
                                WHERE t.DIVISION_ID=tt.DIVISION_ID) T4
                                WHERE T3.DIST_ZONE=T4.ZONE_ID) T5,
                                (SELECT SR_ID,to_char(ceil(avg((a_date-trunc(a_date))*24))-1) ||':' || to_char ( (ceil(avg((a_date-trunc(a_date))*1440))) - ((ceil(avg((a_date-trunc(a_date))*24))-1)*60)) Avg_Time
                                FROM
                                (SELECT SR_ID ,to_date(ENTRY_DATETIME),MIN(ENTRY_DATETIME) a_date,MIN(to_char(ENTRY_DATETIME, 'HH24:MI:SS' ))INTIME FROM T_ORDER_DETAIL
                                WHERE ENTRY_DATE between TO_DATE('" + fdate + "','DD/MM/YYYY') and TO_DATE('" + tdate + "','DD/MM/YYYY') AND SR_ID IN(SELECT DISTINCT SR_ID FROM T_SR_INFO WHERE STATUS='Y' AND ITEM_GROUP_ID='" + groupId + "') ";
                       qrSr = qrSr + @" GROUP BY SR_ID,to_date(ENTRY_DATETIME) order by SR_ID) GROUP BY SR_ID) T6
                                WHERE T5.SR_ID=T6.SR_ID) T7,

                                (SELECT SR_ID,COUNT(OUTLET_ID)TOTAL_MEMO,ROUND(COUNT(OUTLET_ID)/(SELECT (TO_DATE('" + tdate + "','DD/MM/YYYY') - TO_DATE('" + fdate + "','DD/MM/YYYY'))+1 DAYS FROM DUAL))AVG_MEMO FROM ";
                       qrSr = qrSr + @" (SELECT DISTINCT SR_ID,OUTLET_ID,ENTRY_DATE FROM T_ORDER_DETAIL
                                WHERE ENTRY_DATE between TO_DATE('" + fdate + "','DD/MM/YYYY') and TO_DATE('" + tdate + "','DD/MM/YYYY') AND SR_ID IN(SELECT DISTINCT SR_ID FROM T_SR_INFO WHERE STATUS='Y' AND ITEM_GROUP_ID='" + groupId + "') ";
                       qrSr = qrSr + @" ORDER BY ENTRY_DATE) TBL1 GROUP BY SR_ID) T8
                                WHERE T7.SR_ID=T8.SR_ID) T9,

                                (SELECT T10.SR_ID ,TO_CHAR(SUM(T10.AMT)/1000,'999999999.999') TOTAL_AMT, TO_CHAR((SUM(T10.AMT)/(SELECT (TO_DATE('" + tdate + "','DD/MM/YYYY') - TO_DATE('" + fdate + "','DD/MM/YYYY'))+1 DAYS FROM DUAL))/1000,'999999999.999') AVG_AMT FROM ";
                qrSr = qrSr + @"(SELECT TT1.SR_ID,TT1.OUTLET_ID,TT1.ENTRY_DATE,SUM(((TT1.ITEM_CTN*TT2.FACTOR) + TT1.ITEM_QTY)*OUT_PRICE) AMT FROM
                                (SELECT DISTINCT ITEM_ID,SR_ID, OUTLET_ID,ITEM_QTY,ITEM_CTN,OUT_PRICE,ENTRY_DATE FROM T_ORDER_DETAIL
                                WHERE ENTRY_DATE between TO_DATE('" + fdate + "','DD/MM/YYYY') and TO_DATE('" + tdate + "','DD/MM/YYYY')) TT1, (SELECT ITEM_ID,FACTOR FROM T_ITEM WHERE ITEM_GROUP='" + groupId + "') TT2 ";
                qrSr = qrSr + @" WHERE TT1.ITEM_ID=TT2.ITEM_ID
                                GROUP BY TT1.SR_ID,TT1.OUTLET_ID,TT1.ENTRY_DATE ORDER BY TT1.ENTRY_DATE) T10
                                GROUP BY T10.SR_ID) T11
                                WHERE T9.SR_ID=T11.SR_ID";

                OracleCommand cmdP = new OracleCommand(qrSr, con);
                OracleDataAdapter daP = new OracleDataAdapter(cmdP);
                DataSet dsP = new DataSet();
                daP.Fill(dsP);
                int c = dsP.Tables[0].Rows.Count;
                if (c > 0 && dsP.Tables[0].Rows[0][0].ToString() != "")
                {
                    for (int i = 0; i < c; i++)
                    {
                        string srId = dsP.Tables[0].Rows[i]["SR_ID"].ToString();
                        string company = dsP.Tables[0].Rows[i]["SR_GROUP"].ToString();
                        //string group = dsP.Tables[0].Rows[i][5].ToString();
                        string totalAmt = dsP.Tables[0].Rows[i]["TOTAL_AMT"].ToString();
                        string avgAmt = dsP.Tables[0].Rows[i]["AVG_AMT"].ToString();
                        string totalMemo = dsP.Tables[0].Rows[i]["TOTAL_MEMO"].ToString();
                        string avgMemo = dsP.Tables[0].Rows[i]["AVG_MEMO"].ToString();
                        string intime = dsP.Tables[0].Rows[i]["AVG_TIME"].ToString();
                        string srName = dsP.Tables[0].Rows[i]["SR_NAME"].ToString() + "(" + srId + ")";
                        string srMobileNo = dsP.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                        string zoneName = dsP.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                        string divisionName = dsP.Tables[0].Rows[i]["DIVISION_NAME"].ToString();

                        string[] ttm = intime.Split(':');
                        if (ttm[0].Length == 1 || Convert.ToInt32(ttm[0]) < 12)
                        {
                            intime = intime + " AM";
                        }
                        else
                        {
                            intime = intime + " PM";
                        }

                        string leave = "0";
                        string lwp = "0";


                        string qrLv = @"SELECT SR_ID FROM T_LEAVE
                                       WHERE E_DATE BETWEEN TO_DATE('" + fromDate + "','DD/MM/YYYY') AND TO_DATE('" + toDate + "','DD/MM/YYYY') AND SR_ID='" + srId + "'";

                        OracleCommand cmdLv = new OracleCommand(qrLv, con);
                        OracleDataAdapter daLv = new OracleDataAdapter(cmdLv);
                        DataSet dsLv = new DataSet();
                        daLv.Fill(dsLv);
                        int lv = dsLv.Tables[0].Rows.Count;
                        if (lv > 0 && dsLv.Tables[0].Rows[0][0].ToString() != "")
                        {
                            leave = lv.ToString();
                        }

                        string qrLwp = @"SELECT SR_ID,COUNT(SR_ID)LWP FROM T_LWP
                                        WHERE ENTRY_DATE BETWEEN TO_DATE('" + fromDate + "','DD/MM/YYYY') AND TO_DATE('" + toDate + "','DD/MM/YYYY') AND SR_ID='" + srId + "' GROUP BY SR_ID";

                        OracleCommand cmdLwp = new OracleCommand(qrLwp, con);
                        OracleDataAdapter daLwp = new OracleDataAdapter(cmdLwp);
                        DataSet dsLwp = new DataSet();
                        daLwp.Fill(dsLwp);
                        int lp = dsLwp.Tables[0].Rows.Count;
                        if (lp > 0 && dsLwp.Tables[0].Rows[0][0].ToString() != "")
                        {
                            lwp = dsLwp.Tables[0].Rows[0][1].ToString();
                        }

                        
                        //------lpc---------
                        string lpcLine = "0";
                        string qrLpc = @"SELECT ITEM_ID FROM T_ORDER_DETAIL
                                        WHERE SR_ID='" + srId + "' AND ENTRY_DATE BETWEEN TO_DATE('" + fromDate + "','DD/MM/YYYY') AND TO_DATE('" + toDate + "','DD/MM/YYYY')";

                        OracleCommand cmdLpc = new OracleCommand(qrLpc, con);
                        OracleDataAdapter daLpc = new OracleDataAdapter(cmdLpc);
                        DataSet dsLpc = new DataSet();
                        daLpc.Fill(dsLpc);
                        int lpn = dsLpc.Tables[0].Rows.Count;
                        if (lpn > 0 && dsLpc.Tables[0].Rows[0][0].ToString() != "")
                        {
                            lpcLine = lpn.ToString();
                        }

                        double lpcL = Convert.ToDouble(lpcLine);
                        double tMo = Convert.ToDouble(totalMemo);
                        string lpc = TotalAmount(lpcL / tMo);

                        string visitedOutlet = "0";

                        string qrVolt = @"SELECT DISTINCT OUTLET_ID FROM T_ORDER_DETAIL
                                        WHERE OUTLET_ID IN(
                                                            SELECT OUTLET_ID FROM T_OUTLET
                                                            WHERE STATUS='Y'
                                                            AND SR_ID='" + srId + "') ";
                        qrVolt = qrVolt + @" UNION 
                                        SELECT DISTINCT OUTLET_ID FROM T_NON_PRODUCTIVE_SALES
                                        WHERE SR_ID='" + srId + "' AND ENTRY_DATE BETWEEN TO_DATE('" + fromDate + "','DD/MM/YYYY') AND TO_DATE('" + toDate + "','DD/MM/YYYY')";
 
                        OracleCommand cmdV = new OracleCommand(qrVolt, con);
                        OracleDataAdapter daV = new OracleDataAdapter(cmdV);
                        DataSet dsV = new DataSet();
                        daV.Fill(dsV);
                        int v = dsV.Tables[0].Rows.Count;
                        if (v > 0 && dsV.Tables[0].Rows[0][0].ToString() != "")
                        {
                            visitedOutlet = v.ToString();
                        }

                        string totalOutlet = "0";
                        string qrTolt = @"SELECT OUTLET_ID FROM T_OUTLET WHERE STATUS='Y' AND SR_ID='" + srId + "' AND ROUTE_ID IN(SELECT DISTINCT ROUTE_ID FROM T_ORDER_DETAIL WHERE SR_ID='" + srId + "' AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY'))";

                        OracleCommand cmdTt = new OracleCommand(qrTolt, con);
                        OracleDataAdapter daTt = new OracleDataAdapter(cmdTt);
                        DataSet dsTt = new DataSet();
                        daTt.Fill(dsTt);
                        int tt = dsTt.Tables[0].Rows.Count;
                        if (tt > 0 && dsTt.Tables[0].Rows[0][0].ToString() != "")
                        {
                            totalOutlet = tt.ToString();
                        }


                        int tOlt = Convert.ToInt32(totalOutlet);
                        int tVolt = Convert.ToInt32(visitedOutlet);
                        if (tVolt > tOlt)
                        {
                            tVolt = tOlt;
                            visitedOutlet = tOlt.ToString();
                        }

                        double avgOlt = ((double)tVolt / (double)tOlt);
                        //string vOltPcnt = visitedOutlet + " of " + totalOutlet + "(" + DisplayPercentage(avgOlt) + ")";
                        string vOltPcnt = DisplayPercentage(avgOlt);

                        //msg = msg + ";" + srName + "(" + srId + ")" + ";" + srMobileNo + ";" + totalAmt + ";" + avgAmt + ";" + totalMemo + ";" + avgMemo + ";" + leave + ";" + lwp + ";" + company + "(" + group + ");" + srZone + ";" + vOltPcnt + ";" + intime;
                        msg = msg + ";" + srName + ";" + srMobileNo + ";" + totalAmt + ";" + avgAmt + ";" + totalMemo + ";" + avgMemo + ";" + leave + ";" + lwp + ";" + company + ";" + zoneName + ";" + vOltPcnt + ";" + intime + ";" + lpc + ";" + totalOutlet + ";" + visitedOutlet + ";" + divisionName;

                    }
                }
            }
            catch (Exception ex) { }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod]
    public static string GetRouteWiseSoldProducts(string groupId, string fdate, string tdate)
    {
        string msg = "";

        try
        {
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            string qrSr = @"SELECT T3.*,T4.ZONE_NAME,T4.DIVISION_NAME FROM
                            ((SELECT T1.SR_ID,T1.SR_NAME,T1.MOBILE_NO,T1.DIST_ZONE,(T2.COMPANY_NICK_NAME||'('||T1.ITEM_GROUP||')') SR_GROUP FROM (SELECT SR_ID,SR_NAME,MOBILE_NO,DIST_ZONE,ITEM_GROUP,ITEM_COMPANY,ITEM_GROUP_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y')T1, ";
            qrSr = qrSr + @"(SELECT COMPANY_NICK_NAME,COMPANY_ID FROM T_COMPANY)T2 WHERE T1.ITEM_COMPANY=T2.COMPANY_ID)) T3,
                            (SELECT t.ZONE_ID,t.ZONE_NAME,tt.DIVISION_NAME FROM
                            (SELECT ZONE_ID,ZONE_NAME,DIVISION_ID FROM T_ZONE) t,
                            (SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) tt
                            WHERE t.DIVISION_ID=tt.DIVISION_ID) T4
                            WHERE T3.DIST_ZONE=T4.ZONE_ID";

            OracleCommand cmdP = new OracleCommand(qrSr, con);
            OracleDataAdapter daP = new OracleDataAdapter(cmdP);
            DataSet dsP = new DataSet();
            daP.Fill(dsP);
            int c = dsP.Tables[0].Rows.Count;
            if (c > 0 && dsP.Tables[0].Rows[0][0].ToString() != "")
            {
                for (int i = 0; i < c; i++)
                {
                    double totalNetAmount = 0;

                    string srId = dsP.Tables[0].Rows[i]["SR_ID"].ToString();
                    string srName = dsP.Tables[0].Rows[i]["SR_NAME"].ToString() + "(" + srId + ")";
                    string srPhone = dsP.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    srPhone = srPhone == "" ? "No Cell Number" : srPhone;
                    string srGroup = dsP.Tables[0].Rows[i]["SR_GROUP"].ToString();
                    srGroup = srGroup == "" ? "No Group" : srGroup;
                    string company = dsP.Tables[0].Rows[i]["SR_GROUP"].ToString();
                    string divisionName = dsP.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string zoneName = dsP.Tables[0].Rows[i]["ZONE_NAME"].ToString();

                    string srTsm = "No Tsm Assigned";                     
                    string qrTsm = @"SELECT TSM_NAME || '(' || TSM_ID || ')' TSM_INFO FROM T_TSM_ZM
                                    WHERE TSM_ID IN(SELECT TSM_ID FROM T_TSM_SR WHERE SR_ID='" + srId + "')";

                    OracleCommand cmdT = new OracleCommand(qrTsm, con);
                    OracleDataAdapter daT = new OracleDataAdapter(cmdT);
                    DataSet dsT = new DataSet();
                    daT.Fill(dsT);
                    int t = dsT.Tables[0].Rows.Count;
                    if (t > 0 && dsT.Tables[0].Rows[0][0].ToString() != "")
                    {
                        srTsm = dsT.Tables[0].Rows[0]["TSM_INFO"].ToString();                         
                    }

                    string qrProduct = @"SELECT TT3.*,TT4.OUTLET_NAME,TT4.ROUTE_NAME FROM
                                        (SELECT TT1.SR_ID,TT2.ITEM_ID,TT2.ITEM_NAME,TT1.OUTLET_ID,TT1.ITEM_CTN,TT1.ITEM_QTY,TT1.OUT_PRICE,SUM(((TT1.ITEM_CTN*TT2.FACTOR) + TT1.ITEM_QTY)*OUT_PRICE) AMT FROM
                                        (SELECT DISTINCT ITEM_ID,SR_ID, OUTLET_ID,ITEM_QTY,ITEM_CTN,OUT_PRICE,ENTRY_DATE FROM T_ORDER_DETAIL
                                        WHERE SR_ID='" + srId + "' AND ENTRY_DATE between TO_DATE('" + fdate + "','DD/MM/YYYY') and TO_DATE('" + tdate + "','DD/MM/YYYY') ORDER BY ENTRY_DATETIME ASC) TT1, (SELECT ITEM_ID,ITEM_NAME,FACTOR FROM T_ITEM WHERE ITEM_GROUP='" + groupId + "') TT2 ";
                    qrProduct = qrProduct + @"WHERE TT1.ITEM_ID=TT2.ITEM_ID
                                        GROUP BY TT1.SR_ID,TT2.ITEM_ID,TT2.ITEM_NAME,TT1.OUTLET_ID,TT1.ITEM_CTN,TT1.ITEM_QTY,TT1.OUT_PRICE) TT3, 
                                        (SELECT T1.OUTLET_ID,T1.OUTLET_NAME,T2.ROUTE_NAME FROM T_OUTLET T1,T_ROUTE T2 WHERE T1.ROUTE_ID=T2.ROUTE_ID) TT4
                                        WHERE TT3.OUTLET_ID=TT4.OUTLET_ID";

                    OracleCommand cmdPr = new OracleCommand(qrProduct, con);
                    OracleDataAdapter daPr = new OracleDataAdapter(cmdPr);
                    DataSet dsPr = new DataSet();
                    daPr.Fill(dsPr);
                    int pr = dsPr.Tables[0].Rows.Count;
                    if (pr > 0 && dsPr.Tables[0].Rows[0][0].ToString() != "")
                    {                        
                        string route = "";
                        string SRID = "";
                        string outletID = "";
                        string outletName = "";
                        string itemCode = "";
                        string itemName = "";
                        string itemPrice = "0";
                        string carton = "0";
                        string piece = "0";
                        string totalAmt = "0";

                        for (int p = 0; p < pr; p++)
                        {                          
                            route = "";
                            outletName = "";
                            itemCode = "";
                            itemName = "";
                            itemPrice = "0";
                            totalAmt = "0";

                            SRID = dsPr.Tables[0].Rows[p]["SR_ID"].ToString();
                            outletID = dsPr.Tables[0].Rows[p]["OUTLET_ID"].ToString();
                            outletName = dsPr.Tables[0].Rows[p]["OUTLET_NAME"].ToString();
                            route = dsPr.Tables[0].Rows[p]["ROUTE_NAME"].ToString();

                            itemCode = dsPr.Tables[0].Rows[p]["ITEM_ID"].ToString();
                            itemName = dsPr.Tables[0].Rows[p]["ITEM_NAME"].ToString() + "(" + itemCode + ")";

                            itemPrice = dsPr.Tables[0].Rows[p]["OUT_PRICE"].ToString();
                            carton = dsPr.Tables[0].Rows[p]["ITEM_CTN"].ToString();
                            piece = dsPr.Tables[0].Rows[p]["ITEM_QTY"].ToString();

                            totalAmt = dsPr.Tables[0].Rows[p]["AMT"].ToString();

                            totalNetAmount = totalNetAmount + Convert.ToDouble(totalAmt);                                                        
                           
                            itemName = itemName + "(" + itemCode + ")";

                            string dateTime = "";
                            string qrTime = @"SELECT ENTRY_DATETIME FROM T_ORDER_DETAIL 
                                            WHERE SR_ID='" + SRID + "' AND ITEM_ID='" + itemCode + "' AND OUTLET_ID='" + outletID + "' AND ENTRY_DATE between TO_DATE('" + fdate + "','DD/MM/YYYY') and TO_DATE('" + tdate + "','DD/MM/YYYY')";
                            OracleCommand cmdTime = new OracleCommand(qrTime, con);
                            OracleDataAdapter daTime = new OracleDataAdapter(cmdTime);
                            DataSet dsTime = new DataSet();
                            daTime.Fill(dsTime);
                            int tme = dsTime.Tables[0].Rows.Count;
                            if (tme > 0 && dsTime.Tables[0].Rows[0][0].ToString() != "")
                            {
                                dateTime = dsTime.Tables[0].Rows[0]["ENTRY_DATETIME"].ToString();
                            }

                            msg = msg + ";" + srGroup + ";" + srName + ";" + srPhone + ";" + itemName + ";" + itemPrice + "/-;" + carton + ";" + piece + ";" + totalAmt + "/-;" + outletName + ";" + route + ";" + zoneName + ";" + divisionName + ";" + dateTime;

                        }
                        msg = msg + ";" + srGroup + ";" + srName + ";" + "" + ";" + "" + ";" + "" + ";" + "" + ";" + "" + ";Total: " + totalNetAmount.ToString() + "/-;" + "" + ";" + "" + ";" + "" + ";" + "" + ";" + "";
                    }
                    else
                    {                        
                        string totalNetAmounts = "0";
                        msg = msg + ";" + srGroup + ";" + srName + ";" + "" + ";" + "" + ";" + "" + ";" + "" + ";" + "" + ";Total: " + totalNetAmounts.ToString() + "/-;" + "" + ";" + "" + ";" + "" + ";" + "" + ";" + "";
                    }
                }
            }

            con.Close();
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

            string query = @"SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString() + "'";
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



    static string DisplayPercentage(double ratio)
    {
        string percentage = string.Format("{0:0.0%}", ratio);
        return percentage;
    }

    static string TotalAmount(double amount)
    {
        string totalAmt = string.Format("{0:0.00}", amount);
        return totalAmt;
    }

}