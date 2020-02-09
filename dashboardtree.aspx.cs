using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NestedRptGroupWiseSRDetails : System.Web.UI.Page
{
    
    public string companyID = "";
    public string orderdate = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        companyID = HttpContext.Current.Session["company"].ToString();
        orderdate = Request.QueryString["currentdate"].ToString();

        if (!IsPostBack)
        {
            BindFirstGridData();
        }
    }

 

    private static DataTable GetData(string query)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["OracleDBMain"].ConnectionString;

        OracleConnection conn = new OracleConnection(connectionString); // C#

        conn.Open();
        OracleCommand cmdG = new OracleCommand(query, conn);
        OracleDataAdapter da = new OracleDataAdapter(cmdG);

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        da.Fill(dt);

        conn.Close();
        return dt;


    }

    private void BindFirstGridData()
    {
       

        
        string queryActiveSR = @"SELECT COUNT(SR_ID) SRCOUNT_ACTIVE,ITEM_GROUP_ID,ITEM_GROUP_NAME FROM 
                                (SELECT DISTINCT T1.SR_ID,T2.ITEM_GROUP_ID,T2.ITEM_GROUP_NAME FROM T_SR_INFO T1 
                                INNER JOIN T_ITEM_GROUP T2 ON T1.ITEM_GROUP_ID=T2.ITEM_GROUP_ID 
                                WHERE T1.STATUS='Y'AND T2.COMPANY_ID='" + companyID.Trim() + "') "+
                               " GROUP BY ITEM_GROUP_ID,ITEM_GROUP_NAME";

        string queryInActiveSR = @"SELECT COUNT(SR_ID) SRCOUNT_INACTIVE,ITEM_GROUP_ID FROM 
                                (SELECT DISTINCT T1.SR_ID,T2.ITEM_GROUP_ID,T2.ITEM_GROUP_NAME FROM T_SR_INFO T1 
                                INNER JOIN T_ITEM_GROUP T2 ON T1.ITEM_GROUP_ID=T2.ITEM_GROUP_ID 
                                WHERE T1.STATUS ='N'AND T2.COMPANY_ID='" + companyID.Trim() + "') "+
                                " GROUP BY ITEM_GROUP_ID";

        string queryActiveSR_Today = @"SELECT COUNT(DISTINCT SR_ID) SRCOUNT_TODAY,ITEM_GROUP_ID,ITEM_GROUP_NAME,ROUND(SUM(SUBTOTAL)/1000,4) TOTAL, 
                    ROUND((SUM(SUBTOTAL)/1000)/COUNT(DISTINCT SR_ID), 4) SR_AVARAGE_ORDER FROM 
                    (SELECT T1.SR_ID,T3.ITEM_GROUP_ID,T3.ITEM_GROUP_NAME,((T1.ITEM_CTN*T4.FACTOR)+T1.ITEM_QTY)*T1.OUT_PRICE SUBTOTAL 
                    FROM T_ORDER_DETAIL T1 
                    INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID 
                    INNER JOIN T_ITEM_GROUP T3 ON T2.ITEM_GROUP_ID=T3.ITEM_GROUP_ID
                    INNER JOIN T_ITEM T4 ON T1.ITEM_ID=T4.ITEM_ID
                    WHERE T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') AND T3.COMPANY_ID='" + companyID.Trim() + "' AND T1.STATUS='Y') " +
                    " GROUP BY ITEM_GROUP_ID,ITEM_GROUP_NAME";

        DataTable ActiveSR = GetData(queryActiveSR);
        DataTable InActiveSR = GetData(queryInActiveSR);
        DataTable ActiveSRToday = GetData(queryActiveSR_Today);


        var myLINQ = from t1 in ActiveSR.AsEnumerable()
                     join t2 in ActiveSRToday.AsEnumerable() on t1.Field<string>("ITEM_GROUP_ID") equals t2.Field<string>("ITEM_GROUP_ID")
                     select new
                     {
                         item_GROUP_ID = t1.Field<string>("ITEM_GROUP_ID"),
                         item_GROUP_NAME = t1.Field<string>("ITEM_GROUP_NAME"),
                         srCOUNT_ACTIVE = t1.Field<decimal>("SRCOUNT_ACTIVE"),
                         srCOUNT_TODAY = t2.Field<decimal>("SRCOUNT_TODAY"),
                         total = t2.Field<decimal>("TOTAL"),
                         srAvgOrder = t2.Field<decimal>("SR_AVARAGE_ORDER"),
                         srCOUNT_ONLEAVE = t1.Field<decimal>("SRCOUNT_ACTIVE") - t2.Field<decimal>("SRCOUNT_TODAY")

                     };

        DataTable dt_NEW = new DataTable();
        dt_NEW.Columns.Add("ITEM_GROUP_ID");
        dt_NEW.Columns.Add("ITEM_GROUP_NAME");
        dt_NEW.Columns.Add("SRCOUNT_ACTIVE");
        dt_NEW.Columns.Add("SRCOUNT_TODAY");
        dt_NEW.Columns.Add("SRCOUNT_ONLEAVE");
        dt_NEW.Columns.Add("TOTAL");
        dt_NEW.Columns.Add("SR_AVARAGE_ORDER");
        foreach (var item in myLINQ)
        {
            dt_NEW.Rows.Add(item.item_GROUP_ID, item.item_GROUP_NAME, item.srCOUNT_ACTIVE, item.srCOUNT_TODAY, item.srCOUNT_ONLEAVE, item.total, item.srAvgOrder);
        }




        firstGrid.DataSource = dt_NEW;
        firstGrid.DataBind();
    }

    private void BindSecondGridData(string item_GROUP_ID, GridView gvSR)
    {
        //gvSR.ToolTip = item_GROUP_ID;

        string queryActiveSR = @"SELECT COUNT(SR_ID) SRCOUNT_ACTIVE,ITEM_GROUP_ID,ITEM_GROUP_NAME FROM 
                                (SELECT DISTINCT T1.SR_ID,T2.ITEM_GROUP_ID,T2.ITEM_GROUP_NAME FROM T_SR_INFO T1 
                                INNER JOIN T_ITEM_GROUP T2 ON T1.ITEM_GROUP_ID=T2.ITEM_GROUP_ID 
                                WHERE T1.STATUS='Y'AND T2.COMPANY_ID='" + companyID.Trim() + "') "+
                               " GROUP BY ITEM_GROUP_ID,ITEM_GROUP_NAME";

        string queryActiveSR_Today = @"SELECT COUNT(DISTINCT SR_ID) SRCOUNT_TODAY,ITEM_GROUP_ID,ITEM_GROUP_NAME,ROUND(SUM(SUBTOTAL)/1000,4) TOTAL, 
        ROUND((SUM(SUBTOTAL)/1000)/COUNT(DISTINCT SR_ID), 4) SR_AVARAGE_ORDER,DIVISION_NAME,DIVISION_ID FROM 
        (SELECT T1.SR_ID,T3.ITEM_GROUP_ID,T3.ITEM_GROUP_NAME,((T1.ITEM_CTN*T4.FACTOR)+T1.ITEM_QTY)*T1.OUT_PRICE SUBTOTAL, 
        T6.DIVISION_NAME,T5.DIVISION_ID 
        FROM T_ORDER_DETAIL T1 
        INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID 
        INNER JOIN T_ITEM_GROUP T3 ON T2.ITEM_GROUP_ID=T3.ITEM_GROUP_ID
        INNER JOIN T_ITEM T4 ON T1.ITEM_ID=T4.ITEM_ID
        INNER JOIN T_ROUTE T5 ON T1.ROUTE_ID=T5.ROUTE_ID
        INNER JOIN T_DIVISION T6 ON T5.DIVISION_ID=T6.DIVISION_ID
        WHERE T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') AND T3.COMPANY_ID='" + companyID.Trim() + "' AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T1.STATUS='Y') " +
        " GROUP BY ITEM_GROUP_ID,ITEM_GROUP_NAME,DIVISION_NAME,DIVISION_ID";

        DataTable ActiveSR = GetData(queryActiveSR);
        DataTable ActiveSRToday = GetData(queryActiveSR_Today);

        var myLINQ = from t1 in ActiveSR.AsEnumerable()
                     join t2 in ActiveSRToday.AsEnumerable() on t1.Field<string>("ITEM_GROUP_ID") equals t2.Field<string>("ITEM_GROUP_ID")
                     select new
                     {
                         item_GROUP_ID = t1.Field<string>("ITEM_GROUP_ID"),
                         item_GROUP_NAME = t1.Field<string>("ITEM_GROUP_NAME"),
                         srCOUNT_ACTIVE = t1.Field<decimal>("SRCOUNT_ACTIVE"),
                         srCOUNT_TODAY = t2.Field<decimal>("SRCOUNT_TODAY"),
                         total = t2.Field<decimal>("TOTAL"),
                         srAvgOrder = t2.Field<decimal>("SR_AVARAGE_ORDER"),
                         dIVISION_NAME = t2.Field<string>("DIVISION_NAME"),
                         dIVISION_ID = t2.Field<string>("DIVISION_ID"),
                         srCOUNT_ONLEAVE = t1.Field<decimal>("SRCOUNT_ACTIVE") - t2.Field<decimal>("SRCOUNT_TODAY")

                     };

        DataTable dt_NEW = new DataTable();
        dt_NEW.Columns.Add("ITEM_GROUP_ID");
        dt_NEW.Columns.Add("ITEM_GROUP_NAME");
        dt_NEW.Columns.Add("SRCOUNT_ACTIVE");
        dt_NEW.Columns.Add("SRCOUNT_TODAY");
        dt_NEW.Columns.Add("SRCOUNT_ONLEAVE");
        dt_NEW.Columns.Add("TOTAL");
        dt_NEW.Columns.Add("SR_AVARAGE_ORDER");
        dt_NEW.Columns.Add("DIVISION_NAME");
        dt_NEW.Columns.Add("DIVISION_ID");

        foreach (var item in myLINQ)
        {
            dt_NEW.Rows.Add(item.item_GROUP_ID, item.item_GROUP_NAME, item.srCOUNT_ACTIVE, item.srCOUNT_TODAY,
                item.srCOUNT_ONLEAVE, item.total, item.srAvgOrder, item.dIVISION_NAME, item.dIVISION_ID);
        }

        gvSR.DataSource = dt_NEW;
        gvSR.DataBind();
    }

    private void BindThirdGridData(string item_GROUP_ID, string divisionID, GridView thirdGrid)
    {
        companyID = "P1001";
        orderdate = "18/12/2016";
        
        string queryActiveSR = @"SELECT COUNT (DISTINCT T1.SR_ID) SRCOUNT_ACTIVE,T1.ITEM_GROUP_ID,T2.ZONE_ID FROM T_SR_INFO T1
                                INNER JOIN T_ZONE T2 ON T1.DIST_ZONE=T2.ZONE_ID 
                                WHERE T1.STATUS='Y' AND DIVISION_NAME='" +divisionID+"' AND T1.ITEM_GROUP_ID='"+item_GROUP_ID+"' " +
                                " GROUP BY T2.ZONE_ID,T1.ITEM_GROUP_ID";

        string queryActiveSR_Today = @"SELECT COUNT(DISTINCT SR_ID) SRCOUNT_TODAY,ROUND(SUM(SUBTOTAL)/1000,4) TOTAL, 
                        ROUND((SUM(SUBTOTAL)/1000)/COUNT(DISTINCT SR_ID), 4) SR_AVARAGE_ORDER,ZONE_ID,ZONE_NAME,DIVISION_ID,ITEM_GROUP_ID FROM 
                        (SELECT T1.SR_ID,((T1.ITEM_CTN*T4.FACTOR)+T1.ITEM_QTY)*T1.OUT_PRICE SUBTOTAL,T5.ZONE_ID,T6.ZONE_NAME,T5.DIVISION_ID,T2.ITEM_GROUP_ID
                        FROM T_ORDER_DETAIL T1 
                        INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID 
                        INNER JOIN T_ITEM_GROUP T3 ON T2.ITEM_GROUP_ID=T3.ITEM_GROUP_ID
                        INNER JOIN T_ITEM T4 ON T1.ITEM_ID=T4.ITEM_ID
                        INNER JOIN T_ROUTE T5 ON T1.ROUTE_ID=T5.ROUTE_ID
                        INNER JOIN T_ZONE T6 ON T5.ZONE_ID=T6.ZONE_ID
                        WHERE T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') AND T3.COMPANY_ID='" + companyID.Trim() + "' " +
                        " AND T2.ITEM_GROUP_ID='" + item_GROUP_ID+"' AND T1.STATUS='Y'  AND T5.DIVISION_ID='"+divisionID+"') "+
                        " GROUP BY ZONE_ID,ZONE_NAME,DIVISION_ID,ITEM_GROUP_ID";

        DataTable ActiveSR = GetData(queryActiveSR);
        DataTable ActiveSRToday = GetData(queryActiveSR_Today);

        var myLINQ = from t1 in ActiveSR.AsEnumerable()
                     join t2 in ActiveSRToday.AsEnumerable() on t1.Field<string>("ZONE_ID") equals t2.Field<string>("ZONE_ID")
                     select new
                     {
                         srCOUNT_ACTIVE = t1.Field<decimal>("SRCOUNT_ACTIVE"),
                         item_GROUP_ID = t1.Field<string>("ITEM_GROUP_ID"),
                         srCOUNT_TODAY = t2.Field<decimal>("SRCOUNT_TODAY"),
                         zone_ID = t2.Field<string>("ZONE_ID"),
                         zone_NAME = t2.Field<string>("ZONE_NAME"),
                         total = t2.Field<decimal>("TOTAL"),
                         srAvgOrder = t2.Field<decimal>("SR_AVARAGE_ORDER"),
                         dIVISION_ID = t2.Field<string>("DIVISION_ID"),
                         srCOUNT_ONLEAVE = t1.Field<decimal>("SRCOUNT_ACTIVE") - t2.Field<decimal>("SRCOUNT_TODAY")

                     };

        DataTable dt_NEW = new DataTable();
        dt_NEW.Columns.Add("ZONE_ID");
        dt_NEW.Columns.Add("ZONE_NAME");
        dt_NEW.Columns.Add("SRCOUNT_ACTIVE");
        dt_NEW.Columns.Add("SRCOUNT_TODAY");
        dt_NEW.Columns.Add("SRCOUNT_ONLEAVE");
        dt_NEW.Columns.Add("TOTAL");
        dt_NEW.Columns.Add("SR_AVARAGE_ORDER");
        dt_NEW.Columns.Add("DIVISION_ID");
        dt_NEW.Columns.Add("ITEM_GROUP_ID");

        foreach (var item in myLINQ)
        {
            dt_NEW.Rows.Add(item.zone_ID, item.zone_NAME, item.srCOUNT_ACTIVE, item.srCOUNT_TODAY,
                item.srCOUNT_ONLEAVE, item.total, item.srAvgOrder, item.dIVISION_ID,item.item_GROUP_ID);
        }

        thirdGrid.DataSource = dt_NEW;
        thirdGrid.DataBind();

    }


    private void BindSR(string zoneID, GridView fourthGrid)
    {
        fourthGrid.ToolTip = zoneID;
        string query2 = @"SELECT SR_ID,SR_NAME FROM T_SR_INFO WHERE DIST_ZONE='" + zoneID + "'";
        fourthGrid.DataSource = GetData(query2);
        fourthGrid.DataBind();
    }

    private void BindProduct(string srID, GridView fifthGrid)
    {
        companyID = "P1001";
        orderdate = "18/12/2016";
        
        string query2 = @"SELECT T1.ITEM_ID,T2.ITEM_NAME, (((T1.CARTON*T2.FACTOR)+T1.PIECE)*T1.OUT_PRICE) TOTAL_AMT FROM
                        (SELECT DISTINCT ITEM_ID,OUT_PRICE, SUM(ITEM_CTN) CARTON, SUM(ITEM_QTY) PIECE FROM T_ORDER_DETAIL
                        WHERE SR_ID='" + srID + "' AND ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') GROUP BY ITEM_ID,OUT_PRICE) T1, " +
                       "(SELECT ITEM_ID,ITEM_NAME, FACTOR FROM T_ITEM) T2 " +
                        "WHERE T1.ITEM_ID=T2.ITEM_ID";
        fifthGrid.DataSource = GetData(query2);
        fifthGrid.DataBind();
    }

    protected void Show_Hide_FirstLevel_Grid(object sender, EventArgs e)
    {
        ImageButton imgShowHide = (sender as ImageButton);
        GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
        if (imgShowHide.CommandArgument == "Show")
        {
            row.FindControl("pnlFirst").Visible = true;
            imgShowHide.CommandArgument = "Hide";
            imgShowHide.ImageUrl = "~/images/minus.png";
            string dmID = firstGrid.DataKeys[row.RowIndex].Value.ToString();
            GridView secondGrid = row.FindControl("secondGrid") as GridView;
            BindSecondGridData(dmID, secondGrid);
        }
        else
        {
            row.FindControl("pnlFirst").Visible = false;
            imgShowHide.CommandArgument = "Show";
            imgShowHide.ImageUrl = "~/images/plus.png";
        }
    }

    protected void Show_Hide_SecondLevel_Grid(object sender, EventArgs e)
    {
        ImageButton imgShowHide = (sender as ImageButton);
        GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
        if (imgShowHide.CommandArgument == "Show")
        {
            row.FindControl("pnlSecond").Visible = true;
            imgShowHide.CommandArgument = "Hide";
            imgShowHide.ImageUrl = "~/images/minus.png";

            string item_GROUP_ID = Convert.ToString((row.NamingContainer as GridView).DataKeys[row.RowIndex].Values[0]);
            string divisionID = Convert.ToString((row.NamingContainer as GridView).DataKeys[row.RowIndex].Values[1]);


            GridView secondGrid = row.FindControl("thirdGrid") as GridView;
            BindThirdGridData(item_GROUP_ID, divisionID, secondGrid);
        }
        else
        {
            row.FindControl("pnlSecond").Visible = false;
            imgShowHide.CommandArgument = "Show";
            imgShowHide.ImageUrl = "~/images/plus.png";
        }
    }

    protected void Show_Hide_ThirdLevel_Grid(object sender, EventArgs e)
    {
        ImageButton imgShowHide = (sender as ImageButton);
        GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
        if (imgShowHide.CommandArgument == "Show")
        {
            row.FindControl("pnlThird").Visible = true;
            imgShowHide.CommandArgument = "Hide";
            imgShowHide.ImageUrl = "~/images/minus.png";
            string zoneID = Convert.ToString((row.NamingContainer as GridView).DataKeys[row.RowIndex].Values[2]);

            GridView fourthGrid = row.FindControl("fourthGrid") as GridView;
            BindSR(zoneID, fourthGrid);
        }
        else
        {
            row.FindControl("pnlThird").Visible = false;
            imgShowHide.CommandArgument = "Show";
            imgShowHide.ImageUrl = "~/images/plus.png";
        }
    }

    protected void Show_Hide_FourthLevel_Grid(object sender, EventArgs e)
    {
        ImageButton imgShowHide = (sender as ImageButton);
        GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
        if (imgShowHide.CommandArgument == "Show")
        {
            row.FindControl("pnlFourth").Visible = true;
            imgShowHide.CommandArgument = "Hide";
            imgShowHide.ImageUrl = "~/images/minus.png";
            string srID = Convert.ToString((row.NamingContainer as GridView).DataKeys[row.RowIndex].Values[0]);

            GridView fifthGrid = row.FindControl("fifthGrid") as GridView;
            BindProduct(srID, fifthGrid);
        }
        else
        {
            row.FindControl("pnlFourth").Visible = false;
            imgShowHide.CommandArgument = "Show";
            imgShowHide.ImageUrl = "~/images/plus.png";
        }
    }

    protected void OnSecondLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView gvOrders = (sender as GridView);
        gvOrders.PageIndex = e.NewPageIndex;
        BindSecondGridData(gvOrders.ToolTip, gvOrders);
    }

    protected void OnThirdLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView gvDV = (sender as GridView);
        gvDV.PageIndex = e.NewPageIndex;
        //BindThirdGridData(gvDV.ToolTip, gvDV);
    }

    protected void OnFourthLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView fourthGrid = (sender as GridView);
        fourthGrid.PageIndex = e.NewPageIndex;
        BindSR(fourthGrid.ToolTip, fourthGrid);
    }

    protected void OnFifthLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView fifthGrid = (sender as GridView);
        fifthGrid.PageIndex = e.NewPageIndex;
        BindProduct(fifthGrid.ToolTip, fifthGrid);
    }

}