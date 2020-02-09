using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["uid"] = null;
    }

     
    [WebMethod(EnableSession = true)]
    public static string CheckUserData(string uid, string pwd)
    {
        string msg = "Invalid User!";
        string connString = ConfigurationManager.ConnectionStrings["LiveDBConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(connString);

        try
        {
            conn.Open();

            string qr = @"SELECT * FROM TBL_USER WHERE USER_ID='" + uid + "' AND PASSWORD='" + pwd + "'";
            SqlCommand cmd = new SqlCommand(qr, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0 && ds.Tables[0].Rows[0][0].ToString() != "") 
            {
                msg = ds.Tables[0].Rows[0]["USER_TYPE"].ToString();
                HttpContext.Current.Session["uid"] = uid;
            } 

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }

    
    [WebMethod(EnableSession = true)]
    public static string GetStudentInfo(string className)
    {
        string msg = "";
        string connString = ConfigurationManager.ConnectionStrings["LiveDBConnectionString"].ConnectionString;
        SqlConnection conn = new SqlConnection(connString);

        try
        {
            conn.Open();

            string qr = @"SELECT T1.STUDENT_ID,T1.STUDENT_NAME,T1.FATHER_NAME,T1.MOTHER_NAME,T1.STUDENT_MOBILE_NO,T1.ADDRESS,T1.BIRTH_DATH,T1.AGE,T1.GENDER,T1.BLOOD_GROUP,T1.GROUP_NAME,T1.SECTION_NAME,T2.PICTURE_LOCATION FROM TBL_STUDENT_INFO AS T1
                         INNER JOIN TBL_PICTURE_LOCATION AS T2
                         ON T1.STUDENT_ID=T2.STUDENT_ID
                          WHERE  CLASS_ID='" + className + "' ORDER BY T1.ROLL_NO";
            SqlCommand cmd = new SqlCommand(qr, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;
            if (c > 0 && ds.Tables[0].Rows[0][0].ToString() != "") 
            {
                for (int z = 0; z < c; z++)
                {
                    string studentId = ds.Tables[0].Rows[z]["STUDENT_ID"].ToString();
                    string studentName = ds.Tables[0].Rows[z]["STUDENT_NAME"].ToString();
                    string picLocation = ds.Tables[0].Rows[z]["PICTURE_LOCATION"].ToString();
                    msg = msg + ";" + studentId + ";" + studentName + ";" + picLocation;
                }
            } 

            conn.Close();
        }
        catch (Exception ex) { }

        return msg;
    }
}