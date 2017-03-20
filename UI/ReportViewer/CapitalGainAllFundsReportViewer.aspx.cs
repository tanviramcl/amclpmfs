using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UI_ReportViewer_CapitalGainAllFundsReportViewer : System.Web.UI.Page
{
    private ReportDocument rdoc = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
        string strFromdate = "";
        string strTodate = "";
        string fundCodes = "";
        string strSQL,strsql2;
        //string companyCodes = "";
        //string percentageCheck = "";

        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../../Default.aspx");
        }
        else
        {
            strFromdate = (string)Session["FromDate"];
            strTodate = (string)Session["ToDate"];
            fundCodes = (string)Session["fundCodes"];

        }
         CommonGateway commonGatewayObj = new CommonGateway();
        DataTable dtReprtSource = new DataTable();
        StringBuilder sbMst = new StringBuilder();
        StringBuilder sbfilter = new StringBuilder();

           strSQL = "SELECT p.f_cd1,q.f_cd2,p.fund_name1,q.fund_name2,p.COST1,q.COST2,q.SALE2,p.No_Of_Share1,q.No_Of_Share2,q.profit2 from " +                                 
              "(SELECT  t.F_CD f_cd1, f.f_name fund_name1,SUM(AMT_AFT_COM) as COST1, sum(no_share) as No_Of_Share1 FROM FUND_TRANS_HB  t, fund f " +
            " WHERE t.TRAN_TP = 'C' AND VCH_DT BETWEEN '" + strFromdate + "' AND '" + strTodate + "' and t.F_CD IN(" + fundCodes + ") and t.f_cd = f.f_cd and t.f_cd <> 3" +
            " GROUP BY t.F_CD, f.f_name ORDER BY t.F_CD, f.f_name)p, (SELECT t.F_CD f_cd2, f.f_name fund_name2,SUM(AMT_AFT_COM) as SALE2," +
            "sum(no_share) as No_Of_Share2,sum(crt_aft_com * no_share) as COST2,SUM(AMT_AFT_COM) - sum(crt_aft_com * no_share) as profit2" +
             " FROM FUND_TRANS_HB t, fund f WHERE t.TRAN_TP = 'S' AND VCH_DT BETWEEN '" + strFromdate + "' AND '" + strTodate + "' and t.F_CD IN(" + fundCodes + ") and f.f_cd = t.f_cd and t.f_cd <> 3" +
              " GROUP BY t.F_CD, f.f_name)q where p.f_cd1=q.f_cd2 order by p.f_cd1";


        dtReprtSource = commonGatewayObj.Select(strSQL);
        dtReprtSource.TableName = "CapitalGainAllFundsdatasetnew";
        //dtReprtSource.WriteXmlSchema(@"E:\amclpmfs\UI\ReportViewer\Report\xsdCapitalGainAllFundsdatasetnew.xsd");
        // dtReprtSource.WriteXmlSchema(@"D:\officialProject\2-13-2017\amclpmfs\UI\ReportViewer\Report\crtCapitalGainAllFundsdataset1.xsd");

        //DataTable dtReprtSource1 = new DataTable();
        //strsql2 = "SELECT  t.F_CD, f.f_name fund_name,SUM(AMT_AFT_COM) as SALE, sum(no_share) as No_Of_Share,sum(crt_aft_com * no_share) as COST,"+
        //    " SUM(AMT_AFT_COM) - sum(crt_aft_com * no_share) as profit FROM FUND_TRANS_HB t, fund f WHERE t.TRAN_TP = 'S' AND VCH_DT BETWEEN '"+strFromdate+"' AND '"+strTodate+ "' and f.f_cd = t.f_cd and t.f_cd <> 3 GROUP BY t.F_CD, f.f_name order by t.F_CD";
        //dtReprtSource1 = commonGatewayObj.Select(strsql2);
        //dtReprtSource1.TableName = "CapitalGainAllFundsdataset4";
        // dtReprtSource1.WriteXmlSchema(@"D:\officialProject\2-13-2017\amclpmfs\UI\ReportViewer\Report\crtCapitalGainAllFundsdataset2.xsd");

        //DataSet ds = new DataSet();
        //ds.Tables.Add(dtReprtSource);
        //ds.Tables.Add(dtReprtSource1);
        //rpt.SetDataSource(ds);

        if (dtReprtSource.Rows.Count > 0 )
        {
            string Path = Server.MapPath("Report/crptCapitalGainAllFundsReport.rpt");
            rdoc.Load(Path);
            rdoc.SetDataSource(dtReprtSource);
         
            CRV_CapitalGainAllFundsReportViewer.ReportSource = rdoc;
            rdoc.SetParameterValue("prmFromdate", strFromdate);
            rdoc.SetParameterValue("prmTodate", strTodate);
            rdoc = ReportFactory.GetReport(rdoc.GetType());







            //rdoc.Load(Path);
            //rdoc.SetDataSource(dtReprtSource);
            //CRV_PfolioWithProfitLoss.ReportSource = rdoc;
            //CRV_PfolioWithProfitLoss.DisplayToolbar = true;
            //CRV_PfolioWithProfitLoss.HasExportButton = true;
            //CRV_PfolioWithProfitLoss.HasPrintButton = true;
            //rdoc.SetParameterValue("prmbalDate", balDate);
            //rdoc.SetParameterValue("prmStatementType", statementType);
            //rdoc.SetParameterValue("prmappOrEro", appOrEro);
            //rdoc = ReportFactory.GetReport(rdoc.GetType());




        }
        else
        {
            Response.Write("No Data Found");
        }

        //if (dtReprtSource1.Rows.Count > 0)
        //{
        //    string Path = Server.MapPath("Report/testFUND_TRANS_HB.rpt");
        //    rdoc.Load(Path);
        //    rdoc.SetDataSource(dtReprtSource1);

        //    CrystalReportViewer1.ReportSource = rdoc;

        //    rdoc = ReportFactory.GetReport(rdoc.GetType());
        //}
        //else
        //{
        //    Response.Write("No Data Found");
        //}

    }
}