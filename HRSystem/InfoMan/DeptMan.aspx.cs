using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using DBSqlLib;

namespace HRSystem.InfoMan
{
    public partial class DeptMan : System.Web.UI.Page
    {
        string connectionName = "CnStr";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.RefreshGridView();
            }
        }

        protected void btnAddPanel_Click(object sender, EventArgs e)
        {
            Hashtable htAddDept = new Hashtable();
            htAddDept.Add("Department", txtDeptName.Text);
            htAddDept.Add("Description", txtDeptDescrip.Text);
            htAddDept.Add("DeptTel", txtDeptTel.Text);
            DBSql.InsertIntoDB("Departments", htAddDept, connectionName);
            this.RefreshGridView();
        }

        private void RefreshGridView()
        {
            DataTable dt = DBSql.GetDataTable("Select Department, Description, DeptTel as Telephone from Departments", null, connectionName);
            GridViewDept.DataSource = dt;
            GridViewDept.DataBind();
        }

        protected void GridViewDept_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Hashtable ht = new Hashtable();
            TableCell cell = GridViewDept.Rows[e.RowIndex].Cells[1];
            ht.Add("@Department", cell.Text);
            bool delSuccess = DBSql.DeleteFromDB("Delete from Departments where Department=@Department", ht, connectionName);
            if (delSuccess)
            {
                //donePanelLabel.Text = "Record deleted";
                // ModalPopupExtDone.Show();
                this.RefreshGridView();
            }
            else
            {

            }

        }


    }
}