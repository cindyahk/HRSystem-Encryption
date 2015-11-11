using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text.RegularExpressions;
using DBSqlLib;

namespace HRSystem.SysMan
{
    public partial class UserMan : System.Web.UI.Page
    {
        string connectionName = "CnStr";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DataTable dt = DBSql.GetDataTable("select UserName from Users", null,connectionName);
                foreach (DataRow row in dt.Rows)
                {
                    TreeNode node = new TreeNode();
                    node.Text = row["UserName"].ToString();
                    TreeViewUsers.Nodes.Add(node);
                }

                Hashtable ht = new Hashtable();
                ht.Add("@TableName","Users");
                DataTable dtChk = DBSql.GetDataTable("select top 4 column_name  from Information_Schema.Columns WHERE table_name =@TableName order by ORDINAL_POSITION desc", ht, connectionName);
                for (int i = 0; i < 4;i++ )
                {
                    CheckBoxListUserRights.DataSource = dtChk;
                    CheckBoxListUserRights.DataBind();
                    CheckBoxListUserRights.Items[i].Text = dtChk.Rows[i]["column_name"].ToString();
                }
            }
        }

        protected void TreeViewUsers_SelectedNodeChanged(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@UserName",TreeViewUsers.SelectedNode.Text);
            DataTable dt = DBSql.GetDataTable("Select EmpID, BaseSet, EmployeeSet, SalarySet, SystemSet from Users where UserName=@UserName",ht,connectionName);
            txtUserEmpID.Text = dt.Rows[0]["EmpID"].ToString();
            foreach(DataColumn col in dt.Columns)
            {
                string colName = col.ColumnName;
                if (Regex.IsMatch(colName, "(.*Set)"))
                {
                    CheckBoxListUserRights.Items.FindByValue(colName).Selected = (bool)dt.Rows[0][colName];
                }
            }

        }



       
    }
}