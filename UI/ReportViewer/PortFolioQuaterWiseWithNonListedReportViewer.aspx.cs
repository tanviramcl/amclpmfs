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
        sbMst.Append(" select nvl(quarterend.f_name,prevquarterend.f_name) as f_name, nvl(quarterend.COMP_NM,prevquarterend.COMP_nm) as COMP_NM ,nvl(quarterend.SECT_MAJ_NM,prevquarterend.SECT_MAJ_NM) as SECT_MAJ_NM,nvl(quarterend.SECT_MAJ_CD,prevquarterend.SECT_MAJ_CD) as SECT_MAJ_CD,nvl(quarterend.TOT_NOS,0) as TOT_NOS,nvl(quarterend.TOT_MARKET_PRICE,0)as TOT_MARKET_PRICE,nvl(quarterend.TCST_AFT_COM,0) as TCST_AFT_COM,");
        sbMst.Append(" nvl(quarterend.APPRICIATION_ERROTION,0) as APPRICIATION_ERROTION,prevquarterend.f_name as prevfname,prevquarterend.COMP_nm as prevcomp,prevquarterend.SECT_MAJ_NM as prevSECT_MAJ_NM ,prevquarterend.SECT_MAJ_CD as prevSECT_MAJ_CD, ");
        sbMst.Append(" prevquarterend.TOT_NOS as prevTOT_NOS,prevquarterend.TOT_MARKET_PRICE as prevTOT_MARKET_PRICE ,prevquarterend.TCST_AFT_COM as prevTCST_AFT_COM, prevquarterend.PERCENT_OF_APRE_EROSION prevAPPRICIATION_ERROTION ");
        sbMst.Append(" from (SELECT     FUND.f_cd, FUND.F_NAME, COMP.COMP_NM, COMP.COMP_cd, PFOLIO_BK.SECT_MAJ_NM, PFOLIO_BK.SECT_MAJ_CD, ");
        sbMst.Append(" TRUNC(PFOLIO_BK.TOT_NOS,0) AS TOT_NOS, ROUND(PFOLIO_BK.TCST_AFT_COM / PFOLIO_BK.TOT_NOS, 2) AS COST_RT_PER_SHARE, ");
        sbMst.Append(" PFOLIO_BK.TCST_AFT_COM, NVL(PFOLIO_BK.DSE_RT, 0) AS DSE_RT, NVL(PFOLIO_BK.CSE_RT, 0) AS CSE_RT, ROUND(PFOLIO_BK.ADC_RT, 2) AS AVG_RATE, ");
        sbMst.Append(" ROUND(PFOLIO_BK.TOT_NOS * PFOLIO_BK.ADC_RT, 2) AS TOT_MARKET_PRICE, ");
        sbMst.Append("  ROUND(ROUND(PFOLIO_BK.ADC_RT, 2) - ROUND(PFOLIO_BK.TCST_AFT_COM / PFOLIO_BK.TOT_NOS, 2), 2) AS RATE_DIFF,  ");
        sbMst.Append("  ROUND(ROUND(PFOLIO_BK.TOT_NOS * PFOLIO_BK.ADC_RT, 2) - PFOLIO_BK.TCST_AFT_COM, 2) AS APPRICIATION_ERROTION, ");
        sbMst.Append("  ROUND((PFOLIO_BK.TOT_NOS * PFOLIO_BK.ADC_RT - PFOLIO_BK.TCST_AFT_COM)  / PFOLIO_BK.TCST_AFT_COM * 100, 2) AS PERCENT_OF_APRE_EROSION, ");
        sbMst.Append("  ROUND(PFOLIO_BK.TOT_NOS / COMP.NO_SHRS * 100, 2) AS PERCENTAGE_OF_PAIDUP FROM  ");
        sbMst.Append("  PFOLIO_BK INNER JOIN COMP ON PFOLIO_BK.COMP_CD = COMP.COMP_CD ");
        sbMst.Append(" INNER JOIN FUND ON PFOLIO_BK.F_CD = FUND.F_CD WHERE     (PFOLIO_BK.BAL_DT_CTRL = '"+ quaterEndDate + "') AND (FUND.F_CD ="+fundCode+") ORDER BY ");
        sbMst.Append(" PFOLIO_BK.SECT_MAJ_CD, COMP.COMP_NM)quarterend full outer join ");
        sbMst.Append(" ( SELECT    FUND.f_cd, FUND.F_NAME, COMP.COMP_NM,  COMP.COMP_cd,PFOLIO_BK.SECT_MAJ_NM,PFOLIO_BK.SECT_MAJ_CD, ");
        sbMst.Append(" TRUNC(PFOLIO_BK.TOT_NOS,0) AS TOT_NOS, ROUND(PFOLIO_BK.TCST_AFT_COM / PFOLIO_BK.TOT_NOS, 2) AS COST_RT_PER_SHARE, ");
        sbMst.Append("  PFOLIO_BK.TCST_AFT_COM, NVL(PFOLIO_BK.DSE_RT, 0) AS DSE_RT, NVL(PFOLIO_BK.CSE_RT, 0) AS CSE_RT, ROUND(PFOLIO_BK.ADC_RT, 2) AS AVG_RATE, ");
        sbMst.Append("  ROUND(PFOLIO_BK.TOT_NOS * PFOLIO_BK.ADC_RT, 2) AS TOT_MARKET_PRICE, ");
        sbMst.Append(" ROUND(ROUND(PFOLIO_BK.ADC_RT, 2) - ROUND(PFOLIO_BK.TCST_AFT_COM / PFOLIO_BK.TOT_NOS, 2), 2) AS RATE_DIFF, ");
        sbMst.Append(" ROUND((PFOLIO_BK.TOT_NOS * PFOLIO_BK.ADC_RT - PFOLIO_BK.TCST_AFT_COM)  / PFOLIO_BK.TCST_AFT_COM * 100, 2) AS PERCENT_OF_APRE_EROSION, ");
        sbMst.Append(" ROUND(PFOLIO_BK.TOT_NOS / COMP.NO_SHRS * 100, 2) AS PERCENTAGE_OF_PAIDUP FROM  ");
        sbMst.Append("  PFOLIO_BK INNER JOIN COMP ON PFOLIO_BK.COMP_CD = COMP.COMP_CD ");
        sbMst.Append(" INNER JOIN FUND ON PFOLIO_BK.F_CD = FUND.F_CD WHERE     (PFOLIO_BK.BAL_DT_CTRL = '" + prevQuaterEndDate + "') AND (FUND.F_CD =" + fundCode + ") ORDER BY ");
        sbMst.Append(" PFOLIO_BK.SECT_MAJ_CD, COMP.COMP_NM )prevquarterend on quarterend.f_cd=prevquarterend.f_cd ");
        sbMst.Append(" and quarterend.comp_Cd=prevquarterend.comp_Cd ");
        dtReprtSource = commonGatewayObj.Select(sbMst.ToString());

     
        if (dtReprtSource.Rows.Count > 0)
        {
          
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
