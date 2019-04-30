<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_TaskPage.aspx.cs" Inherits="TimeLink._TaskPage" EnableEventValidation="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Task page</title>
    <link rel="stylesheet" type="text/css" href="~/css/taskpage.css" />
    <link rel="shortcut icon" href="~/Img/appIcon3.png" type="image/x-icon" />
    <script type="text/javascript">
        function counter(elem) {
            var currentTextLength = textLength(elem);
            if (currentTextLength > 250) {
                elem.value = elem.value.substring(0, currentTextLength - newLines(elem));
            }
            document.getElementById("<%= Label1.ClientID %>").innerHTML = textLength(elem);
        }

        function newLines(element) {
            return element.value.split(/\r\n|\r|\n/).length;
        }

        function textLength(element) {
            return element.value.length + newLines(element) - 1;
        }
    </script>
</head>
<body>
    <form id="task_form" runat="server">
        <div>
            <asp:Label ID="lblGoalName" runat="server" Font-Bold="true"></asp:Label>
        </div>
        <br />
        <table border="0">
            <caption>New task data:</caption>
            <tr>
                <td class="name">Task name:</td>
                <td>
                    <asp:TextBox ID="tbxTaskName" runat="server" Width="460px" Text="New Task" MaxLength="50"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="name">Description:</td>
                <td>
                    <asp:TextBox ID="tbxDescription" runat="server" TextMode="MultiLine"
                        Rows="10" Width="460px" MaxLength="250"
                        ToolTip="Max length is 250 characters"
                        onkeyup="counter(this);" onkeydown="counter(this);">                                         
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td />
                <td style="padding-left: 335px;">Max:250. Count:<asp:Label ID="Label1" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td class="name" style="">Ready Status:</td>
                <td>
                    <asp:DropDownList ID="dlstDone" runat="server">
                        <asp:ListItem Value="true">Yes</asp:ListItem>
                        <asp:ListItem Selected="True" Value="false">No</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
        </table>
        <br />
        <div>
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
        </div>
    </form>
</body>
</html>
