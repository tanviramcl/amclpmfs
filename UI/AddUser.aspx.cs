using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UI_CompanyInformation : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();

    DropDownList dropDownListObj = new DropDownList();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../Default.aspx");
        }
        DataTable dtEmpwithdesNameDropDownList = dropDownListObj.UserNameDropDownList();
        DataTable dtUserTypeList = dropDownListObj.UserTypeDropDownList();
        if (!IsPostBack)
        {
            useNameDropDownList.DataSource = dtEmpwithdesNameDropDownList;
            useNameDropDownList.DataTextField = "Name";
            useNameDropDownList.DataValueField = "ID";
            useNameDropDownList.DataBind();

            userRoleDropDownList.DataSource = dtUserTypeList;
            userRoleDropDownList.DataTextField = "ROLE_NAME";
            userRoleDropDownList.DataValueField = "ROLE_ID";
            userRoleDropDownList.DataBind();


        }


    }


    protected void UserNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {

        string userId=useNameDropDownList.SelectedValue;
       // string Name = useNameDropDownList.SelectedItem.Text.ToString();
        DataTable dtFromUserList = new DataTable();
       string strdESFromempQuery = "select a.ID,a.Name,B.ID as DesignationID,B.NAME as DesignationName from (select * from emp_info where valid='Y'   order by Id asc ) a  inner join EMP_DESIGNATION  b  on a.DESIG_ID=B.ID where a.ID='"+ userId + "'";
        dtFromUserList = commonGatewayObj.Select(strdESFromempQuery);
        userDesignationTextBox.Text = dtFromUserList.Rows[0]["DesignationName"].ToString();

    }

    protected void ButtonSave_Click(object sender, EventArgs e)
    {
       // System.Threading.Thread.Sleep(5000);
    }
}