using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using DBSqlLib;
using EncryptionEngine;

namespace HRSystem.SysMan
{
    public partial class AddUser : System.Web.UI.Page
    {
        HrSystem.MD5.ProcessPassword procPwd = new HrSystem.MD5.ProcessPassword();
        string connectionName = "CnStr";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }
        }

        private void ClearTextBoxes()
        {
            foreach (Control cc in Panel1.Controls.OfType<TextBox>())
            {
                TextBox tb = cc as TextBox;
                tb.Text = string.Empty;

            }
            foreach (Control cc in Panel1.Controls.OfType<CheckBoxList>())
            {

                CheckBoxList cbList = cc as CheckBoxList;
                cbList.ClearSelection();

            }
        }

        protected void btnSearch_Click1(object sender, EventArgs e)
        {
            if (txtSearchEmp.Text == "")
            {
                labelSearchResult.Text = "Search input cannot be empty";
            }
            else
            {
                Hashtable ht = new Hashtable();
                switch (DropDownListSearch.SelectedItem.Value)
                {
                    case "Employee ID":
                        ht.Add("@EmpID", txtSearchEmp.Text);
                        break;
                    case "Family Name":
                        ht.Add("@FamilyName", txtSearchEmp.Text);
                        break;
                }
                DataTable dt = ReadWriteDB.SelectFromDB("spGetEmp", "Employees", ht, (string)Session["LoggedInUser"], connectionName);//DBSql.GetDataTableSP("spGetEmp", ht,connectionName);
                if (dt.Rows.Count > 0)
                {
                    labelSearchResult.Text = "Employee record exists!";
                    txtEmpID.Text = dt.Rows[0]["EmpID"].ToString();
                    txtEmpName.Text = dt.Rows[0]["Familyname"].ToString() + " " + dt.Rows[0]["FirstName"].ToString();
                }
                else
                {
                    labelSearchResult.Text = "Employee record does not exist!";
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string str;
            Hashtable ht = new Hashtable();
            ht.Add("@EmpID", txtEmpID.Text);

            DataTable dt = DBSql.GetDataTable("select UserName from Users where EmpID=@EmpID;", ht, connectionName);
            if (dt.Rows.Count > 0)
            {
                //user with EmployeeID already exists
                str = "A user with Employee ID" + txtEmpID.Text + " already exists. Add new user Failed!";
            }
            else
            {
                Tuple<string, string> newTuple = procPwd.ProcessPasswordToMD5(txtPassword.Text, txtUserName.Text, true);
                ht.Add("@UserName", txtUserName.Text);
                ht.Add("@RandomKey", newTuple.Item1);
                ht.Add("@Pwd_hash", newTuple.Item2);
                ht.Add("@BaseSet", CheckBoxListUserRights.Items.FindByValue("BaseSet").Selected);
                ht.Add("@EmployeeSet", CheckBoxListUserRights.Items.FindByValue("EmployeeSet").Selected);
                ht.Add("@SalarySet", CheckBoxListUserRights.Items.FindByValue("SalarySet").Selected);
                ht.Add("@SystemSet", CheckBoxListUserRights.Items.FindByValue("SystemSet").Selected);
                DBSql.InsUpdIntoDBSP("spInsertUser", ht, connectionName);
                str = "Success. User has been added!";

            }

            Response.Write("<script Language='JAVASCRIPT'>alert('" + str + "');document.location='" + ResolveClientUrl(Request.RawUrl) + "';</script>");
            // Response.Redirect(Request.RawUrl, true);
        }



    }
}