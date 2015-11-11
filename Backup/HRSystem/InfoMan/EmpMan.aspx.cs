using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using DBSqlLib;
using EncryptionEngine;

namespace HRSystem.InfoMan
{
    public partial class EmpMan : System.Web.UI.Page
    {
        string connectionName = "CnStr";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DropDownListNationality.DataSource = GetCountryList();
                DropDownListNationality.DataBind();

                DropDownListDept.DataSource = GetDeptList();
                DropDownListDept.DataBind();

                DropDownListPositions.DataSource = GetPosList();
                DropDownListPositions.DataBind();

                DropDownListNationality.Items.Insert(0, new ListItem("--Select--", "0"));
                DropDownListPositions.Items.Insert(0, new ListItem("--Select--", "0"));
                DropDownListDept.Items.Insert(0, new ListItem("--Select--", "0"));
                DropDownListGender.Items.Insert(0, new ListItem("--Select--", "0"));
                DropDownListMaritalStatus.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }

        private List<String> GetCountryList()
        {
            List<String> countryList = new List<string>();
            foreach (CultureInfo info in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo inforeg = new RegionInfo(info.LCID);
                if (!countryList.Contains(inforeg.EnglishName))
                {
                    countryList.Add(inforeg.EnglishName);
                }
            }
            countryList.Sort();
            return countryList;
        }

