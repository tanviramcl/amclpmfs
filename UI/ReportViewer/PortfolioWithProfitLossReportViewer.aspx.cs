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

public partial class UI_ReportViewer_PortfolioWithProfitLossReportViewer : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    Pf1s1DAO pf1Obj = new Pf1s1DAO();
    private ReportDocument rdoc = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder sbFilter = new StringBuilder();
        string fundCode = "";
        string balDate = "";
        string statementType = "";
        string orderType = "";
        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../../Default.aspx");
        }
        else
        {
            statementType = (string)Session["statementType"];
            fundCode = (string)Session["fundCode"];
            balDate = (string)Session["balDate"];
            orderType = (string)Session["orderType"];
        }
        string appOrEro = "";
        DataTable dtReprtSource = new DataTable();
        StringBuilder sbMst = new StringBuilder();
        StringBuilder sbfilter = new StringBuilder();
        sbfilter.Append(" ");
        sbMst.Append("SELECT     INVEST.FUND.F_NAME, INVEST.COMP.COMP_NM, INVEST.PFOLIO_BK.SECT_MAJ_NM, INVEST.PFOLIO_BK.SECT_MAJ_CD, ");
        sbMst.Append("TRUNC(INVEST.PFOLIO_BK.TOT_NOS, 0) AS TOT_NOS, ROUND(INVEST.PFOLIO_BK.TCST_AFT_COM / INVEST.PFOLIO_BK.TOT_NOS, 2) ");
        sbMst.Append("AS COST_RT_PER_SHARE, INVEST.PFOLIO_BK.TCST_AFT_COM, NVL(INVEST.PFOLIO_BK.DSE_RT, INVEST.PFOLIO_BK.CSE_RT) AS DSE_RT, ");
        sbMst.Append("ROUND(INVEST.PFOLIO_BK.TOT_NOS * NVL(INVEST.PFOLIO_BK.DSE_RT, INVEST.PFOLIO_BK.CSE_RT), 2) AS TOT_MARKET_PRICE, ");
        sbMst.Append("ROUND(ROUND(NVL(INVEST.PFOLIO_BK.DSE_RT, INVEST.PFOLIO_BK.CSE_RT), 2) ");
        sbMst.Append("- ROUND(INVEST.PFOLIO_BK.TCST_AFT_COM / INVEST.PFOLIO_BK.TOT_NOS, 2), 2) AS RATE_DIFF, ");
        sbMst.Append("ROUND(ROUND(INVEST.PFOLIO_BK.TOT_NOS * NVL(INVEST.PFOLIO_BK.DSE_RT, INVEST.PFOLIO_BK.CSE_RT), 2) ");
        sbMst.Append("- INVEST.PFOLIO_BK.TCST_AFT_COM, 2) AS APPRICIATION_ERROTION ");
        sbMst.Append("FROM         INVEST.PFOLIO_BK INNER JOIN ");
        sbMst.Append("INVEST.COMP ON INVEST.PFOLIO_BK.COMP_CD = INVEST.COMP.COMP_CD INNER JOIN ");
        sbMst.Append("INVEST.FUND ON INVEST.PFOLIO_BK.F_CD = INVEST.FUND.F_CD ");
        sbMst.Append("WHERE     (INVEST.PFOLIO_BK.BAL_DT_CTRL = '" + balDate + "') AND (INVEST.FUND.F_CD =" + fundCode + ") ");
        if (string.Compare(statementType, "Profit", true) == 0)
        {
            sbMst.Append(" AND  (ROUND(ROUND(INVEST.PFOLIO_BK.TOT_NOS * NVL(INVEST.PFOLIO_BK.DSE_RT, INVEST.PFOLIO_BK.CSE_RT), 2)  - INVEST.PFOLIO_BK.TCST_AFT_COM, 2) >= 0) ");
            appOrEro = "Appreciation";
        }
        else if (string.Compare(statementType, "Loss", true) == 0)
        {
            sbMst.Append(" AND  (ROUND(ROUND(INVEST.PFOLIO_BK.TOT_NOS * NVL(INVEST.PFOLIO_BK.DSE_RT, INVEST.PFOLIO_BK.CSE_RT), 2)  - INVEST.PFOLIO_BK.TCST_AFT_COM, 2) < 0) ");
            appOrEro = "Erosion";
        }

        if (string.Compare(orderType, "orderByInvest", true) == 0)
        {
            sbMst.Append("ORDER BY INVEST.PFOLIO_BK.TCST_AFT_COM DESC ");
        }
        else if (string.Compare(orderType, "orderByAppriciation", true) == 0)
        {
            sbMst.Append("ORDER BY APPRICIATION_ERROTION DESC ");
        }
        else if (string.Compare(orderType, "orderByErosion", true) == 0)
        {
            sbMst.Append("ORDER BY APPRICIATION_ERROTION ");
        }
        else if (string.Compare(orderType, "orderByRateDiffASC", true) == 0)
        {
            sbMst.Append("ORDER BY RATE_DIFF ");
        }
        else if (string.Compare(orderType, "orderByRateDiffDSC", true) == 0)
        {
            sbMst.Append("ORDER BY RATE_DIFF DESC ");
        }
        else
        {
            sbMst.Append("ORDER BY INVEST.PFOLIO_BK.SECT_MAJ_CD, INVEST.COMP.COMP_NM ");
        }
        //sbMst.Append(sbfilter.ToString());
        dtReprtSource = commonGatewayObj.Select(sbMst.ToString());
        if (string.Compare(statementType, "All", true) == 0)
        {
            appOrEro = "Appreciation/ Erosion";
            if ((string.Compare(orderType, "orderByInvest", true) != 0) && (string.Compare(orderType, "orderByAppriciation", true) != 0) && (string.Compare(orderType, "orderByErosion", true) != 0) && (string.Compare(orderType, "orderByRateDiffASC", true) != 0) && (string.Compare(orderType, "orderByRateDiffDSC", true) != 0))
            {
                DataTable dtNonlistedSecrities = new DataTable();
                sbMst = new StringBuilder();
                sbMst.Append("SELECT      'OTHERS' AS COMP_NM,'OTHERS' AS SECT_MAJ_NM, 999 AS SECT_MAJ_CD, INV_AMOUNT AS TCST_AFT_COM, INV_AMOUNT AS TOT_MARKET_PRICE, 0 AS APPRICIATION_ERROTION ");
                sbMst.Append("FROM         INVEST.NON_LISTED_SECURITIES ");
                sbMst.Append("WHERE     (F_CD = " + fundCode + ") AND (INV_DATE = ");
                sbMst.Append(" (SELECT     MAX(INV_DATE) AS EXPR1 ");
                sbMst.Append("FROM          INVEST.NON_LISTED_SECURITIES NON_LISTED_SECURITIES_1 ");
                sbMst.Append("WHERE     (F_CD = " + fundCode + ") AND (INV_DATE <= '" + balDate + "'))) ");
                dtNonlistedSecrities = commonGatewayObj.Select(sbMst.ToString());

                if (dtNonlistedSecrities.Rows.Count > 0)
                {
                    DataRow drReport;
                    for (int loop = 0; loop < dtNonlistedSecrities.Rows.Count; loop++)
                    {
                        drReport = dtReprtSource.NewRow();
                        drReport["COMP_NM"] = dtNonlistedSecrities.Rows[loop]["COMP_NM"].ToString();
                        drReport["SECT_MAJ_NM"] = dtNonlistedSecrities.Rows[loop]["SECT_MAJ_NM"].ToString();
                        drReport["SECT_MAJ_CD"] = dtNonlistedSecrities.Rows[loop]["SECT_MAJ_CD"].ToString();
                        drReport["TCST_AFT_COM"] = dtNonlistedSecrities.Rows[loop]["TCST_AFT_COM"].ToString();
                        drReport["TOT_MARKET_PRICE"] = dtNonlistedSecrities.Rows[loop]["TOT_MARKET_PRICE"].ToString();
                        drReport["APPRICIATION_ERROTION"] = dtNonlistedSecrities.Rows[loop]["APPRICIATION_ERROTION"].ToString();

                        dtReprtSource.Rows.Add(drReport);
                    }
                }
            }
        }

        if (dtReprtSource.Rows.Count > 0)
        {
            dtReprtSource.TableName = "PortfolioWithProfitLoss";
            //dtReprtSource.WriteXmlSchema(@"F:\PortfolioManagementSystem\UI\ReportViewer\Report\crtPortfolioWithProfitLossReport.xsd");
            //ReportDocument rdoc = new ReportDocument();
            string Path = "";
            if ((string.Compare(orderType, "orderByInvest", true) != 0) && (string.Compare(orderType, "orderByAppriciation", true) != 0) && (string.Compare(orderType, "orderByErosion", true) != 0) && (string.Compare(orderType, "orderByRateDiffASC", true) != 0) && (string.Compare(orderType, "orderByRateDiffDSC", true) != 0))
            {
                Path = Server.MapPath("Report/crtPortfolioWithProfitLossReport.rpt");
            }
            else
            {
                Path = Server.MapPath("Report/crtPortfolioOrderByInvestmentOrAppriciation.rpt");
            }
            rdoc.Load(Path);
            rdoc.SetDataSource(dtReprtSource);
            CRV_PfolioWithProfitLoss.ReportSource = rdoc;
            CRV_PfolioWithProfitLoss.DisplayToolbar = true;
            CRV_PfolioWithProfitLoss.HasExportButton = true;
            CRV_PfolioWithProfitLoss.HasPrintButton = true;
            rdoc.SetParameterValue("prmbalDate", balDate);
            rdoc.SetParameterValue("prmStatementType", statementType);
            rdoc.SetParameterValue("prmappOrEro", appOrEro);
            rdoc = ReportFactory.GetReport(rdoc.GetType());
        }
        else
        {
            Response.Write("No Data Found");
        }
    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        CRV_PfolioWithProfitLoss.Dispose();
        CRV_PfolioWithProfitLoss = null;
        rdoc.Close();
        rdoc.Dispose();
        rdoc = null;
        GC.Collect();
    }
}
