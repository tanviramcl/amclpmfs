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
using System.Text;
using System.Data.OracleClient;
using System.IO;
using System.Collections.Generic;
//using AMCL.DL;
//using AMCL.BL;
//using AMCL.UTILITY;
//using AMCL.GATEWAY;
//using AMCL.COMMON;


public partial class BalanceUpdateProcess : System.Web.UI.Page
{
    DropDownList dropDownListObj = new DropDownList();
    CommonGateway commonGatewayObj = new CommonGateway();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../Default.aspx");
        }
        DataTable dtBalanceDate = getbalanceDate();
        DataTable dtDate1 = GetDate1();

      //  lblProcessing.Text = "";
        if (!IsPostBack)
        {
            txtbalanceDate1.Text = dtBalanceDate.Rows[0]["balancedate1"].ToString();
            txtbalanceDate2.Text = dtBalanceDate.Rows[0]["balancedate2"].ToString();

            string Date1 = Convert.ToDateTime(dtDate1.Rows[0]["date1"]).ToString("dd-MMM-yyyy");
            string dttotalrow = GetTotalrowPortfolio(Date1);
            txttotalRowCount.Text = dttotalrow;

            lblProcessing.Text = "";
        }



    }


   

    
  
    protected void btnProcessingforBackup_Click(object sender, EventArgs e)
    {
        DataTable dtBalanceDate = getbalanceDate();
        string Bal_Date1 = Convert.ToDateTime(dtBalanceDate.Rows[0]["balancedate1"]).ToString("dd-MMM-yyyy");
        DataTable dtDateFromPfolioBk = GetDate1();
        DataTable dtRateUpdDateFromComp = GetDate2();

        string Date1 = Convert.ToDateTime(dtDateFromPfolioBk.Rows[0]["date1"]).ToString("dd-MMM-yyyy"); 
        string Date2 = Convert.ToDateTime(dtRateUpdDateFromComp.Rows[0]["date2"]).ToString("dd-MMM-yyyy");
        lblProcessing.Text = "Processing completed!!!!";

        if (Date1 == Date2)
        {
          //  ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data Already Updated! or Market Price is not current');", true);
          lblProcessing.Text= "Data Already Updated! or Market Price is not current !!!";
        }
        else {

            
            DataTable dtsource = new DataTable();
            StringBuilder sbMst = new StringBuilder();
            StringBuilder sbfilter = new StringBuilder();
            sbfilter.Append(" ");
            sbMst.Append("select a.F_CD, a.COMP_CD, a.TOT_NOS,nvl( a.TOT_COST,0) as TOT_COST, a.TCST_AFT_COM, a.BAL_DT, comp.avg_RT, nvl(comp.CSE_RT,0) as CSE_RT ,");
            sbMst.Append("comp.ADC_RT, sect_maj.SECT_MAJ_NM, sect_maj.SECT_MAJ_CD, '" + Bal_Date1 + "' as BalanceDate from comp, mprice_temp, fund_folio_hb a, sect_maj");
            sbMst.Append(" where mprice_temp.f_cd = a.f_cd and comp.comp_cd = a.comp_cd and ");
            sbMst.Append("comp.sect_maj_cd = sect_maj.sect_maj_cd and comp.comp_cd = mprice_temp.comp_cd and a.tot_nos > 0");
            sbMst.Append(sbfilter.ToString());
            dtsource = commonGatewayObj.Select(sbMst.ToString());

            List<PortFolioBk> portFoliobkdatalist = new List<PortFolioBk>();
            portFoliobkdatalist = (from DataRow dr in dtsource.Rows
                                   select new PortFolioBk()
                                   {
                                       F_CD = dr["F_CD"].ToString(),
                                       COMP_CD = dr["COMP_CD"].ToString(),
                                       TOT_NOS = dr["TOT_NOS"].ToString(),
                                       TOT_COST = dr["TOT_COST"].ToString(),
                                       TCST_AFT_COM = dr["TCST_AFT_COM"].ToString(),
                                       BAL_DT = Convert.ToDateTime(dr["BAL_DT"]).ToString("dd-MMM-yyyy"),
                                       // BAL_DT = dr["BAL_DT"].ToString(),
                                       avg_RT = dr["avg_RT"].ToString(),
                                       CSE_RT = dr["CSE_RT"].ToString(),
                                       ADC_RT = dr["ADC_RT"].ToString(),
                                       SECT_MAJ_NM = dr["SECT_MAJ_NM"].ToString(),
                                       SECT_MAJ_CD = dr["SECT_MAJ_CD"].ToString(),
                                       BalanceDate = dr["BalanceDate"].ToString()
                                   }).ToList();


            foreach (PortFolioBk pk in portFoliobkdatalist)
            {
                
                string strInsQuery = "insert into  pfolio_bk(F_CD,COMP_CD,TOT_NOS,TOT_COST,TCST_AFT_COM,BAL_DT,DSE_RT,CSE_RT,ADC_RT,SECT_MAJ_NM,SECT_MAJ_CD,BAL_DT_CTRL)values(" + Convert.ToInt32(pk.F_CD) + "," + Convert.ToInt32(pk.COMP_CD) + "," + Convert.ToDouble(pk.TOT_NOS) + "," + Convert.ToDouble(pk.TOT_COST) + "," + Convert.ToDouble(pk.TCST_AFT_COM) + ",TO_Date('" + pk.BAL_DT + "')," + Convert.ToDouble(pk.avg_RT) + "," + Convert.ToDouble(pk.CSE_RT) + ",'" + Convert.ToDouble(pk.ADC_RT) + "','" + pk.SECT_MAJ_NM.ToString() + "','" + pk.SECT_MAJ_CD + "',TO_DATE('" + pk.BalanceDate + "'))";
                int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);

            }

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data Inserted Successfully');", true);
        }
        string dttotalrow = GetTotalrowPortfolio(txtbalanceDate1.Text);
        txttotalRowCount.Text= dttotalrow;

     //   Response.Redirect("FinalProcessing.aspx");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DataTable dtDate1 = GetDate1();
        DataTable dtBalanceDate = getbalanceDate();


        string Date1 = Convert.ToDateTime(dtDate1.Rows[0]["date1"]).ToString("dd-MMM-yyyy");
        DateTime baldate1 = DateTime.ParseExact(txtbalanceDate2.Text, "dd/MM/yyyy", null);
        string dt1 = Convert.ToDateTime(baldate1).ToString("dd-MMM-yyyy");

        // int row;
        lblProcessing.Text = "Processing completed!!!!";
        if (Date1 == dt1)
        {
            string dttotalrow = GetTotalrowPortfolio(dt1);
            int row= Convert.ToInt32(dttotalrow);
            string strDelQuery = "delete from pfolio_bk where bal_dt_ctrl='"+dt1+"'";
            int NumOfRows = commonGatewayObj.ExecuteNonQuery(strDelQuery);
            //  ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Delete Successfully');", true);
            lblProcessing.Text = "Delete  Successfully";
            // txttotalRowCount.Text = Convert.ToString(row);

            ClearFields();
        }
        else
        {
            lblProcessing.Text = "Delete unsuccessfully"; 
        }
       // Response.Redirect("FinalProcessing.aspx");
        
    }
    public class PortFolioBk
    {
        public string F_CD { get; set;}
        public string COMP_CD { get; set; }
        public string TOT_NOS { get; set; }
        public string TOT_COST { get; set; }
        public string TCST_AFT_COM { get; set; }
        public string BAL_DT { get; set; }
        public string avg_RT { get; set; }
        public string CSE_RT { get; set; }
        public string ADC_RT { get; set; }
        public string SECT_MAJ_NM { get; set; }
        public string SECT_MAJ_CD { get; set; }
        public string BalanceDate { get; set; }

    }

    public DataTable getbalanceDate()//For Authorized Signatory
    {
        DataTable pdate = commonGatewayObj.Select("select max(bal_dt_ctrl)+1 as BalanceDate  from pfolio_bk");
        DataTable pdateDropDownList = new DataTable();
        pdateDropDownList.Columns.Add("balancedate1", typeof(string));
        pdateDropDownList.Columns.Add("balancedate2", typeof(string));
        DataRow dr = pdateDropDownList.NewRow();
        dr = pdateDropDownList.NewRow();
        dr["balancedate1"] = Convert.ToDateTime(pdate.Rows[0]["BalanceDate"]).ToString("dd-MMM-yyyy");
        dr["balancedate2"] = Convert.ToDateTime(pdate.Rows[0]["BalanceDate"]).ToString("dd-MMM-yyyy");
        pdateDropDownList.Rows.Add(dr);
       
        return pdateDropDownList;
    }

    public DataTable GetDate1()
    {
        DataTable maxdate1 = commonGatewayObj.Select("select max(bal_dt_ctrl) as date1 from pfolio_bk");
        DataTable Date1 = new DataTable();
        Date1.Columns.Add("date1", typeof(string));

        DataRow dr = Date1.NewRow();

        for (int loop = 0; loop < maxdate1.Rows.Count; loop++)
        {
            dr = Date1.NewRow();
            dr["date1"] = maxdate1.Rows[loop]["date1"].ToString();

            Date1.Rows.Add(dr);
        }
        return Date1;
    }
    public DataTable GetDate2()
    {
        DataTable maxdate2 = commonGatewayObj.Select("select max(rt_upd_dt) as date2  from comp");
        DataTable Date2 = new DataTable();
        Date2.Columns.Add("date2", typeof(string));

        DataRow dr = Date2.NewRow();

        for (int loop = 0; loop < maxdate2.Rows.Count; loop++)
        {
            dr = Date2.NewRow();
            dr["date2"] = maxdate2.Rows[loop]["date2"].ToString();

            Date2.Rows.Add(dr);
        }
        return Date2;
    }

    //public DataTable GetTotalrowPortfolio_bk(string date)
    //{
    //    DataTable trow = commonGatewayObj.Select("select count(*) as TotalRow from pfolio_bk where baL_dt_ctrl='"+date+ "'");
    //    DataTable txttotalrow = new DataTable();
    //    txttotalrow.Columns.Add("TOTALROW", typeof(string));

    //    DataRow dr = txttotalrow.NewRow();

    //    for (int loop = 0; loop < trow.Rows.Count; loop++)
    //    {
    //        dr = txttotalrow.NewRow();
    //        dr["TOTALROW"] = trow.Rows[loop]["TOTALROW"].ToString();

    //        txttotalrow.Rows.Add(dr);
    //    }
    //    return txttotalrow;
    //}


    public string GetTotalrowPortfolio(string date)
    {
        DataTable dtTotRowCount = commonGatewayObj.Select("select count(*) as TotalRow from pfolio_bk where baL_dt_ctrl='" + date + "'");
        string totRows = dtTotRowCount.Rows[0]["TotalRow"].ToString();
        
        return totRows;
    }

    private void ClearFields()
    {
        DataTable dtBalanceDate = getbalanceDate();
        txtbalanceDate1.Text = dtBalanceDate.Rows[0]["balancedate1"].ToString(); ;
        txtbalanceDate2.Text = dtBalanceDate.Rows[0]["balancedate2"].ToString();
        txttotalRowCount.Text = "";


    }

}
