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

public partial class UI_NonListedSecuritiesInvestmentEntryForm : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    DropDownList dropDownListObj = new DropDownList();
    Pf1s1DAO pf1s1DAOObj = new Pf1s1DAO();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Session["UserID"] == null)
            {
                Session.RemoveAll();
                Response.Redirect("../Default.aspx");
            }


            if (string.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                // not there!
                Session["NonlistedDetails"] = NonlistedSecuritiesDetails();

                DataTable dtNonlistedSecurities = (DataTable)Session["dtNonlistedSecurities"];


                saveButton.Text = "Add";
            }
            else
            {
                string CompanyCode = Convert.ToString(Request.QueryString["ID"]).Trim();
                Session["dtNonlistedSecuritiesByCOMPCODE"] = NonlistedSecuritiesDetailsByCOMPID(CompanyCode);
                DataTable dtNonlistedSecuritiesByCOMPCODE = (DataTable)Session["dtNonlistedSecuritiesByCOMPCODE"];

                if (dtNonlistedSecuritiesByCOMPCODE.Rows.Count > 0)
                {
                    fundNameDropDownList.SelectedValue = dtNonlistedSecuritiesByCOMPCODE.Rows[0]["F_CD"].ToString();
                    nonlistedCompanyDropDownList.SelectedValue = dtNonlistedSecuritiesByCOMPCODE.Rows[0]["COMP_CD"].ToString();
                    InvestMentDateTextBox.Text = dtNonlistedSecuritiesByCOMPCODE.Rows[0]["INV_DATE"].ToString();
                    Label2.Visible = false;
                    nonlistedCategoryDropDownList.Visible = false;
                    amountTextBox.Text = dtNonlistedSecuritiesByCOMPCODE.Rows[0]["AMOUNT"].ToString();
                    rateTextBox.Text = dtNonlistedSecuritiesByCOMPCODE.Rows[0]["RATE"].ToString();

                    noOfShareTextBox.Text = dtNonlistedSecuritiesByCOMPCODE.Rows[0]["NO_SHARES"].ToString();

                    saveButton.Text = "Update";
                    //saveButton.Text = "Update";
                }

            }




            DataTable dtFundNameDropDownList = dropDownListObj.FundNameDropDownList();
            DataTable dtCompanyDropdownlist = dropDownListObj.NolistedCompanyCodeNameDropDownList();
            DataTable dtnonlistedCategoryDropdownlist = dropDownListObj.NolistedCategoryTypeDropDownList();
            DataTable dtPortfolioAsOnDropDownList = BalanceDateDropDownList();
            //    DataTable dtNonlistedSecurities = NonlistedSecuritiesDetails();




            //grdShowDSEMP.DataSource = dtNonlistedSecurities;
            //grdShowDSEMP.DataBind();

            if (!IsPostBack)
            {
                fundNameDropDownList.DataSource = dtFundNameDropDownList;
                fundNameDropDownList.DataTextField = "F_NAME";
                fundNameDropDownList.DataValueField = "F_CD";
                fundNameDropDownList.DataBind();


                //PortfolioAsOnDropDownList.DataSource = dtPortfolioAsOnDropDownList;
                //PortfolioAsOnDropDownList.DataTextField = "Balance_Date";
                //PortfolioAsOnDropDownList.DataValueField = "bal_dt_ctrl";
                //PortfolioAsOnDropDownList.DataBind();

                nonlistedCompanyDropDownList.DataSource = dtCompanyDropdownlist;
                nonlistedCompanyDropDownList.DataTextField = "COMP_NM";
                nonlistedCompanyDropDownList.DataValueField = "COMP_CD";
                nonlistedCompanyDropDownList.DataBind();

                nonlistedCategoryDropDownList.DataSource = dtnonlistedCategoryDropdownlist;
                nonlistedCategoryDropDownList.DataTextField = "CAT_NM";
                nonlistedCategoryDropDownList.DataValueField = "CAT_ID";
                nonlistedCategoryDropDownList.DataBind();




            }
        }
    }
    protected void saveButton_Click(object sender, EventArgs e)
    {

        string pfolioMaxDate = "";
        string loginId = Session["UserID"].ToString();
        string invDate = InvestMentDateTextBox.Text.ToString();
        DataTable dtNonlistedMaxDate = commonGatewayObj.Select("Select TO_CHAR(MAX(INV_DATE), 'DD-MON-YYYY ') as INV_DATE   from NON_LISTED_SECURITIES where f_cd=" + fundNameDropDownList.SelectedValue.ToString()+"");
        if (dtNonlistedMaxDate != null && dtNonlistedMaxDate.Rows.Count > 0)
        {
            pfolioMaxDate = dtNonlistedMaxDate.Rows[0]["INV_DATE"].ToString();
        }
        DateTime dtNonlistedDate = DateTime.Parse(pfolioMaxDate);
        DateTime dtINVDATE = DateTime.ParseExact(invDate, "dd/MM/yyyy", null);

        if (dtNonlistedDate.Date > dtINVDATE.Date)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Investment date must be greater than or equal to : " + dtNonlistedDate.ToString("dd-MMM-yyyy") + "');", true);
        }
        else
        {
            //It's an earlier or equal date
            DataTable dtnonListedDetails = new DataTable();
            string strQuery = "Select F_CD,COMP_CD,AMOUNT,RATE,NO_SHARES,ENTRY_BY,ENTRY_DATE from NON_LISTED_SECURITIES_DETAILS where f_cd=" + fundNameDropDownList.SelectedValue.ToString() + " and COMP_CD="+ nonlistedCompanyDropDownList.SelectedValue.ToString() + " and INV_DATE='" + dtINVDATE.ToString("dd-MMM-yyyy") + "'";
            dtnonListedDetails = commonGatewayObj.Select(strQuery);
            if (dtnonListedDetails != null && dtnonListedDetails.Rows.Count > 0)
            {
                string strUPQuery = "update NON_LISTED_SECURITIES_DETAILS SET COMP_CD ='" + nonlistedCompanyDropDownList.SelectedValue.ToString() + "',AMOUNT =" + amountTextBox.Text.ToString() + ",RATE=" + rateTextBox.Text.ToString() + ",NO_SHARES =" + noOfShareTextBox.Text.ToString() + ",ENTRY_BY ='" + loginId + "',ENTRY_DATE ='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' where F_CD='" + fundNameDropDownList.SelectedValue.ToString() + "' and  INV_DATE='" + dtINVDATE.ToString("dd-MMM-yyyy") + "'";

                int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPQuery);

                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Update Sucessfully')", true);
            }
            else
            {
                string strInsQuery;

                strInsQuery = "insert into NON_LISTED_SECURITIES_DETAILS(F_CD,COMP_CD,AMOUNT,RATE,NO_SHARES,INV_DATE,ENTRY_BY,ENTRY_DATE)values('" + fundNameDropDownList.SelectedValue.ToString() + "','" + nonlistedCompanyDropDownList.SelectedValue.ToString() + "','" + amountTextBox.Text.ToString() + "','" + rateTextBox.Text.ToString() + "','" + noOfShareTextBox.Text.ToString() + "','" + dtINVDATE.ToString("dd-MMM-yyyy") + "','" + loginId + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "')";

                int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Insert Sucessfully')", true);

            }
            Response.Redirect("NonListedSecurities.aspx");
        }

       
    }

    public DataTable BalanceDateDropDownList()//Get Howla Date from invest.fund_trans_hb Table
    {
        DataTable dtHowlaDate = commonGatewayObj.Select("select distinct bal_dt_ctrl from pfolio_bk order by bal_dt_ctrl desc");
        DataTable dtHowlaDateDropDownList = new DataTable();
        dtHowlaDateDropDownList.Columns.Add("Balance_Date", typeof(string));
        dtHowlaDateDropDownList.Columns.Add("bal_dt_ctrl", typeof(string));
        DataRow dr = dtHowlaDateDropDownList.NewRow();
        dr["Balance_Date"] = "--Select--";
        dr["bal_dt_ctrl"] = "0";
        dtHowlaDateDropDownList.Rows.Add(dr);
        for (int loop = 0; loop < dtHowlaDate.Rows.Count; loop++)
        {
            dr = dtHowlaDateDropDownList.NewRow();
            dr["Balance_Date"] = Convert.ToDateTime(dtHowlaDate.Rows[loop]["bal_dt_ctrl"]).ToString("dd-MMM-yyyy");
            dr["bal_dt_ctrl"] = Convert.ToDateTime(dtHowlaDate.Rows[loop]["bal_dt_ctrl"]).ToString("dd-MMM-yyyy");
            dtHowlaDateDropDownList.Rows.Add(dr);
        }
        return dtHowlaDateDropDownList;
    }

    private void FillNonListedSecuritiesGrid()
    {

        string strQuery, strQueryMaxinvDate, invDate;
        DataTable dt,dtMaxInvDate;
      //  DateTime date1 = DateTime.ParseExact(InvestMentDateTextBox.Text, "dd/MM/yyyy", null);


       // TO_CHAR(max(vch_dt), 'DD-MON-YYYY')last_tr_dt

         //   clsDataHandle selectData = new clsDataHandle();
         strQueryMaxinvDate = "select TO_CHAR(max(INV_DATE), 'DD-MON-YYYY')inv_date from NON_LISTED_SECURITIES where f_cd=" + fundNameDropDownList.SelectedValue.ToString();
        dtMaxInvDate = commonGatewayObj.Select(strQueryMaxinvDate);

        if (!dtMaxInvDate.Rows[0].IsNull("inv_date"))
        {
            invDate = dtMaxInvDate.Rows[0]["inv_date"].ToString();
          //  invDate = Convert.ToDateTime(dt.Rows[0]["vch_dt"].ToString()).ToString("dd-MMM-yyyy");
        }
        else
        {
            invDate = "01-Jan-1970";
        }
            strQuery = "SELECT  F_CD,COMP_CD,AMOUNT,RATE,NO_SHARES,TO_CHAR(INV_DATE, 'DD-MON-YYYY') as  INV_DATE  FROM NON_LISTED_SECURITIES_DETAILS" +
            " where F_CD=" + fundNameDropDownList.SelectedValue.ToString() + " and inv_date>='"+ dtMaxInvDate.Rows[0]["inv_date"].ToString()+"'";


        try
        {

            dt = commonGatewayObj.Select(strQuery);
            GridViewNonListedSecurities.DataSource = dt;
            GridViewNonListedSecurities.DataBind();


        }

        catch (Exception err)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "TScript", "alert('No data found')" + err.ToString(), true);
        }

    }

  

    protected void rateChange_TextChanged(object sender, EventArgs e)
    {

        if (amountTextBox.Text != "" && rateTextBox.Text != "")
        {
            double ammount = Convert.ToDouble(amountTextBox.Text);
            double rate = Convert.ToDouble(rateTextBox.Text);
            double noOfShare = (ammount / rate);
            int share = Convert.ToInt32(noOfShare);
           
            noOfShareTextBox.Text = share.ToString();
          //  ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Ammount" + noOfShareTextBox.Text.ToString() + "');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Please Enter Ammount !');", true);
        }
        
       
    }

    public string UpdateAmmountInNonlisted(string FundId, string Date,string Ammount)
    {

        CommonGateway commonGatewayObj = new CommonGateway();
       
        string strUpdateNonlisted = "";
        string message = "";
        if (FundId != "")
        {

            strUpdateNonlisted = "UPDATE NON_LISTED_SECURITIES SET INV_AMOUNT = '"+ Convert.ToDouble(Ammount) + "' WHERE F_CD = " + FundId+" and INV_DATE ='"+Date+ "'";

            int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUpdateNonlisted);
            if (NumOfRows > 0)
            {
                message = "Data Updated Successfully";
            }


        }

        return message;

    }

    [System.Web.Services.WebMethod]

    public static string RATEONCHANGE(string Ammount, string Rate)
    {
        string noofSare = "";
      if (Ammount != "" && Rate != "")
        {
            double ammount = Convert.ToDouble(Rate);
            double rate = Convert.ToDouble(Rate);
            double noOfShare = (ammount / rate);
            int share = Convert.ToInt32(noOfShare);
            noofSare = share.ToString();
        }

        return noofSare;

    }


    public DataTable NonlistedSecuritiesDetails()
    {
        DataTable dtNonlistedDetails = commonGatewayObj.Select("  SELECT  F_CD,COMP_CD,AMOUNT,RATE,NO_SHARES,TO_CHAR(INV_DATE, 'DD-MON-YYYY') as  INV_DATE,ENTRY_BY,  TO_CHAR (ENTRY_DATE, 'DD-MON-YYYY') as  ENTRY_DATE  FROM NON_LISTED_SECURITIES_DETAILS   ");
        return dtNonlistedDetails;
    }
    public DataTable NonlistedSecuritiesDetailsByCOMPID(string COMPCODE)
    {
        DataTable dtNonlistedDetails = commonGatewayObj.Select("  SELECT  F_CD,COMP_CD,AMOUNT,RATE,NO_SHARES,TO_CHAR(INV_DATE, 'DD-MON-YYYY') as  INV_DATE,ENTRY_BY,  TO_CHAR (ENTRY_DATE, 'DD-MON-YYYY') as  ENTRY_DATE  FROM NON_LISTED_SECURITIES_DETAILS  where comp_cd="+COMPCODE+" ");
        return dtNonlistedDetails;
    }

    public void ClearFields()
    {
        amountTextBox.Text = "";
      
    }

    protected void GridViewNonListedSecurities_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }

    //protected void grdMemoInformation_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    string strQuery, strSumMemo, strDtlMemo, SummaryMemoPDFPath, DetailMemoPDFPath;
    //    DataTable dt;
    //    clsDataHandle selectData = new clsDataHandle();
    //    txtMemoNo_Unicode.ReadOnly = true; //Added for AG
    //    strQuery = "select a.meeting_type_id,b.meeting_type_name,a.meeting_no,a.agenda_no,a.br_div_dept_cd,a.br_div_dept_nm,a.subject_unicode,a.hints1,a.hints2,a.hints3,a.ShortSubject_Unicode,a.memo_no_unicode,a.sum_memo,a.dtl_memo" +
    //        " from agrani_memo_inf a, agrani_met_type b where a.meeting_type_id=(select meeting_type_id from agrani_met_type where meeting_type_name='" +
    //        grdMemoInformation.SelectedRow.Cells[0].Text.ToString() + "')" + " and a.meeting_no=" + grdMemoInformation.SelectedRow.Cells[1].Text.ToString() +
    //        " and a.agenda_no=" + grdMemoInformation.SelectedRow.Cells[2].Text.ToString() +
    //        " and a.meeting_type_id=b.meeting_type_id";

    //    try
    //    {

    //        dt = selectData.SelectDataTable(strQuery);
    //        drpDwnLstMeetingType.SelectedValue = dt.Rows[0]["meeting_type_id"].ToString();
    //        drpDwnLstMeetingType.SelectedItem.Text = dt.Rows[0]["meeting_type_name"].ToString();

    //        drpDwnLstMeetingNo.SelectedItem.Text = dt.Rows[0]["meeting_no"].ToString();


    //        txtAgendaSL.Text = dt.Rows[0]["agenda_no"].ToString();
    //        drpdwnlstDivOrDeptName.SelectedValue = dt.Rows[0]["br_div_dept_cd"].ToString();
    //        drpdwnlstDivOrDeptName.SelectedItem.Text = dt.Rows[0]["br_div_dept_nm"].ToString();

    //        txtSubject_Unicode.Text = dt.Rows[0]["subject_Unicode"].ToString();
    //        drpDwnlstHints1.SelectedItem.Text = dt.Rows[0]["hints1"].ToString();
    //        drpDwnlstHints2.SelectedItem.Text = dt.Rows[0]["hints2"].ToString();
    //        drpDwnlstHints3.SelectedItem.Text = dt.Rows[0]["hints3"].ToString();
    //        txtShortSubject_Unicode.Text = dt.Rows[0]["ShortSubject_Unicode"].ToString();
    //        txtMemoNo_Unicode.Text = dt.Rows[0]["memo_no_Unicode"].ToString();



    //        if (dt.Rows[0]["sum_memo"].ToString().Length == 19 || dt.Rows[0]["sum_memo"].ToString().Length == 0)// Since,if no file uploaded then default file name will be of 19 characters
    //        {
    //            HyperLinkSummaryMemoPDf.Enabled = false;
    //            HyperLinkSummaryMemoPDf.Visible = false;
    //        }

    //        else
    //        {
    //            HyperLinkSummaryMemoPDf.Enabled = true;
    //            HyperLinkSummaryMemoPDf.Visible = true;
    //            SummaryMemoPDFPath = "~/UploadedSummaryMemo/" + dt.Rows[0]["sum_memo"].ToString();
    //            HyperLinkSummaryMemoPDf.NavigateUrl = SummaryMemoPDFPath;
    //        }

    //        if (dt.Rows[0]["dtl_memo"].ToString().Length == 19 || dt.Rows[0]["dtl_memo"].ToString().Length == 0) // Since,if no file uploaded then default file name will be of 19 characters
    //        {

    //            HyperLinkDetailMemoPDf.Enabled = false;
    //            HyperLinkDetailMemoPDf.Visible = false;

    //        }
    //        else
    //        {
    //            HyperLinkDetailMemoPDf.Enabled = true;
    //            HyperLinkDetailMemoPDf.Visible = true;
    //            DetailMemoPDFPath = "~/UploadedDetailMemo/" + dt.Rows[0]["dtl_memo"].ToString();

    //            HyperLinkDetailMemoPDf.NavigateUrl = DetailMemoPDFPath;
    //        }


    //        drpDwnLstMeetingType.Enabled = false;
    //        drpDwnLstMeetingNo.Enabled = false;
    //        txtAgendaSL.Enabled = false;
    //        InsFlag = false;
    //    }

    //    catch (Exception err)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "TScript", "alert('No data found')" + err.ToString(), true);
    //    }


    //}
    protected void GridViewNonListedSecurities_SelectedIndexChanged(object sender, EventArgs e)
    {
        nonlistedCompanyDropDownList.SelectedIndex = -1;
        string strQuery;
        DataTable dtNonListedDetails;
        //strQuery = "select a.meeting_type_id,b.meeting_type_name,a.meeting_no,a.agenda_no,a.br_div_dept_cd,a.br_div_dept_nm,a.subject_unicode,a.hints1,a.hints2,a.hints3,a.ShortSubject_Unicode,a.memo_no_unicode,a.sum_memo,a.dtl_memo" +
        //    " from agrani_memo_inf a, agrani_met_type b where a.meeting_type_id=(select meeting_type_id from agrani_met_type where meeting_type_name='" +
        //    grdMemoInformation.SelectedRow.Cells[0].Text.ToString() + "')" + " and a.meeting_no=" + grdMemoInformation.SelectedRow.Cells[1].Text.ToString() +
        //    " and a.agenda_no=" + grdMemoInformation.SelectedRow.Cells[2].Text.ToString() +
        //    " and a.meeting_type_id=b.meeting_type_id";


        //strQuery ="SELECT F_CD, COMP_CD, AMOUNT, RATE, NO_SHARES, TO_CHAR(INV_DATE, 'DD-MON-YYYY') as INV_DATE  FROM NON_LISTED_SECURITIES_DETAILS"+
        //    " where F_CD=" + fundNameDropDownList.SelectedValue.ToString() + " and inv_date>='" + dtMaxInvDate.Rows[0]["inv_date"].ToString() + "'";

        strQuery = "SELECT  F_CD,COMP_CD,AMOUNT,RATE,NO_SHARES,TO_CHAR(INV_DATE, 'DD-MON-YYYY') as  INV_DATE  FROM NON_LISTED_SECURITIES_DETAILS" +
           " where F_CD=" + GridViewNonListedSecurities.SelectedRow.Cells[0].Text.ToString() + " and inv_date='" + GridViewNonListedSecurities.SelectedRow.Cells[5].Text.ToString() +
           "' and comp_cd="+ GridViewNonListedSecurities.SelectedRow.Cells[1].Text.ToString();
           
        dtNonListedDetails = commonGatewayObj.Select(strQuery);
        fundNameDropDownList.SelectedValue= dtNonListedDetails.Rows[0]["F_CD"].ToString();
        nonlistedCompanyDropDownList.SelectedValue = dtNonListedDetails.Rows[0]["COMP_CD"].ToString();
        amountTextBox.Text = dtNonListedDetails.Rows[0]["AMOUNT"].ToString();
        rateTextBox.Text = dtNonListedDetails.Rows[0]["RATE"].ToString();
        noOfShareTextBox.Text=dtNonListedDetails.Rows[0]["NO_SHARES"].ToString();

    }

    protected void GridViewNonListedSecurities_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridViewNonListedSecurities, "Select$" + e.Row.RowIndex);
        }
    }

    protected void fundNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillNonListedSecuritiesGrid();
    }
}
