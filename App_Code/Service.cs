using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

/// <summary>
/// Summary description for Service
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class Service : System.Web.Services.WebService
{

    public Service()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetInfo(string SR_ID, string Date_Time)
    {
        //SR_ID = "152015";
        //Date_Time = "5/2/2017";


        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());
            con.Open();

            //            string srC = @"SELECT DISTINCT T1.TRAN_ID title,T1.LATITUDE lat,T1.LONGITUDE lng, T2.OUTLET_NAME description FROM T_ORDER_DETAIL T1 
            //                INNER JOIN T_OUTLET T2 ON T1.OUTLET_ID=T2.OUTLET_ID
            //                WHERE T1.LATITUDE !=0 AND T1.ENTRY_DATE=TO_DATE('5/2/2017','DD/MM/YYYY') 
            //                AND T1.SR_ID='152015' and rownum <= 20";

            string srC = @"SELECT title,lat,lng, ('Name: ' || SR_NAME || '(' || SR_ID || ')<br/>Mobile No: ' || MOBILE_NO || '<br/>Group: ' || ITEM_GROUP || '<br/>Outlet: ' || OUTLET_NAME||'<br/>Order Time: '||UploadTime || '<br/>Route: ' || ROUTE_NAME) description FROM 
                        (SELECT DISTINCT T1.TRAN_ID title,T1.LATITUDE lat,
                        T1.LONGITUDE lng, T2.OUTLET_NAME,to_char( T1.ENTRY_DATETIME , 'HH24:MI:SS' ) UploadTime, 
                        T3.SR_NAME,T3.MOBILE_NO,T1.SR_ID,T4.ROUTE_NAME,T3.ITEM_GROUP  FROM T_ORDER_DETAIL T1 
                        INNER JOIN T_OUTLET T2 ON T1.OUTLET_ID=T2.OUTLET_ID
                        INNER JOIN T_SR_INFO T3 ON T3.SR_ID=T1.SR_ID
                        INNER JOIN T_ROUTE T4 ON T1.ROUTE_ID=T4.ROUTE_ID
                        WHERE T1.LATITUDE !=0 AND T1.ENTRY_DATE=TO_DATE('" + Date_Time + "','DD/MM/YYYY') AND T1.SR_ID='" + SR_ID + "' ORDER BY UploadTime )";

            OracleCommand cC = new OracleCommand(srC, con);
            OracleDataAdapter daC = new OracleDataAdapter(cC);
            DataSet dsC = new DataSet();
            daC.Fill(dsC);
            int cS = dsC.Tables[0].Rows.Count;
            con.Close();

            int c = dsC.Tables[0].Rows.Count;

            return JsonConvert.SerializeObject(dsC.Tables[0]);

        }
        catch (Exception ex)
        {
            msg = ex.ToString();
        }

        return msg;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetInfoFirstTime(string Date_Time)
    {
        //SR_ID = "152015";
        //Date_Time = "5/2/2017";


        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());
            con.Open();

//            string srC = @"SELECT title,lat,lng, (OUTLET_NAME||','||UploadTime) description FROM 
//                (SELECT DISTINCT T1.TRAN_ID title,T1.LATITUDE lat,
//                T1.LONGITUDE lng, T2.OUTLET_NAME,to_char( T1.ENTRY_DATETIME , 'HH24:MI:SS' ) UploadTime  FROM T_ORDER_DETAIL T1 
//                INNER JOIN T_OUTLET T2 ON T1.OUTLET_ID=T2.OUTLET_ID
//                WHERE T1.LATITUDE !=0 AND T1.ENTRY_DATE=TO_DATE('" + Date_Time + "','DD/MM/YYYY') ORDER BY UploadTime )";

            string qrC = @"SELECT title,lat,lng, ('Name: ' || SR_NAME || '(' || SR_ID || ')<br/>Mobile No: ' || MOBILE_NO || '<br/>Group: ' || ITEM_GROUP || '<br/>Outlet: ' || OUTLET_NAME||'<br/>Order Time: '||UploadTime || '<br/>Route: ' || ROUTE_NAME) description FROM 
                        (SELECT DISTINCT T1.TRAN_ID title,T1.LATITUDE lat,
                        T1.LONGITUDE lng, T2.OUTLET_NAME,to_char( T1.ENTRY_DATETIME , 'HH24:MI:SS' ) UploadTime, 
                        T3.SR_NAME,T3.MOBILE_NO,T1.SR_ID,T4.ROUTE_NAME,T3.ITEM_GROUP  FROM T_ORDER_DETAIL T1 
                        INNER JOIN T_OUTLET T2 ON T1.OUTLET_ID=T2.OUTLET_ID
                        INNER JOIN T_SR_INFO T3 ON T3.SR_ID=T1.SR_ID
                        INNER JOIN T_ROUTE T4 ON T1.ROUTE_ID=T4.ROUTE_ID
                        WHERE T1.LATITUDE !=0 AND T1.ENTRY_DATE=TO_DATE('" + Date_Time + "','DD/MM/YYYY') ORDER BY UploadTime)";

            OracleCommand cC = new OracleCommand(qrC, con);
            OracleDataAdapter daC = new OracleDataAdapter(cC);
            DataSet dsC = new DataSet();
            daC.Fill(dsC);
            int cS = dsC.Tables[0].Rows.Count;
            con.Close();

            int c = dsC.Tables[0].Rows.Count;

            return JsonConvert.SerializeObject(dsC.Tables[0]);

        }
        catch (Exception ex)
        {
            msg = ex.ToString();
        }

        return msg;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetDivision(string countryName)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

//            string query = @"SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION
//                            WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString() + "%'";

            string query = @"SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION
                            WHERE COUNTRY_NAME LIKE '%" + countryName + "%' ORDER BY DIVISION_NAME ASC";
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
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetCompanyGroup(string ownCompany)
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
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetCompany()
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT COMPANY_ID,COMPANY_FULL_NAME FROM T_COMPANY WHERE STATUS='Y' ORDER BY COMPANY_FULL_NAME ASC";
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
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetSR(string zone, string group)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT SR_ID,SR_NAME FROM T_SR_INFO  WHERE ITEM_GROUP_ID='" + group.Trim() + "' AND DIST_ZONE='" + zone.Trim() + "' ORDER BY SR_NAME ASC";
            OracleCommand cmd = new OracleCommand(query, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    string srID = ds.Tables[0].Rows[i]["SR_ID"].ToString();
                    string srName = ds.Tables[0].Rows[i]["SR_NAME"].ToString();
                    msg = msg + ";" + srID + ";" + srName;
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
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetZone(string division)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string query = @"SELECT ZONE_ID,ZONE_NAME FROM T_ZONE
                            WHERE DIVISION_ID='" + division + "' ORDER BY ZONE_NAME";
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




}
