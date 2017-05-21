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


public partial class DateWiseTransaction : System.Web.UI.Page
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


        }
    }

  



    protected void fundNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {

        string strQuery;
        CommonGateway commonGatewayObj = new CommonGateway();
        DataTable dt = new DataTable();

        txtHowlaDateFrom.Text = "";
        txtLastHowlaDate.Text = "";
        stockExchangeDropDownList.DataValueField = stockExchangeDropDownList.SelectedValue;

        if (stockExchangeDropDownList.SelectedValue == "D") // For DSE
            {

                strQuery = "select TO_CHAR(max(vch_dt),'DD-MON-YYYY')last_tr_dt,TO_CHAR(max(vch_dt) + 1,'DD-MON-YYYY')vch_dt  from invest.fund_trans_hb where f_cd =" + fundNameDropDownList.SelectedValue.ToString() +
                     " and tran_tp in ('C','S') and stock_ex in ('D','A')";
            dt = commonGatewayObj.Select(strQuery);
        }

            else if (stockExchangeDropDownList.SelectedValue == "C")// For CSE
            {
                strQuery = "select TO_CHAR(max(vch_dt),'DD-MON-YYYY')last_tr_dt,TO_CHAR(max(vch_dt) + 1,'DD-MON-YYYY')vch_dt  from invest.fund_trans_hb where f_cd =" + fundNameDropDownList.SelectedValue.ToString() +
                                " and tran_tp in ('C','S') and stock_ex in ('C','A')";
            dt = commonGatewayObj.Select(strQuery);
        }


            else // For new stock exchange
            {
            dt = null;
              //  strQuery = ""; // SQL Query must be added here
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('This stock exchange is not added yet.');", true);
            }

            


            if (!dt.Rows[0].IsNull("vch_dt"))
            {

                txtHowlaDateFrom.Text = dt.Rows[0]["vch_dt"].ToString();
                txtLastHowlaDate.Text = dt.Rows[0]["last_tr_dt"].ToString();


                strQuery = "SELECT TO_CHAR(SYSDATE, 'DD-MON-YYYY')currentDate FROM dual";
                dt = commonGatewayObj.Select(strQuery);
                if (dt.Rows.Count > 0)
                {

                    txtHowlaDateTo.Text = dt.Rows[0]["currentDate"].ToString();

                }

            }

            else
            {
                txtHowlaDateFrom.Text = "";
                txtLastHowlaDate.Text = "";
                txtHowlaDateTo.Text = "";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('This Trading Date not found  else.');", true);
            }

        }
    

    private void ClearFields()
    {
        stockExchangeDropDownList.SelectedValue = "0";
        fundNameDropDownList.SelectedValue = "0";
        //  stockExchangeDropDownList.DataValueField = stockExchangeDropDownList.SelectedValue;
        // fundNameDropDownList.DataValueField = stockExchangeDropDownList.SelectedValue;
        txtHowlaDateFrom.Text = "";
        txtLastHowlaDate.Text = "";
        txtHowlaDateTo.Text = "";


    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strSleFromHowlaQuery, strSelFromFundTransHBQuery, strHowlaDateFrom, strLastHowlaDate, strHowlaDateTo, strSelFundofTwentyFivePaisa, strSelFundofTwentyPaisa, LoginID = Session["UserID"].ToString();
        char tp;
        Double amt, temp = 0, amt_cm = 0;
        string LoginName = Session["UserName"].ToString().ToUpper();
        CommonGateway commonGatewayObj = new CommonGateway();

        DateTime dtimeHowlaDateFrom, dtimeLastHowlaDate, dtimeHowlaDateTo;

       
            dtimeHowlaDateFrom = Convert.ToDateTime(txtHowlaDateFrom.Text.ToString());
            strHowlaDateFrom = dtimeHowlaDateFrom.ToString("dd-MMM-yyyy");
            dtimeLastHowlaDate = Convert.ToDateTime(txtLastHowlaDate.Text.ToString());
            strLastHowlaDate = dtimeLastHowlaDate.ToString("dd-MMM-yyyy");
            dtimeHowlaDateTo = Convert.ToDateTime(txtHowlaDateTo.Text.ToString());
            strHowlaDateTo = dtimeHowlaDateTo.ToString("dd-MMM-yyyy");




        if ((dtimeHowlaDateFrom < dtimeLastHowlaDate) || (dtimeHowlaDateTo <= dtimeLastHowlaDate))
        {
            // message('This Trading Date is already allocated. ' || to_char(:div_rec.sp_dt_fm));
            ClearFields();
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('This Trading Date is already allocated.');", true);
        }

        else
        {

            DataTable dtFromHowla = new DataTable();
            DataTable dtFromFundTrans = new DataTable();

            if (stockExchangeDropDownList.SelectedValue == "D")   // For DSE
            {

                strSleFromHowlaQuery = "select TO_CHAR(sp_date,'DD-MON-YYYY')sp_date, f_cd, comp_cd, in_out, sum(sp_qty)qty, substr(bk_cd, 1, 1) brk," +
                  " sum(sp_qty * sp_rate) amt, ROUND((sum(sp_qty * sp_rate) / sum(sp_qty)),2) rate from invest.howla where sp_date between '" + strHowlaDateFrom +
                  "' and '" + strHowlaDateTo + "' and f_cd =" + fundNameDropDownList.SelectedValue.ToString() + " group by sp_date, f_cd, comp_cd, in_out, substr(bk_cd,1, 1) order by sp_date,f_cd,comp_cd";
            }

            else if (stockExchangeDropDownList.SelectedValue == "C")  // For CSE
            {

                strSleFromHowlaQuery = "select TO_CHAR(sp_date,'DD-MON-YYYY')sp_date, f_cd, comp_cd, in_out, sum(sp_qty)qty, substr(bk_cd, 1, 1) brk," +
                " sum(sp_qty * sp_rate) amt, ROUND((sum(sp_qty * sp_rate) / sum(sp_qty)),3) rate from invest.howla_cse where sp_date between '" + strHowlaDateFrom +
                "' and '" + strHowlaDateTo + "' and f_cd =" + fundNameDropDownList.SelectedValue.ToString() + " group by sp_date, f_cd, comp_cd, in_out, substr(bk_cd,1, 1) order by sp_date,f_cd,comp_cd";
                
            }
            else // For new stock exchange
            {
                strSleFromHowlaQuery = ""; // SQL Query must be added here
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('This stock exchange is not added yet.');", true);
            }
            dtFromHowla = commonGatewayObj.Select(strSleFromHowlaQuery);


            DataTable dtFundofTwentyFivePaisa = new DataTable();
            strSelFundofTwentyFivePaisa = "select f_cd,f_name,sl_buy_com_pct from invest.fund where sl_buy_com_pct=0.25 order by f_cd";
            dtFundofTwentyFivePaisa = commonGatewayObj.Select(strSelFundofTwentyFivePaisa);

            List<int> fundListofTwentyFivePaisa = (from row in dtFundofTwentyFivePaisa.AsEnumerable()
                                                   select Convert.ToInt32(row["f_cd"].ToString())).ToList();


            DataTable dtFundofTwentyPaisa = new DataTable();
            strSelFundofTwentyPaisa = "select f_cd,f_name,sl_buy_com_pct from invest.fund where sl_buy_com_pct=0.20 order by f_cd";
            dtFundofTwentyPaisa = commonGatewayObj.Select(strSelFundofTwentyPaisa);

            List<int> fundListofTwentyPaisa = (from row in dtFundofTwentyPaisa.AsEnumerable()
                                               select Convert.ToInt32(row["f_cd"].ToString())).ToList();



            if (dtFromHowla.Rows.Count > 0)
            {
                for (int i = 0; i < dtFromHowla.Rows.Count; i++)
                {
                    if (stockExchangeDropDownList.SelectedValue == "D")
                    {

                    // For editing purposes


                    // For editing


                    //  if (dtFromHowla.Rows[i]["in_out"].ToString() == "I" && (dtFromHowla.Rows[i]["f_cd"].ToString() == "1" || dtFromHowla.Rows[i]["f_cd"].ToString() == "2" || dtFromHowla.Rows[i]["f_cd"].ToString() == "4" || dtFromHowla.Rows[i]["f_cd"].ToString() == "6" || dtFromHowla.Rows[i]["f_cd"].ToString() == "7" || dtFromHowla.Rows[i]["f_cd"].ToString() == "8" || dtFromHowla.Rows[i]["f_cd"].ToString() == "9" || dtFromHowla.Rows[i]["f_cd"].ToString() == "10" || dtFromHowla.Rows[i]["f_cd"].ToString() == "11" || dtFromHowla.Rows[i]["f_cd"].ToString() == "12" || dtFromHowla.Rows[i]["f_cd"].ToString() == "13" || dtFromHowla.Rows[i]["f_cd"].ToString() == "20"))

                    if (dtFromHowla.Rows[i]["in_out"].ToString() == "I" && fundListofTwentyFivePaisa.Contains(Convert.ToInt32(dtFromHowla.Rows[i]["f_cd"].ToString())))
                    {
                        tp = 'C';
                        amt = Convert.ToDouble(dtFromHowla.Rows[i]["amt"].ToString());
                        temp = amt * (1 + 0.0025);
                        //  amt_cm = amt * (1 + 0.0025);
                        amt_cm = amt * (1 + Convert.ToDouble(dtFundofTwentyFivePaisa.Rows[0]["sl_buy_com_pct"].ToString()) / 100);

                        strSelFromFundTransHBQuery = "select f_cd, comp_cd from invest.fund_trans_hb where vch_dt ='" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd =" + dtFromHowla.Rows[i]["f_cd"].ToString() +
                          " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('D','A')";
                        dtFromFundTrans = commonGatewayObj.Select(strSelFromFundTransHBQuery);
                        if (dtFromFundTrans.Rows.Count > 0)
                        {
                            string strUPdQuery = "update invest.fund_trans_hb set no_share =" + dtFromHowla.Rows[i]["qty"].ToString() + ",  rate =" + dtFromHowla.Rows[i]["rate"].ToString() + ",amount =" + dtFromHowla.Rows[i]["amt"].ToString() + ", stock_ex ='" + dtFromHowla.Rows[i]["brk"].ToString() +
                                  "', amt_aft_com =" + amt_cm + " where vch_dt = '" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd = " + dtFromHowla.Rows[i]["f_cd"].ToString() +
                              " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('D','A')";

                            int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQuery);
                            ClearFields();
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data updated!');", true);
                        }

                        else
                        {

                            string strInsQuery = "insert into invest.fund_trans_hb(vch_dt, f_cd, comp_cd," +
                " tran_tp, no_share, rate, amount, stock_ex, amt_aft_com,op_name) values('" + dtFromHowla.Rows[i]["sp_date"].ToString() + "'," + dtFromHowla.Rows[i]["f_cd"].ToString() + "," + dtFromHowla.Rows[i]["comp_cd"].ToString() + "," +
                           " decode('" + dtFromHowla.Rows[i]["in_out"].ToString() + "', 'I', 'C', 'O', 'S')," + dtFromHowla.Rows[i]["qty"].ToString() + "," + dtFromHowla.Rows[i]["rate"].ToString() + "," + dtFromHowla.Rows[i]["amt"].ToString() + ",'" + dtFromHowla.Rows[i]["brk"].ToString() + "'," + amt_cm + ",'" + LoginID + "')";

                            int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                            ClearFields();
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);

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

                        strSelFromFundTransHBQuery = "select f_cd, comp_cd from invest.fund_trans_hb where vch_dt ='" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd =" + dtFromHowla.Rows[i]["f_cd"].ToString() +
                          " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('D','A')";
                        dtFromFundTrans = commonGatewayObj.Select(strSelFromFundTransHBQuery);
                        if (dtFromFundTrans.Rows.Count > 0)
                        {
                            string strUPdQuery = "update invest.fund_trans_hb set no_share =" + dtFromHowla.Rows[i]["qty"].ToString() + ",  rate =" + dtFromHowla.Rows[i]["rate"].ToString() + ",amount =" + dtFromHowla.Rows[i]["amt"].ToString() + ", stock_ex ='" + dtFromHowla.Rows[i]["brk"].ToString() +
                                  "', amt_aft_com =" + amt_cm + " where vch_dt = '" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd = " + dtFromHowla.Rows[i]["f_cd"].ToString() +
                              " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('D','A')";

                            int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQuery);
                            ClearFields();
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data updated!');", true);
                        }

                        else
                        {


                            string strInsQuery = "insert into invest.fund_trans_hb(vch_dt, f_cd, comp_cd," +
                            " tran_tp, no_share, rate, amount, stock_ex, amt_aft_com,op_name) values('" + dtFromHowla.Rows[i]["sp_date"].ToString() + "'," + dtFromHowla.Rows[i]["f_cd"].ToString() + "," + dtFromHowla.Rows[i]["comp_cd"].ToString() + "," +
                           " decode('" + dtFromHowla.Rows[i]["in_out"].ToString() + "', 'I', 'C', 'O', 'S')," + dtFromHowla.Rows[i]["qty"].ToString() + "," + dtFromHowla.Rows[i]["rate"].ToString() + "," + dtFromHowla.Rows[i]["amt"].ToString() + ",'" + dtFromHowla.Rows[i]["brk"].ToString() + "'," + amt_cm + ",'" + LoginID + "')";

                            int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                            ClearFields();
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);
                        }
                    }

                    // else if (dtFromHowla.Rows[i]["in_out"].ToString() == "I" && (dtFromHowla.Rows[i]["f_cd"].ToString() == "14" || dtFromHowla.Rows[i]["f_cd"].ToString() == "15" || dtFromHowla.Rows[i]["f_cd"].ToString() == "16" || dtFromHowla.Rows[i]["f_cd"].ToString() == "17" || dtFromHowla.Rows[i]["f_cd"].ToString() == "18" || dtFromHowla.Rows[i]["f_cd"].ToString() == "19" || dtFromHowla.Rows[i]["f_cd"].ToString() == "20" || dtFromHowla.Rows[i]["f_cd"].ToString() == "21" || dtFromHowla.Rows[i]["f_cd"].ToString() == "22" || dtFromHowla.Rows[i]["f_cd"].ToString() == "23" || dtFromHowla.Rows[i]["f_cd"].ToString() == "24" || dtFromHowla.Rows[i]["f_cd"].ToString() == "25" || dtFromHowla.Rows[i]["f_cd"].ToString() == "26" || dtFromHowla.Rows[i]["f_cd"].ToString() == "27" || dtFromHowla.Rows[i]["f_cd"].ToString() == "28" || dtFromHowla.Rows[i]["f_cd"].ToString() == "29" || dtFromHowla.Rows[i]["f_cd"].ToString() == "30"))
                    else if (dtFromHowla.Rows[i]["in_out"].ToString() == "I" && fundListofTwentyPaisa.Contains(Convert.ToInt32(dtFromHowla.Rows[i]["f_cd"].ToString())))
                    {

                        tp = 'C';
                        amt = Convert.ToDouble(dtFromHowla.Rows[i]["amt"].ToString());
                        // amt_cm = amt * (1 + 0.0020);
                        amt_cm = amt * (1 + Convert.ToDouble(dtFundofTwentyPaisa.Rows[0]["sl_buy_com_pct"].ToString()) / 100);

                        strSelFromFundTransHBQuery = "select f_cd, comp_cd from invest.fund_trans_hb where vch_dt ='" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd =" + dtFromHowla.Rows[i]["f_cd"].ToString() +
                          " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('D','A')";
                        dtFromFundTrans = commonGatewayObj.Select(strSelFromFundTransHBQuery);
                        if (dtFromFundTrans.Rows.Count > 0)
                        {
                            string strUPdQuery = "update invest.fund_trans_hb set no_share =" + dtFromHowla.Rows[i]["qty"].ToString() + ",  rate =" + dtFromHowla.Rows[i]["rate"].ToString() + ",amount =" + dtFromHowla.Rows[i]["amt"].ToString() + ", stock_ex ='" + dtFromHowla.Rows[i]["brk"].ToString() +
                                  "', amt_aft_com =" + amt_cm + " where vch_dt = '" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd = " + dtFromHowla.Rows[i]["f_cd"].ToString() +
                              " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('D','A')";

                            int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQuery);
                            ClearFields();
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data updated!');", true);
                        }

                        else
                        {


                            string strInsQuery = "insert into invest.fund_trans_hb(vch_dt, f_cd, comp_cd," +
                            " tran_tp, no_share, rate, amount, stock_ex, amt_aft_com,op_name) values('" + dtFromHowla.Rows[i]["sp_date"].ToString() + "'," + dtFromHowla.Rows[i]["f_cd"].ToString() + "," + dtFromHowla.Rows[i]["comp_cd"].ToString() + "," +
                           " decode('" + dtFromHowla.Rows[i]["in_out"].ToString() + "', 'I', 'C', 'O', 'S')," + dtFromHowla.Rows[i]["qty"].ToString() + "," + dtFromHowla.Rows[i]["rate"].ToString() + "," + dtFromHowla.Rows[i]["amt"].ToString() + ",'" + dtFromHowla.Rows[i]["brk"].ToString() + "'," + amt_cm + ",'" + LoginID + "')";

                            int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                            ClearFields();
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);
                        }
                    }

                    // else if (dtFromHowla.Rows[i]["in_out"].ToString() == "O" && (dtFromHowla.Rows[i]["f_cd"].ToString() == "14" || dtFromHowla.Rows[i]["f_cd"].ToString() == "15" || dtFromHowla.Rows[i]["f_cd"].ToString() == "16" || dtFromHowla.Rows[i]["f_cd"].ToString() == "17" || dtFromHowla.Rows[i]["f_cd"].ToString() == "18" || dtFromHowla.Rows[i]["f_cd"].ToString() == "19" || dtFromHowla.Rows[i]["f_cd"].ToString() == "20" || dtFromHowla.Rows[i]["f_cd"].ToString() == "21" || dtFromHowla.Rows[i]["f_cd"].ToString() == "22" || dtFromHowla.Rows[i]["f_cd"].ToString() == "23" || dtFromHowla.Rows[i]["f_cd"].ToString() == "24" || dtFromHowla.Rows[i]["f_cd"].ToString() == "25" || dtFromHowla.Rows[i]["f_cd"].ToString() == "26" || dtFromHowla.Rows[i]["f_cd"].ToString() == "27" || dtFromHowla.Rows[i]["f_cd"].ToString() == "28" || dtFromHowla.Rows[i]["f_cd"].ToString() == "29" || dtFromHowla.Rows[i]["f_cd"].ToString() == "30"))
                    else if (dtFromHowla.Rows[i]["in_out"].ToString() == "O" && fundListofTwentyPaisa.Contains(Convert.ToInt32(dtFromHowla.Rows[i]["f_cd"].ToString())))
                    {

                        tp = 'S';
                        amt = Convert.ToDouble(dtFromHowla.Rows[i]["amt"].ToString());
                        // amt_cm = amt * (1 - 0.0020);
                        amt_cm = amt * (1 - Convert.ToDouble(dtFundofTwentyPaisa.Rows[0]["sl_buy_com_pct"].ToString()) / 100);

                        strSelFromFundTransHBQuery = "select f_cd, comp_cd from invest.fund_trans_hb where vch_dt ='" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd =" + dtFromHowla.Rows[i]["f_cd"].ToString() +
                          " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('D','A')";
                        dtFromFundTrans = commonGatewayObj.Select(strSelFromFundTransHBQuery);
                        if (dtFromFundTrans.Rows.Count > 0)
                        {
                            string strUPdQuery = "update invest.fund_trans_hb set no_share =" + dtFromHowla.Rows[i]["qty"].ToString() + ",  rate =" + dtFromHowla.Rows[i]["rate"].ToString() + ",amount =" + dtFromHowla.Rows[i]["amt"].ToString() + ", stock_ex ='" + dtFromHowla.Rows[i]["brk"].ToString() +
                                  "', amt_aft_com =" + amt_cm + " where vch_dt = '" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd = " + dtFromHowla.Rows[i]["f_cd"].ToString() +
                              " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('D','A')";

                            int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQuery);
                            ClearFields();
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data updated!');", true);
                        }

                        else
                        {

                            string strInsQuery = "insert into invest.fund_trans_hb(vch_dt, f_cd, comp_cd," +
                            " tran_tp, no_share, rate, amount, stock_ex, amt_aft_com,op_name) values('" + dtFromHowla.Rows[i]["sp_date"].ToString() + "'," + dtFromHowla.Rows[i]["f_cd"].ToString() + "," + dtFromHowla.Rows[i]["comp_cd"].ToString() + "," +
                           " decode('" + dtFromHowla.Rows[i]["in_out"].ToString() + "', 'I', 'C', 'O', 'S')," + dtFromHowla.Rows[i]["qty"].ToString() + "," + dtFromHowla.Rows[i]["rate"].ToString() + "," + dtFromHowla.Rows[i]["amt"].ToString() + ",'" + dtFromHowla.Rows[i]["brk"].ToString() + "'," + amt_cm + ",'" + LoginID + "')";

                            int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                            ClearFields();
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);

                        }
                    }


                }

                    if (stockExchangeDropDownList.SelectedValue == "C")
                    {

                        // For editing purposes


                       


                        //  if (dtFromHowla.Rows[i]["in_out"].ToString() == "I" && (dtFromHowla.Rows[i]["f_cd"].ToString() == "1" || dtFromHowla.Rows[i]["f_cd"].ToString() == "2" || dtFromHowla.Rows[i]["f_cd"].ToString() == "4" || dtFromHowla.Rows[i]["f_cd"].ToString() == "6" || dtFromHowla.Rows[i]["f_cd"].ToString() == "7" || dtFromHowla.Rows[i]["f_cd"].ToString() == "8" || dtFromHowla.Rows[i]["f_cd"].ToString() == "9" || dtFromHowla.Rows[i]["f_cd"].ToString() == "10" || dtFromHowla.Rows[i]["f_cd"].ToString() == "11" || dtFromHowla.Rows[i]["f_cd"].ToString() == "12" || dtFromHowla.Rows[i]["f_cd"].ToString() == "13" || dtFromHowla.Rows[i]["f_cd"].ToString() == "20"))

                        if (dtFromHowla.Rows[i]["in_out"].ToString() == "I" && fundListofTwentyFivePaisa.Contains(Convert.ToInt32(dtFromHowla.Rows[i]["f_cd"].ToString())))
                        {
                            tp = 'C';
                            amt = Convert.ToDouble(dtFromHowla.Rows[i]["amt"].ToString());
                            temp = amt * (1 + 0.0025);
                            //  amt_cm = amt * (1 + 0.0025);
                            amt_cm = amt * (1 + Convert.ToDouble(dtFundofTwentyFivePaisa.Rows[0]["sl_buy_com_pct"].ToString()) / 100);

                            strSelFromFundTransHBQuery = "select f_cd, comp_cd from invest.fund_trans_hb where vch_dt ='" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd =" + dtFromHowla.Rows[i]["f_cd"].ToString() +
                              " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('C','A')";
                            dtFromFundTrans = commonGatewayObj.Select(strSelFromFundTransHBQuery);
                            if (dtFromFundTrans.Rows.Count > 0)
                            {
                                string strUPdQuery = "update invest.fund_trans_hb set no_share =" + dtFromHowla.Rows[i]["qty"].ToString() + ",  rate =" + dtFromHowla.Rows[i]["rate"].ToString() + ",amount =" + dtFromHowla.Rows[i]["amt"].ToString() + ", stock_ex ='" + dtFromHowla.Rows[i]["brk"].ToString() +
                                      "', amt_aft_com =" + amt_cm + " where vch_dt = '" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd = " + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                  " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('C','A')";

                                int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQuery);
                                ClearFields();
                                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data updated!');", true);
                            }

                            else
                            {

                                string strInsQuery = "insert into invest.fund_trans_hb(vch_dt, f_cd, comp_cd," +
                    " tran_tp, no_share, rate, amount, stock_ex, amt_aft_com,op_name) values('" + dtFromHowla.Rows[i]["sp_date"].ToString() + "'," + dtFromHowla.Rows[i]["f_cd"].ToString() + "," + dtFromHowla.Rows[i]["comp_cd"].ToString() + "," +
                               " decode('" + dtFromHowla.Rows[i]["in_out"].ToString() + "', 'I', 'C', 'O', 'S')," + dtFromHowla.Rows[i]["qty"].ToString() + "," + dtFromHowla.Rows[i]["rate"].ToString() + "," + dtFromHowla.Rows[i]["amt"].ToString() + ",'" + dtFromHowla.Rows[i]["brk"].ToString() + "'," + amt_cm + ",'" + LoginID + "')";

                                int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                                ClearFields();
                                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);

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

                            strSelFromFundTransHBQuery = "select f_cd, comp_cd from invest.fund_trans_hb where vch_dt ='" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd =" + dtFromHowla.Rows[i]["f_cd"].ToString() +
                              " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('C','A')";
                            dtFromFundTrans = commonGatewayObj.Select(strSelFromFundTransHBQuery);
                            if (dtFromFundTrans.Rows.Count > 0)
                            {
                                string strUPdQuery = "update invest.fund_trans_hb set no_share =" + dtFromHowla.Rows[i]["qty"].ToString() + ",  rate =" + dtFromHowla.Rows[i]["rate"].ToString() + ",amount =" + dtFromHowla.Rows[i]["amt"].ToString() + ", stock_ex ='" + dtFromHowla.Rows[i]["brk"].ToString() +
                                      "', amt_aft_com =" + amt_cm + " where vch_dt = '" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd = " + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                  " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('C','A')";

                                int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQuery);
                                ClearFields();
                                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data updated!');", true);
                            }

                            else
                            {


                                string strInsQuery = "insert into invest.fund_trans_hb(vch_dt, f_cd, comp_cd," +
                                " tran_tp, no_share, rate, amount, stock_ex, amt_aft_com,op_name) values('" + dtFromHowla.Rows[i]["sp_date"].ToString() + "'," + dtFromHowla.Rows[i]["f_cd"].ToString() + "," + dtFromHowla.Rows[i]["comp_cd"].ToString() + "," +
                               " decode('" + dtFromHowla.Rows[i]["in_out"].ToString() + "', 'I', 'C', 'O', 'S')," + dtFromHowla.Rows[i]["qty"].ToString() + "," + dtFromHowla.Rows[i]["rate"].ToString() + "," + dtFromHowla.Rows[i]["amt"].ToString() + ",'" + dtFromHowla.Rows[i]["brk"].ToString() + "'," + amt_cm + ",'" + LoginID + "')";

                                int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                                ClearFields();
                                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);
                            }
                        }

                        // else if (dtFromHowla.Rows[i]["in_out"].ToString() == "I" && (dtFromHowla.Rows[i]["f_cd"].ToString() == "14" || dtFromHowla.Rows[i]["f_cd"].ToString() == "15" || dtFromHowla.Rows[i]["f_cd"].ToString() == "16" || dtFromHowla.Rows[i]["f_cd"].ToString() == "17" || dtFromHowla.Rows[i]["f_cd"].ToString() == "18" || dtFromHowla.Rows[i]["f_cd"].ToString() == "19" || dtFromHowla.Rows[i]["f_cd"].ToString() == "20" || dtFromHowla.Rows[i]["f_cd"].ToString() == "21" || dtFromHowla.Rows[i]["f_cd"].ToString() == "22" || dtFromHowla.Rows[i]["f_cd"].ToString() == "23" || dtFromHowla.Rows[i]["f_cd"].ToString() == "24" || dtFromHowla.Rows[i]["f_cd"].ToString() == "25" || dtFromHowla.Rows[i]["f_cd"].ToString() == "26" || dtFromHowla.Rows[i]["f_cd"].ToString() == "27" || dtFromHowla.Rows[i]["f_cd"].ToString() == "28" || dtFromHowla.Rows[i]["f_cd"].ToString() == "29" || dtFromHowla.Rows[i]["f_cd"].ToString() == "30"))
                        else if (dtFromHowla.Rows[i]["in_out"].ToString() == "I" && fundListofTwentyPaisa.Contains(Convert.ToInt32(dtFromHowla.Rows[i]["f_cd"].ToString())))
                        {

                            tp = 'C';
                            amt = Convert.ToDouble(dtFromHowla.Rows[i]["amt"].ToString());
                            // amt_cm = amt * (1 + 0.0020);
                            amt_cm = amt * (1 + Convert.ToDouble(dtFundofTwentyPaisa.Rows[0]["sl_buy_com_pct"].ToString()) / 100);

                            strSelFromFundTransHBQuery = "select f_cd, comp_cd from invest.fund_trans_hb where vch_dt ='" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd =" + dtFromHowla.Rows[i]["f_cd"].ToString() +
                              " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('C','A')";
                            dtFromFundTrans = commonGatewayObj.Select(strSelFromFundTransHBQuery);
                            if (dtFromFundTrans.Rows.Count > 0)
                            {
                                string strUPdQuery = "update invest.fund_trans_hb set no_share =" + dtFromHowla.Rows[i]["qty"].ToString() + ",  rate =" + dtFromHowla.Rows[i]["rate"].ToString() + ",amount =" + dtFromHowla.Rows[i]["amt"].ToString() + ", stock_ex ='" + dtFromHowla.Rows[i]["brk"].ToString() +
                                      "', amt_aft_com =" + amt_cm + " where vch_dt = '" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd = " + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                  " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('C','A')";

                                int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQuery);
                                ClearFields();
                                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data updated!');", true);
                            }

                            else
                            {


                                string strInsQuery = "insert into invest.fund_trans_hb(vch_dt, f_cd, comp_cd," +
                                " tran_tp, no_share, rate, amount, stock_ex, amt_aft_com,op_name) values('" + dtFromHowla.Rows[i]["sp_date"].ToString() + "'," + dtFromHowla.Rows[i]["f_cd"].ToString() + "," + dtFromHowla.Rows[i]["comp_cd"].ToString() + "," +
                               " decode('" + dtFromHowla.Rows[i]["in_out"].ToString() + "', 'I', 'C', 'O', 'S')," + dtFromHowla.Rows[i]["qty"].ToString() + "," + dtFromHowla.Rows[i]["rate"].ToString() + "," + dtFromHowla.Rows[i]["amt"].ToString() + ",'" + dtFromHowla.Rows[i]["brk"].ToString() + "'," + amt_cm + ",'" + LoginID + "')";

                                int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                                ClearFields();
                                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);
                            }
                        }

                        // else if (dtFromHowla.Rows[i]["in_out"].ToString() == "O" && (dtFromHowla.Rows[i]["f_cd"].ToString() == "14" || dtFromHowla.Rows[i]["f_cd"].ToString() == "15" || dtFromHowla.Rows[i]["f_cd"].ToString() == "16" || dtFromHowla.Rows[i]["f_cd"].ToString() == "17" || dtFromHowla.Rows[i]["f_cd"].ToString() == "18" || dtFromHowla.Rows[i]["f_cd"].ToString() == "19" || dtFromHowla.Rows[i]["f_cd"].ToString() == "20" || dtFromHowla.Rows[i]["f_cd"].ToString() == "21" || dtFromHowla.Rows[i]["f_cd"].ToString() == "22" || dtFromHowla.Rows[i]["f_cd"].ToString() == "23" || dtFromHowla.Rows[i]["f_cd"].ToString() == "24" || dtFromHowla.Rows[i]["f_cd"].ToString() == "25" || dtFromHowla.Rows[i]["f_cd"].ToString() == "26" || dtFromHowla.Rows[i]["f_cd"].ToString() == "27" || dtFromHowla.Rows[i]["f_cd"].ToString() == "28" || dtFromHowla.Rows[i]["f_cd"].ToString() == "29" || dtFromHowla.Rows[i]["f_cd"].ToString() == "30"))
                        else if (dtFromHowla.Rows[i]["in_out"].ToString() == "O" && fundListofTwentyPaisa.Contains(Convert.ToInt32(dtFromHowla.Rows[i]["f_cd"].ToString())))
                        {

                            tp = 'S';
                            amt = Convert.ToDouble(dtFromHowla.Rows[i]["amt"].ToString());
                            // amt_cm = amt * (1 - 0.0020);
                            amt_cm = amt * (1 - Convert.ToDouble(dtFundofTwentyPaisa.Rows[0]["sl_buy_com_pct"].ToString()) / 100);

                            strSelFromFundTransHBQuery = "select f_cd, comp_cd from invest.fund_trans_hb where vch_dt ='" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd =" + dtFromHowla.Rows[i]["f_cd"].ToString() +
                              " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('C','A')";
                            dtFromFundTrans = commonGatewayObj.Select(strSelFromFundTransHBQuery);
                            if (dtFromFundTrans.Rows.Count > 0)
                            {
                                string strUPdQuery = "update invest.fund_trans_hb set no_share =" + dtFromHowla.Rows[i]["qty"].ToString() + ",  rate =" + dtFromHowla.Rows[i]["rate"].ToString() + ",amount =" + dtFromHowla.Rows[i]["amt"].ToString() + ", stock_ex ='" + dtFromHowla.Rows[i]["brk"].ToString() +
                                      "', amt_aft_com =" + amt_cm + " where vch_dt = '" + dtFromHowla.Rows[i]["sp_date"].ToString() + "' and f_cd = " + dtFromHowla.Rows[i]["f_cd"].ToString() +
                                  " and comp_cd =" + dtFromHowla.Rows[i]["comp_cd"].ToString() + " and tran_tp ='" + tp + "' and stock_ex in ('C','A')";

                                int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPdQuery);
                                ClearFields();
                                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Data updated!');", true);
                            }

                            else
                            {

                                string strInsQuery = "insert into invest.fund_trans_hb(vch_dt, f_cd, comp_cd," +
                                " tran_tp, no_share, rate, amount, stock_ex, amt_aft_com,op_name) values('" + dtFromHowla.Rows[i]["sp_date"].ToString() + "'," + dtFromHowla.Rows[i]["f_cd"].ToString() + "," + dtFromHowla.Rows[i]["comp_cd"].ToString() + "," +
                               " decode('" + dtFromHowla.Rows[i]["in_out"].ToString() + "', 'I', 'C', 'O', 'S')," + dtFromHowla.Rows[i]["qty"].ToString() + "," + dtFromHowla.Rows[i]["rate"].ToString() + "," + dtFromHowla.Rows[i]["amt"].ToString() + ",'" + dtFromHowla.Rows[i]["brk"].ToString() + "'," + amt_cm + ",'" + LoginID + "')";

                                int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                                ClearFields();
                                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);

                            }
                        }


                    }



                }
            }
            else
            {
                ClearFields();
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('No data found!');", true);
            }

    }
}

    protected void txtLastHowlaDate_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtHowlaDateFrom_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtHowlaDateTo_TextChanged(object sender, EventArgs e)
    {

    }
}




