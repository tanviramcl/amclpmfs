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

public partial class UI_ReportViewer_ReceivableCashDividendReportViewer : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    private ReportDocument rdoc = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder sbFilter = new StringBuilder();
        string recordDateFrom = "";
        string recordDateTo = "";
        string agmDateFrom = "";
        string agmDateTo = "";
        string fundCode = "";
        
        DataTable dtIntimationReport = new DataTable();

        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../../Default.aspx");
        }
        else
        {
            fundCode = (string)Session["fundCode"];
            recordDateFrom = (string)Session["recordDateFrom"];
            recordDateTo = (string)Session["recordDateTo"];
            agmDateFrom = (string)Session["agmDateFrom"];
            agmDateTo = (string)Session["agmDateTo"];
        }

        DataTable dtReprtSource = new DataTable();
        StringBuilder sbMst = new StringBuilder();
       
        sbMst.Append(" SELECT     INVEST.FUND.F_NAME, INVEST.COMP.COMP_NM, INVEST.BOOK_CL.AGM, INVEST.BOOK_CL.RECORD_DT, INVEST.COMP.FC_VAL, INVEST.BOOK_CL.CASH, ");
        sbMst.Append(" INVEST.COMP.FC_VAL * INVEST.BOOK_CL.CASH / 100 AS DIVIDEND_PER_SHARE, INVEST.PFOLIO_BK.TOT_NOS,  ");
        sbMst.Append(" INVEST.PFOLIO_BK.TOT_NOS * INVEST.COMP.FC_VAL / 100 * INVEST.BOOK_CL.CASH AS GROSS_DIVIDEND, decode(INVEST.FUND.F_CD, 1, ");
        sbMst.Append(" INVEST.PFOLIO_BK.TOT_NOS * INVEST.COMP.FC_VAL / 100 * INVEST.BOOK_CL.CASH * .2, 0) AS TAX, decode(INVEST.FUND.F_CD, 1,  ");
        sbMst.Append(" INVEST.PFOLIO_BK.TOT_NOS * INVEST.COMP.FC_VAL / 100 * INVEST.BOOK_CL.CASH * .8,  ");
        sbMst.Append(" INVEST.PFOLIO_BK.TOT_NOS * INVEST.COMP.FC_VAL / 100 * INVEST.BOOK_CL.CASH) AS NET_DIVIDEND ");
        sbMst.Append(" FROM         INVEST.BOOK_CL INNER JOIN ");
        sbMst.Append(" INVEST.COMP ON INVEST.BOOK_CL.COMP_CD = INVEST.COMP.COMP_CD INNER JOIN ");
        sbMst.Append(" INVEST.FUND INNER JOIN ");
        sbMst.Append(" INVEST.PFOLIO_BK ON INVEST.FUND.F_CD = INVEST.PFOLIO_BK.F_CD ON INVEST.BOOK_CL.COMP_CD = INVEST.PFOLIO_BK.COMP_CD AND ");
        sbMst.Append(" INVEST.BOOK_CL.RECORD_DT = INVEST.PFOLIO_BK.BAL_DT_CTRL ");
        sbMst.Append(" WHERE       (INVEST.BOOK_CL.CASH IS NOT NULL) ");

        if ((recordDateFrom != "") && (recordDateTo != ""))
        {
            sbMst.Append(" AND (INVEST.BOOK_CL.RECORD_DT BETWEEN '"+recordDateFrom+"' AND '"+recordDateTo+"')");
        }
        
        if ((agmDateFrom != "") && (agmDateTo == ""))
        {
            sbMst.Append(" AND (INVEST.BOOK_CL.AGM >= '" + agmDateFrom + "')");
        }
        else if ((agmDateFrom == "") && (agmDateTo != ""))
        {
            sbMst.Append(" AND (INVEST.BOOK_CL.AGM <= '" + agmDateTo + "')");
        }
        else if ((agmDateFrom != "") && (agmDateTo != ""))
        {
            sbMst.Append(" AND (INVEST.BOOK_CL.AGM BETWEEN '" + agmDateFrom + "' AND '" + agmDateTo + "')");
        }

        if (fundCode != "0")
        {
            sbMst.Append(" AND (INVEST.FUND.F_CD = " + Convert.ToInt16(fundCode.ToString()) + ")");
        }
        sbMst.Append(" ORDER BY 2 ");

        dtReprtSource = commonGatewayObj.Select(sbMst.ToString());
        if (dtReprtSource.Rows.Count > 0)
        {
            dtReprtSource.TableName = "ReceivableCashDividendReport";
            //dtReprtSource.WriteXmlSchema(@"G:\F Drive\PortfolioManagementSystem\UI\ReportViewer\Report\crtReceivableCashDividendReport.xsd");
            string Path = "";
            Path = Server.MapPath("Report/crtReceivableCashDividendReport.rpt");
            rdoc.Load(Path);
            rdoc.SetDataSource(dtReprtSource);
            CRV_ReceivableCashDividend.ReportSource = rdoc;
            CRV_ReceivableCashDividend.DisplayToolbar = true;
            CRV_ReceivableCashDividend.HasExportButton = true;
            CRV_ReceivableCashDividend.HasPrintButton = true;
            rdoc.SetParameterValue("prmRecordDateFrom", recordDateFrom);
            rdoc.SetParameterValue("prmRecordDateTo", recordDateTo);
            rdoc = ReportFactory.GetReport(rdoc.GetType());
        }
        else
        {
            Response.Write("No Data Found");
        }
    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        CRV_ReceivableCashDividend.Dispose();
        CRV_ReceivableCashDividend = null;
        rdoc.Close();
        rdoc.Dispose();
        rdoc = null;
        GC.Collect();
    }
}
