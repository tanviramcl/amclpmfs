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
        sbfilter.Append(" ");


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
            sbMst.Append("select t.VCH_DT, t.F_CD  ,f.f_name,t.COMP_CD , c.comp_nm,c.comp_nm  || '('|| t.COMP_CD|| ')',t.TRAN_TP , decode ( t.TRAN_TP, 'C', 'Cost','S','Sell','B','Bonus','R','Right','P','IPO') tran_type,");
            sbMst.Append(" t.VCH_NO, t.NO_SHARE, t.RATE ,t.COST_RATE, t.CRT_AFT_COM , t.AMOUNT , t.AMT_AFT_COM,t.AMT_AFT_COM/t.NO_SHARE avg_rate,t.STOCK_EX ,decode(t.STOCK_EX,'D','DSE','C','CSE',' ALL') stock_name,");
            sbMst.Append(" t.OP_NAME   from fund_trans_hb t,comp c , fund f where vch_dt between '"+Fromdate+"' and '"+Todate+"' and c.comp_cd=t.comp_cd and f.f_cd=t.f_cd order by t.f_cd, tran_tp,t.VCH_DT");
            sbMst.Append(sbfilter.ToString());
            dtReprtSource = commonGatewayObj.Select(sbMst.ToString());
            dtReprtSource.TableName = "SellBuyCheckReport";
            dtReprtSource.WriteXmlSchema(@"D:\officialProject\4-5-2017\amclpmfs\UI\ReportViewer\Report\crtCapitalGainCompanyWiseReport.xsd");
            if (dtReprtSource.Rows.Count > 0)
            {

                string Path = Server.MapPath("Report/CR_SellBuyCheckReport.rpt");
                rdoc.Load(Path);
                rdoc.SetDataSource(dtReprtSource);
                CR_SellBuyCheckReport.ReportSource = rdoc;
                CR_SellBuyCheckReport.DisplayToolbar = true;
                CR_SellBuyCheckReport.HasExportButton = true;
                CR_SellBuyCheckReport.HasPrintButton = true;
                rdoc.SetParameterValue("Fromdate", Fromdate);
                rdoc.SetParameterValue("Todate", Todate);
                rdoc = ReportFactory.GetReport(rdoc.GetType());

            }
            else
            {
                Response.Write("No Data Found");
            }
           
        }
        else if (fundCode != "0" && companycode == "0" && transtype == "0")
        {
            //a = "" + fundCode;
        }
        else if (fundCode == "0" && companycode != "0" && transtype == "0")
        {
            //a = "" + companycode;
        }
        else if (fundCode != "0" && companycode == "0" && transtype != "0")
        {
           // a = "" + fundCode + "" + transtype;
        }




        sbMst.Append(sbfilter.ToString());


    }
}