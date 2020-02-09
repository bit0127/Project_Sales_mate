using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.IO;



using System.Xml;
using System.Configuration;

public static class CommonDBSvc
{
    public static DataSet ds = new DataSet();

    public static int i;
    public static string BasePath = System.Web.HttpContext.Current.Server.MapPath("\\");
    //=@"D:\reportingBI\Reporting BI\";//default base path.

    //public void LoadBasePath()
    //{
    //    //string strQuery = "Select ParamValue From ApplicationSetting Where ParamName = 'BasePath'";
    //    //string path = this.ExecuteQueryAndReturnAll(strQuery).Split('|')[0];
    //    //this.BasePath = @path;
    //    this.BasePath = System.Web.HttpContext.Current.Server.MapPath("\\");
    //}

    //public string LoadTable(string tableName, string wherePart)
    //{
    //    string Query = null;
    //    Query = "select * from " + tableName + " where " + wherePart;


    //    DataSet ds = new DataSet();
    //    ds = GetDataSet(Query);

    //    string retVal = null;
    //    retVal = "";
    //    try
    //    {
    //        if (ds.Tables.Count >= 1)
    //        {
    //            if (ds.Tables[0].Columns.Count >= 1)
    //            {
    //                for (i = 0; i <= ds.Tables[0].Columns.Count - 1; i++)
    //                {
    //                    retVal = retVal + (ds.Tables[0].Rows[0][i]).ToString().Trim() + "|";

    //                }
    //            }
    //            else
    //            {
    //                return "NoData";
    //            }
    //        }
    //        else
    //        {
    //            return "NoData";
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        return e.Message;
    //    }
    //    return retVal;
    //}

    public static string GetConnectionString()
    {
        return System.Configuration.ConfigurationManager.ConnectionStrings["CnStringMain"].ConnectionString;
    }

    public static DataSet GetDataSet(string Query)
    {
        try
        {
            string cnString = null;
            cnString = GetConnectionString();
            DataSet ds = new DataSet("DataSet1");
            SqlConnection con = new SqlConnection(cnString);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(Query, con);
            da.Fill(ds);
            con.Close();
            return ds;
        }
        catch
        {
            return ds;
        }
    }

    //public string CheckExistInTable(string TableName, string Cond)
    //{

    //    string cnStr = GetConnectionString();
    //    SqlConnection con = new SqlConnection(cnStr);
    //    SqlCommand cmd = new SqlCommand("select * from " + TableName + " where " + Cond, con);
    //    try
    //    {
    //        con.Open();
    //        cmd.ExecuteNonQuery();
    //        SqlDataReader rd = null;
    //        rd = cmd.ExecuteReader();

    //        if ((rd.HasRows))
    //        {
    //            con.Close();
    //            return "YES";
    //        }
    //        else
    //        {
    //            con.Close();
    //            return "NO";
    //        }
    //    }
    //    catch
    //    {
    //        con.Close();
    //        return "-ERR";
    //    }
    //}

    //public static string GetValueFromTable(string TableName, string ColName, string Cond)
    //{

    //    string cnStr = GetConnectionString();
    //    SqlConnection con = new SqlConnection(cnStr);
    //    string retVal = null;
    //    string query = null;
    //    if (string.IsNullOrEmpty(Cond))
    //    {
    //        query = "select cast(" + ColName + " as nvarchar(50)) from " + TableName;
    //    }
    //    else
    //    {
    //        query = "select cast(" + ColName + " as nvarchar(50)) from " + TableName + " where " + Cond;
    //    }
    //    try
    //    {
    //        SqlCommand cmd = new SqlCommand(query, con);
    //        con.Open();
    //        cmd.ExecuteNonQuery();
    //        SqlDataReader rd = null;
    //        rd = cmd.ExecuteReader();
    //        if ((rd.HasRows))
    //        {
    //            rd.Read();
    //            retVal = rd.GetString(0);
    //            con.Close();
    //            return retVal;
    //        }
    //        else
    //        {
    //            con.Close();
    //            return "NOVALUE";
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        con.Close();
    //        return "-ERR" + e.Message + " " + query;
    //    }
    //}

    //public string IsExist(string TableName, string ColName, string Cond)
    //{

