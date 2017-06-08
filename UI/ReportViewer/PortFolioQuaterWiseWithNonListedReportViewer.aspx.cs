using System;
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
        string quaterEndDate = "";
        string prevQuaterEndDate = "";
        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../../Default.aspx");
        }
        else
        {
            fundCode = (string)Session["fundCodes"];
            quaterEndDate = (string)Session["quaterEndDate"];
            prevQuaterEndDate = (string)Session["PrevQuaterEnddate"];
        }

        DataTable dtReprtSource = new DataTable();
        StringBuilder sbMst = new StringBuilder();
        StringBuilder sbfilter = new StringBuilder();
        sbfilter.Append(" ");
        sbMst.Append(" select quarterend.f_name, quarterend.COMP_NM as ,quarterend.SECT_MAJ_NM,quarterend.SECT_MAJ_CD,quarterend.TOT_NOS,quarterend.TOT_MARKET_PRICE,quarterend.TCST_AFT_COM, ");
        sbMst.Append(" quarterend.APPRICIATION_ERROTION,prevquarterend.f_name as prevfname,prevquarterend.COMP_nm as prevcomp,prevquarterend.SECT_MAJ_NM as prevSECT_MAJ_NM ,prevquarterend.SECT_MAJ_CD as prevSECT_MAJ_CD, ");
        sbMst.Append(" prevquarterend.TOT_NOS as prevTOT_NOS,prevquarterend.TOT_MARKET_PRICE as prevTOT_MARKET_PRICE ,prevquarterend.TCST_AFT_COM as prevTCST_AFT_COM, prevquarterend.PERCENT_OF_APRE_EROSION prevAPPRICIATION_ERROTION ");
        sbMst.Append(" from (SELECT     INVEST.FUND.f_cd,INVEST.FUND.F_NAME, INVEST.COMP.COMP_NM, INVEST.COMP.COMP_cd, INVEST.PFOLIO_BK.SECT_MAJ_NM,INVEST.PFOLIO_BK.SECT_MAJ_CD, ");
        sbMst.Append(" TRUNC(INVEST.PFOLIO_BK.TOT_NOS,0) AS TOT_NOS, ROUND(INVEST.PFOLIO_BK.TCST_AFT_COM / INVEST.PFOLIO_BK.TOT_NOS, 2) AS COST_RT_PER_SHARE, ");
        sbMst.Append(" INVEST.PFOLIO_BK.TCST_AFT_COM, NVL(INVEST.PFOLIO_BK.DSE_RT, 0) AS DSE_RT, NVL(INVEST.PFOLIO_BK.CSE_RT, 0) AS CSE_RT, ROUND(INVEST.PFOLIO_BK.ADC_RT, 2) AS AVG_RATE, ");
        sbMst.Append(" ROUND(INVEST.PFOLIO_BK.TOT_NOS * INVEST.PFOLIO_BK.ADC_RT, 2) AS TOT_MARKET_PRICE, ");
        sbMst.Append("  ROUND(ROUND(INVEST.PFOLIO_BK.ADC_RT, 2) - ROUND(INVEST.PFOLIO_BK.TCST_AFT_COM / INVEST.PFOLIO_BK.TOT_NOS, 2), 2) AS RATE_DIFF,  ");
        sbMst.Append("  ROUND(ROUND(INVEST.PFOLIO_BK.TOT_NOS * INVEST.PFOLIO_BK.ADC_RT, 2) - INVEST.PFOLIO_BK.TCST_AFT_COM, 2) AS APPRICIATION_ERROTION, ");
        sbMst.Append("  ROUND((INVEST.PFOLIO_BK.TOT_NOS * INVEST.PFOLIO_BK.ADC_RT - INVEST.PFOLIO_BK.TCST_AFT_COM)  / INVEST.PFOLIO_BK.TCST_AFT_COM * 100, 2) AS PERCENT_OF_APRE_EROSION, ");
        sbMst.Append("  ROUND(INVEST.PFOLIO_BK.TOT_NOS / INVEST.COMP.NO_SHRS * 100, 2) AS PERCENTAGE_OF_PAIDUP FROM  ");
        sbMst.Append(" INVEST.PFOLIO_BK INNER JOIN INVEST.COMP ON INVEST.PFOLIO_BK.COMP_CD = INVEST.COMP.COMP_CD ");
        sbMst.Append(" INNER JOIN INVEST.FUND ON INVEST.PFOLIO_BK.F_CD = INVEST.FUND.F_CD WHERE     (INVEST.PFOLIO_BK.BAL_DT_CTRL = '31-Mar-2016') AND (INVEST.FUND.F_CD =4) ORDER BY ");
        sbMst.Append(" INVEST.PFOLIO_BK.SECT_MAJ_CD, INVEST.COMP.COMP_NM)quarterend full outer join ");
        sbMst.Append(" ( SELECT    INVEST.FUND.f_cd, INVEST.FUND.F_NAME, INVEST.COMP.COMP_NM,  INVEST.COMP.COMP_cd,INVEST.PFOLIO_BK.SECT_MAJ_NM,INVEST.PFOLIO_BK.SECT_MAJ_CD, ");
        sbMst.Append(" TRUNC(INVEST.PFOLIO_BK.TOT_NOS,0) AS TOT_NOS, ROUND(INVEST.PFOLIO_BK.TCST_AFT_COM / INVEST.PFOLIO_BK.TOT_NOS, 2) AS COST_RT_PER_SHARE, ");
        sbMst.Append("  INVEST.PFOLIO_BK.TCST_AFT_COM, NVL(INVEST.PFOLIO_BK.DSE_RT, 0) AS DSE_RT, NVL(INVEST.PFOLIO_BK.CSE_RT, 0) AS CSE_RT, ROUND(INVEST.PFOLIO_BK.ADC_RT, 2) AS AVG_RATE, ");
        sbMst.Append("  ROUND(INVEST.PFOLIO_BK.TOT_NOS * INVEST.PFOLIO_BK.ADC_RT, 2) AS TOT_MARKET_PRICE, ");
        sbMst.Append(" ROUND(ROUND(INVEST.PFOLIO_BK.ADC_RT, 2) - ROUND(INVEST.PFOLIO_BK.TCST_AFT_COM / INVEST.PFOLIO_BK.TOT_NOS, 2), 2) AS RATE_DIFF, ");
        sbMst.Append(" ROUND((INVEST.PFOLIO_BK.TOT_NOS * INVEST.PFOLIO_BK.ADC_RT - INVEST.PFOLIO_BK.TCST_AFT_COM)  / INVEST.PFOLIO_BK.TCST_AFT_COM * 100, 2) AS PERCENT_OF_APRE_EROSION, ");
        sbMst.Append(" ROUND(INVEST.PFOLIO_BK.TOT_NOS / INVEST.COMP.NO_SHRS * 100, 2) AS PERCENTAGE_OF_PAIDUP FROM  ");
        sbMst.Append("  INVEST.PFOLIO_BK INNER JOIN INVEST.COMP ON INVEST.PFOLIO_BK.COMP_CD = INVEST.COMP.COMP_CD ");
        sbMst.Append(" INNER JOIN INVEST.FUND ON INVEST.PFOLIO_BK.F_CD = INVEST.FUND.F_CD WHERE     (INVEST.PFOLIO_BK.BAL_DT_CTRL = '21-dec-2015') AND (INVEST.FUND.F_CD =4) ORDER BY ");
        sbMst.Append(" INVEST.PFOLIO_BK.SECT_MAJ_CD, INVEST.COMP.COMP_NM )prevquarterend on quarterend.f_cd=prevquarterend.f_cd ");
        sbMst.Append(" and quarterend.comp_Cd=prevquarterend.comp_Cd ");
        dtReprtSource = commonGatewayObj.Select(sbMst.ToString());

        //DataTable dtNonlistedSecrities = new DataTable();
        //sbMst = new StringBuilder();
        //sbMst.Append("SELECT      INV_AMOUNT AS COST_PRICE, INV_AMOUNT AS MARKET_PRICE ");
        //sbMst.Append("FROM         INVEST.NON_LISTED_SECURITIES ");
        //sbMst.Append("WHERE     (F_CD = " + fundCode + ") AND (INV_DATE = ");
        //sbMst.Append(" (SELECT     MAX(INV_DATE) AS EXPR1 ");
        //sbMst.Append("FROM          INVEST.NON_LISTED_SECURITIES NON_LISTED_SECURITIES_1 ");
        //sbMst.Append("WHERE     (F_CD = " + fundCode + ") AND (INV_DATE <= '" + quaterEndDate + "'))) ");
        //dtNonlistedSecrities = commonGatewayObj.Select(sbMst.ToString());

        //Decimal nonlistedCostPrice = 0;
        //Decimal nonlistedMarketPrice = 0;
        //if (dtNonlistedSecrities.Rows.Count > 0)
        //{
        //    nonlistedCostPrice = Convert.ToDecimal(dtNonlistedSecrities.Rows[0][0]);
        //    nonlistedMarketPrice = Convert.ToDecimal(dtNonlistedSecrities.Rows[0][0]);
        //}
        if (dtReprtSource.Rows.Count > 0)
        {
            //Decimal totalInvest = 0;
            //for (int loop = 0; loop < dtReprtSource.Rows.Count; loop++)
            //{
            //    totalInvest = totalInvest + Convert.ToDecimal(dtReprtSource.Rows[loop]["TCST_AFT_COM"]);
            //}
            dtReprtSource.TableName = "PortfolioQuarterlyReport";
           // dtReprtSource.WriteXmlSchema(@"D:\officialProject\4-5-2017\amclpmfs\UI\ReportViewer\Report\CR_PortfolioQuarterlyReport.xsd");
            ReportDocument rdoc = new ReportDocument();
            string Path = "";
            Path = Server.MapPath("Report/crtPortfolioQuaterlyReport.rpt");
            rdoc.Load(Path);
            rdoc.SetDataSource(dtReprtSource);
            CRV_PfolioWithNonListed.ReportSource = rdoc;
            CRV_PfolioWithNonListed.DisplayToolbar = true;
            CRV_PfolioWithNonListed.HasExportButton = true;
            CRV_PfolioWithNonListed.HasPrintButton = true;
            rdoc.SetParameterValue("prmbalDate", quaterEndDate);
            rdoc.SetParameterValue("prmbalDate2", prevQuaterEndDate);
            //rdoc.SetParameterValue("prmTotalInvest", totalInvest);
            //rdoc.SetParameterValue("prmNonlistedCostPrice", nonlistedCostPrice);
            //rdoc.SetParameterValue("prmNonlisteMarketPrice", nonlistedMarketPrice);
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
