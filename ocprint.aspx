﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ocprint.aspx.cs" Inherits="ocprint" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
       <title>Sales-Mate</title>
    <link rel="shortcut icon" href="Nimages/logopran.gif" />
    <link href="Ncss/jquery-ui.css" rel="stylesheet" />    
   <%-- <link href="Ncss/bootstrap-min2.css" rel="stylesheet" type="text/css"/>--%>



    <link rel="stylesheet" type="text/css" href="CSS/basic.css" />
    <link rel="stylesheet" type="text/css" href="CSS/lightbox.css" />
    <link rel="stylesheet" type="text/css" href="CSS/Format.css" />
    <link rel="stylesheet" type="text/css" href="CSS/styles.css" />
    <link rel="stylesheet" type="text/css" href="CSS/demo.css" />
    <link rel="stylesheet" type="text/css" href="CSS/sidemenu.css" />
    
    <script src="NScripts/jquery-2.1.4.min.js" type="text/javascript"></script>
    <script src="NScripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="NJavaScript/jquery.min.js" type="text/javascript"></script>
    <script src="NJavaScript/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="NJavaScript/jquery-ui.js" type="text/javascript"></script>
    <script src="NScripts/script.js" type="text/javascript"></script>    
  
    <!-- jQuery -->
    <script type="text/javascript" src="JS/slide-show-google-api.js"></script>



    <link href="NgridCss/dataTables.jqueryui.min.css" rel="stylesheet" />    
    <link href="NgridCss/buttons.jqueryui.min.css" rel="stylesheet" />
  
    <script src="NgridJs/jquery-1.12.0.min.js" type="text/javascript"></script>  
    <script src="NgridJs/jquery.dataTables.min.js" type="text/javascript"></script>  
    <script src="NgridJs/dataTables.jqueryui.min.js" type="text/javascript"></script>     
    <script src="NgridJs/dataTables.buttons.min.js" type="text/javascript"></script>     
    <script src="NgridJs/buttons.jqueryui.min.js" type="text/javascript"></script>    
    <script src="NgridJs/jszip.min.js" type="text/javascript"></script>    
    <script src="NgridJs/pdfmake.min.js" type="text/javascript"></script>
    <script src="NgridJs/vfs_fonts.js" type="text/javascript"></script>    
    <script src="NgridJs/buttons.html5.min.js" type="text/javascript"></script>    
    <script src="NgridJs/buttons.print.min.js" type="text/javascript"></script>



    <!-- FlexSlider -->    
    <script type="text/javascript" src="JS/jquery.flexslider-min.js"></script>
    <script type="text/javascript" src="JS/prototype.js"></script>
    <script type="text/javascript" src="JS/lightbox.js"></script>  
    
   
   
   
    <style type="text/css">
        
        .plc::-webkit-input-placeholder
        {
            color:#b2cde0;
        }
        
        .btn-success {
            background-color: #5cb85c;
            border-color: #4cae4c;
            color: #fff;
            
        }
        .boxbg5
        {
            background-color: #FFF;
            padding: 1px;
            height: 50px;
            color: #ffffff;
            text-align: center;
            font-size: 14px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24);
            border-radius: 5px;
            margin-top: -30px;
        }
        .btn {
            -moz-user-select: none;
            background-image: none;
            border: 1px solid transparent;
            border-radius: 4px;
            cursor: pointer;
            display: inline-block;
            font-size: 14px;
            font-weight: 400;
            line-height: 1.42857;
            margin-bottom: 0;
            padding: 6px 12px;
            text-align: center;
            vertical-align: middle;
            white-space: nowrap;
        }
        ul {
            list-style-type: none;
            margin: 0;
            padding: 0;
            overflow: hidden;
            background-color: #607d8b;
        }

        ul li {
            float: left;
            border-right:1px solid #bbb;
        }

        ul li:last-child {
            border-right: none;
        }

        ul li a {
            display: block;
            color: white;
            text-align: center;
            padding: 14px 16px;
            font-size: 14px;
            text-decoration: none;
        }

        ul li a:hover:not(.active) {
            background-color: #8dc060;
        }

        .active {
            background-color: #8dc060;
        }
        
        #content {
	        float: left;
            padding-top: 10px;
            padding-left: 5px;
            width: 95.5%;
        }

        #middle {
	        width: 580px; /* Account for margins + border values */
	        float: left;
	        padding: 5px;	        
        }

        #sidebar {
	        width: 97%;
	        padding-top: 10px;
	        float: right;
            padding-right: 5px;
        }
        #pagewrap {
	        padding-top: 5px;
	        width: 1135px;
	        margin: -14px auto;
        }
        
        .border-bottom
        {
            border-bottom: 1px solid #bbb;
        }
        
        .title{
            display: inline-block;
            height: 25px;
        }
        
        .txtColor
        {
            border:1px solid #5cb85c;       
            height:23px;      
        }
        
     </style>



