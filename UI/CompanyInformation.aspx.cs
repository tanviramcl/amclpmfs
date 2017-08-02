using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

      //  companyNameTextBox.Text = "sss";
    }

    protected void saveButton_Click(object sender, EventArgs e)
    {

        DataTable dtsource = new DataTable();
        string Query = "select comp_nm,mlot,fc_val,avg_rt,rt_upd_dt,instr_cd,cseinstr_cd,trade_meth,sect_maj_cd,cat_tp,add1,add2,reg_off,opn_dt,tel,prod,atho_cap,paid_cap,no_shrs,sbase_rt,base_upd_dt,flot_dt_fm,flot_dt_to,margin,flag,premium,rissu_dt_fm,rissu_dt_to from comp where comp_cd ='" + companyCodeTextBox.Text + "'";

        dtsource = commonGatewayObj.Select(Query.ToString());
        if (dtsource.Rows.Count > 0)
        {

            updatedata();
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Update Successfully');", true);
        }
        else {

            insertdata();
            ClearFields();
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);

        }
        System.Threading.Thread.Sleep(5000);

    }


    private void insertdata()
    {
       
        DataTable companyinfo = new DataTable();
       

        string strInsQuery = "insert into comp(comp_cd,comp_nm,instr_cd,paid_cap,no_shrs,fc_val,mlot, sbase_rt, base_upd_dt, avg_rt,rt_upd_dt,cat_tp,sect_maj_cd,trade_meth,flot_dt_fm,flot_dt_to,cseinstr_cd,flag,add1,add2,reg_off,tel,opn_dt,premium,rissu_dt_fm,rissu_dt_to,margin)values('" +Convert.ToUInt32(companyCodeTextBox.Text.ToString())+"','"+companyNameTextBox.Text.ToString()+"','"+dsecodeTextBox.Text.ToString()+ "','"+Convert.ToUInt32(TextBoxPaidUpCapital.Text.ToString())+ "','" + Convert.ToUInt32(totalshareTextBox.Text.ToString()) + "','" + Convert.ToUInt32(faceValueTextBox.Text.ToString()) + "','" + Convert.ToUInt32(MarketLotTextBox.Text.ToString()) + "','" + Convert.ToUInt32(baserateTextBox.Text.ToString()) + "','"+baseupdateDateTextBox.Text.ToString()+ "','"+avarageMarketRateTextBox.Text.ToString()+"','"+lasttradingdateTextBox.Text.ToString()+ "','"+categoryDropDownList.SelectedValue.ToString()+ "','"+sectorTextBox.Text.ToString()+"','"+groupDropDownList.SelectedValue.ToString()+ "','"+floatdatefromTextBox.Text.ToString()+ "','" + floatdatetoTextBox.Text.ToString() + "','"+csecodeTextBox.Text.ToString()+ "','" + flugTextBox.Text.ToString() + "','" + addressTextBox1.Value.ToString() + "','" + addressTextBox2.Value.ToString() + "','" +regofficeTextBox2.Text.ToString() + "','" + phnNoTextBox.Text.ToString() + "','"+openingdateTextBox.Text.ToString()+"','"+premiumTextBox.Text.ToString()+"','"+RIssuefromTextBox.Text.ToString()+"','"+ RIssuetoTextBox.Text.ToString()+"','"+ merginTextBox.Text.ToString()+ "')";

         int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
      //  ClearFields();
       
    }

    private void updatedata()
    {

        DataTable companyinfoupdate = new DataTable();

      
        string strUPQuery = "update comp set instr_cd ='"+dsecodeTextBox.Text.ToString()+ "',comp_nm ='"+companyNameTextBox.Text.ToString()+ "',cseinstr_cd ='"+csecodeTextBox.Text.ToString()+"',trade_meth ='"+groupDropDownList.SelectedValue.ToString()+"',add1 ='"+addressTextBox1.Value.ToString()+"',add2 ='"+addressTextBox2.Value.ToString()+"',reg_off ='"+regofficeTextBox2.Text.ToString()+"',tel ='"+phnNoTextBox.Text.ToString()+"',atho_cap = '"+TextBoxPaidUpCapital.Text.ToString()+"',paid_cap ='"+TextBoxPaidUpCapital.Text.ToString()+"',no_shrs ='"+totalshareTextBox.Text.ToString()+"',fc_val ='"+faceValueTextBox.Text.ToString()+"',mlot ='"+MarketLotTextBox.Text.ToString()+"',flot_dt_fm ='"+floatdatefromTextBox.Text.ToString()+"',flot_dt_to ='"+floatdatetoTextBox.Text.ToString()+"',rissu_dt_fm ='"+RIssuefromTextBox.Text.ToString()+"',rissu_dt_to ='"+RIssuetoTextBox.Text.ToString()+"',sbase_rt ='"+baserateTextBox.Text.ToString()+"',base_upd_dt ='"+baseupdateDateTextBox.Text.ToString()+"',avg_rt ='"+avarageMarketRateTextBox.Text.ToString()+"',rt_upd_dt ='"+lasttradingdateTextBox.Text.ToString()+"',margin = '"+merginTextBox.Text.ToString()+"',premium ='"+premiumTextBox.Text.ToString()+"',sect_maj_cd ='"+sectorTextBox.Text.ToString()+"',opn_dt ='"+openingdateTextBox.Text.ToString()+"',cat_tp ='"+categoryDropDownList.SelectedValue.ToString()+"' where comp_cd ="+companyCodeTextBox.Text.ToString()+"";
    
        int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPQuery);
          ClearFields();

    }




    protected void companyCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        DataTable dtsource = new DataTable();
        List<CompanyInfo> companyInfolist = new List<CompanyInfo>();
        string Query = "select comp_nm,mlot,fc_val,avg_rt,TO_CHAR(rt_upd_dt,'dd-MON-yyyy')rt_upd_dt,instr_cd,cseinstr_cd,trade_meth,sect_maj_cd,cat_tp,add1,add2,reg_off,TO_CHAR(opn_dt,'dd-MON-yyyy')opn_dt,tel,prod,atho_cap,paid_cap,no_shrs,sbase_rt,TO_CHAR(base_upd_dt,'dd-MON-yyyy')base_upd_dt,TO_Char(flot_dt_fm,'dd-MON-yyyy')flot_dt_fm,TO_CHAR(flot_dt_to,'dd-MON-yyyy')flot_dt_to,margin,flag,premium,TO_CHAR(rissu_dt_fm,'dd-MON-yyyy')rissu_dt_fm,TO_CHAR(rissu_dt_to,'dd-MON-yyyy')rissu_dt_to from comp where comp_cd ='" + companyCodeTextBox.Text+"'";

        dtsource = commonGatewayObj.Select(Query.ToString());

        if (dtsource.Rows.Count > 0)
        {

            companyInfolist = (from DataRow dr in dtsource.Rows
                               select new CompanyInfo()
                               {
                                   // COMP_CD = dr["COMP_CD"].ToString(),
                                   COMP_NM = dr["COMP_NM"].ToString(),
                                   SECT_MAJ_CD = dr["SECT_MAJ_CD"].ToString(),
                                   CAT_TP = dr["CAT_TP"].ToString(),
                                   MLOT = dr["MLOT"].ToString(),
                                   AVG_RT = dr["AVG_RT"].ToString(),
                                   FC_VAL = dr["FC_VAL"].ToString(),
                                   RT_UPD_DT = dr["RT_UPD_DT"].ToString(),
                                   INSTR_CD = dr["INSTR_CD"].ToString(),
                                   //   CSE_INSTR_CD = dr["CSE_INSTR_CD"].ToString(),
                                   TRADE_METH = dr["TRADE_METH"].ToString(),
                                   OPN_DT = dr["OPN_DT"].ToString(),
                                   ADD1 = dr["ADD1"].ToString(),
                                   ADD2 = dr["ADD2"].ToString(),
                                   TEL = dr["TEL"].ToString(),
                                   ATHO_CAP = dr["ATHO_CAP"].ToString(),
                                   REG_OFF = dr["REG_OFF"].ToString(),
                                   PAID_CAP = dr["PAID_CAP"].ToString(),
                                   NO_SHRS = dr["NO_SHRS"].ToString(),
                                   SBASE_RT = dr["SBASE_RT"].ToString(),
                                   PROD = dr["PROD"].ToString(),
                                   BASE_UPD_DT = dr["BASE_UPD_DT"].ToString(),
                                   FLOT_DT_FM = dr["FLOT_DT_FM"].ToString(),
                                   FLOT_DT_TO = dr["FLOT_DT_TO"].ToString(),
                                   RISSU_DT_FM = dr["RISSU_DT_FM"].ToString(),
                                   RISSU_DT_TO = dr["RISSU_DT_TO"].ToString(),
                                   MARGIN = dr["MARGIN"].ToString(),
                                   PREMIUM = dr["PREMIUM"].ToString(),
                                   FLAG = dr["FLAG"].ToString(),
                               }).ToList();


            foreach (CompanyInfo compInfo in companyInfolist)
            {
                // companyCodeTextBox.Text = ;
                companyNameTextBox.Text = compInfo.COMP_NM;
                dsecodeTextBox.Text = compInfo.INSTR_CD;
                TextBoxPaidUpCapital.Text = compInfo.PAID_CAP;
                totalshareTextBox.Text = compInfo.NO_SHRS;
                faceValueTextBox.Text = compInfo.FC_VAL;
                TextBoxPaidUpCapital.Text = compInfo.PAID_CAP;
                MarketLotTextBox.Text = compInfo.MLOT;
                baserateTextBox.Text = compInfo.SBASE_RT;
                baseupdateDateTextBox.Text = compInfo.BASE_UPD_DT;
                avarageMarketRateTextBox.Text = compInfo.AVG_RT;
                lasttradingdateTextBox.Text = compInfo.RT_UPD_DT;
                categoryDropDownList.SelectedValue = compInfo.CAT_TP;
                sectorTextBox.Text = compInfo.SECT_MAJ_CD;
                groupDropDownList.SelectedValue = compInfo.TRADE_METH;
                floatdatefromTextBox.Text = compInfo.FLOT_DT_FM;
                floatdatetoTextBox.Text = compInfo.FLOT_DT_TO;
                csecodeTextBox.Text = compInfo.CSE_INSTR_CD;
                flugTextBox.Text = compInfo.FLAG;
                addressTextBox1.Value = compInfo.ADD1;
                addressTextBox2.Value = compInfo.ADD2;
                regofficeTextBox2.Text = compInfo.REG_OFF;
                phnNoTextBox.Text = compInfo.TEL;
                openingdateTextBox.Text = compInfo.OPN_DT;
                premiumTextBox.Text = compInfo.PREMIUM;
                RIssuefromTextBox.Text = compInfo.RISSU_DT_FM;
                RIssuetoTextBox.Text = compInfo.RISSU_DT_TO;
                merginTextBox.Text = compInfo.MARGIN;
                TextBoxAuthorizedcapital.Text = compInfo.ATHO_CAP;
            }
        }
        else
        {
            ClearFields();

        }

    }


    private void ClearFields()
    {
      //  companyCodeTextBox.Text = "";
        companyNameTextBox.Text = "";
        dsecodeTextBox.Text = "";
        TextBoxPaidUpCapital.Text = "";
        totalshareTextBox.Text = "";
        faceValueTextBox.Text = "";
        TextBoxPaidUpCapital.Text = "";
        MarketLotTextBox.Text = "";
        baserateTextBox.Text = "";
        baseupdateDateTextBox.Text = "";
        avarageMarketRateTextBox.Text = "";
        lasttradingdateTextBox.Text = "";
        categoryDropDownList.SelectedValue = "";
        sectorTextBox.Text = "";
        groupDropDownList.SelectedValue = "";
        floatdatefromTextBox.Text = "";
        floatdatetoTextBox.Text = "";
        csecodeTextBox.Text = "";
        flugTextBox.Text = "";
        addressTextBox1.Value = "";
        addressTextBox2.Value = "";
        regofficeTextBox2.Text = "";
        phnNoTextBox.Text = "";
        openingdateTextBox.Text = "";
        premiumTextBox.Text = "";
        RIssuefromTextBox.Text = "";
        RIssuetoTextBox.Text = "";
        merginTextBox.Text = "";
        TextBoxAuthorizedcapital.Text = "";
        productTextBox.Text = "";
    }

    public class CompanyInfo
    {
        public string COMP_CD { get; set; }
        public string COMP_NM { get; set; }
        public string SECT_MAJ_CD { get; set; }
        public string CAT_TP { get; set; }
        public string MLOT { get; set; }
        public string AVG_RT { get; set; }
        public string FC_VAL { get; set; }
        public string RT_UPD_DT { get; set; }
        public string INSTR_CD { get; set; }
        public string CSE_INSTR_CD { get; set; }
        public string TRADE_METH { get; set; }
        public string OPN_DT { get; set; }
        public string ADD1 { get; set; }
        public string ADD2 { get; set; }
        public string TEL { get; set; }
        public string ATHO_CAP { get; set; }
        public string REG_OFF { get; set; }
        public string PAID_CAP { get; set; }
        public string NO_SHRS { get; set; }
        public string SBASE_RT { get; set; }
        public string PROD { get; set; }
        public string BASE_UPD_DT { get; set; }
        public string FLOT_DT_FM { get; set; }
        public string FLOT_DT_TO { get; set; }
        public string RISSU_DT_FM { get; set; }
        public string RISSU_DT_TO { get; set; }
        public string MARGIN { get; set; }
        public string PREMIUM { get; set; }
        public string FLAG { get; set; }
     

    }

  
}