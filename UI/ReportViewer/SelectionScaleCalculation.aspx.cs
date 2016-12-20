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

public partial class UI_ReportViewer_SelectionScaleCalculation : System.Web.UI.Page
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

        DataTable dtSelectionScale = new DataTable();
        StringBuilder sbMst = new StringBuilder();
        StringBuilder sbfilter = new StringBuilder();
        sbfilter.Append(" ");
        sbMst.Append("SELECT     INVEST.AMCL_EMP_SALARY_SELECTION.ID, INVEST.EMP_INFO.NAME, INVEST.DESIGNATION.DESIG, INVEST.EMP_INFO.ICB_ID, ");
        sbMst.Append(" INVEST.AMCL_EMP_SALARY_SELECTION.MONTH, INVEST.AMCL_EMP_SALARY_SELECTION.BASIC_AS_30JUN09, ");
        sbMst.Append(" INVEST.AMCL_EMP_SALARY_SELECTION.BASIC_CURRENT, INVEST.AMCL_EMP_SALARY_SELECTION.HOUSE_RENT_AS_30JUN09, ");
        sbMst.Append("INVEST.AMCL_EMP_SALARY_SELECTION.HOUSE_RENT_NEW, INVEST.AMCL_EMP_SALARY_SELECTION.BASIC_AS_PREV,  ");
        sbMst.Append("INVEST.AMCL_EMP_SALARY_SELECTION.FIXATION_BASIC, INVEST.AMCL_EMP_SALARY_SELECTION.ALLOWANCE_BASIC_DIFF, ");
        sbMst.Append("INVEST.AMCL_EMP_SALARY_SELECTION.ALLOWANCE_HOUSE_RENT, INVEST.AMCL_EMP_SALARY_SELECTION.ALLOWANCE_PENSION,  ");
        sbMst.Append("INVEST.AMCL_EMP_SALARY_SELECTION.ALLOWANCE_DEPUTATION, INVEST.AMCL_EMP_SALARY_SELECTION.GROSS_ALLOWANCE,  ");
        sbMst.Append(" INVEST.AMCL_EMP_SALARY_SELECTION.DEDUCTION_PF_OWN_CON, INVEST.AMCL_EMP_SALARY_SELECTION.DEDUCTION_PENSION,  ");
        sbMst.Append("INVEST.AMCL_EMP_SALARY_SELECTION.GROSS_DEDUCTION, INVEST.AMCL_EMP_SALARY_SELECTION.ARREAR_AMOUNT,INVEST.AMCL_EMP_SALARY_SELECTION.NET_PAYABLE  ");
        sbMst.Append("FROM       INVEST.EMP_INFO INNER JOIN  ");
        sbMst.Append("INVEST.DESIGNATION ON INVEST.EMP_INFO.RANK = INVEST.DESIGNATION.RANK INNER JOIN ");
        sbMst.Append("INVEST.AMCL_EMP_SALARY_SELECTION ON INVEST.EMP_INFO.ID = INVEST.AMCL_EMP_SALARY_SELECTION.ID ");

        sbMst.Append(sbfilter.ToString());
        dtSelectionScale = commonGatewayObj.Select(sbMst.ToString());
        dtSelectionScale.TableName = "SelectionScale";

        if (dtSelectionScale.Rows.Count > 0)
        {
            //dtSelectionScale.WriteXmlSchema(@"F:\PortfolioManagementSystem\UI\ReportViewer\Report\crtmSelectionScaleReport.xsd");

            //ReportDocument rdoc = new ReportDocument();
            string Path = Server.MapPath("Report/SelectionScaleReport.rpt");
            rdoc.Load(Path);
            rdoc.SetDataSource(dtSelectionScale);
            CrystalReportViewerSelectionScaleCalculation.ReportSource = rdoc;
            CrystalReportViewerSelectionScaleCalculation.DisplayToolbar = true;
            CrystalReportViewerSelectionScaleCalculation.HasExportButton = true;
            CrystalReportViewerSelectionScaleCalculation.HasPrintButton = true;
            rdoc = ReportFactory.GetReport(rdoc.GetType());
        }
        else
        {
            Response.Write("No Data Found");
        }
    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        CrystalReportViewerSelectionScaleCalculation.Dispose();
        CrystalReportViewerSelectionScaleCalculation = null;
        rdoc.Close();
        rdoc.Dispose();
        rdoc = null;
        GC.Collect();
    }
}