//        if (:stock = 'D')then

//                  if j.in_out = 'I'and: f_cd in (1,2,4,20,6,7,8,9,10,11,12,13) then
//                        tp:= 'C';
//        amt_cm:= j.amt * (1 + 0.0025);
//        elsif j.in_out = 'O' and: f_cd in (1,2,4,20,6,7,8,9,10,11,12,13) then
//                 tp:= 'S';
//        amt_cm:= j.amt * (1 - 0.0025);


//        elsif j.in_out = 'I'and: f_cd in (14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30) then
//                  tp:= 'C';
//        amt_cm:= j.amt * (1 + 0.0020);
//        elsif j.in_out = 'O' and: f_cd in (14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30) then
//                  tp:= 'S';
//        amt_cm:= j.amt * (1 - 0.0020);


//        end if;

//        --New code should be added here all times

//-- end if;

//        end if;







// Hashtable httable = new Hashtable();
//// httable.Add("VCH_DT", Convert.ToDateTime(txtHowlaDateFrom.Text.ToString()).ToString("dd-MMM-yyyy"));
// if (!stockExchangeDropDownList.SelectedValue.Equals("0"))
// {
//     httable.Add("STOCK_EX", Convert.ToChar(stockExchangeDropDownList.SelectedValue));
// }
// if (!fundNameDropDownList.SelectedValue.Equals("0"))
// {
//     httable.Add("F_CD", Convert.ToInt16(fundNameDropDownList.SelectedValue));
// }

