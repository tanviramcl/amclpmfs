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
using System.Globalization;
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

        DataTable dtFundNameDropDownList = dropDownListObj.FundNameDropDownList();
        if (!IsPostBack)
        {
            fundNameDropDownList.DataSource = dtFundNameDropDownList;
            fundNameDropDownList.DataTextField = "F_NAME";
            fundNameDropDownList.DataValueField = "F_CD";
            fundNameDropDownList.DataBind();
           
            //  lblProcessingRelatedMessage.Visible = false;
           
        }

    }
    
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

    }
    private void ClearFields()
    {
       
        fundNameDropDownList.SelectedValue = "0";
        txtBalanceDate.Text = "";
        txtLastBalDate.Text = "";
        txtLastUpadateDate.Text = "";
        txtMarketPriceDate.Text = "";
        txtNoOfSaleShare.Text = "";
        txtNoSaleRecord.Text = "";
        txtNoPurchaseRecord.Text = "";
        txtNoPurchaseShares.Text = "";
        


    }
    protected void fundNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strQLastBalDtFrFundControl, strBalanceDate, strLastBalDate, strLastUpadateDate, strLastUpadatePlusOneDate, strQForSellShares, strQForPurchaseShares, strQForMrktPrice;
        DateTime?  dtimeBalanceDate, dtimeLastBalDate, dtimeLastUpadateDate, dtimeLastUpadatePlusOneDate;

       
        DataTable dtFromDual = new DataTable();
        DataTable dtFromFundControl = new DataTable();
        DataTable dtFromMarketPrice = new DataTable();
        DataTable dtFromFundTrHBForSale = new DataTable();
        DataTable dtFromFundTrHBForBuy = new DataTable();
        lblProcessing.Text = "";
        //  ClearFields();

        /* For converting balance update process:
         * Field(Developer): upto_dt -> txtBalanceDate
         * last_bal_dt  -> txtLastUpadateDate
         * lst_b_dt  -> txtLastBalDate
        
         */
        try
        {
            //strQuery = "SELECT TO_CHAR(SYSDATE, 'DD-MON-YYYY')currentDate FROM dual";
            //dtFromDual = commonGatewayObj.Select(strQuery);
            //if (dtFromDual.Rows.Count > 0)
            //{

            //    txtBalanceDate.Text = dtFromDual.Rows[0]["currentDate"].ToString();

            //}

            DateTime dtimeCurrentDate = DateTime.Now;

            string currentDate = Convert.ToDateTime(dtimeCurrentDate).ToString("dd-MMM-yyyy");

            if (!string.IsNullOrEmpty(currentDate))
            {
                txtBalanceDate.Text= currentDate;
            }

                strQLastBalDtFrFundControl = "select TO_CHAR(bal_dt,'DD-MON-YYYY')lst_bal_dt from fund_control where f_cd =" + fundNameDropDownList.SelectedValue.ToString();

            dtFromFundControl = commonGatewayObj.Select(strQLastBalDtFrFundControl);
            if (dtFromFundControl!=null && dtFromFundControl.Rows.Count > 0)
            {

                txtLastUpadateDate.Text = dtFromFundControl.Rows[0]["lst_bal_dt"].ToString();
                txtLastBalDate.Text = dtFromFundControl.Rows[0]["lst_bal_dt"].ToString();

            }
            else
            {
                txtLastUpadateDate.Text = "30-JUN-2005";
            }


            if (!string.IsNullOrEmpty(txtBalanceDate.Text.Trim()))
            {
                dtimeBalanceDate = Convert.ToDateTime(txtBalanceDate.Text.ToString());

                strBalanceDate = dtimeBalanceDate.Value.ToString("dd-MMM-yyyy");
            }
            else
            {
                dtimeBalanceDate = null;
                strBalanceDate = "";
            }


            if (!string.IsNullOrEmpty(txtLastBalDate.Text.Trim()))
            {
                dtimeLastBalDate = Convert.ToDateTime(txtLastBalDate.Text.ToString());
                strLastBalDate = dtimeLastBalDate.Value.ToString("dd-MMM-yyyy");
            }
            else
            {
                dtimeLastBalDate = null;
                strLastBalDate = "";
            }


            if (!string.IsNullOrEmpty(txtLastUpadateDate.Text.Trim()))
            {
                dtimeLastUpadateDate = Convert.ToDateTime(txtLastUpadateDate.Text.ToString());
                strLastUpadateDate = dtimeLastUpadateDate.Value.ToString("dd-MMM-yyyy");
                dtimeLastUpadatePlusOneDate = dtimeLastUpadateDate.Value.AddDays(1);
                strLastUpadatePlusOneDate = dtimeLastUpadatePlusOneDate.Value.ToString("dd-MMM-yyyy");
            }
            else
            {
                dtimeLastUpadateDate = null;
                strLastUpadateDate = "";
                dtimeLastUpadatePlusOneDate = null;
                strLastUpadatePlusOneDate = "";
            }

            strQForMrktPrice = "select TO_CHAR(max(Tran_date),'DD-MON-YYYY')mp_dt from market_price where tran_date <='" + strBalanceDate + "'";

            dtFromMarketPrice = commonGatewayObj.Select(strQForMrktPrice);
            if (dtFromMarketPrice != null && dtFromMarketPrice.Rows.Count > 0)
            {

                txtMarketPriceDate.Text = dtFromMarketPrice.Rows[0]["mp_dt"].ToString();


            }

            if (dtimeBalanceDate <= dtimeLastUpadateDate)
            {

                txtLastUpadateDate.Text = "01-JUL-2002";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Process will be started from July 2002.');", true);
            }
            // For sell of shares

            strQForSellShares = "select  count(*)NoSaleRecord,sum(no_share)NoSaleShares from fund_trans_hb where f_cd =" + fundNameDropDownList.SelectedValue.ToString() +
                " and tran_tp = 'S' and vch_dt between '" + strLastUpadatePlusOneDate + "' and '" + strBalanceDate + "'";

            dtFromFundTrHBForSale = commonGatewayObj.Select(strQForSellShares);
            if (dtFromFundTrHBForSale != null && dtFromFundTrHBForSale.Rows.Count > 0)
            {
                txtNoSaleRecord.Text = dtFromFundTrHBForSale.Rows[0]["NoSaleRecord"].ToString();
                txtNoOfSaleShare.Text = dtFromFundTrHBForSale.Rows[0]["NoSaleShares"].ToString();

            }
            else
            {
                txtNoSaleRecord.Text = "0";
                txtNoOfSaleShare.Text = "0";

            }


            /* For converting balance update process:
          * Field(Developer): upto_dt -> txtBalanceDate
          * last_bal_dt  -> txtLastUpadateDate
          * lst_b_dt  -> txtLastBalDate

          */

            // For purchase of shares
            strQForPurchaseShares = "select  count(*)NoPurchaseRecord,sum(no_share)NoPurchaseShares from fund_trans_hb where f_cd =" + fundNameDropDownList.SelectedValue.ToString() +
                " and tran_tp = 'C' and vch_dt between '" + strLastUpadatePlusOneDate + "' and '" + strBalanceDate + "'";

            dtFromFundTrHBForBuy = commonGatewayObj.Select(strQForPurchaseShares);
            if (dtFromFundTrHBForBuy.Rows.Count > 0)
            {
                txtNoPurchaseRecord.Text = dtFromFundTrHBForBuy.Rows[0]["NoPurchaseRecord"].ToString();
                txtNoPurchaseShares.Text = dtFromFundTrHBForBuy.Rows[0]["NoPurchaseShares"].ToString();

            }
            else
            {
                txtNoPurchaseRecord.Text = "0";
                txtNoPurchaseShares.Text = "0";

            }
        }
        catch (Exception ex)
        {

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('" + ex.Message.ToString() + "');", true);
        }

    }

    private string adv_proc1(string vchDtFrom,string vchDtTo, string f_cd)
    {

        string strQuery, strSelFromFundFolioHBQuery, strUpdFundfolioHBForTrTypeS, strUpdFundfolioHBForTrTypeNotS, strInsIntoFundFolioHBForTrTypeNotS, strInsIntoFundFolioHBForTrTypeS, strUpdateFundTransHB, LoginID = Session["UserID"].ToString(), strRetVal;
        
        DataTable dtFromFundTransHB = new DataTable();
        DataTable dtFromFundFolioHB = new DataTable();

        //strQuery = "select TO_CHAR(vch_dt,'DD-MON-YYYY')vch_dt, f_cd, comp_cd, no_share, rate, nvl(amount,0)amount,amt_aft_com, tran_tp, stock_ex from fund_trans_hb" +
        //" where vch_dt between '" + vchDtFrom + "' and '" + vchDtTo + "' and f_cd=" + f_cd +
        //" order by  comp_cd";

        strQuery = "select vch_dt, f_cd, comp_cd, no_share, rate, nvl(amount,0)amount,amt_aft_com, tran_tp, stock_ex" +
                    " from fund_trans_hb where vch_dt between '" + vchDtFrom + "' and '" + vchDtTo + "' and f_cd=" + f_cd +" order by vch_dt,comp_cd";

            dtFromFundTransHB = commonGatewayObj.Select(strQuery);
     
        if (dtFromFundTransHB!=null && dtFromFundTransHB.Rows.Count > 0)
        {

            //lblProcessingRelatedMessage.Visible = true;
            //lblProcessingRelatedMessage.Text = "process is running!!!!";
            for (int i = 0; i < dtFromFundTransHB.Rows.Count; i++)
            {

                Double cmp = 0, mt_shr = 0, mt_cost = 0, mt_cst_aft_com = 0, mcost_rt = 0, mcost_rt_acm = 0, m_amt, m_amt_acm, m_no = 0, m_cost = 0, m_cost_acm = 0;

            string vchdate = Convert.ToDateTime(dtFromFundTransHB.Rows[i]["vch_dt"]).ToString("dd-MMM-yyyy");
            if (!string.IsNullOrEmpty(dtFromFundTransHB.Rows[i]["amount"].ToString()))
                {
                    m_amt = Convert.ToDouble(dtFromFundTransHB.Rows[i]["amount"].ToString());

                }

                else
                {
                    m_amt = 0;
                }
                if (!string.IsNullOrEmpty(dtFromFundTransHB.Rows[i]["amt_aft_com"].ToString()))
                {
                    m_amt_acm = Convert.ToDouble(dtFromFundTransHB.Rows[i]["amt_aft_com"].ToString());

                }

                else
                {

                    m_amt_acm = 0;
                }

                strSelFromFundFolioHBQuery = "select comp_cd, tot_nos, tot_cost, tcst_aft_com from fund_folio_hb" +
                " where f_cd = " + dtFromFundTransHB.Rows[i]["f_cd"].ToString() + " and comp_cd = " + dtFromFundTransHB.Rows[i]["comp_cd"].ToString();

                dtFromFundFolioHB = commonGatewayObj.Select(strSelFromFundFolioHBQuery);

                if (dtFromFundFolioHB!=null && dtFromFundFolioHB.Rows.Count > 0)
                {
                    for (int j = 0; j < dtFromFundFolioHB.Rows.Count; j++)
                    {

                        if (!string.IsNullOrEmpty(dtFromFundFolioHB.Rows[j]["comp_cd"].ToString().Trim()))
                        {
                            cmp = Convert.ToDouble(dtFromFundFolioHB.Rows[j]["comp_cd"].ToString());
                        }

                        if (!string.IsNullOrEmpty(dtFromFundFolioHB.Rows[j]["tot_nos"].ToString().Trim()))
                        {
                            mt_shr = Convert.ToDouble(dtFromFundFolioHB.Rows[j]["tot_nos"].ToString());
                        }
                        if (!string.IsNullOrEmpty(dtFromFundFolioHB.Rows[j]["tot_cost"].ToString().Trim()))
                        {
                            mt_cost = Convert.ToDouble(dtFromFundFolioHB.Rows[j]["tot_cost"].ToString());
                        }
                        if (!string.IsNullOrEmpty(dtFromFundFolioHB.Rows[j]["tcst_aft_com"].ToString().Trim()))
                        {
                            mt_cst_aft_com = Convert.ToDouble(dtFromFundFolioHB.Rows[j]["tcst_aft_com"].ToString());
                        }



                        if (mt_shr == 0)
                        {
                            mcost_rt = 0;
                            mcost_rt_acm = 0;

                        }

                        else

                        {
                            mcost_rt = Math.Round(mt_cost / mt_shr, 2);
                            mcost_rt_acm = Math.Round(mt_cst_aft_com / mt_shr, 2);
                        }

                        if (dtFromFundTransHB.Rows[i]["tran_tp"].ToString() == "S")
                        {
                            m_amt = Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) * mcost_rt;
                            m_amt_acm = Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) * mcost_rt_acm;
                            m_no = mt_shr - Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString());
                            m_cost = mt_cost - m_amt;
                            m_cost_acm = mt_cst_aft_com - m_amt_acm;

                            //m_amt:= j.no_share * mcost_rt;
                            //m_amt_acm:= j.no_share * mcost_rt_acm;
                            //m_no:= mt_shr - j.no_share;
                            //m_cost:= mt_cost - m_amt;
                            //m_cost_acm:= mt_cst_aft_com - m_amt_acm;

                            strUpdFundfolioHBForTrTypeS = "update fund_folio_hb set o_no_shr = nvl(o_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) + "," +
                                                   "o_rate = (nvl(o_rate, 0) * nvl(o_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["amount"].ToString()) + ") / (nvl(o_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) + ")," +
                                                   "ort_aft_com = (nvl(ort_aft_com, 0) * nvl(o_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["amt_aft_com"].ToString()) + ")/(nvl(o_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) + ")," +
                                                   "tot_nos =" + m_no +
                                                   ",tot_cost =" + m_cost +
                                                   ",tcst_aft_com =" + m_cost_acm +
                                                   // ", bal_dt = '" + dtFromFundTransHB.Rows[i]["vch_dt"].ToString() + "'" +
                                                   ", bal_dt = '" + vchdate + "'" +
                                                   ", op_name = '" + LoginID + "'" +
                                                   " where f_cd = " + dtFromFundTransHB.Rows[i]["f_cd"].ToString() + " and comp_cd = " + dtFromFundTransHB.Rows[i]["comp_cd"].ToString();

                            int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUpdFundfolioHBForTrTypeS);
                          

                        }

                        else
                        {

                          
                            m_no = mt_shr + Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString());
                            m_cost = mt_cost + m_amt;
                            m_cost_acm = mt_cst_aft_com + m_amt_acm;

                            strUpdFundfolioHBForTrTypeNotS = "update fund_folio_hb set i_no_shr = nvl(i_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) + "," +
                                                    "i_rate = (nvl(i_rate, 0) * nvl(i_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["amount"].ToString()) + ") / (nvl(i_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) + ")," +
                                                    "irt_aft_com = (nvl(irt_aft_com, 0) * nvl(i_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["amt_aft_com"].ToString()) + ")/(nvl(i_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) + ")," +
                                                    "tot_nos =" + m_no +
                                                    ",tot_cost =" + m_cost +
                                                    ",tcst_aft_com =" + m_cost_acm +
                                                     //", bal_dt = '" + dtFromFundTransHB.Rows[i]["vch_dt"].ToString() + "'" +
                                                     ", bal_dt = '" + vchdate + "'" +
                                                    ", op_name = '" + LoginID + "'" +
                                                    " where f_cd = " + dtFromFundTransHB.Rows[i]["f_cd"].ToString() + " and comp_cd = " + dtFromFundTransHB.Rows[i]["comp_cd"].ToString();

                            int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUpdFundfolioHBForTrTypeNotS);
                           
                        }


                    }

                } // End of if  dtFromFundFolioHB.Rows.Count > 0


                else

                {

                    /*  Here we had  if (dtFromFundTransHB.Rows[i]["tran_tp"].ToString() == "S")
                     *  before changing to if (dtFromFundTransHB.Rows[i]["tran_tp"].ToString() != "S")
                     *  on 04 jul 2017
                     *    */


                    /*
                     single funf
                     */


                    if (dtFromFundTransHB.Rows[i]["tran_tp"].ToString() != "S") 
                        {
                       
                        m_no = mt_shr + Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString());
                        m_cost= mt_cost + m_amt;
                        m_cost_acm= mt_cst_aft_com + m_amt_acm;

                        strInsIntoFundFolioHBForTrTypeNotS = "insert into fund_folio_hb(f_cd, comp_cd, i_no_shr, i_rate,irt_aft_com, bal_dt, tot_nos, tot_cost, op_name, tcst_aft_com)" +
                        " values(" +
                        dtFromFundTransHB.Rows[i]["f_cd"].ToString() + "," +
                        dtFromFundTransHB.Rows[i]["comp_cd"].ToString() + "," +
                        dtFromFundTransHB.Rows[i]["no_share"].ToString() + "," +
                        "nvl(" + dtFromFundTransHB.Rows[i]["amount"].ToString() + ", 0)/nvl(" + dtFromFundTransHB.Rows[i]["no_share"].ToString() + ",1)," +
                        dtFromFundTransHB.Rows[i]["amt_aft_com"].ToString() + "/nvl(" + dtFromFundTransHB.Rows[i]["no_share"].ToString() + ",1),'" +
                        // dtFromFundTransHB.Rows[i]["vch_dt"].ToString() + "'," +
                        vchdate + "'," +
                        m_no + "," +
                        m_cost + ",'" +
                        LoginID + "'," +
                        m_cost_acm + ")";

                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsIntoFundFolioHBForTrTypeNotS);
                        //'" + LoginID + "'" +
                    }
                    else
                    {
                        m_amt= Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) * mcost_rt;
                        m_amt_acm= Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) * mcost_rt_acm;
                        m_no= mt_shr - Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString());
                        m_cost= mt_cost - m_amt;
                        m_cost_acm= mt_cst_aft_com - m_amt_acm;

                        strInsIntoFundFolioHBForTrTypeS = "insert into fund_folio_hb(f_cd, comp_cd, o_no_shr, o_rate,ort_aft_com, bal_dt, tot_nos, tot_cost, op_name, tcst_aft_com)" +
                       " values(" +
                       dtFromFundTransHB.Rows[i]["f_cd"].ToString() + "," +
                       dtFromFundTransHB.Rows[i]["comp_cd"].ToString() + "," +
                       dtFromFundTransHB.Rows[i]["no_share"].ToString() + "," +
                       "nvl(" + dtFromFundTransHB.Rows[i]["amount"].ToString() + ", 0)/nvl(" + dtFromFundTransHB.Rows[i]["no_share"].ToString() + ",1)," +
                       dtFromFundTransHB.Rows[i]["amt_aft_com"].ToString() + "/nvl(" + dtFromFundTransHB.Rows[i]["no_share"].ToString() + ",1),'" +
                       //dtFromFundTransHB.Rows[i]["vch_dt"].ToString() + "'," +
                       vchdate + "'," +
                       m_no + "," +
                       m_cost + ",'" +
                       LoginID + "'," +
                       m_cost_acm +")";

                       int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsIntoFundFolioHBForTrTypeS);
                       
                    }

                }

                if (dtFromFundTransHB.Rows[i]["tran_tp"].ToString() == "S")

                {
                    strUpdateFundTransHB = "update fund_trans_hb set " +
                        " cost_rate =" + mcost_rt +","+
                        " crt_aft_com=" + mcost_rt_acm +
                        " where f_cd =" + dtFromFundTransHB.Rows[i]["f_cd"].ToString() + " and comp_cd=" + dtFromFundTransHB.Rows[i]["comp_cd"].ToString() +
                    //" and vch_dt ='" + dtFromFundTransHB.Rows[i]["vch_dt"].ToString() + "' and tran_tp ='S' and stock_ex ='" + dtFromFundTransHB.Rows[i]["stock_ex"].ToString()+"'";
                    " and vch_dt ='" + vchdate + "' and tran_tp ='S' and stock_ex ='" + dtFromFundTransHB.Rows[i]["stock_ex"].ToString()+"'";

                    int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUpdateFundTransHB);
                   

                }


            }
            strRetVal = "Processing Completed";
            return strRetVal;

        }
        else
        {
            strRetVal = "No data found !";
            return strRetVal;
            //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('No data found !');", true);
            //lblProcessing.Text = "No data found !";

        }


    }
    protected void btnSave_Click(object sender, EventArgs e)
    {


        string strBalanceDate, strLastBalDate, strLastUpadateDate, strLastUpadatePlusOneDate, strUpdateFundTransHB, strDelFromFundFolioHB, strMarketPriceDate,temp;
       
        DateTime? dtimeBalanceDate, dtimeLastBalDate, dtimeLastUpadateDate, dtimeLastUpadatePlusOneDate, dtMarketPriceDate;
        DataTable dtFromFundTransHB = new DataTable();
        DataTable dtFromFundFolioHB = new DataTable();
        CommonGateway commonGatewayObj = new CommonGateway();
        DataTable dtSource = new DataTable();
        DataTable dtSource2 = new DataTable();

        try
        {
            if (!string.IsNullOrEmpty(txtBalanceDate.Text.Trim()))
            {
                dtimeBalanceDate = Convert.ToDateTime(txtBalanceDate.Text.ToString());

                strBalanceDate = dtimeBalanceDate.Value.ToString("dd-MMM-yyyy");
            }
            else
            {
                dtimeBalanceDate = null;
                strBalanceDate = "";
            }

            if (!string.IsNullOrEmpty(txtLastBalDate.Text.Trim()))
            {
                dtimeLastBalDate = Convert.ToDateTime(txtLastBalDate.Text.ToString());
                strLastBalDate = dtimeLastBalDate.Value.ToString("dd-MMM-yyyy");
            }
            else
            {
                dtimeLastBalDate = null;
                strLastBalDate = "";
            }
            if (!string.IsNullOrEmpty(txtLastUpadateDate.Text.Trim()))
            {
                dtimeLastUpadateDate = Convert.ToDateTime(txtLastUpadateDate.Text.ToString());
                strLastUpadateDate = dtimeLastUpadateDate.Value.ToString("dd-MMM-yyyy");
                dtimeLastUpadatePlusOneDate = dtimeLastUpadateDate.Value.AddDays(1);
                strLastUpadatePlusOneDate = dtimeLastUpadatePlusOneDate.Value.ToString("dd-MMM-yyyy");
            }
            else
            {
                dtimeLastUpadateDate = null;
                strLastUpadateDate = "";
                dtimeLastUpadatePlusOneDate = null;
                strLastUpadatePlusOneDate = "";
            }

            if (!string.IsNullOrEmpty(txtMarketPriceDate.Text.Trim()))
            {
                dtMarketPriceDate = Convert.ToDateTime(txtMarketPriceDate.Text.ToString());
                strMarketPriceDate = dtMarketPriceDate.Value.ToString("dd-MMM-yyyy");
            }
            else
            {
                dtMarketPriceDate = null;
                strMarketPriceDate = "";
            }
            if (dtimeBalanceDate > dtimeLastBalDate)
            {

                 temp = adv_proc1(strLastUpadatePlusOneDate, strBalanceDate, fundNameDropDownList.SelectedValue.ToString());
                if (temp.Trim() == "Processing Completed")
                {
                    lblProcessing.Text = "Processing completed!!!!";
                   // ClearFields();
                }
                else
                {
                    lblProcessing.Text = "No data found!!!!";
                  //  ClearFields();
                }
                //lblProcessing.Text = "Processing completed!!!!";
                //ClearFields();
            }

            else
            {
                commonGatewayObj.BeginTransaction();
                strDelFromFundFolioHB = "delete from fund_folio_hb where f_cd =" + fundNameDropDownList.SelectedValue.ToString();
                int noDelRowsFromFundFolioHB = commonGatewayObj.ExecuteNonQuery(strDelFromFundFolioHB);

                strUpdateFundTransHB = "update fund_trans_hb set cost_rate = null,crt_aft_com = null where f_cd =" + fundNameDropDownList.SelectedValue.ToString();
                int noUpdRowsFundTransHB = commonGatewayObj.ExecuteNonQuery(strUpdateFundTransHB);
                commonGatewayObj.CommitTransaction();

                temp= adv_proc1(strLastUpadatePlusOneDate, strBalanceDate, fundNameDropDownList.SelectedValue.ToString());
                if (temp.Trim() == "Processing Completed")
                {
                    lblProcessing.Text = "Processing completed!!!!";
                   // ClearFields();
                }
                else {
                    lblProcessing.Text = "No data found!!!!";
                 //   ClearFields();
                }
              //  lblProcessing.Text = "Processing completed!!!!";
              

                // Code goes here Code goes here

            }



            string strCompnayTransdate = "select a.comp_cd, max(a.tran_date) tran_date from market_price a, fund_folio_hb b where a.comp_cd = b.comp_cd and b.f_cd =" + fundNameDropDownList.SelectedValue.ToString() + " and a.tran_date <= '" + strBalanceDate + "' group by a.comp_cd order by a.comp_cd";
                                                                                                                                                                                    
            dtSource = commonGatewayObj.Select(strCompnayTransdate);

            List<CompanayTransdate> lstCompnayTransdate = new List<CompanayTransdate>();
            List<CompanyAvarageRate> lstCompnayAvgrate = new List<CompanyAvarageRate>();
            lstCompnayTransdate = (from DataRow dr in dtSource.Rows
                                   select new CompanayTransdate()
                                   {
                                       COMP_CD = dr["COMP_CD"].ToString(),
                                       TRAN_DATE = dr["TRAN_DATE"].ToString()
                                   }).ToList();

            string dltQuery = "delete from mprice_temp where f_cd=" + fundNameDropDownList.SelectedValue.ToString();
            int dltNumOfRows = commonGatewayObj.ExecuteNonQuery(dltQuery);

            foreach (CompanayTransdate comtransdate in lstCompnayTransdate)
            {


                string strInsQuery = "select " + fundNameDropDownList.SelectedValue.ToString() + " as FundId, comp_cd, avg_rt from  market_price where comp_cd = " + comtransdate.COMP_CD + " and tran_date = '" + Convert.ToDateTime(comtransdate.TRAN_DATE).ToString("dd-MMM-yyyy") + "'";

                dtSource2 = commonGatewayObj.Select(strInsQuery);

                lstCompnayAvgrate = (from DataRow dr in dtSource2.Rows
                                     select new CompanyAvarageRate()
                                     {
                                         FUNDID = dr["FUNDID"].ToString(),
                                         COMP_CD = dr["COMP_CD"].ToString(),
                                         AVG_RT = dr["AVG_RT"].ToString()
                                     }).ToList();
                

                foreach (CompanyAvarageRate comAvgrate in lstCompnayAvgrate)
                {

                    string strQueryInsMprice_Temp = "insert into mprice_temp(f_cd, comp_cd, avg_rt) values ('" + comAvgrate.FUNDID + "','" + comAvgrate.COMP_CD + "','" + comAvgrate.AVG_RT + "')";
                    int NumOfRows = commonGatewayObj.ExecuteNonQuery(strQueryInsMprice_Temp);
                }

            }
            string LoginID = Session["UserID"].ToString();
            DateTime dtimeCurrentDateTimeForLog = DateTime.Now;
            string strCurrentDateTimeForLog = dtimeCurrentDateTimeForLog.ToString("dd-MMM-yyyy HH:mm:ss tt", CultureInfo.InvariantCulture);

            //string strupdateQueryfund_control = "update fund_control set bal_dt='" + strBalanceDate + "',mprice_dt='" + strMarketPriceDate + "' where f_cd =" + fundNameDropDownList.SelectedValue.ToString() + "";

            string strupdateQueryfund_control = "update fund_control set op_name='" + LoginID + "',upd_date_time='" + strCurrentDateTimeForLog + "',bal_dt='" + strBalanceDate + "',mprice_dt='" + strMarketPriceDate + "' where f_cd =" + fundNameDropDownList.SelectedValue.ToString() + "";
            int updatefund_controlNumOfRows = commonGatewayObj.ExecuteNonQuery(strupdateQueryfund_control);
           

            // System.Threading.Thread.Sleep(3000);

            // ClearFields();


           
            

        }
        catch (Exception ex)
        {

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    public class CompanayTransdate
    {
        public string COMP_CD { get; set; }
        public string TRAN_DATE { get; set; }
    }
    public class CompanyAvarageRate
    {
        public string FUNDID { get; set; }
        public string COMP_CD { get; set; }
        public string AVG_RT { get; set; }
    }
}
