<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_GoalsPage.aspx.cs" Inherits="TimeLink._GoalsPage" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Goals page</title>
    <link rel="shortcut icon" href="~/Img/appIcon3.png" type="image/x-icon" />
    <link rel="stylesheet" type="text/css" href="~/css/default.css" />
</head>
<body>
    <form id="goals_form" runat="server">
        <div class="logout">
            <ul>
                <li>
                    <asp:LoginStatus ID="lgnStatus" runat="server" />
                </li>
                <li style="padding-right: 10px;">
                    <asp:LoginName ID="lgnUserEmail" runat="server"></asp:LoginName>
                </li>
            </ul>
        </div>
        <div class="filters">
            <table border="0">
                <caption style="text-align: left; font-weight: bold;">Filters:</caption>
                <tr>
                    <td>Ready:</td>
                    <td colspan="2">
                        <asp:DropDownList ID="dlstFilterDone" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dlstFilterDone_SelectedIndexChanged">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem Value="True">Yes</asp:ListItem>
                            <asp:ListItem Value="False">No</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Name:</td>
                    <td colspan="2">
                        <asp:TextBox ID="tbxFilterGoalName" runat="server" AutoPostBack="true" MaxLength="20" OnTextChanged="tbxFilterGoalName_TextChanged"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Tasks count:</td>
                    <td>from
                        <asp:TextBox ID="tbxCountFrom" runat="server" MaxLength="3" Width="40" AutoPostBack="true"
                            OnTextChanged="tbxCountFrom_TextChanged" ToolTip="Only Numbers allowed"></asp:TextBox>
                    </td>
                    <td>to
                <asp:TextBox ID="tbxCountTo" runat="server" MaxLength="3" Width="40" AutoPostBack="true"
                    OnTextChanged="tbxCountTo_TextChanged" ToolTip="Only Numbers allowed"></asp:TextBox>                                    
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnClean" runat="server" Text="Clean" OnClick="btnClean_Click" />
        </div>
        <br />
        <div class="table">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" AllowPaging="true" AllowCustomPaging="true" AllowSorting="true" EmptyDataText="No Data." ShowHeaderWhenEmpty="true" Width="800px"
                HeaderStyle-BackColor="#66ccff"
                OnPageIndexChanging="GridView1_PageIndexChanging"
                OnSorting="GridView1_Sorting"
                OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="40px"
                        HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:Label ID="lblAllGoals" runat="server">
                                <asp:CheckBox ID="chkAllGoals" runat="server" ToolTip="Select All" AutoPostBack="true" OnCheckedChanged="chkAllGoals_CheckedChanged" />
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkGoal" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="Done" SortExpression="Done"
                        HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkDone" runat="server" AutoPostBack="true" Checked='<%# Eval("Done").ToString()=="True" ? true : false %>'
                                Text='<%# Eval("Done").ToString()=="True" ? "yes" : "no" %>'
                                OnCheckedChanged="chkDone_CheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="GoalName" HeaderText="GOAL NAME" SortExpression="GoalName">
                        <ItemStyle HorizontalAlign="Left" Width="360px"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="TASKS COUNT" SortExpression="TasksCount">
                        <ItemTemplate>
                            <asp:Label ID="lblCount" runat="server" Text='<%# ((HashSet<TimeLink.Models.T_TASK>)Eval("T_TASK")).Count() %>' Width="40px"></asp:Label>                      
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CreateDate" HeaderText="CREATE DATE" SortExpression="CreateDate" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                        <ItemStyle HorizontalAlign="Left" Width="160px" ></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="GoalID" Visible="false" ItemStyle-Width="0px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="HiddenGoalID" runat="server"
                                Text='<%# Eval("GoalID") %>'></asp:Label>
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
                <li >New goal:<asp:Button ID="btnCreate" runat="server" Text="Create" OnClick="btnCreate_Click" />
                </li>
                <li >Delete goal:<asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
                </li>
                <li ">Export to csv:<asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" />
                </li>
            </ul>
        </div>
    </form>
</body>
</html>
