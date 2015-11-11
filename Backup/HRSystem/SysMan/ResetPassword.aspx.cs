using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using DBSqlLib;
using EncryptionEngine;

namespace HRSystem.SysMan
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        string connectionName = "CnStr";
        HrSystem.MD5.ProcessPassword procPwd = new HrSystem.MD5.ProcessPassword();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //if User Management Admin is resetting password for a user
                string yourPreviousUrl = Request.UrlReferrer.ToString();
                if (yourPreviousUrl.Contains("ModifyUser.aspx"))
                {
                    txtUserName.Text = (string)Session["UserNameToReset"];
                    txtEmpName.Text = (string)Session["EmpNameToReset"];
                }
                else
                {
                    //logged in user is resetting his own password
                    txtUserName.Text = (string)Session["LoggedInUser"];
                    txtEmpName.Text = (string)Session["LoggedInEmpName"];
                }
            }
        }

        protected void btnResetPwd_Click(object sender, EventArgs e)
        {
            Tuple<string, string> newTuple = procPwd.ProcessPasswordToMD5(txtPassword.Text,txtUserName.Text,true);
            Hashtable ht = new Hashtable();
            ht.Add("@UserName",txtUserName.Text);
            ht.Add("@Pwd_hash",newTuple.Item2);
            ht.Add("@RandomKey",newTuple.Item1);
            string message;
            bool success = DBSql.InsUpdIntoDBSP("spResetPwd",ht,connectionName);
            if (success)
                message = "Password successfully modified";
            else message = "Failed! Password could not be modified!";
            Response.Write("<script Language='JAVASCRIPT'>alert('" + message + "');document.location='" + ResolveClientUrl("~/pMain.aspx") + "';</script>");
        }
    }
}