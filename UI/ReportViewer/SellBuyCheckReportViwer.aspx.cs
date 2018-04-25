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
      

        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../../Default.aspx");
        }
        else
        {
            //Fromdate = (string)Session["Fromdate"];
            //Todate = (string)Session["Todate"];
            //fundCode = (string)Session["fundCodes"];
            //companycode = (string)Session["companycode"];
            //transtype = (string)Session["transtype"];

             Fromdate = Convert.ToString(Request.QueryString["p1date"]).Trim();
             Todate = Convert.ToString(Request.QueryString["p2date"]).Trim();
            fundCode = Convert.ToString(Request.QueryString["fundcode"]).Trim();
            companycode = Convert.ToString(Request.QueryString["companycode"]).Trim();
            transtype = Convert.ToString(Request.QueryString["transtype"]).Trim();

        }

        if (fundCode == "0" && companycode == "0" && transtype == "0")
        {
            sbMst.Append("select  t.F_CD as f_cd,f.f_name,t.COMP_CD , c.comp_nm,c.comp_nm  || '('|| t.COMP_CD|| ')',t.TRAN_TP , t.VCH_DT, decode ( t.TRAN_TP, 'C', 'Cost','S','Sell','B','Bonus','R','Right','P','IPO','I','IPO') tran_type,");
            sbMst.Append(" t.VCH_NO, t.NO_SHARE, t.RATE ,t.COST_RATE, t.CRT_AFT_COM , t.AMOUNT , t.AMT_AFT_COM,ROUND(t.AMT_AFT_COM/t.NO_SHARE ,2)avg_rate,t.STOCK_EX ,decode(t.STOCK_EX,'D','DSE','C','CSE',' ALL') stock_name,t.OP_NAME   from fund_trans_hb t,comp c , fund f");
            sbMst.Append(" where vch_dt between '" + Fromdate + "' and '" + Todate + "' and c.comp_cd=t.comp_cd and f.f_cd=t.f_cd order by t.F_CD,t.VCH_DT DESC");
            sbMst.Append(sbfilter.ToString());
            dtReprtSource = commonGatewayObj.Select(sbMst.ToString());
            dtReprtSource.TableName = "SellBuyCheckReport";
            dtReprtSource.WriteXmlSchema(@"D:\IAMCL_10-7-17\amclpmfs\amclpmfs\UI\ReportViewer\Report\CR_SellBuyCheckReport.xsd");
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
            sbMst.Append("select t.VCH_DT, t.F_CD  , f.f_name fund_name,t.COMP_CD , c.comp_nm,c.comp_nm  || '('|| t.COMP_CD|| ')',t.TRAN_TP , decode ( t.TRAN_TP, 'C', 'Cost','S','Sell','B','Bonus','R','Right','I','IPO',' ') tran_type,");
            sbMst.Append(" t.VCH_NO, t.NO_SHARE, t.RATE ,t.COST_RATE, t.CRT_AFT_COM , t.AMOUNT , t.AMT_AFT_COM,ROUND(t.AMT_AFT_COM/t.NO_SHARE,2) as avg_rate,t.STOCK_EX ,");
            sbMst.Append(" decode(t.STOCK_EX,'D','DSE','C','CSE','A','ALL',' ') stock_name, t.OP_NAME   from fund_trans_hb t,comp c , fund f where vch_dt between '" + Fromdate + "' and '" + Todate + "' and c.comp_cd=t.comp_cd and t.f_cd='" + fundCode + "' and t.f_cd=f.f_cd order by t.f_cd,t.VCH_DT");
            sbMst.Append(sbfilter.ToString());
            dtReprtSource = commonGatewayObj.Select(sbMst.ToString());
            dtReprtSource.TableName = "SellBuyCheckReportfundwise";
            // dtReprtSource.WriteXmlSchema(@"D:\officialProject\4-5-2017\amclpmfs\UI\ReportViewer\Report\CR_SellBuyCheckReportfundwise.xsd");
            if (dtReprtSource.Rows.Count > 0)
            {
                string Path = Server.MapPath("Report/CR_SellBuyCheckReportfundwise.rpt");
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
        else if (fundCode == "0" || fundCode != "0" && companycode != "0" && transtype == "0")
        {
            sbMst.Append("select t.VCH_DT, t.F_CD  ,  f.f_name fund_name, t.COMP_CD , c.comp_nm, c.comp_nm  || '('|| t.COMP_CD|| ')',t.TRAN_TP , decode ( t.TRAN_TP, 'C', 'Cost','S','Sale','B','Bonus','I','IPO','R','Right','D','Split',' ') tran_type,");
            sbMst.Append(" t.VCH_NO, t.NO_SHARE, t.RATE ,t.COST_RATE, t.CRT_AFT_COM , t.AMOUNT , t.AMT_AFT_COM,ROUND(t.AMT_AFT_COM/t.NO_SHARE,2) as avg_rate,t.STOCK_EX ,decode(t.STOCK_EX,'D','DSE','C','CSE',' ') stock_name,t.OP_NAME ");
            sbMst.Append(" from fund_trans_hb t,comp c, fund f where vch_dt between '" + Fromdate + "' and '" + Todate + "' and c.comp_cd=t.comp_cd and c.comp_cd='" + companycode + "' and t.NO_SHARE>=1 and f.f_cd=t.f_cd order by t.f_cd, tran_tp,t.VCH_DT");
            sbMst.Append(sbfilter.ToString());
            dtReprtSource = commonGatewayObj.Select(sbMst.ToString());
            dtReprtSource.TableName = "CR_SellBuyCheckReportcompanywise";
          //  dtReprtSource.WriteXmlSchema(@"D:\officialProject\4-5-2017\amclpmfs\UI\ReportViewer\Report\CR_SellBuyCheckReportCompanywise2.xsd");
            if (dtReprtSource.Rows.Count > 0)
            {
                string Path = Server.MapPath("Report/CR_SellBuyCheckReportcompanywise.rpt");
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
        else if (fundCode != "0" && companycode == "0" && transtype != "0")
        {
            sbMst.Append("SELECT  f.F_CD, f.comp_cd, c.comp_nm, decode(f.f_cd, 1,'ICB Asset Management Company Ltd.',2, 'ICB AMCL Unit Fund',3, 'ICB AMCL First Mutual Fund', 4,'ICB AMCL Pension Holders'' Unit Fund',5,'ICB AMCL Islamic Mutual Fund',6, 'ICB AMCL First NRB Mutual Fund') fund_name,");
            sbMst.Append(" SUM(AMT_AFT_COM) as purchase, sum(no_share) as No_Of_Share   FROM FUND_TRANS_HB f, comp c WHERE F.TRAN_TP= '" + transtype + "' AND VCH_DT BETWEEN '" + Fromdate + "' AND '" + Todate + "' ");
            sbMst.Append(" and f_cd='" + fundCode + "' and c.comp_cd=f.comp_cd GROUP BY f.F_CD,f.comp_cd ,c.comp_nm");
            sbMst.Append(sbfilter.ToString());
            dtReprtSource = commonGatewayObj.Select(sbMst.ToString());
            dtReprtSource.TableName = "SellBuyCheckReportcompanywiseALL";
         //   dtReprtSource.WriteXmlSchema(@"D:\officialProject\4-5-2017\amclpmfs\UI\ReportViewer\Report\CR_SellBuyCheckReportcompanywiseALL.xsd");
            if (dtReprtSource.Rows.Count > 0)
            {
                string Path = Server.MapPath("Report/CR_SellBuyCheckReportcompanywiseALL.rpt");
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
        else
        {
            Response.Write("No Data Found");
        }


    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        CR_SellBuyCheckReport.Dispose();
        CR_SellBuyCheckReport = null;
        rdoc.Close();
        rdoc.Dispose();
        rdoc = null;
        GC.Collect();
    }

}