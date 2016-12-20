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

public partial class UI_ReportViewer_AssetPercentageCheckReportViewer : System.Web.UI.Page
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

        string tranDate = Request.QueryString["transactionDate"].ToString();
        string percentageCheck = Request.QueryString["percentageCheck"].ToString();
        DataTable dtReprtSource = new DataTable();
        StringBuilder sbMst = new StringBuilder();
        StringBuilder sbfilter = new StringBuilder();
        sbfilter.Append(" ");
        sbMst.Append(" SELECT     INVEST.ASSET_VALUE.F_NAME, INVEST.COMP.COMP_NM, INVEST.PFOLIO_BK.SECT_MAJ_NM, INVEST.PFOLIO_BK.TOT_NOS,  ");
        sbMst.Append(" INVEST.PFOLIO_BK.TCST_AFT_COM, ROUND(INVEST.PFOLIO_BK.TCST_AFT_COM / INVEST.PFOLIO_BK.TOT_NOS, 2) AS COST_RT_PER_SHARE, ");
        sbMst.Append(" NVL(INVEST.PFOLIO_BK.DSE_RT, 0) AS DSE_RT, NVL(INVEST.PFOLIO_BK.CSE_RT, 0) AS CSE_RT, ROUND(INVEST.PFOLIO_BK.ADC_RT, 2) ");
        sbMst.Append(" AS AVG_RATE, ROUND(INVEST.PFOLIO_BK.TOT_NOS * INVEST.PFOLIO_BK.ADC_RT, 2) AS TOT_MARKET_PRICE,  ");
        sbMst.Append(" ROUND(ROUND(INVEST.PFOLIO_BK.ADC_RT, 2) - ROUND(INVEST.PFOLIO_BK.TCST_AFT_COM / INVEST.PFOLIO_BK.TOT_NOS, 2), 2) AS RATE_DIFF, ");
        sbMst.Append(" ROUND(ROUND(INVEST.PFOLIO_BK.TOT_NOS * INVEST.PFOLIO_BK.ADC_RT, 2) - INVEST.PFOLIO_BK.TCST_AFT_COM, 2) ");
        sbMst.Append(" AS APPRICIATION_ERROTION, INVEST.PFOLIO_BK.BAL_DT_CTRL, INVEST.ASSET_VALUE.ASSET_VALUE,  ");
        sbMst.Append(" ROUND(INVEST.PFOLIO_BK.TCST_AFT_COM / INVEST.ASSET_VALUE.ASSET_VALUE * 100, 2) AS HOLDING_PERCENTAGE_OF_ASSET, ");
        sbMst.Append(" ROUND(INVEST.PFOLIO_BK.TOT_NOS / INVEST.COMP.NO_SHRS * 100, 2) AS PERCENTAGE_OF_PAIDUP ");
        sbMst.Append(" FROM         INVEST.PFOLIO_BK INNER JOIN  ");
        sbMst.Append(" INVEST.ASSET_VALUE ON INVEST.PFOLIO_BK.F_CD = INVEST.ASSET_VALUE.F_CD INNER JOIN ");
        sbMst.Append(" INVEST.COMP ON INVEST.PFOLIO_BK.COMP_CD = INVEST.COMP.COMP_CD WHERE ");
        if (percentageCheck != "")
        {
            sbMst.Append(" (ROUND(INVEST.PFOLIO_BK.TCST_AFT_COM / INVEST.ASSET_VALUE.ASSET_VALUE * 100, 2) >="+percentageCheck+") and ");
        }
        sbMst.Append(" (INVEST.PFOLIO_BK.BAL_DT_CTRL = '" + Convert.ToDateTime(Request.QueryString["transactionDate"]).ToString("dd-MMM-yyyy") + "')  ");
        sbMst.Append(" ORDER BY INVEST.PFOLIO_BK.SECT_MAJ_NM, INVEST.COMP.COMP_NM, INVEST.PFOLIO_BK.F_CD ");


        sbMst.Append(sbfilter.ToString());
        dtReprtSource = commonGatewayObj.Select(sbMst.ToString());
        dtReprtSource.TableName = "AssetPercentageCheck";

        if (dtReprtSource.Rows.Count > 0)
        {
            //dtReprtSource.WriteXmlSchema(@"F:\PortfolioManagementSystem\UI\ReportViewer\Report\crtmAssetPercentageCheckReport.xsd");

            //ReportDocument rdoc = new ReportDocument();
            string Path = Server.MapPath("Report/AssetPercentageCheckReport.rpt");
            rdoc.Load(Path);
            rdoc.SetDataSource(dtReprtSource);
            CRV_AssetPercentageCheck.ReportSource = rdoc;
            rdoc.SetParameterValue("prmtransactionDate", tranDate);
            rdoc = ReportFactory.GetReport(rdoc.GetType());
            CRV_AssetPercentageCheck.DisplayToolbar = true;
            CRV_AssetPercentageCheck.HasExportButton = true;
            CRV_AssetPercentageCheck.HasPrintButton = true;
        }
        else
        {
            Response.Write("No Data Found");
        }
    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        CRV_AssetPercentageCheck.Dispose();
        CRV_AssetPercentageCheck = null;
        rdoc.Close();
        rdoc.Dispose();
        rdoc = null;
        GC.Collect();
    }
}
