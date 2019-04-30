<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_RegistrationPage.aspx.cs" Inherits="TimeLink._RegistrationPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration page</title>
    <link rel="stylesheet" type="text/css" href="~/Css/default.css" />
    <link rel="shortcut icon" href="~/Img/appIcon3.png" type="image/x-icon" />
</head>
<body>
    <form id="registration_form" runat="server" style="min-height: 600px; min-width: 800px;">
        <p>
            <asp:Label ID="lblConfirmation" runat="server" Text="" Visible="true"></asp:Label>
        </p>
        <table border="0">
            <caption style="text-align: left; font-weight: bold; background-color: limegreen;">Registration form:</caption>
            <tr>
                <td class="name">Email:</td>
                <td>
                    <asp:TextBox ID="tbxEmail" runat="server" TextMode="Email" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                        ControlToValidate="tbxEmail"
                        ErrorMessage="*"
                        ToolTip="Please enter valid email address."
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                        ControlToValidate="tbxEmail"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        ErrorMessage="*"
                        ToolTip="Invalid email address."
                        ForeColor="Red">
                    </asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td class="name">Password:</td>
                <td>
                    <asp:TextBox ID="tbxPassword" runat="server" TextMode="Password" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                        ControlToValidate="tbxPassword"
                        ErrorMessage="*"
                        ToolTip="Please enter valid password."
                        ForeColor="Red">
                    </asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td class="name">Confirm Password:</td>
                <td>
                    <asp:TextBox ID="tbxConfirm" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                        ErrorMessage="*"
                        ControlToValidate="tbxConfirm"
                        ToolTip="Compare Password is a required field"
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server"
                        ControlToValidate="tbxConfirm"
                        ControlToCompare="tbxPassword"
                        ErrorMessage="*"
                        ToolTip="Password must be the same."
                        ForeColor="Red">
                    </asp:CompareValidator></td>
            </tr>
            <tr style="height: 10px" />
            <tr>
                <td class="name" style="">First name:</td>
                <td>
                    <asp:TextBox ID="tbxFirstName" TextMode="SingleLine" runat="server" MaxLength="50"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="name" style="">Second name:</td>
                <td>
                    <asp:TextBox ID="tbxSecondName" TextMode="SingleLine" runat="server" MaxLength="50"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="name" style="">Gender:</td>
                <td>
                    <asp:DropDownList ID="dlstGender" runat="server">
                        <asp:ListItem Selected="True">Male</asp:ListItem>
                        <asp:ListItem>Female</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
        </table>
        <p>
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnButton_Click" CausesValidation="false" />
            <asp:Button ID="btnConfirm" runat="server" Text="Registration" OnClick="btnButton_Click" />
        </p>
    </form>
</body>
</html>