//if (!companyNameDropDownList.SelectedValue.Equals("0"))
//{
//    httable.Add("COMP_CD", Convert.ToInt16(companyNameDropDownList.SelectedValue));
//}
//if (!transTypeDropDownList.SelectedValue.Equals("0"))
//{
//    httable.Add("TRAN_TP", Convert.ToChar(transTypeDropDownList.SelectedValue));
//}
//if (!noOfShareTextBox.Text.Equals(""))
//{
//    httable.Add("NO_SHARE", Convert.ToDouble(noOfShareTextBox.Text));
//}
//if (!amountTextBox.Text.Equals(""))
//{
//    httable.Add("AMOUNT", Convert.ToDouble(amountTextBox.Text));
//}
//if (!voucherNoTextBox.Text.Equals(""))
//{
//    httable.Add("VCH_NO", (voucherNoTextBox.Text).ToString());
//}
//if (!rateTextBox.Text.Equals(""))
//{
//    httable.Add("RATE", Convert.ToDouble(rateTextBox.Text));
//}
//if (!amountAfterComissionTextBox.Text.Equals(""))
//{
//    httable.Add("AMT_AFT_COM", Convert.ToDouble(amountAfterComissionTextBox.Text));
//}



//httable.Add("ENTRY_DATE", DateTime.Today.ToString("dd-MMM-yyyy"));
//httable.Add("OP_NAME", LoginID);

//if (pf1s1DAOObj.IsDuplicateBonusRightEntry(Convert.ToInt32(fundNameDropDownList.SelectedValue.ToString()), Convert.ToInt32(companyNameDropDownList.SelectedValue.ToString()), Convert.ToDateTime(howlaDateTextBox.Text.Trim().ToString()).ToString("dd-MMM-yyyy"), transTypeDropDownList.SelectedValue.ToString(), Convert.ToInt32(noOfShareTextBox.Text.Trim().ToString())))
//{
//    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Save Failed:You are not Smart User  Trying to Duplicate entry');", true);
//}
//else
//{
//    commonGatewayObj.Insert(httable, "invest.fund_trans_hb");
//    ClearFields();
//    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);
//}
//fundNameDropDownList.Focus();


