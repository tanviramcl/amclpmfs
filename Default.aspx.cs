using System;
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



public partial class _Default : System.Web.UI.Page 
{
    CommonGateway commonGatewayObj = new CommonGateway();
    protected void Page_Load(object sender, EventArgs e)

    {
        //Excel.ApplicationClass app = new Excel.ApplicationClass();
        loginErrorLabel.Visible = false;
        loginIDTextBox.Focus();
    }
    protected void loginButton_Click(object sender, EventArgs e)
    {
       // Captcha1.ValidateCaptcha(txtCaptcha.Text.Trim());

        //if (Captcha1.UserValidated)
        //{
            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "Valid";
            string loginId = EncodePasswordToBase64(loginIDTextBox.Text.Trim());
            if (IsUesrCheck(loginIDTextBox.Text.Trim().ToString(), loginPasswardTextBox.Text.Trim().ToString()))
            {
                Response.Redirect("UI/Home.aspx");
            }
            else
            {
                loginErrorLabel.Visible = true;
                loginErrorLabel.Text = "Invalid LoginID or Passward";
                loginIDTextBox.Text = "";
                loginPasswardTextBox.Text = "";
            }
        //}
        //else
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    lblMessage.Text = "InValid Captcha Code";
        //    txtCaptcha.Text = "";
        //    loginPasswardTextBox.Focus();
        //}
    }
    public bool IsUesrCheck(string loginID,string loginPassword)
    {
        DataTable dtUserInfo = new DataTable();
        dtUserInfo = commonGatewayObj.Select("SELECT * FROM USER_TABLE WHERE USER_ID='" + loginID + "' AND PASSWORD='" + loginPassword + "'");
        if (dtUserInfo.Rows.Count > 0)
        {
            string UserID = dtUserInfo.Rows[0]["USER_ID"].ToString();
            string UserName = dtUserInfo.Rows[0]["NAME"].ToString();
            string UserType = dtUserInfo.Rows[0]["USER_TYPE"].ToString();

            Session["UserID"] = UserID;
            Session["UserName"] = UserName;
            Session["UserType"] = UserType;
            return true;
        }
        else
        {
            return false;
        }
    }
    //this function Convert to Encord your Password 
    public static string EncodePasswordToBase64(string password)
    {
        try
        {
            byte[] encData_byte = new byte[password.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }
        catch (Exception ex)
        {
            throw new Exception("Error in base64Encode" + ex.Message);
        }
    } 
    //this function Convert to Decord your Password
    public string DecodeFrom64(string encodedData)
    {
        System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
        System.Text.Decoder utf8Decode = encoder.GetDecoder();
        byte[] todecode_byte = Convert.FromBase64String(encodedData);
        int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        char[] decoded_char = new char[charCount];
        utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        string result = new String(decoded_char);
        return result;
    }
}
