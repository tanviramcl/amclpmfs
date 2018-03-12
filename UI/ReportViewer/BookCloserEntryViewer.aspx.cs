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
using CrystalDecisions.CrystalReports.Engine;
using System.Text;

public partial class UI_ReportViewer_Report_BookCloserEntryViewer : System.Web.UI.Page
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

        string entryDate = "";
        string toEntryDate = "";
        if (Request.QueryString["entryDate"] != "")
        {
            entryDate = Request.QueryString["entryDate"].ToString();
        }
        if (Request.QueryString["toEntryDate"] != "")
        {
            toEntryDate = Request.QueryString["toEntryDate"].ToString();
        }
        //DateTime toentryDate = Convert.ToDateTime(Request.QueryString["toEntryDate"]);

        int comCode = Convert.ToInt32(Request.QueryString["compCode"]);
        DataTable dtReprtSource = new DataTable();

        StringBuilder sbMst = new StringBuilder();
        StringBuilder sbfilter = new StringBuilder();
        sbfilter.Append(" ");

        dtReprtSource.TableName = "Report";

        sbMst.Append("SELECT COMP.COMP_NM, BOOK_CL.COMP_CD, BOOK_CL.FY, BOOK_CL.RECORD_DT, BOOK_CL.BOOK_TO, BOOK_CL.BONUS, BOOK_CL.RIGHT_APPR_DT, BOOK_CL.\"RIGHT\",");
        sbMst.Append("BOOK_CL.CASH, BOOK_CL.AGM, BOOK_CL.REMARKS, BOOK_CL.POSTED, BOOK_CL.PDATE FROM COMP INNER JOIN BOOK_CL ON COMP.COMP_CD = BOOK_CL.COMP_CD WHERE(1 = 1)");

        if (entryDate != "" && toEntryDate == "")
        {
            sbfilter.Append(" AND  (BOOK_CL.ENTRY_DATE >='" + Convert.ToDateTime(Request.QueryString["entryDate"]).ToString("dd-MMM-yyyy") + "')");
        }
        else if (entryDate == "" && toEntryDate != "")
        {
            sbfilter.Append(" AND  (BOOK_CL.ENTRY_DATE <='" + Convert.ToDateTime(Request.QueryString["toEntryDate"]).ToString("dd-MMM-yyyy") + "')");
        }
        else if (entryDate != "" && toEntryDate != "")
        {
            sbfilter.Append(" AND  (BOOK_CL.ENTRY_DATE >='" + Convert.ToDateTime(Request.QueryString["entryDate"]).ToString("dd-MMM-yyyy") + "') AND  (BOOK_CL.ENTRY_DATE <='" + Convert.ToDateTime(Request.QueryString["toEntryDate"]).ToString("dd-MMM-yyyy") + "')");
        }

        if (comCode != 0)
        {
            sbfilter.Append(" AND (BOOK_CL.COMP_CD =" + comCode + ")");
        }

        sbMst.Append(sbfilter.ToString());
        sbMst.Append(" ORDER BY COMP.COMP_NM,FY");
        dtReprtSource = commonGatewayObj.Select(sbMst.ToString());


        if (dtReprtSource.Rows.Count > 0)
        {
            //dtReprtSource.WriteXmlSchema(@"G:\AMCL.BookCloser\UI\ReportViewer\Report\crtmReport.xsd");


            //ReportDocument rdoc = new ReportDocument();
            string Path = Server.MapPath("Report/crtmReport.rpt");
            rdoc.Load(Path);
            rdoc.SetDataSource(dtReprtSource);
            CrystalReportViewer1.ReportSource = rdoc;
            rdoc = ReportFactory.GetReport(rdoc.GetType());
            CrystalReportViewer1.DisplayToolbar = true;
            CrystalReportViewer1.HasExportButton = true;
            CrystalReportViewer1.HasPrintButton = true;

        }
        else
        {
            Response.Write("No Data Found");
        }

    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        CrystalReportViewer1.Dispose();
        CrystalReportViewer1 = null;
        rdoc.Close();
        rdoc.Dispose();
        rdoc = null;
        GC.Collect();
    }
}
