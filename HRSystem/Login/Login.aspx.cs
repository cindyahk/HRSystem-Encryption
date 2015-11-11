using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using DBSqlLib;
namespace HRSystem.Login
{
   
    public partial class Login : System.Web.UI.Page
    {
        string connectionName = "CnStr";
        HrSystem.MD5.ProcessPassword procPwd = new HrSystem.MD5.ProcessPassword();
        bool userExists = false;
        bool passwordCorrect = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

     
        protected void LoginControl_Authenticate(object sender, AuthenticateEventArgs e)
        {
            //retrieve username and password hash value to compare with calculated password hash value
            Hashtable ht = new Hashtable();
            ht.Add("@UserName", LoginControl.UserName);
            DataTable dt = DBSql.GetDataTable("select Pwd_hash, EmpID, BaseSet,SystemSet, SalarySet,EmployeeSet from Users where UserName=@UserName", ht,connectionName);
          
            if (dt.Rows.Count > 0)//username exists
            {
                userExists = true;
 
                //get Employee Name information
                ht.Clear();
                ht.Add("@EmpID", dt.Rows[0]["EmpID"]);
                DataTable dt1 = DBSql.GetDataTable("select FamilyName,FirstName from Employees where EmpID=@EmpID", ht, connectionName);

                //calculate password hash value
                Tuple<string, string> newTuple = procPwd.ProcessPasswordToMD5(LoginControl.Password,LoginControl.UserName, false);

                //compare stored password hash value with calculated input password hash
                if (dt.Rows[0]["Pwd_hash"].ToString().Equals(newTuple.Item2))
                {
                    //logged in
                    e.Authenticated = true;
                    Session["LoggedInUser"]= LoginControl.UserName;
                    Session["LoggedInEmpName"] = dt1.Rows[0]["FamilyName"].ToString() + " "+ dt1.Rows[0]["FirstName"].ToString();
                    Session["SystemSet"] = (bool)dt.Rows[0]["SystemSet"];
                    passwordCorrect = true;      
                }
                else
                {
                    passwordCorrect = false;                   
                }

            }
            else
            {
                //username does not exist, try again
                userExists = false;                
            } 
        }

        protected void LoginControl_LoginError(object sender, EventArgs e)
        {
            if (!userExists)
            {
                LoginControl.FailureText = "User does not exist";
            }
            else if (!passwordCorrect)
            { 
                LoginControl.FailureText = "Password is wrong. Please try again!"; 
            }
        }

       
    }
}