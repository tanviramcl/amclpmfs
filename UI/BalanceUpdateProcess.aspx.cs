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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strQuery, strBalanceDate, strLastBalDate, strLastUpadateDate, strLastUpadatePlusOneDate, strSelFromFundFolioHBQuery;
        Double cmp=0, mt_shr=0, mt_cost=0, mt_cst_aft_com=0;
        DateTime? dtimeBalanceDate, dtimeLastBalDate, dtimeLastUpadateDate, dtimeLastUpadatePlusOneDate;
        DataTable dtFromFundTransHB = new DataTable();
        DataTable dtFromFundFolioHB = new DataTable();
        CommonGateway commonGatewayObj = new CommonGateway();

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
        if (dtimeBalanceDate > dtimeLastBalDate)
        {
            lblProcessingRelatedMessage.Visible = true;
            lblProcessingRelatedMessage.Text = "Advanced process is running!!!!";

            strQuery = "select vch_dt, f_cd, comp_cd, no_share, rate, amount,amt_aft_com, tran_tp, stock_ex from invest.fund_trans_hb" +
             " where vch_dt between '" + strLastUpadatePlusOneDate + "' and '" + strBalanceDate + "' and f_cd=" + fundNameDropDownList.SelectedValue.ToString() +
             " order by f_cd, vch_dt, comp_cd";



            dtFromFundTransHB = commonGatewayObj.Select(strQuery);
            if (dtFromFundTransHB.Rows.Count > 0)
            {

                for (int i = 0; i < dtFromFundTransHB.Rows.Count; i++)
                {

                    strSelFromFundFolioHBQuery = "select comp_cd, tot_nos, tot_cost, tcst_aft_com from invest.fund_folio_hb"+
                    " where f_cd = " + dtFromFundTransHB.Rows[i]["f_cd"].ToString() + " and comp_cd = " + dtFromFundTransHB.Rows[i]["comp_cd"].ToString();

                    dtFromFundFolioHB= commonGatewayObj.Select(strSelFromFundFolioHBQuery);

                    if (dtFromFundFolioHB.Rows.Count > 0)
                    {
                        
                        cmp = Convert.ToDouble(dtFromFundFolioHB.Rows[i]["comp_cd"].ToString());
                        mt_shr = Convert.ToDouble(dtFromFundFolioHB.Rows[i]["tot_nos"].ToString());
                        mt_cost = Convert.ToDouble(dtFromFundFolioHB.Rows[i]["tot_cost"].ToString());
                        mt_cst_aft_com = Convert.ToDouble(dtFromFundFolioHB.Rows[i]["tcst_aft_com"].ToString());
                        


                        // strSelFromFundFolioHBQuery = "select comp_cd, tot_nos, tot_cost, tcst_aft_com
                        // into cmp, mt_shr, mt_cost, mt_cst_aft_com from invest.fund_folio_hb
                        //where f_cd = j.f_cd and comp_cd = j.comp_cd;
                    }
                }




            }
            else
            {
                

            }

           
        }



        //     Begin
        //        --if :div_rec.nos >= 1 or: div_rec.nop >= 1 then

        //                     -- if :div_rec.upto_dt >:div_rec.lst_bal_dt then
        //          if :div_rec.upto_dt >:div01.lst_b_dt then

        //                  message(' Advanced process is running ');
        //     adv_proc1;

        //          else
        //   ---message(to_char(:div_rec.nos) || ' Data is updated in howla_temp');
        //     message(' Back process is running ');
        //     back_proc;
        //     end if;
        //     ---del_aft_proc;
        //     upd_m_price;
        //     update invest.fund_control set bal_dt =:div_rec.upto_dt, mprice_dt =:div01.mp_dt
        //            where f_cd =:div_rec.f_cd;

        //     --else
        //--message('There is no data ' || to_char(:div_rec.nos));
        //     --end if;
        //     Exception
        //          when others then
        //       rollback;

        //     end;

    }
}
