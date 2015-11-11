<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="ResetPassword.aspx.cs" Inherits="HRSystem.SysMan.ResetPassword" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Panel ID="Panel1" runat="server">
        <table>
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
                    <asp:TextBox ID="txtUserName" runat="server" AutoCompleteType="Disabled" ReadOnly="True"  BackColor="Silver"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Username is required"
                        ForeColor="Red" ControlToValidate="txtUserName" ValidationGroup="ValidateResetPwd">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Password"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:PasswordStrength ID="txtPassword_PasswordStrength" runat="server" 
                        Enabled="True" TargetControlID="txtPassword" TextStrengthDescriptionStyles="textIndicator_poor;textIndicator_weak;textIndicator_good;textIndicator_strong;textIndicator_excellent"
                        PrefixText="Strength:" StrengthIndicatorType="Text" TextStrengthDescriptions="Poor;Weak;Good;Strong;Excellent"
                        MinimumUpperCaseCharacters="1" MinimumSymbolCharacters="1" MinimumNumericCharacters="1"
                        MinimumLowerCaseCharacters="1" RequiresUpperAndLowerCaseCharacters="True" DisplayPosition="RightSide"
                        HelpStatusLabelID="Label6" CalculationWeightings="25;25;15;35">
                    </asp:PasswordStrength>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Password required"
                        ControlToValidate="txtPassword" ForeColor="Red" Text="*" ValidationGroup="ValidateResetPwd"></asp:RequiredFieldValidator>
                   
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Your password does not fulfill strength requirements"
                        ControlToValidate="txtPassword" ValidationGroup="ValidateResetPwd" ValidationExpression="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{10,}$"
                        ForeColor="Red"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td style="height: 20px" colspan="2">
                    <asp:Label ID="Label6" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Please Enter password again: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPassword1" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please re-enter password"
                        ControlToValidate="txtPassword1" ForeColor="Red" ValidationGroup="ValidateResetPwd">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="The two passwords don't match"
                        ControlToValidate="txtPassword1" ControlToCompare="txtPassword" ForeColor="Red"
                        ValidationGroup="ValidateResetPwd"></asp:CompareValidator>
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="ValidateResetPwd"
            HeaderText="The following fields need to be filled:" ForeColor="Red" />
        <asp:Button ID="btnResetPwd" runat="server" Text="Reset Password" 
            ValidationGroup="ValidateResetPwd" onclick="btnResetPwd_Click"
            />
    </asp:Panel>
</asp:Content>
