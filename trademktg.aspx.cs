using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class trademktg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //if (Session["userid"].ToString() == null || Session["userid"].ToString() == "")
            //{
            //    Response.Redirect("login.aspx");
            //}
        }
    }

    [WebMethod(EnableSession = true)]
    public static string AddGroupInfo(string groupId, string groupName, string status)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT GROUP_ID FROM T_MKTG_GROUP WHERE GROUP_ID='" + groupId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["GROUP_ID"].ToString() != "")
            {
                string qrS = @"UPDATE T_MKTG_GROUP SET GROUP_NAME='" + groupName + "', STATUS='" + status + "' WHERE GROUP_ID='" + groupId + "'";
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
                string query = @"INSERT INTO T_MKTG_GROUP(GROUP_ID,GROUP_NAME,ENTRY_BY,ENTRY_DATE,STATUS)
                            VALUES ('" + groupId + "','" + groupName + "','" + HttpContext.Current.Session["userid"].ToString() + "',TO_DATE(SYSDATE),'" + status + "')";
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
                query = @"SELECT GROUP_ID,GROUP_NAME,STATUS FROM T_MKTG_GROUP WHERE STATUS='Y' ORDER BY GROUP_ID";
            }
            else
            {
                query = @"SELECT GROUP_ID,GROUP_NAME,STATUS FROM T_MKTG_GROUP WHERE STATUS='Y' AND GROUP_ID='" + groupId + "'";
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
                    string GROUP_ID = ds.Tables[0].Rows[i]["GROUP_ID"].ToString();
                    string GROUP_NAME = ds.Tables[0].Rows[i]["GROUP_NAME"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + GROUP_ID + ";" + GROUP_NAME + ";" + STATUS;
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
    public static string AddDivisionInfo(string groupId, string groupName, string divisionId, string divisionName, string country, string status)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT DIVISION_ID FROM T_MKTG_DIVISION WHERE DIVISION_ID='" + divisionId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["DIVISION_ID"].ToString() != "")
            {
                string qrS = @"UPDATE T_MKTG_DIVISION SET DIVISION_NAME='" + divisionName + "', COUNTRY_NAME='" + country + "',GROUP_ID='" + groupId + "',GROUP_NAME='" + groupName + "', STATUS='" + status + "' WHERE DIVISION_ID='" + divisionId + "'";
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

                string query = @"INSERT INTO T_MKTG_DIVISION(DIVISION_ID,DIVISION_NAME,COUNTRY_NAME,STATUS,ENTRY_DATE,ENTRY_BY,GROUP_ID,GROUP_NAME)
                            VALUES ('" + divisionId + "','" + divisionName + "','" + country + "','" + status + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + groupId + "','" + groupName + "')";
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

    [WebMethod]
    public static string GetOperationManager(string groupId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = @"SELECT OPM_ID,OPM_NAME FROM T_MKTG_OPERATION_MNGR WHERE STATUS='Y' AND GROUP_ID='" + groupId + "' ORDER BY OPM_NAME"; ;

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string OPM_ID = ds.Tables[0].Rows[i]["OPM_ID"].ToString();
                    string OPM_NAME = ds.Tables[0].Rows[i]["OPM_NAME"].ToString();

                    msg = msg + ";" + OPM_ID + ";" + OPM_NAME;
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
    public static string GetGroupWiseOperationManager(string groupId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = @"SELECT OPM_ID,OPM_NAME FROM T_MKTG_OPERATION_MNGR WHERE STATUS='Y' AND GROUP_ID='" + groupId + "' ORDER BY OPM_NAME"; ;

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string OPM_ID = ds.Tables[0].Rows[i]["OPM_ID"].ToString();
                    string OPM_NAME = ds.Tables[0].Rows[i]["OPM_NAME"].ToString();

                    msg = msg + ";" + OPM_ID + ";" + OPM_NAME;
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
    public static string GetAreaMngrByOptMngr(string opmId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = @" SELECT RM_ID,RM_NAME FROM T_MKTG_AREA_MNGR WHERE OPM_ID='" + opmId.Trim() + "' ORDER BY RM_NAME";

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

                    msg = msg + ";" + RM_ID + ";" + RM_NAME;
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
    public static string GetSupervisorByAreaMngr(string areaMngrId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = @" SELECT SPR_ID,SPR_NAME FROM T_MKTG_SUPERVISOR WHERE MNGR_ID='" + areaMngrId.Trim() + "' ORDER BY SPR_NAME";

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string SPR_ID = ds.Tables[0].Rows[i]["SPR_ID"].ToString();
                    string SPR_NAME = ds.Tables[0].Rows[i]["SPR_NAME"].ToString();

                    msg = msg + ";" + SPR_ID + ";" + SPR_NAME;
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
    public static string GetMarchandiserBySupervisor(string superId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = @" SELECT SR_ID,SR_NAME FROM T_MKTG_SR_INFO WHERE SUPERVISOR_ID='" + superId.Trim() + "' ORDER BY SR_NAME";

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

                    msg = msg + ";" + SR_ID + ";" + SR_NAME;
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
    public static string GetDivisionalManager(string groupId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = @" SELECT RM_ID,RM_NAME FROM T_MKTG_AREA_MNGR WHERE GROUP_ID='" + groupId + "'";

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

                    msg = msg + ";" + RM_ID + ";" + RM_NAME;
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
                query = @"SELECT * FROM T_MKTG_DIVISION ORDER BY DIVISION_ID";
            }
            else
            {
                query = @"SELECT * FROM T_MKTG_DIVISION WHERE DIVISION_ID='" + division + "' ORDER BY DIVISION_ID";
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
                    string GROUP_ID = ds.Tables[0].Rows[i]["GROUP_ID"].ToString();
                    string GROUP_NAME = ds.Tables[0].Rows[i]["GROUP_NAME"].ToString();
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + COUNTRY_NAME + ";" + STATUS + ";" + GROUP_ID + ";" + GROUP_NAME;
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
    public static string GetRegionByDivision(string divisionId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = @"SELECT REGION_ID,REGION_NAME FROM T_MKTG_REGION WHERE DIVISION_ID='" + divisionId + "' ORDER BY REGION_ID";
           
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string REGION_ID = ds.Tables[0].Rows[i]["REGION_ID"].ToString();
                    string REGION_NAME = ds.Tables[0].Rows[i]["REGION_NAME"].ToString();


                    msg = msg + ";" + REGION_ID + ";" + REGION_NAME;
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
    public static string GetDivision(string countryName, string group)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";

            query = @"SELECT * FROM T_MKTG_DIVISION WHERE GROUP_ID = '"+ group +"' AND STATUS='Y' AND COUNTRY_NAME LIKE'%" + countryName + "%' ORDER BY DIVISION_ID";
           

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

                    msg = msg + ";" + DIVISION_ID + ";" + DIVISION_NAME;
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
    public static string GetDivisionByCountry(string countryName)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";

            query = @"SELECT * FROM T_MKTG_DIVISION WHERE STATUS='Y' AND COUNTRY_NAME LIKE'%" + countryName + "%' AND DIVISION_ID NOT IN('PAllDiv','RAllDiv','FAllDiv','PEAllDiv') ORDER BY DIVISION_ID";
           

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

                    msg = msg + ";" + DIVISION_ID + ";" + DIVISION_NAME;
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
    public static string GetaLLDivision(string countryName)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";

            query = @"SELECT * FROM T_MKTG_DIVISION ORDER BY DIVISION_ID";


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

                    msg = msg + ";" + DIVISION_ID + ";" + DIVISION_NAME;
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
    public static string AddRouteInfo(string routeId, string rmName, string country, string division, string zone, string status, string regionId, string regionName)
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
                query = @"INSERT INTO T_MKTG_ROUTE(ROUTE_ID,ROUTE_NAME,ZONE_ID,DIVISION_ID,COUNTRY_NAME,STATUS,ENTRY_DATE,ENTRY_BY,REGION_ID,REGION_NAME)
                            VALUES ((SALES.SEQ_ROUTE_ID.NEXTVAL),'" + rmName + "','" + zone + "','" + division + "','" + country + "','" + status + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + regionId + "','" + regionName + "')";
            }
            else
            {
                query = @"UPDATE T_MKTG_ROUTE SET ROUTE_NAME='" + rmName + "',ZONE_ID='" + zone + "',DIVISION_ID='" + division + "',COUNTRY_NAME='" + country + "',STATUS='" + status + "',REGION_ID='" + regionId + "',REGION_NAME='" + regionName + "' WHERE ROUTE_ID='" + routeId + "'";
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
                        (SELECT ROUTE_ID,ROUTE_NAME,ZONE_ID,DIVISION_ID,COUNTRY_NAME,STATUS,REGION_ID,REGION_NAME FROM T_MKTG_ROUTE) T1,
                        (SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T2 WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                        (SELECT * FROM T_MKTG_ZONE) T4
                        WHERE T3.ZONE_ID=T4.ZONE_ID";
            }
            else
            {
                query = @"SELECT T3.*,T4.ZONE_NAME FROM
                        (SELECT T1.*,T2.DIVISION_NAME FROM
                        (SELECT ROUTE_ID,ROUTE_NAME,ZONE_ID,DIVISION_ID,COUNTRY_NAME,STATUS,REGION_ID,REGION_NAME FROM T_MKTG_ROUTE WHERE ROUTE_ID='" + route + "') T1, ";
                query = query + @" (SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T2 WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                        (SELECT * FROM T_MKTG_ZONE) T4
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
                    string REGION_ID = ds.Tables[0].Rows[i]["REGION_ID"].ToString();
                    string REGION_NAME = ds.Tables[0].Rows[i]["REGION_NAME"].ToString();                    
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();


                    msg = msg + ";" + ROUTE_ID + ";" + ROUTE_NAME + ";" + ZONE_ID + ";" + ZONE_NAME + ";" + REGION_ID + ";" + REGION_NAME + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + COUNTRY_NAME + ";" + STATUS;
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


            string query = @"SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE
                            WHERE COUNTRY_NAME='" + country + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "'";
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

            string qr = "SELECT DAY_NAME,ROUTE_ID FROM T_MKTG_SR_ROUTE_DAY WHERE SR_ID='" + srId + "'";
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


    [WebMethod(EnableSession = true)]
    public static string AddSRRouteDayInfo(string srId, string country, string division, string zone, string day1, string day2, string day3, string day4, string day5, string day6, string route1, string route2, string route3, string route4, string route5, string route6, string route1Id, string route2Id, string route3Id, string route4Id, string route5Id, string route6Id)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT SR_ID FROM T_MKTG_SR_ROUTE_DAY WHERE SR_ID='" + srId + "' AND STATUS='Y'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["SR_ID"].ToString() != "")
            {

                string query1 = @"UPDATE T_MKTG_SR_ROUTE_DAY SET ROUTE_ID='" + route1Id + "',ROUTE_NAME='" + route1 + "' WHERE DAY_NAME='" + day1 + "' AND COUNTRY_NAME='" + country + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "' AND SR_ID='" + srId + "'";
                OracleCommand cmd1 = new OracleCommand(query1, conn);
                int c1 = cmd1.ExecuteNonQuery();

                string query2 = @"UPDATE T_MKTG_SR_ROUTE_DAY SET ROUTE_ID='" + route2Id + "',ROUTE_NAME='" + route2 + "' WHERE DAY_NAME='" + day2 + "' AND COUNTRY_NAME='" + country + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "' AND SR_ID='" + srId + "'";
                OracleCommand cmd2 = new OracleCommand(query2, conn);
                int c2 = cmd2.ExecuteNonQuery();

                string query3 = @"UPDATE T_MKTG_SR_ROUTE_DAY SET ROUTE_ID='" + route3Id + "',ROUTE_NAME='" + route3 + "' WHERE DAY_NAME='" + day3 + "' AND COUNTRY_NAME='" + country + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "' AND SR_ID='" + srId + "'";
                OracleCommand cmd3 = new OracleCommand(query3, conn);
                int c3 = cmd3.ExecuteNonQuery();

                string query4 = @"UPDATE T_MKTG_SR_ROUTE_DAY SET ROUTE_ID='" + route4Id + "',ROUTE_NAME='" + route4 + "' WHERE DAY_NAME='" + day4 + "' AND COUNTRY_NAME='" + country + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "' AND SR_ID='" + srId + "'";
                OracleCommand cmd4 = new OracleCommand(query4, conn);

                int c4 = cmd4.ExecuteNonQuery();

                string query5 = @"UPDATE T_MKTG_SR_ROUTE_DAY SET ROUTE_ID='" + route5Id + "',ROUTE_NAME='" + route5 + "' WHERE DAY_NAME='" + day5 + "' AND COUNTRY_NAME='" + country + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "' AND SR_ID='" + srId + "'";
                OracleCommand cmd5 = new OracleCommand(query5, conn);

                int c5 = cmd5.ExecuteNonQuery();

                string query6 = @"UPDATE T_MKTG_SR_ROUTE_DAY SET ROUTE_ID='" + route6Id + "',ROUTE_NAME='" + route6 + "' WHERE DAY_NAME='" + day6 + "' AND COUNTRY_NAME='" + country + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "' AND SR_ID='" + srId + "'";
                OracleCommand cmd6 = new OracleCommand(query6, conn);

                int c6 = cmd6.ExecuteNonQuery();

                msg = "Successful!";
            }
            else
            {
                string query1 = @"INSERT INTO T_MKTG_SR_ROUTE_DAY(SR_ID,DAY_NAME,ROUTE_NAME,COUNTRY_NAME,DIVISION_ID,ZONE_ID,STATUS,ENTRY_DATE,ENTRY_BY,ROUTE_ID)
                            VALUES ('" + srId + "','" + day1 + "','" + route1 + "','" + country + "','" + division + "','" + zone + "','Y',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + route1Id + "')";
                OracleCommand cmd1 = new OracleCommand(query1, conn);

                int c1 = cmd1.ExecuteNonQuery();

                string query2 = @"INSERT INTO T_MKTG_SR_ROUTE_DAY(SR_ID,DAY_NAME,ROUTE_NAME,COUNTRY_NAME,DIVISION_ID,ZONE_ID,STATUS,ENTRY_DATE,ENTRY_BY,ROUTE_ID)
                            VALUES ('" + srId + "','" + day2 + "','" + route2 + "','" + country + "','" + division + "','" + zone + "','Y',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + route2Id + "')";
                OracleCommand cmd2 = new OracleCommand(query2, conn);

                int c2 = cmd2.ExecuteNonQuery();

                string query3 = @"INSERT INTO T_MKTG_SR_ROUTE_DAY(SR_ID,DAY_NAME,ROUTE_NAME,COUNTRY_NAME,DIVISION_ID,ZONE_ID,STATUS,ENTRY_DATE,ENTRY_BY,ROUTE_ID)
                            VALUES ('" + srId + "','" + day3 + "','" + route3 + "','" + country + "','" + division + "','" + zone + "','Y',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + route3Id + "')";
                OracleCommand cmd3 = new OracleCommand(query3, conn);

                int c3 = cmd3.ExecuteNonQuery();

                string query4 = @"INSERT INTO T_MKTG_SR_ROUTE_DAY(SR_ID,DAY_NAME,ROUTE_NAME,COUNTRY_NAME,DIVISION_ID,ZONE_ID,STATUS,ENTRY_DATE,ENTRY_BY,ROUTE_ID)
                            VALUES ('" + srId + "','" + day4 + "','" + route4 + "','" + country + "','" + division + "','" + zone + "','Y',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + route4Id + "')";
                OracleCommand cmd4 = new OracleCommand(query4, conn);

                int c4 = cmd4.ExecuteNonQuery();

                string query5 = @"INSERT INTO T_MKTG_SR_ROUTE_DAY(SR_ID,DAY_NAME,ROUTE_NAME,COUNTRY_NAME,DIVISION_ID,ZONE_ID,STATUS,ENTRY_DATE,ENTRY_BY,ROUTE_ID)
                            VALUES ('" + srId + "','" + day5 + "','" + route5 + "','" + country + "','" + division + "','" + zone + "','Y',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + route5Id + "')";
                OracleCommand cmd5 = new OracleCommand(query5, conn);

                int c5 = cmd5.ExecuteNonQuery();

                string query6 = @"INSERT INTO T_MKTG_SR_ROUTE_DAY(SR_ID,DAY_NAME,ROUTE_NAME,COUNTRY_NAME,DIVISION_ID,ZONE_ID,STATUS,ENTRY_DATE,ENTRY_BY,ROUTE_ID)
                            VALUES ('" + srId + "','" + day6 + "','" + route6 + "','" + country + "','" + division + "','" + zone + "','Y',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + route6Id + "')";
                OracleCommand cmd6 = new OracleCommand(query6, conn);

                int c6 = cmd6.ExecuteNonQuery();

                msg = "Successful!";
            }

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }

    
    
    [WebMethod(EnableSession = true)]
    public static string AddSRTarget(string srId, string srName, string groupId, string groupName, string country, string division, string region, string zone, string targetAmt, string operatingCost, string popCost, string promoCost, string month, string year, string optId, string optName, string mngrId, string mngrName, string sprId, string sprName)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT SR_ID FROM SALES.T_MKTG_TARGET WHERE SR_ID='" + srId.Trim() + "' AND MONTH_NAME='" + month.Trim() + "' AND YEAR_NAME='" + year.Trim() + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int K = ds.Tables[0].Rows.Count;
            if (K > 0 && ds.Tables[0].Rows[0]["SR_ID"].ToString() != "")
            {
                string qrS = @"UPDATE SALES.T_MKTG_TARGET SET SR_NAME='" + srName + "',GROUP_ID='" + groupId.Trim() + "',GROUP_NAME='" + groupName + "',COUNTRY='" + country.Trim() + "',DIVISION_ID='" + division.Trim() + "',REGION_ID='" + region.Trim() + "',ZONE_ID='" + zone.Trim() + "',SUPERVISOR='" + sprId.Trim() + "',OPT_ID='" + optId + "',OPT_NAME='" + optName + "',AREA_MNGR_ID='" + mngrId + "',AREA_MNGR_NAME='" + mngrName + "',SUPERVISOR_NAME='" + sprName + "',TARGET_AMT='" + targetAmt.Trim() + "',OPERATING_COST='" + operatingCost.Trim() + "',POP_COST='" + popCost.Trim() + "',PROMO_COST='" + promoCost.Trim() + "' WHERE SR_ID='" + srId.Trim() + "' AND MONTH_NAME='" + month.Trim() + "' AND YEAR_NAME='" + year.Trim() + "'";
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
                string query = @"INSERT INTO SALES.T_MKTG_TARGET(SR_ID,SR_NAME,GROUP_NAME,COUNTRY,DIVISION_ID,ZONE_ID,SUPERVISOR,TARGET_AMT,OPERATING_COST,POP_COST,PROMO_COST,MONTH_NAME,YEAR_NAME,ENTRY_DATE,GROUP_ID,REGION_ID,OPT_ID,OPT_NAME,AREA_MNGR_ID,AREA_MNGR_NAME,SUPERVISOR_NAME) 
                           VALUES('" + srId + "','" + srName + "','" + groupName + "','" + country + "','" + division + "','" + zone + "','" + sprId + "','" + targetAmt + "','" + operatingCost + "','" + popCost + "','" + promoCost + "','" + month + "','" + year + "',TO_DATE(SYSDATE),'" + groupId + "','" + region.Trim() + "','" + optId + "','" + optName + "','" + mngrId + "','" + mngrName + "','" + sprName + "')";
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
    public static string GetSRTargetInfo(string srId, string monthName)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string qr = "";
            if (srId == "srId")
            {
                qr = @"SELECT T5.*,T6.ZONE_NAME FROM                       
                     (SELECT T3.*,T4.REGION_NAME FROM                        
                     (SELECT T1.*,T2.DIVISION_NAME FROM
                     (SELECT * FROM SALES.T_MKTG_TARGET WHERE YEAR_NAME='" + DateTime.Now.Year + "') T1, ";
                qr = qr + @" (SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T2
                     WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,  
                     (SELECT REGION_ID,REGION_NAME FROM T_MKTG_REGION) T4
                     WHERE T3.REGION_ID=T4.REGION_ID) T5,                    
                     (SELECT ZONE_ID,ZONE_NAME FROM T_MKTG_ZONE) T6
                     WHERE T5.ZONE_ID=T6.ZONE_ID";
            }
            else
            {
                qr = @"SELECT T5.*,T6.ZONE_NAME FROM                       
                     (SELECT T3.*,T4.REGION_NAME FROM                        
                     (SELECT T1.*,T2.DIVISION_NAME FROM
                     (SELECT * FROM SALES.T_MKTG_TARGET WHERE SR_ID='" + srId + "' AND MONTH_NAME LIKE '%" + monthName + "%' AND YEAR_NAME='" + DateTime.Now.Year + "') T1, ";
                qr = qr + @" (SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T2
                     WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,  
                     (SELECT REGION_ID,REGION_NAME FROM T_MKTG_REGION) T4
                     WHERE T3.REGION_ID=T4.REGION_ID) T5,                    
                     (SELECT ZONE_ID,ZONE_NAME FROM T_MKTG_ZONE) T6
                     WHERE T5.ZONE_ID=T6.ZONE_ID";
            }

            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int K = ds.Tables[0].Rows.Count;
            if (K > 0 && ds.Tables[0].Rows[0]["SR_ID"].ToString() != "")
            {
                for (int i = 0; i < K; i++)
                {
                    string SR_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string SR_NAME = ds.Tables[0].Rows[i]["SR_NAME"].ToString();

                    string GROUP_ID = ds.Tables[0].Rows[i]["GROUP_ID"].ToString();
                    string GROUP_NAME = ds.Tables[0].Rows[i]["GROUP_NAME"].ToString();
                    
                    string COUNTRY = ds.Tables[0].Rows[i]["COUNTRY"].ToString();
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();

                    string REGION_ID = ds.Tables[0].Rows[i]["REGION_ID"].ToString();
                    string REGION_NAME = ds.Tables[0].Rows[i]["REGION_NAME"].ToString();

                    string ZONE_ID = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();

                    string SUPERVISOR_ID = ds.Tables[0].Rows[i]["SUPERVISOR"].ToString();
                    string SUPERVISOR_NAME = ds.Tables[0].Rows[i]["SUPERVISOR_NAME"].ToString();

                    string OPT_ID = ds.Tables[0].Rows[i]["OPT_ID"].ToString();
                    string OPT_NAME = ds.Tables[0].Rows[i]["OPT_NAME"].ToString();

                    string AREA_MNGR_ID = ds.Tables[0].Rows[i]["AREA_MNGR_ID"].ToString();
                    string AREA_MNGR_NAME = ds.Tables[0].Rows[i]["AREA_MNGR_NAME"].ToString();
                    
                    string TARGET_AMT = ds.Tables[0].Rows[i]["TARGET_AMT"].ToString();
                    string OPERATING_COST = ds.Tables[0].Rows[i]["OPERATING_COST"].ToString();
                    string POP_COST = ds.Tables[0].Rows[i]["POP_COST"].ToString();
                    string PROMO_COST = ds.Tables[0].Rows[i]["PROMO_COST"].ToString();
                    string MONTH_NAME = ds.Tables[0].Rows[i]["MONTH_NAME"].ToString();
                    string YEAR_NAME = ds.Tables[0].Rows[i]["YEAR_NAME"].ToString();



                    msg = msg + ";" + SR_ID + ";" + SR_NAME + ";" + GROUP_ID + ";" + GROUP_NAME + ";" + COUNTRY + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + REGION_ID + ";" + REGION_NAME + ";" + ZONE_ID + ";" + ZONE_NAME + ";" + SUPERVISOR_ID + ";" + SUPERVISOR_NAME + ";" + TARGET_AMT + ";" + OPERATING_COST + ";" + POP_COST + ";" + PROMO_COST + ";" + MONTH_NAME + ";" + YEAR_NAME + ";" + OPT_ID + ";" + OPT_NAME + ";" + AREA_MNGR_ID + ";" + AREA_MNGR_NAME;
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
    public static string GetOutletWiseSummMktg(string group, string groupName, string fdate, string tdate, string country, string division, string zone)
    {
        string msg = "";

        try
        {
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());
            con.Open();

            string qrSr = "";
            if (group == "AllGroup" && division == "AllDivision" && zone == "AllZone")
            {
                qrSr = @"SELECT TBL3.*,TBL4.COMPANY_NAME,TBL4.GROUP_NAME FROM
                         (SELECT TBL1.*,TBL2.OUTLET_NAME,TBL2.MOBILE_NUMBER,TBL2.ZONE_ID FROM
                         (SELECT OUTLET_ID,ITEM_ID,ITEM_NAME,SUM(SALES_AMT) SALES_AMOUNT FROM T_MKTG_SALES
                         WHERE GROUP_NAME LIKE '%" + groupName + "%' AND OUTLET_ID IN(SELECT OUTLET_ID FROM T_MKTG_OUTLET WHERE COUNTRY_NAME LIKE '%" + country + "%') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY OUTLET_ID,ITEM_ID,ITEM_NAME) TBL1,(SELECT OUTLET_ID,OUTLET_NAME,MOBILE_NUMBER,ZONE_ID FROM T_MKTG_OUTLET WHERE COUNTRY_NAME='" + country + "') TBL2 WHERE TBL1.OUTLET_ID=TBL2.OUTLET_ID) TBL3, (SELECT COMPANY_NAME,GROUP_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL4 WHERE TBL3.ITEM_ID=TBL4.ITEM_ID";

            }
            else if (group == "AllGroup" && division == "AllDivision" && zone != "AllZone")
            {
                qrSr = @"SELECT TBL3.*,TBL4.COMPANY_NAME,TBL4.GROUP_NAME FROM
                         (SELECT TBL1.*,TBL2.OUTLET_NAME,TBL2.MOBILE_NUMBER,TBL2.ZONE_ID FROM
                         (SELECT OUTLET_ID,ITEM_ID,ITEM_NAME,SUM(SALES_AMT) SALES_AMOUNT FROM T_MKTG_SALES
                         WHERE GROUP_NAME LIKE '%" + groupName + "%' AND OUTLET_ID IN(SELECT OUTLET_ID FROM T_MKTG_OUTLET WHERE COUNTRY_NAME LIKE '%" + country + "%' AND ZONE_ID='" + zone + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY OUTLET_ID,ITEM_ID,ITEM_NAME) TBL1,(SELECT OUTLET_ID,OUTLET_NAME,MOBILE_NUMBER,ZONE_ID FROM T_MKTG_OUTLET WHERE COUNTRY_NAME='" + country + "') TBL2 WHERE TBL1.OUTLET_ID=TBL2.OUTLET_ID) TBL3, (SELECT COMPANY_NAME,GROUP_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL4 WHERE TBL3.ITEM_ID=TBL4.ITEM_ID";

            }
            else if (group == "AllGroup" && division != "AllDivision" && zone == "AllZone")
            {
                qrSr = @"SELECT TBL3.*,TBL4.COMPANY_NAME,TBL4.GROUP_NAME FROM
                         (SELECT TBL1.*,TBL2.OUTLET_NAME,TBL2.MOBILE_NUMBER,TBL2.ZONE_ID FROM
                         (SELECT OUTLET_ID,ITEM_ID,ITEM_NAME,SUM(SALES_AMT) SALES_AMOUNT FROM T_MKTG_SALES
                         WHERE GROUP_NAME LIKE '%" + groupName + "%' AND OUTLET_ID IN(SELECT OUTLET_ID FROM T_MKTG_OUTLET WHERE COUNTRY_NAME LIKE '%" + country + "%' AND DIVISION_ID='" + division + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY OUTLET_ID,ITEM_ID,ITEM_NAME) TBL1,(SELECT OUTLET_ID,OUTLET_NAME,MOBILE_NUMBER,ZONE_ID FROM T_MKTG_OUTLET WHERE COUNTRY_NAME='" + country + "') TBL2 WHERE TBL1.OUTLET_ID=TBL2.OUTLET_ID) TBL3, (SELECT COMPANY_NAME,GROUP_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL4 WHERE TBL3.ITEM_ID=TBL4.ITEM_ID";

            }
            else if (group == "AllGroup" && division != "AllDivision" && zone != "AllZone")
            {
                qrSr = @"SELECT TBL3.*,TBL4.COMPANY_NAME,TBL4.GROUP_NAME FROM
                         (SELECT TBL1.*,TBL2.OUTLET_NAME,TBL2.MOBILE_NUMBER,TBL2.ZONE_ID FROM
                         (SELECT OUTLET_ID,ITEM_ID,ITEM_NAME,SUM(SALES_AMT) SALES_AMOUNT FROM T_MKTG_SALES
                         WHERE GROUP_NAME LIKE '%" + groupName + "%' AND OUTLET_ID IN(SELECT OUTLET_ID FROM T_MKTG_OUTLET WHERE COUNTRY_NAME LIKE '%" + country + "%' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY OUTLET_ID,ITEM_ID,ITEM_NAME) TBL1,(SELECT OUTLET_ID,OUTLET_NAME,MOBILE_NUMBER,ZONE_ID FROM T_MKTG_OUTLET WHERE COUNTRY_NAME='" + country + "') TBL2 WHERE TBL1.OUTLET_ID=TBL2.OUTLET_ID) TBL3, (SELECT COMPANY_NAME,GROUP_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL4 WHERE TBL3.ITEM_ID=TBL4.ITEM_ID";

            }
            else if (group != "AllGroup" && division != "AllDivision" && zone != "AllZone")
            {
                qrSr = @"SELECT TBL3.*,TBL4.COMPANY_NAME,TBL4.GROUP_NAME FROM
                         (SELECT TBL1.*,TBL2.OUTLET_NAME,TBL2.MOBILE_NUMBER,TBL2.ZONE_ID FROM
                         (SELECT OUTLET_ID,ITEM_ID,ITEM_NAME,SUM(SALES_AMT) SALES_AMOUNT FROM T_MKTG_SALES
                         WHERE GROUP_NAME LIKE '%" + groupName + "%' AND OUTLET_ID IN(SELECT OUTLET_ID FROM T_MKTG_OUTLET WHERE COUNTRY_NAME LIKE '%" + country + "%' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY OUTLET_ID,ITEM_ID,ITEM_NAME) TBL1,(SELECT OUTLET_ID,OUTLET_NAME,MOBILE_NUMBER,ZONE_ID FROM T_MKTG_OUTLET WHERE COUNTRY_NAME='" + country + "') TBL2 WHERE TBL1.OUTLET_ID=TBL2.OUTLET_ID) TBL3, (SELECT COMPANY_NAME,GROUP_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL4 WHERE TBL3.ITEM_ID=TBL4.ITEM_ID";

            }
            else if (group != "AllGroup" && division == "AllDivision" && zone != "AllZone")
            {
                qrSr = @"SELECT TBL3.*,TBL4.COMPANY_NAME,TBL4.GROUP_NAME FROM
                         (SELECT TBL1.*,TBL2.OUTLET_NAME,TBL2.MOBILE_NUMBER,TBL2.ZONE_ID FROM
                         (SELECT OUTLET_ID,ITEM_ID,ITEM_NAME,SUM(SALES_AMT) SALES_AMOUNT FROM T_MKTG_SALES
                         WHERE GROUP_NAME LIKE '%" + groupName + "%' AND  OUTLET_ID IN(SELECT OUTLET_ID FROM T_MKTG_OUTLET WHERE COUNTRY_NAME LIKE '%" + country + "%' AND ZONE_ID='" + zone + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY OUTLET_ID,ITEM_ID,ITEM_NAME) TBL1,(SELECT OUTLET_ID,OUTLET_NAME,MOBILE_NUMBER,ZONE_ID FROM T_MKTG_OUTLET WHERE COUNTRY_NAME='" + country + "') TBL2 WHERE TBL1.OUTLET_ID=TBL2.OUTLET_ID) TBL3, (SELECT COMPANY_NAME,GROUP_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL4 WHERE TBL3.ITEM_ID=TBL4.ITEM_ID";

            }
            else if (group != "AllGroup" && division != "AllDivision" && zone == "AllZone")
            {
                qrSr = @"SELECT TBL3.*,TBL4.COMPANY_NAME,TBL4.GROUP_NAME FROM
                         (SELECT TBL1.*,TBL2.OUTLET_NAME,TBL2.MOBILE_NUMBER,TBL2.ZONE_ID FROM
                         (SELECT OUTLET_ID,ITEM_ID,ITEM_NAME,SUM(SALES_AMT) SALES_AMOUNT FROM T_MKTG_SALES
                         WHERE GROUP_NAME LIKE '%" + groupName + "%' AND OUTLET_ID IN(SELECT OUTLET_ID FROM T_MKTG_OUTLET WHERE COUNTRY_NAME LIKE '%" + country + "%' AND DIVISION_ID='" + division + "' ) AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY OUTLET_ID,ITEM_ID,ITEM_NAME) TBL1,(SELECT OUTLET_ID,OUTLET_NAME,MOBILE_NUMBER,ZONE_ID FROM T_MKTG_OUTLET WHERE COUNTRY_NAME='" + country + "') TBL2 WHERE TBL1.OUTLET_ID=TBL2.OUTLET_ID) TBL3, (SELECT COMPANY_NAME,GROUP_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL4 WHERE TBL3.ITEM_ID=TBL4.ITEM_ID";

            }
                       
            OracleCommand cmdP = new OracleCommand(qrSr, con);
            OracleDataAdapter daP = new OracleDataAdapter(cmdP);
            DataSet dsP = new DataSet();
            daP.Fill(dsP);
            int c = dsP.Tables[0].Rows.Count;
            if (c > 0 && dsP.Tables[0].Rows[0][0].ToString() != "")
            {
                for (int i = 0; i < c; i++)
                {
                    string outletName = dsP.Tables[0].Rows[i]["OUTLET_NAME"].ToString();
                    string mobile = dsP.Tables[0].Rows[i]["MOBILE_NUMBER"].ToString();
                    string outletId = dsP.Tables[0].Rows[i]["OUTLET_ID"].ToString();
                    string itemId = dsP.Tables[0].Rows[i]["ITEM_ID"].ToString();
                    string itemName = dsP.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                    string salesAmt = dsP.Tables[0].Rows[i]["SALES_AMOUNT"].ToString();
                     
                     
                    string zones = dsP.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string company = dsP.Tables[0].Rows[i]["COMPANY_NAME"].ToString();
                    string groups = dsP.Tables[0].Rows[i]["GROUP_NAME"].ToString();

                    string routeName = "";
                    string zoneName = "";
                    string divisionName = "";

                    string qrCom = @"SELECT T5.ROUTE_NAME,T6.ZONE_NAME,T5.DIVISION_NAME FROM
                                    (SELECT ZONE_ID,ZONE_NAME FROM T_MKTG_ZONE) T6,
                                    (SELECT T3.ROUTE_NAME,T4.DIVISION_NAME,T4.ZONE_ID FROM
                                    (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) T3,
                                    (SELECT T1.DIVISION_NAME,T2.ROUTE_ID,T2.ZONE_ID FROM
                                    (SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T1, 
                                    (SELECT DIVISION_ID,ROUTE_ID,ZONE_ID FROM T_MKTG_OUTLET WHERE OUTLET_ID='" + outletId + "') T2 WHERE T1.DIVISION_ID=T2.DIVISION_ID) T4 WHERE T3.ROUTE_ID=T4.ROUTE_ID) T5 WHERE T6.ZONE_ID=T5.ZONE_ID";
                                        
                    OracleCommand cmdC = new OracleCommand(qrCom, con);
                    OracleDataAdapter daC = new OracleDataAdapter(cmdC);
                    DataSet dsC = new DataSet();
                    daC.Fill(dsC);
                    int cc = dsC.Tables[0].Rows.Count;
                    if (cc > 0 && dsC.Tables[0].Rows[0][0].ToString() != "")
                    {
                        routeName = dsC.Tables[0].Rows[0]["ROUTE_NAME"].ToString();
                        zoneName = dsC.Tables[0].Rows[0]["ZONE_NAME"].ToString();
                        divisionName = dsC.Tables[0].Rows[0]["DIVISION_NAME"].ToString();
                    }

                    msg = msg + ";" + divisionName + ";" + zoneName + ";" + routeName + ";" + outletName + "(" + mobile + ")" + ";" + itemName + ";" + salesAmt + ";" + company + ";" + groups;
                    
                }
            }

            con.Close();
        }
        catch (Exception ex) { }

        return msg;
    }


    [WebMethod]
    public static string GetProductWiseSummMktg(string group, string groupName, string fdate, string tdate, string country, string division, string zone)
    {
        string msg = "";

        try
        {
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass(); 
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());
            con.Open();

            string qrSr = "";

            if (group == "AllGroup" && division == "AllDivision" && zone == "AllZone")
            {
//                qrSr = @"SELECT TBL5.*,(TBL6.TOTAL_COST+TBL5.BLCK_COST) SERVICE_COST FROM
//                            (SELECT DISTINCT TBL3.*,TBL4.ROUTE_NAME FROM
//                            (SELECT TBL2.COMPANY_NAME,TBL1.* FROM
//                            (SELECT SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID, SUM(SALES_AMT) SALES_AMOUNT,SUM(BLOCK_SIZE) BLCK_SIZE,SUM(BLOCK_COST) BLCK_COST,COUNT(BLOCK_SIZE) TOTAL_BLOCK
//                            FROM T_MKTG_SALES WHERE GROUP_NAME LIKE '%" + groupName + "%' AND SR_ID IN(SELECT SR_ID FROM T_MKTG_SR_INFO WHERE COUNTRY_NAME LIKE '%" + country + "%' AND GROUP_ID = '" + group + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID) TBL1, ";

//                qrSr = qrSr + @"(SELECT COMPANY_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL2
//                            WHERE TBL1.ITEM_ID=TBL2.ITEM_ID) TBL3,
//                            (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) TBL4
//                            WHERE TBL3.ROUTE_ID=TBL4.ROUTE_ID) TBL5,
//                            (SELECT SR_ID,(OPERATING_COST+POP_COST+PROMO_COST) TOTAL_COST FROM T_MKTG_TARGET) TBL6
//                            WHERE TBL5.SR_ID=TBL6.SR_ID";

                qrSr = @"SELECT TBL5.* FROM
                            (SELECT DISTINCT TBL3.*,TBL4.ROUTE_NAME FROM
                            (SELECT TBL2.COMPANY_NAME,TBL1.* FROM
                            (SELECT SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID, SUM(SALES_AMT) SALES_AMOUNT,SUM(BLOCK_SIZE) BLCK_SIZE,SUM(BLOCK_COST) BLCK_COST,COUNT(BLOCK_SIZE) TOTAL_BLOCK
                            FROM T_MKTG_SALES WHERE GROUP_NAME LIKE '%" + groupName + "%' AND SR_ID IN(SELECT SR_ID FROM T_MKTG_SR_INFO WHERE COUNTRY_NAME LIKE '%" + country + "%' AND GROUP_ID = '" + group + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID) TBL1, ";

                qrSr = qrSr + @"(SELECT COMPANY_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL2
                            WHERE TBL1.ITEM_ID=TBL2.ITEM_ID) TBL3,
                            (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) TBL4
                            WHERE TBL3.ROUTE_ID=TBL4.ROUTE_ID) TBL5";

            }
            else if (group == "AllGroup" && division == "AllDivision" && zone != "AllZone")
            {
//                qrSr = @"SELECT TBL5.*,(TBL6.TOTAL_COST+TBL5.BLCK_COST) SERVICE_COST FROM
//                            (SELECT DISTINCT TBL3.*,TBL4.ROUTE_NAME FROM
//                            (SELECT TBL2.COMPANY_NAME,TBL1.* FROM
//                            (SELECT SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID, SUM(SALES_AMT) SALES_AMOUNT,SUM(BLOCK_SIZE) BLCK_SIZE,SUM(BLOCK_COST) BLCK_COST,COUNT(BLOCK_SIZE) TOTAL_BLOCK
//                            FROM T_MKTG_SALES WHERE GROUP_NAME LIKE '%" + groupName + "%' AND SR_ID IN(SELECT SR_ID FROM T_MKTG_SR_INFO WHERE COUNTRY_NAME LIKE '%" + country + "%' AND GROUP_ID = '" + group + "' AND ZONE_ID='" + zone + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID) TBL1, ";

//                qrSr = qrSr + @"(SELECT COMPANY_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL2
//                            WHERE TBL1.ITEM_ID=TBL2.ITEM_ID) TBL3,
//                            (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) TBL4
//                            WHERE TBL3.ROUTE_ID=TBL4.ROUTE_ID) TBL5,
//                            (SELECT SR_ID,(OPERATING_COST+POP_COST+PROMO_COST) TOTAL_COST FROM T_MKTG_TARGET) TBL6
//                            WHERE TBL5.SR_ID=TBL6.SR_ID";
                qrSr = @"SELECT TBL5.* FROM
                            (SELECT DISTINCT TBL3.*,TBL4.ROUTE_NAME FROM
                            (SELECT TBL2.COMPANY_NAME,TBL1.* FROM
                            (SELECT SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID, SUM(SALES_AMT) SALES_AMOUNT,SUM(BLOCK_SIZE) BLCK_SIZE,SUM(BLOCK_COST) BLCK_COST,COUNT(BLOCK_SIZE) TOTAL_BLOCK
                            FROM T_MKTG_SALES WHERE GROUP_NAME LIKE '%" + groupName + "%' AND SR_ID IN(SELECT SR_ID FROM T_MKTG_SR_INFO WHERE COUNTRY_NAME LIKE '%" + country + "%' AND GROUP_ID = '" + group + "' AND ZONE_ID='" + zone + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID) TBL1, ";

                qrSr = qrSr + @"(SELECT COMPANY_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL2
                            WHERE TBL1.ITEM_ID=TBL2.ITEM_ID) TBL3,
                            (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) TBL4
                            WHERE TBL3.ROUTE_ID=TBL4.ROUTE_ID) TBL5";
            }
            else if (group == "AllGroup" && division != "AllDivision" && zone == "AllZone")
            {
//                qrSr = @"SELECT TBL5.*,(TBL6.TOTAL_COST+TBL5.BLCK_COST) SERVICE_COST FROM
//                            (SELECT DISTINCT TBL3.*,TBL4.ROUTE_NAME FROM
//                            (SELECT TBL2.COMPANY_NAME,TBL1.* FROM
//                            (SELECT SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID, SUM(SALES_AMT) SALES_AMOUNT,SUM(BLOCK_SIZE) BLCK_SIZE,SUM(BLOCK_COST) BLCK_COST,COUNT(BLOCK_SIZE) TOTAL_BLOCK
//                            FROM T_MKTG_SALES WHERE GROUP_NAME LIKE '%" + groupName + "%' AND SR_ID IN(SELECT SR_ID FROM T_MKTG_SR_INFO WHERE COUNTRY_NAME LIKE'%" + country + "%' AND GROUP_ID = '" + group + "' AND DIVISION_ID='" + division + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID) TBL1, ";

//                qrSr = qrSr + @"(SELECT COMPANY_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL2
//                            WHERE TBL1.ITEM_ID=TBL2.ITEM_ID) TBL3,
//                            (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) TBL4
//                            WHERE TBL3.ROUTE_ID=TBL4.ROUTE_ID) TBL5,
//                            (SELECT SR_ID,(OPERATING_COST+POP_COST+PROMO_COST) TOTAL_COST FROM T_MKTG_TARGET) TBL6
//                            WHERE TBL5.SR_ID=TBL6.SR_ID";

                qrSr = @"SELECT TBL5.* FROM
                            (SELECT DISTINCT TBL3.*,TBL4.ROUTE_NAME FROM
                            (SELECT TBL2.COMPANY_NAME,TBL1.* FROM
                            (SELECT SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID, SUM(SALES_AMT) SALES_AMOUNT,SUM(BLOCK_SIZE) BLCK_SIZE,SUM(BLOCK_COST) BLCK_COST,COUNT(BLOCK_SIZE) TOTAL_BLOCK
                            FROM T_MKTG_SALES WHERE GROUP_NAME LIKE '%" + groupName + "%' AND SR_ID IN(SELECT SR_ID FROM T_MKTG_SR_INFO WHERE COUNTRY_NAME LIKE'%" + country + "%' AND GROUP_ID = '" + group + "' AND DIVISION_ID='" + division + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID) TBL1, ";

                qrSr = qrSr + @"(SELECT COMPANY_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL2
                            WHERE TBL1.ITEM_ID=TBL2.ITEM_ID) TBL3,
                            (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) TBL4
                            WHERE TBL3.ROUTE_ID=TBL4.ROUTE_ID) TBL5";
            }
            else if (group == "AllGroup" && division != "AllDivision" && zone != "AllZone")
            {
//                qrSr = @"SELECT TBL5.*,(TBL6.TOTAL_COST+TBL5.BLCK_COST) SERVICE_COST FROM
//                            (SELECT DISTINCT TBL3.*,TBL4.ROUTE_NAME FROM
//                            (SELECT TBL2.COMPANY_NAME,TBL1.* FROM
//                            (SELECT SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID, SUM(SALES_AMT) SALES_AMOUNT,SUM(BLOCK_SIZE) BLCK_SIZE,SUM(BLOCK_COST) BLCK_COST,COUNT(BLOCK_SIZE) TOTAL_BLOCK
//                            FROM T_MKTG_SALES WHERE GROUP_NAME LIKE '%" + groupName + "%' AND SR_ID IN(SELECT SR_ID FROM T_MKTG_SR_INFO WHERE COUNTRY_NAME LIKE'%" + country + "%' AND GROUP_ID = '" + group + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID) TBL1, ";

//                qrSr = qrSr + @"(SELECT COMPANY_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL2
//                            WHERE TBL1.ITEM_ID=TBL2.ITEM_ID) TBL3,
//                            (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) TBL4
//                            WHERE TBL3.ROUTE_ID=TBL4.ROUTE_ID) TBL5,
//                            (SELECT SR_ID,(OPERATING_COST+POP_COST+PROMO_COST) TOTAL_COST FROM T_MKTG_TARGET) TBL6
//                            WHERE TBL5.SR_ID=TBL6.SR_ID";
                qrSr = @"SELECT TBL5.* FROM
                            (SELECT DISTINCT TBL3.*,TBL4.ROUTE_NAME FROM
                            (SELECT TBL2.COMPANY_NAME,TBL1.* FROM
                            (SELECT SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID, SUM(SALES_AMT) SALES_AMOUNT,SUM(BLOCK_SIZE) BLCK_SIZE,SUM(BLOCK_COST) BLCK_COST,COUNT(BLOCK_SIZE) TOTAL_BLOCK
                            FROM T_MKTG_SALES WHERE GROUP_NAME LIKE '%" + groupName + "%' AND SR_ID IN(SELECT SR_ID FROM T_MKTG_SR_INFO WHERE COUNTRY_NAME LIKE'%" + country + "%' AND GROUP_ID = '" + group + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID) TBL1, ";

                qrSr = qrSr + @"(SELECT COMPANY_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL2
                            WHERE TBL1.ITEM_ID=TBL2.ITEM_ID) TBL3,
                            (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) TBL4
                            WHERE TBL3.ROUTE_ID=TBL4.ROUTE_ID) TBL5";
            }
            else if (group != "AllGroup" && division != "AllDivision" && zone != "AllZone")
            {
//                qrSr = @"SELECT TBL5.*,(TBL6.TOTAL_COST+TBL5.BLCK_COST) SERVICE_COST FROM
//                            (SELECT DISTINCT TBL3.*,TBL4.ROUTE_NAME FROM
//                            (SELECT TBL2.COMPANY_NAME,TBL1.* FROM
//                            (SELECT SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID, SUM(SALES_AMT) SALES_AMOUNT,SUM(BLOCK_SIZE) BLCK_SIZE,SUM(BLOCK_COST) BLCK_COST,COUNT(BLOCK_SIZE) TOTAL_BLOCK
//                            FROM T_MKTG_SALES WHERE GROUP_NAME LIKE '%" + groupName + "%' AND SR_ID IN(SELECT SR_ID FROM T_MKTG_SR_INFO WHERE COUNTRY_NAME LIKE '%" + country + "%' AND GROUP_ID = '" + group + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID) TBL1, ";

//                qrSr = qrSr + @"(SELECT COMPANY_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL2
//                            WHERE TBL1.ITEM_ID=TBL2.ITEM_ID) TBL3,
//                            (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) TBL4
//                            WHERE TBL3.ROUTE_ID=TBL4.ROUTE_ID) TBL5,
//                            (SELECT SR_ID,(OPERATING_COST+POP_COST+PROMO_COST) TOTAL_COST FROM T_MKTG_TARGET) TBL6
//                            WHERE TBL5.SR_ID=TBL6.SR_ID";
                qrSr = @"SELECT TBL5.* FROM
                            (SELECT DISTINCT TBL3.*,TBL4.ROUTE_NAME FROM
                            (SELECT TBL2.COMPANY_NAME,TBL1.* FROM
                            (SELECT SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID, SUM(SALES_AMT) SALES_AMOUNT,SUM(BLOCK_SIZE) BLCK_SIZE,SUM(BLOCK_COST) BLCK_COST,COUNT(BLOCK_SIZE) TOTAL_BLOCK
                            FROM T_MKTG_SALES WHERE GROUP_NAME LIKE '%" + groupName + "%' AND SR_ID IN(SELECT SR_ID FROM T_MKTG_SR_INFO WHERE COUNTRY_NAME LIKE '%" + country + "%' AND GROUP_ID = '" + group + "' AND DIVISION_ID='" + division + "' AND ZONE_ID='" + zone + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID) TBL1, ";

                qrSr = qrSr + @"(SELECT COMPANY_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL2
                            WHERE TBL1.ITEM_ID=TBL2.ITEM_ID) TBL3,
                            (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) TBL4
                            WHERE TBL3.ROUTE_ID=TBL4.ROUTE_ID) TBL5";
            }
            else if (group != "AllGroup" && division == "AllDivision" && zone != "AllZone")
            {
//                qrSr = @"SELECT TBL5.*,(TBL6.TOTAL_COST+TBL5.BLCK_COST) SERVICE_COST FROM
//                            (SELECT DISTINCT TBL3.*,TBL4.ROUTE_NAME FROM
//                            (SELECT TBL2.COMPANY_NAME,TBL1.* FROM
//                            (SELECT SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID, SUM(SALES_AMT) SALES_AMOUNT,SUM(BLOCK_SIZE) BLCK_SIZE,SUM(BLOCK_COST) BLCK_COST,COUNT(BLOCK_SIZE) TOTAL_BLOCK
//                            FROM T_MKTG_SALES WHERE GROUP_NAME LIKE '%" + groupName + "%' AND SR_ID IN(SELECT SR_ID FROM T_MKTG_SR_INFO WHERE COUNTRY_NAME LIKE'%" + country + "%' AND GROUP_ID = '" + group + "' AND ZONE_ID='" + zone + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID) TBL1, ";

//                qrSr = qrSr + @"(SELECT COMPANY_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL2
//                            WHERE TBL1.ITEM_ID=TBL2.ITEM_ID) TBL3,
//                            (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) TBL4
//                            WHERE TBL3.ROUTE_ID=TBL4.ROUTE_ID) TBL5,
//                            (SELECT SR_ID,(OPERATING_COST+POP_COST+PROMO_COST) TOTAL_COST FROM T_MKTG_TARGET) TBL6
//                            WHERE TBL5.SR_ID=TBL6.SR_ID";
                qrSr = @"SELECT TBL5.* FROM
                            (SELECT DISTINCT TBL3.*,TBL4.ROUTE_NAME FROM
                            (SELECT TBL2.COMPANY_NAME,TBL1.* FROM
                            (SELECT SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID, SUM(SALES_AMT) SALES_AMOUNT,SUM(BLOCK_SIZE) BLCK_SIZE,SUM(BLOCK_COST) BLCK_COST,COUNT(BLOCK_SIZE) TOTAL_BLOCK
                            FROM T_MKTG_SALES WHERE GROUP_NAME LIKE '%" + groupName + "%' AND SR_ID IN(SELECT SR_ID FROM T_MKTG_SR_INFO WHERE COUNTRY_NAME LIKE'%" + country + "%' AND GROUP_ID = '" + group + "' AND ZONE_ID='" + zone + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID) TBL1, ";

                qrSr = qrSr + @"(SELECT COMPANY_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL2
                            WHERE TBL1.ITEM_ID=TBL2.ITEM_ID) TBL3,
                            (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) TBL4
                            WHERE TBL3.ROUTE_ID=TBL4.ROUTE_ID) TBL5";
            }
            else if (group != "AllGroup" && division != "AllDivision" && zone == "AllZone")
            {
//                qrSr = @"SELECT TBL5.*,(TBL6.TOTAL_COST+TBL5.BLCK_COST) SERVICE_COST FROM
//                            (SELECT DISTINCT TBL3.*,TBL4.ROUTE_NAME FROM
//                            (SELECT TBL2.COMPANY_NAME,TBL1.* FROM
//                            (SELECT SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID, SUM(SALES_AMT) SALES_AMOUNT,SUM(BLOCK_SIZE) BLCK_SIZE,SUM(BLOCK_COST) BLCK_COST,COUNT(BLOCK_SIZE) TOTAL_BLOCK
//                            FROM T_MKTG_SALES WHERE GROUP_NAME LIKE '%" + groupName + "%' AND SR_ID IN(SELECT SR_ID FROM T_MKTG_SR_INFO WHERE COUNTRY_NAME LIKE'%" + country + "%' AND GROUP_ID = '" + group + "' AND DIVISION_ID='" + division + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID) TBL1, ";

//                qrSr = qrSr + @"(SELECT COMPANY_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL2
//                            WHERE TBL1.ITEM_ID=TBL2.ITEM_ID) TBL3,
//                            (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) TBL4
//                            WHERE TBL3.ROUTE_ID=TBL4.ROUTE_ID) TBL5,
//                            (SELECT SR_ID,(OPERATING_COST+POP_COST+PROMO_COST) TOTAL_COST FROM T_MKTG_TARGET) TBL6
//                            WHERE TBL5.SR_ID=TBL6.SR_ID";

                qrSr = @"SELECT TBL5.* FROM
                            (SELECT DISTINCT TBL3.*,TBL4.ROUTE_NAME FROM
                            (SELECT TBL2.COMPANY_NAME,TBL1.* FROM
                            (SELECT SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID, SUM(SALES_AMT) SALES_AMOUNT,SUM(BLOCK_SIZE) BLCK_SIZE,SUM(BLOCK_COST) BLCK_COST,COUNT(BLOCK_SIZE) TOTAL_BLOCK
                            FROM T_MKTG_SALES WHERE GROUP_NAME LIKE '%" + groupName + "%' AND SR_ID IN(SELECT SR_ID FROM T_MKTG_SR_INFO WHERE COUNTRY_NAME LIKE'%" + country + "%' AND GROUP_ID = '" + group + "' AND DIVISION_ID='" + division + "') AND ENTRY_DATE BETWEEN TO_DATE('" + fdate + "','DD/MM/YYYY') AND TO_DATE('" + tdate + "','DD/MM/YYYY') GROUP BY SR_ID,GROUP_NAME,ITEM_NAME,ITEM_ID,ROUTE_ID) TBL1, ";

                qrSr = qrSr + @"(SELECT COMPANY_NAME,ITEM_ID FROM T_MKTG_ITEM) TBL2
                            WHERE TBL1.ITEM_ID=TBL2.ITEM_ID) TBL3,
                            (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) TBL4
                            WHERE TBL3.ROUTE_ID=TBL4.ROUTE_ID) TBL5";

            }           

            OracleCommand cmdP = new OracleCommand(qrSr, con);
            OracleDataAdapter daP = new OracleDataAdapter(cmdP);
            DataSet dsP = new DataSet();
            daP.Fill(dsP);
            int c = dsP.Tables[0].Rows.Count;
            if (c > 0 && dsP.Tables[0].Rows[0][0].ToString() != "")
            {
                for (int i = 0; i < c; i++)
                {
                    string company = dsP.Tables[0].Rows[i][0].ToString();
                    string srid = dsP.Tables[0].Rows[i][1].ToString();
                    string groupNames = dsP.Tables[0].Rows[i][2].ToString();
                    string itemName = dsP.Tables[0].Rows[i][3].ToString();
                    string itemid = dsP.Tables[0].Rows[i][4].ToString();
                    string routeId = dsP.Tables[0].Rows[i][5].ToString();
                    string salesAmt = dsP.Tables[0].Rows[i][6].ToString();
                    string blockSize = dsP.Tables[0].Rows[i][7].ToString();
                    string blockCost = dsP.Tables[0].Rows[i][8].ToString() == "" ? "0" : dsP.Tables[0].Rows[i][8].ToString();
                    string totalBlock = dsP.Tables[0].Rows[i][9].ToString();
                    string routeName = dsP.Tables[0].Rows[i][10].ToString();

                    string serviceCost = "Service cost need to set";
                    string qrSC = @"SELECT SR_ID,(OPERATING_COST+POP_COST+PROMO_COST) TOTAL_COST FROM T_MKTG_TARGET WHERE SR_ID='" + srid + "'";
                    OracleCommand cmdSC = new OracleCommand(qrSC, con);
                    OracleDataAdapter daSC = new OracleDataAdapter(cmdSC);
                    DataSet dsSC = new DataSet();
                    daSC.Fill(dsSC);
                    int cSc = dsSC.Tables[0].Rows.Count;
                    if (cSc > 0 && dsSC.Tables[0].Rows[0][0].ToString() != "")
                    {
                        serviceCost = (Convert.ToDouble(dsSC.Tables[0].Rows[0]["TOTAL_COST"].ToString()) + Convert.ToDouble(blockCost)).ToString();                        
                    }                    

                    string zones = "";
                    string divisions = "";
                    string qrCom = @"SELECT T1.DIVISION_NAME,T2.ZONE_NAME FROM
                                    (SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T1,                           
                                    (SELECT DIVISION_ID,ZONE_NAME FROM T_MKTG_ZONE WHERE ZONE_ID IN(SELECT DISTINCT ZONE_ID FROM T_MKTG_ROUTE WHERE ROUTE_ID='" + routeId + "'))T2 WHERE T1.DIVISION_ID=T2.DIVISION_ID";

                    OracleCommand cmdC = new OracleCommand(qrCom, con);
                    OracleDataAdapter daC = new OracleDataAdapter(cmdC);
                    DataSet dsC = new DataSet();
                    daC.Fill(dsC);
                    int cc = dsC.Tables[0].Rows.Count;
                    if (cc > 0 && dsC.Tables[0].Rows[0][0].ToString() != "")
                    {
                        zones = dsC.Tables[0].Rows[0]["ZONE_NAME"].ToString();
                        divisions = dsC.Tables[0].Rows[0]["DIVISION_NAME"].ToString();
                    }

                    msg = msg + ";" + company + ";" + groupNames + ";" + divisions + ";" + zones + ";" + routeName + ";" + itemName + ";" + totalBlock + ";" + blockSize + ";" + serviceCost + ";" + salesAmt;
                }
            }

            con.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
       
                        
    [WebMethod(EnableSession = true)]
    public static string ShowHRMonitoringReport(string group, string groupName, string country, string division, string zone, string fromDate, string toDate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string qr = "";

            string srInfo = "";
            string grInfo = "";
            string dvInfo = "";
            string znInfo = "";
            
            if (group == "AllGroup") 
            {
                grInfo = " GROUP_NAME IN('PRAN-CS-Others','Fridge','RFL')";
            }
            else
            {
                grInfo = " GROUP_NAME LIKE '%" + groupName + "%'";
            }

            if (division == "AllDivision") 
            {
                dvInfo = " DIVISION_ID IN(SELECT DIVISION_ID FROM T_MKTG_DIVISION)";
            }
            else
            {
                dvInfo = " DIVISION_ID='" + division + "'";
            }


            if (zone == "AllZone") 
            {
                znInfo = " ZONE_ID IN(SELECT ZONE_ID FROM T_MKTG_ZONE)";
            }
            else
            {
                znInfo = " ZONE_ID='" + zone + "'";
            }

            srInfo = @"SELECT T3.*,T4.ROUTE_NAME FROM
                    (SELECT T1.SR_ID,T1.SR_NAME,T1.MOBILE_NO,T1.GROUP_NAME,T2.IN_TIME,T2.OUT_TIME,T2.ROUTE_ID FROM  
                    (SELECT SR_ID,SR_NAME,MOBILE_NO,GROUP_NAME FROM T_MKTG_SR_INFO WHERE COUNTRY_NAME = '" + country + "' AND " + grInfo + " AND " + dvInfo + " AND " + znInfo + ") T1, ";
            srInfo = srInfo + @" (SELECT ROUTE_ID,ROUT_SRID,MIN(IMAGE_CAPTURE_DT) IN_TIME,MAX(IMAGE_CAPTURE_DT) OUT_TIME FROM OUTLET_INFO 
                    WHERE ENTRY_DATE BETWEEN TO_DATE('" + fromDate + "','DD/MM/YYYY') AND TO_DATE('" + toDate + "','DD/MM/YYYY') GROUP BY ROUTE_ID,ROUT_SRID ) T2 WHERE T1.SR_ID=T2.ROUT_SRID) T3, (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) T4 WHERE T3.ROUTE_ID=T4.ROUTE_ID";


            OracleCommand cmdSR = new OracleCommand(srInfo, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int K = ds.Tables[0].Rows.Count;
            if (K > 0 && ds.Tables[0].Rows[0]["SR_ID"].ToString() != "")
            {
                for (int i = 0; i < K; i++)
                {
                    string SR_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string SR_NAME = ds.Tables[0].Rows[i]["SR_NAME"].ToString();
                    string MOBILE_NO = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string GROUP_NAME = ds.Tables[0].Rows[i]["GROUP_NAME"].ToString();
                    //string IN_TIME = ds.Tables[0].Rows[i]["IN_TIME"].ToString();
                    //string OUT_TIME = ds.Tables[0].Rows[i]["OUT_TIME"].ToString();
                    string ROUTE_ID = ds.Tables[0].Rows[i]["ROUTE_ID"].ToString();
                    string ROUTE_NAME = ds.Tables[0].Rows[i]["ROUTE_NAME"].ToString();

                    string IN_TIME = "";
                    string OUT_TIME = "";
                    string qrT = @"SELECT ROUTE_ID,ROUT_SRID,MIN(IMAGE_CAPTURE_DT) IN_TIME,MAX(IMAGE_CAPTURE_DT) OUT_TIME FROM OUTLET_INFO 
                                 WHERE ROUT_SRID='" + SR_ID + "' AND ROUTE_ID='" + ROUTE_ID + "' AND ENTRY_DATE BETWEEN TO_DATE('" + fromDate + "','DD/MM/YYYY') AND TO_DATE('" + toDate + "','DD/MM/YYYY') GROUP BY ROUTE_ID,ROUT_SRID order by IN_TIME";
                    OracleCommand cmdT = new OracleCommand(qrT, conn);
                    OracleDataAdapter daT = new OracleDataAdapter(cmdT);
                    DataSet dvT = new DataSet();
                    daT.Fill(dvT);
                    int t = dvT.Tables[0].Rows.Count;
                    if (t > 0 && dvT.Tables[0].Rows[0]["ROUTE_ID"].ToString() != "")
                    {
                        IN_TIME = dvT.Tables[0].Rows[0]["IN_TIME"].ToString();
                        OUT_TIME = dvT.Tables[0].Rows[t-1]["OUT_TIME"].ToString();
                    }

                    string visitedOutlet = "";
                    string totalOutlet = "";
                    string avgVisitedOutlet = "";

                    string qrV = @"SELECT T1.VISITED_OUTLET,T2.TOTAL_OUTLET FROM
                                (SELECT DISTINCT ROUTE_ID,COUNT(DISTINCT OUTLET_ID) VISITED_OUTLET FROM OUTLET_INFO 
                                WHERE ROUT_SRID='" + SR_ID + "' AND ROUTE_ID='" + ROUTE_ID + "' AND ENTRY_DATE BETWEEN TO_DATE('" + fromDate + "','DD/MM/YYYY') AND TO_DATE('" + toDate + "','DD/MM/YYYY') GROUP BY ROUTE_ID) T1, ";
                    qrV = qrV + @" (SELECT ROUTE_ID,COUNT(OUTLET_ID) TOTAL_OUTLET FROM T_MKTG_OUTLET WHERE ROUTE_ID='" + ROUTE_ID + "' AND STATUS='Y' GROUP BY ROUTE_ID) T2 WHERE T1.ROUTE_ID=T2.ROUTE_ID";

                    OracleCommand cmdv = new OracleCommand(qrV, conn);
                    OracleDataAdapter dav = new OracleDataAdapter(cmdv);
                    DataSet dv = new DataSet();
                    dav.Fill(dv);
                    int v = dv.Tables[0].Rows.Count;
                    if (v > 0 && dv.Tables[0].Rows[0]["VISITED_OUTLET"].ToString() != "")
                    {
                        visitedOutlet = dv.Tables[0].Rows[0]["VISITED_OUTLET"].ToString();
                        totalOutlet = dv.Tables[0].Rows[0]["TOTAL_OUTLET"].ToString();
                        avgVisitedOutlet = DisplayPercentage(Convert.ToDouble(visitedOutlet) / Convert.ToDouble(totalOutlet));                        
                    }

                    msg = msg + ";" + SR_ID + ";" + SR_NAME + ";" + MOBILE_NO + ";" + GROUP_NAME + ";" + IN_TIME + ";" + OUT_TIME + ";" + visitedOutlet + ";" + totalOutlet + ";" + avgVisitedOutlet + ";" + ROUTE_NAME;
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
    public static string GetRoute(string zoneId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE
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
    public static string GetZones(string division)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT ZONE_ID,ZONE_NAME FROM T_MKTG_ZONE
                            WHERE DIVISION_ID='" + division + "'";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string comId = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string comName = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();
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
    public static string AddOutletInfo(string outletId, string outletName, string outletAddress, string outletPropritorName, string outletNameBangla, string outletAddressBangla, string outletPropritorNameBangla, string outletPhone, string outletEmailAddress, string country, string division, string zone, string route, string fridge, string signboard, string rack, string category, string grade, string status, string product)
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

                query = @"INSERT INTO T_MKTG_OUTLET(OUTLET_ID,OUTLET_NAME,OUTLET_ADDRESS,PROPRITOR_NAME,MOBILE_NUMBER,EMAIL_ADDRESS,COUNTRY_NAME,DIVISION_ID,ZONE_ID,ROUTE_ID,FRIDGE,SIGNBOARD,RACK,STATUS,ENTRY_DATE,ENTRY_BY, OUTLET_BL_NAME,ADDRESS_BL,PROPRITOR_BL,CATEGORY_NAME,GRADE,PRODUCTS)
                            VALUES ((SALES.SEQ_MKTG_OUTLET_ID.NEXTVAL),'" + outletName + "','" + outletAddress + "','" + outletPropritorName + "','" + outletPhone + "','" + outletEmailAddress + "','" + country + "','" + division + "','" + zone + "','" + route + "','" + fridge + "','" + signboard + "','" + rack + "','" + status + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + outletNameBangla + "','" + outletAddressBangla + "','" + outletPropritorNameBangla + "','" + category + "','" + grade + "','" + product + "')";
            }
            else
            {
                query = @"UPDATE T_MKTG_OUTLET SET OUTLET_NAME='" + outletName + "',OUTLET_ADDRESS='" + outletAddress + "',PROPRITOR_NAME='" + outletPropritorName + "',MOBILE_NUMBER='" + outletPhone + "',EMAIL_ADDRESS='" + outletEmailAddress + "',COUNTRY_NAME='" + country + "',DIVISION_ID='" + division + "',ZONE_ID='" + zone + "',ROUTE_ID='" + route + "',FRIDGE='" + fridge + "',SIGNBOARD='" + signboard + "',RACK='" + rack + "',STATUS='" + status + "',OUTLET_BL_NAME='" + outletNameBangla + "',ADDRESS_BL='" + outletAddressBangla + "',PROPRITOR_BL='" + outletPropritorNameBangla + "',CATEGORY_NAME='" + category + "',GRADE='" + grade + "',PRODUCTS='" + product + "' WHERE OUTLET_ID='" + outletId + "'";
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
                        (SELECT * FROM T_MKTG_OUTLET) T1,
                        (SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T2 WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                        (SELECT * FROM T_MKTG_ZONE) T4
                        WHERE T3.ZONE_ID=T4.ZONE_ID) T5,
                        (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) T6
                        WHERE T5.ROUTE_ID=T6.ROUTE_ID";
            }
            else
            {
                query = @"SELECT T5.*,T6.ROUTE_NAME FROM
                        (SELECT T3.*,T4.ZONE_NAME FROM
                        (SELECT T1.*,T2.DIVISION_NAME FROM
                        (SELECT * FROM T_MKTG_OUTLET WHERE OUTLET_ID='" + outlet + "') T1, ";
                query = query + @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T2 WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                        (SELECT * FROM T_MKTG_ZONE) T4
                        WHERE T3.ZONE_ID=T4.ZONE_ID) T5,
                        (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) T6
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

                    msg = msg + ";" + OUTLET_ID + ";" + OUTLET_NAME + ";" + OUTLET_ADDRESS + ";" + PROPRITOR_NAME + ";" + MOBILE_NUMBER + ";" + EMAIL_ADDRESS + ";" + COUNTRY_NAME + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + ZONE_ID + ";" + ZONE_NAME + ";" + ROUTE_ID + ";" + ROUTE_NAME + ";" + FRIDGE + ";" + SIGNBOARD + ";" + RACK + ";" + CATEGORY_NAME + ";" + GRADE + ";" + STATUS + ";" + PRODUCTS;
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
                        (SELECT * FROM T_MKTG_OUTLET WHERE ROUTE_ID='" + routeId + "') T1, ";
            query = query + @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T2 WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                        (SELECT * FROM T_MKTG_ZONE) T4
                        WHERE T3.ZONE_ID=T4.ZONE_ID) T5,
                        (SELECT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE) T6
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

                    msg = msg + ";" + OUTLET_ID + ";" + OUTLET_NAME + ";" + OUTLET_ADDRESS + ";" + PROPRITOR_NAME + ";" + MOBILE_NUMBER + ";" + EMAIL_ADDRESS + ";" + COUNTRY_NAME + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + ZONE_ID + ";" + ZONE_NAME + ";" + ROUTE_ID + ";" + ROUTE_NAME + ";" + FRIDGE + ";" + SIGNBOARD + ";" + RACK + ";" + CATEGORY_NAME + ";" + GRADE + ";" + STATUS + ";" + PRODUCTS;
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
    public static string AddRegionInfo(string groupId, string groupName, string regionId, string regionName, string divisionId, string divisionName, string country, string status)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT REGION_ID FROM T_MKTG_REGION WHERE REGION_ID='" + regionId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["REGION_ID"].ToString() != "")
            {
                string qrS = @"UPDATE T_MKTG_REGION SET REGION_NAME='" + regionName + "',GROUP_ID='" + groupId + "',GROUP_NAME='" + groupName + "',DIVISION_ID='" + divisionId + "',DIVISION_NAME='" + divisionName + "',COUNTRY_NAME='" + country + "',STATUS='" + status + "' WHERE REGION_ID='" + regionId + "'";
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

                string query = @"INSERT INTO T_MKTG_REGION(GROUP_ID,GROUP_NAME,REGION_ID,REGION_NAME,COUNTRY_NAME,STATUS,ENTRY_DATE,ENTRY_BY,DIVISION_ID,DIVISION_NAME)
                            VALUES ('" + groupId + "','" + groupName + "','" + regionId + "','" + regionName + "','" + country + "','" + status + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + divisionId + "','" + divisionName + "')";
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


 [WebMethod]
 public static string GetRegionInfo(string regionId)
 {
     string msg = "";
     OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

     try
     {
         OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
         conn.Open();
         string query = "";
         if (regionId == "regionId")
         {
             query = @"SELECT * FROM T_MKTG_REGION";
         }
         else
         {
             query = @"SELECT * FROM T_MKTG_REGION WHERE REGION_ID='" + regionId + "'";

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
                 string GROUP_ID = ds.Tables[0].Rows[i]["GROUP_ID"].ToString();
                 string GROUP_NAME = ds.Tables[0].Rows[i]["GROUP_NAME"].ToString();
                 string REGION_ID = ds.Tables[0].Rows[i]["REGION_ID"].ToString();
                 string REGION_NAME = ds.Tables[0].Rows[i]["REGION_NAME"].ToString();
                 string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                 string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                 string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                 string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                 msg = msg + ";" + GROUP_ID + ";" + GROUP_NAME + ";" + REGION_ID + ";" + REGION_NAME + ";" + COUNTRY_NAME + ";" + STATUS + ";" + DIVISION_ID + ";" + DIVISION_NAME;
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
 public static string AddZoneInfo(string zoneId, string zoneName, string division, string country, string status, string regionId, string regionName, string groupId, string groupName)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT ZONE_ID FROM T_MKTG_ZONE WHERE ZONE_ID='" + zoneId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["ZONE_ID"].ToString() != "")
            {
                string qrS = @"UPDATE T_MKTG_ZONE SET ZONE_NAME='" + zoneName + "',DIVISION_ID='" + division + "',COUNTRY_NAME='" + country + "',STATUS='" + status + "',GROUP_ID='" + groupId + "', GROUP_NAME='" + groupName + "', REGION_ID='" + regionId + "',REGION_NAME='" + regionName + "' WHERE ZONE_ID='" + zoneId + "'";
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

                string query = @"INSERT INTO T_MKTG_ZONE(ZONE_ID,ZONE_NAME,DIVISION_ID,COUNTRY_NAME,STATUS,ENTRY_DATE,ENTRY_BY,REGION_ID,REGION_NAME,GROUP_ID,GROUP_NAME)
                            VALUES ('" + zoneId + "','" + zoneName + "','" + division + "','" + country + "','" + status + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + regionId + "','" + regionName + "','" + groupId + "','" + groupName + "')";
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
    public static string AddNewItemInfo(string itemCode, string itemName, string itemPrice, string groupId, string groupName, string companyName, string activeness)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT ITEM_ID FROM T_MKTG_ITEM WHERE ITEM_ID='" + itemCode + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["ITEM_ID"].ToString() != "")
            {
                string qrS = @"UPDATE T_MKTG_ITEM SET ITEM_NAME='" + itemName + "',GROUP_ID='" + groupId + "',GROUP_NAME='" + groupName + "',COMPANY_NAME='" + companyName + "',PRICE='" + itemPrice + "',STATUS='" + activeness + "' WHERE ITEM_ID='" + itemCode + "'";
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

                string query = @"INSERT INTO T_MKTG_ITEM(ITEM_ID,ITEM_NAME,GROUP_NAME,COMPANY_NAME,PRICE,STATUS,ENTRY_DATE,GROUP_ID)
                                VALUES ('" + itemCode + "','" + itemName + "','" + groupName + "','" + companyName + "','" + itemPrice + "','" + activeness + "',TO_DATE(SYSDATE),'" + groupId + "')";
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


    [WebMethod]
    public static string GetItemInfo(string itemId)
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
                query = @"SELECT * FROM T_MKTG_ITEM";
            }
            else
            {
                query = @"SELECT * FROM T_MKTG_ITEM WHERE ITEM_ID='" + itemId + "'";

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
                    string PRICE = ds.Tables[0].Rows[i]["PRICE"].ToString();
                    string GROUP_ID = ds.Tables[0].Rows[i]["GROUP_ID"].ToString();
                    string GROUP_NAME = ds.Tables[0].Rows[i]["GROUP_NAME"].ToString();
                    string COMPANY_NAME = ds.Tables[0].Rows[i]["COMPANY_NAME"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + ITEM_ID + ";" + ITEM_NAME + ";" + PRICE + ";" + GROUP_NAME + ";" + COMPANY_NAME + ";" + STATUS + ";" + GROUP_ID;
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
    public static string GetGroupWiseItemInfo(string group)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string query = "";
            if (group == "AllGroup")
            {
                query = @"SELECT ITEM_ID,ITEM_NAME FROM T_MKTG_ITEM";
            }
            else
            {
                query = @" SELECT ITEM_ID,ITEM_NAME FROM T_MKTG_ITEM WHERE GROUP_NAME LIKE '%" + group + "%'";

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
    public static string GetZone(string zoneId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = "";
            if (zoneId == "zoneId")
            {
                query = @"SELECT T1.*,T2.DIVISION_NAME FROM
                         (SELECT * FROM T_MKTG_ZONE ORDER BY ZONE_ID) T1,
                         (SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T2
                         WHERE T1.DIVISION_ID=T2.DIVISION_ID";
            }
            else
            {
                query = @"SELECT T1.*,T2.DIVISION_NAME FROM
                         (SELECT * FROM T_MKTG_ZONE WHERE ZONE_ID='" + zoneId + "' ORDER BY ZONE_ID) T1, (SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T2 WHERE T1.DIVISION_ID=T2.DIVISION_ID";
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
                    string comId = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string comName = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                    string divisionId = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string divisionName = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string country = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();
                    string regionId = ds.Tables[0].Rows[i]["REGION_ID"].ToString();
                    string regionName = ds.Tables[0].Rows[i]["REGION_NAME"].ToString();
                    string GROUP_ID = ds.Tables[0].Rows[i]["GROUP_ID"].ToString();
                    string GROUP_NAME = ds.Tables[0].Rows[i]["GROUP_NAME"].ToString();
                    msg = msg + ";" + comId + ";" + comName + ";" + divisionId + ";" + divisionName + ";" + country + ";" + STATUS + ";" + regionId + ";" + regionName + ";" + GROUP_ID + ";" + GROUP_NAME;
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
    public static string GetZoneInfo(string zoneId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = "";
            if (zoneId == "zoneId")
            {
                query = @"SELECT * FROM T_MKTG_ZONE ORDER BY ZONE_ID";
            }
            else
            {
                query = @"SELECT * FROM T_MKTG_ZONE WHERE ZONE_ID='" + zoneId + "' ORDER BY ZONE_ID";
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
                    string zoneIds = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string zoneNames = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();

                    msg = msg + ";" + zoneIds + ";" + zoneNames;
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
    public static string GetAllZone(string regionId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = "";

            query = @"SELECT * FROM T_MKTG_ZONE WHERE REGION_ID='" + regionId + "'";
             
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string zoneIds = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string zoneNames = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();

                    msg = msg + ";" + zoneIds + ";" + zoneNames;
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
    public static string GetAreaMngr(string mngr)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = "";
            if (mngr == "mngr")
            {
                query = @"SELECT * FROM T_MKTG_AREA_MNGR ORDER BY RM_ID";
            }
            else
            {
                query = @"SELECT * FROM T_MKTG_AREA_MNGR WHERE GROUP_ID='" + mngr + "' ORDER BY RM_ID";
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
                    string mngrId = ds.Tables[0].Rows[i]["RM_ID"].ToString();
                    string mngrName = ds.Tables[0].Rows[i]["RM_NAME"].ToString();

                    msg = msg + ";" + mngrId + ";" + mngrName;
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
    public static string GetSupervisor(string mngr)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = "";
            if (mngr == "mngr")
            {
                query = @"SELECT * FROM T_MKTG_SUPERVISOR ORDER BY SPR_ID";
            }
            else
            {
                query = @"SELECT * FROM T_MKTG_SUPERVISOR WHERE SPR_ID='" + mngr + "' ORDER BY SPR_ID";
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
                    string mngrId = ds.Tables[0].Rows[i]["SPR_ID"].ToString();
                    string mngrName = ds.Tables[0].Rows[i]["SPR_NAME"].ToString();

                    msg = msg + ";" + mngrId + ";" + mngrName;
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
    public static string GetZoneInfoByDivision(string divisionId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = "";
            query = @"SELECT * FROM T_MKTG_ZONE WHERE DIVISION_ID='" + divisionId + "' ORDER BY ZONE_ID";
          
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string zoneIds = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string zoneNames = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();

                    msg = msg + ";" + zoneIds + ";" + zoneNames;
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
    public static string GetZoneInfoByRegion(string regionId)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = "";
            query = @"SELECT * FROM T_MKTG_ZONE WHERE REGION_ID='" + regionId + "' ORDER BY REGION_ID";

            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string zoneIds = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string zoneNames = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();

                    msg = msg + ";" + zoneIds + ";" + zoneNames;
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
    public static string GetRouteInfoByZone(string zoneId, string fromDate, string toDate)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = "";
            query = @"SELECT DISTINCT ROUTE_ID,ROUTE_NAME FROM T_MKTG_ROUTE WHERE ZONE_ID='" + zoneId + "' ORDER BY ROUTE_NAME";

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

                    string qr = @"SELECT SR_ID,SR_NAME FROM T_MKTG_SR_INFO WHERE SR_ID IN(SELECT DISTINCT ROUT_SRID FROM OUTLET_INFO
                                            WHERE ENTRY_DATE BETWEEN TO_DATE('" + fromDate.Trim() + "','MM/DD/YYYY') AND TO_DATE('" + toDate.Trim() + "','MM/DD/YYYY') AND ROUTE_ID='" + ROUTE_ID + "') AND STATUS='Y'";

                    OracleCommand cmds = new OracleCommand(qr, conn);
                    OracleDataAdapter das = new OracleDataAdapter(cmds);
                    DataSet dss = new DataSet();
                    das.Fill(dss);
                    int cs = dss.Tables[0].Rows.Count;
                    if (cs > 0)
                    {
                        string SR_NAME = dss.Tables[0].Rows[0]["SR_NAME"].ToString() + " - " + dss.Tables[0].Rows[0]["SR_ID"].ToString();
                        ROUTE_NAME = ROUTE_NAME + " - " + SR_NAME;
                    }

                    msg = msg + ";" + ROUTE_ID + ";" + ROUTE_NAME;
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
    public static string GetSupervisorByDivisionalMngr(string mngr)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = "";
            query = @"SELECT SPR_ID,SPR_NAME FROM T_MKTG_SUPERVISOR WHERE MNGR_ID='" + mngr + "'";
          
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string SPR_ID = ds.Tables[0].Rows[i]["SPR_ID"].ToString();
                    string SPR_NAME = ds.Tables[0].Rows[i]["SPR_NAME"].ToString();

                    msg = msg + ";" + SPR_ID + ";" + SPR_NAME;
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
    public static string AddRMInfo(string rmId, string rmName, string Pwd, string Phone, string emailAddress, string country, string division, string zone, string groupId, string group, string status, string opm, string opmName)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT RM_ID FROM T_MKTG_AREA_MNGR WHERE RM_ID='" + rmId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["RM_ID"].ToString() != "")
            {
                string qrS = @"UPDATE T_MKTG_AREA_MNGR SET RM_NAME='" + rmName + "',RM_PWD='" + Pwd + "',MOBILE_NO='" + Phone + "',EMAIL_ADDRESS='" + emailAddress + "',DIVISION_ID='" + division + "',COUNTRY_NAME='" + country + "',ZONE_ID='" + zone + "',GROUP_ID='" + groupId + "',GROUP_NAME='" + group + "',OPM_ID='" + opm + "',OPM_NAME='" + opmName + "',STATUS='" + status + "' WHERE RM_ID='" + rmId + "'";
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

                string query = @"INSERT INTO T_MKTG_AREA_MNGR(RM_ID,RM_NAME,RM_PWD,MOBILE_NO,EMAIL_ADDRESS,DIVISION_ID,COUNTRY_NAME,ZONE_ID,GROUP_NAME,STATUS,ENTRY_DATE,ENTRY_BY,OPM_ID,OPM_NAME,GROUP_ID)
                            VALUES ('" + rmId + "','" + rmName + "','" + Pwd + "','" + Phone + "','" + emailAddress + "','" + division + "','" + country + "','" + zone + "','" + group + "','" + status + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + opm + "','" + opmName + "','" + groupId + "')";
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
    public static string AddOperationMngrInfo(string rmId, string rmName, string Pwd, string Phone, string emailAddress, string country, string division, string group, string groupId, string status)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT OPM_ID FROM T_MKTG_OPERATION_MNGR WHERE OPM_ID='" + rmId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["OPM_ID"].ToString() != "")
            {
                string qrS = @"UPDATE T_MKTG_OPERATION_MNGR SET OPM_NAME='" + rmName + "',OPM_PWD='" + Pwd + "',MOBILE_NO='" + Phone + "',EMAIL_ADDRESS='" + emailAddress + "',DIVISION_ID='" + division + "',COUNTRY_NAME='" + country + "',GROUP_ID='" + groupId + "',GROUP_NAME='" + group + "',STATUS='" + status + "' WHERE OPM_ID='" + rmId + "'";
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

                string query = @"INSERT INTO T_MKTG_OPERATION_MNGR(OPM_ID,OPM_NAME,OPM_PWD,MOBILE_NO,EMAIL_ADDRESS,DIVISION_ID,COUNTRY_NAME,GROUP_NAME,STATUS,ENTRY_DATE,ENTRY_BY,GROUP_ID)
                            VALUES ('" + rmId + "','" + rmName + "','" + Pwd + "','" + Phone + "','" + emailAddress + "','" + division + "','" + country + "','" + group + "','" + status + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + groupId + "')";
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
    public static string AddSupervisorInfo(string rmId, string rmName, string Pwd, string Phone, string emailAddress, string country, string division, string zone, string groupId, string group, string status, string areaMngr, string regionId, string regionName)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT SPR_ID FROM T_MKTG_SUPERVISOR WHERE SPR_ID='" + rmId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["SPR_ID"].ToString() != "")
            {
                string qrS = @"UPDATE T_MKTG_SUPERVISOR SET SPR_NAME='" + rmName + "',SPR_PWD='" + Pwd + "',MOBILE_NO='" + Phone + "',EMAIL_ADDRESS='" + emailAddress + "',DIVISION_ID='" + division + "',COUNTRY_NAME='" + country + "',ZONE_ID='" + zone + "',MNGR_ID='" + areaMngr + "',GROUP_ID='" + groupId + "',GROUP_NAME='" + group + "',STATUS='" + status + "',REGION_ID='" + regionId + "',REGION_NAME='" + regionName + "' WHERE SPR_ID='" + rmId + "'";
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

                string query = @"INSERT INTO T_MKTG_SUPERVISOR(SPR_ID,SPR_NAME,SPR_PWD,MOBILE_NO,EMAIL_ADDRESS,DIVISION_ID,COUNTRY_NAME,ZONE_ID,MNGR_ID,GROUP_NAME,STATUS,ENTRY_DATE,ENTRY_BY,GROUP_ID,REGION_ID,REGION_NAME)
                            VALUES ('" + rmId + "','" + rmName + "','" + Pwd + "','" + Phone + "','" + emailAddress + "','" + division + "','" + country + "','" + zone + "','" + areaMngr + "','" + group + "','" + status + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + groupId + "','" + regionId + "','" + regionName + "')";
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
    public static string AddSRInfo(string srId, string srName, string pwd, string Phone, string emailAddress, string country, string division, string region, string regionName, string zone, string mngr, string supervisor, string groupId, string groupName, string status)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qr = @"SELECT SR_ID FROM T_MKTG_SR_INFO WHERE SR_ID='" + srId + "'";
            OracleCommand cmdSR = new OracleCommand(qr, conn);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet ds = new DataSet();
            daSR.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0 && ds.Tables[0].Rows[0]["SR_ID"].ToString() != "")
            {

                string queryS = @"UPDATE T_MKTG_SR_INFO SET SR_NAME='" + srName + "',SR_PWD='" + pwd + "',MOBILE_NO='" + Phone + "',EMAIL_ADDRESS='" + emailAddress + "',COUNTRY_NAME='" + country + "',DIVISION_ID='" + division + "',REGION_ID='" + region + "',REGION_NAME='" + regionName + "',ZONE_ID='" + zone + "',MNGR_ID='" + mngr + "',SUPERVISOR_ID='" + supervisor + "', GROUP_ID='" + groupId + "', GROUP_NAME='" + groupName + "',STATUS='" + status + "' WHERE SR_ID='" + srId + "' ";

                OracleCommand cmdS = new OracleCommand(queryS, conn);

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
                string query = @"INSERT INTO T_MKTG_SR_INFO(SR_ID,SR_NAME,SR_PWD,MOBILE_NO,EMAIL_ADDRESS,COUNTRY_NAME,DIVISION_ID,ZONE_ID,MNGR_ID,SUPERVISOR_ID,GROUP_NAME,STATUS,ENTRY_DATE,ENTRY_BY,GROUP_ID,REGION_ID,REGION_NAME)
                            VALUES ('" + srId + "','" + srName + "','" + pwd + "','" + Phone + "','" + emailAddress + "','" + country + "','" + division + "','" + zone + "','" + mngr + "','" + supervisor + "','" + groupName + "','" + status + "',TO_DATE(SYSDATE),'" + HttpContext.Current.Session["userid"].ToString() + "','" + groupId + "','" + region + "','" + regionName + "')";
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
                query = @"SELECT T7.*,T8.SPR_NAME FROM
                        (SELECT T5.*,T6.RM_NAME FROM
                        (SELECT T3.*,T4.ZONE_NAME FROM
                        (SELECT T1.*,T2.DIVISION_NAME FROM
                        (SELECT * FROM T_MKTG_SR_INFO) T1,
                        (SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T2
                        WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                        (SELECT ZONE_ID,ZONE_NAME FROM T_MKTG_ZONE) T4
                        WHERE T3.ZONE_ID=T4.ZONE_ID) T5,
                        (SELECT RM_ID,RM_NAME FROM T_MKTG_AREA_MNGR) T6
                        WHERE T5.MNGR_ID=T6.RM_ID) T7,
                        (SELECT SPR_ID,SPR_NAME FROM T_MKTG_SUPERVISOR) T8
                        WHERE T7.SUPERVISOR_ID=T8.SPR_ID";
            }
            else
            {
                query = @"SELECT T7.*,T8.SPR_NAME FROM
                        (SELECT T5.*,T6.RM_NAME FROM
                        (SELECT T3.*,T4.ZONE_NAME FROM
                        (SELECT T1.*,T2.DIVISION_NAME FROM
                        (SELECT * FROM T_MKTG_SR_INFO WHERE SR_ID='" + srId + "') T1, ";
                query = query + @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T2
                        WHERE T1.DIVISION_ID=T2.DIVISION_ID) T3,
                        (SELECT ZONE_ID,ZONE_NAME FROM T_MKTG_ZONE) T4
                        WHERE T3.ZONE_ID=T4.ZONE_ID) T5,
                        (SELECT RM_ID,RM_NAME FROM T_MKTG_AREA_MNGR) T6
                        WHERE T5.MNGR_ID=T6.RM_ID) T7,
                        (SELECT SPR_ID,SPR_NAME FROM T_MKTG_SUPERVISOR) T8
                        WHERE T7.SUPERVISOR_ID=T8.SPR_ID";
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
                    string SR_ID = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string SR_NAME = ds.Tables[0].Rows[i]["SR_NAME"].ToString();
                    string SR_PWD = ds.Tables[0].Rows[i]["SR_PWD"].ToString();
                    string MOBILE_NO = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string EMAIL_ADDRESS = ds.Tables[0].Rows[i]["EMAIL_ADDRESS"].ToString();
                     
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string ZONE_ID = ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string ZONE_NAME = ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();

                    string MNGR_ID = ds.Tables[0].Rows[i]["MNGR_ID"].ToString();
                    string MNGR_NAME = ds.Tables[0].Rows[i]["RM_NAME"].ToString();

                    string SUPERVISOR_ID = ds.Tables[0].Rows[i]["SUPERVISOR_ID"].ToString();
                    string SUPERVISOR_NAME = ds.Tables[0].Rows[i]["SPR_NAME"].ToString();

                    string GROUP_ID = ds.Tables[0].Rows[i]["GROUP_ID"].ToString();
                    string GROUP_NAME = ds.Tables[0].Rows[i]["GROUP_NAME"].ToString();
                    
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();
                    string REGION_ID = ds.Tables[0].Rows[i]["REGION_ID"].ToString();
                    string REGION_NAME = ds.Tables[0].Rows[i]["REGION_NAME"].ToString();

                    msg = msg + ";" + SR_ID + ";" + SR_NAME + ";" + SR_PWD + ";" + MOBILE_NO + ";" + EMAIL_ADDRESS + ";" + COUNTRY_NAME + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + ZONE_ID + ";" + ZONE_NAME + ";" + MNGR_ID + ";" + MNGR_NAME + ";" + SUPERVISOR_ID + ";" + SUPERVISOR_NAME + ";" + GROUP_NAME + ";" + STATUS + ";" + GROUP_ID + ";" + REGION_ID + ";" + REGION_NAME;
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
                query = @" SELECT T1.*,T2.DIVISION_NAME FROM
                         (SELECT * FROM T_MKTG_AREA_MNGR) T1,
                         (SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T2
                         WHERE T1.DIVISION_ID=T2.DIVISION_ID ";
            }
            else
            {
                query = @"SELECT T1.*,T2.DIVISION_NAME FROM
                         (SELECT * FROM T_MKTG_AREA_MNGR WHERE RM_ID='" + rm + "') T1, ";
                query = query + @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T2
                         WHERE T1.DIVISION_ID=T2.DIVISION_ID ";
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
                    string MOTHER_COMPANY = ""; // ds.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    string OWN_COMPANY = "";// ds.Tables[0].Rows[i]["ZONE_NAME"].ToString();
                    string GROUP_ID = ds.Tables[0].Rows[i]["GROUP_ID"].ToString();  
                    string ITEM_GROUP = ds.Tables[0].Rows[i]["GROUP_NAME"].ToString();                     
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();
                    string OPM_ID = ds.Tables[0].Rows[i]["OPM_ID"].ToString();
                    string OPM_NAME = ds.Tables[0].Rows[i]["OPM_NAME"].ToString();

                    msg = msg + ";" + RM_ID + ";" + RM_NAME + ";" + RM_PWD + ";" + MOBILE_NO + ";" + EMAIL_ADDRESS + ";" + COUNTRY_NAME + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + MOTHER_COMPANY + ";" + OWN_COMPANY + ";" + ITEM_GROUP + ";" + STATUS + ";" + OPM_ID + ";" + OPM_NAME + ";" + GROUP_ID;
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
    public static string GetOperationMngrInfo(string rm)
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
                query = @"SELECT T1.*,T2.DIVISION_NAME FROM
                         (SELECT * FROM T_MKTG_OPERATION_MNGR) T1,                          
                         (SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T2
                         WHERE T1.DIVISION_ID=T2.DIVISION_ID";
            }
            else
            {
                query = @"SELECT T1.*,T2.DIVISION_NAME FROM
                         (SELECT * FROM T_MKTG_OPERATION_MNGR WHERE OPM_ID='" + rm + "') T1, (SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T2 WHERE T1.DIVISION_ID=T2.DIVISION_ID";
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
                    string RM_ID = ds.Tables[0].Rows[i]["OPM_ID"].ToString();
                    string RM_NAME = ds.Tables[0].Rows[i]["OPM_NAME"].ToString();
                    string RM_PWD = ds.Tables[0].Rows[i]["OPM_PWD"].ToString();
                    string MOBILE_NO = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string EMAIL_ADDRESS = ds.Tables[0].Rows[i]["EMAIL_ADDRESS"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();                     
                    string GROUP_ID = ds.Tables[0].Rows[i]["GROUP_ID"].ToString();
                    string ITEM_GROUP = ds.Tables[0].Rows[i]["GROUP_NAME"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + RM_ID + ";" + RM_NAME + ";" + RM_PWD + ";" + MOBILE_NO + ";" + EMAIL_ADDRESS + ";" + COUNTRY_NAME + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + ITEM_GROUP + ";" + STATUS + ";" + GROUP_ID;
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
    public static string GetSupervisorInfo(string rm)
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
                query = @"SELECT T3.*,T4.DIVISION_NAME FROM                         
                         (SELECT T1.* FROM
                         (SELECT * FROM T_MKTG_SUPERVISOR) T1,
                         (SELECT REGION_ID FROM T_MKTG_REGION) T2
                         WHERE T1.REGION_ID=T2.REGION_ID) T3,
                         (SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T4
                         WHERE T3.DIVISION_ID=T4.DIVISION_ID ";
            }
            else
            {
                query = @"SELECT T3.*,T4.DIVISION_NAME FROM                         
                        (SELECT T1.* FROM
                         (SELECT * FROM T_MKTG_SUPERVISOR WHERE SPR_ID='" + rm + "') T1, ";
                query = query + @"(SELECT REGION_ID FROM T_MKTG_REGION) T2
                         WHERE T1.REGION_ID=T2.REGION_ID) T3,
                         (SELECT DIVISION_ID,DIVISION_NAME FROM T_MKTG_DIVISION) T4
                         WHERE T3.DIVISION_ID=T4.DIVISION_ID ";
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
                    string RM_ID = ds.Tables[0].Rows[i]["SPR_ID"].ToString();
                    string RM_NAME = ds.Tables[0].Rows[i]["SPR_NAME"].ToString();
                    string RM_PWD = ds.Tables[0].Rows[i]["SPR_PWD"].ToString();
                    string MOBILE_NO = ds.Tables[0].Rows[i]["MOBILE_NO"].ToString();
                    string EMAIL_ADDRESS = ds.Tables[0].Rows[i]["EMAIL_ADDRESS"].ToString();
                    string COUNTRY_NAME = ds.Tables[0].Rows[i]["COUNTRY_NAME"].ToString();
                    string DIVISION_ID = ds.Tables[0].Rows[i]["DIVISION_ID"].ToString();
                    string DIVISION_NAME = ds.Tables[0].Rows[i]["DIVISION_NAME"].ToString();
                    string REGION_ID = ds.Tables[0].Rows[i]["REGION_ID"].ToString();
                    string REGION_NAME = ds.Tables[0].Rows[i]["REGION_NAME"].ToString();
                    string GROUP_ID = ds.Tables[0].Rows[i]["GROUP_ID"].ToString();
                    string ITEM_GROUP = ds.Tables[0].Rows[i]["GROUP_NAME"].ToString();
                    string MNGR_ID = ds.Tables[0].Rows[i]["MNGR_ID"].ToString();
                    string STATUS = ds.Tables[0].Rows[i]["STATUS"].ToString();

                    msg = msg + ";" + RM_ID + ";" + RM_NAME + ";" + RM_PWD + ";" + MOBILE_NO + ";" + EMAIL_ADDRESS + ";" + COUNTRY_NAME + ";" + DIVISION_ID + ";" + DIVISION_NAME + ";" + REGION_ID + ";" + REGION_NAME + ";" + ITEM_GROUP + ";" + STATUS + ";" + MNGR_ID + ";" + GROUP_ID;
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


    static string DisplayPercentage(double ratio)
    {
        string percentage = string.Format("{0:0.0%}", ratio);
        return percentage;
    }

}