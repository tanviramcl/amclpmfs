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



public partial class DateWiseTransaction : System.Web.UI.Page
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

        lblProcessing.Text = "";

        DataTable tblAllfundInfo = getTblAllFundInfo();
        List<TblFundInfo> tblAllfundInfolist = new List<TblFundInfo>();
       

            tblAllfundInfolist = (from DataRow dr in tblAllfundInfo.Rows
                               select new TblFundInfo()
                               {
                                   // COMP_CD = dr["COMP_CD"].ToString(),
                                   F_CD = dr["F_CD"].ToString(),
                                   F_NAME = dr["F_NAME"].ToString(),
                                   Howla_Date_From = dr["Howla_Date_From"].ToString(),
                                   Howla_LastUpdated_Date = dr["Howla_LastUpdated_Date"].ToString(),
                                   Howla_Date_To = dr["Howla_Date_To"].ToString(),
                                   Stock_Exchange = dr["Stock_Exchange"].ToString(),

                               }).ToList();
        if (tblAllfundInfolist.Count > 0)
        {
            var dtFundinfolist= tblAllfundInfolist.OrderByDescending(fund => fund.Stock_Exchange).ToList();
            grdShowDSEMP.DataSource = dtFundinfolist;
            grdShowDSEMP.DataBind();
        }

    }

  




    protected void btnProcess_Click(object sender, EventArgs e)
    {
        DataTable tblAllfundInfo = getTblAllFundInfo();
        if (tblAllfundInfo.Rows.Count > 0)
        {
          Save(tblAllfundInfo);
        }
        else
        {
            Response.Write("No Data Found");
        }
    }

    public DataTable FundNameDropDownList()//For All Funds
    {
        DataTable dtFundName = commonGatewayObj.Select("SELECT F_NAME, F_CD FROM FUND WHERE IS_F_CLOSE IS NULL AND BOID IS NOT NULL ORDER BY F_CD");
        DataTable dtFundNameDropDownList = new DataTable();
        dtFundNameDropDownList.Columns.Add("F_NAME", typeof(string));
        dtFundNameDropDownList.Columns.Add("F_CD", typeof(string));
        DataRow dr = dtFundNameDropDownList.NewRow();

        for (int loop = 0; loop < dtFundName.Rows.Count; loop++)
        {
            dr = dtFundNameDropDownList.NewRow();
            dr["F_NAME"] = dtFundName.Rows[loop]["F_NAME"].ToString();
            dr["F_CD"] = Convert.ToInt32(dtFundName.Rows[loop]["F_CD"]);
            dtFundNameDropDownList.Rows.Add(dr);
        }
        return dtFundNameDropDownList;
    }


   
    public DataTable StockList()//For All Funds
    {
        DataTable dtData = new DataTable();

        dtData = new DataTable();
        dtData.Columns.Add("Stock_Code");
        dtData.Columns.Add("Stock_Name");
        dtData.Columns.Add("Stock_Value");

        DataRow dr = dtData.NewRow();
        dr[0] = "1";
        dr[1] = "DSE";
        dr[2] = "D";
        dtData.Rows.Add(dr);
        dr = dtData.NewRow();
        dr[0] = "2";
        dr[1] = "CSE";
        dr[2] = "C";
        dtData.Rows.Add(dr);



        return dtData;
    }

    public DataTable getTblAllFundInfo()
    {
        DataTable dtFundNameDropDownList = FundNameDropDownList();

        DataTable stockExchangeList = StockList();

        DateTime? dtimeHowlaDateFrom, dtimeLastHowlaDate, dtHowlaDateto;



        string strQuery;

        DataTable dt = new DataTable();
        lblProcessing.Text = "";
        string strtxtHowlaDateFrom, strtxtLastHowlaDate, strtHowlaDateFrom, strHowlaDateTo, strLastHowlaDate, strHowlaDateto;



        DataTable tblAllfundInfo = new DataTable();
        tblAllfundInfo.Columns.Add("F_CD", typeof(int));
        tblAllfundInfo.Columns.Add("F_NAME", typeof(string));
        tblAllfundInfo.Columns.Add("Howla_Date_From", typeof(string));
        tblAllfundInfo.Columns.Add("Howla_LastUpdated_Date", typeof(string));
        tblAllfundInfo.Columns.Add("Howla_Date_To", typeof(string));
        tblAllfundInfo.Columns.Add("Stock_Exchange", typeof(string));

        for (int i = 0; i < dtFundNameDropDownList.Rows.Count; i++)
        {





            for (int j = 0; j < stockExchangeList.Rows.Count; j++)
            {
                if (stockExchangeList.Rows[j]["Stock_Value"].ToString() == "D") // For DSE
                {
                    strQuery = "select TO_CHAR(max(vch_dt),'DD-MON-YYYY')last_tr_dt,TO_CHAR(max(vch_dt) + 1,'DD-MON-YYYY')vch_dt  from fund_trans_hb where f_cd =" + dtFundNameDropDownList.Rows[i]["F_CD"].ToString() +
                         " and tran_tp in ('C','S') and stock_ex in ('D','A')";
                    dt = commonGatewayObj.Select(strQuery);
                }



                if (stockExchangeList.Rows[j]["Stock_Value"].ToString() == "C")// For CSE
                {
                    strQuery = "select TO_CHAR(max(vch_dt),'DD-MON-YYYY')last_tr_dt,TO_CHAR(max(vch_dt) + 1,'DD-MON-YYYY')vch_dt  from fund_trans_hb where f_cd =" + dtFundNameDropDownList.Rows[i]["F_CD"].ToString() +
                                    " and tran_tp in ('C','S') and stock_ex in ('C','A')";
                    dt = commonGatewayObj.Select(strQuery);
                }

                if (dt != null && dt.Rows.Count > 0)
                {


                    if (!dt.Rows[0].IsNull("vch_dt"))
                    {

                        strtxtHowlaDateFrom = dt.Rows[0]["vch_dt"].ToString();
                        strtxtLastHowlaDate = dt.Rows[0]["last_tr_dt"].ToString();

                        if (!string.IsNullOrEmpty(strtxtHowlaDateFrom))
                        {
                            dtimeHowlaDateFrom = Convert.ToDateTime(strtxtHowlaDateFrom);

                            strtHowlaDateFrom = dtimeHowlaDateFrom.Value.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            dtimeHowlaDateFrom = null;
                            strtHowlaDateFrom = "";
                        }

                        if (!string.IsNullOrEmpty(strtxtLastHowlaDate))
                        {
                            dtimeLastHowlaDate = Convert.ToDateTime(strtxtLastHowlaDate);

                            strLastHowlaDate = dtimeHowlaDateFrom.Value.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            dtimeLastHowlaDate = null;
                            strLastHowlaDate = "";
                        }


                        dtHowlaDateto = DateTime.Now;

                        strHowlaDateto = Convert.ToDateTime(dtHowlaDateto).ToString("dd-MMM-yyyy");

                        if (!string.IsNullOrEmpty(strHowlaDateto))
                        {

                            strHowlaDateTo = strHowlaDateto;
                        }

                        tblAllfundInfo.Rows.Add(Convert.ToInt32(dtFundNameDropDownList.Rows[i]["F_CD"].ToString()), dtFundNameDropDownList.Rows[i]["F_NAME"].ToString(), strtHowlaDateFrom, strtxtLastHowlaDate, strHowlaDateto, stockExchangeList.Rows[j]["Stock_Value"].ToString());

                    }

                    else
                    {

                        strtxtHowlaDateFrom = "";
                        strtxtLastHowlaDate = "";
                        strHowlaDateTo = "";
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('This Trading Date not found  else.');", true);
                    }

                }


            }


        }
        return tblAllfundInfo;
    }

    public void Save(DataTable tblAllfundInfo)
    {
        string strSleFromHowlaQuery, strSelFromFundTransHBQuery, strHowlaDateFrom, strLastHowlaDate, strHowlaDateTo, strSelFundofTwentyFivePaisa, strSelFundofTwentyPaisa, LoginID = Session["UserID"].ToString();
        char tp;
       
        Double amt, amt_cm = 0;


        string strlblProcessing = "", strProcessing="";

        string LoginName = Session["UserName"].ToString().ToUpper();
        CommonGateway commonGatewayObj = new CommonGateway();

        DateTime dtimeHowlaDateFrom, dtimeLastHowlaDate, dtimeHowlaDateTo;

      

        for (int l = 0; l < tblAllfundInfo.Rows.Count; l++)
        {

          
            try
            {
                dtimeHowlaDateFrom = Convert.ToDateTime(tblAllfundInfo.Rows[l]["Howla_Date_From"].ToString());
                strHowlaDateFrom = dtimeHowlaDateFrom.ToString("dd-MMM-yyyy");
                dtimeLastHowlaDate = Convert.ToDateTime(tblAllfundInfo.Rows[l]["Howla_LastUpdated_Date"].ToString());
                strLastHowlaDate = dtimeLastHowlaDate.ToString("dd-MMM-yyyy");
                dtimeHowlaDateTo = Convert.ToDateTime(tblAllfundInfo.Rows[l]["Howla_Date_To"].ToString());
                strHowlaDateTo = dtimeHowlaDateTo.ToString("dd-MMM-yyyy");

                /*
                 Here, 
                 dtimeHowlaDateFrom = Maximum voucher date from fund_trans_hb+1;
                 dtimeLastHowlaDate = Maximum voucher date from fund_trans_hb;
                 dtimeHowlaDateTo = Current date from server where oracle is installed.

                 */



                if ((dtimeHowlaDateFrom < dtimeLastHowlaDate) || (dtimeHowlaDateTo <= dtimeLastHowlaDate))
                {
                   strProcessing = "This Trading Date is already allocated !!";

                    if (!string.IsNullOrEmpty(strProcessing))
                    {
                        strlblProcessing = strProcessing;
                    }
                    else
                    {
                        strlblProcessing = "";
                    }
                }
                else
                {

                    DataTable dtFromHowla = new DataTable();
                    DataTable dtFromFundTrans = new DataTable();

                    if (tblAllfundInfo.Rows[l]["Stock_Exchange"].ToString() == "D")   // For DSE
                    {

                        strSleFromHowlaQuery = "select TO_CHAR(sp_date,'DD-MON-YYYY')sp_date, f_cd, comp_cd, in_out, sum(sp_qty)qty, substr(bk_cd, 1, 1) brk," +
                          " sum(sp_qty * sp_rate) amt, ROUND((sum(sp_qty * sp_rate) / sum(sp_qty)),2) rate from howla where sp_date between '" + strHowlaDateFrom +
                          "' and '" + strHowlaDateTo + "' and f_cd =" + tblAllfundInfo.Rows[l]["F_CD"].ToString() + " group by sp_date, f_cd, comp_cd, in_out, substr(bk_cd,1, 1) order by sp_date,f_cd,comp_cd";
                        dtFromHowla = commonGatewayObj.Select(strSleFromHowlaQuery);
                    }

                    else if (tblAllfundInfo.Rows[l]["Stock_Exchange"].ToString() == "C")  // For CSE
                    {

                        strSleFromHowlaQuery = "select TO_CHAR(sp_date,'DD-MON-YYYY')sp_date, f_cd, comp_cd, in_out, sum(sp_qty)qty, substr(bk_cd, 1, 1) brk," +
                        " sum(sp_qty * sp_rate) amt, ROUND((sum(sp_qty * sp_rate) / sum(sp_qty)),2) rate from howla_cse where sp_date between '" + strHowlaDateFrom +
                        "' and '" + strHowlaDateTo + "' and f_cd =" + tblAllfundInfo.Rows[l]["F_CD"].ToString() + " group by sp_date, f_cd, comp_cd, in_out, substr(bk_cd,1, 1) order by sp_date,f_cd,comp_cd";
                        dtFromHowla = commonGatewayObj.Select(strSleFromHowlaQuery);
                    }
                    //else // For new stock exchange
                    //{
                    //    strSleFromHowlaQuery = ""; // SQL Query must be added here
                    //    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('This stock exchange is not added yet.');", true);
                    //}



                    DataTable dtFundofTwentyFivePaisa = new DataTable();
                    strSelFundofTwentyFivePaisa = "select f_cd,f_name,sl_buy_com_pct from fund where sl_buy_com_pct=0.25 order by f_cd";
                    dtFundofTwentyFivePaisa = commonGatewayObj.Select(strSelFundofTwentyFivePaisa);

                    List<int> fundListofTwentyFivePaisa = (from row in dtFundofTwentyFivePaisa.AsEnumerable()
                                                           select Convert.ToInt32(row["f_cd"].ToString())).ToList();


                    DataTable dtFundofTwentyPaisa = new DataTable();
                    strSelFundofTwentyPaisa = "select f_cd,f_name,sl_buy_com_pct from fund where sl_buy_com_pct=0.20 order by f_cd";
                    dtFundofTwentyPaisa = commonGatewayObj.Select(strSelFundofTwentyPaisa);

                    List<int> fundListofTwentyPaisa = (from row in dtFundofTwentyPaisa.AsEnumerable()
                                                       select Convert.ToInt32(row["f_cd"].ToString())).ToList();



                    if (dtFromHowla != null && dtFromHowla.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtFromHowla.Rows.Count; i++)
                        {
                            if (tblAllfundInfo.Rows[l]["Stock_Exchange"].ToString() == "D")
                            {

                                // For editing purposes


                                // For editing


                                //  if (dtFromHowla.Rows[i]["in_out"].ToString() == "I" && (dtFromHowla.Rows[i]["f_cd"].ToString() == "1" || dtFromHowla.Rows[i]["f_cd"].ToString() == "2" || dtFromHowla.Rows[i]["f_cd"].ToString() == "4" || dtFromHowla.Rows[i]["f_cd"].ToString() == "6" || dtFromHowla.Rows[i]["f_cd"].ToString() == "7" || dtFromHowla.Rows[i]["f_cd"].ToString() == "8" || dtFromHowla.Rows[i]["f_cd"].ToString() == "9" || dtFromHowla.Rows[i]["f_cd"].ToString() == "10" || dtFromHowla.Rows[i]["f_cd"].ToString() == "11" || dtFromHowla.Rows[i]["f_cd"].ToString() == "12" || dtFromHowla.Rows[i]["f_cd"].ToString() == "13" || dtFromHowla.Rows[i]["f_cd"].ToString() == "20"))

                                if (dtFromHowla.Rows[i]["in_out"].ToString() == "I" && fundListofTwentyFivePaisa.Contains(Convert.ToInt32(dtFromHowla.Rows[i]["f_cd"].ToString())))
                                {
                                    tp = 'C';
                                    amt = Convert.ToDouble(dtFromHowla.Rows[i]["amt"].ToString());
                                    //  temp = amt * (1 + 0.0025);
                                    //  amt_cm = amt * (1 + 0.0025);
                                    amt_cm = amt * (1 + Convert.ToDouble(dtFundofTwentyFivePaisa.Rows[0]["sl_buy_com_pct"].ToString()) / 100);

                                    strSelFromFundTransHBQuery = "select f_cd, comp_cd from fund_trans_hb where vch_dt ='" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd =" + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                      " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('D','A')";
                                    dtFromFundTrans = commonGatewayObj.Select(strSelFromFundTransHBQuery);

                                    if (dtFromFundTrans != null && dtFromFundTrans.Rows.Count > 0)
                                    {
                                        string strUPdQuery = "update fund_trans_hb set no_share =" + dtFromHowla.Rows[i]["qty"].ToString() + ",  rate =" + dtFromHowla.Rows[i]["rate"].ToString() + ",amount =" + dtFromHowla.Rows[i]["amt"].ToString() + ", stock_ex ='" + dtFromHowla.Rows[i]["brk"].ToString() + "'" +
                                              ",vch_no='" + txtVoucherNumber.Text.Replace("'", "''") + "'," +
                                              " amt_aft_com =" + amt_cm + " where vch_dt = '" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd = " + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                          " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('D','A')";

                                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQuery);
                                        //  ClearFields();
                                        //  ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data updated!');", true);
                                    }



                                    else
                                    {

                                        string strInsQuery = "insert into fund_trans_hb(vch_dt, f_cd, comp_cd, vch_no," +
                                        " tran_tp, no_share, rate, amount, stock_ex, amt_aft_com,op_name) values('" + dtFromHowla.Rows[i]["sp_date"].ToString() + "'," + dtFromHowla.Rows[i]["f_cd"].ToString() + "," + dtFromHowla.Rows[i]["comp_cd"].ToString() +
                                         ",'" + txtVoucherNumber.Text.Replace("'", "''") + "'," +
                                       " decode('" + dtFromHowla.Rows[i]["in_out"].ToString() + "', 'I', 'C', 'O', 'S')," + dtFromHowla.Rows[i]["qty"].ToString() + "," + dtFromHowla.Rows[i]["rate"].ToString() + "," + dtFromHowla.Rows[i]["amt"].ToString() + ",'" + dtFromHowla.Rows[i]["brk"].ToString() + "'," + amt_cm + ",'" + LoginID + "')";

                                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                                        //ClearFields();
                                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);

                                    }
                                }

                                // else if (dtFromHowla.Rows[i]["in_out"].ToString() == "O" && (dtFromHowla.Rows[i]["f_cd"].ToString() == "1" || dtFromHowla.Rows[i]["f_cd"].ToString() == "2" || dtFromHowla.Rows[i]["f_cd"].ToString() == "4" || dtFromHowla.Rows[i]["f_cd"].ToString() == "6" || dtFromHowla.Rows[i]["f_cd"].ToString() == "7" || dtFromHowla.Rows[i]["f_cd"].ToString() == "8" || dtFromHowla.Rows[i]["f_cd"].ToString() == "9" || dtFromHowla.Rows[i]["f_cd"].ToString() == "10" || dtFromHowla.Rows[i]["f_cd"].ToString() == "11" || dtFromHowla.Rows[i]["f_cd"].ToString() == "12" || dtFromHowla.Rows[i]["f_cd"].ToString() == "13" || dtFromHowla.Rows[i]["f_cd"].ToString() == "20"))
                                else if (dtFromHowla.Rows[i]["in_out"].ToString() == "O" && fundListofTwentyFivePaisa.Contains(Convert.ToInt32(dtFromHowla.Rows[i]["f_cd"].ToString())))
                                {

                                    tp = 'S';
                                    amt = Convert.ToDouble(dtFromHowla.Rows[i]["amt"].ToString());
                                    //amt_cm = amt * (1 - 0.0025);
                                    //temp= amt * (1 - 0.0025);
                                    amt_cm = amt * (1 - Convert.ToDouble(dtFundofTwentyFivePaisa.Rows[0]["sl_buy_com_pct"].ToString()) / 100);

                                    strSelFromFundTransHBQuery = "select f_cd, comp_cd from fund_trans_hb where vch_dt ='" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd =" + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                      " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('D','A')";
                                    dtFromFundTrans = commonGatewayObj.Select(strSelFromFundTransHBQuery);
                                    if (dtFromFundTrans != null && dtFromFundTrans.Rows.Count > 0)
                                    {
                                        string strUPdQuery = "update fund_trans_hb set no_share =" + dtFromHowla.Rows[i]["qty"].ToString() + ",  rate =" + dtFromHowla.Rows[i]["rate"].ToString() + ",amount =" + dtFromHowla.Rows[i]["amt"].ToString() + ", stock_ex ='" + dtFromHowla.Rows[i]["brk"].ToString() + "'" +
                                             ",vch_no='" + txtVoucherNumber.Text.Replace("'", "''") + "'," +
                                              " amt_aft_com =" + amt_cm + " where vch_dt = '" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd = " + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                          " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('D','A')";

                                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQuery);
                                        //ClearFields();
                                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data updated!');", true);
                                    }

                                    else
                                    {


                                        string strInsQuery = "insert into fund_trans_hb(vch_dt, f_cd, comp_cd,vch_no," +
                                        " tran_tp, no_share, rate, amount, stock_ex, amt_aft_com,op_name) values('" + dtFromHowla.Rows[i]["sp_date"].ToString() + "'," + dtFromHowla.Rows[i]["f_cd"].ToString() + "," + dtFromHowla.Rows[i]["comp_cd"].ToString() +
                                         ",'" + txtVoucherNumber.Text.Replace("'", "''") + "'," +
                                       " decode('" + dtFromHowla.Rows[i]["in_out"].ToString() + "', 'I', 'C', 'O', 'S')," + dtFromHowla.Rows[i]["qty"].ToString() + "," + dtFromHowla.Rows[i]["rate"].ToString() + "," + dtFromHowla.Rows[i]["amt"].ToString() + ",'" + dtFromHowla.Rows[i]["brk"].ToString() + "'," + amt_cm + ",'" + LoginID + "')";

                                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                                        //ClearFields();
                                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);
                                    }
                                }

                                // else if (dtFromHowla.Rows[i]["in_out"].ToString() == "I" && (dtFromHowla.Rows[i]["f_cd"].ToString() == "14" || dtFromHowla.Rows[i]["f_cd"].ToString() == "15" || dtFromHowla.Rows[i]["f_cd"].ToString() == "16" || dtFromHowla.Rows[i]["f_cd"].ToString() == "17" || dtFromHowla.Rows[i]["f_cd"].ToString() == "18" || dtFromHowla.Rows[i]["f_cd"].ToString() == "19" || dtFromHowla.Rows[i]["f_cd"].ToString() == "20" || dtFromHowla.Rows[i]["f_cd"].ToString() == "21" || dtFromHowla.Rows[i]["f_cd"].ToString() == "22" || dtFromHowla.Rows[i]["f_cd"].ToString() == "23" || dtFromHowla.Rows[i]["f_cd"].ToString() == "24" || dtFromHowla.Rows[i]["f_cd"].ToString() == "25" || dtFromHowla.Rows[i]["f_cd"].ToString() == "26" || dtFromHowla.Rows[i]["f_cd"].ToString() == "27" || dtFromHowla.Rows[i]["f_cd"].ToString() == "28" || dtFromHowla.Rows[i]["f_cd"].ToString() == "29" || dtFromHowla.Rows[i]["f_cd"].ToString() == "30"))
                                else if (dtFromHowla.Rows[i]["in_out"].ToString() == "I" && fundListofTwentyPaisa.Contains(Convert.ToInt32(dtFromHowla.Rows[i]["f_cd"].ToString())))
                                {

                                    tp = 'C';
                                    amt = Convert.ToDouble(dtFromHowla.Rows[i]["amt"].ToString());
                                    // amt_cm = amt * (1 + 0.0020);
                                    amt_cm = amt * (1 + Convert.ToDouble(dtFundofTwentyPaisa.Rows[0]["sl_buy_com_pct"].ToString()) / 100);

                                    strSelFromFundTransHBQuery = "select f_cd, comp_cd from fund_trans_hb where vch_dt ='" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd =" + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                      " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('D','A')";
                                    dtFromFundTrans = commonGatewayObj.Select(strSelFromFundTransHBQuery);
                                    if (dtFromFundTrans.Rows.Count > 0)
                                    {
                                        string strUPdQuery = "update fund_trans_hb set no_share =" + dtFromHowla.Rows[i]["qty"].ToString() + ",  rate =" + dtFromHowla.Rows[i]["rate"].ToString() + ",amount =" + dtFromHowla.Rows[i]["amt"].ToString() + ", stock_ex ='" + dtFromHowla.Rows[i]["brk"].ToString() + "'" +
                                             ",vch_no='" + txtVoucherNumber.Text.Replace("'", "''") + "'," +
                                              " amt_aft_com =" + amt_cm + " where vch_dt = '" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd = " + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                          " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('D','A')";

                                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQuery);
                                        //ClearFields();
                                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data updated!');", true);
                                    }

                                    else
                                    {


                                        string strInsQuery = "insert into fund_trans_hb(vch_dt, f_cd, comp_cd,vch_no," +
                                        " tran_tp, no_share, rate, amount, stock_ex, amt_aft_com,op_name) values('" + dtFromHowla.Rows[i]["sp_date"].ToString() + "'," + dtFromHowla.Rows[i]["f_cd"].ToString() + "," + dtFromHowla.Rows[i]["comp_cd"].ToString() +
                                         ",'" + txtVoucherNumber.Text.Replace("'", "''") + "'," +
                                       " decode('" + dtFromHowla.Rows[i]["in_out"].ToString() + "', 'I', 'C', 'O', 'S')," + dtFromHowla.Rows[i]["qty"].ToString() + "," + dtFromHowla.Rows[i]["rate"].ToString() + "," + dtFromHowla.Rows[i]["amt"].ToString() + ",'" + dtFromHowla.Rows[i]["brk"].ToString() + "'," + amt_cm + ",'" + LoginID + "')";

                                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                                        //ClearFields();
                                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);
                                    }
                                }

                                // else if (dtFromHowla.Rows[i]["in_out"].ToString() == "O" && (dtFromHowla.Rows[i]["f_cd"].ToString() == "14" || dtFromHowla.Rows[i]["f_cd"].ToString() == "15" || dtFromHowla.Rows[i]["f_cd"].ToString() == "16" || dtFromHowla.Rows[i]["f_cd"].ToString() == "17" || dtFromHowla.Rows[i]["f_cd"].ToString() == "18" || dtFromHowla.Rows[i]["f_cd"].ToString() == "19" || dtFromHowla.Rows[i]["f_cd"].ToString() == "20" || dtFromHowla.Rows[i]["f_cd"].ToString() == "21" || dtFromHowla.Rows[i]["f_cd"].ToString() == "22" || dtFromHowla.Rows[i]["f_cd"].ToString() == "23" || dtFromHowla.Rows[i]["f_cd"].ToString() == "24" || dtFromHowla.Rows[i]["f_cd"].ToString() == "25" || dtFromHowla.Rows[i]["f_cd"].ToString() == "26" || dtFromHowla.Rows[i]["f_cd"].ToString() == "27" || dtFromHowla.Rows[i]["f_cd"].ToString() == "28" || dtFromHowla.Rows[i]["f_cd"].ToString() == "29" || dtFromHowla.Rows[i]["f_cd"].ToString() == "30"))
                                else if (dtFromHowla.Rows[i]["in_out"].ToString() == "O" && fundListofTwentyPaisa.Contains(Convert.ToInt32(dtFromHowla.Rows[i]["f_cd"].ToString())))
                                {

                                    tp = 'S';
                                    amt = Convert.ToDouble(dtFromHowla.Rows[i]["amt"].ToString());
                                    // amt_cm = amt * (1 - 0.0020);
                                    amt_cm = amt * (1 - Convert.ToDouble(dtFundofTwentyPaisa.Rows[0]["sl_buy_com_pct"].ToString()) / 100);

                                    strSelFromFundTransHBQuery = "select f_cd, comp_cd from fund_trans_hb where vch_dt ='" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd =" + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                      " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('D','A')";

                                    dtFromFundTrans = commonGatewayObj.Select(strSelFromFundTransHBQuery);
                                    if (dtFromFundTrans.Rows.Count > 0)
                                    {
                                        string strUPdQuery = "update fund_trans_hb set no_share =" + dtFromHowla.Rows[i]["qty"].ToString() + ",  rate =" + dtFromHowla.Rows[i]["rate"].ToString() + ",amount =" + dtFromHowla.Rows[i]["amt"].ToString() + ", stock_ex ='" + dtFromHowla.Rows[i]["brk"].ToString() + "'" +
                                             ",vch_no='" + txtVoucherNumber.Text.Replace("'", "''") + "'," +
                                              " amt_aft_com =" + amt_cm + " where vch_dt = '" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd = " + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                          " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('D','A')";

                                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQuery);
                                        //ClearFields();
                                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data updated!');", true);
                                    }

                                    else
                                    {

                                        string strInsQuery = "insert into fund_trans_hb(vch_dt, f_cd, comp_cd,vch_no," +
                                        " tran_tp, no_share, rate, amount, stock_ex, amt_aft_com,op_name) values('" + dtFromHowla.Rows[i]["sp_date"].ToString() + "'," + dtFromHowla.Rows[i]["f_cd"].ToString() + "," + dtFromHowla.Rows[i]["comp_cd"].ToString() + "," +
                                         " '" + txtVoucherNumber.Text.Replace("'", "''") + "'," +
                                       " decode('" + dtFromHowla.Rows[i]["in_out"].ToString() + "', 'I', 'C', 'O', 'S')," + dtFromHowla.Rows[i]["qty"].ToString() + "," + dtFromHowla.Rows[i]["rate"].ToString() + "," + dtFromHowla.Rows[i]["amt"].ToString() + ",'" + dtFromHowla.Rows[i]["brk"].ToString() + "'," + amt_cm + ",'" + LoginID + "')";

                                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                                        //ClearFields();
                                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);

                                    }
                                }


                            }

                            if (tblAllfundInfo.Rows[l]["Stock_Exchange"].ToString() == "C")
                            {

                                // For editing purposes





                                //  if (dtFromHowla.Rows[i]["in_out"].ToString() == "I" && (dtFromHowla.Rows[i]["f_cd"].ToString() == "1" || dtFromHowla.Rows[i]["f_cd"].ToString() == "2" || dtFromHowla.Rows[i]["f_cd"].ToString() == "4" || dtFromHowla.Rows[i]["f_cd"].ToString() == "6" || dtFromHowla.Rows[i]["f_cd"].ToString() == "7" || dtFromHowla.Rows[i]["f_cd"].ToString() == "8" || dtFromHowla.Rows[i]["f_cd"].ToString() == "9" || dtFromHowla.Rows[i]["f_cd"].ToString() == "10" || dtFromHowla.Rows[i]["f_cd"].ToString() == "11" || dtFromHowla.Rows[i]["f_cd"].ToString() == "12" || dtFromHowla.Rows[i]["f_cd"].ToString() == "13" || dtFromHowla.Rows[i]["f_cd"].ToString() == "20"))

                                if (dtFromHowla.Rows[i]["in_out"].ToString() == "I" && fundListofTwentyFivePaisa.Contains(Convert.ToInt32(dtFromHowla.Rows[i]["f_cd"].ToString())))
                                {
                                    tp = 'C';
                                    amt = Convert.ToDouble(dtFromHowla.Rows[i]["amt"].ToString());
                                    // temp = amt * (1 + 0.0025);
                                    //  amt_cm = amt * (1 + 0.0025);
                                    amt_cm = amt * (1 + Convert.ToDouble(dtFundofTwentyFivePaisa.Rows[0]["sl_buy_com_pct"].ToString()) / 100);

                                    strSelFromFundTransHBQuery = "select f_cd, comp_cd from fund_trans_hb where vch_dt ='" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd =" + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                      " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('C','A')";
                                    dtFromFundTrans = commonGatewayObj.Select(strSelFromFundTransHBQuery);
                                    if (dtFromFundTrans.Rows.Count > 0)
                                    {
                                        string strUPdQuery = "update fund_trans_hb set no_share =" + dtFromHowla.Rows[i]["qty"].ToString() + ",  rate =" + dtFromHowla.Rows[i]["rate"].ToString() + ",amount =" + dtFromHowla.Rows[i]["amt"].ToString() + ", stock_ex ='" + dtFromHowla.Rows[i]["brk"].ToString() + "'" +
                                              ", vch_no='" + txtVoucherNumber.Text.Replace("'", "''") + "'," +
                                              " amt_aft_com =" + amt_cm + " where vch_dt = '" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd = " + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                          " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('C','A')";

                                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQuery);
                                        //ClearFields();
                                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data updated!');", true);
                                    }

                                    else
                                    {

                                        string strInsQuery = "insert into fund_trans_hb(vch_dt, f_cd, comp_cd,vch_no," +
                                        " tran_tp, no_share, rate, amount, stock_ex, amt_aft_com,op_name) values('" + dtFromHowla.Rows[i]["sp_date"].ToString() + "'," + dtFromHowla.Rows[i]["f_cd"].ToString() + "," + dtFromHowla.Rows[i]["comp_cd"].ToString() +
                                       ", '" + txtVoucherNumber.Text.Replace("'", "''") + "'," +
                                       " decode('" + dtFromHowla.Rows[i]["in_out"].ToString() + "', 'I', 'C', 'O', 'S')," + dtFromHowla.Rows[i]["qty"].ToString() + "," + dtFromHowla.Rows[i]["rate"].ToString() + "," + dtFromHowla.Rows[i]["amt"].ToString() + ",'" + dtFromHowla.Rows[i]["brk"].ToString() + "'," + amt_cm + ",'" + LoginID + "')";

                                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                                        //ClearFields();
                                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);

                                    }
                                }

                                // else if (dtFromHowla.Rows[i]["in_out"].ToString() == "O" && (dtFromHowla.Rows[i]["f_cd"].ToString() == "1" || dtFromHowla.Rows[i]["f_cd"].ToString() == "2" || dtFromHowla.Rows[i]["f_cd"].ToString() == "4" || dtFromHowla.Rows[i]["f_cd"].ToString() == "6" || dtFromHowla.Rows[i]["f_cd"].ToString() == "7" || dtFromHowla.Rows[i]["f_cd"].ToString() == "8" || dtFromHowla.Rows[i]["f_cd"].ToString() == "9" || dtFromHowla.Rows[i]["f_cd"].ToString() == "10" || dtFromHowla.Rows[i]["f_cd"].ToString() == "11" || dtFromHowla.Rows[i]["f_cd"].ToString() == "12" || dtFromHowla.Rows[i]["f_cd"].ToString() == "13" || dtFromHowla.Rows[i]["f_cd"].ToString() == "20"))
                                else if (dtFromHowla.Rows[i]["in_out"].ToString() == "O" && fundListofTwentyFivePaisa.Contains(Convert.ToInt32(dtFromHowla.Rows[i]["f_cd"].ToString())))
                                {

                                    tp = 'S';
                                    amt = Convert.ToDouble(dtFromHowla.Rows[i]["amt"].ToString());
                                    //amt_cm = amt * (1 - 0.0025);
                                    //temp= amt * (1 - 0.0025);
                                    amt_cm = amt * (1 - Convert.ToDouble(dtFundofTwentyFivePaisa.Rows[0]["sl_buy_com_pct"].ToString()) / 100);

                                    strSelFromFundTransHBQuery = "select f_cd, comp_cd from fund_trans_hb where vch_dt ='" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd =" + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                      " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('C','A')";
                                    dtFromFundTrans = commonGatewayObj.Select(strSelFromFundTransHBQuery);
                                    if (dtFromFundTrans.Rows.Count > 0)
                                    {
                                        string strUPdQuery = "update fund_trans_hb set no_share =" + dtFromHowla.Rows[i]["qty"].ToString() + ",  rate =" + dtFromHowla.Rows[i]["rate"].ToString() + ",amount =" + dtFromHowla.Rows[i]["amt"].ToString() + ", stock_ex ='" + dtFromHowla.Rows[i]["brk"].ToString() + "'" +
                                             ",vch_no='" + txtVoucherNumber.Text.Replace("'", "''") + "'," +
                                              " amt_aft_com =" + amt_cm + " where vch_dt = '" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd = " + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                          " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('C','A')";

                                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQuery);
                                        //ClearFields();
                                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data updated!');", true);
                                    }

                                    else
                                    {


                                        string strInsQuery = "insert into fund_trans_hb(vch_dt, f_cd, comp_cd,vch_no," +
                                        " tran_tp, no_share, rate, amount, stock_ex, amt_aft_com,op_name) values('" + dtFromHowla.Rows[i]["sp_date"].ToString() + "'," + dtFromHowla.Rows[i]["f_cd"].ToString() + "," + dtFromHowla.Rows[i]["comp_cd"].ToString() + "," +
                                         " '" + txtVoucherNumber.Text.Replace("'", "''") + "'," +
                                       " decode('" + dtFromHowla.Rows[i]["in_out"].ToString() + "', 'I', 'C', 'O', 'S')," + dtFromHowla.Rows[i]["qty"].ToString() + "," + dtFromHowla.Rows[i]["rate"].ToString() + "," + dtFromHowla.Rows[i]["amt"].ToString() + ",'" + dtFromHowla.Rows[i]["brk"].ToString() + "'," + amt_cm + ",'" + LoginID + "')";

                                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                                        //ClearFields();
                                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);
                                    }
                                }

                                // else if (dtFromHowla.Rows[i]["in_out"].ToString() == "I" && (dtFromHowla.Rows[i]["f_cd"].ToString() == "14" || dtFromHowla.Rows[i]["f_cd"].ToString() == "15" || dtFromHowla.Rows[i]["f_cd"].ToString() == "16" || dtFromHowla.Rows[i]["f_cd"].ToString() == "17" || dtFromHowla.Rows[i]["f_cd"].ToString() == "18" || dtFromHowla.Rows[i]["f_cd"].ToString() == "19" || dtFromHowla.Rows[i]["f_cd"].ToString() == "20" || dtFromHowla.Rows[i]["f_cd"].ToString() == "21" || dtFromHowla.Rows[i]["f_cd"].ToString() == "22" || dtFromHowla.Rows[i]["f_cd"].ToString() == "23" || dtFromHowla.Rows[i]["f_cd"].ToString() == "24" || dtFromHowla.Rows[i]["f_cd"].ToString() == "25" || dtFromHowla.Rows[i]["f_cd"].ToString() == "26" || dtFromHowla.Rows[i]["f_cd"].ToString() == "27" || dtFromHowla.Rows[i]["f_cd"].ToString() == "28" || dtFromHowla.Rows[i]["f_cd"].ToString() == "29" || dtFromHowla.Rows[i]["f_cd"].ToString() == "30"))
                                else if (dtFromHowla.Rows[i]["in_out"].ToString() == "I" && fundListofTwentyPaisa.Contains(Convert.ToInt32(dtFromHowla.Rows[i]["f_cd"].ToString())))
                                {

                                    tp = 'C';
                                    amt = Convert.ToDouble(dtFromHowla.Rows[i]["amt"].ToString());
                                    // amt_cm = amt * (1 + 0.0020);
                                    amt_cm = amt * (1 + Convert.ToDouble(dtFundofTwentyPaisa.Rows[0]["sl_buy_com_pct"].ToString()) / 100);

                                    strSelFromFundTransHBQuery = "select f_cd, comp_cd from fund_trans_hb where vch_dt ='" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd =" + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                      " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('C','A')";
                                    dtFromFundTrans = commonGatewayObj.Select(strSelFromFundTransHBQuery);
                                    if (dtFromFundTrans.Rows.Count > 0)
                                    {
                                        string strUPdQuery = "update fund_trans_hb set no_share =" + dtFromHowla.Rows[i]["qty"].ToString() + ",  rate =" + dtFromHowla.Rows[i]["rate"].ToString() + ",amount =" + dtFromHowla.Rows[i]["amt"].ToString() + ", stock_ex ='" + dtFromHowla.Rows[i]["brk"].ToString() + "'" +
                                             ", vch_no='" + txtVoucherNumber.Text.Replace("'", "''") + "'," +
                                              " amt_aft_com =" + amt_cm + " where vch_dt = '" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd = " + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                          " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('C','A')";

                                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQuery);
                                        //ClearFields();
                                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data updated!');", true);
                                    }

                                    else
                                    {


                                        string strInsQuery = "insert into fund_trans_hb(vch_dt, f_cd, comp_cd,vch_no," +
                                        " tran_tp, no_share, rate, amount, stock_ex, amt_aft_com,op_name) values('" + dtFromHowla.Rows[i]["sp_date"].ToString() + "'," + dtFromHowla.Rows[i]["f_cd"].ToString() + "," + dtFromHowla.Rows[i]["comp_cd"].ToString() + "," +
                                         " '" + txtVoucherNumber.Text.Replace("'", "''") + "'," +
                                       " decode('" + dtFromHowla.Rows[i]["in_out"].ToString() + "', 'I', 'C', 'O', 'S')," + dtFromHowla.Rows[i]["qty"].ToString() + "," + dtFromHowla.Rows[i]["rate"].ToString() + "," + dtFromHowla.Rows[i]["amt"].ToString() + ",'" + dtFromHowla.Rows[i]["brk"].ToString() + "'," + amt_cm + ",'" + LoginID + "')";

                                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                                        //ClearFields();
                                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);
                                    }
                                }

                                // else if (dtFromHowla.Rows[i]["in_out"].ToString() == "O" && (dtFromHowla.Rows[i]["f_cd"].ToString() == "14" || dtFromHowla.Rows[i]["f_cd"].ToString() == "15" || dtFromHowla.Rows[i]["f_cd"].ToString() == "16" || dtFromHowla.Rows[i]["f_cd"].ToString() == "17" || dtFromHowla.Rows[i]["f_cd"].ToString() == "18" || dtFromHowla.Rows[i]["f_cd"].ToString() == "19" || dtFromHowla.Rows[i]["f_cd"].ToString() == "20" || dtFromHowla.Rows[i]["f_cd"].ToString() == "21" || dtFromHowla.Rows[i]["f_cd"].ToString() == "22" || dtFromHowla.Rows[i]["f_cd"].ToString() == "23" || dtFromHowla.Rows[i]["f_cd"].ToString() == "24" || dtFromHowla.Rows[i]["f_cd"].ToString() == "25" || dtFromHowla.Rows[i]["f_cd"].ToString() == "26" || dtFromHowla.Rows[i]["f_cd"].ToString() == "27" || dtFromHowla.Rows[i]["f_cd"].ToString() == "28" || dtFromHowla.Rows[i]["f_cd"].ToString() == "29" || dtFromHowla.Rows[i]["f_cd"].ToString() == "30"))
                                else if (dtFromHowla.Rows[i]["in_out"].ToString() == "O" && fundListofTwentyPaisa.Contains(Convert.ToInt32(dtFromHowla.Rows[i]["f_cd"].ToString())))
                                {

                                    tp = 'S';
                                    amt = Convert.ToDouble(dtFromHowla.Rows[i]["amt"].ToString());
                                    // amt_cm = amt * (1 - 0.0020);
                                    amt_cm = amt * (1 - Convert.ToDouble(dtFundofTwentyPaisa.Rows[0]["sl_buy_com_pct"].ToString()) / 100);

                                    strSelFromFundTransHBQuery = "select f_cd, comp_cd from fund_trans_hb where vch_dt ='" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd =" + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                      " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('C','A')";
                                    dtFromFundTrans = commonGatewayObj.Select(strSelFromFundTransHBQuery);
                                    if (dtFromFundTrans.Rows.Count > 0)
                                    {
                                        string strUPdQuery = "update fund_trans_hb set no_share =" + dtFromHowla.Rows[i]["qty"].ToString() + ",  rate =" + dtFromHowla.Rows[i]["rate"].ToString() + ",amount =" + dtFromHowla.Rows[i]["amt"].ToString() + ", stock_ex ='" + dtFromHowla.Rows[i]["brk"].ToString() + "'" +
                                             ", vch_no='" + txtVoucherNumber.Text.Replace("'", "''") + "'," +
                                              " amt_aft_com =" + amt_cm + " where vch_dt = '" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd = " + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                          " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('C','A')";

                                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQuery);
                                        //ClearFields();
                                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data updated!');", true);
                                    }

                                    else
                                    {
                                        // This is insert
                                        string strInsQuery = "insert into fund_trans_hb(vch_dt, f_cd, comp_cd,vch_no," +
                                        " tran_tp, no_share, rate, amount, stock_ex, amt_aft_com,op_name) values('" + dtFromHowla.Rows[i]["sp_date"].ToString() + "'," + dtFromHowla.Rows[i]["f_cd"].ToString() + "," + dtFromHowla.Rows[i]["comp_cd"].ToString() + "," +
                                         " '" + txtVoucherNumber.Text.Replace("'", "''") + "'," +
                                       " decode('" + dtFromHowla.Rows[i]["in_out"].ToString() + "', 'I', 'C', 'O', 'S')," + dtFromHowla.Rows[i]["qty"].ToString() + "," + dtFromHowla.Rows[i]["rate"].ToString() + "," + dtFromHowla.Rows[i]["amt"].ToString() + ",'" + dtFromHowla.Rows[i]["brk"].ToString() + "'," + amt_cm + ",'" + LoginID + "')";

                                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                                        //ClearFields();
                                        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);

                                    }
                                }


                            }



                        }

                        //...................for bond.............update fundtrans_hb........................


                        DataTable dtSourcefundtranshbbybond;
                        string currentdate, strUPdQueryforBond;

                        DateTime dtimeCurrentDate = DateTime.Now;



                        if (!string.IsNullOrEmpty(dtimeCurrentDate.ToString()))
                        {
                            currentdate = Convert.ToDateTime(dtimeCurrentDate).ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            currentdate = "";
                        }

                        string strCompnaybond = "SELECT  VCH_DT, F_CD, COMP_CD, TRAN_TP, VCH_NO, NO_SHARE, RATE, COST_RATE, CRT_AFT_COM,AMOUNT, AMT_AFT_COM, STOCK_EX,OP_NAME FROM FUND_TRANS_HB where comp_cd in(950,954) and VCH_DT='" + currentdate + "'";
                        dtSourcefundtranshbbybond = commonGatewayObj.Select(strCompnaybond);

                        if (dtSourcefundtranshbbybond != null && dtSourcefundtranshbbybond.Rows.Count > 0)
                        {

                            for (int k = 0; k < dtSourcefundtranshbbybond.Rows.Count; k++)
                            {
                                if (dtSourcefundtranshbbybond.Rows[k]["TRAN_TP"].ToString() == "C")
                                {
                                    strUPdQueryforBond = "UPDATE FUND_TRANS_HB SET AMT_AFT_COM = AMOUNT + 50 + AMOUNT * 0.002  WHERE  comp_cd =" + dtSourcefundtranshbbybond.Rows[k]["COMP_CD"].ToString() + " and VCH_DT='" + currentdate + "' and TRAN_TP = 'C' ";

                                    int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQueryforBond);
                                }
                                else if (dtSourcefundtranshbbybond.Rows[k]["TRAN_TP"].ToString() == "S")
                                {
                                    strUPdQueryforBond = "UPDATE FUND_TRANS_HB SET AMT_AFT_COM = AMOUNT-50 - AMOUNT * 0.002 WHERE  comp_cd =" + dtSourcefundtranshbbybond.Rows[k]["COMP_CD"].ToString() + " and VCH_DT='" + currentdate + "' and TRAN_TP = 'S' ";

                                    int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQueryforBond);
                                }

                            }
                        }
                      strProcessing = "Processing completed!!!!";

                        if (!string.IsNullOrEmpty(strProcessing))
                        {
                            strlblProcessing = strProcessing;
                        }
                        else
                        {
                            strlblProcessing = "";
                        }
                        // ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Process completed!');", true);
                    }
                    else
                    {

                        strProcessing = "No data found!!!!";

                        if (!string.IsNullOrEmpty(strProcessing))
                        {
                            strlblProcessing = strProcessing;
                        }
                        else
                        {
                            strlblProcessing = "";
                        }
                        // ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('No data found!');", true);
                    }



                }

                lblProcessing.Text = strlblProcessing;

            }
            catch (Exception ex)
            {


                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('" + ex.Message.ToString() + "');", true);
            }
        }

           
    }


    public class TblFundInfo
    {
        public string F_CD { get; set; }
        public string F_NAME { get; set; }
        public string Howla_Date_From { get; set; }
        public string Howla_LastUpdated_Date { get; set; }
        public string Howla_Date_To { get; set; }
        public string Stock_Exchange { get; set; }
    }
}






