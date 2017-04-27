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

public partial class UI_AssetPercentageNAVSummaryAndPortfolioReportViewer : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    Pf1s1DAO pf1Obj = new Pf1s1DAO();
    private ReportDocument rdoc = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder sbFilter = new StringBuilder();
        string fundCode = "";
        string fundName = "";
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
            fundName = (string)Session["fundName"];
            balDate = (string)Session["balDate"];
           
        }
        string strSQL;
        double cs_asset,cf_unlist,TotalAssetValue_Mar;
        DataTable dtReprtSource = new DataTable();
        DataTable dtNAV = new DataTable();
        DataTable dtNonListed = new DataTable();
        strSQL = "select nvl(sum(costprice),0) total_asset_c,nvl(sum(marketprice),0) total_asset_m from nav.nav_details n, nav.nav_master m where " +
                  " n.NAVROWTYPE = 'A' and n.navfundid =  " + fundCode + " and  m.navfundid = " + fundCode + " and m.navno = (select max(navno) from nav.nav_master m where m.navfundid = " + fundCode + " AND m.NAVDATE <='" + balDate + "' )" +
                  " and n.navfundid = m.navfundid and m.navno = n.navno";

        dtNAV = commonGatewayObj.Select(strSQL);
        cs_asset = Convert.ToDouble(dtNAV.Rows[0]["total_asset_c"].ToString());
        if (cs_asset == 0)
        {
            cs_asset = 1;
        }
        TotalAssetValue_Mar = Convert.ToDouble(dtNAV.Rows[0]["total_asset_m"].ToString());


        strSQL = "select f_cd, inv_amount, inv_date FROM(SELECT f_cd, inv_amount, inv_date, " +
               "rank() over (partition by f_cd order by inv_date desc) rnk FROM NON_LISTED_SECURITIES where  inv_amount>0 and F_CD IN(" + fundCode + ") and inv_date<='" + balDate + "' )" +
               " WHERE rnk = 1";
        dtNonListed = commonGatewayObj.Select(strSQL);
        if (dtNonListed.Rows.Count > 0)
        {
            cf_unlist = Convert.ToDouble(dtNonListed.Rows[0]["inv_amount"].ToString());
        }
        else
        {
            cf_unlist = 0;
        }
        if (string.Compare(statementType, "Summary (Total Asset Value)", true) == 0)
        {

            strSQL = "select p.sect_maj_nm,p.f_cd, p.sect_maj_cd, sum(p.nos_t)TotalShares, sum(p.rt_acm),sum(p.tcst_aft_com)TotalCost,sum(p.cost_percent)TotalCostPercent,sum(p.m_p)," +cs_asset+" cs_asset,"+
                TotalAssetValue_Mar+ "TotalAssetValue_Mar,p.unl_p," + cf_unlist + "NonListVal from " +
           "(select trim(c.comp_nm)comp_nm, sect_maj_nm,a.f_cd, a.sect_maj_cd, trunc(tot_nos) nos_t,  bal_dt , trunc(tcst_aft_com / tot_nos, 2) rt_acm, " +
                     "trunc(tcst_aft_com, 2) tcst_aft_com,ROUND((tot_cost / tot_nos),2) c_rt, tot_cost,round((tcst_aft_com/"+cs_asset+")*100,2) cost_percent," +
                     "nvl(a.dse_rt, 0) DSE_rate, nvl(a.cse_rt, 0)  CSE_rate, a.adc_rt m_rt, a.dse_rt* tot_nos m_p," +
                     "(a.dse_rt - trunc(tcst_aft_com / tot_nos, 2)) diff ," +
                     "(round(a.dse_rt, 2) - trunc(tcst_aft_com / tot_nos, 2)) * trunc(tot_nos) gain,round((" + cf_unlist/cs_asset+")*100,2)unl_p" +
                     " from invest.pfolio_bk a, invest.comp c ,  nav.nav_master n " +
                      " where c.comp_cd = a.comp_cd and a.f_cd =" + fundCode + " and a.bal_dt_ctrl ='" + balDate + "' and n.navfundid = " + fundCode +
                      " and n.navno = (select max(navno) from nav.nav_master where navfundid = " + fundCode + "   AND NAVDATE<='" + balDate + "' ) order by a.sect_maj_cd)p"+
                      " group by p.sect_maj_cd,p.sect_maj_nm,p.f_cd order by p.sect_maj_cd";


         
        }
        else if (string.Compare(statementType, "Portfolio (Total Asset Value)", true) == 0)
        {
            //strSQL = "select trim(c.comp_nm), sect_maj_nm, a.sect_maj_cd,trunc(tot_nos) nos_t,  bal_dt , trunc(tcst_aft_com / tot_nos, 2) rt_acm," +
            //         "trunc(tcst_aft_com, 2) tcst_aft_com,ROUND((tot_cost / tot_nos),2) c_rt, tot_cost, tcst_aft_com /" + cs_asset * 100 + ") cost_percent," +
            //         " nvl(a.dse_rt, 0) DSE_rate, nvl(a.cse_rt, 0)  CSE_rate, a.adc_rt m_rt, a.adc_rt* tot_nos m_p,(a.adc_rt * tot_nos - tcst_aft_com) diff ," +
            //         "(round(a.adc_rt, 2) - trunc(tcst_aft_com / tot_nos, 2)) * trunc(tot_nos) gain,trunc(tot_nos) / c.no_shrs * 100  paid_cap," + cs_asset + " asset," +
            //         ":cf_unlist /:cs_asset * 100 unl_p" +
            //          " from invest.pfolio_bk a, comp c, nav.nav_master n where c.comp_cd = a.comp_cd and " +
            //         " f_cd =" + fundCode + " and a.bal_dt_ctrl ='" + balDate + "' and n.navfundid ="  + fundCode  +
            //         " and navno = (select max(navno) from nav.nav_master where navfundid =" + fundCode + " AND NAVDATE<='" + balDate + "' )" +
            //          " order by c.comp_nm";
            strSQL = "";
        }
        
        else
        {
            strSQL = "select trim(c.comp_nm), sect_maj_nm, a.sect_maj_cd, trunc(tot_nos) nos_t,  bal_dt , trunc(tcst_aft_com / tot_nos, 2) rt_acm," +
                     " trunc(tcst_aft_com, 2) tcst_aft_com,    tot_cost / tot_nos c_rt, tot_cost, tcst_aft_com / n.navtotalcostprice * 100 cost_percent," +
                     " nvl(a.dse_rt, 0) DSE_rate, --nvl(a.cse_rt, 0)  CSE_rate, a.adc_rt m_rt, a.dse_rt* tot_nos m_p," +
                     " (a.dse_rt - trunc(tcst_aft_com / tot_nos, 2)) diff ,(round(a.dse_rt, 2) - trunc(tcst_aft_com / tot_nos, 2)) * trunc(tot_nos) gain," +
                     " n.navtotalcostprice asset from invest.pfolio_bk a, comp c, nav.nav_master n where c.comp_cd = a.comp_cd and" +
                      "f_cd = " + fundCode + " and a.bal_dt_ctrl ='" + balDate + "' and n.navfundid = " + fundCode  +
                      "and navno = (select max(navno) from nav.nav_master where navfundid = " + fundCode + " AND NAVDATE<='" + balDate + "'  )" +
                     "order by c.comp_nm";
           
        }
        //sbMst.Append(sbfilter.ToString());
        dtReprtSource = commonGatewayObj.Select(strSQL);
       
        if (dtReprtSource.Rows.Count > 0)
        {
            dtReprtSource.TableName = "AsstPerNAVSummaryAndPortfolio";
           // dtReprtSource.WriteXmlSchema(@"E:\iamclpfmsnew\amclpmfs\UI\ReportViewer\Report\xsdAsstPerNAVSummaryAndPortfolio.xsd");
                       
            string Path = "";
            if (string.Compare(statementType, "Summary (Total Asset Value)", true) == 0)
            {
                Path = Server.MapPath("Report/crtAssetPercSummaryTotalAssetValueReport.rpt");
            }
            else if (string.Compare(statementType, "Portfolio (Total Asset Value)", true) == 0)
            {
                Path = Server.MapPath("Report/crtAssetPercPortfolioTotalAssetValueReport.rpt");
            }
            else
            {
                Path = Server.MapPath("Report/crtAssetPercNetAssetValue.rpt");
            }

            rdoc.Load(Path);
            rdoc.SetDataSource(dtReprtSource);
            CRV_AssetPercNAVSummaryAndPortfolio.ReportSource = rdoc;
            CRV_AssetPercNAVSummaryAndPortfolio.DisplayToolbar = true;
            CRV_AssetPercNAVSummaryAndPortfolio.HasExportButton = true;
            CRV_AssetPercNAVSummaryAndPortfolio.HasPrintButton = true;
            rdoc.SetParameterValue("prmbalDate", balDate);
            rdoc.SetParameterValue("prmStatementType", statementType);
            rdoc.SetParameterValue("prmFundName", fundName);
           
            //  rdoc.SetParameterValue("prmappOrEro", appOrEro);
            rdoc = ReportFactory.GetReport(rdoc.GetType());
        }
        else
        {
            Response.Write("No Data Found");
        }
    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        CRV_AssetPercNAVSummaryAndPortfolio.Dispose();
        CRV_AssetPercNAVSummaryAndPortfolio = null;
        rdoc.Close();
        rdoc.Dispose();
        rdoc = null;
        GC.Collect();
    }
}
