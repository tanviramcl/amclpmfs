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
using CrystalDecisions.CrystalReports.Engine;
using System.Text;

public partial class UI_ReportViewer_CompanyWiseAllPortfoliosReportDSEonlyReportViewer : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    private ReportDocument rdoc = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder sbFilter = new StringBuilder();
        string howlaDate = "";
        string fundCodes = "";
        string companyCodes = "";
        string percentageCheck = "";
       
        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../../Default.aspx");
        }
        else
        {
            howlaDate = (string)Session["howlaDate"];
            fundCodes = (string)Session["fundCodes"];
            percentageCheck = (string)Session["percentageCheck"];
            companyCodes = (string)Session["companyCodes"];
        }
        DataTable dtReprtSource = new DataTable();
        StringBuilder sbMst = new StringBuilder();
        StringBuilder sbfilter = new StringBuilder();
        sbfilter.Append(" ");
        sbMst.Append(" SELECT     PFOLIO_BK.SECT_MAJ_NM, COMP.COMP_NM, FUND.F_NAME, PFOLIO_BK.TOT_NOS, PFOLIO_BK.TCST_AFT_COM, ");
        sbMst.Append(" ROUND(PFOLIO_BK.TCST_AFT_COM / PFOLIO_BK.TOT_NOS, 2) AS COST_RT_PER_SHARE, NVL(PFOLIO_BK.DSE_RT, ");
        sbMst.Append(" PFOLIO_BK.CSE_RT) AS DSE_RT, ROUND(PFOLIO_BK.TOT_NOS * NVL(PFOLIO_BK.DSE_RT, PFOLIO_BK.CSE_RT), 2) ");
        sbMst.Append(" AS TOT_MARKET_PRICE, ROUND(ROUND(NVL(PFOLIO_BK.DSE_RT, PFOLIO_BK.CSE_RT), 2) ");
        sbMst.Append(" - ROUND(PFOLIO_BK.TCST_AFT_COM / PFOLIO_BK.TOT_NOS, 2), 2) AS RATE_DIFF, ");
        sbMst.Append(" ROUND(ROUND(PFOLIO_BK.TOT_NOS * NVL(PFOLIO_BK.DSE_RT, PFOLIO_BK.CSE_RT), 2) - PFOLIO_BK.TCST_AFT_COM, 2) ");
        sbMst.Append(" AS APPRICIATION_ERROTION, PFOLIO_BK.BAL_DT_CTRL, ROUND(PFOLIO_BK.TOT_NOS / COMP.NO_SHRS * 100, 3) ");
        sbMst.Append(" AS PERCENTAGE_OF_PAIDUP ");
        sbMst.Append(" FROM         PFOLIO_BK INNER JOIN ");
        sbMst.Append(" COMP ON PFOLIO_BK.COMP_CD = COMP.COMP_CD INNER JOIN ");
        sbMst.Append(" FUND ON PFOLIO_BK.F_CD = FUND.F_CD ");
        sbMst.Append(" WHERE     (PFOLIO_BK.BAL_DT_CTRL = '"+howlaDate.ToString()+"') ");
        if (percentageCheck != "")
        {
            sbMst.Append(" AND (ROUND(PFOLIO_BK.TOT_NOS / COMP.NO_SHRS * 100, 3) >=" + percentageCheck + ") ");
        }
        if (fundCodes != "")
        {
            sbMst.Append(" AND FUND.F_CD IN(" + fundCodes + ") ");
        }
        if (companyCodes != "")
        {
            sbMst.Append(" AND PFOLIO_BK.COMP_CD IN(" + companyCodes + ") ");
        }
        sbMst.Append(" ORDER BY PFOLIO_BK.SECT_MAJ_NM, COMP.COMP_NM, PFOLIO_BK.F_CD ");

        sbMst.Append(sbfilter.ToString());
        dtReprtSource = commonGatewayObj.Select(sbMst.ToString());
        dtReprtSource.TableName = "CompanyWiseAllPortfoliosReportDSEonly";

        if (dtReprtSource.Rows.Count > 0)
        {
            //dtReprtSource.WriteXmlSchema(@"G:\F Drive\PortfolioManagementSystem\UI\ReportViewer\Report\crtmCompanyWiseAllPortfoliosReportDSEonlyReport.xsd");
            string Path = Server.MapPath("Report/crtmCompanyWiseAllPortfoliosReportDSEonlyReport.rpt");
            rdoc.Load(Path);
            rdoc.SetDataSource(dtReprtSource);
            CRV_CompanyWiseAllPortfolioDSEonly.ReportSource = rdoc;
            //rdoc.SetParameterValue("prmtransactionDate", tranDate);
            rdoc = ReportFactory.GetReport(rdoc.GetType());
        }
        else
        {
            Response.Write("No Data Found");
        }
    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        CRV_CompanyWiseAllPortfolioDSEonly.Dispose();
        CRV_CompanyWiseAllPortfolioDSEonly = null;
        rdoc.Close();
        rdoc.Dispose();
        rdoc = null;
        GC.Collect();
    }
}
