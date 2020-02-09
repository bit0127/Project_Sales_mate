using Microsoft.Reporting.WebForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {

            //var connectionString = ConfigurationManager.ConnectionStrings["OracleDBMain"].ConnectionString;

           

//            string constr = @"Data Source=(DESCRIPTION=
//                (ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost )(PORT=1522)))
//                (CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=PAL)));
//                User Id=OUTLET;Password=outlet";

            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();
            string sql = "SELECT * FROM TBL_EMP";
            OracleCommand command = new OracleCommand(sql, conn);
            OracleDataAdapter da = new OracleDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int c = ds.Tables[0].Rows.Count;

            if (c > 0)
            {
                
               

            }
            else
            {
                string q = "INSERT INTO TBL_EMP(ID,NAME) VALUES('139399','Rathindra Nath')";
                OracleCommand cmd = new OracleCommand(q, conn);
                int i = cmd.ExecuteNonQuery();
                string msg = "Inserted!";
            }
            conn.Close();

        }
        catch (Exception ex)
        {

        }
    }
}