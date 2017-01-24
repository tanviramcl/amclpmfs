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
        DataTable dtBalanceDate = getbalanceDate();
        if (!IsPostBack)
        {
            txtbalanceDate1.Text = dtBalanceDate.Rows[0]["balancedate1"].ToString();
            txtbalanceDate2.Text = dtBalanceDate.Rows[0]["balancedate2"].ToString();
           
        }

    }


    protected void txtPurchaseRecord_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txttotalRowCount_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtbalanceDate2_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtbalanceDate1_TextChanged(object sender, EventArgs e)
    {

    }

    public DataTable getbalanceDate()//For Authorized Signatory
    {

        CommonGateway commonGatewayObj = new CommonGateway();
        DataTable pdate = commonGatewayObj.Select("select max(bal_dt_ctrl)+1as BalanceDate  from pfolio_bk");
        DataTable pdateDropDownList = new DataTable();
        pdateDropDownList.Columns.Add("balancedate1", typeof(string));
        pdateDropDownList.Columns.Add("balancedate2", typeof(string));
        DataRow dr = pdateDropDownList.NewRow();

        for (int loop = 0; loop < pdate.Rows.Count; loop++)
        {
            dr = pdateDropDownList.NewRow();
            dr["balancedate1"] = Convert.ToDateTime(pdate.Rows[loop]["BalanceDate"]).ToString("dd-MMM-yyyy");
            dr["balancedate2"] = Convert.ToDateTime(pdate.Rows[loop]["BalanceDate"]).ToString("dd-MMM-yyyy");
            pdateDropDownList.Rows.Add(dr);
        }
        return pdateDropDownList;
    }
}
