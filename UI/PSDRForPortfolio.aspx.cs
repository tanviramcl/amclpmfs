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

        DateTime date1 = DateTime.ParseExact(RIssuefromTextBox.Text, "dd/MM/yyyy", null);
        string fundcode = fundcodeTextBox.Text;
        string companycode = companyCodeTextBox.Text;
        string psdrNo = psdrNoTextBox.Text;
        string marketlot = oddormarketlotDropDownList.SelectedValue;
        string securType = securitiestypeDropDownList.SelectedValue;
        string sp_date = Convert.ToDateTime(date1).ToString("dd-MMM-yyyy");
        string folioNo = folioNoTextBox.Text;
        string alotmentNo = AllotmentNoTextBox.Text;
        string certificateNo = certificateNoTextBox.Text;
        int ditinctto= Convert.ToInt32(DictincttivetoTextBox.Text);
        int DictincFrom= Convert.ToInt32(DictincttivefromTextBox.Text);
        int noofShares = Convert.ToInt32(noofshareTextBox.Text);
        string loginId = Session["UserID"].ToString();

        if ((alotmentNo != "" && securType == "P") || (certificateNo != "" &&  ditinctto.ToString() != ""))
        {
            if ((certificateNo != "" && DictincFrom != 0))
            {
                ditinctto = (noofShares + DictincFrom) - 1;
            }

            string strInsQuery = "insert into PSDR_FI(F_CD,comp_cd,PSDR_NO,NO_SHARES,SH_TYPE,OM_LOT,SP_DATE,DIS_NO_FM,DIS_NO_TO, FOLIO_NO,CERT_NO,POSTED,OP_NAME) values('" + Convert.ToUInt32(fundcode) + "','" + Convert.ToInt32(companycode) + "','" + Convert.ToInt32(psdrNo) + "','" + noofShares + "','" + securType + "','" + marketlot + "','" + sp_date + "','" + DictincFrom + "','" + ditinctto + "','" + folioNo + "','" + certificateNo + "','A','"+loginId+"')";

            int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
            clearField();

        }

    }

    private void clearField()
    {
        fundcodeTextBox.Text = "";
        companyCodeTextBox.Text="";
        psdrNoTextBox.Text="";
        oddormarketlotDropDownList.SelectedValue = "0";
        securitiestypeDropDownList.SelectedValue="0"; 
        folioNoTextBox.Text="";
        AllotmentNoTextBox.Text="";
        certificateNoTextBox.Text="";
        DictincttivetoTextBox.Text="";
        DictincttivefromTextBox.Text="";
        noofshareTextBox.Text="";
        RIssuefromTextBox.Text = "";

    }

    protected void allotmentTextBox_TextChanged(object sender, EventArgs e)
    {

        DataTable dtsource = new DataTable();
        DataTable dtsource1 = new DataTable();
        List<PSDR_FI> psdrfilist = new List<PSDR_FI>();
        List<PSDR> psdrlist = new List<PSDR>();


    //    select allot_no into: psdr.allot_no
    //from invest.psdr_fi
    //where comp_cd = :psdr.comp_cd
    //and allot_no = ltrim(rtrim(:psdr.allot_no))
    // and: psdr.allot_no is not null
    // and: psdr.cert_no is null;
    //    message('Duplicate Allotment No. (Fund)');

        string Query1 = "", Query2 = "";

        //if (companyCodeTextBox.Text == "")
        //{
        //    Query1 = "select allot_no from psdr_fi where comp_cd = " + companyCodeTextBox.Text + " ";
        //    Query2 = "select allot_no from psdr where comp_cd = " + companyCodeTextBox.Text;
        //}
        //else
        
            Query1 = "select allot_no from psdr_fi where comp_cd = " + companyCodeTextBox.Text + " and allot_no = '" + AllotmentNoTextBox.Text + "' ";
            Query2 ="select allot_no from psdr where comp_cd = '" + companyCodeTextBox.Text + "' and allot_no = '" + AllotmentNoTextBox.Text + "' ";
        

        
        dtsource = commonGatewayObj.Select(Query1.ToString());
        
        dtsource1 = commonGatewayObj.Select(Query2.ToString());

        if (dtsource.Rows.Count > 0)
        {
            psdrfilist = (from DataRow dr in dtsource.Rows
                          select new PSDR_FI()
                          {
                              allot_no = dr["allot_no"].ToString()
                          }).ToList();
            foreach (PSDR_FI phdrfi in psdrfilist)
            {
                // companyCodeTextBox.Text = ;
                AllotmentNoTextBox.Text = phdrfi.allot_no;

            }


        }
        else if (dtsource1.Rows.Count > 0)
        {
            psdrlist = (from DataRow dr in dtsource.Rows
                        select new PSDR()
                        {
                            allot_no = dr["allot_no"].ToString()
                        }).ToList();
            foreach (PSDR phdr in psdrlist)
            {
                // companyCodeTextBox.Text = ;
                AllotmentNoTextBox.Text = phdr.allot_no;

            }
        }
       

    }
    
    protected void DictincttivefromTextBox__TextChanged(object sender, EventArgs e)
    {
        if ((certificateNoTextBox.Text).ToString() != "" && (DictincttivefromTextBox.Text).ToString() != "")
        {
            int value = (Convert.ToInt32(noofshareTextBox.Text) + Convert.ToInt32(DictincttivefromTextBox.Text)) - 1;

            DictincttivetoTextBox.Text = value.ToString();
        }
    }

    protected void certificateNoTextBox_TextChanged(object sender, EventArgs e)
    {
        DataTable dtsource = new DataTable();
        DataTable dtsource1 = new DataTable();
        List<PSDR_FI> psdrfilist = new List<PSDR_FI>();
        List<PSDR> psdrlist = new List<PSDR>();
        string Query1="",Query2="";

        //if (companyCodeTextBox.Text == "")
        //{
        //    Query1 = "select cert_no  from psdr_fi where  cert_no = '" + certificateNoTextBox.Text + "' ";
        //    Query2 = "select cert_no from psdr where  cert_no = '" + certificateNoTextBox.Text + "' ";
        //}
        //else
        //{

  //      PROCEDURE check_ckp IS
  //    nops  number:= 0;
  //      BEGIN
  //        select count(cert_no) into nops
  //from invest.psdr_fi
  // where comp_cd = :psdr.comp_cd
  // and cert_no = ltrim(:psdr.cert_no);
  //      if nops > 1 then
  //   message('Duplicate Certificate No. (Fund)');
  //      raise form_trigger_failure;
  //      elsif nops is null then
  //   message('Certificate Not Found');
  //      raise form_trigger_failure;
  //else
  //   :system.message_level := 0;
  //      end if;
  //      END;

        Query1 = "select cert_no  from psdr_fi where comp_cd = '" + companyCodeTextBox.Text + "' and cert_no = '" + certificateNoTextBox.Text + "' ";
            Query2 = "select cert_no from psdr where comp_cd = '" + companyCodeTextBox.Text + "' and cert_no = '" + certificateNoTextBox.Text + "' ";
        



        dtsource = commonGatewayObj.Select(Query1.ToString());

        dtsource1 = commonGatewayObj.Select(Query2.ToString());

        if (dtsource.Rows.Count > 0)
        {
            psdrfilist = (from DataRow dr in dtsource.Rows
                          select new PSDR_FI()
                          {
                              cert_no = dr["cert_no"].ToString()
                          }).ToList();
            foreach (PSDR_FI phdrfi in psdrfilist)
            {
                // companyCodeTextBox.Text = ;
                AllotmentNoTextBox.Text = phdrfi.allot_no;

            }


        }
        else if (dtsource1.Rows.Count > 0)
        {
            psdrlist = (from DataRow dr in dtsource.Rows
                        select new PSDR()
                        {
                            cert_no = dr["cert_no"].ToString()
                        }).ToList();
            foreach (PSDR phdr in psdrlist)
            {
                // companyCodeTextBox.Text = ;
                certificateNoTextBox.Text = phdr.cert_no;

            }
        }
      


    }

    protected void FundCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        // Fund name must be shown on label



    }

    protected void compCodeTextBox_TextChanged(object sender, EventArgs e)
    {

//        if :psdr.comp_cd = 150 then
//   message('Invalid Company Code');
//        raise form_trigger_failure;
//else
//  get_comp_cd(:psdr.comp_cd,:psdr.comp_name);
//        select mlot into: psdr.mlot from invest.comp
//                 where comp_cd =:psdr.comp_cd;
//        end if;
// Company name must be shown on label


    }



    public class PSDR_FI
    {
        public string allot_no { get; set; }
        public string cert_no { get; set; }
    }

    public class PSDR
    {
        public string allot_no { get; set; }
        public string cert_no { get; set; }
    }


}