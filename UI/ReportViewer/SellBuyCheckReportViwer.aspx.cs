using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UI_ReportViewer_StockDeclarationBeforePostedReportViewer : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    private ReportDocument rdoc = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../../Default.aspx");
        }

        
        DataTable dtReprtSource = new DataTable();
        StringBuilder sbMst = new StringBuilder();
        StringBuilder sbfilter = new StringBuilder();


        string Fromdate = "";
        string Todate = "";
        string fundCode = "";
        string companycode = "";
        string transtype = "";
        string a = "";

        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../../Default.aspx");
        }
        else
        {
            Fromdate = (string)Session["Fromdate"];
            Todate = (string)Session["Todate"];
            fundCode = (string)Session["fundCodes"];
            companycode = (string)Session["companycode"];
            transtype = (string)Session["transtype"];

        }

        if (fundCode == "0" && companycode == "0" && transtype == "0")
        {
           
            a = "All";
        }
        else if (fundCode != "0" && companycode == "0" && transtype == "0")
        {
            a = "" + fundCode;
        }
        else if (fundCode == "0" && companycode != "0" && transtype == "0")
        {
            a = "" + companycode;
        }
        else if (fundCode != "0" && companycode == "0" && transtype != "0")
        {
            a = "" + fundCode + "" + transtype;
        }




        sbMst.Append(sbfilter.ToString());


    }
}