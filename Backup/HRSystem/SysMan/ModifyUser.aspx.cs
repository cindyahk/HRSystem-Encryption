using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using DBSqlLib;

namespace HRSystem.SysMan
{
    public partial class ModifyUser : System.Web.UI.Page
    {
        string connectionName = "CnStr";
        protected void Page_Load(object sender, EventArgs e)
        {

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
                    case "Username": ht.Add("@UserName", txtSearchEmp.Text);
                        break;
                    case "Employee ID": ht.Add("@EmpID", txtSearchEmp.Text);
                        break;
                }
                DataTable dt = DBSql.GetDataTableSP("spGetUser", ht,connectionName);
                if (dt.Rows.Count > 0)
                {
                    labelSearchResult.Text = "User exists!";
                    labelSearchResult.ForeColor = System.Drawing.Color.Green;

                    txtEmpID.Text = dt.Rows[0]["EmpID"].ToString();
                    txtEmpName.Text = dt.Rows[0]["Familyname"].ToString() + " " + dt.Rows[0]["FirstName"].ToString();
                    txtUserName.Text = dt.Rows[0]["UserName"].ToString();
                    foreach (DataColumn col in dt.Columns)
                    {
                        string colName = col.ColumnName;
                        if (Regex.IsMatch(colName,"(.*Set)"))
                        {
                            CheckBoxListUserRights.Items.FindByValue(colName).Selected = (bool)dt.Rows[0][colName];
                        }
                    }
                }
                else
                {
                    labelSearchResult.Text = "User does not exist!";
                    labelSearchResult.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            string str;
            ht.Add("@EmpID", txtEmpID.Text);
            ht.Add("@UserName", txtUserName.Text);
            ht.Add("@BaseSet", CheckBoxListUserRights.Items.FindByValue("BaseSet").Selected);
            ht.Add("@EmployeeSet", CheckBoxListUserRights.Items.FindByValue("EmployeeSet").Selected);
            ht.Add("@SalarySet", CheckBoxListUserRights.Items.FindByValue("SalarySet").Selected);
            ht.Add("@SystemSet", CheckBoxListUserRights.Items.FindByValue("SystemSet").Selected);
            bool success = DBSql.InsUpdIntoDBSP("spUpdateUser", ht,connectionName);
            if (success) str = "Success. User has been modified!";
            else str = "Fail! User has not been modified";

            Response.Write("<script Language='JAVASCRIPT'>alert('" + str + "');document.location='" + ResolveClientUrl(Request.RawUrl) + "';</script>");
        }

        protected void btnResetPwd_Click(object sender, EventArgs e)
        {
            Session["UserNameToReset"] = txtUserName.Text;
            Session["EmpNameToReset"] = txtEmpName.Text;
            Response.Redirect("~/SysMan/ResetPassword.aspx");
        }
    }
}