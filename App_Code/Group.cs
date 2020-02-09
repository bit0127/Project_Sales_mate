using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Group
/// </summary>
public class Group
{
	 
        //--Group information----------------
        public string groupID { get; set; }
        public string productGroup { get; set; }
        public string groupName { get; set; }

        //--Outlet information---------------
        public string itemCode { get; set; }
        public string srID { get; set; }        
        public string outletID { get; set; }
        public string outletName { get; set; }
        public string outletAddress { get; set; }
        
        public string imageCaptureDate { get; set; }
        public string marchandiseOption { get; set; }
        
        public string beforeOutletImage { get; set; }
        public string afterOutletImage { get; set; }
        public string othersOutletImage { get; set; }
        
        public byte[] outletImage { get; set; }

        public string beforeOutletImageCaptureTime { get; set; }
        public string afterOutletImageCaptureTime { get; set; }
        public string othersOutletImageCaptureTime { get; set; }
        public string totalOutlet { get; set; }

        public string base64 { get; set; }
   
        //--Zone information----------------------
        public string zoneID { get; set; }
        public string zoneName { get; set; }

        //---ITEM Information
        public string ITEM_SHORTNAME { get; set; }
        public string ITEM_OUTPRICE { get; set; }
        public string STATUS { get; set; }    
  
        //---Trade programs----------------
        public string ItemName { get; set; }
        public string Piece { get; set; }
        public string Free { get; set; }
        public string Discount { get; set; }
        public string Gift { get; set; }


        public string ItemClass { get; set; }
        public string ItemNumber { get; set; }

        

    public class RootObject
    {
        public List<Group> items { get; set; }
    }
}