    //    string cnStr = GetConnectionString();
    //    SqlConnection con = new SqlConnection(cnStr);
    //    string retVal = null;
    //    string query = null;
    //    if (string.IsNullOrEmpty(Cond))
    //    {
    //        query = "select cast(" + ColName + " as nvarchar(50)) from " + TableName;
    //    }
    //    else
    //    {
    //        query = "select cast(" + ColName + " as nvarchar(50)) from " + TableName + " where " + Cond;
    //    }
    //    try
    //    {
    //        SqlCommand cmd = new SqlCommand(query, con);
    //        con.Open();
    //        cmd.ExecuteNonQuery();
    //        SqlDataReader rd = null;
    //        rd = cmd.ExecuteReader();
    //        if ((rd.HasRows))
    //        {
    //            rd.Read();
    //            retVal = rd.GetString(0);
    //            con.Close();
    //            return "yes";
    //        }
    //        else
    //        {
    //            con.Close();
    //            return "NOVALUE";
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        con.Close();
    //        return "-ERR" + e.Message + " " + query;
    //    }
    //}

    public static string ExecuteQuery(string Query)
    {
        string cnStr = GetConnectionString();
        //cnStr = "Data Source=192.168.241.2;Initial Catalog=master;Persist Security Info=True;User ID=sa;Password=Nopass123";
        SqlConnection con = new SqlConnection(cnStr);
        try
        {
            SqlCommand cmd = new SqlCommand(Query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return "Successful!";
        }
        catch (Exception e)
        {
            con.Close();
            return "-ERR: " + e.Message;
        }
    }

    //public string ExecuteQuery(string Query, string ConnectionString)
    //{
    //    SqlConnection con = new SqlConnection(ConnectionString);
    //    try
    //    {
    //        SqlCommand cmd = new SqlCommand(Query, con);
    //        cmd.CommandTimeout = 0;
    //        con.Open();
    //        cmd.ExecuteNonQuery();
    //        con.Close();
    //        return "Executed";
    //    }
    //    catch (Exception e)
    //    {
    //        con.Close();
    //        return "-ERR: " + e.Message;
    //    }
    //}

    //public string ExecuteQueryAndReturnAll(string Query)
    //{
    //    string cnStr = GetConnectionString();
    //    SqlConnection con = new SqlConnection(cnStr);
    //    string retVal = "";
    //    int i = 0;
        
    //    string typedata = null;
    //    try
    //    {
    //        SqlCommand cmd = new SqlCommand(Query, con);
    //        con.Open();
    //        //cmd.ExecuteNonQuery();
    //        SqlDataReader rd = null;
    //        rd = cmd.ExecuteReader();
    //        if ((rd.HasRows))
    //        {
    //            while (rd.Read())
    //            {
    //                for (i = 0; i <= rd.FieldCount - 1; i++)
    //                {
    //                    typedata = rd[i].GetType().Name;
    //                    if (rd[i].GetType().Name == "Decimal")
    //                    {
    //                        retVal = retVal + rd.GetDecimal(i) + "|";
    //                    }
    //                    else if (rd[i].GetType().Name == "Int32")
    //                    {
    //                        retVal = retVal + rd.GetInt32(i) + "|";
    //                    }
    //                    else if (rd[i].GetType().Name == "Int64")
    //                    {
    //                        retVal = retVal + rd.GetInt64(i) + "|";
    //                    }
    //                    else if (rd[i].GetType().Name == "DateTime")
    //                    {
    //                        retVal = retVal + rd.GetDateTime(i) + "|";
    //                    }
    //                    else
    //                    {
    //                        retVal = retVal + rd.GetString(i) + "|";
    //                    }
    //                }

    //            }
    //            con.Close();
    //            return retVal;
    //        }
    //        else
    //        {
    //            con.Close();
    //            return "NOVALUE";
    //        }

            

    //    }
    //    catch (Exception e)
    //    {
    //        con.Close();
    //        return "-ERR" + e.Message;
    //    }
    //}


    //public string GetValueFromFunction(string FunctionName, string Param)
    //{

    //    string cnStr = GetConnectionString();
    //    SqlConnection con = new SqlConnection(cnStr);
    //    string retVal = null;
    //    try
    //    {
    //        SqlCommand cmd = new SqlCommand("select  dbo." + FunctionName + " (" + Param + ") ", con);
    //        con.Open();
    //        cmd.ExecuteNonQuery();
    //        SqlDataReader rd = null;
    //        rd = cmd.ExecuteReader();
    //        if ((rd.HasRows))
    //        {
    //            rd.Read();
    //            retVal = rd.GetString(0);
    //            con.Close();
    //            return retVal;
    //        }
    //        else
    //        {
    //            con.Close();
    //            return "NOVALUE";
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        con.Close();
    //        return "-ERR" + e.Message;
    //    }
    //}


    //public string GetValueListFromTable(string Query)
    //{
    //    DataSet ds = new DataSet();
    //    ds = GetDataSet(Query);

    //    string retVal = null;
    //    try
    //    {
    //        retVal = "";
    //        for (i = 0; i <= ds.Tables[0].Columns.Count - 1; i++)
    //        {
    //            retVal = retVal + ds.Tables[0].Rows[0][i] + "|";
    //        }

    //    }
    //    catch (Exception e)
    //    {
    //        return "-ERR" + e.Message;
    //    }
    //    return retVal;
    //}

    //public string InsertQuery(string TableName, string Values)
    //{
    //    string cnStr = GetConnectionString();
    //    SqlConnection con = new SqlConnection(cnStr);
    //    try
    //    {
    //        SqlCommand cmd = new SqlCommand("INSERT INTO " + TableName + " VALUES (" + Values + ")", con);
    //        con.Open();
    //        cmd.ExecuteNonQuery();
    //        con.Close();
    //        return "+OK";
    //    }
    //    catch
    //    {
    //        con.Close();
    //        return "-ERR";
    //    }
    //}

    //public DataTable GetData(string Query)
    //{
    //    string cnStr = GetConnectionString();
    //    SqlConnection con = new SqlConnection(cnStr);
    //    try
    //    {
    //        con.Open();
    //        SqlCommand cmd = new SqlCommand(Query, con);

    //        DataTable dt = new DataTable();
    //        dt.Load(cmd.ExecuteReader());
    //        con.Close();
    //        return dt;
    //    }
    //    catch(Exception)
    //    {
    //        con.Close();
    //        return new DataTable();
    //    }
    //}

    //public DataTable GetData(string cnStr, string Query)
    //{
    //    SqlConnection con = new SqlConnection(cnStr);
    //    try
    //    {
    //        con.Open();
    //        SqlCommand cmd = new SqlCommand(Query, con);

    //        DataTable dt = new DataTable();
    //        dt.Load(cmd.ExecuteReader());
    //        con.Close();
    //        return dt;
    //    }
    //    catch(Exception)
    //    {
    //        con.Close();
    //        return new DataTable();
    //    }
    //}

    //public string UpdateQuery(string TableName, string Set, string Cond)
    //{
    //    string cnStr = GetConnectionString();
    //    SqlConnection con = new SqlConnection(cnStr);
    //    string qry;
    //    try
    //    {
    //        qry = "UPDATE " + TableName + " SET " + Set + " WHERE " + Cond;
            
    //        SqlCommand cmd = new SqlCommand(qry, con);
    //        con.Open();
    //        cmd.ExecuteNonQuery();
    //        con.Close();
    //        return "+OK";
    //    }
    //    catch
    //    {
    //        con.Close();
    //        return "-ERR";
    //    }
    //}

    //public string DeleteQuery(string TableName, string Cond)
    //{
    //    string cnStr = GetConnectionString();
    //    SqlConnection con = new SqlConnection(cnStr);
    //    string qry;
    //    try
    //    {
    //        qry = "DELETE FROM " + TableName;
    //        if (Cond != "")
    //        {
    //            qry += " WHERE " + Cond;
    //        }
    //        SqlCommand cmd = new SqlCommand(qry, con);
    //        con.Open();
    //        cmd.ExecuteNonQuery();
    //        con.Close();
    //        return "+OK";
    //    }
    //    catch
    //    {
    //        con.Close();
    //        return "-ERR";
    //    }
    //}

    //public DataTable SelectDataTable(String Sql)
    //{
    //    DataTable dt = new DataTable();
    //    SqlConnection oSqlConnection = new SqlConnection(GetConnectionString());
    //    try
    //    {
    //        oSqlConnection.Open();
    //        SqlDataAdapter sqlda = new SqlDataAdapter(Sql, GetConnectionString());
    //        sqlda.Fill(dt);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }
    //    finally
    //    {
    //        oSqlConnection.Close();
    //        oSqlConnection.Dispose();
    //    }

    //    return dt;
    //}

   
}

