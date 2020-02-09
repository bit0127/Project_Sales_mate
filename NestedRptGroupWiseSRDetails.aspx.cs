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
    
    string companyID = "";
    string orderdate = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        companyID = HttpContext.Current.Session["company"].ToString().Trim();
        orderdate = Request.QueryString["currentdate"].ToString();

        if (!IsPostBack)
        {
            try {
                BindFirstGridData();
            }
            catch (Exception ex)
            {
                Response.Redirect("login.aspx", false);
            }
        }
    }

    private static DataTable GetData(string query)
    {
        
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
        OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
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

        string queryOnLeaveSR = @"SELECT T1.ITEM_GROUP_ID,COUNT(DISTINCT T2.SR_ID) SRCOUNT_ONLEAVE FROM T_SR_INFO T1
                            LEFT JOIN T_LEAVE T2 ON T1.SR_ID=T2.SR_ID AND T1.ITEM_COMPANY='" + companyID.Trim() + "' " +
                            " AND T2.FROM_DATE=TO_DATE('" + orderdate + "','DD/MM/YYYY') Group By T1.ITEM_GROUP_ID";

        string queryActiveSR_Today = @"SELECT COUNT(DISTINCT SR_ID) SRCOUNT_TODAY,ITEM_GROUP_ID,ITEM_GROUP_NAME,ROUND(SUM(SUBTOTAL)/1000,4) TOTAL, 
                    ROUND((SUM(SUBTOTAL)/1000)/COUNT(DISTINCT SR_ID), 4) SR_AVARAGE_ORDER FROM 
                    (SELECT T1.SR_ID,T3.ITEM_GROUP_ID,T3.ITEM_GROUP_NAME,((T1.ITEM_CTN*T4.FACTOR)+T1.ITEM_QTY)*T1.OUT_PRICE SUBTOTAL 
                    FROM T_ORDER_DETAIL T1 
                    INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID 
                    INNER JOIN T_ITEM_GROUP T3 ON T2.ITEM_GROUP_ID=T3.ITEM_GROUP_ID
                    INNER JOIN T_ITEM T4 ON T1.ITEM_ID=T4.ITEM_ID
                    WHERE T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') AND T3.COMPANY_ID='" + companyID.Trim() + "' AND T1.STATUS='Y') " +
                    " GROUP BY ITEM_GROUP_ID,ITEM_GROUP_NAME";


        string qrVstTotOutlet = @"SELECT TT1.TOTAL_OUTLET,TT2.VISITED_OUTLET, TT1.ITEM_GROUP_ID FROM
                                (SELECT COUNT( DISTINCT T1.OUTLET_ID) TOTAL_OUTLET, T2.ITEM_GROUP_ID FROM T_OUTLET T1 
                                INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' AND T1.STATUS='Y' " +
                                "GROUP BY T2.ITEM_GROUP_ID) TT1, " +
                                @"(SELECT COUNT(OUTLET_ID) VISITED_OUTLET, ITEM_GROUP_ID FROM 
                                (SELECT OUTLET_ID, ITEM_GROUP_ID FROM
                                (SELECT T1.OUTLET_ID, T2.ITEM_GROUP_ID FROM T_ORDER_DETAIL T1 
                                INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' " +
                                @"AND T1.ENTRY_DATE=TO_DATE('" + orderdate + "','DD/MM/YYYY')) " +
                                @"UNION
                                (SELECT T1.OUTLET_ID, T2.ITEM_GROUP_ID FROM T_NON_PRODUCTIVE_SALES T1 
                                INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' " +
                                "AND T1.ENTRY_DATE=TO_DATE('" + orderdate + "','DD/MM/YYYY'))) " +
                                @"GROUP BY ITEM_GROUP_ID) TT2
                                WHERE TT1.ITEM_GROUP_ID=TT2.ITEM_GROUP_ID";

        string qeTotalMemo = @"SELECT COUNT(DISTINCT OUTLET_ID) TOTAL_MEMO,ITEM_GROUP_ID FROM
                                (SELECT T1.OUTLET_ID,T2.ITEM_GROUP_ID FROM T_ORDER_DETAIL T1 
                                INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' " +
                                @"AND T1.ENTRY_DATE=TO_DATE('" + orderdate + "','DD/MM/YYYY')) " +
                                "GROUP BY ITEM_GROUP_ID";


        string queryLPC = @"SELECT ROUND(TT1.TOTAL_LINE/TT2.PRODUCTIVE_VISITED_OUTLET,2) LPC,TT2.ITEM_GROUP_ID FROM
                            (SELECT COUNT(TRAN_ID) TOTAL_LINE,T2.ITEM_GROUP_ID FROM T_ORDER_DETAIL T1 
                            INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' "+
                            " AND T1.ENTRY_DATE=TO_DATE('" + orderdate + "','DD/MM/YYYY') "+
                            @"GROUP BY T2.ITEM_GROUP_ID)TT1,
                            (SELECT COUNT(DISTINCT OUTLET_ID) PRODUCTIVE_VISITED_OUTLET,T2.ITEM_GROUP_ID FROM T_ORDER_DETAIL T1 
                            INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' "+ 
                            " AND T1.ENTRY_DATE=TO_DATE('"+orderdate+"','DD/MM/YYYY') " +
                            " GROUP BY T2.ITEM_GROUP_ID)TT2 WHERE TT1.ITEM_GROUP_ID=TT2.ITEM_GROUP_ID";

        string queryStrikeRate = @" SELECT ROUND((TOTAL_MEMO/VISITED_OUTLET)*100,2)|| '%' STRIKE_RATE, ITEM_GROUP_ID FROM 
                                 (SELECT TT2.TOTAL_MEMO,TT2.ITEM_GROUP_ID,TT1.VISITED_OUTLET FROM
                                (SELECT COUNT(OUTLET_ID) VISITED_OUTLET, ITEM_GROUP_ID FROM 
                                (SELECT OUTLET_ID, ITEM_GROUP_ID FROM
                                (SELECT T1.OUTLET_ID, T2.ITEM_GROUP_ID FROM T_ORDER_DETAIL T1 
                                INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' " +
                                " AND T1.ENTRY_DATE=TO_DATE('" + orderdate + "','DD/MM/YYYY')) " +
                                " UNION (SELECT T1.OUTLET_ID, T2.ITEM_GROUP_ID FROM T_NON_PRODUCTIVE_SALES T1 " +
                                " INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' " +
                                @"AND T1.ENTRY_DATE=TO_DATE('" + orderdate + "','DD/MM/YYYY'))) GROUP BY ITEM_GROUP_ID) TT1," +
                                "(SELECT COUNT(DISTINCT OUTLET_ID) TOTAL_MEMO,ITEM_GROUP_ID FROM " +
                                "(SELECT T1.OUTLET_ID,T2.ITEM_GROUP_ID FROM T_ORDER_DETAIL T1 " +
                                "INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' ";
        queryStrikeRate = queryStrikeRate + @" AND T1.ENTRY_DATE=TO_DATE('" + orderdate + "','DD/MM/YYYY') AND T1.STATUS='Y' )" +
                                @"GROUP BY ITEM_GROUP_ID) TT2
                                WHERE TT1.ITEM_GROUP_ID=TT2.ITEM_GROUP_ID)";


        DataTable ActiveSR = GetData(queryActiveSR);
        DataTable OnLeave = GetData(queryOnLeaveSR);
        DataTable ActiveSRToday = GetData(queryActiveSR_Today);
        DataTable TotalAndVisitedOutlet = GetData(qrVstTotOutlet);
        DataTable TotalMemo = GetData(qeTotalMemo);
        DataTable LPC= GetData(queryLPC);
        DataTable STRIKERATE = GetData(queryStrikeRate);

        var myLINQ = from t1 in ActiveSR.AsEnumerable()
                     join t2 in ActiveSRToday.AsEnumerable() on t1.Field<string>("ITEM_GROUP_ID") equals t2.Field<string>("ITEM_GROUP_ID")
                     join t3 in OnLeave.AsEnumerable() on t1.Field<string>("ITEM_GROUP_ID") equals t3.Field<string>("ITEM_GROUP_ID")
                     join t4 in TotalAndVisitedOutlet.AsEnumerable() on t1.Field<string>("ITEM_GROUP_ID") equals t4.Field<string>("ITEM_GROUP_ID")
                     join t5 in TotalMemo.AsEnumerable() on t1.Field<string>("ITEM_GROUP_ID") equals t5.Field<string>("ITEM_GROUP_ID")
                     join t6 in LPC.AsEnumerable() on t1.Field<string>("ITEM_GROUP_ID") equals t6.Field<string>("ITEM_GROUP_ID")
                     join t7 in STRIKERATE.AsEnumerable() on t1.Field<string>("ITEM_GROUP_ID") equals t7.Field<string>("ITEM_GROUP_ID")

                     select new
                     {
                         item_GROUP_ID = t1.Field<string>("ITEM_GROUP_ID"),
                         item_GROUP_NAME = t1.Field<string>("ITEM_GROUP_NAME"),
                         srCOUNT_ACTIVE = t1.Field<decimal>("SRCOUNT_ACTIVE"),
                         srCOUNT_TODAY = t2.Field<decimal>("SRCOUNT_TODAY"),
                         total = t2.Field<decimal>("TOTAL"),
                         srAvgOrder = t2.Field<decimal>("SR_AVARAGE_ORDER"),
                         srCOUNT_ONLEAVE = t3.Field<decimal>("SRCOUNT_ONLEAVE"),
                         totalOutlet = t4.Field<decimal>("TOTAL_OUTLET"),
                         visitedOutlet = t4.Field<decimal>("VISITED_OUTLET"),
                         totalMemo = t5.Field<decimal>("TOTAL_MEMO"),
                         lpc = t6.Field<decimal>("LPC"),
                         strikrate = t7.Field<string>("STRIKE_RATE"),                        
                         srCOUNT_InActive = t1.Field<decimal>("SRCOUNT_ACTIVE") - t2.Field<decimal>("SRCOUNT_TODAY")

                     };

    
        DataTable dt_NEW = new DataTable();
        dt_NEW.Columns.Add("ITEM_GROUP_ID");
        dt_NEW.Columns.Add("ITEM_GROUP_NAME");
        dt_NEW.Columns.Add("SRCOUNT_ACTIVE");
        dt_NEW.Columns.Add("SRCOUNT_TODAY");
        dt_NEW.Columns.Add("SRCOUNT_INACTIVE");
        dt_NEW.Columns.Add("SRCOUNT_ONLEAVE");
        dt_NEW.Columns.Add("TOTAL_OUTLET");
        dt_NEW.Columns.Add("VISITED_OUTLET");
        dt_NEW.Columns.Add("TOTAL_MEMO");
        dt_NEW.Columns.Add("TOTAL");
        dt_NEW.Columns.Add("SR_AVARAGE_ORDER");
        dt_NEW.Columns.Add("LPC");
        dt_NEW.Columns.Add("STRIKE_RATE");
        dt_NEW.Columns.Add("LEAVE_FROM_JOB");
        foreach (var item in myLINQ)
        {
            if (item.srCOUNT_InActive > 0)
            {
                dt_NEW.Rows.Add(item.item_GROUP_ID, item.item_GROUP_NAME, item.srCOUNT_ACTIVE, item.srCOUNT_TODAY, item.srCOUNT_InActive,
                item.srCOUNT_ONLEAVE, item.totalOutlet, item.visitedOutlet, item.totalMemo, item.total, item.srAvgOrder, item.lpc, item.strikrate,0);
            }
            else
            {
                dt_NEW.Rows.Add(item.item_GROUP_ID, item.item_GROUP_NAME, item.srCOUNT_ACTIVE, item.srCOUNT_TODAY, 0,
               item.srCOUNT_ONLEAVE, item.totalOutlet, item.visitedOutlet, item.totalMemo, item.total, item.srAvgOrder, item.lpc, item.strikrate, item.srCOUNT_InActive * -1);
            }
 
        }




        firstGrid.DataSource = dt_NEW;
        firstGrid.DataBind();
    }

    private void BindSecondGridData(string item_GROUP_ID, GridView gvSecond)
    {
        gvSecond.ToolTip = item_GROUP_ID;

        string queryActiveSR = @"SELECT COUNT(SR_ID) SRCOUNT_ACTIVE,ITEM_GROUP_ID,DIVISION_ID FROM 
                        (SELECT DISTINCT T1.SR_ID,T1.DIVISION_NAME DIVISION_ID,T2.ITEM_GROUP_ID FROM T_SR_INFO T1 
                        INNER JOIN T_ITEM_GROUP T2 ON T1.ITEM_GROUP_ID=T2.ITEM_GROUP_ID 
                        WHERE T1.STATUS='Y'AND T2.COMPANY_ID='" + companyID + "' AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "') GROUP BY ITEM_GROUP_ID,DIVISION_ID";

        string queryOnLeaveSR = @"SELECT * FROM (SELECT T1.ITEM_GROUP_ID,COUNT(DISTINCT T2.SR_ID) SRCOUNT_ONLEAVE,T1.DIVISION_NAME DIVISION_ID FROM T_SR_INFO T1
                        LEFT JOIN T_LEAVE T2 ON T1.SR_ID=T2.SR_ID AND T1.ITEM_COMPANY='" + companyID + "' AND T1.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T2.FROM_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') Group By T1.ITEM_GROUP_ID,T1.DIVISION_NAME) WHERE ITEM_GROUP_ID='" + item_GROUP_ID + "'";
                      

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
                        "GROUP BY ITEM_GROUP_ID,ITEM_GROUP_NAME,DIVISION_NAME,DIVISION_ID";

        string qrVstTotOutlet = @"SELECT TT1.TOTAL_OUTLET,TT2.VISITED_OUTLET, TT1.ITEM_GROUP_ID,TT1.DIVISION_ID FROM
                        (SELECT COUNT( DISTINCT T1.OUTLET_ID) TOTAL_OUTLET, T2.ITEM_GROUP_ID,T2.DIVISION_NAME DIVISION_ID FROM T_OUTLET T1 
                        INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' AND T1.STATUS='Y' AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' " +
                        @"GROUP BY T2.ITEM_GROUP_ID,T2.DIVISION_NAME) TT1, 
                        (SELECT COUNT(OUTLET_ID) VISITED_OUTLET, ITEM_GROUP_ID FROM 
                        (SELECT OUTLET_ID, ITEM_GROUP_ID FROM
                        (SELECT T1.OUTLET_ID, T2.ITEM_GROUP_ID FROM T_ORDER_DETAIL T1 
                        INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' " +
                        @"AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY')) UNION " +
                        " (SELECT T1.OUTLET_ID, T2.ITEM_GROUP_ID FROM T_NON_PRODUCTIVE_SALES T1 " +
                        " INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "'" +
                        @" AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY'))) GROUP BY ITEM_GROUP_ID) TT2 WHERE TT1.ITEM_GROUP_ID=TT2.ITEM_GROUP_ID";

        string qeTotalMemo = @"SELECT COUNT(DISTINCT OUTLET_ID) TOTAL_MEMO,ITEM_GROUP_ID, DIVISION_ID FROM
                                (SELECT T1.OUTLET_ID,T2.ITEM_GROUP_ID,T2.DIVISION_NAME DIVISION_ID FROM T_ORDER_DETAIL T1 
                                INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "'" +
                                @"AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY')) " +
                                "GROUP BY ITEM_GROUP_ID,DIVISION_ID";


        string queryLPC = @"SELECT ROUND(TT1.TOTAL_LINE/TT2.PRODUCTIVE_VISITED_OUTLET,2) LPC,TT2.ITEM_GROUP_ID,TT2.DIVISION_ID FROM
                (SELECT COUNT(TRAN_ID) TOTAL_LINE,T2.ITEM_GROUP_ID,T2.DIVISION_NAME DIVISION_ID FROM T_ORDER_DETAIL T1 
                INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' " +
            "AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') GROUP BY T2.DIVISION_NAME,T2.ITEM_GROUP_ID)TT1, " +
            "(SELECT COUNT(DISTINCT OUTLET_ID) PRODUCTIVE_VISITED_OUTLET,T2.ITEM_GROUP_ID,T2.DIVISION_NAME DIVISION_ID FROM T_ORDER_DETAIL T1 " +
            "INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' " +
            " AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY')  GROUP BY T2.ITEM_GROUP_ID,T2.DIVISION_NAME)TT2 " +
            " WHERE TT1.DIVISION_ID=TT2.DIVISION_ID";

        string queryStrikeRate = @"SELECT ROUND((TOTAL_MEMO/VISITED_OUTLET)*100,2)|| '%' STRIKE_RATE, ITEM_GROUP_ID,DIVISION_ID FROM 
                                    (SELECT TT2.TOTAL_MEMO,TT2.ITEM_GROUP_ID,TT1.VISITED_OUTLET,TT2.DIVISION_ID FROM
                                    (SELECT COUNT(OUTLET_ID) VISITED_OUTLET, ITEM_GROUP_ID FROM 
                                    (SELECT OUTLET_ID, ITEM_GROUP_ID FROM
                                    (SELECT T1.OUTLET_ID, T2.ITEM_GROUP_ID FROM T_ORDER_DETAIL T1 
                                    INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T2.ITEM_COMPANY='" + companyID + "' " +
                                @"AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY'))  UNION (SELECT T1.OUTLET_ID, T2.ITEM_GROUP_ID " +
                                @"FROM T_NON_PRODUCTIVE_SALES T1  INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' " +
                                @"AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY'))) GROUP BY ITEM_GROUP_ID) TT1, " +
                                @"(SELECT COUNT(DISTINCT OUTLET_ID) TOTAL_MEMO,ITEM_GROUP_ID,DIVISION_ID
                                    FROM (SELECT T1.OUTLET_ID,T2.ITEM_GROUP_ID,T2.DIVISION_NAME DIVISION_ID FROM T_ORDER_DETAIL T1 
                                    INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' " +
                                @"AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') AND T1.STATUS='Y' )GROUP BY ITEM_GROUP_ID,DIVISION_ID) TT2 WHERE TT1.ITEM_GROUP_ID=TT2.ITEM_GROUP_ID)";


        DataTable ActiveSR = GetData(queryActiveSR);
        DataTable OnLeave = GetData(queryOnLeaveSR);
        DataTable ActiveSRToday = GetData(queryActiveSR_Today);
        DataTable TotalAndVisitedOutlet = GetData(qrVstTotOutlet);
        DataTable TotalMemo = GetData(qeTotalMemo);
        DataTable LPC = GetData(queryLPC);
        DataTable STRIKERATE = GetData(queryStrikeRate);

        var myLINQ = from t1 in ActiveSR.AsEnumerable()
                     join t2 in ActiveSRToday.AsEnumerable() on t1.Field<string>("DIVISION_ID") equals t2.Field<string>("DIVISION_ID")
                     join t3 in OnLeave.AsEnumerable() on t1.Field<string>("DIVISION_ID") equals t3.Field<string>("DIVISION_ID")
                     join t4 in TotalAndVisitedOutlet.AsEnumerable() on t1.Field<string>("DIVISION_ID") equals t4.Field<string>("DIVISION_ID")
                     join t5 in TotalMemo.AsEnumerable() on t1.Field<string>("DIVISION_ID") equals t5.Field<string>("DIVISION_ID")
                     join t6 in LPC.AsEnumerable() on t1.Field<string>("DIVISION_ID") equals t6.Field<string>("DIVISION_ID")
                     join t7 in STRIKERATE.AsEnumerable() on t1.Field<string>("DIVISION_ID") equals t7.Field<string>("DIVISION_ID")
                     select new
                     {
                         item_GROUP_ID = t1.Field<string>("ITEM_GROUP_ID"),
                         srCOUNT_ACTIVE = t1.Field<decimal>("SRCOUNT_ACTIVE"),
                         srCOUNT_ONLEAVE = t3.Field<decimal>("SRCOUNT_ONLEAVE"),
                         srCOUNT_TODAY = t2.Field<decimal>("SRCOUNT_TODAY"),
                         totalOutlet = t4.Field<decimal>("TOTAL_OUTLET"),
                         visitedOutlet = t4.Field<decimal>("VISITED_OUTLET"),
                         total = t2.Field<decimal>("TOTAL"),
                         totalMemo = t5.Field<decimal>("TOTAL_MEMO"),
                         lpc = t6.Field<decimal>("LPC"),
                         strikrate = t7.Field<string>("STRIKE_RATE"),
                         srAvgOrder = t2.Field<decimal>("SR_AVARAGE_ORDER"),
                         dIVISION_NAME = t2.Field<string>("DIVISION_NAME"),
                         dIVISION_ID = t2.Field<string>("DIVISION_ID"),
                         srCOUNT_InActive = t1.Field<decimal>("SRCOUNT_ACTIVE") - t2.Field<decimal>("SRCOUNT_TODAY")

                     };

        DataTable dt_NEW = new DataTable();
        dt_NEW.Columns.Add("ITEM_GROUP_ID");
        dt_NEW.Columns.Add("SRCOUNT_ACTIVE");
        dt_NEW.Columns.Add("SRCOUNT_TODAY");
        dt_NEW.Columns.Add("SRCOUNT_INACTIVE");
        dt_NEW.Columns.Add("SRCOUNT_ONLEAVE");
        dt_NEW.Columns.Add("TOTAL_OUTLET");
        dt_NEW.Columns.Add("VISITED_OUTLET");
        dt_NEW.Columns.Add("TOTAL_MEMO");
        dt_NEW.Columns.Add("TOTAL");
        dt_NEW.Columns.Add("SR_AVARAGE_ORDER");
        dt_NEW.Columns.Add("DIVISION_NAME");
        dt_NEW.Columns.Add("DIVISION_ID");
        dt_NEW.Columns.Add("LPC");
        dt_NEW.Columns.Add("STRIKE_RATE");
        dt_NEW.Columns.Add("LEAVE_FROM_JOB");

        foreach (var item in myLINQ)
        {
            if (item.srCOUNT_InActive > 0)
            {

                dt_NEW.Rows.Add(item.item_GROUP_ID, item.srCOUNT_ACTIVE, item.srCOUNT_TODAY, item.srCOUNT_InActive, item.srCOUNT_ONLEAVE, item.totalOutlet, 
                   item.visitedOutlet, item.totalMemo, item.total, item.srAvgOrder, item.dIVISION_NAME, item.dIVISION_ID, item.lpc, item.strikrate,0);
            }

            else
            {

                dt_NEW.Rows.Add(item.item_GROUP_ID, item.srCOUNT_ACTIVE, item.srCOUNT_TODAY, 0, item.srCOUNT_ONLEAVE, item.totalOutlet,
                   item.visitedOutlet, item.totalMemo, item.total, item.srAvgOrder, item.dIVISION_NAME, item.dIVISION_ID, item.lpc, item.strikrate, item.srCOUNT_InActive*-1);
            }
 
        }

        gvSecond.DataSource = dt_NEW;
        gvSecond.DataBind();
    }

    private void BindThirdGridData(string item_GROUP_ID, string divisionID, GridView thirdGrid)
    {
        thirdGrid.ToolTip = divisionID;

        string queryActiveSR = @"SELECT COUNT (DISTINCT T1.SR_ID) SRCOUNT_ACTIVE,T1.ITEM_GROUP_ID,T2.ZONE_ID FROM T_SR_INFO T1
                                INNER JOIN T_ZONE T2 ON T1.DIST_ZONE=T2.ZONE_ID 
                                WHERE T1.STATUS='Y' AND DIVISION_NAME='" + divisionID + "' AND T1.ITEM_GROUP_ID='" + item_GROUP_ID + "' " +
                                " GROUP BY T2.ZONE_ID,T1.ITEM_GROUP_ID";

        string queryOnLeaveSR = @"SELECT * FROM (SELECT T1.ITEM_GROUP_ID,COUNT(DISTINCT T2.SR_ID) SRCOUNT_ONLEAVE,T1.DIVISION_NAME DIVISION_ID,T3.ZONE_ID,T3.ZONE_NAME FROM T_SR_INFO T1
                    INNER JOIN T_ZONE T3 ON T1.DIST_ZONE=T3.ZONE_ID 
                    LEFT JOIN T_LEAVE T2 ON T1.SR_ID=T2.SR_ID AND T1.ITEM_COMPANY='" + companyID + "' AND T1.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T1.DIVISION_NAME='" + divisionID + "' AND T2.FROM_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') " +
                    @"GROUP BY T1.ITEM_GROUP_ID,T1.DIVISION_NAME,T3.ZONE_ID,T3.ZONE_NAME) WHERE DIVISION_ID ='" + divisionID + "' AND ITEM_GROUP_ID='" + item_GROUP_ID + "'";


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
                        " AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T1.STATUS='Y'  AND T5.DIVISION_ID='" + divisionID + "') " +
                        " GROUP BY ZONE_ID,ZONE_NAME,DIVISION_ID,ITEM_GROUP_ID";

        string qrVstTotOutlet = @"SELECT TT1.TOTAL_OUTLET,TT2.VISITED_OUTLET, TT1.ITEM_GROUP_ID,TT1.DIVISION_ID,TT1.DIST_ZONE ZONE_ID FROM
                        (SELECT COUNT( DISTINCT T1.OUTLET_ID) TOTAL_OUTLET, T2.ITEM_GROUP_ID,T2.DIVISION_NAME DIVISION_ID,T2.DIST_ZONE FROM T_OUTLET T1 
                        INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' AND T1.STATUS='Y'" +
                        " AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T2.DIVISION_NAME='" + divisionID + "' " +
                        @" GROUP BY T2.DIST_ZONE,T2.DIVISION_NAME,T2.ITEM_GROUP_ID) TT1, 
                        (SELECT COUNT(OUTLET_ID) VISITED_OUTLET, ITEM_GROUP_ID,DIST_ZONE FROM 
                        (SELECT OUTLET_ID, ITEM_GROUP_ID,DIST_ZONE FROM
                        (SELECT T1.OUTLET_ID, T2.ITEM_GROUP_ID,T2.DIST_ZONE FROM T_ORDER_DETAIL T1 
                        INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "'" +
                        " AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T2.DIVISION_NAME='" + divisionID + "' AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY')) UNION " +
                        @" (SELECT T1.OUTLET_ID, T2.ITEM_GROUP_ID,T2.DIST_ZONE FROM T_NON_PRODUCTIVE_SALES T1  
                        INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID 
                        AND T2.ITEM_COMPANY='" + companyID + "' AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T2.DIVISION_NAME='" + divisionID + "' " +
                        " AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY'))) " +
                        " GROUP BY DIST_ZONE,ITEM_GROUP_ID) TT2 WHERE TT1.DIST_ZONE=TT2.DIST_ZONE";

        string qeTotalMemo = @"SELECT COUNT(DISTINCT OUTLET_ID) TOTAL_MEMO,ITEM_GROUP_ID, DIVISION_ID,DIST_ZONE ZONE_ID FROM
                            (SELECT T1.OUTLET_ID,T2.ITEM_GROUP_ID,T2.DIVISION_NAME DIVISION_ID,T2.DIST_ZONE FROM T_ORDER_DETAIL T1 
                            INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' " +
                           " AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T2.DIVISION_NAME='" + divisionID + "' " +
                           " AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY')) GROUP BY DIST_ZONE,DIVISION_ID,ITEM_GROUP_ID";


        string queryLPC = @"SELECT ROUND(TT1.TOTAL_LINE/TT2.PRODUCTIVE_VISITED_OUTLET,2) LPC,TT2.ITEM_GROUP_ID,TT2.DIVISION_ID,TT1.DIST_ZONE ZONE_ID  
                        FROM (SELECT COUNT(TRAN_ID) TOTAL_LINE,T2.ITEM_GROUP_ID,T2.DIVISION_NAME DIVISION_ID,T2.DIST_ZONE FROM 
                        T_ORDER_DETAIL T1 
                        INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' " +
                        "AND T2.DIVISION_NAME='" + divisionID + "' AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') " +
                        " GROUP BY T2.DIST_ZONE,T2.DIVISION_NAME,T2.ITEM_GROUP_ID)TT1, " +
                        "(SELECT COUNT(DISTINCT OUTLET_ID) PRODUCTIVE_VISITED_OUTLET,T2.ITEM_GROUP_ID,T2.DIVISION_NAME DIVISION_ID, " +
                       " T2.DIST_ZONE FROM T_ORDER_DETAIL T1 INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' " +
                       " AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T2.DIVISION_NAME='" + divisionID + "' AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') " +
                       " GROUP BY T2.DIST_ZONE,T2.ITEM_GROUP_ID,T2.DIVISION_NAME)TT2 " +
                       " WHERE TT1.DIST_ZONE=TT2.DIST_ZONE";

        string queryStrikeRate = @"SELECT ROUND((TOTAL_MEMO/VISITED_OUTLET)*100,2)|| '%' STRIKE_RATE, ITEM_GROUP_ID,DIVISION_ID,DIST_ZONE ZONE_ID FROM 
                            (SELECT TT2.TOTAL_MEMO,TT2.ITEM_GROUP_ID,TT1.VISITED_OUTLET,TT2.DIVISION_ID,TT1.DIST_ZONE FROM
                            (SELECT COUNT(OUTLET_ID) VISITED_OUTLET, ITEM_GROUP_ID,DIST_ZONE FROM 
                            (SELECT OUTLET_ID, ITEM_GROUP_ID,DIST_ZONE FROM
                            (SELECT T1.OUTLET_ID, T2.ITEM_GROUP_ID,T2.DIST_ZONE FROM T_ORDER_DETAIL T1 
                            INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' " +
                            @"AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T2.DIVISION_NAME='" + divisionID + "' AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY')) " +
                            @"UNION 
                            (SELECT T1.OUTLET_ID, T2.ITEM_GROUP_ID,T2.DIST_ZONE FROM T_NON_PRODUCTIVE_SALES T1  
                            INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID 
                            AND T2.ITEM_COMPANY='" + companyID + "' AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T2.DIVISION_NAME='" + divisionID + "' " +
                            @"AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY'))) " +
                            @"GROUP BY DIST_ZONE,ITEM_GROUP_ID) TT1,
                            (SELECT COUNT(DISTINCT OUTLET_ID) TOTAL_MEMO,ITEM_GROUP_ID,DIVISION_ID,DIST_ZONE
                            FROM (SELECT T1.OUTLET_ID,T2.ITEM_GROUP_ID,T2.DIVISION_NAME DIVISION_ID,T2.DIST_ZONE FROM T_ORDER_DETAIL T1 
                            INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' AND T2.DIVISION_NAME='" + divisionID + "' AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' " +
                            @"AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') AND T1.STATUS='Y' ) GROUP BY DIST_ZONE,ITEM_GROUP_ID,DIVISION_ID) TT2 WHERE TT1.DIST_ZONE=TT2.DIST_ZONE)";


        DataTable ActiveSR = GetData(queryActiveSR);
        DataTable OnLeave = GetData(queryOnLeaveSR);
        DataTable ActiveSRToday = GetData(queryActiveSR_Today);
        DataTable TotalAndVisitedOutlet = GetData(qrVstTotOutlet);
        DataTable TotalMemo = GetData(qeTotalMemo);
        DataTable LPC = GetData(queryLPC);
        DataTable STRIKERATE = GetData(queryStrikeRate);


        var myLINQ = from t1 in ActiveSR.AsEnumerable()
                     join t2 in ActiveSRToday.AsEnumerable() on t1.Field<string>("ZONE_ID") equals t2.Field<string>("ZONE_ID")
                     join t3 in OnLeave.AsEnumerable() on t1.Field<string>("ZONE_ID") equals t3.Field<string>("ZONE_ID")
                     join t4 in TotalAndVisitedOutlet.AsEnumerable() on t1.Field<string>("ZONE_ID") equals t4.Field<string>("ZONE_ID")
                     join t5 in TotalMemo.AsEnumerable() on t1.Field<string>("ZONE_ID") equals t5.Field<string>("ZONE_ID")
                     join t6 in LPC.AsEnumerable() on t1.Field<string>("ZONE_ID") equals t6.Field<string>("ZONE_ID")
                     join t7 in STRIKERATE.AsEnumerable() on t1.Field<string>("ZONE_ID") equals t7.Field<string>("ZONE_ID")
                     select new
                     {
                         srCOUNT_ACTIVE = t1.Field<decimal>("SRCOUNT_ACTIVE"),
                         item_GROUP_ID = t1.Field<string>("ITEM_GROUP_ID"),
                         srCOUNT_ONLEAVE = t3.Field<decimal>("SRCOUNT_ONLEAVE"),
                         srCOUNT_TODAY = t2.Field<decimal>("SRCOUNT_TODAY"),
                         totalOutlet = t4.Field<decimal>("TOTAL_OUTLET"),
                         visitedOutlet = t4.Field<decimal>("VISITED_OUTLET"),
                         totalMemo = t5.Field<decimal>("TOTAL_MEMO"),
                         lpc = t6.Field<decimal>("LPC"),
                         strikrate = t7.Field<string>("STRIKE_RATE"),
                         zone_ID = t2.Field<string>("ZONE_ID"),
                         zone_NAME = t2.Field<string>("ZONE_NAME"),
                         total = t2.Field<decimal>("TOTAL"),
                         srAvgOrder = t2.Field<decimal>("SR_AVARAGE_ORDER"),
                         dIVISION_ID = t2.Field<string>("DIVISION_ID"),
                         srCOUNT_InActive = t1.Field<decimal>("SRCOUNT_ACTIVE") - t2.Field<decimal>("SRCOUNT_TODAY")

                     };

        DataTable dt_NEW = new DataTable();
        dt_NEW.Columns.Add("ZONE_ID");
        dt_NEW.Columns.Add("ZONE_NAME");
        dt_NEW.Columns.Add("SRCOUNT_ACTIVE");
        dt_NEW.Columns.Add("SRCOUNT_TODAY");
        dt_NEW.Columns.Add("SRCOUNT_INACTIVE");
        dt_NEW.Columns.Add("SRCOUNT_ONLEAVE");
        dt_NEW.Columns.Add("TOTAL_OUTLET");
        dt_NEW.Columns.Add("VISITED_OUTLET");
        dt_NEW.Columns.Add("TOTAL_MEMO");
        dt_NEW.Columns.Add("LPC");
        dt_NEW.Columns.Add("STRIKE_RATE");
        dt_NEW.Columns.Add("TOTAL");
        dt_NEW.Columns.Add("SR_AVARAGE_ORDER");
        dt_NEW.Columns.Add("DIVISION_ID");
        dt_NEW.Columns.Add("ITEM_GROUP_ID");
        dt_NEW.Columns.Add("LEAVE_FROM_JOB");
        foreach (var item in myLINQ)
        {
            if (item.srCOUNT_InActive > 0)
            {
                dt_NEW.Rows.Add(item.zone_ID, item.zone_NAME, item.srCOUNT_ACTIVE, item.srCOUNT_TODAY, item.srCOUNT_InActive, item.srCOUNT_ONLEAVE, 
                item.totalOutlet, item.visitedOutlet, item.totalMemo, item.lpc, item.strikrate, item.total, item.srAvgOrder, item.dIVISION_ID, item.item_GROUP_ID,0);
     
            }
            else
            {
                dt_NEW.Rows.Add(item.zone_ID, item.zone_NAME, item.srCOUNT_ACTIVE, item.srCOUNT_TODAY, 0, item.srCOUNT_ONLEAVE,
                item.totalOutlet, item.visitedOutlet, item.totalMemo, item.lpc, item.strikrate, item.total, item.srAvgOrder, item.dIVISION_ID, item.item_GROUP_ID, item.srCOUNT_InActive * -1);
            }

    
        }

        thirdGrid.DataSource = dt_NEW;
        thirdGrid.DataBind();

    }

    private void BindSR(string item_GROUP_ID, string divisionID, string zoneID, GridView fourthGrid)
    {
        fourthGrid.ToolTip = zoneID;

            string sr_Info = @"SELECT DISTINCT T1.SR_ID,T2.SR_NAME,T1.ROUTE_ID,T3.ROUTE_NAME,T2.DIST_ZONE ZONE_ID,T2.DIVISION_NAME DIVISION_ID,
                            T2.ITEM_GROUP_ID FROM T_ORDER_DETAIL T1 
                            INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID
                            INNER JOIN T_ROUTE T3 ON T1.ROUTE_ID=T3.ROUTE_ID AND T2.ITEM_COMPANY='" + companyID + "' " +
                            " AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T2.DIST_ZONE='" + zoneID + "' AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') " +
                            @" UNION 
                            SELECT DISTINCT T1.SR_ID,T2.SR_NAME,T1.ROUTE_ID,T3.ROUTE_NAME,T2.DIST_ZONE ZONE_ID,T2.DIVISION_NAME DIVISION_ID,
                            T2.ITEM_GROUP_ID FROM T_NON_PRODUCTIVE_SALES T1 
                            INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID 
                            INNER JOIN T_ROUTE T3 ON T1.ROUTE_ID=T3.ROUTE_ID AND T2.ITEM_COMPANY='" + companyID + "' " +
                            " AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T2.DIST_ZONE='" + zoneID + "' AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY')";

            string totalAndVisistedOutlet = @"SELECT TT1.TOTAL_OUTLET,TT2.VISITED_OUTLET, TT1.ITEM_GROUP_ID,TT1.DIVISION_ID,TT1.DIST_ZONE ZONE_ID,TT1.SR_ID FROM
                            (SELECT COUNT( DISTINCT T1.OUTLET_ID) TOTAL_OUTLET, T2.ITEM_GROUP_ID,T2.DIVISION_NAME DIVISION_ID,
                            T2.DIST_ZONE,T1.SR_ID,T1.ROUTE_ID 
                            FROM T_OUTLET T1 
                            INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID
                            AND T2.ITEM_COMPANY='" + companyID + "' AND T1.STATUS='Y'  AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T2.DIVISION_NAME='" + divisionID + "' " +
                                        " AND T2.DIST_ZONE='" + zoneID + "' GROUP BY T1.SR_ID,T1.ROUTE_ID,T2.DIST_ZONE,T2.DIVISION_NAME,T2.ITEM_GROUP_ID) TT1, " +
                                        @"(SELECT COUNT(OUTLET_ID) VISITED_OUTLET, ITEM_GROUP_ID,DIST_ZONE,SR_ID,ROUTE_ID FROM 
                            (SELECT OUTLET_ID, ITEM_GROUP_ID,DIST_ZONE,SR_ID,ROUTE_ID FROM
                            (SELECT T1.OUTLET_ID, T2.ITEM_GROUP_ID,T2.DIST_ZONE,T1.SR_ID,T1.ROUTE_ID FROM T_ORDER_DETAIL T1 
                            INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID 
                            AND T2.ITEM_COMPANY='" + companyID + "' AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T2.DIVISION_NAME='" + divisionID + "' " +
                                        " AND T2.DIST_ZONE='" + zoneID + "' AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY')) " +
                                        @"UNION  (SELECT T1.OUTLET_ID, T2.ITEM_GROUP_ID,T2.DIST_ZONE,T1.SR_ID,T1.ROUTE_ID FROM T_NON_PRODUCTIVE_SALES T1  
                            INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' " +
                                        " AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T2.DIVISION_NAME='" + divisionID + "'  AND T2.DIST_ZONE='" + zoneID + "' " +
                                        " AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY'))) " +
                                        " GROUP BY ROUTE_ID,SR_ID,DIST_ZONE,ITEM_GROUP_ID) TT2  WHERE TT1.SR_ID=TT2.SR_ID AND TT1.ROUTE_ID=TT2.ROUTE_ID";

        string qeTotalMemo = @"SELECT COUNT(DISTINCT OUTLET_ID) TOTAL_MEMO, SR_ID FROM (SELECT T1.OUTLET_ID,T1.SR_ID,T2.ITEM_GROUP_ID,T2.DIVISION_NAME DIVISION_ID,
                            T2.DIST_ZONE FROM T_ORDER_DETAIL T1 INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID AND T2.ITEM_COMPANY='" + companyID + "' " +
                          " AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T2.DIVISION_NAME='" + divisionID + "' AND T2.DIST_ZONE='"+zoneID+"'  " +
                          " AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY')) GROUP BY SR_ID ";

        string totalAmount = @"SELECT SR_ID, SUM(SUBTOTAL)/1000 TOTAL_AMOUNT FROM (SELECT T1.SR_ID,(T1.ITEM_CTN*T3.FACTOR+T1.ITEM_QTY)*T1.OUT_PRICE SUBTOTAL FROM T_ORDER_DETAIL T1
                            INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID 
                            INNER JOIN T_ITEM T3 ON T1.ITEM_ID=T3.ITEM_ID
                            AND T2.ITEM_COMPANY='" + companyID + "' AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' " +
                            " AND T2.DIVISION_NAME='" + divisionID + "' AND T2.DIST_ZONE='" + zoneID + "' AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY')) " +
                            " GROUP BY SR_ID ";


        string query = @"SELECT T7.*,T8.TOTAL_AMOUNT FROM
                        (SELECT T5.*,T6.TOTAL_MEMO FROM
                        (SELECT T3.*,T4.VISITED_OUTLET FROM
                        (SELECT T1.*,T2.TOTAL_OUTLET FROM
                        (SELECT DISTINCT T1.SR_ID,T2.SR_NAME,T1.ROUTE_ID,T3.ROUTE_NAME,T2.DIST_ZONE ZONE_ID,T2.DIVISION_NAME DIVISION_ID,
                                                    T2.ITEM_GROUP_ID FROM T_ORDER_DETAIL T1 
                                                    INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID
                                                    INNER JOIN T_ROUTE T3 ON T1.ROUTE_ID=T3.ROUTE_ID AND T2.ITEM_COMPANY='" + companyID + "' " +
                                                    @"AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T2.DIST_ZONE='" + zoneID + "' AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') " +
                                                    @"UNION 
                                                    SELECT DISTINCT T1.SR_ID,T2.SR_NAME,T1.ROUTE_ID,T3.ROUTE_NAME,T2.DIST_ZONE ZONE_ID,T2.DIVISION_NAME DIVISION_ID,
                                                    T2.ITEM_GROUP_ID FROM T_NON_PRODUCTIVE_SALES T1 
                                                    INNER JOIN T_SR_INFO T2 ON T1.SR_ID=T2.SR_ID 
                                                    INNER JOIN T_ROUTE T3 ON T1.ROUTE_ID=T3.ROUTE_ID AND T2.ITEM_COMPANY='" + companyID + "' " +
                                                    @"AND T2.ITEM_GROUP_ID='" + item_GROUP_ID + "' AND T2.DIST_ZONE='" + zoneID + "' AND T1.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY')) T1, " +

                        @"(SELECT SR_ID,ROUTE_ID,COUNT(OUTLET_ID) TOTAL_OUTLET FROM T_OUTLET WHERE STATUS='Y' AND ZONE_ID='" + zoneID + "' GROUP BY SR_ID,ROUTE_ID) T2 WHERE T1.SR_ID=T2.SR_ID AND T1.ROUTE_ID=T2.ROUTE_ID) T3, " +

                        @"(SELECT SR_ID,ROUTE_ID,COUNT(DISTINCT OUTLET_ID) VISITED_OUTLET FROM
                        (SELECT SR_ID,ROUTE_ID,OUTLET_ID FROM T_ORDER_DETAIL 
                        UNION
                        SELECT SR_ID,ROUTE_ID,OUTLET_ID FROM T_NON_PRODUCTIVE_SALES)
                        GROUP BY SR_ID,ROUTE_ID) T4
                        WHERE T3.SR_ID=T4.SR_ID AND T3.ROUTE_ID=T4.ROUTE_ID) T5,
                        (SELECT SR_ID,ROUTE_ID,COUNT(DISTINCT OUTLET_ID) TOTAL_MEMO FROM T_ORDER_DETAIL GROUP BY SR_ID,ROUTE_ID) T6
                        WHERE T5.SR_ID=T6.SR_ID AND T5.ROUTE_ID=T6.ROUTE_ID) T7,

                        (SELECT SUM(((TT1.ITEM_CTN*TT2.FACTOR)+TT1.ITEM_QTY)*TT1.OUT_PRICE) TOTAL_AMOUNT, TT1.SR_ID,TT1.ROUTE_ID FROM " +
                        @"(SELECT TBL1.* FROM (SELECT ITEM_ID,ITEM_QTY,ITEM_CTN,OUT_PRICE,OUTLET_ID,ROUTE_ID,SR_ID FROM T_ORDER_DETAIL WHERE ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY')) TBL1,(SELECT SR_ID FROM T_SR_INFO WHERE STATUS='Y' AND ITEM_GROUP_ID='" + item_GROUP_ID + "') TBL2 WHERE TBL1.SR_ID=TBL2.SR_ID) TT1, " +
                        @"(SELECT ITEM_ID,FACTOR FROM T_ITEM WHERE ITEM_GROUP='" + item_GROUP_ID + "') TT2 " +
                        @"WHERE TT1.ITEM_ID=TT2.ITEM_ID
                        GROUP BY TT1.SR_ID,TT1.ROUTE_ID) T8
                        WHERE T7.SR_ID=T8.SR_ID AND T7.ROUTE_ID=T8.ROUTE_ID ORDER BY T7.SR_ID";

        DataTable ActiveSR = GetData(query);
        //DataTable TotalAndVisitedOutlet = GetData(totalAndVisistedOutlet);
        //DataTable TotalMemo = GetData(qeTotalMemo);
        //DataTable TotalAmount = GetData(totalAmount);

        /*var myLINQ = from t1 in ActiveSR.AsEnumerable()
                     join t2 in TotalAndVisitedOutlet.AsEnumerable() on t1.Field<string>("SR_ID") equals t2.Field<string>("SR_ID")
                     join t3 in TotalMemo.AsEnumerable() on t1.Field<string>("SR_ID") equals t3.Field<string>("SR_ID")
                     join t4 in TotalAmount.AsEnumerable() on t1.Field<string>("SR_ID") equals t4.Field<string>("SR_ID")
                     select new
                     {
                         item_GROUP_ID = t1.Field<string>("ITEM_GROUP_ID"),
                         division_ID = t1.Field<string>("DIVISION_ID"),
                         zone_ID = t1.Field<string>("ZONE_ID"),
                         route_ID = t1.Field<string>("ROUTE_ID"),
                         route_NAME = t1.Field<string>("ROUTE_NAME"),
                         sr_ID = t1.Field<string>("SR_ID"),
                         sr_NAME = t1.Field<string>("SR_NAME"),
                         totalOutlet = t2.Field<decimal>("TOTAL_OUTLET"),
                         visitedOutlet = t2.Field<decimal>("VISITED_OUTLET"),
                         totalMemo = t3.Field<decimal>("TOTAL_MEMO"),
                         totalAmount = t4.Field<decimal>("TOTAL_AMOUNT"),
                        
                     };*/

        var myLINQ = from t1 in ActiveSR.AsEnumerable()                     
                     select new
                     {
                         item_GROUP_ID = t1.Field<string>("ITEM_GROUP_ID"),
                         division_ID = t1.Field<string>("DIVISION_ID"),
                         zone_ID = t1.Field<string>("ZONE_ID"),
                         route_ID = t1.Field<string>("ROUTE_ID"),
                         route_NAME = t1.Field<string>("ROUTE_NAME"),
                         sr_ID = t1.Field<string>("SR_ID"),
                         sr_NAME = t1.Field<string>("SR_NAME"),
                         totalOutlet = t1.Field<decimal>("TOTAL_OUTLET"),
                         visitedOutlet = t1.Field<decimal>("VISITED_OUTLET"),
                         totalMemo = t1.Field<decimal>("TOTAL_MEMO"),
                         totalAmount = t1.Field<decimal>("TOTAL_AMOUNT"),

                     };

        DataTable dt_NEW = new DataTable();
        dt_NEW.Columns.Add("ITEM_GROUP_ID");
        dt_NEW.Columns.Add("DIVISION_ID");
        dt_NEW.Columns.Add("ZONE_ID");
        dt_NEW.Columns.Add("ROUTE_ID");
        dt_NEW.Columns.Add("ROUTE_NAME");
        dt_NEW.Columns.Add("SR_ID");
        dt_NEW.Columns.Add("SR_NAME");
        dt_NEW.Columns.Add("TOTAL_OUTLET");
        dt_NEW.Columns.Add("VISITED_OUTLET");
        dt_NEW.Columns.Add("TOTAL_MEMO");
        dt_NEW.Columns.Add("TOTAL_AMOUNT");

        foreach (var item in myLINQ)
        {
            dt_NEW.Rows.Add(item.item_GROUP_ID, item.division_ID, item.zone_ID, item.route_ID, item.route_NAME,
            item.sr_ID, item.sr_NAME,item.totalOutlet,item.visitedOutlet,item.totalMemo,item.totalAmount);
        }




        fourthGrid.DataSource = dt_NEW;
        fourthGrid.DataBind();
    }

    private void BindProduct(string srID, GridView fifthGrid)
    {
        fifthGrid.ToolTip = srID;

        string salesProductSummary = @"SELECT T1.SR_ID,T1.ITEM_ID,T2.ITEM_NAME,T1.CARTON,T1.PIECE,T1.OUT_PRICE,(((T1.CARTON*T2.FACTOR)+T1.PIECE)*T1.OUT_PRICE) TOTAL_AMT FROM
                        (SELECT DISTINCT SR_ID,ITEM_ID,OUT_PRICE, SUM(ITEM_CTN) CARTON, SUM(ITEM_QTY) PIECE FROM T_ORDER_DETAIL
                        WHERE SR_ID='" + srID + "' AND ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') GROUP BY ITEM_ID,OUT_PRICE,SR_ID) T1, " +
                       "(SELECT ITEM_ID,ITEM_NAME, FACTOR FROM T_ITEM) T2 " +
                       " WHERE T1.ITEM_ID=T2.ITEM_ID  ORDER BY T1.CARTON DESC,T1.PIECE DESC";
        fifthGrid.DataSource = GetData(salesProductSummary);
        fifthGrid.DataBind();
    }

    private void BindOutletWiseProductDetails(string srID, string itemID, GridView fifthGrid)
    {
        fifthGrid.ToolTip = srID;

        string salesProductSummary = @"SELECT T1.OUTLET_ID,T1.OUTLET_NAME,SALESTIME,T1.SR_ID,T1.ITEM_ID,T2.ITEM_NAME,T1.CARTON,T1.PIECE,T1.OUT_PRICE,
                            (((T1.CARTON*T2.FACTOR)+T1.PIECE)*T1.OUT_PRICE) TOTAL_AMT FROM
                            (SELECT DISTINCT T3.SR_ID,T3.ITEM_ID,T3.OUTLET_ID,T4.OUTLET_NAME,T3.OUT_PRICE, SUM(T3.ITEM_CTN) CARTON,
                            TO_CHAR(T3.ENTRY_DATETIME, 'HH24:MI:SS') SALESTIME,
                            SUM(ITEM_QTY) PIECE FROM T_ORDER_DETAIL T3
                            INNER JOIN T_OUTLET T4 ON T3.OUTLET_ID=T4.OUTLET_ID " +
                            " WHERE T3.SR_ID='" + srID + "' AND T3.ITEM_ID='" + itemID + "' AND T3.ENTRY_DATE=TO_DATE('" + orderdate.Trim() + "','DD/MM/YYYY') " +
                            @" GROUP BY T3.OUTLET_ID,T3.SR_ID,T3.ITEM_ID,T3.OUT_PRICE,T3.ENTRY_DATETIME,T4.OUTLET_NAME) T1, 
                            (SELECT ITEM_ID,ITEM_NAME, FACTOR FROM T_ITEM) T2  WHERE T1.ITEM_ID=T2.ITEM_ID  ORDER BY T1.CARTON DESC,T1.PIECE DESC";
        fifthGrid.DataSource = GetData(salesProductSummary);
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
            string item_GROUP_ID = Convert.ToString((row.NamingContainer as GridView).DataKeys[row.RowIndex].Values[0]);
            string divisionID = Convert.ToString((row.NamingContainer as GridView).DataKeys[row.RowIndex].Values[1]);
            string zoneID = Convert.ToString((row.NamingContainer as GridView).DataKeys[row.RowIndex].Values[2]);

            GridView fourthGrid = row.FindControl("fourthGrid") as GridView;
            BindSR(item_GROUP_ID,divisionID,zoneID, fourthGrid);
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

    protected void Show_Hide_FifthLevel_Grid(object sender, EventArgs e)
    {
        ImageButton imgShowHide = (sender as ImageButton);
        GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
        if (imgShowHide.CommandArgument == "Show")
        {
            row.FindControl("pnlFifth").Visible = true;
            imgShowHide.CommandArgument = "Hide";
            imgShowHide.ImageUrl = "~/images/minus.png";
            string srID = Convert.ToString((row.NamingContainer as GridView).DataKeys[row.RowIndex].Values[0]);
            string itemID = Convert.ToString((row.NamingContainer as GridView).DataKeys[row.RowIndex].Values[1]);

            GridView sixthGrid = row.FindControl("sixthGrid") as GridView;
            BindOutletWiseProductDetails(srID, itemID, sixthGrid);
        }
        else
        {
            row.FindControl("pnlFifth").Visible = false;
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
        //BindSR(fourthGrid.ToolTip, fourthGrid);
    }

    protected void OnFifthLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView fifthGrid = (sender as GridView);
        fifthGrid.PageIndex = e.NewPageIndex;
        BindProduct(fifthGrid.ToolTip, fifthGrid);
    }

    protected void OnSixthLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView sixthGrid = (sender as GridView);
        sixthGrid.PageIndex = e.NewPageIndex;
        BindProduct(sixthGrid.ToolTip, sixthGrid);
    }

}