﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using CrystalDecisions.CrystalReports.Engine;

public partial class UI_ReportViewer_PortfolioWithNonListedReportViewer : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    Pf1s1DAO pf1Obj = new Pf1s1DAO();
    private ReportDocument rdoc = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder sbFilter = new StringBuilder();
        string fundCode = "";
        string balDate = "";
        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../../Default.aspx");
        }
        else
        {
            fundCode = (string)Session["fundCode"];
            balDate = (string)Session["balDate"];
        }

        DataTable dtReprtSource = new DataTable();
        StringBuilder sbMst = new StringBuilder();
        StringBuilder sbfilter = new StringBuilder();
        sbfilter.Append(" ");
        sbMst.Append("SELECT     INVEST.FUND.F_NAME, INVEST.COMP.COMP_NM, INVEST.PFOLIO_BK.SECT_MAJ_NM,INVEST.PFOLIO_BK.SECT_MAJ_CD, TRUNC(INVEST.PFOLIO_BK.TOT_NOS,0) AS TOT_NOS, ");
        sbMst.Append("ROUND(INVEST.PFOLIO_BK.TCST_AFT_COM / INVEST.PFOLIO_BK.TOT_NOS, 2) AS COST_RT_PER_SHARE, INVEST.PFOLIO_BK.TCST_AFT_COM, ");
        sbMst.Append("NVL(INVEST.PFOLIO_BK.DSE_RT, 0) AS DSE_RT, NVL(INVEST.PFOLIO_BK.CSE_RT, 0) AS CSE_RT, ROUND(INVEST.PFOLIO_BK.ADC_RT, 2) ");
        sbMst.Append("AS AVG_RATE, ROUND(INVEST.PFOLIO_BK.TOT_NOS * INVEST.PFOLIO_BK.ADC_RT, 2) AS TOT_MARKET_PRICE, ");
        sbMst.Append("ROUND(ROUND(INVEST.PFOLIO_BK.ADC_RT, 2) - ROUND(INVEST.PFOLIO_BK.TCST_AFT_COM / INVEST.PFOLIO_BK.TOT_NOS, 2), 2) AS RATE_DIFF, ");
        sbMst.Append("ROUND(ROUND(INVEST.PFOLIO_BK.TOT_NOS * INVEST.PFOLIO_BK.ADC_RT, 2) - INVEST.PFOLIO_BK.TCST_AFT_COM, 2) ");
        sbMst.Append("AS APPRICIATION_ERROTION, ROUND((INVEST.PFOLIO_BK.TOT_NOS * INVEST.PFOLIO_BK.ADC_RT - INVEST.PFOLIO_BK.TCST_AFT_COM) ");
        sbMst.Append(" / INVEST.PFOLIO_BK.TCST_AFT_COM * 100, 2) AS PERCENT_OF_APRE_EROSION, ");
        sbMst.Append("ROUND(INVEST.PFOLIO_BK.TOT_NOS / INVEST.COMP.NO_SHRS * 100, 2) AS PERCENTAGE_OF_PAIDUP ");
        sbMst.Append("FROM         INVEST.PFOLIO_BK INNER JOIN ");
        sbMst.Append("INVEST.COMP ON INVEST.PFOLIO_BK.COMP_CD = INVEST.COMP.COMP_CD INNER JOIN ");
        sbMst.Append("INVEST.FUND ON INVEST.PFOLIO_BK.F_CD = INVEST.FUND.F_CD ");
        sbMst.Append("WHERE     (INVEST.PFOLIO_BK.BAL_DT_CTRL = '" + balDate + "') AND (INVEST.FUND.F_CD =" + fundCode + ") ");
        sbMst.Append("ORDER BY INVEST.PFOLIO_BK.SECT_MAJ_CD, INVEST.COMP.COMP_NM ");

        sbMst.Append(sbfilter.ToString());
        dtReprtSource = commonGatewayObj.Select(sbMst.ToString());

        DataTable dtNonlistedSecrities = new DataTable();
        sbMst = new StringBuilder();
        sbMst.Append("SELECT      INV_AMOUNT AS COST_PRICE, INV_AMOUNT AS MARKET_PRICE ");
        sbMst.Append("FROM         INVEST.NON_LISTED_SECURITIES ");
        sbMst.Append("WHERE     (F_CD = " + fundCode + ") AND (INV_DATE = ");
        sbMst.Append(" (SELECT     MAX(INV_DATE) AS EXPR1 ");
        sbMst.Append("FROM          INVEST.NON_LISTED_SECURITIES NON_LISTED_SECURITIES_1 ");
        sbMst.Append("WHERE     (F_CD = " + fundCode + ") AND (INV_DATE <= '" + balDate + "'))) ");
        dtNonlistedSecrities = commonGatewayObj.Select(sbMst.ToString());

        Decimal nonlistedCostPrice = 0;
        Decimal nonlistedMarketPrice = 0;
        if (dtNonlistedSecrities.Rows.Count > 0)
        {
            nonlistedCostPrice = Convert.ToDecimal(dtNonlistedSecrities.Rows[0][0]);
            nonlistedMarketPrice = Convert.ToDecimal(dtNonlistedSecrities.Rows[0][0]);
        }
        if (dtReprtSource.Rows.Count > 0)
        {
            Decimal totalInvest = 0;
            for (int loop = 0; loop < dtReprtSource.Rows.Count; loop++)
            {
                totalInvest = totalInvest + Convert.ToDecimal(dtReprtSource.Rows[loop]["TCST_AFT_COM"]);
            }
            dtReprtSource.TableName = "PortfolioWithNonListedReport";
            //dtReprtSource.WriteXmlSchema(@"F:\PortfolioManagementSystem\UI\ReportViewer\Report\crtPortfolioWithNonListedReport.xsd");
            //ReportDocument rdoc = new ReportDocument();
            string Path = "";
            Path = Server.MapPath("Report/crtPortfolioWithNonListedReport.rpt");
            rdoc.Load(Path);
            rdoc.SetDataSource(dtReprtSource);
            CRV_PfolioWithNonListed.ReportSource = rdoc;
            CRV_PfolioWithNonListed.DisplayToolbar = true;
            CRV_PfolioWithNonListed.HasExportButton = true;
            CRV_PfolioWithNonListed.HasPrintButton = true;
            rdoc.SetParameterValue("prmbalDate", balDate);
            rdoc.SetParameterValue("prmTotalInvest", totalInvest);
            rdoc.SetParameterValue("prmNonlistedCostPrice", nonlistedCostPrice);
            rdoc.SetParameterValue("prmNonlisteMarketPrice", nonlistedMarketPrice);
            rdoc = ReportFactory.GetReport(rdoc.GetType());
        }
        else
        {
            Response.Write("No Data Found");
        }
    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        CRV_PfolioWithNonListed.Dispose();
        CRV_PfolioWithNonListed = null;
        rdoc.Close();
        rdoc.Dispose();
        rdoc = null;
        GC.Collect();
    }
}
