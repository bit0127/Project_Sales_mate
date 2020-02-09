using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class chartinfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    [WebMethod(EnableSession = true)]
    public static List<Data> GetNationalSalesData(string fromDate, string toDate)
    {
        List<Data> dataList = new List<Data>();
        try
        {
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

            DataTable dt = new DataTable();
            dt.Columns.Add("HOSINFO", typeof(string));
            dt.Columns.Add("TOTAL_AMOUNT", typeof(double));

            try
            {
                OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
                conn.Open();
                
                //pie

                string qrHos = @"SELECT (T1.HOS_NAME || '(' ||T1.STAFF_ID||') - '||T1.MOBILE_NO||' - '||T1.ITEM_GROUP) HOSINFO,T2.ITEM_GROUP_ID FROM
                            (SELECT STAFF_ID,HOS_NAME,MOBILE_NO,ITEM_GROUP FROM T_HOS)T1,
                            (SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString() + "' AND STATUS='Y') T2 WHERE T1.ITEM_GROUP=T2.ITEM_GROUP_NAME";
                OracleCommand cmd = new OracleCommand(qrHos, conn);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                int s = ds.Tables[0].Rows.Count;

                string HOSINFO = "No HOS info";
                if (s > 0)
                {
                    int k = 1;

                    for (int i = 0; i < s; i++)
                    {
                        HOSINFO = ds.Tables[0].Rows[i]["HOSINFO"].ToString();
                        string groupId = ds.Tables[0].Rows[i]["ITEM_GROUP_ID"].ToString();

                        string qrAmt = @"SELECT TO_CHAR(SUM((T1.ITEM_QTY+(T1.ITEM_CTN*T2.FACTOR))*T1.OUT_PRICE),'999999999.99') AMT FROM         
                                                (SELECT * FROM T_ORDER_DETAIL    
                                                WHERE ENTRY_DATE BETWEEN TO_DATE('" + fromDate + "','DD/MM/YYYY') AND TO_DATE('" + toDate + "','DD/MM/YYYY') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y')) T1, (SELECT ITEM_ID,FACTOR FROM T_ITEM) T2 WHERE T1.ITEM_ID=T2.ITEM_ID";

                        OracleCommand cmdAmt = new OracleCommand(qrAmt, conn);
                        OracleDataAdapter daAmt = new OracleDataAdapter(cmdAmt);
                        DataSet dsAmt = new DataSet();
                        daAmt.Fill(dsAmt);
                        int amt = dsAmt.Tables[0].Rows.Count;
                        if (amt > 0 && dsAmt.Tables[0].Rows[0][0].ToString() != "")
                        {
                            string totalAmount = dsAmt.Tables[0].Rows[0]["AMT"].ToString();
                            totalAmount = String.Format("{0:0.00}", (Convert.ToDouble(totalAmount) / 1000));

//                            
                            dt.Rows.Add(HOSINFO, Convert.ToDouble(totalAmount));

                        }

                    }
                }


                conn.Close();
            }
            catch (Exception ex) { }


            string hos = "";
            int val = 0;

            foreach (DataRow dr in dt.Rows)
            {

                hos = dr[0].ToString();
                val = Convert.ToInt32(dr[1]);
                dataList.Add(new Data(hos, val));

            }
        }
        catch (Exception ex)
        {

        }

        return dataList;

    }


    [WebMethod(EnableSession = true)]
    public static List<Data> GetDivisionalSalesData(string fromDate, string toDate)
    {
        List<Data> dataList = new List<Data>();
        try
        {
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

            DataTable dt = new DataTable();
            dt.Columns.Add("RMINFO", typeof(string));
            dt.Columns.Add("TOTAL_AMOUNT", typeof(double));

            try
            {
                OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
                conn.Open();

                //pie

                string qrHos = @"SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString() + "' AND STATUS='Y'";
                OracleCommand cmd = new OracleCommand(qrHos, conn);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                int s = ds.Tables[0].Rows.Count;

                string RMINFO = "No RM info";
                if (s > 0)
                { 

                    for (int i = 0; i < s; i++)
                    {

                        string groupId = ds.Tables[0].Rows[i]["ITEM_GROUP_ID"].ToString();
                        string groupName = ds.Tables[0].Rows[i]["ITEM_GROUP_NAME"].ToString();

                        string qrRM = @"SELECT T1.*,T2.DIVISION_NAME FROM
                                              (SELECT RM_ID,RM_NAME,MOBILE_NO,DIVISION_ID FROM T_AGM_RM WHERE STATUS='Y' AND ITEM_GROUP LIKE '%" + groupName + "%') T1," +
                                                       @"(SELECT DIVISION_ID,DIVISION_NAME FROM T_DIVISION) T2
                                              WHERE T1.DIVISION_ID=T2.DIVISION_ID";
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
                                string DIVISION_NAME = dsRM.Tables[0].Rows[q]["DIVISION_NAME"].ToString();
                                string T_AMOUNT = "0";

                                RMINFO = RM_NAME + "(" + RM_ID + ") - " + MOBILE_NO + " - " + DIVISION_NAME + " - " + groupName;

                                string qrAmnt = @"SELECT SUM(T3.TOTAL_AMT) TOTAL_AMOUNT FROM
                                            (SELECT T1.SR_ID, (SUM(((T1.ITEM_CTN*T2.FACTOR)+T1.ITEM_QTY)*T1.OUT_PRICE)/1000) TOTAL_AMT FROM
                                            (SELECT SR_ID,ITEM_ID,ITEM_CTN,ITEM_QTY,OUT_PRICE FROM T_ORDER_DETAIL WHERE ENTRY_DATE BETWEEN TO_DATE('" + fromDate.Trim() + "','DD/MM/YYYY') AND TO_DATE('" + toDate.Trim() + "','DD/MM/YYYY')) T1, " +
                                                @"(SELECT ITEM_ID,FACTOR FROM T_ITEM WHERE ITEM_GROUP='" + groupId + "') T2 " +
                                                @"WHERE T1.ITEM_ID=T2.ITEM_ID GROUP BY T1.SR_ID) T3,
                                            (SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y' " +
                                                   @"AND SR_ID IN(SELECT SR_ID FROM T_TSM_SR WHERE TSM_ID IN(SELECT TSM_ID FROM T_TSM_ZM WHERE AGM_RM_ID='" + RM_ID + "' AND STATUS='Y')) " +
                                                @") T4
                                            WHERE T3.SR_ID=T4.SR_ID";

                                OracleCommand cmddd = new OracleCommand(qrAmnt, conn);
                                OracleDataAdapter daaa = new OracleDataAdapter(cmddd);
                                DataSet dsss = new DataSet();
                                daaa.Fill(dsss);
                                int ss = dsss.Tables[0].Rows.Count;

                                if (ss > 0 && dsss.Tables[0].Rows[0]["TOTAL_AMOUNT"].ToString().Length > 0)
                                {
                                    T_AMOUNT = String.Format("{0:0.00}", Convert.ToDouble(dsss.Tables[0].Rows[0]["TOTAL_AMOUNT"].ToString()));
                                    dt.Rows.Add(RMINFO, Convert.ToDouble(T_AMOUNT));
                                }

                            }
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception ex) { }


            string hos = "";
            int val = 0;

            foreach (DataRow dr in dt.Rows)
            {

                hos = dr[0].ToString();
                val = Convert.ToInt32(dr[1]);
                dataList.Add(new Data(hos, val));

            }
        }
        catch (Exception ex)
        {

        }

        return dataList;

    }

         
    public void GetNationalSalesvsTargetData(string fromDate, string toDate)
    {
        List<SalesVsTargetData> dataList = new List<SalesVsTargetData>();
        StringBuilder str = new StringBuilder();
        DataTable dt = new DataTable();
        dt.Columns.Add("ITEM_GROUP_NAME", typeof(string));
        dt.Columns.Add("TOTAL_AMOUNT", typeof(double));
        dt.Columns.Add("TARGET", typeof(double));

        try
        { 
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
                        
            try
            {
                OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
                conn.Open();
                
                //pie
                               
                double target = 5050000;

                string qrHos = @"SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString() + "' AND STATUS='Y'";
                OracleCommand cmd = new OracleCommand(qrHos, conn);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                int s = ds.Tables[0].Rows.Count;

                string ITEM_GROUP_NAME = "No Group info";
                if (s > 0)
                { 
                    for (int i = 0; i < s; i++)
                    {
                        ITEM_GROUP_NAME = ds.Tables[0].Rows[i]["ITEM_GROUP_NAME"].ToString();
                        string groupId = ds.Tables[0].Rows[i]["ITEM_GROUP_ID"].ToString();

                        string qrAmt = @"SELECT TO_CHAR(SUM((T1.ITEM_QTY+(T1.ITEM_CTN*T2.FACTOR))*T1.OUT_PRICE),'999999999.99') AMT FROM         
                                                (SELECT * FROM T_ORDER_DETAIL    
                                                WHERE ENTRY_DATE BETWEEN TO_DATE('" + fromDate + "','DD/MM/YYYY') AND TO_DATE('" + toDate + "','DD/MM/YYYY') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y')) T1, (SELECT ITEM_ID,FACTOR FROM T_ITEM) T2 WHERE T1.ITEM_ID=T2.ITEM_ID";

                        OracleCommand cmdAmt = new OracleCommand(qrAmt, conn);
                        OracleDataAdapter daAmt = new OracleDataAdapter(cmdAmt);
                        DataSet dsAmt = new DataSet();
                        daAmt.Fill(dsAmt);
                        int amt = dsAmt.Tables[0].Rows.Count;
                        if (amt > 0 && dsAmt.Tables[0].Rows[0][0].ToString() != "")
                        {
                            string totalAmount = dsAmt.Tables[0].Rows[0]["AMT"].ToString();
                            totalAmount = String.Format("{0:0.00}", (Convert.ToDouble(totalAmount) / 1000));

                            dt.Rows.Add(ITEM_GROUP_NAME, Convert.ToDouble(totalAmount), Convert.ToDouble(target));

                        }

                        target = target + 125000;
                    }
                }


                conn.Close();
            }
            catch (Exception ex) { }


            string group = "";
            double sales = 0;
            double target_amt = 0;

            foreach (DataRow dr in dt.Rows)
            {

                group = dr[0].ToString();
                sales = Convert.ToDouble(dr[1]);
                target_amt = Convert.ToDouble(dr[2]);
                dataList.Add(new SalesVsTargetData(group, sales, target_amt));

            }
 
            /*str.Append(@"<script type=*text/javascript*> google.load( *visualization*, *1*, {packages:[*corechart*]});

                       google.setOnLoadCallback(drawChart);

                       function drawChart() {

        var data = new google.visualization.DataTable();

        data.addColumn('string', 'Group Name');

        data.addColumn('number', 'Sales');

        data.addColumn('number', 'Target');      

 

        data.addRows(" + dt.Rows.Count + ");");

 

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {

                str.Append("data.setValue( " + i + "," + 0 + "," + "'" + dt.Rows[i]["ITEM_GROUP_NAME"].ToString() + "');");

                str.Append("data.setValue(" + i + "," + 1 + "," + dt.Rows[i]["TOTAL_AMOUNT"].ToString() + ") ;");

                str.Append("data.setValue(" + i + "," + 2 + "," + dt.Rows[i]["TARGET"].ToString() + ") ;");
                 
            }



            str.Append(" var chart = new google.visualization.ColumnChart(document.getElementById('chart_div'));");

            str.Append(" chart.draw(data, {width: 650, height: 300, title: 'Group wise Sales Performance',");

            str.Append("hAxis: {title: 'Year', titleTextStyle: {color: 'green'}}");

            str.Append("}); }");

            str.Append("</script>");

            str.ToString().Replace('*', '"');*/
            
        }

        catch

        {   }

        
    }


    private void GetLineData(string fromDate, string toDate)
    {
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
        StringBuilder str = new StringBuilder();

        DataTable dt = new DataTable();
        dt.Columns.Add("HOSINFO", typeof(string));
        dt.Columns.Add("TOTAL_AMOUNT", typeof(double));

        try

        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string qrHos = @"SELECT (T1.HOS_NAME || '(' ||T1.STAFF_ID||') - '||T1.MOBILE_NO||' - '||T1.ITEM_GROUP) HOSINFO,T2.ITEM_GROUP_ID FROM
                            (SELECT STAFF_ID,HOS_NAME,MOBILE_NO,ITEM_GROUP FROM T_HOS)T1,
                            (SELECT ITEM_GROUP_ID,ITEM_GROUP_NAME FROM T_ITEM_GROUP WHERE COMPANY_ID='" + HttpContext.Current.Session["company"].ToString() + "' AND STATUS='Y') T2 WHERE T1.ITEM_GROUP=T2.ITEM_GROUP_NAME";
            OracleCommand cmd = new OracleCommand(qrHos, conn);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int s = ds.Tables[0].Rows.Count;

            string HOSINFO = "No HOS info";
            if (s > 0)
            {
               
                for (int i = 0; i < s; i++)
                {
                    HOSINFO = ds.Tables[0].Rows[i]["HOSINFO"].ToString();
                    string groupId = ds.Tables[0].Rows[i]["ITEM_GROUP_ID"].ToString();

                    string qrAmt = @"SELECT TO_CHAR(SUM((T1.ITEM_QTY+(T1.ITEM_CTN*T2.FACTOR))*T1.OUT_PRICE),'999999999.99') AMT FROM         
                                                (SELECT * FROM T_ORDER_DETAIL    
                                                WHERE ENTRY_DATE BETWEEN TO_DATE('" + fromDate + "','DD/MM/YYYY') AND TO_DATE('" + toDate + "','DD/MM/YYYY') AND SR_ID IN(SELECT SR_ID FROM T_SR_INFO WHERE ITEM_GROUP_ID='" + groupId + "' AND STATUS='Y')) T1, (SELECT ITEM_ID,FACTOR FROM T_ITEM) T2 WHERE T1.ITEM_ID=T2.ITEM_ID";

                    OracleCommand cmdAmt = new OracleCommand(qrAmt, conn);
                    OracleDataAdapter daAmt = new OracleDataAdapter(cmdAmt);
                    DataSet dsAmt = new DataSet();
                    daAmt.Fill(dsAmt);
                    int amt = dsAmt.Tables[0].Rows.Count;
                    if (amt > 0 && dsAmt.Tables[0].Rows[0][0].ToString() != "")
                    {
                        string totalAmount = dsAmt.Tables[0].Rows[0]["AMT"].ToString();
                        totalAmount = String.Format("{0:0.00}", (Convert.ToDouble(totalAmount) / 1000));
                                                
                        dt.Rows.Add(HOSINFO, Convert.ToDouble(totalAmount));

                    }

                }
            }

            conn.Close();
          

            str.Append(@"<script type=*text/javascript*> google.load( *visualization*, *1*, {packages:[*corechart*]});

            google.setOnLoadCallback(drawChart);

            function drawChart() {

            var data = new google.visualization.DataTable();

            data.addColumn('string', 'Year');

            data.addColumn('number', 'Purchase');

            data.addColumn('number', 'Sales');

            data.addColumn('number', 'Expenses');

 

            data.addRows(" + dt.Rows.Count + ");");

 

            for (int i = 0; i <= dt.Rows.Count - 1; i++)

            {

                str.Append("data.setValue( " + i + "," + 0 + "," + "'" + dt.Rows[i]["year"].ToString() + "');");

                str.Append("data.setValue(" + i + "," + 1 + "," + dt.Rows[i]["purchase"].ToString() + ") ;");

                str.Append("data.setValue(" + i + "," + 2 + "," + dt.Rows[i]["sales"].ToString() + ") ;");

                str.Append("data.setValue(" + i + "," + 3 + "," + dt.Rows[i]["expences"].ToString() + ");");

 

            }

           

            str.Append("   var chart = new google.visualization.LineChart(document.getElementById('chart_div'));");           

            str.Append(" chart.draw(data, {width: 660, height: 300, title: 'Company Performance',");           

            str.Append("hAxis: {title: 'Year', titleTextStyle: {color: 'green'}}");

            str.Append("}); }");

            str.Append("</script>");

            //lt.Text = str.ToString().Replace('*', '"');        

        }

        catch

        { }   

    }
 



}

public class Data
{

    public string ColumnName = "";

    public int Value = 0;

    public Data(string columnName, int value)
    {

        ColumnName = columnName;

        Value = value;
    }
}


public class SalesVsTargetData
{

    public string GroupName = "";
    public double Sales = 0;
    public double Trgt = 0;


    public SalesVsTargetData(string groupName, double sales, double trgt)
    {
        GroupName = groupName;
        Sales = sales;
        Trgt = trgt;
    }
}