﻿using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UI_ReportViewer_NonDemateSharesCheckReportViwer : System.Web.UI.Page
{

    CommonGateway commonGatewayObj = new CommonGateway();
    private ReportDocument rdoc = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {

        string Fromdate = "";
        string Todate = "";
        string fundCode = "";
        string statementType = "";
        string fundName = "";

        DataTable dtReprtSource = new DataTable();
        StringBuilder sbMst = new StringBuilder();
        StringBuilder sbfilter = new StringBuilder();
        sbfilter.Append(" ");


        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../../Default.aspx");
        }
        else {
            Fromdate = (string)Session["Fromdate"];
            Todate = (string)Session["Todate"];
            fundCode = (string)Session["fundCodes"];
            statementType = (string)Session["statementType"];
            fundName = (string)Session["fundName"];

        }


        if (string.Compare(statementType, "DateWise", true) == 0)
        {
            sbMst.Append("SELECT   VCH_DT, SUM(decode(TRAN_TP,'S',AMT_AFT_COM,0)) AS SALE_PRICE, SUM(decode(TRAN_TP,'C',AMT_AFT_COM,0)) AS BUY_PRICE, ");
            sbMst.Append(" sum(decode(TRAN_TP,'S',CRT_AFT_COM*NO_SHARE,0)) as COST_PRICE, SUM(decode(TRAN_TP,'S',AMT_AFT_COM,0))-sum(decode(TRAN_TP,'S',CRT_AFT_COM*NO_SHARE,0) )as capital_Gain ");
            sbMst.Append(" FROM   FUND_TRANS_HB WHERE       (FUND_TRANS_HB.VCH_DT BETWEEN '"+Fromdate+"' AND '"+Todate+"') and f_Cd='"+fundCode+"' and stock_ex in ('C','D') GROUP BY VCH_DT ");
            sbMst.Append(sbfilter.ToString());
            dtReprtSource = commonGatewayObj.Select(sbMst.ToString());
            dtReprtSource.TableName = "StatementOfProfitOnSaleOfInvestmentDateWise";
            dtReprtSource.WriteXmlSchema(@"D:\officialProject\4-5-2017\amclpmfs\UI\ReportViewer\Report\CR_StatementOfProfitOnSaleOfInvestmentDateWiseReportVeiwer.xsd");
            if (dtReprtSource.Rows.Count > 0)
            {

                string Path = Server.MapPath("Report/CR_StatementOfProfitOnSaleOfInvestmentDateWiseReport.rpt");
                rdoc.Load(Path);
                rdoc.SetDataSource(dtReprtSource);
                CR_StatementOfProfitOnSaleOfInvestmentReportVeiwer.ReportSource = rdoc;
                CR_StatementOfProfitOnSaleOfInvestmentReportVeiwer.DisplayToolbar = true;
                CR_StatementOfProfitOnSaleOfInvestmentReportVeiwer.HasExportButton = true;
                CR_StatementOfProfitOnSaleOfInvestmentReportVeiwer.HasPrintButton = true;
                rdoc.SetParameterValue("Fromdate", Fromdate);
                rdoc.SetParameterValue("Todate", Todate);
                rdoc.SetParameterValue("prmFundName", fundName);

                rdoc = ReportFactory.GetReport(rdoc.GetType());

            }
            else
            {
                Response.Write("No Data Found");
            }

        }
        else if (string.Compare(statementType, "CompanyWise", true) == 0)
        {
            sbMst.Append("SELECT   COMP.COMP_NM,sum(NO_SHARE), SUM(decode(TRAN_TP,'S',AMT_AFT_COM,0)) AS SALE_PRICE, sum(decode(TRAN_TP,'S',CRT_AFT_COM*NO_SHARE,0)) as COST_PRICE, SUM(decode(TRAN_TP,'S', ");
            sbMst.Append(" AMT_AFT_COM,0))-sum(decode(TRAN_TP,'S',CRT_AFT_COM*NO_SHARE,0) )as capital_Gain FROM  FUND_TRANS_HB INNER JOIN COMP ON FUND_TRANS_HB.COMP_CD = COMP.COMP_CD");
            sbMst.Append(" WHERE (FUND_TRANS_HB.VCH_DT BETWEEN '"+Fromdate+"' AND '"+Todate+"') and f_Cd='"+fundCode+"' and stock_ex in('D','C') GROUP BY COMP.COMP_NM");
            sbMst.Append(sbfilter.ToString());
            dtReprtSource = commonGatewayObj.Select(sbMst.ToString());
            dtReprtSource.TableName = "StatementOfProfitOnSaleOfInvestmentCompanyWise";
            dtReprtSource.WriteXmlSchema(@"D:\officialProject\4-5-2017\amclpmfs\UI\ReportViewer\Report\CR_StatementOfProfitOnSaleOfInvestmentCompanyWiseReportVeiwer.xsd");
            if (dtReprtSource.Rows.Count > 0)
            {

                string Path = Server.MapPath("Report/CR_StatementOfProfitOnSaleOfInvestmentCompanyWiseWiseReport.rpt");
                rdoc.Load(Path);
                rdoc.SetDataSource(dtReprtSource);
                CR_StatementOfProfitOnSaleOfInvestmentReportVeiwer.ReportSource = rdoc;
                CR_StatementOfProfitOnSaleOfInvestmentReportVeiwer.DisplayToolbar = true;
                CR_StatementOfProfitOnSaleOfInvestmentReportVeiwer.HasExportButton = true;
                CR_StatementOfProfitOnSaleOfInvestmentReportVeiwer.HasPrintButton = true;
                rdoc.SetParameterValue("Fromdate", Fromdate);
                rdoc.SetParameterValue("Todate", Todate);
                rdoc.SetParameterValue("prmFundName", fundName);

                rdoc = ReportFactory.GetReport(rdoc.GetType());

            }
            else
            {
                Response.Write("No Data Found");
            }


        }

    }

       

       
      
    
}