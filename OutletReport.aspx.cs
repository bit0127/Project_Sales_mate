using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OutletReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public static byte[] StringToByteArrays(string hex)
    {
        return Enumerable.Range(0, hex.Length)
                         .Where(x => x % 2 == 0)
                         .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                         .ToArray();
    }


    public static byte[] StringToByteArray(string hex)
    {
        byte[] HexAsBytes = null;
        try
        {
            hex = hex.Substring(2, hex.Length - 2);
            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hex));
            }

            HexAsBytes = new byte[hex.Length / 2];

            for (int index = 0; index < HexAsBytes.Length; index++)
            {
                string byteValue = hex.Substring(index * 2, 2);
                HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
        }
        catch (Exception ex)
        {
        }


        return HexAsBytes;
    }

    [WebMethod]
    public static List<Group> LoadAllGroup()
    {
        List<Group> objGroupList = new List<Group>();
        Group objGroup = null;

//        try
//        {
//            string USER = "25272";// "99863";
//           OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
//           OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());
//           con.Open();

//            string queryGroup = @"SELECT T_SCRT.SCRT_MENU MGRUP_OID,T_MGRUP.MGRUP_NAME MGroup, T_MGRUP.NCGP_TEXT groupId, 
//                                T_MGRUP.MGRUP_MCOM Company
//                                FROM OUTLET.T_SCRT, OUTLET.T_MGRUP 
//
//                                WHERE T_SCRT.SCRT_RFNO = 'MGroup_Lov'
//                                AND T_SCRT.SCRT_ACTV = 'Y' 
//                                AND T_SCRT.SCRT_USER = " + "'" + USER + "'";

//            queryGroup = queryGroup + @" AND (T_SCRT.SCRT_MENU = T_MGRUP.MGRUP_OID) 
//                                AND T_MGRUP.MGRUP_ACTV = 'Y' 
//                                AND LENGTH(T_MGRUP.MGRUP_OID) > 4 
//                                GROUP BY T_SCRT.SCRT_MENU, T_MGRUP.MGRUP_NAME,T_MGRUP.NCGP_TEXT, T_MGRUP.MGRUP_MCOM, T_MGRUP.MGRUP_SQEN, T_MGRUP.MGRUP_OID
//                                ORDER BY T_MGRUP.MGRUP_SQEN ASC, T_MGRUP.MGRUP_OID ASC";

//            OracleCommand cmdGroup = new OracleCommand(queryGroup, con);

//            OracleDataAdapter daGid = new OracleDataAdapter(cmdGroup);
//            DataSet dsGid = new DataSet();
//            daGid.Fill(dsGid);
//            int g = dsGid.Tables[0].Rows.Count;
//            if (g > 0)
//            {
//                for (int i = 0; i < g; i++)
//                {
//                    objGroup = new Group();
//                    objGroup.groupID = dsGid.Tables[0].Rows[i]["groupId"].ToString();
//                    objGroup.productGroup = dsGid.Tables[0].Rows[i]["MGroup"].ToString();
//                    objGroup.groupName = dsGid.Tables[0].Rows[i]["Company"].ToString();

//                    objGroupList.Add(objGroup);

//                }
//            }


//            con.Close();
//            con.Dispose();
//        }
//        catch (Exception ex)
//        {

//        }

        objGroup = new Group();
        objGroup.groupID = "PRAN-CS-Others";
        objGroup.productGroup = "PRAN-CS-Others";
        objGroup.groupName = "PRAN-CS-Others";
        objGroupList.Add(objGroup);

        objGroup = new Group();
        objGroup.groupID = "RFL";
        objGroup.productGroup = "RFL";
        objGroup.groupName = "RFL";
        objGroupList.Add(objGroup);

        objGroup = new Group();
        objGroup.groupID = "Fridge";
        objGroup.productGroup = "Fridge";
        objGroup.groupName = "Fridge";
        objGroupList.Add(objGroup);

        return objGroupList.ToList();
    }

    [WebMethod]
    public static List<Group> LoadAllZone(string groupID)
    {
        List<Group> objZoneList = new List<Group>();

        Group objZone = null;

        try
        {
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());
            con.Open();
            string group = "";

            if (groupID == "PRAN")
            {
                group = "PRAN-CS-Others";
            }

            string queryZone = @"SELECT ZONE_ID,ZONE_NAME FROM T_MKTG_ZONE WHERE ZONE_ID IN(SELECT ZONE_ID FROM T_MKTG_SR_INFO WHERE GROUP_NAME LIKE '%" + group + "%')";
            OracleCommand cmdZone = new OracleCommand(queryZone, con);
            OracleDataAdapter daZone = new OracleDataAdapter(cmdZone);
            DataSet dsZone = new DataSet();
            daZone.Fill(dsZone);
            int c = dsZone.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    objZone = new Group();

                    objZone.zoneID = dsZone.Tables[0].Rows[i]["ZONE_ID"].ToString();
                    objZone.zoneName = dsZone.Tables[0].Rows[i]["ZONE_NAME"].ToString();

                    objZoneList.Add(objZone);

                }
            }
            con.Close();
            con.Dispose();
        }
        catch (Exception ex)
        {

        }

        return objZoneList.ToList();
    }

    [WebMethod]
    public static string GetZoneWiseSROutlet(string routeId, string category)
    {
        string msg = "";
        OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();

        try
        {
            OracleConnection conn = new OracleConnection(objOracleDB.OracleConnectionString());
            conn.Open();

            string cryteria = "";

            string query = "";
            if (category == "SRWise")
            {
                cryteria = "SR(Opt) :";
                query = @"SELECT SR_ID,SR_NAME FROM T_MKTG_SR_INFO WHERE ZONE_ID IN(SELECT DISTINCT ZONE_ID FROM T_MKTG_ROUTE WHERE ROUTE_ID='" + routeId + "') AND STATUS='Y' ORDER BY SR_NAME";
            }
            else if (category == "OutletWise")
            {
                cryteria = "SR(Olt) :";
                query = @"SELECT OUTLET_ID,OUTLET_NAME FROM T_MKTG_OUTLET WHERE ROUTE_ID='" + routeId + "' AND STATUS='Y' ORDER BY OUTLET_NAME";
            }
            else if (category == "ProductWise")
            {
                query = @"SELECT GROUP_ID,GROUP_NAME,STATUS FROM T_MKTG_GROUP WHERE STATUS='Y' ORDER BY GROUP_ID";
            }
            else if (category == "SalesAssetWise")
            {
                query = @"SELECT GROUP_ID,GROUP_NAME,STATUS FROM T_MKTG_GROUP WHERE STATUS='Y' ORDER BY GROUP_ID";
            }
            else if (category == "Rental")
            {
                query = @"SELECT GROUP_ID,GROUP_NAME,STATUS FROM T_MKTG_GROUP WHERE STATUS='Y' ORDER BY GROUP_ID";
            }
            else if (category == "NonRental")
            {

            }

            if (cryteria != "")
            {
                OracleCommand cmd = new OracleCommand(query, conn);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                int c = ds.Tables[0].Rows.Count;
                if (c > 0)
                {
                    for (int i = 0; i < c; i++)
                    {
                        string GROUP_ID = ds.Tables[0].Rows[i][0].ToString();
                        string GROUP_NAME = ds.Tables[0].Rows[i][1].ToString();

                        msg = msg + ";" + GROUP_ID + ";" + GROUP_NAME;
                    }

                }
                else
                {
                    msg = "Not Exist";
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
    public static List<Group> LoadAllOutlet(string zoneID)
    {
        List<Group> objOutletList = new List<Group>();

        Group objOutlet = null;

        try
        {
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());
            con.Open();

            string queryOutlet = @"SELECT DISTINCT OUTLET_INFO.OUTLET_ID OUTLET_ID,T_OULT.OULT_NAME OUTLET_NAME
                                    FROM OUTLET_INFO 
                                    LEFT JOIN T_OULT ON OUTLET_INFO.OUTLET_ID=T_OULT.OULT_ID WHERE T_OULT.OULT_ZONE='" + zoneID + "'";
            OracleCommand cmdOutlet = new OracleCommand(queryOutlet, con);
            OracleDataAdapter daOutlet = new OracleDataAdapter(cmdOutlet);
            DataSet dsOutlet = new DataSet();
            daOutlet.Fill(dsOutlet);
            int c = dsOutlet.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    objOutlet = new Group();

                    objOutlet.outletID = dsOutlet.Tables[0].Rows[i]["OUTLET_ID"].ToString();
                    objOutlet.outletName = dsOutlet.Tables[0].Rows[i]["OUTLET_NAME"].ToString();

                    objOutletList.Add(objOutlet);

                }
            }
            con.Close();
            con.Dispose();
        }
        catch (Exception ex)
        {

        }

        return objOutletList.ToList();
    }

    [WebMethod]
    public static List<Group> LoadAllSR(string zoneID, string opt, string group)
    {
        List<Group> objSRList = new List<Group>();
        Group objSR = null;

        string id = "";
        string name = "";

        try
        {
            if (group == "PRAN")
            {
                group = "PRAN-CS-Others";
            }

            string querySR = "";
            if (opt == "srwise")
            {
                querySR = @"SELECT SR_ID,SR_NAME FROM T_MKTG_SR_INFO WHERE ZONE_ID='" + zoneID + "' AND GROUP_NAME='" + group + "'";
                id = "SR_ID";
                name = "SR_NAME";
            }
            else if (opt == "outletwise")
            {
                querySR = @"SELECT T2.OUTLET_ID,T2.OUTLET_NAME FROM
                            (SELECT SR_ID,GROUP_NAME FROM T_MKTG_SR_INFO WHERE GROUP_NAME='" + group + "') T1, (SELECT SR_ID,OUTLET_ID,OUTLET_NAME FROM T_MKTG_OUTLET WHERE ZONE_ID='" + zoneID + "') T2 WHERE T1.SR_ID=T2.SR_ID";
                id = "OUTLET_ID";
                name = "OUTLET_NAME";
            }
            else if (opt == "productwise")
            {
                querySR = @"SELECT ITEM_ID,ITEM_NAME FROM T_MKTG_ITEM";

                id = "ITEM_ID";
                name = "ITEM_NAME";
            }
            else if (opt == "rentaltwise")
            {

            }
            else if (opt == "nonrentaltwise")
            {

            }
            else if (opt == "")
            {

            }
            objSRList = getData(querySR, id, name);

        }
        catch (Exception ex)
        {

        }

        return objSRList.ToList();
    }

    public static List<Group> getData(string querySR, string id, string name)
    {
        List<Group> objSRList = new List<Group>();
        Group objSR = null;
        try
        {
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());
            con.Open();

            OracleCommand cmdSR = new OracleCommand(querySR, con);
            OracleDataAdapter daSR = new OracleDataAdapter(cmdSR);
            DataSet dsSR = new DataSet();
            daSR.Fill(dsSR);
            int c = dsSR.Tables[0].Rows.Count;
            if (c > 0)
            {
                for (int i = 0; i < c; i++)
                {
                    objSR = new Group();

                    objSR.outletID = dsSR.Tables[0].Rows[i][id].ToString();
                    objSR.outletName = objSR.outletID + " - " + dsSR.Tables[0].Rows[i][name].ToString();

                    objSRList.Add(objSR);

                }
            }
            con.Close();
            con.Dispose();
        }
        catch (Exception ex) { }

        return objSRList;
    }

    [WebMethod]
    public static List<Group> GetAllOutletInfo(string toDate, string fromDate, string group, string zone, string outlet, string route)
    {
        List<Group> objOutletList = new List<Group>();

        Group objOutlet = null;
        int totalOutlet = 0;

        try
        {
            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());
            con.Open();

            string queryOutlet = "";
//            if (opt == "srwise")
//            {
//                queryOutlet = @"SELECT OUTLET_INFO.ITEM_CODE,OUTLET_INFO.OUTLET_IMAGE,OUTLET_INFO.IMAGE_CAPTURE_DT,OUTLET_INFO.OUTLET_ID, T_OULT.OULT_NAME, T_OULT.OULT_ADDR, OUTLET_INFO.ROUT_SRID, OUTLET_INFO.MARCHANDISE                                  
//                                        FROM OUTLET.OUTLET_INFO, OUTLET.T_DTSR, OUTLET.T_OULT
//                                        WHERE (OUTLET_INFO.OUTLET_ID = T_OULT.OULT_ID) AND (OUTLET_INFO.ROUT_SRID = T_DTSR.DTSR_SRID)
//                                        AND T_DTSR.DTSR_STATUS = 'Y' 
//                                        AND T_DTSR.DTSR_DISTGROUP = '" + group + "' AND T_DTSR.DTSR_ZONE = '" + zone + "'  and trunc(OUTLET_INFO.IMAGE_CAPTURE_DT) BETWEEN TO_DATE('" + fromDate + "','mm-dd-yyyy') and TO_DATE('" + toDate + "','mm-dd-yyyy')";
//                queryOutlet = queryOutlet + @" AND OUTLET_INFO.ROUT_SRID LIKE '%" + outlet + "%' ORDER BY OUTLET_INFO.OUTLET_ID,OUTLET_INFO.IMAGE_CAPTURE_DT";

//            }
//            else if (opt == "outletwise")
//            {
                
//                queryOutlet = @"SELECT OUTLET_INFO.ITEM_CODE,OUTLET_INFO.OUTLET_IMAGE,OUTLET_INFO.IMAGE_CAPTURE_DT,OUTLET_INFO.OUTLET_ID, T_OULT.OULT_NAME, T_OULT.OULT_ADDR, OUTLET_INFO.ROUT_SRID, OUTLET_INFO.MARCHANDISE                                  
//                                 FROM T_OULT,OUTLET_INFO 
//                                 WHERE T_OULT.OULT_ID=OUTLET_INFO.OUTLET_ID
//                                 AND TRUNC(OUTLET_INFO.IMAGE_CAPTURE_DT) BETWEEN TO_DATE('" + fromDate + "','mm-dd-yyyy') and TO_DATE('" + toDate + "','mm-dd-yyyy')";
//                queryOutlet = queryOutlet + @" AND OUTLET_INFO.OUTLET_ID LIKE '%" + outlet + "%' ORDER BY OUTLET_INFO.OUTLET_ID,OUTLET_INFO.IMAGE_CAPTURE_DT";

//            }
//            else if (opt == "productwise")
//            {
                 
//                queryOutlet = @"SELECT OUTLET_INFO.*,T_OULT.OULT_NAME,T_OULT.OULT_ADDR 
//                         FROM 
//                         OUTLET_INFO,T_OULT 
//                         WHERE                         
//                         OUTLET_INFO.OUTLET_ID = T_OULT.OULT_ID ";
//                queryOutlet = queryOutlet + @"  AND trunc(OUTLET_INFO.IMAGE_CAPTURE_DT) BETWEEN TO_DATE('" + fromDate + "','mm-dd-yyyy') and TO_DATE('" + toDate + "','mm-dd-yyyy') ";

//                queryOutlet = queryOutlet + @" AND OUTLET_INFO.ITEM_NAME LIKE '%" + outlet + "%' ORDER BY OUTLET_INFO.OUTLET_ID,OUTLET_INFO.ROUT_SRID, OUTLET_INFO.IMAGE_CAPTURE_DT";


//            }
//            else if (opt == "rentaltwise")
//            {

//            }
//            else if (opt == "nonrentaltwise")
//            {

//            }
//            else if (opt == "")
//            {

//            }

            
            queryOutlet = @"SELECT DISTINCT OUTLET_INFO.OUTLET_ID 
                            FROM SALES.OUTLET_INFO,SALES.T_MKTG_OUTLET
                            WHERE OUTLET_INFO.OUTLET_ID = T_MKTG_OUTLET.OUTLET_ID
                            AND OUTLET_INFO.ROUTE_ID='" + route.Trim() + "' AND OUTLET_INFO.ENTRY_DATE BETWEEN TO_DATE('" + fromDate + "','mm-dd-yyyy') and TO_DATE('" + toDate + "','mm-dd-yyyy') ORDER BY OUTLET_INFO.OUTLET_ID";

            OracleCommand cmdOutlet = new OracleCommand(queryOutlet, con);
            OracleDataAdapter daOutlet = new OracleDataAdapter(cmdOutlet);
            DataSet dsOutlet = new DataSet();
            daOutlet.Fill(dsOutlet);
            int c = dsOutlet.Tables[0].Rows.Count;
            if (c > 0 && dsOutlet.Tables[0].Rows[0]["OUTLET_ID"].ToString() != "")
            {
                for (int i = 0; i < c; i++)
                {
                    string outletID = dsOutlet.Tables[0].Rows[i]["OUTLET_ID"].ToString();

                    string qrPic = @"SELECT OUTLET_INFO.ITEM_CODE,OUTLET_INFO.OUTLET_IMAGE,OUTLET_INFO.IMAGE_CAPTURE_DT,OUTLET_INFO.OUTLET_ID, 
                            T_MKTG_OUTLET.OUTLET_NAME, T_MKTG_OUTLET.OUTLET_ADDRESS, OUTLET_INFO.ROUT_SRID, OUTLET_INFO.MARCHANDISE  
                            FROM SALES.OUTLET_INFO,SALES.T_MKTG_OUTLET
                            WHERE OUTLET_INFO.OUTLET_ID = T_MKTG_OUTLET.OUTLET_ID
                            AND OUTLET_INFO.ROUTE_ID='" + route.Trim() + "' AND OUTLET_INFO.ENTRY_DATE BETWEEN TO_DATE('" + fromDate + "','mm-dd-yyyy') and TO_DATE('" + toDate + "','mm-dd-yyyy') AND OUTLET_INFO.OUTLET_ID='" + outletID + "' ORDER BY OUTLET_INFO.IMAGE_CAPTURE_DT";


                    OracleCommand cmdPic = new OracleCommand(qrPic, con);
                    OracleDataAdapter daPic = new OracleDataAdapter(cmdPic);
                    DataSet dsPic = new DataSet();
                    daPic.Fill(dsPic);
                    int p = dsPic.Tables[0].Rows.Count;

                    if (p > 0 && dsPic.Tables[0].Rows[0]["OUTLET_ID"].ToString() != "")
                    {
                        objOutlet = new Group();
                        totalOutlet++;
                        for (int k = 0; k < p; k++)
                        {
                            objOutlet.itemCode = dsPic.Tables[0].Rows[k]["ITEM_CODE"].ToString();
                            objOutlet.srID = dsPic.Tables[0].Rows[k]["ROUT_SRID"].ToString();
                            objOutlet.outletID = dsPic.Tables[0].Rows[k]["OUTLET_ID"].ToString();
                            objOutlet.outletName = dsPic.Tables[0].Rows[k]["OUTLET_NAME"].ToString();
                            objOutlet.outletAddress = dsPic.Tables[0].Rows[k]["OUTLET_ADDRESS"].ToString();
                            objOutlet.marchandiseOption = dsPic.Tables[0].Rows[k]["MARCHANDISE"].ToString();

                            objOutlet.marchandiseOption = dsPic.Tables[0].Rows[k]["MARCHANDISE"].ToString();
                            string marchandize = dsPic.Tables[0].Rows[k]["MARCHANDISE"].ToString();
                            if (marchandize == "Before")
                            {
                                objOutlet.beforeOutletImage = Convert.ToBase64String((byte[])(dsPic.Tables[0].Rows[k]["OUTLET_IMAGE"]));
                                objOutlet.beforeOutletImageCaptureTime = dsPic.Tables[0].Rows[k]["IMAGE_CAPTURE_DT"].ToString();
                            }
                            else if (marchandize == "After")
                            {
                                objOutlet.afterOutletImage = Convert.ToBase64String((byte[])(dsPic.Tables[0].Rows[k]["OUTLET_IMAGE"]));
                                objOutlet.afterOutletImageCaptureTime = dsPic.Tables[0].Rows[k]["IMAGE_CAPTURE_DT"].ToString();
                            }
                            else if (marchandize == "Others")
                            {
                                objOutlet.othersOutletImage = Convert.ToBase64String((byte[])(dsPic.Tables[0].Rows[k]["OUTLET_IMAGE"]));
                                objOutlet.othersOutletImageCaptureTime = dsPic.Tables[0].Rows[k]["IMAGE_CAPTURE_DT"].ToString();
                            }
                        }

                        objOutlet.totalOutlet = totalOutlet.ToString();
                        objOutletList.Add(objOutlet);
                       
                    }

                }
            }
            
//            queryOutlet = @"SELECT OUTLET_INFO.ITEM_CODE,OUTLET_INFO.OUTLET_IMAGE,OUTLET_INFO.IMAGE_CAPTURE_DT,OUTLET_INFO.OUTLET_ID, 
//                            T_MKTG_OUTLET.OUTLET_NAME, T_MKTG_OUTLET.OUTLET_ADDRESS, OUTLET_INFO.ROUT_SRID, OUTLET_INFO.MARCHANDISE  
//                            FROM SALES.OUTLET_INFO,SALES.T_MKTG_OUTLET
//                            WHERE OUTLET_INFO.OUTLET_ID = T_MKTG_OUTLET.OUTLET_ID
//                            AND OUTLET_INFO.ROUTE_ID IN (SELECT DISTINCT ROUTE_ID FROM T_MKTG_ROUTE WHERE ZONE_ID='" + zone + "') AND OUTLET_INFO.ENTRY_DATE BETWEEN TO_DATE('" + fromDate + "','mm-dd-yyyy') and TO_DATE('" + toDate + "','mm-dd-yyyy') ORDER BY OUTLET_INFO.OUTLET_ID";

//            OracleCommand cmdOutlet = new OracleCommand(queryOutlet, con);
//            OracleDataAdapter daOutlet = new OracleDataAdapter(cmdOutlet);
//            DataSet dsOutlet = new DataSet();
//            daOutlet.Fill(dsOutlet);
//            int c = dsOutlet.Tables[0].Rows.Count;
//            if (c > 0)
            //{

            //    string[] outletArray = new string[c];
            //    string[] itemtArray = new string[c];
            //    string[] imgCaptureTimeArray = new string[c];

            //    var listOfOutlet = new List<string>();

            //    outletArray[0] = dsOutlet.Tables[0].Rows[0]["OUTLET_ID"].ToString();
            //    itemtArray[0] = dsOutlet.Tables[0].Rows[0]["ITEM_CODE"].ToString();
            //    imgCaptureTimeArray[0] = Convert.ToDateTime(dsOutlet.Tables[0].Rows[0]["IMAGE_CAPTURE_DT"].ToString()).ToShortDateString();

            //    listOfOutlet.Add(dsOutlet.Tables[0].Rows[0]["OUTLET_ID"].ToString());


            //    for (int i = 1; i < c; i++)
            //    {
            //        string outletID = dsOutlet.Tables[0].Rows[i]["OUTLET_ID"].ToString();
            //        outletArray[i] = outletID;

            //        string itemCode = dsOutlet.Tables[0].Rows[i]["ITEM_CODE"].ToString();
            //        itemtArray[i] = itemCode;

            //        string imgCaptureTime = Convert.ToDateTime(dsOutlet.Tables[0].Rows[i]["IMAGE_CAPTURE_DT"].ToString()).ToShortDateString();
            //        imgCaptureTimeArray[i] = imgCaptureTime;

            //    }

            //    int h = 0;
            //    int n = 0;
            //    int m = 0;
            //    for (int j = 1; j <= outletArray.Length; j++)
            //    {
            //        if (m > 0)
            //        {
            //            listOfOutlet.Add(outletArray[j - 1].ToString());
            //        }
            //        m = 0;
            //        if (j != outletArray.Length)
            //        {
            //            if (outletArray[j] == outletArray[j - 1] && itemtArray[j] == itemtArray[j - 1] && imgCaptureTimeArray[j] == imgCaptureTimeArray[j - 1])
            //            {
            //                //specificOutletArray[j] = outletArray[j].ToString();
            //                listOfOutlet.Add(outletArray[j].ToString());
            //            }
            //        }
            //        else
            //        {
            //            if (n > 0)
            //            {
            //                h = j;
            //            }
            //            else
            //            {
            //                h = 0;
            //            }

            //            var specificOutletArray = listOfOutlet.ToArray();

            //            if (listOfOutlet.Count == 1)
            //            {
            //                objOutlet = new Group();

            //                objOutlet.itemCode = dsOutlet.Tables[0].Rows[0]["ITEM_CODE"].ToString();
            //                objOutlet.srID = dsOutlet.Tables[0].Rows[0]["ROUT_SRID"].ToString();
            //                objOutlet.outletID = dsOutlet.Tables[0].Rows[0]["OUTLET_ID"].ToString();
            //                objOutlet.outletName = dsOutlet.Tables[0].Rows[0]["OUTLET_NAME"].ToString();
            //                objOutlet.outletAddress = dsOutlet.Tables[0].Rows[0]["OUTLET_ADDRESS"].ToString();
            //                objOutlet.marchandiseOption = dsOutlet.Tables[0].Rows[0]["MARCHANDISE"].ToString();

            //                objOutlet.marchandiseOption = dsOutlet.Tables[0].Rows[0]["MARCHANDISE"].ToString();
            //                string marchandize = dsOutlet.Tables[0].Rows[0]["MARCHANDISE"].ToString();
            //                if (marchandize == "Before")
            //                {
            //                    objOutlet.beforeOutletImage = Convert.ToBase64String((byte[])(dsOutlet.Tables[0].Rows[0]["OUTLET_IMAGE"]));
            //                    objOutlet.beforeOutletImageCaptureTime = dsOutlet.Tables[0].Rows[0]["IMAGE_CAPTURE_DT"].ToString();
            //                }
            //                else if (marchandize == "After")
            //                {
            //                    objOutlet.afterOutletImage = Convert.ToBase64String((byte[])(dsOutlet.Tables[0].Rows[0]["OUTLET_IMAGE"]));
            //                    objOutlet.afterOutletImageCaptureTime = dsOutlet.Tables[0].Rows[0]["IMAGE_CAPTURE_DT"].ToString();
            //                }
            //                else if (marchandize == "Others")
            //                {
            //                    objOutlet.othersOutletImage = Convert.ToBase64String((byte[])(dsOutlet.Tables[0].Rows[0]["OUTLET_IMAGE"]));
            //                    objOutlet.othersOutletImageCaptureTime = dsOutlet.Tables[0].Rows[0]["IMAGE_CAPTURE_DT"].ToString();
            //                }

            //                objOutletList.Add(objOutlet);
            //            }
            //            else if (listOfOutlet.Count == 2)
            //            {
            //                for (int k = 0; k < listOfOutlet.Count; k++)
            //                {
            //                    objOutlet = new Group();
            //                    objOutlet.itemCode = dsOutlet.Tables[0].Rows[k]["ITEM_CODE"].ToString();
            //                    objOutlet.srID = dsOutlet.Tables[0].Rows[k]["ROUT_SRID"].ToString();
            //                    objOutlet.outletID = dsOutlet.Tables[0].Rows[k]["OUTLET_ID"].ToString();
            //                    objOutlet.outletName = dsOutlet.Tables[0].Rows[k]["OUTLET_NAME"].ToString();
            //                    objOutlet.outletAddress = dsOutlet.Tables[0].Rows[k]["OUTLET_ADDR"].ToString();
            //                    objOutlet.marchandiseOption = dsOutlet.Tables[0].Rows[k]["MARCHANDISE"].ToString();

            //                    objOutlet.marchandiseOption = dsOutlet.Tables[0].Rows[k]["MARCHANDISE"].ToString();
            //                    string marchandize = dsOutlet.Tables[0].Rows[k]["MARCHANDISE"].ToString();
            //                    if (marchandize == "Before")
            //                    {
            //                        objOutlet.beforeOutletImage = Convert.ToBase64String((byte[])(dsOutlet.Tables[0].Rows[k]["OUTLET_IMAGE"]));
            //                        objOutlet.beforeOutletImageCaptureTime = dsOutlet.Tables[0].Rows[k]["IMAGE_CAPTURE_DT"].ToString();
            //                    }
            //                    else if (marchandize == "After")
            //                    {
            //                        objOutlet.afterOutletImage = Convert.ToBase64String((byte[])(dsOutlet.Tables[0].Rows[k]["OUTLET_IMAGE"]));
            //                        objOutlet.afterOutletImageCaptureTime = dsOutlet.Tables[0].Rows[k]["IMAGE_CAPTURE_DT"].ToString();
            //                    }
            //                    else if (marchandize == "Others")
            //                    {
            //                        objOutlet.othersOutletImage = Convert.ToBase64String((byte[])(dsOutlet.Tables[0].Rows[k]["OUTLET_IMAGE"]));
            //                        objOutlet.othersOutletImageCaptureTime = dsOutlet.Tables[0].Rows[k]["IMAGE_CAPTURE_DT"].ToString();
            //                    }
            //                }

            //                objOutletList.Add(objOutlet);
            //            }
            //            else
            //            {
            //                int count = 0;
            //                int b = 0;
            //                int a = 0;
            //                int e = 0;
            //                for (int k = 0; k < listOfOutlet.Count; k++)
            //                {
            //                    if (count == 0)
            //                    {
            //                        objOutlet = new Group();
            //                    }
            //                    objOutlet.itemCode = dsOutlet.Tables[0].Rows[h]["ITEM_CODE"].ToString();
            //                    objOutlet.srID = dsOutlet.Tables[0].Rows[h]["ROUT_SRID"].ToString();
            //                    objOutlet.outletID = dsOutlet.Tables[0].Rows[h]["OUTLET_ID"].ToString();
            //                    objOutlet.outletName = dsOutlet.Tables[0].Rows[h]["OUTLET_NAME"].ToString();
            //                    objOutlet.outletAddress = dsOutlet.Tables[0].Rows[h]["OUTLET_ADDRESS"].ToString();
            //                    objOutlet.marchandiseOption = dsOutlet.Tables[0].Rows[h]["MARCHANDISE"].ToString();

            //                    objOutlet.marchandiseOption = dsOutlet.Tables[0].Rows[h]["MARCHANDISE"].ToString();
            //                    string marchandize = dsOutlet.Tables[0].Rows[h]["MARCHANDISE"].ToString();
            //                    if (marchandize == "Before")
            //                    {
            //                        objOutlet.beforeOutletImage = Convert.ToBase64String((byte[])(dsOutlet.Tables[0].Rows[h]["OUTLET_IMAGE"]));
            //                        objOutlet.beforeOutletImageCaptureTime = dsOutlet.Tables[0].Rows[h]["IMAGE_CAPTURE_DT"].ToString();

            //                    }
            //                    else if (marchandize == "After")
            //                    {

            //                        objOutlet.afterOutletImage = Convert.ToBase64String((byte[])(dsOutlet.Tables[0].Rows[h]["OUTLET_IMAGE"]));
            //                        objOutlet.afterOutletImageCaptureTime = dsOutlet.Tables[0].Rows[h]["IMAGE_CAPTURE_DT"].ToString();

            //                    }
            //                    else if (marchandize == "Others")
            //                    {

            //                        objOutlet.othersOutletImage = Convert.ToBase64String((byte[])(dsOutlet.Tables[0].Rows[h]["OUTLET_IMAGE"]));
            //                        objOutlet.othersOutletImageCaptureTime = dsOutlet.Tables[0].Rows[h]["IMAGE_CAPTURE_DT"].ToString();

            //                    }

            //                    count = 1;
            //                    h++;



            //                }

            //                objOutletList.Add(objOutlet);
            //            }
            //            h = listOfOutlet.Count;
            //            listOfOutlet.Clear();
            //            n = 1;
            //            m = 1;
            //        }
            //    }

            //}
            con.Close();
            con.Dispose();
        }
        catch (Exception ex)
        {

        }

        return objOutletList.ToList();
    }

    [WebMethod]
    public static void ShowOutletPicture(string outletID)
    {
        try
        {
            string[] outletInfo = outletID.Split('-');


            OracleDBConnectionClass objOracleDB = new OracleDBConnectionClass();
            OracleConnection con = new OracleConnection(objOracleDB.OracleConnectionString());
            con.Open();

            string queryOutletInfo = "SELECT OUTLET_IMAGE FROM OUTLET_INFO" +
                                    " WHERE OUTLET_ID= '" + outletInfo[0] + "'" +
                                    " AND MARCHANDISE='" + outletInfo[3] + "'" +
                                    " AND ROUT_SRID='" + outletInfo[2] + "' ORDER BY IMAGE_CAPTURE_DT ASC";
            OracleCommand cmdOutletInfo = new OracleCommand(queryOutletInfo, con);
            OracleDataAdapter daOutletInfo = new OracleDataAdapter(cmdOutletInfo);
            DataSet dsOutletInfo = new DataSet();
            daOutletInfo.Fill(dsOutletInfo);
            int c = dsOutletInfo.Tables[0].Rows.Count;
            if (c > 0)
            {
                byte[] binData = (byte[])dsOutletInfo.Tables[0].Rows[0]["OUTLET_IMAGE"];
                //ViewImage(binData);

                //image.src = "data:image/png;base64," + Convert.ToBase64String(binData);
            }
            con.Close();
            con.Dispose();

        }
        catch (Exception ex)
        {

        }
    }
}