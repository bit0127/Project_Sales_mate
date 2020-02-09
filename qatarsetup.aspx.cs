using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class qatarsetup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //HttpContext.Current.Session["country"] = Request.QueryString["country"].ToString();
            HttpContext.Current.Session["company"] = Request.QueryString["company"].ToString();
        }
    }
 
    [WebMethod(EnableSession = true)]
    public static string AddBaseInfo(string baseId, string baseName, string zoneId, string zoneName, string regionId, string regionName, string country, string status)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {            
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT BASE_ID FROM T_BASE_INFO WHERE BASE_ID='" + baseId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["BASE_ID"].ToString() != "")
            {
                string qrS = @"DELETE FROM T_BASE_INFO WHERE BASE_ID='" + baseId + "'";
                OracleCommand cmdS = new OracleCommand(qrS, conn);
                int cS = cmdS.ExecuteNonQuery();
            }

            string query = @"INSERT INTO T_BASE_INFO(BASE_ID,BASE_NAME,COUNTRY_NAME,ZONE_ID,ZONE_NAME,REGION_ID,REGION_NAME,STATUS)
                            VALUES ('" + baseId + "','" + baseName + "','" + country + "','" + zoneId + "','" + zoneName + "','" + regionId + "','" + regionName + "','" + status + "')";
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
    public static string GetBaseInfo(string baseId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (baseId == "baseId")
            {
                query = @"SELECT * FROM T_BASE_INFO";
            }
            else
            {
                query = @"SELECT * FROM T_BASE_INFO WHERE BASE_ID='" + baseId + "'";
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
                    string BASE_ID = ds.Tables[0].Rows[i]["BASE_ID"].ToString();
                    string BSSE_NAME = ds.Tables[0].Rows[i]["BASE_NAME"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string DIVISION_ID = ds.Tables[0].Rows[i]["REGION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["REGION_NAME"].ToString();
                    string ZONE_ID = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + BASE_ID + ";" + BSSE_NAME + ";" + COUNTRY_NAME + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + STATUS + ";" + ZONE_ID + ";" + ZONE_NAME;
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
    public static string GetBaseInfoByZone(string zoneId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";

            query = @"SELECT * FROM T_BASE_INFO WHERE ZONE_ID='" + zoneId + "' AND STATUS='Y'";            

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string BASE_ID = ds.Tables[0].Rows[i]["BASE_ID"].ToString();
                    string BSSE_NAME = ds.Tables[0].Rows[i]["BASE_NAME"].ToString();

                    msg = msg + ";" + BASE_ID + ";" + BSSE_NAME;
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

    [WebMethod(EnableSession = true)]
    public static string AddQatarRouteInfo(string routeId, string rmName, string country, string division, string zone, string baseId, string status)
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
                query = @"INSERT INTO T_ROUTE(ROUTE_ID,ROUTE_NAME,ZONE_ID,DIVISION_ID,COUNTRY_NAME,STATUS,ENTRY_DATE,ENTRY_BY,BASE_ID)
                            VALUES ((SALES.SEQ_ROUTE_ID.NEXTVAL),'" + rmName + "','" + zone.Trim() + "','" + division.Trim() + "','" + country.Trim() + "','" + status.Trim() + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + baseId.Trim() + "')";
            }
            else
            {
                query = @"UPDATE T_ROUTE SET ROUTE_NAME='" + rmName + "',ZONE_ID='" + zone.Trim() + "',DIVISION_ID='" + division.Trim() + "',COUNTRY_NAME='" + country.Trim() + "',BASE_ID='" + baseId.Trim() + "',STATUS='" + status.Trim() + "' WHERE ROUTE_ID='" + routeId.Trim() + "'";
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


    [WebMethod]
    public static string GetSRRoute(string country)
    {
        string msg = "";
        string days = "";

        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
 
            string query = @"SELECT ROUTE_ID,ROUTE_NAME FROM T_ROUTE
                            WHERE COUNTRY_NAME LIKE '%" + country.Trim() + "%' ORDER BY ROUTE_NAME";
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
    public static string GetCOOInfo(string coo)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (coo == "coo")
            {
                query = @"SELECT COO_ID,COO_NAME,DESIGNATION,MOTHER_COMPANY,OWN_COMPANY,PASS_WORD,STATUS FROM T_COO WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString().Trim() + "'";
            }
            else
            {
                query = @"SELECT COO_ID,COO_NAME,DESIGNATION,MOTHER_COMPANY,OWN_COMPANY,PASS_WORD,STATUS FROM T_COO WHERE COO_ID='" + coo + "'";
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
                    string COO_ID = ds.Tables[0].Rows[i]["COO_ID"].ToString();
                    string COO_NAME = ds.Tables[0].Rows[i]["COO_NAME"].ToString();
                    string DESIGNATION = ds.Tables[0].Rows[i]["DESIGNATION"].ToString();
                    string MOTHER_COMPANY = ds.Tables[0].Rows[i]["MOTHER_COMPANY"].ToString();
                    string OWN_COMPANY = ds.Tables[0].Rows[i]["OWN_COMPANY"].ToString();
                    string PASS_WORD = ds.Tables[0].Rows[i]["PASS_WORD"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + COO_ID + ";" + COO_NAME + ";" + DESIGNATION + ";" + MOTHER_COMPANY + ";" + OWN_COMPANY + ";" + PASS_WORD + ";" + STATUS;
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
    public static string GetHOSInfo(string hos)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (hos == "hos")
            {
                query = @"SELECT T3.*,T4.ITEM_GROUP_ID FROM
                        (SELECT T1.*,T2.COMPANY_ID FROM
                        (SELECT * FROM T_HOS) T1,
                        (SELECT * FROM T_COMPANY WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString().Trim() + "') T2 ";
                query = query + @"WHERE T1.OWN_COMPANY=T2.COMPANY_FULL_NAME) T3,
                        (SELECT * FROM T_ITEM_GROUP) T4
                        WHERE T3.ITEM_GROUP=T4.ITEM_GROUP_NAME";
            }
            else
            {
                query = @"SELECT T3.*,T4.ITEM_GROUP_ID FROM
                        (SELECT T1.*,T2.COMPANY_ID FROM
                        (SELECT * FROM T_HOS WHERE STAFF_ID='" + hos + "') T1, ";
                query = query + @"(SELECT * FROM T_COMPANY) T2
                        WHERE T1.OWN_COMPANY=T2.COMPANY_FULL_NAME) T3,
                        (SELECT * FROM T_ITEM_GROUP) T4
                        WHERE T3.ITEM_GROUP=T4.ITEM_GROUP_NAME";
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
                    string HOS_ID = ds.Tables[0].Rows[i]["STAFF_ID"].ToString();
                    string HOS_NAME = ds.Tables[0].Rows[i]["HOS_NAME"].ToString();
                    string PASSWORD = ds.Tables[0].Rows[i]["PASSWORD"].ToString();
                    string MOBILE_NO = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string EMAIL_ADDRESS = ds.Tables[0].Rows[i]["EMAIL_ADDRESS"].ToString();
                    string MOTHER_COMPANY = ds.Tables[0].Rows[i]["MOTHER_COMPANY"].ToString();
                    string OWN_COMPANY = ds.Tables[0].Rows[i]["OWN_COMPANY"].ToString();
                    string OWN_COMPANY_ID = ds.Tables[0].Rows[i]["COMPANY_ID"].ToString();
                    string ITEM_GROUP = ds.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                    string ITEM_GROUP_ID = ds.Tables[0].Rows[i]["ITEM_GROUP_ID"].ToString();
                    string WORKING_AREA = ds.Tables[0].Rows[i]["WORKING_AREA"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + HOS_ID + ";" + HOS_NAME + ";" + PASSWORD + ";" + MOBILE_NO + ";" + EMAIL_ADDRESS + ";" + MOTHER_COMPANY + ";" + OWN_COMPANY + ";" + ITEM_GROUP + ";" + WORKING_AREA + ";" + STATUS + ";" + OWN_COMPANY_ID + ";" + ITEM_GROUP_ID;
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
    public static string GetRMInfo(string rm)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (rm == "rm")
            {
                query = @"SELECT T4.*,T5.ITEM_GROUP_ID FROM
                        (SELECT T3.*,T4.COMPANY_ID FROM
                        (SELECT T1.*,T2.DIVISION_NAME FROM
                         (SELECT * FROM T_AGM_RM) T1,
                         (SELECT * FROM T_DIVISION) T2
                         WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                         (SELECT * FROM T_COMPANY WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString().Trim() + "') T4 ";
                query = query + @"WHERE T3.OWN_COMPANY=T4.COMPANY_FULL_NAME) T4,
                        (SELECT * FROM T_ITEM_GROUP) T5
                        WHERE T4.ITEM_GROUP=T5.ITEM_GROUP_NAME";
            }
            else
            {
                query = @"SELECT T4.*,T5.ITEM_GROUP_ID FROM
                        (SELECT T3.*,T4.COMPANY_ID FROM
                        (SELECT T1.*,T2.DIVISION_NAME FROM
                         (SELECT * FROM T_AGM_RM WHERE RM_ID='" + rm + "') T1,";
                query = query + @"(SELECT * FROM T_DIVISION) T2
                         WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                         (SELECT * FROM T_COMPANY) T4
                        WHERE T3.OWN_COMPANY=T4.COMPANY_FULL_NAME) T4,
                        (SELECT * FROM T_ITEM_GROUP) T5
                        WHERE T4.ITEM_GROUP=T5.ITEM_GROUP_NAME";
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
                    string RM_ID = ds.Tables[0].Rows[i]["RM_ID"].ToString();
                    string RM_NAME = ds.Tables[0].Rows[i]["RM_NAME"].ToString();
                    string RM_PWD = ds.Tables[0].Rows[i]["RM_PWD"].ToString();
                    string MOBILE_NO = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string EMAIL_ADDRESS = ds.Tables[0].Rows[i]["EMAIL_ADDRESS"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string MOTHER_COMPANY = ds.Tables[0].Rows[i]["MOTHER_COMPANY"].ToString();
                    string OWN_COMPANY = ds.Tables[0].Rows[i]["OWN_COMPANY"].ToString();
                    string OWN_COMPANY_ID = ds.Tables[0].Rows[i]["COMPANY_ID"].ToString();
                    string ITEM_GROUP = ds.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                    string ITEM_GROUP_ID = ds.Tables[0].Rows[i]["ITEM_GROUP_ID"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + RM_ID + ";" + RM_NAME + ";" + RM_PWD + ";" + MOBILE_NO + ";" + EMAIL_ADDRESS + ";" + COUNTRY_NAME + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + MOTHER_COMPANY + ";" + OWN_COMPANY + ";" + OWN_COMPANY_ID + ";" + ITEM_GROUP + ";" + ITEM_GROUP_ID + ";" + STATUS;
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
    public static string GetTSMInfo(string tsm)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (tsm == "tsm")
            {
                query = @"SELECT T7.*,T8.ZONE_NAME FROM
                        (SELECT T5.*,T6.ITEM_GROUP_ID FROM
                        (SELECT T3.*,T4.COMPANY_ID FROM
                        (SELECT T1.*,T2.DIVISION_NAME FROM
                         (SELECT * FROM T_TSM_ZM) T1,
                         (SELECT * FROM T_DIVISION) T2
                         WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                         (SELECT * FROM T_COMPANY WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString().Trim() + "') T4 ";
                query = query + @"WHERE T3.OWN_COMPANY=T4.COMPANY_FULL_NAME) T5,
                        (SELECT * FROM T_ITEM_GROUP) T6
                        WHERE T5.ITEM_GROUP=T6.ITEM_GROUP_NAME) T7,
                        (SELECT * FROM T_ZONE) T8
                        WHERE T7.ZONE_ID=T8.ZONE_ID";
            }
            else
            {
                query = @"SELECT T7.*,T8.ZONE_NAME FROM
                        (SELECT T5.*,T6.ITEM_GROUP_ID FROM
                        (SELECT T3.*,T4.COMPANY_ID FROM
                        (SELECT T1.*,T2.DIVISION_NAME FROM
                         (SELECT * FROM T_TSM_ZM WHERE TSM_ID='" + tsm + "') T1, ";
                query = query + @" (SELECT * FROM T_DIVISION) T2
                         WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                         (SELECT * FROM T_COMPANY) T4
                        WHERE T3.OWN_COMPANY=T4.COMPANY_FULL_NAME) T5,
                        (SELECT * FROM T_ITEM_GROUP) T6
                        WHERE T5.ITEM_GROUP=T6.ITEM_GROUP_NAME) T7,
                        (SELECT * FROM T_ZONE) T8
                        WHERE T7.ZONE_ID=T8.ZONE_ID";
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
                    string RM_ID = ds.Tables[0].Rows[i]["TSM_ID"].ToString();
                    string RM_NAME = ds.Tables[0].Rows[i]["TSM_NAME"].ToString();
                    string RM_PWD = ds.Tables[0].Rows[i]["TSM_PWD"].ToString();
                    string MOBILE_NO = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string EMAIL_ADDRESS = ds.Tables[0].Rows[i]["EMAIL_ADDRESS"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string ZONE_ID = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                    string MOTHER_COMPANY = ds.Tables[0].Rows[i]["MOTHER_COMPANY"].ToString();
                    string OWN_COMPANY = ds.Tables[0].Rows[i]["OWN_COMPANY"].ToString();
                    string OWN_COMPANY_ID = ds.Tables[0].Rows[i]["COMPANY_ID"].ToString();
                    string ITEM_GROUP = ds.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                    string ITEM_GROUP_ID = ds.Tables[0].Rows[i]["ITEM_GROUP_ID"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + RM_ID + ";" + RM_NAME + ";" + RM_PWD + ";" + MOBILE_NO + ";" + EMAIL_ADDRESS + ";" + COUNTRY_NAME + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + MOTHER_COMPANY + ";" + OWN_COMPANY + ";" + OWN_COMPANY_ID + ";" + ITEM_GROUP + ";" + ITEM_GROUP_ID + ";" + STATUS + ";" + ZONE_ID + ";" + ZONE_NAME;
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
    public static string GetSRInfo(string srId)
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
                query = @"SELECT T5.*, T6.COMPANY_NICK_NAME FROM
                        (SELECT T3.*, T4.ZONE_NAME FROM
                        (SELECT TT1.*,T2.DIVISION_NAME FROM
                        (SELECT T1.SR_ID,T1.SR_NAME,T1.SR_PWD,T1.MOBILE_NO,T1.EMAIL_ADDRESS,T1.DIST_ID,T1.COUNTRY_NAME,T1.DIVISION_NAME DIVISION_ID,T1.DIST_ZONE,T1.ITEM_GROUP,T1.PRE_ORDER,T1.DIRECT_SALES,T1.DELIVERY,T1.STATUS,
                        T1.MOTHER_COMPANY,T1.ITEM_COMPANY,T1.ITEM_GROUP_ID 
                        FROM T_SR_INFO T1) TT1, ";
                query = query + @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T2
                        WHERE TT1.DIVISION_ID=T2.DIVISION_ID) T3,

                        (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE) T4
                        WHERE T3.DIST_ZONE=T4.ZONE_ID) T5,

                        (SELECT COMPANY_ID,COMPANY_NICK_NAME FROM T_COMPANY WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString().Trim() + "') T6 WHERE T5.ITEM_COMPANY=T6.COMPANY_ID ";
            }
            else
            {
                query = @"SELECT T5.*, T6.COMPANY_NICK_NAME FROM
                        (SELECT T3.*, T4.ZONE_NAME FROM
                        (SELECT TT1.*,T2.DIVISION_NAME FROM
                        (SELECT T1.SR_ID,T1.SR_NAME,T1.SR_PWD,T1.MOBILE_NO,T1.EMAIL_ADDRESS,T1.DIST_ID,T1.COUNTRY_NAME,T1.DIVISION_NAME DIVISION_ID,T1.DIST_ZONE,T1.ITEM_GROUP,T1.PRE_ORDER,T1.DIRECT_SALES,T1.DELIVERY,T1.STATUS,
                        T1.MOTHER_COMPANY,T1.ITEM_COMPANY,T1.ITEM_GROUP_ID 
                        FROM T_SR_INFO T1  WHERE T1.SR_ID='" + srId + "') TT1, ";
                query = query + @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T2
                        WHERE TT1.DIVISION_ID=T2.DIVISION_ID) T3,

                        (SELECT ZONE_ID,ZONE_NAME FROM T_ZONE) T4
                        WHERE T3.DIST_ZONE=T4.ZONE_ID) T5,

                        (SELECT COMPANY_ID,COMPANY_NICK_NAME FROM T_COMPANY) T6
                        WHERE T5.ITEM_COMPANY=T6.COMPANY_ID ";
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
                    string SR_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString().Trim();
                    string SR_NAME = ds.Tables[0].Rows[i]["SR_NAME"].ToString().Trim();
                    string SR_PWD = ds.Tables[0].Rows[i]["SR_PWD"].ToString();
                    string MOBILE_NO = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string EMAIL_ADDRESS = ds.Tables[0].Rows[i]["EMAIL_ADDRESS"].ToString();
                    string DIST_ID = ds.Tables[0].Rows[i]["DIST_ID"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string DIST_ZONE = ds.Tables[0].Rows[i]["DIST_ZONE"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                    string MOTHER_COMPANY = ds.Tables[0].Rows[i]["MOTHER_COMPANY"].ToString();
                    string ITEM_COMPANY_ID = ds.Tables[0].Rows[i]["ITEM_COMPANY"].ToString();
                    string COMPANY_NICK_NAME = ds.Tables[0].Rows[i]["COMPANY_NICK_NAME"].ToString();
                    string ITEM_GROUP = ds.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                    string ITEM_GROUP_ID = ds.Tables[0].Rows[i]["ITEM_GROUP_ID"].ToString();
                    string PRE_ORDER = ds.Tables[0].Rows[i]["PRE_ORDER"].ToString();
                    string DIRECT_SALES = ds.Tables[0].Rows[i]["DIRECT_SALES"].ToString();
                    string DELIVERY = ds.Tables[0].Rows[i]["DELIVERY"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + SR_ID + ";" + SR_NAME + ";" + SR_PWD + ";" + MOBILE_NO + ";" + EMAIL_ADDRESS + ";" + DIST_ID + ";" + COUNTRY_NAME + ";" + DIVISION_ID + ";" + DIST_ZONE + ";" + ITEM_GROUP + ";" + PRE_ORDER + ";" + DIRECT_SALES + ";" + DELIVERY + ";" + STATUS + ";" + ITEM_GROUP_ID + ";" + MOTHER_COMPANY + ";" + COMPANY_NICK_NAME + ";" + DIVISION_NAME + ";" + ZONE_NAME + ";" + ITEM_COMPANY_ID;
                    //msg = msg + ";" + SR_ID + ";" + SR_NAME;
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
    public static string GetDivisionInfo(string division)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (division == "division")
            {
                query = @"SELECT * FROM T_DIVISION WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString().Trim() + "%' ORDER BY DIVISION_NAME";
            }
            else
            {
                query = @"SELECT * FROM T_DIVISION WHERE DIVISION_ID='" + division + "' ORDER BY DIVISION_NAME";
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
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + COUNTRY_NAME + ";" + STATUS;
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

    [WebMethod(EnableSession = true)]
    public static string AddDivisionInfo(string divisionId, string divisionName, string country, string status)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT DIVISION_ID FROM T_DIVISION WHERE DIVISION_ID='" + divisionId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["DIVISION_ID"].ToString() != "")
            {
                string qrS = @"DELETE FROM T_DIVISION WHERE DIVISION_ID='" + divisionId + "'";
                OracleCommand cmdS = new OracleCommand(qrS, conn);
                int cS = cmdS.ExecuteNonQuery();
            }

            string query = @"INSERT INTO T_DIVISION(DIVISION_ID,DIVISION_NAME,COUNTRY_NAME,STATUS,ENTRY_DATE,ENTRY_BY)
                            VALUES ('" + divisionId + "','" + divisionName + "','" + country + "','" + status + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "')";
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
    public static string AddZoneInfo(string zoneId, string zoneName, string division, string country, string status)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT ZONE_ID FROM T_ZONE WHERE ZONE_ID='" + zoneId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["ZONE_ID"].ToString() != "")
            {
                string qrS = @"DELETE FROM T_ZONE WHERE ZONE_ID='" + zoneId + "'";
                OracleCommand cmdS = new OracleCommand(qrS, conn);
                int cS = cmdS.ExecuteNonQuery();
            }

            string query = @"INSERT INTO T_ZONE(ZONE_ID,ZONE_NAME,DIVISION_ID,COUNTRY_NAME,STATUS,ENTRY_DATE,ENTRY_BY)
                            VALUES ('" + zoneId + "','" + zoneName + "','" + division + "','" + country + "','" + status + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "')";
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
    








}