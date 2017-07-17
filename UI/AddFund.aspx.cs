using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UI_CompanyInformation : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../Default.aspx");
        }

        

        Session["funds"] = GetFundName();

        DataTable dtNoOfFunds = (DataTable)Session["funds"];



        //  companyNameTextBox.Text = "sss";
    }

    protected void saveButton_Click(object sender, EventArgs e)
    {
        insertdata();

    }
    protected void fundCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        DataTable dtgetfund  ;
        if (fundcodeTextBox.Text.ToString() != "")
        {
            string strfundcode = "SELECT  *  FROM    FUND WHERE    BOID IS NOT NULL  and f_cd = " + fundcodeTextBox.Text.ToString() + "";
            dtgetfund = commonGatewayObj.Select(strfundcode);
            if (dtgetfund != null && dtgetfund.Rows.Count > 0)
            {
                txtfundName.Value = dtgetfund.Rows[0]["F_NAME"].ToString();
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('This fund is already available !');", true);

            }
        }

    
    }

    private void insertdata()
    {
      
        DataTable fundinfo = new DataTable();
        string strInsQuery;
      
            strInsQuery = "insert into Fund(F_CD,F_NAME,COMP_CD,F_TYPE,IS_F_CLOSE,CUSTOMER,BOID,SL_BUY_COM_PCT)values('" + Convert.ToUInt32(fundcodeTextBox.Text.ToString()) + "','" + txtfundName.Value.ToString() + "','" + txtCompanyCode.Text.ToString() + "','" + FundTypeDropDownList.Text.ToString() + "','" + txtfundClose.Text.ToString() + "','" + customerCode.Text.ToString() + "','" + boIdTextBox.Text.ToString() + "','" + txtsellbuycommision.Text.ToString() + "')";
        

          int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Fund Insert Sucessfylly !');", true);

    }

    private void updatedata()
    {

        //DataTable companyinfoupdate = new DataTable();


        //string strUPQuery = "update comp set instr_cd ='" + dsecodeTextBox.Text.ToString() + "',comp_nm ='" + companyNameTextBox.Text.ToString() + "',cseinstr_cd ='" + csecodeTextBox.Text.ToString() + "',trade_meth ='" + groupDropDownList.SelectedValue.ToString() + "',add1 ='" + addressTextBox1.Value.ToString() + "',add2 ='" + addressTextBox2.Value.ToString() + "',reg_off ='" + regofficeTextBox2.Text.ToString() + "',tel ='" + phnNoTextBox.Text.ToString() + "',atho_cap = '" + TextBoxPaidUpCapital.Text.ToString() + "',paid_cap ='" + TextBoxPaidUpCapital.Text.ToString() + "',no_shrs ='" + totalshareTextBox.Text.ToString() + "',fc_val ='" + faceValueTextBox.Text.ToString() + "',mlot ='" + MarketLotTextBox.Text.ToString() + "',flot_dt_fm ='" + floatdatefromTextBox.Text.ToString() + "',flot_dt_to ='" + floatdatetoTextBox.Text.ToString() + "',rissu_dt_fm ='" + RIssuefromTextBox.Text.ToString() + "',rissu_dt_to ='" + RIssuetoTextBox.Text.ToString() + "',sbase_rt ='" + baserateTextBox.Text.ToString() + "',base_upd_dt ='" + baseupdateDateTextBox.Text.ToString() + "',avg_rt ='" + avarageMarketRateTextBox.Text.ToString() + "',rt_upd_dt ='" + lasttradingdateTextBox.Text.ToString() + "',margin = '" + merginTextBox.Text.ToString() + "',premium ='" + premiumTextBox.Text.ToString() + "',sect_maj_cd ='" + sectorTextBox.Text.ToString() + "',opn_dt ='" + openingdateTextBox.Text.ToString() + "',cat_tp ='" + categoryDropDownList.SelectedValue.ToString() + "' where comp_cd =" + companyCodeTextBox.Text.ToString() + "";

        //int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPQuery);
        //ClearFields();

    }


    private DataTable GetFundName()
    {
        DataTable dtFundName = new DataTable();

        StringBuilder sbMst = new StringBuilder();
        StringBuilder sbOrderBy = new StringBuilder();
        sbOrderBy.Append("");
        sbMst.Append(" SELECT     *     FROM         FUND  ");
        sbMst.Append(" WHERE    BOID IS NOT NULL ");
        sbOrderBy.Append(" ORDER BY FUND.F_CD ");

        sbMst.Append(sbOrderBy.ToString());
        dtFundName = commonGatewayObj.Select(sbMst.ToString());
    
        Session["dtFundName"] = dtFundName;
        return dtFundName;
    }

    private void ClearField()
    {

    }

    public class Fund
    {
        public int  F_CD { get; set; }
        public string F_NAME { get; set; }
        public int COMP_CD { get; set; }
        public string F_TYPE { get; set; }
        public string IS_F_CLOSE { get; set; }
        public string CUSTOMER { get; set; }
        public string BOID { get; set; }
        public double SL_BUY_COM_PCT { get; set; }

    }


}