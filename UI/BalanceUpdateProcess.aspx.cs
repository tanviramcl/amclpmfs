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
        }

    }


    protected void txtFundcode_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtBalanceDate_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtlastUpadateDate_TextChanged(object sender, EventArgs e)
    {

    }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtSaleRecord_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtNoOfSaleShare2_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtNoOfSaleRecord2_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtNoOfSaleShare_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtPurchaseShares_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtPurchaseRecord_TextChanged(object sender, EventArgs e)
    {

    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void fundNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
