<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HRSystem.Login.Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="margin-top: 40px; margin-left: 15%">
        <asp:Label ID="Label3" runat="server" Text="Welcome to Human Resources System Management!"
            Font-Bold="True" Font-Names="Garamond" Font-Size="40px" ForeColor="White"></asp:Label>
    </div>
    <div style="clear: both; height: 30px;">
    </div>
    <div>
        <asp:Panel ID="Panel1" runat="server" BorderStyle="Solid" BorderColor="#333333" Width="35%"
            Style="margin-left: 30%; padding: 10px" BackImageUrl="~/images/login.jpg">
            <div style="padding: 30px">
                <asp:Login ID="LoginControl" runat="server" ForeColor="#333333" Font-Size="16px"
                    TitleText="Login to your Human Resources System account!" DestinationPageUrl="~/pMain.aspx"
                    DisplayRememberMe="False" MembershipProvider="MyMembershipProvider" OnAuthenticate="LoginControl_Authenticate"
                    LoginButtonStyle-Height="80px" 
                    LoginButtonStyle-Width="40px" LoginButtonStyle-ForeColor="White" 
                    LoginButtonStyle-BackColor="#0092C8"
                    PasswordRequiredErrorMessage="Password is required" 
                    UserNameRequiredErrorMessage="Username is required" 
                    onloginerror="LoginControl_LoginError">
                 
                    <LabelStyle Font-Size="16px" ForeColor="#333333" />
                    <LoginButtonStyle BackColor="#0092C8" ForeColor="White" Height="40px" 
                        Width="80px" />
                    <TitleTextStyle Font-Size="24px" ForeColor="#333333" />
                    <ValidatorTextStyle Font-Size="14px" ForeColor="Red"/>
                    
                    
                </asp:Login>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="LoginControl" ForeColor="Red" HeaderText="The following fields are required:"/>
            </div>
            <div style="clear: both; height: 20px;">
            </div>
        </asp:Panel>
        <asp:RoundedCornersExtender ID="Panel1_RoundedCornersExtender" runat="server" Enabled="True"
            Radius="10" TargetControlID="Panel1">
        </asp:RoundedCornersExtender>
    </div>
    </form>
</body>
</html>
