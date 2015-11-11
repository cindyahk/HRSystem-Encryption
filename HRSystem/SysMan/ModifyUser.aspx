<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ModifyUser.aspx.cs" Inherits="HRSystem.SysMan.ModifyUser" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Panel ID="Panel1" runat="server">
        <table>
            <tr>
                <td colspan="20">
                    <asp:Label ID="Label3" runat="server" Text="Search"></asp:Label>
                    <asp:DropDownList ID="DropDownListSearch" runat="server">
                        <asp:ListItem>Username</asp:ListItem>
                        <asp:ListItem>Employee ID</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtSearchEmp" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click1"/>
                </td>
                <td>
                    <asp:Label ID="labelSearchResult" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
       
        <table>
            <tr>
                <td >
                    <asp:Label ID="Label2" runat="server" Text="EmployeeID"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEmpID" runat="server" BackColor="Silver" ReadOnly="True"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Employee ID is required. Please first check if Employee ID exists by using Search." ControlToValidate="txtEmpID" ForeColor="Red" Text="*" ValidationGroup="ValidateUserModify"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text="Employee Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmpName" runat="server" BackColor="Silver" ReadOnly="True"></asp:TextBox>
                    </td>
            </tr>
            
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Username"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Username is required" ForeColor="Red" ControlToValidate="txtUserName" ValidationGroup="ValidateUserModify">*</asp:RequiredFieldValidator>
                </td>
            </tr>
          
            <tr>
                <asp:CheckBoxList ID="CheckBoxListUserRights" runat="server">
                    <asp:ListItem>BaseSet</asp:ListItem>
                    <asp:ListItem>EmployeeSet</asp:ListItem>
                    <asp:ListItem>SystemSet</asp:ListItem>
                    <asp:ListItem>SalarySet</asp:ListItem>
                </asp:CheckBoxList>
           
            </tr>
            </table>
            
             <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="ValidateUserModify" HeaderText="The following fields need to be filled:" ForeColor="Red"/>
             <asp:Button ID="btnModify" runat="server" Text="Modify" 
              ValidationGroup="ValidateUserModify" onclick="btnModify_Click" />    
             <asp:Button ID="btnResetPwd" runat="server" Text="Reset Password" 
            ValidationGroup="ValidateUserModify" onclick="btnResetPwd_Click" />
    </asp:Panel>
</asp:Content>
