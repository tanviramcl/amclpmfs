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

public partial class UI_ReportViewer_MarketValuationWithNonListedSecuritesCompanyAndAllFundsReportViewer : Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    private ReportDocument rdoc = new ReportDocument();
    string strSQL;
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder sbFilter = new StringBuilder();
        string strPortfolioAsOnDate = "";
        string fundCodes = "";
        //string companyCodes = "";
        //string percentageCheck = "";
       
        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../../Default.aspx");
        }
        else
        {
            strPortfolioAsOnDate = (string)Session["PortfolioAsOnDate"];
            fundCodes = (string)Session["fundCodes"];
            
        }
        DataTable dtReprtSource = new DataTable();
        StringBuilder sbMst = new StringBuilder();
        StringBuilder sbfilter = new StringBuilder();
        //strSQL = "select u.f_cd, u.fund_name,u.bal_dt_ctrl,u.TOTAL_COMPANY_PROFIT,u.TOTAL_SHARE_PROFIT,u.TOTAL_COST_PROFIT,u.TOTAL_MARKET_PRICE_PROFIT,u.PROFIT,u.tp_PROFIT," +
        //         " v.TOTAL_COMPANY_LOSS,v.TOTAL_SHARE_LOSS,v.TOTAL_COST_LOSS,v.TOTAL_MARKET_PRICE_LOSS,v.LOSS,v.tp_LOSS from "+
        //          "(select p.f_cd , f.f_name fund_name,TO_CHAR(bal_dt_ctrl,'dd-MON-yyyy')bal_dt_ctrl,COUNT(p.COMP_CD)TOTAL_COMPANY_PROFIT, sum(trunc(p.tot_nos)) TOTAL_SHARE_PROFIT," +
        //         "sum(p.tcst_aft_com) TOTAL_COST_PROFIT, sum(p.tot_nos * c.adc_rt) TOTAL_MARKET_PRICE_PROFIT, sum(p.tot_nos * c.adc_rt) - sum(p.tcst_aft_com)PROFIT," +
        //         "decode(sum(trunc(p.tot_nos)), 0, 'noneed', 'PROFIT') tp_PROFIT from pfolio_bk p, comp c, fund f " +
        //         " where p.bal_dt_ctrl ='" + strPortfolioAsOnDate.ToString() + "' and f.F_CD IN(" + fundCodes + ") and f.f_cd not in(3,5,18) and p.comp_cd = c.comp_cd " +
        //         " and(round(p.adc_rt, 2) - trunc(p.tcst_aft_com / tot_nos, 2)) * trunc(tot_nos) >= 0 and p.f_cd = f.f_cd " +
        //         " group by p.f_cd, bal_dt_ctrl,f.f_name)u," +
        //         " (select p.f_cd ,f.f_name,TO_CHAR(bal_dt_ctrl,'dd-MON-yyyy')bal_dt_ctrl,COUNT(p.COMP_CD) TOTAL_COMPANY_LOSS, sum(trunc(p.tot_nos)) TOTAL_SHARE_LOSS, sum(p.tcst_aft_com) TOTAL_COST_LOSS," +
        //         " sum(p.tot_nos * c.adc_rt) TOTAL_MARKET_PRICE_LOSS, sum(p.tot_nos * c.adc_rt) - sum(p.tcst_aft_com) LOSS," +
        //         " decode(sum(trunc(p.tot_nos)), 0, 'noneed', 'LOSS') tp_LOSS" +
        //         " from  pfolio_bk p, comp c ,fund f where p.bal_dt_ctrl ='" + strPortfolioAsOnDate.ToString() + "' and f.F_CD IN(" + fundCodes + ") and f.f_cd not in(3,5,18)" +
        //         " and p.comp_cd = c.comp_cd and(round(p.adc_rt, 2) - trunc(p.tcst_aft_com / tot_nos, 2)) * trunc(tot_nos) < 0 " +
        //         " and p.f_cd = f.f_cd group by p.f_cd, bal_dt_ctrl, f.f_name)v"+
        //         " where u.f_cd=v.f_cd order by u.f_cd";




        /*    select f_cd, inv_amount, inv_date
     FROM(SELECT f_cd, inv_amount,
                   inv_date,
                   rank() over(partition by f_cd order by inv_date desc) rnk
              FROM NON_LISTED_SECURITIES)
    WHERE rnk = 1  
    */
        strSQL = "select n.f_cd ,m.f_name,m.bal_dt_ctrl,m.TOTAL_COMPANY, m.TOTAL_SHARE, m.TOTAL_COST,m.TOTAL_MARKET_PRICE,m.EROSION,n.inv_amount,n.inv_date,(m.TOTAL_COST+n.inv_amount)TOT_COST_nl," +
                 "m.TOTAL_MARKET_PRICE+n.inv_amount,n.inv_amount as non_list from" +
                 "(select p.f_cd ,f.f_name,TO_CHAR(p.bal_dt_ctrl,'dd-MON-yyyy')bal_dt_ctrl,COUNT(nvl(p.COMP_CD,0)) TOTAL_COMPANY, sum(trunc(nvl(p.tot_nos,0))) TOTAL_SHARE, sum(nvl(p.tcst_aft_com,0)) TOTAL_COST," +
                  "sum(nvl(p.tot_nos,0) * nvl(p.adc_rt,0)) TOTAL_MARKET_PRICE, sum(nvl(p.tot_nos,0) * nvl(p.adc_rt,0)) - sum(nvl(p.tcst_aft_com,0)) EROSION" +
                 " from  pfolio_bk p ,fund f where p.bal_dt_ctrl ='" + strPortfolioAsOnDate.ToString() + "' and f.F_CD IN(" + fundCodes + ") " +
                 " and p.f_cd = f.f_cd group by p.f_cd, bal_dt_ctrl, f.f_name)m,(select f_cd,inv_amount,inv_date FROM (SELECT f_cd,inv_amount, inv_date," +
                 "rank() over (partition by f_cd order by inv_date desc) rnk" +
                 " FROM NON_LISTED_SECURITIES where  inv_amount>0 and F_CD IN(" + fundCodes + ") and inv_date<='" + strPortfolioAsOnDate.ToString() + "' )" +
                 " WHERE rnk = 1)n where m.f_cd(+)=n.f_cd order by n.f_cd";
    


        //strSQL = "select p.f_cd , f.f_name fund_name,bal_dt_ctrl,COUNT(p.COMP_CD)TOTAL_COMPANY, sum(trunc(p.tot_nos)) TOTAL_SHARE," +
        //           "sum(p.tcst_aft_com) TOTAL_COST, sum(p.tot_nos * c.adc_rt) TOTAL_MARKET_PRICE, sum(p.tot_nos * c.adc_rt) - sum(p.tcst_aft_com)  EROSION," +
        //           "decode(sum(trunc(p.tot_nos)), 0, 'noneed', 'PROFIT') tp from pfolio_bk p, comp c, fund f " +
        //           " where p.bal_dt_ctrl ='" + strPortfolioAsOnDate.ToString() + "' and f.F_CD IN(" + fundCodes + ") and f.f_cd not in(3,5,18) and p.comp_cd = c.comp_cd " +
        //           " and(round(p.adc_rt, 2) - trunc(p.tcst_aft_com / tot_nos, 2)) * trunc(tot_nos) >= 0 and p.f_cd = f.f_cd " +
        //           " group by p.f_cd, bal_dt_ctrl,f.f_name " +
        //           " union" +
        //           " select p.f_cd ,f.f_name,bal_dt_ctrl,COUNT(p.COMP_CD) TOTAL_COMPANY, sum(trunc(p.tot_nos)) TOTAL_SHARE, sum(p.tcst_aft_com) TOTAL_COST," +
        //           " sum(p.tot_nos * c.adc_rt) TOTAL_MARKET_PRICE, sum(p.tot_nos * c.adc_rt) - sum(p.tcst_aft_com) EROSION," +
        //           " decode(sum(trunc(p.tot_nos)), 0, 'noneed', 'LOSS') tp" +
        //           " from  pfolio_bk p, comp c ,fund f where p.bal_dt_ctrl ='" + strPortfolioAsOnDate.ToString() + "' and f.F_CD IN(" + fundCodes + ") and f.f_cd not in(3,5,18)" +
        //           " and p.comp_cd = c.comp_cd and(round(p.adc_rt, 2) - trunc(p.tcst_aft_com / tot_nos, 2)) * trunc(tot_nos) < 0 " +
        //           " and p.f_cd = f.f_cd group by p.f_cd, bal_dt_ctrl, f.f_name order by(9) desc";


        dtReprtSource = commonGatewayObj.Select(strSQL);
        dtReprtSource.TableName = "MrktValWithNonListedSecuritiesAllFunds";
       // dtReprtSource.WriteXmlSchema(@"E:\amclpmfs\UI\ReportViewer\Report\xsdMrktValWithNonListedSecuritiesAllFunds.xsd");
        if (dtReprtSource.Rows.Count > 0)
        {
            string Path = Server.MapPath("Report/crptMarketValuationWithNonListedSecuritesReport.rpt");
            rdoc.Load(Path);
            rdoc.SetDataSource(dtReprtSource);
            CRV_MarketValiationWithProfitLossReportViewer.ReportSource = rdoc;
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
        CRV_MarketValiationWithProfitLossReportViewer.Dispose();
        CRV_MarketValiationWithProfitLossReportViewer = null;
        rdoc.Close();
        rdoc.Dispose();
        rdoc = null;
        GC.Collect();
    }
}
