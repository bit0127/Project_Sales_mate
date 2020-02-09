using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OracleDBConnectionClass
/// </summary>
public class OracleDBConnectionClass
{
	public OracleDBConnectionClass()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string OracleConnectionString() 
    {
        var connectionString = "";
        try
        {
            connectionString = ConfigurationManager.ConnectionStrings["OracleDBMain"].ConnectionString;
        }
        catch (Exception ex)
        {

        }

        return connectionString;
    }
}