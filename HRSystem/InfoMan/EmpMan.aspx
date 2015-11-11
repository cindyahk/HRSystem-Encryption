<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="EmpMan.aspx.cs" Inherits="HRSystem.InfoMan.EmpMan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style2
        {
            width: 154px;
        }
        .style3
        {
            width: 128px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel">
        <asp:Panel ID="PanelContent" runat="server">
            <asp:Panel ID="PanelHeader" runat="server">
                <table cellspacing="10" cellpadding="1">
                    <tr>
                        <td colspan="1">
                            <asp:Button ID="btnAddEmp" runat="server" Text="Add New Employee" />
                        </td>
                        <td colspan="20">
                            <asp:Label ID="Label1" runat="server" Text="Search"></asp:Label>
                            <asp:DropDownList ID="DropDownListSearch" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="DropDownListSearch_SelectedIndexChanged"  >
                                <asp:ListItem>Family Name</asp:ListItem>
                                <asp:ListItem>Employee ID</asp:ListItem>
                                <asp:ListItem>Department</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="DropDownListSearchDept" runat="server" Visible="False">
                            </asp:DropDownList>
                            <asp:TextBox ID="txtSearchEmp" runat="server"></asp:TextBox>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:GridView ID="GridViewEmp" runat="server" Width="75%" OnRowDeleting="GridViewEmp_RowDeleting"
                AutoGenerateColumns="False">
                <Columns>
                <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnImgDelete" runat="server" ImageUrl="~/images/delete.gif"
                                ImageAlign="Middle" CommandName="Delete" OnClientClick="return confirm('Are you certain you want to delete this questionnaire?');" ToolTip="Delete Employee Record" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ReadOnly="true" DataField="EmpID" HeaderText="Employee ID" />
                    <asp:BoundField ReadOnly="true" DataField="FamilyName" HeaderText="Family Name" />
                    <asp:BoundField ReadOnly="true" DataField="FirstName" HeaderText="First Name" />
                    <asp:BoundField ReadOnly="true" DataField="Gender" HeaderText="Gender" />
                    <asp:BoundField ReadOnly="true" DataField="Nationality" HeaderText="Nationality" />
                    <asp:BoundField ReadOnly="true" DataField="DateOfBirth" HeaderText="Date Of Birth" />
                    <asp:BoundField ReadOnly="true" DataField="MaritalStatus" HeaderText="Marital Status" />
                    <asp:BoundField ReadOnly="true" DataField="Address" HeaderText="Address" />
                    <asp:BoundField ReadOnly="true" DataField="HireDate" HeaderText="Hire Date" />              
                    <asp:BoundField ReadOnly="true" DataField="Salary" HeaderText="Salary" />
                    <asp:BoundField ReadOnly="true" DataField="SocialSecurityNum" HeaderText="Social Security Number" />
                    <asp:BoundField ReadOnly="true" DataField="Department" HeaderText="Department" />
                    <asp:BoundField ReadOnly="true" DataField="Position" HeaderText="Position" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnImageModify" runat="server" ImageUrl="~/images/edit.jpg" ToolTip="Modify Employee Record" OnClick="BtnImageModify_Click"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="Gainsboro" Font-Names="Segoe UI" Font-Size="13px" Height="35px"
                    HorizontalAlign="Justify" />
                <RowStyle Font-Names="Segoe UI" Font-Size="12px" ForeColor="#333333" />
            </asp:GridView>
        </asp:Panel>
    </div>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnAddEmp"
        PopupControlID="modalPanelAddEmp" BackgroundCssClass="modalBackground" CancelControlID="btnCancelPanel"
        RepositionMode="RepositionOnWindowResizeAndScroll">
    </asp:ModalPopupExtender>
    
    <asp:Panel ID="modalPanelAddEmp" runat="server" CssClass="modalPanel" Width="424px"
        ScrollBars="Auto">
        <div class="panelHeader">Add/Modify New Employee</div>
        <div class="panelContainer">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="ValidateEmp"
                HeaderText="You must enter the following fields:" ForeColor="Red" />
            <asp:Panel ID="PanelPersonalInfo" runat="server" GroupingText="Personal Information"
                Width="387px">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Employee ID"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmpID" runat="server" ReadOnly="True" BackColor="Silver"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="FamilyName"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFamilyName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Family Name is required"
                                ValidationGroup="ValidateEmp" ControlToValidate="txtFamilyName" Display="Dynamic"
                                ForeColor="Red">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Family Name can only contain letters"
                                ValidationGroup="ValidateEmp" ControlToValidate="txtFamilyName" Display="Dynamic"
                                ForeColor="Red" ValidationExpression="[a-zA-Z]+">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="FirstName"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="First Name is required"
                                ValidationGroup="ValidateEmp" ControlToValidate="txtFirstName" Display="Dynamic"
                                ForeColor="Red">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="First Name can only contain letters"
                                ValidationGroup="ValidateEmp" ControlToValidate="txtFirstName" Display="Dynamic"
                                ForeColor="Red" ValidationExpression="[a-zA-Z]+">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="MaritalStatus"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListMaritalStatus" runat="server" Width="128px" Height="26px">
                                <asp:ListItem>Single</asp:ListItem>
                                <asp:ListItem>Married</asp:ListItem>
                                <asp:ListItem>Divorced</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Marital Status is required"
                                ValidationGroup="ValidateEmp" ControlToValidate="DropDownListMaritalStatus" ForeColor="Red"
                                Display="Dynamic" InitialValue="0" Text="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Nationality"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListNationality" runat="server" Width="128px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Nationality is required"
                                ValidationGroup="ValidateEmp" ControlToValidate="DropDownListNationality" ForeColor="Red"
                                Display="Dynamic" InitialValue="0">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text="DateOfBirth"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDOB" runat="server"></asp:TextBox>
                            <asp:CalendarExtender ID="txtDOB_CalendarExtender" runat="server" Enabled="True"
                                TargetControlID="txtDOB" Format="yyyy/MM/dd">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Date of Birth is required"
                                ValidationGroup="ValidateEmp" ControlToValidate="txtDOB" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Address"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Address is required"
                                ValidationGroup="ValidateEmp" ControlToValidate="txtAddress" Display="Dynamic"
                                ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="Gender"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListGender" runat="server" Width="128px">
                                <asp:ListItem>F</asp:ListItem>
                                <asp:ListItem>M</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Gender is required"
                                ValidationGroup="ValidateEmp" ControlToValidate="DropDownListGender" Display="Dynamic"
                                ForeColor="Red" InitialValue="0">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="SocialSecurityNumber"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSSID" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Social Security Number is required"
                                ValidationGroup="ValidateEmp" ControlToValidate="txtSSID" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="PanelRelatedInfo" runat="server" GroupingText="Related Information"
                Width="387px">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="Department"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListDept" runat="server" Width="91%">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Department is required"
                                ValidationGroup="ValidateEmp" ControlToValidate="DropDownListDept" Display="Dynamic"
                                ForeColor="Red" InitialValue="0">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label12" runat="server" Text="Position"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListPositions" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Position is required"
                                ValidationGroup="ValidateEmp" ControlToValidate="DropDownListPositions" Display="Dynamic"
                                ForeColor="Red" InitialValue="0">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label13" runat="server" Text="Salary"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSalary" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Salary is required"
                                ValidationGroup="ValidateEmp" ControlToValidate="txtSalary" Display="Dynamic"
                                ForeColor="Red">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Salary can only contain numbers"
                                ValidationGroup="ValidateEmp" ControlToValidate="txtSalary" Display="Dynamic"
                                ForeColor="Red" ValidationExpression="[\d]+">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label14" runat="server" Text="Hire Date"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtHireDate" runat="server"></asp:TextBox>
                            <asp:CalendarExtender ID="txtHireDate_CalendarExtender" runat="server" Enabled="True"
                                TargetControlID="txtHireDate" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Hire Date is required"
                                ValidationGroup="ValidateEmp" ControlToValidate="txtHireDate" Display="Dynamic"
                                ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnAddPanel" runat="server" ValidationGroup="ValidateEmp" OnClick="btnAddPanel_Click" Text="Add"/>
                        <asp:Button ID="btnModifyPanel" runat="server" ValidationGroup="ValidateEmp" Text="Modify" Enabled="false" OnClick="btnModifyPanel_Click"/>
                    </td>
                    <td>
                        <asp:Button ID="btnCancelPanel" runat="server" Text="Cancel" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>
