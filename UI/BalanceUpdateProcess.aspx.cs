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

            lblProcessingRelatedMessage.Visible = false;
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
        string strQuery, strBalanceDate, strLastBalDate, strLastUpadateDate, strLastUpadatePlusOneDate;
        DateTime? dtimeBalanceDate, dtimeLastBalDate, dtimeLastUpadateDate, dtimeLastUpadatePlusOneDate;

        CommonGateway commonGatewayObj = new CommonGateway();
        DataTable dt = new DataTable();

        //  ClearFields();

        /* For converting balance update process:
         * Field(Developer): upto_dt -> txtBalanceDate
         * last_bal_dt  -> txtLastUpadateDate
         * lst_b_dt  -> txtLastBalDate
        
         */

        strQuery = "SELECT TO_CHAR(SYSDATE, 'DD-MON-YYYY')currentDate FROM dual";
        dt = commonGatewayObj.Select(strQuery);
        if (dt.Rows.Count > 0)
        {

            txtBalanceDate.Text = dt.Rows[0]["currentDate"].ToString();

        }

        strQuery = "select TO_CHAR(bal_dt,'DD-MON-YYYY')lst_bal_dt from invest.fund_control where f_cd =" + fundNameDropDownList.SelectedValue.ToString();
                 
        dt = commonGatewayObj.Select(strQuery);
        if (dt.Rows.Count > 0)
        {

            txtLastUpadateDate.Text = dt.Rows[0]["lst_bal_dt"].ToString();
            txtLastBalDate.Text = dt.Rows[0]["lst_bal_dt"].ToString();

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
        
        strQuery = "select TO_CHAR(max(Tran_date),'DD-MON-YYYY')mp_dt from invest.market_price where tran_date <='" + strBalanceDate +"'";

        dt = commonGatewayObj.Select(strQuery);
        if (dt.Rows.Count > 0)
        {

            txtMarketPriceDate.Text = dt.Rows[0]["mp_dt"].ToString();
           

        }
       
        if (dtimeBalanceDate <= dtimeLastUpadateDate)
        {

            txtLastUpadateDate.Text = "01-JUL-2002";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Process start from July 2002.');", true);
        }
        

        strQuery = "select  count(*)NoSaleRecord,sum(no_share)NoSaleShares from invest.fund_trans_hb where f_cd =" + fundNameDropDownList.SelectedValue.ToString() +
            " and tran_tp = 'S' and vch_dt between '" + strLastUpadatePlusOneDate + "' and '" + strBalanceDate + "'";

        dt = commonGatewayObj.Select(strQuery);
        if (dt.Rows.Count > 0)
        {
            txtNoSaleRecord.Text = dt.Rows[0]["NoSaleRecord"].ToString();
            txtNoOfSaleShare.Text = dt.Rows[0]["NoSaleShares"].ToString();

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
        strQuery = "select  count(*)NoPurchaseRecord,sum(no_share)NoPurchaseShares from invest.fund_trans_hb where f_cd =" + fundNameDropDownList.SelectedValue.ToString() +
            " and tran_tp = 'C' and vch_dt between '" + strLastUpadatePlusOneDate + "' and '" + strBalanceDate + "'";

        dt = commonGatewayObj.Select(strQuery);
        if (dt.Rows.Count > 0)
        {
            txtNoPurchaseRecord.Text = dt.Rows[0]["NoPurchaseRecord"].ToString();
            txtNoPurchaseShares.Text = dt.Rows[0]["NoPurchaseShares"].ToString();

        }
        else
        {
            txtNoPurchaseRecord.Text = "0";
            txtNoPurchaseShares.Text = "0";
        }
        /*
        Begin
          If   :div_rec.upto_dt <= :div_rec.lst_bal_dt then
                           :div_rec.lst_bal_dt:= '01-JUL-02';
        Message('Process start from July 2002');
              		  :div01.tf:= 'T';
        Else

                       Message('Process start from ' || to_char(:div_rec.lst_bal_dt, 'DD-MM-RRRR'));
        End if;

        End;


        begin
             select  count(*),sum(no_share) into: div_rec.nos,:div_rec.hnos
                from invest.fund_trans_hb
             where f_cd =:div_rec.f_cd and tran_tp = 'S'
            and vch_dt between: div_rec.lst_bal_dt + 1 and: div_rec.upto_dt;

        exception
           when no_data_found then
           :div_rec.nos:= 0;
           :div_rec.hnos:= 0;
        end;

        begin
        select  count(*),sum(no_share) into: div_rec.nop,:div_rec.hnop
                    from invest.fund_trans_hb
               where f_cd =:div_rec.f_cd and tran_tp = 'C'
            and vch_dt between: div_rec.lst_bal_dt + 1 and: div_rec.upto_dt;
        message('start from ' || to_char(:div_rec.lst_bal_dt + 1, 'DD-MM-RRRR') || ' to_dt ' || to_char(:div_rec.upto_dt, 'DD-MM-RRRR'));
        exception
        when no_data_found then
   :div_rec.nop:= 0;
   :div_rec.hnop:= 0;
        end;


        if :div_rec.nos >= 1 or: div_rec.nop >= 1 then

           Message('Data Found');
        Next_item;
   else
   	     Message('Data Not Found');
        go_item('Div_rec.f_cd');
        raise form_trigger_failure;
        end if;
        */
    }

    private void adv_proc1(string vchDtFrom,string vchDtTo, string f_cd)
    {

        string strQuery, strSelFromFundFolioHBQuery, strUpdateFundfolioHB,strInsertIntoFundFolioHB, strUpdateFundTransHB, LoginID = Session["UserID"].ToString();
        Double cmp = 0, mt_shr = 0, mt_cost = 0, mt_cst_aft_com = 0, mcost_rt = 0, mcost_rt_acm = 0, m_amt, m_amt_acm, m_no = 0, m_cost = 0, m_cost_acm = 0;
        DataTable dtFromFundTransHB = new DataTable();
        DataTable dtFromFundFolioHB = new DataTable();
        CommonGateway commonGatewayObj = new CommonGateway();

        lblProcessingRelatedMessage.Visible = true;
        lblProcessingRelatedMessage.Text = "Advanced process is running!!!!";

        strQuery = "select TO_CHAR(vch_dt,'DD-MON-YYYY')vch_dt, f_cd, comp_cd, no_share, rate, nvl(amount,0)amount,amt_aft_com, tran_tp, stock_ex from invest.fund_trans_hb" +
        " where vch_dt between '" + vchDtFrom + "' and '" + vchDtTo + "' and f_cd=" + f_cd +
        " order by f_cd, vch_dt, comp_cd";

        dtFromFundTransHB = commonGatewayObj.Select(strQuery);

        if (dtFromFundTransHB.Rows.Count > 0)
        {

            for (int i = 0; i < dtFromFundTransHB.Rows.Count; i++)
            {

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

                strSelFromFundFolioHBQuery = "select comp_cd, tot_nos, tot_cost, tcst_aft_com from invest.fund_folio_hb" +
                " where f_cd = " + dtFromFundTransHB.Rows[i]["f_cd"].ToString() + " and comp_cd = " + dtFromFundTransHB.Rows[i]["comp_cd"].ToString();

                dtFromFundFolioHB = commonGatewayObj.Select(strSelFromFundFolioHBQuery);

                if (dtFromFundFolioHB.Rows.Count > 0)
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

                            strUpdateFundfolioHB = "update invest.fund_folio_hb set o_no_shr = nvl(o_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) + "," +
                                                   "o_rate = (nvl(o_rate, 0) * nvl(o_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["amount"].ToString()) + ") / (nvl(o_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) + ")," +
                                                   "ort_aft_com = (nvl(ort_aft_com, 0) * nvl(o_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["amt_aft_com"].ToString()) + ")/(nvl(o_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) + ")," +
                                                   "tot_nos =" + m_no +
                                                   ",tot_cost =" + m_cost +
                                                   ",tcst_aft_com =" + m_cost_acm +
                                                   ", bal_dt = '" + dtFromFundTransHB.Rows[i]["vch_dt"].ToString() + "'" +
                                                   ", op_name = '" + LoginID + "'" +
                                                   " where f_cd = " + dtFromFundTransHB.Rows[i]["f_cd"].ToString() + " and comp_cd = " + dtFromFundTransHB.Rows[i]["comp_cd"].ToString();

                            int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUpdateFundfolioHB);
                          

                        }

                        else
                        {

                            /* if j.tran_tp='C' then  */
                            m_no = mt_shr + Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString());
                            m_cost = mt_cost + m_amt;
                            m_cost_acm = mt_cst_aft_com + m_amt_acm;

                            strUpdateFundfolioHB = "update invest.fund_folio_hb set i_no_shr = nvl(i_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) + "," +
                                                    "i_rate = (nvl(i_rate, 0) * nvl(i_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["amount"].ToString()) + ") / (nvl(i_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) + ")," +
                                                    "irt_aft_com = (nvl(irt_aft_com, 0) * nvl(i_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["amt_aft_com"].ToString()) + ")/(nvl(i_no_shr, 0) +" + Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) + ")," +
                                                    "tot_nos =" + m_no +
                                                    ",tot_cost =" + m_cost +
                                                    ",tcst_aft_com =" + m_cost_acm +
                                                    ", bal_dt = '" + dtFromFundTransHB.Rows[i]["vch_dt"].ToString() + "'" +
                                                    ", op_name = '" + LoginID + "'" +
                                                    " where f_cd = " + dtFromFundTransHB.Rows[i]["f_cd"].ToString() + " and comp_cd = " + dtFromFundTransHB.Rows[i]["comp_cd"].ToString();

                            int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUpdateFundfolioHB);
                           
                        }


                    }
                    //     if mt_shr = 0 then
                    //         mcost_rt := 0;
                    //     mcost_rt_acm:= 0;
                    //else
                    //	   mcost_rt:= round(mt_cost / mt_shr, 2);
                    //     mcost_rt_acm:= round(mt_cst_aft_com / mt_shr, 2);
                    //     end if;
                }


                else

                {

                    if (dtFromFundTransHB.Rows[i]["tran_tp"].ToString() == "S")
                    {
                        m_no= mt_shr + Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString());
                        m_cost= mt_cost + m_amt;
                        m_cost_acm= mt_cst_aft_com + m_amt_acm;

                        strInsertIntoFundFolioHB = "insert into invest.fund_folio_hb(f_cd, comp_cd, i_no_shr, i_rate,irt_aft_com, bal_dt, tot_nos, tot_cost, tcst_aft_com)" +
                        " values(" +
                        dtFromFundTransHB.Rows[i]["f_cd"].ToString() + "," +
                        dtFromFundTransHB.Rows[i]["comp_cd"].ToString() + "," +
                        dtFromFundTransHB.Rows[i]["no_share"].ToString() + "," +
                        "nvl(" + dtFromFundTransHB.Rows[i]["amount"].ToString() + ", 0)/nvl(" + dtFromFundTransHB.Rows[i]["no_share"].ToString() + ",1)," +
                        dtFromFundTransHB.Rows[i]["amt_aft_com"].ToString() + "/nvl(" + dtFromFundTransHB.Rows[i]["no_share"].ToString() + ",1),'" +
                        dtFromFundTransHB.Rows[i]["vch_dt"].ToString() + "'," +
                        m_no + "," +
                        m_cost + "," +
                        m_cost_acm + ")";

                        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsertIntoFundFolioHB);
                       
                    }
                    else
                    {
                        m_amt= Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) * mcost_rt;
                        m_amt_acm= Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString()) * mcost_rt_acm;
                        m_no= mt_shr - Convert.ToDouble(dtFromFundTransHB.Rows[i]["no_share"].ToString());
                        m_cost= mt_cost - m_amt;
                        m_cost_acm= mt_cst_aft_com - m_amt_acm;

                        strInsertIntoFundFolioHB = "insert into invest.fund_folio_hb(f_cd, comp_cd, o_no_shr, o_rate,ort_aft_com, bal_dt, tot_nos, tot_cost, tcst_aft_com)" +
                       " values(" +
                       dtFromFundTransHB.Rows[i]["f_cd"].ToString() + "," +
                       dtFromFundTransHB.Rows[i]["comp_cd"].ToString() + "," +
                       dtFromFundTransHB.Rows[i]["no_share"].ToString() + "," +
                       dtFromFundTransHB.Rows[i]["amount"].ToString() + "/nvl(" + dtFromFundTransHB.Rows[i]["no_share"].ToString() + ",1)," +
                       dtFromFundTransHB.Rows[i]["amt_aft_com"].ToString() + "/nvl(" + dtFromFundTransHB.Rows[i]["no_share"].ToString() + ",1),'" +
                       dtFromFundTransHB.Rows[i]["vch_dt"].ToString() + "'," +
                       m_no + "," +
                       m_cost + "," +
                       m_cost_acm+")";

                       int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsertIntoFundFolioHB);
                       
                    }

                }

                if (dtFromFundTransHB.Rows[i]["tran_tp"].ToString() == "S")
                {
                    strUpdateFundTransHB = "update invest.fund_trans_hb set " +
                        " cost_rate =" + mcost_rt +","+
                        " crt_aft_com=" + mcost_rt_acm +
                        " where f_cd =" + dtFromFundTransHB.Rows[i]["f_cd"].ToString() + " and comp_cd=" + dtFromFundTransHB.Rows[i]["comp_cd"].ToString() +
                        " and vch_dt ='" + dtFromFundTransHB.Rows[i]["vch_dt"].ToString() + "' and tran_tp ='S' and stock_ex ='" + dtFromFundTransHB.Rows[i]["stock_ex"].ToString()+"'";

                    int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUpdateFundTransHB);
                   

                }
                   

            }




        }
        else
        {
          //  ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data Error !');", true);
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('No data found !');", true);

        }


    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strBalanceDate, strLastBalDate, strLastUpadateDate, strLastUpadatePlusOneDate, strUpdateFundTransHB, strDelFromFundFolioHB, strMarketPriceDate;
       
        DateTime? dtimeBalanceDate, dtimeLastBalDate, dtimeLastUpadateDate, dtimeLastUpadatePlusOneDate, dtMarketPriceDate;
        DataTable dtFromFundTransHB = new DataTable();
        DataTable dtFromFundFolioHB = new DataTable();
        CommonGateway commonGatewayObj = new CommonGateway();
        DataTable dtSource = new DataTable();
        DataTable dtSource2 = new DataTable();
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
            lblProcessingRelatedMessage.Visible = true;
            adv_proc1(strLastUpadatePlusOneDate, strBalanceDate, fundNameDropDownList.SelectedValue.ToString());
           

        }

        else
        {

            strDelFromFundFolioHB = "delete from invest.fund_folio_hb where f_cd =" + fundNameDropDownList.SelectedValue.ToString();
            int noDelRowsFromFundFolioHB = commonGatewayObj.ExecuteNonQuery(strDelFromFundFolioHB);

            strUpdateFundTransHB = "update invest.fund_trans_hb set cost_rate = null,crt_aft_com = null where f_cd =" + fundNameDropDownList.SelectedValue.ToString();
            int noUpdRowsFundTransHB = commonGatewayObj.ExecuteNonQuery(strUpdateFundTransHB);
            lblProcessingRelatedMessage.Visible = true;
            adv_proc1(strLastUpadatePlusOneDate, strBalanceDate, fundNameDropDownList.SelectedValue.ToString());
            

            // Code goes here Code goes here

        }



        //        PROCEDURE upd_m_price IS
        //BEGIN
        //   DECLARE
        //   cursor cmp is
        //     select a.comp_cd, max(a.tran_date) tran_date
        //     from invest.market_price a, invest.fund_folio_hb b
        //     where a.comp_cd = b.comp_cd and b.f_cd =:div_rec.f_cd
        //        and a.tran_date <=:div_rec.upto_dt
        //        group by a.comp_cd order by a.comp_cd;
        //        rmp cmp% rowtype;
        //        BEGIN
        //           delete from invest.mprice_temp where f_cd =:div_rec.f_cd;
        //        for rmp in cmp loop

        //              insert into invest.mprice_temp(f_cd, comp_cd, avg_rt)

        //                select :div_rec.f_cd, comp_cd, avg_rt

        //                 from invest.market_price

        //                 where comp_cd = rmp.comp_cd and tran_date = rmp.tran_date;
        //            --Message(' f_cd  ' || to_char(:div_rec.f_cd) || '  comp_cd  ' || to_char(rmp.comp_cd));
        //        end loop;
        //        ---update
        //  END;
        //        END;



        string strCompnayTransdate = "select a.comp_cd, max(a.tran_date) tran_date from invest.market_price a, invest.fund_folio_hb b where a.comp_cd = b.comp_cd and b.f_cd =" + fundNameDropDownList.SelectedValue.ToString() + " and a.tran_date <= '" + strBalanceDate + "' group by a.comp_cd order by a.comp_cd";

        dtSource = commonGatewayObj.Select(strCompnayTransdate);

        List<CompanayTransdate> lstCompnayTransdate = new List<CompanayTransdate>();
        List<CompanyAvarageRate> lstCompnayAvgrate = new List<CompanyAvarageRate>();
        lstCompnayTransdate = (from DataRow dr in dtSource.Rows
                               select new CompanayTransdate()
                               {
                                   COMP_CD = dr["COMP_CD"].ToString(),
                                   TRAN_DATE = dr["TRAN_DATE"].ToString()
                               }).ToList();

        string dltQuery = "delete from invest.mprice_temp where f_cd=" + fundNameDropDownList.SelectedValue.ToString();
        int dltNumOfRows = commonGatewayObj.ExecuteNonQuery(dltQuery);

        foreach (CompanayTransdate comtransdate in lstCompnayTransdate)
        {


            string strInsQuery = "select " + fundNameDropDownList.SelectedValue.ToString() + " as FundId, comp_cd, avg_rt from  invest.market_price where comp_cd = " + comtransdate.COMP_CD + " and tran_date = '" + Convert.ToDateTime(comtransdate.TRAN_DATE).ToString("dd-MMM-yyyy") + "'";

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

                string Query = "insert into invest.mprice_temp(f_cd, comp_cd, avg_rt) values ('" + comAvgrate.FUNDID + "','" + comAvgrate.COMP_CD + "','" + comAvgrate.AVG_RT + "')";
                int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
            }

        }
        string strupdateQueryfund_control = "update invest.fund_control set bal_dt='" + strBalanceDate + "',mprice_dt='" + strMarketPriceDate + "' where f_cd =" + fundNameDropDownList.SelectedValue.ToString() + "";
        int updatefund_controlNumOfRows = commonGatewayObj.ExecuteNonQuery(strupdateQueryfund_control);
        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data Insert Successfully');", true);
        ClearFields();




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