</head>
<body>
    <form id="form1" runat="server">
    <div>
     <div style="width:935px; height:auto;float:left;margin-left:200px;">
               <div id="dvMiddleContent" style="border:1px solid #8dc060;width:99.2%; height:auto;float:left;padding-bottom:10px;">
                    <div style='width:70%;height:36px;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:25px;padding-top:10px;margin-top:10px;margin-left:15%;'><span>Outlet Wise OC Print</span></div> 
                                  <div style='width:69.8%;height:auto;margin-top: 2px;border:1px solid #8dc060;padding-top:10px;margin-left:15%;'> 
                                   <table style='padding-top:10px;padding-bottom: 10px;margin-left:150px;'> 
                                        <tr><td>Order Date :</td><td><asp:TextBox ID="txtOrderDate" runat="server" style='border:1px solid #8dc060;width:200px;height:25px;'></asp:TextBox></td><td><span style='color:#ec407a;'>* DD/MM/YYYY</span></td></tr> 
                                        <tr><td>Country Name :</td><td>
                                            <asp:DropDownList ID="ddlCountry" runat="server" style='height:28px;width:204px;border:1px solid #8dc060;'></asp:DropDownList>
                                            </td><td><span style='color:#ec407a;'>*</span></td></tr> 

                                       <tr><td>Division Name :</td><td>
                                           <asp:DropDownList ID="ddlDivision" runat="server" style='height:28px;width:204px;border:1px solid #8dc060;'></asp:DropDownList>
                                            </td><td><span style='color:#ec407a;'>*</span></td></tr> 

                                       <tr><td>Zone :</td><td>
                                           <asp:DropDownList ID="ddlZone" runat="server" style='height:28px;width:204px;border:1px solid #8dc060;'></asp:DropDownList>
                                                          </td><td><span style='color:#ec407a;'>*</span></td></tr> 

                                       <tr><td>Route :</td><td>
                                           <asp:DropDownList ID="ddlRoute" runat="server" style='height:28px;width:204px;border:1px solid #8dc060;'></asp:DropDownList>
                                                           </td><td><span style='color:#ec407a;'>*</span></td></tr> 

                                       <tr><td>Outlet :</td><td>
                                           <asp:DropDownList ID="ddlOutlet" runat="server" style='height:28px;width:204px;border:1px solid #8dc060;'></asp:DropDownList>
                                                           </td><td><span style='color:#ec407a;'>*</span></td></tr> 

                                       <tr><td></td><td style='text-align:right;'> 
                                           
                                           <asp:Button ID="btnShow" runat="server" Text="Show" style='background-color: #8dc060;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 6px 12px;text-align: center;vertical-align: middle;white-space: nowrap;' OnClick="btnShow_Click" />
                                                    </td></tr> 
                                     </table>

                                  </div>
               </div>
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <rsweb:ReportViewer ID="ReportViewer1" Width="930" Height="500" ShowPrintButton="true" runat="server"></rsweb:ReportViewer>
                    
               </div>         
    </div>
    </form>
</body>
</html>
