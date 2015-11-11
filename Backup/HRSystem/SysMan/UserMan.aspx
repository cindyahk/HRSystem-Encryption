<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="UserMan.aspx.cs" Inherits="HRSystem.SysMan.UserMan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel ID="Panel1" runat="server">
        <table bgcolor="#6699FF">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Users"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TreeView ID="TreeViewUsers" runat="server" BorderStyle="Solid" BorderColor="Black"
                        Height="150px" Width="100px" BorderWidth="1px" OnSelectedNodeChanged="TreeViewUsers_SelectedNodeChanged"
                        BackColor="White">
                    </asp:TreeView>
                </td>
                <td>
                    <table bgcolor="#6699FF" border="1px solid black">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Employee ID"></asp:Label>
                                <asp:TextBox ID="txtUserEmpID" runat="server" ReadOnly="True" BackColor="#999999"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="User Rights"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBoxList ID="CheckBoxListUserRights" runat="server" DataTextField="column_name">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
</asp:Content>
