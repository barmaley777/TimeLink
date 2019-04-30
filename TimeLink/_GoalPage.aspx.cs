using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using TimeLink.Constants;
using TimeLink.Models;
using TimeLink.Services;

namespace TimeLink
{
    public partial class _GoalPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
            }

            MyDataModel context = new MyDataModel();

            int goalID = 0;
            bool isGetGoalID = int.TryParse(Request.QueryString["GoalID"], out goalID);
            lblHiddenGoalID.Text = goalID.ToString();

            if (!IsPostBack)
            {
                if (isGetGoalID && goalID != 0)
                {
                    tbxGoalName.Visible = true;
                    T_GOAL currentGoal = T_GOALservice.GetGoalByID(context, goalID);
                    string actualUserName = Page.User.Identity.Name;

                    if (currentGoal == null)
                    {
                        BadGoalID(Messages.errorMissingGoalID);
                        return;
                    }
                    else if (currentGoal.T_ACCOUNT.Email != actualUserName)
                    {
                        BadGoalID(Messages.errorIncorrectGoalID);
                        return;
                    }
                    else
                    {
                        tbxGoalName.Text = currentGoal.GoalName;
                    }
                }
                else
                {
                    BadGoalID(Messages.errorMissingGoalID);
                }
                BindData();

            }
        }

        private void BadGoalID(string errorMessage)
        {
            tbxGoalName.Visible = false;
            lblGoalName.Text = errorMessage;
            lblGoalName.ForeColor = Color.Red;
            GridView1.DataSource = Enumerable.Empty<T_TASK>();
            GridView1.DataBind();
        }

        private IQueryable<T_TASK> GetData(string sortExpression = "", SortDirection sortDirection = SortDirection.Ascending)
        {
            MyDataModel context = new MyDataModel();

            GridView1.PageSize = int.Parse(dlstPageSize.SelectedValue);
            IQueryable<T_TASK> tasks = null;

            int goalID = 0;
            int.TryParse(Request.QueryString["GoalID"], out goalID);
            tasks = context.T_TASK.Where(n => n.GoalID == goalID);

            string filterName = tbxFilterName.Text;
            string filterDone = dlstFilterDone.SelectedValue;

            //filters
            if (string.IsNullOrEmpty(filterDone))
            {
                tasks = T_TASKservice.GetTasksByFilters(tasks, filterName);

            }
            else
            {
                bool isDone = bool.Parse(filterDone);
                tasks = T_TASKservice.GetTasksByFilters(tasks, filterName, isDone);
            }

            int tasksCount = tasks.Count();

            //sorting
            if (ViewState["sortexpression"] != null)
            {
                SortDirection currentDirection = (SortDirection)Enum.Parse(typeof(SortDirection), ViewState["sortdirection"].ToString());
                switch (ViewState["sortexpression"].ToString().ToUpper())
                {
                    case "TASKID":
                        tasks = (currentDirection == SortDirection.Ascending) ? tasks.OrderBy(n => n.TaskID) : tasks.OrderByDescending(n => n.TaskID);
                        break;
                    case "DONE":
                        tasks = (currentDirection == SortDirection.Ascending) ? tasks.OrderBy(n => n.Done) : tasks.OrderByDescending(n => n.Done);
                        break;
                    case "TASKNAME":
                        tasks = (currentDirection == SortDirection.Ascending) ? tasks.OrderBy(n => n.TaskName) : tasks.OrderByDescending(n => n.TaskName);
                        break;
                    case "DESCRIPTION":
                        tasks = (currentDirection == SortDirection.Ascending) ? tasks.OrderBy(n => n.Description) : tasks.OrderByDescending(n => n.Description);
                        break;
                    case "UPDATEDATE":
                        tasks = (currentDirection == SortDirection.Ascending) ? tasks.OrderBy(n => n.UpdateDate) : tasks.OrderByDescending(n => n.UpdateDate);
                        break;
                    default:
                        tasks = tasks.OrderBy(item => item.TaskID);
                        break;
                }
            }
            else
            {
                tasks = tasks.OrderBy(n => n.TaskID);
            }

            //page index
            int recCount = tasks.Count();
            if ((GridView1.PageIndex * GridView1.PageSize) > recCount)
            {
                GridView1.PageIndex = recCount / GridView1.PageSize;
            }

            return tasks;
        }

        private void BindData()
        {
            IQueryable<T_TASK> tasks = GetData();
            GridView1.DataSource = tasks.Skip(GridView1.PageIndex * GridView1.PageSize).Take(GridView1.PageSize).ToList();
            GridView1.VirtualItemCount = tasks.Count();
            GridView1.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/_TaskPage.aspx?GoalID=" + lblHiddenGoalID.Text.Trim());
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            MyDataModel context = new MyDataModel();
            foreach (GridViewRow row in GridView1.Rows)
            {
                //Searching CheckBox("chkDel") in an individual row of Grid  
                CheckBox chkTask = (CheckBox)row.FindControl("chkTask");
                //If CheckBox is checked than delete the record with particular empid  
                if (chkTask.Checked)
                {
                    int taskID = int.Parse(((Label)row.FindControl("HiddenTaskID")).Text.Trim());
                    T_TASKservice.RemoveTaskByID(context, taskID);
                }
            }

            BindData();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            MyDataModel context = new MyDataModel();
            int goalID = int.Parse(lblHiddenGoalID.Text.Trim());
            IQueryable<T_TASK> tasks = T_TASKservice.GetTasksByGoalID(context, goalID);
            T_TASK isFalseDone = T_TASKservice.GetIncompliteTasks(tasks, false).FirstOrDefault(); //created goal have null tasks and t.Done always null
            T_GOAL currentGoal = T_GOALservice.GetGoalByID(context, goalID);
            if (currentGoal != null)
            {
                if (isFalseDone != null || tasks.Count() == 0)
                {
                    currentGoal.Done = false;
                }
                else
                {
                    currentGoal.Done = true;
                }
                context.SaveChanges();
            }

            Response.Redirect("~/_GoalsPage.aspx");
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
            MyDataModel context = new MyDataModel();

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
                        cellValue = cellValue.Replace("\r\n", "\\r\\n ");

                        if (i == 1)
                        {
                            Control ctrl = dr.Cells[1].FindControl("chkDone");
                            sw.Write(((CheckBox)ctrl).Text + ',');
                        }
                        else if (i == 2)
                        {
                            Control ctrl = dr.FindControl("HiddenTaskID");
                            int taskID = int.Parse(((Label)ctrl).Text);
                            string taskName = T_TASKservice.GetTaskByID(context, taskID).TaskName;
                            sw.Write(taskName + ',');
                        }
                        else
                        {
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

        protected void dlstPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView1.PageSize = int.Parse(dlstPageSize.SelectedValue);
            GridView1.PageIndex = 0;
            BindData();
        }

        protected void dlstFilterDone_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void tbxFilterName_TextChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string taskNameText = e.Row.Cells[2].Text;
                if (taskNameText.Length > 15)
                {
                    string shortText1 = taskNameText.Substring(0, 15);
                    shortText1 += "...";
                    e.Row.Cells[2].Text = shortText1;
                }

                string descriptionText = e.Row.Cells[3].Text;
                if (descriptionText.Length > 30)
                {
                    string shortText2 = descriptionText.Substring(0, 30);
                    shortText2 += "...";
                    e.Row.Cells[3].Text = shortText2;
                }

                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer'; this.style.fontWeight='bold'; ";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none',this.style.fontWeight=''; ";

                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Attributes["onclick"] = string.Format("window.open('_TaskPage.aspx?GoalID={0}&TaskID={1}','_self', 'width=600,height=400');", lblHiddenGoalID.Text.Trim(), ((Label)e.Row.FindControl("HiddenTaskID")).Text.Trim());
                }
            }
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

        protected void btnClean_Click(object sender, EventArgs e)
        {
            tbxFilterName.Text = "";
            dlstFilterDone.SelectedIndex = 0;
            BindData();
        }

        protected void chkAllTasks_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                foreach (GridViewRow row in GridView1.Rows)
                {
                    CheckBox chkTask = (CheckBox)row.FindControl("chkTask");
                    chkTask.Checked = true;
                }
            }
            else
            {
                foreach (GridViewRow row in GridView1.Rows)
                {
                    CheckBox chkTask = (CheckBox)row.FindControl("chkTask");
                    chkTask.Checked = false;
                }
            }
        }

        protected void tbxGoalName_TextChanged(object sender, EventArgs e)
        {
            int goalID = int.Parse(Request.QueryString["GoalID"]);
            MyDataModel context = new MyDataModel();
            string newGoalName = tbxGoalName.Text;
            if (string.IsNullOrEmpty(newGoalName))
            {
                newGoalName = "noname";
            }
            T_GOALservice.GetGoalByID(context, goalID).GoalName = newGoalName;
            context.SaveChanges();
        }

        protected void chkDone_CheckedChanged(object sender, EventArgs e)
        {
            MyDataModel context = new MyDataModel();
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)((checkbox).NamingContainer);

            int taskID = int.Parse(((Label)row.FindControl("HiddenTaskID")).Text.Trim());

            if (checkbox.Checked)
            {
                T_TASKservice.GetTaskByID(context, taskID).Done = true;
            }
            else
            {
                T_TASKservice.GetTaskByID(context, taskID).Done = false;
            }
            context.SaveChanges();
            BindData();
        }
    }
}