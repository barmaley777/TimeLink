<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TimeLink.Default" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login page</title>
    <link rel="stylesheet" href="~/css/default.css" type="text/css" />
    <link rel="shortcut icon" href="~/Img/appIcon3.png" type="image/x-icon" />
</head>
<body>
    <form id="login_form" runat="server">
        <div class="login" style="width: 800px;">
            <p style="background-color: orangered; padding-left: 0px; padding-top: 0px;">
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </p>
            <ul>
                <li>
                    <asp:Label ID="lblEmail" runat="server" Text=" Email: " AssociatedControlID="tbxEmail">Email: </asp:Label>
                    <asp:TextBox ID="tbxEmail" runat="server" TextMode="Email" placeholder="your@email.com"></asp:TextBox>
                </li>
                <li>
                    <asp:Label ID="lblPassword" runat="server" Text=" Password: " AssociatedControlID="tbxPassword">Password: </asp:Label>
                    <asp:TextBox ID="tbxPassword" runat="server" TextMode="Password" placeholder="password"></asp:TextBox>
                </li>
                <li>
                    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                </li>
                <li>
                    <asp:Button ID="btnRegistration" runat="server" Text="Registration" PostBackUrl="_RegistrationPage.aspx" />
                </li>
            </ul>
        </div>
    </form>
</body>
</html>