        private List<String> GetDeptList()
        {
            List<String> deptList = new List<string>();
            DataTable dt = DBSql.GetDataTable("select Department from Departments", null,connectionName);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    deptList.Add(row["Department"].ToString());
                }
            }
            return deptList;
        }

        private List<String> GetPosList()
        {
            List<String> posList = new List<string>();
            DataTable dt = DBSql.GetDataTable("select Position from Positions", null,connectionName);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    posList.Add(row["Position"].ToString());
                }
            }
            return posList;
        }

        protected void GridViewEmp_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string delStr = "Delete from Employees where EmpID=@EmpID";
            Hashtable ht = new Hashtable();
            TableCell cell = GridViewEmp.Rows[e.RowIndex].Cells[1];
            ht.Add("@EmpID", cell.Text);
            bool delSuccess = DBSql.DeleteFromDB(delStr, ht,connectionName);
            string message="";
            if (delSuccess)
            {
                FillGridViewWithEmp();
                message = "Record deleted successfully!";
            }
            else
            {
                message = "Record could not be deleted successfully!";
            }
            Response.Write("<script Language='JAVASCRIPT'>alert('" + message + "');document.location='" + ResolveClientUrl(Request.RawUrl) + "';</script>");
        }

        private void ClearGridViewEmp()
        {
            GridViewEmp.DataSource = null;
            GridViewEmp.DataBind();
        }

        private void UpdateEmpRecord()
        {
            Hashtable ht = new Hashtable();
            ht.Add("@EmpID", txtEmpID.Text);
            DataTable dt = ReadWriteDB.SelectFromDB("spGetEmp", "Employees", ht, (string)Session["LoggedInUser"], connectionName);//DBSql.GetDataTable("spGetEmp",ht,connectionName);//dbsql.GetDataTableSP("spGetEmp", ht);
            if (dt.Rows.Count > 0)
            {
                GridViewEmp.DataSource = dt;
                GridViewEmp.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ClearGridViewEmp();
            FillGridViewWithEmp();
        }

        protected void FillGridViewWithEmp()
        {
            Hashtable ht = new Hashtable();
            switch (DropDownListSearch.SelectedItem.Value)
            {
                case "Employee ID": ht.Add("@EmpID", txtSearchEmp.Text); break;
                case "Family Name": ht.Add("@FamilyName", txtSearchEmp.Text); break;
                case "Department": ht.Add("@Department", DropDownListSearchDept.SelectedItem.Value);
                    break;
            }
            DataTable dt = ReadWriteDB.SelectFromDB("spGetEmp", "Employees", ht, (string)Session["LoggedInUser"], connectionName);//dbsql.GetDataTableSP("spGetEmp", ht);
            if (dt.Rows.Count > 0)
            {
                GridViewEmp.DataSource = dt;
                GridViewEmp.DataBind();
            }
        }

        protected void btnAddPanel_Click(object sender, EventArgs e)
        {
            //find the last used Employee ID and generate the current EmpID
            DataTable dtEmpID = DBSql.GetDataTable("select EmpID from Employees;", null,connectionName);
            int employeeCount = dtEmpID.Rows.Count;
            DataRow last = dtEmpID.Rows[employeeCount - 1];
            int lastEmpID = Int32.Parse(last["EmpID"].ToString());

            //set the new Employee ID
            int newEmpID = lastEmpID + 1;
            txtEmpID.Text = newEmpID.ToString();

            Hashtable ht = FillEmpHashtable();

            Tuple<bool,string> tupleResult = ReadWriteDB.InsertUpdateInDB("spInsertEmp", "Employees", (string)Session["LoggedInUser"], ht, connectionName);//dbsql.InsUpdIntoDBSP("spInsertEmp", ht);
            string message;
            if (tupleResult.Item1)
                message = tupleResult.Item2;
            else
                message = tupleResult.Item2;

            //UpdateEmpRecord(); //show emp record in gridview    
            Response.Write("<script Language='JAVASCRIPT'>alert('" + message + "');document.location='" + ResolveClientUrl(Request.RawUrl) + "';</script>");      
        }

        private Hashtable FillEmpHashtable()
        {
            Hashtable ht = new Hashtable();
            ht.Add("@EmpID", Int32.Parse(txtEmpID.Text));
            ht.Add("@FamilyName", txtFamilyName.Text);
            ht.Add("@FirstName", txtFirstName.Text);
            ht.Add("@Gender", DropDownListGender.SelectedItem.Text);
            ht.Add("@Nationality", DropDownListNationality.SelectedItem.Text);
            ht.Add("@Birth", DateTime.Parse(txtHireDate.Text));
            ht.Add("@MaritalStatus", DropDownListMaritalStatus.SelectedItem.Text);
            ht.Add("@Address", txtAddress.Text);
            ht.Add("@HireDate", DateTime.Parse(txtHireDate.Text));
            ht.Add("@Department", DropDownListDept.SelectedItem.Text);
            ht.Add("@Salary", Int32.Parse(txtSalary.Text));
            ht.Add("@SocialSecurityNum", txtSSID.Text);
            ht.Add("@Position", DropDownListPositions.SelectedItem.Text);
            return ht;
        }

        protected void BtnImageModify_Click(object sender, EventArgs e)
        {
            btnAddPanel.Enabled = false;
            btnModifyPanel.Enabled = true;
            Hashtable ht = new Hashtable();
            ht.Add("@EmpID", GridViewEmp.Rows[0].Cells[1].Text);
            DataTable dt = ReadWriteDB.SelectFromDB("spGetEmp", "Employees", ht, (string)Session["LoggedInUser"], connectionName);//DBSql.GetDataTableSP("spGetEmp", ht,connectionName);
            if (dt.Rows.Count > 0)
            {
                txtEmpID.Text = dt.Rows[0]["EmpID"].ToString();
                txtFamilyName.Text = dt.Rows[0]["FamilyName"].ToString();
                txtFirstName.Text = dt.Rows[0]["FirstName"].ToString();
                txtAddress.Text = dt.Rows[0]["Address"].ToString();
                txtSalary.Text = dt.Rows[0]["Salary"].ToString();
                txtSSID.Text = dt.Rows[0]["SocialSecurityNum"].ToString();
                txtDOB.Text = dt.Rows[0]["DateOfBirth"].ToString();
                txtHireDate.Text = dt.Rows[0]["HireDate"].ToString();

                DropDownListGender.Text = dt.Rows[0]["Gender"].ToString();
                DropDownListNationality.Text = dt.Rows[0]["Nationality"].ToString();
                DropDownListMaritalStatus.Text = dt.Rows[0]["MaritalStatus"].ToString();
                DropDownListPositions.Text = dt.Rows[0]["Position"].ToString();
                DropDownListDept.Text = dt.Rows[0]["Department"].ToString();
            }
            ModalPopupExtender1.Show();
        }

        protected void btnModifyPanel_Click(object sender, EventArgs e)
        {
            Hashtable ht = FillEmpHashtable();
            Tuple<bool,string> tupleResult = ReadWriteDB.InsertUpdateInDB("spUpdateEmp", "Employees", (string)Session["LoggedInUser"],ht,connectionName);//dbsql.InsUpdIntoDBSP("spUpdateEmp", ht);
            string message;
            if (tupleResult.Item1)
                message = tupleResult.Item2;
            else message = tupleResult.Item2;
            Response.Write("<script Language='JAVASCRIPT'>alert('" + message + "');document.location='" + ResolveClientUrl(Request.RawUrl) + "';</script>");
            UpdateEmpRecord(); //show emp record in gridview
        }

        protected void DropDownListSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewEmp.DataSource = null;
            GridViewEmp.DataBind();

            if (DropDownListSearch.SelectedItem.Value == "Department")
            {
                DropDownListSearchDept.Visible = true;
                DropDownListSearchDept.DataSource = GetDeptList();
                DropDownListSearchDept.DataBind();
                DropDownListSearchDept.Items.Insert(0, new ListItem("--Select Department--", "0"));

                txtSearchEmp.Enabled = false;
                txtSearchEmp.BackColor = System.Drawing.Color.LightGray;
            }
            else 
            {
                DropDownListSearchDept.DataSource = null;
                DropDownListSearchDept.DataBind();
                DropDownListSearchDept.Visible = false;
                txtSearchEmp.Enabled = true;
                txtSearchEmp.BackColor = System.Drawing.Color.White;
            }
        }

     

    }
}