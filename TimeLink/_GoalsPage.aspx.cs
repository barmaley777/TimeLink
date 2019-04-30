using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeLink.Models;
using TimeLink.Services;

namespace TimeLink
{
    public partial class _GoalsPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
            }
            else
            {
                FormsAuthentication.SetAuthCookie(Page.User.Identity.Name, true);
            }

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private IQueryable<T_GOAL> GetData(string sortExpression = "", SortDirection sortDirection = SortDirection.Ascending)
        {
            MyDataModel context = new MyDataModel();

            GridView1.PageSize = int.Parse(dlstPageSize.SelectedValue);
            string userName = Page.User.Identity.Name;
            IQueryable<T_GOAL> goals = null;
            goals = T_GOALservice.GetGoalsByEmail(context, userName);            
            string filterName = tbxFilterGoalName.Text;
            string filterDone = dlstFilterDone.SelectedValue;
            int countFrom = 0;
            int.TryParse(tbxCountFrom.Text,out countFrom);
            int countTo = 0;
            if (goals.Any()) {
                countTo = T_GOALservice.GetMaxTasksCount(goals); 
            }
            
            if (!string.IsNullOrEmpty(tbxCountTo.Text) ) {
                int.TryParse(tbxCountTo.Text, out countTo);
            }
                       
            //filters
            if (string.IsNullOrEmpty(filterDone))
            {
                goals = T_GOALservice.GetGoalsByFilters(goals, filterName, countFrom, countTo);
            }
            else
            {
                bool isDone = bool.Parse(filterDone);
                goals = T_GOALservice.GetGoalsByFilters(goals, filterName, countFrom, countTo, isDone);
            }

            int tasksCount = goals.Count();

            //sorting
            if (ViewState["sortexpression"] != null)
            {
                string a = ViewState["sortexpression"].ToString().ToUpper();
                SortDirection currentDirection = (SortDirection)Enum.Parse(typeof(SortDirection), ViewState["sortdirection"].ToString());
                switch (ViewState["sortexpression"].ToString().ToUpper())
                {
                    case "DONE":
                        goals = (currentDirection == SortDirection.Ascending) ? goals.OrderBy(n => n.Done) : goals.OrderByDescending(n => n.Done);
                        break;
                    case "GOALNAME":
                        goals = (currentDirection == SortDirection.Ascending) ? goals.OrderBy(n => n.GoalName) : goals.OrderByDescending(n => n.GoalName);
                        break;
                    case "TASKSCOUNT":
                        goals = (currentDirection == SortDirection.Ascending) ? goals.OrderBy(n => n.T_TASK.Count) : goals.OrderByDescending(n => n.T_TASK.Count);
                        break;
                    case "CREATEDATE":
                        goals = (currentDirection == SortDirection.Ascending) ? goals.OrderBy(n => n.CreateDate) : goals.OrderByDescending(n => n.CreateDate);
                        break;
                    default:
                        goals = goals.OrderBy(item => item.GoalID);
                        break;
                }
            }
            else
            {
                goals = goals.OrderBy(n => n.GoalID);
            }

            //page index
            int recCount = goals.Count();
            if ((GridView1.PageIndex * GridView1.PageSize) > recCount)
            {
                GridView1.PageIndex = recCount / GridView1.PageSize;
            }

            return goals;
        }

        private void BindData()
        {
            IQueryable<T_GOAL> goals = GetData();
            GridView1.DataSource = goals.Skip(GridView1.PageIndex * GridView1.PageSize).Take(GridView1.PageSize).ToList();
            GridView1.VirtualItemCount = goals.Count();
            GridView1.DataBind();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            MyDataModel context = new MyDataModel();
            string userName = Page.User.Identity.Name;
            T_ACCOUNT account = T_ACCOUNTservice.GetAccountByEmail(context, userName);
            T_GOAL newGoal = T_GOALservice.AddNewGoal(context, userName, account);
            Response.Redirect("~/_GoalPage.aspx?GoalID=" + newGoal.GoalID);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            MyDataModel context = new MyDataModel();
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox chkTask = (CheckBox)row.FindControl("chkGoal");

                if (chkTask.Checked)
                {
                    int goalID = int.Parse(((Label)row.FindControl("HiddenGoalID")).Text.Trim());
                    IQueryable<T_TASK> tasksForDelete = T_TASKservice.GetTasksByGoalID(context, goalID);
                    if (tasksForDelete.Any())
                    {
                        foreach (var task in tasksForDelete)
                        {
                            context.T_TASK.Remove(task);
                        }                       
                    }
                    T_GOAL goalForDelete = T_GOALservice.GetGoalByID(context, goalID);
                    context.T_GOAL.Remove(goalForDelete);
                    context.SaveChanges();
                }
            }

            BindData();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string goalNameText = e.Row.Cells[2].Text;

                if (goalNameText.Length > 40)
                {
                    string shortText1 = goalNameText.Substring(0, 40);
                    shortText1 += "...";
                    e.Row.Cells[2].Text = shortText1;
                }
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer'; this.style.fontWeight='bold'; ";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none',this.style.fontWeight=''; ";  
                              
                for (int i = 2; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Attributes["onclick"] = string.Format("window.open('_GoalPage.aspx?GoalID={0}','_self', 'width=600,height=400');", ((Label)e.Row.FindControl("HiddenGoalID")).Text.Trim());
                }
            }
        }

        protected void chkDone_CheckedChanged(object sender, EventArgs e)
        {
            MyDataModel context = new MyDataModel();
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)((checkbox).NamingContainer);
            int goalID = int.Parse(((Label)row.FindControl("HiddenGoalID")).Text.Trim());

            if (checkbox.Checked)
            {
                T_GOALservice.GetGoalByID(context, goalID).Done = true;
                foreach (var task in T_TASKservice.GetTasksByGoalID(context, goalID))
                {
                    task.Done = true;
                }
            }
            else
            {
                T_GOALservice.GetGoalByID(context, goalID).Done = false;
                foreach (var task in T_TASKservice.GetTasksByGoalID(context, goalID))
                {
                    task.Done = false;
                }
            }
            context.SaveChanges();

            BindData();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState["sortexpression"] = e.SortExpression;

            if (ViewState["sortdirection"] == null || ViewState["sortdirection"].ToString() == "Ascending")
            {
                ViewState["sortdirection"] = SortDirection.Descending;
            }
            else
            {
                ViewState["sortdirection"] = SortDirection.Ascending;
            }

            BindData();
        }

        protected void tbxFilterGoalName_TextChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void dlstFilterDone_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void tbxCountFrom_TextChanged(object sender, EventArgs e)
        {
            checkFieldValue(sender, e);
        }

        protected void tbxCountTo_TextChanged(object sender, EventArgs e)
        {
            checkFieldValue(sender, e);
        }

        private void checkFieldValue(object sender, EventArgs e) {
            Regex regx = new Regex(@"^(\s*|\d+)$");
            TextBox textBox = (TextBox)sender;

            if (regx.IsMatch(textBox.Text.Trim()))
            {
                textBox.BorderColor = Color.Empty;              
                BindData();
            }
            else
            {
                textBox.BorderColor = Color.Red;
            }
        }

        protected void btnClean_Click(object sender, EventArgs e)
        {
            tbxCountFrom.Text = string.Empty;
            tbxCountFrom.BorderColor = Color.Empty;
            tbxCountTo.Text = string.Empty;
            tbxCountFrom.BorderColor = Color.Empty;
            tbxFilterGoalName.Text = string.Empty;
            dlstFilterDone.SelectedIndex = 0;
            BindData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string fileName = "goal_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
            Response.Clear();
            Response.Buffer = true;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ";");
            Response.ContentType = "text/csv";
            Response.Charset = "utf-8";
            StreamWriter sw = new StreamWriter(Response.OutputStream);
            int pageIndex = GridView1.PageIndex;

            for (int i = 1; i < GridView1.Columns.Count - 1; i++)
            {
                sw.Write(GridView1.Columns[i] + ",");
            }
            sw.Write(sw.NewLine);

            for (int n = 0; n < GridView1.PageCount; n++)
            {
                GridView1.PageIndex = n;
                BindData();

                foreach (GridViewRow dr in GridView1.Rows)
                {
                    for (int i = 1; i < GridView1.Columns.Count - 1; i++)
                    {
                        string cellValue = dr.Cells[i].Text;
                        if (i == 1)
                        {
                            Control ctrl = dr.Cells[1].FindControl("chkDone");
                            sw.Write(((CheckBox)ctrl).Text + ',');
                        }
                        else if (i == 3) {
                            Control ctrl = dr.Cells[3].FindControl("lblCount");
                            sw.Write(((Label)ctrl).Text + ',');                         
                        }
                        else
                        {
                            cellValue = cellValue.Replace("\r\n", "\\r\\n ");
                            sw.Write(cellValue + ',');
                        }
                    }

                    sw.Write(sw.NewLine);
                }
            }
            sw.Close();
            Response.End();

            GridView1.PageIndex = pageIndex;
        }

        protected void chkAllGoals_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                foreach (GridViewRow row in GridView1.Rows)
                {
                    CheckBox chkTask = (CheckBox)row.FindControl("chkGoal");
                    chkTask.Checked = true;
                }
            }
            else
            {
                foreach (GridViewRow row in GridView1.Rows)
                {
                    CheckBox chkTask = (CheckBox)row.FindControl("chkGoal");
                    chkTask.Checked = false;
                }
            }

        }

        protected void dlstPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView1.PageSize = int.Parse(dlstPageSize.SelectedValue);
            GridView1.PageIndex = 0;
            BindData();
        }
    }
}