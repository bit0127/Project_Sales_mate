using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class srtracking : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            HttpContext.Current.Session["currentdate"] = Request.QueryString["currentdate"].ToString();
            HttpContext.Current.Session["division"] = Request.QueryString["division"].ToString();
            HttpContext.Current.Session["zone"] = Request.QueryString["zone"].ToString();
                         
            string query = @"SELECT T3.*,('Outlet Name: ' || T4.OUTLET_NAME || '</br>Address: ' || T4.OUTLET_ADDRESS || '</br>Contact Number: ' || T4.MOBILE_NUMBER) OUTLET_INFO FROM
                        (SELECT T1.SR_ID,T1.LATITUDE,T1.LONGITUDE,T1.OUTLET_ID,(T2.SR_NAME || '</br>Mobile Number: ' || T2.MOBILE_NO || '</br>' || '</br>Group: ' || T2.ITEM_GROUP || '</br>') DESCRIPTIONS, T1.ENTRY_DATETIME currentTime FROM
                        (SELECT DISTINCT SR_ID,LATITUDE,LONGITUDE,OUTLET_ID,ENTRY_DATETIME FROM T_ORDER_DETAIL WHERE SR_ID IN( ";
            query = query + @"SELECT SR_ID FROM T_SR_INFO WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString() + "%' AND DIVISION_NAME='" + HttpContext.Current.Session["division"].ToString() + "' AND DIST_ZONE='" + HttpContext.Current.Session["zone"].ToString() + "') AND ENTRY_DATE=TO_DATE('" + HttpContext.Current.Session["currentdate"].ToString() + "','DD/MM/YYYY') AND LATITUDE<>0  ORDER BY ENTRY_DATE DESC) T1, ";
            query = query + @"(SELECT SR_ID,SR_NAME,MOBILE_NO,ITEM_GROUP FROM T_SR_INFO WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString() + "%' AND DIVISION_NAME='" + HttpContext.Current.Session["division"].ToString() + "' AND DIST_ZONE='" + HttpContext.Current.Session["zone"].ToString() + "') T2 WHERE T1.SR_ID=T2.SR_ID) T3, ";
            query = query + @"(SELECT OUTLET_ID,OUTLET_NAME,OUTLET_ADDRESS,MOBILE_NUMBER FROM T_OUTLET WHERE COUNTRY_NAME LIKE '%" + HttpContext.Current.Session["country"].ToString() + "%' AND DIVISION_ID='" + HttpContext.Current.Session["division"].ToString() + "' AND ZONE_ID='" + HttpContext.Current.Session["zone"].ToString() + "') T4 WHERE T3.OUTLET_ID=T3.OUTLET_ID";


            DataTable dt = this.GetData(query);
            rptMarkers.DataSource = dt;
            rptMarkers.DataBind();
        }
    }

    private DataTable GetData(string query)
    {
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

         
            OracleCommand cmd = new OracleCommand(query);
            using (OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString()))
            {
                using (OracleDataAdapter sda = new OracleDataAdapter())
                {
                    cmd.Connection = con;

                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
         
    }
}