<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="DeptMan.aspx.cs" Inherits="HRSystem.InfoMan.DeptMan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="panel">
        <asp:Panel ID="PanelContent" runat="server">
            <asp:Button ID="btnAdd" runat="server" Text="Add Department" BackColor="Gainsboro"
                Font-Names="Arial" ForeColor="#000000" Style="margin-bottom: 0px" />
            <p>
                <asp:GridView ID="GridViewDept" runat="server" Width="75%" OnRowDeleting="GridViewDept_RowDeleting">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnImgDelete" runat="server" ImageUrl="~/images/delete.gif"
                                    ImageAlign="Middle" CommandName="Delete" OnClientClick="return confirm('Are you certain you want to delete this questionnaire?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle BackColor="Gainsboro" Font-Names="Segoe UI" Font-Size="13px" Height="35px"
                        HorizontalAlign="Justify" />
                    <RowStyle Font-Names="Segoe UI" Font-Size="12px" ForeColor="#333333" />
                </asp:GridView>
            </p>
        </asp:Panel>
    </div>

    <!--Popup Panel for adding new department-->
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:ModalPopupExtender runat="server" PopupControlID="modalPanelAddDept" TargetControlID="btnAdd"
        BackgroundCssClass="modalBackground" CancelControlID="btnCancelPanel">
        <Animations>
    <OnShown><FadeIn Duration="0.25" Fps="10"/></OnShown>
    <OnHiding><FadeOut Duration="0.25" Fps="10"/></OnHiding>
        </Animations>
    </asp:ModalPopupExtender>
    <asp:Panel runat="server" ID="modalPanelAddDept" CssClass="modalPanel" Width="548px">
        <div class="panelHeader">
            Add New Department
        </div>
        <div class="panelContainer">
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Department"></asp:Label>
                    <asp:TextBox ID="txtDeptName" runat="server" Height="23px" Style="margin-left: 66px"
                        Width="162px" ValidationGroup="validateDept"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Department Name is required"
                        ControlToValidate="txtDeptName" Display="Dynamic" ForeColor="Red" ValidationGroup="validateDept">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Department name can only accept letters"
                        ControlToValidate="txtDeptName" Display="Dynamic" ForeColor="Red" Text="*" ValidationExpression="[a-zA-Z]+"
                        ValidationGroup="validateDept"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Telephone Number"></asp:Label>
                    <asp:TextBox ID="txtDeptTel" runat="server" Height="23px" Style="margin-left: 18px"
                        Width="162px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Department Telephone is required"
                        ControlToValidate="txtDeptTel" Display="Dynamic" ForeColor="Red" ValidationGroup="validateDept">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Format should be xxxx-xxxx"
                        ValidationExpression="\d{4}-\d{4}" ControlToValidate="txtDeptTel" Display="Dynamic"
                        ForeColor="Red" Text="*" ValidationGroup="validateDept"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
               <td><asp:Label ID="Label3" runat="server" Text="Description"></asp:Label></td> 
            </tr>
            <tr>
            <td>
                <asp:TextBox ID="txtDeptDescrip" runat="server" Height="42px" Style="margin-left: 3px"
                    TextMode="MultiLine" Width="256px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Description is required"
                    ControlToValidate="txtDeptDescrip" Display="Dynamic" ForeColor="Red" ValidationGroup="validateDept">*</asp:RequiredFieldValidator>
            </td>
            </tr>
            <tr>
                <td><asp:ValidationSummary ID="ValidationSummary" runat="server" ForeColor="Red" HeaderText="You must enter a value in the following fields:"
                    DisplayMode="BulletList" ShowSummary="true" ValidationGroup="validateDept" /></td>
            </tr>
            <tr>
                <td><asp:Button ID="btnAddPanel" runat="server" Text="Add" OnClick="btnAddPanel_Click"
                    ValidationGroup="validateDept" />
                <asp:Button ID="btnCancelPanel" runat="server" Text="Cancel" />
                </td>
            </tr>
            <br />
            <br />
            </table>
        </div>
    </asp:Panel>
</asp:Content>
