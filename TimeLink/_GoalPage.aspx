<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_GoalPage.aspx.cs" Inherits="TimeLink._GoalPage" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Goal page</title>
    <link rel="shortcut icon" href="~/Img/appIcon3.png" type="image/x-icon" />
    <link rel="stylesheet" href="~/css/default.css" type="text/css" />
</head>
<body>
    <form id="goal_form" runat="server">
        <div class="logout">
            <ul>
                <li>
                    <asp:LoginStatus ID="lgnStatus" runat="server" LogoutAction="RedirectToLoginPage"  />
                </li>
                <li style="padding-right: 10px;">
                    <asp:LoginName ID="lgnUserEmail" runat="server"></asp:LoginName>
                </li>
            </ul>
        </div>
        <div class="goalName">
            <asp:Label ID="lblHiddenGoalID" runat="server" Visible="false" Text=""></asp:Label>
            <asp:Label ID="lblGoalName" runat="server" Text="Goal Name: "></asp:Label>
            <asp:TextBox ID="tbxGoalName" runat="server" AutoPostBack="true" MaxLength="50" Width="320px" OnTextChanged="tbxGoalName_TextChanged"></asp:TextBox>
        </div>
        <div class="filters">
            <table border="0">
                <caption style="text-align: left; font-weight: bold;">Filters:</caption>
                <tr>
                    <td>Ready:</td>
                    <td>
                        <asp:DropDownList ID="dlstFilterDone" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dlstFilterDone_SelectedIndexChanged">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem Value="True">Yes</asp:ListItem>
                            <asp:ListItem Value="False">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Name:</td>
                    <td>
                        <asp:TextBox ID="tbxFilterName" runat="server" AutoPostBack="true" OnTextChanged="tbxFilterName_TextChanged"></asp:TextBox></td>
                </tr>
            </table>
            <asp:Button ID="btnClean" runat="server" Text="Clean" OnClick="btnClean_Click" />
        </div>
        <br />
        <div class="table">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" AllowPaging="true" AllowCustomPaging="true" AllowSorting="true" EmptyDataText="No Data." ShowHeaderWhenEmpty="true" Width="800px"
                HeaderStyle-BackColor="#66ccff"
                OnRowDataBound="GridView1_RowDataBound"
                OnPageIndexChanging="GridView1_PageIndexChanging"
                OnSorting="GridView1_Sorting">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="40px"
                        HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:Label ID="lblAllTasks" runat="server">
                                <asp:CheckBox ID="chkAllTasks" runat="server" ToolTip="Select All" AutoPostBack="true" OnCheckedChanged="chkAllTasks_CheckedChanged" />
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkTask" runat="server" AutoPostBack="true" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="DONE" SortExpression="Done"
                        HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkDone" runat="server" AutoPostBack="true" Checked='<%# Eval("Done").ToString()=="True" ? true : false %>'
                                Text='<%# Eval("Done").ToString()=="True" ? "yes" : "no" %>'
                                OnCheckedChanged="chkDone_CheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TaskName" HeaderText="TASK NAME" SortExpression="TaskName">
                        <ItemStyle HorizontalAlign="Left" Width="140px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Description" HeaderText="DESCIPTION" SortExpression="Description">
                        <ItemStyle HorizontalAlign="Left" Width="380px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="UpdateDate" HeaderText="UPDATED" SortExpression="UpdateDate" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="TaskID" Visible="false" ItemStyle-Width="0px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="HiddenTaskID" runat="server"
                                Text='<%# Eval("TaskID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="paging">
                items per page:
            <asp:DropDownList ID="dlstPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dlstPageSize_SelectedIndexChanged">
                <asp:ListItem Selected="True">5</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>25</asp:ListItem>
            </asp:DropDownList>
            </div>
        </div>
        <br />
        <br />
        <div class="buttons">
            <b>Action buttons:</b>
            <ul style="text-align: left; padding: 0px; margin: 0px;">
                <li >Add task:<asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
                </li>
                <li >Delete task:<asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
                </li>
                <li >Goals page:<asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
                </li>
                <li ">Export to csv:<asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" />
                </li>
            </ul>
        </div>
    </form>
</body>
</html>
