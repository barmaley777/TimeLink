using System;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using TimeLink.Constants;
using TimeLink.Models;
using TimeLink.Services;

namespace TimeLink
{
    public partial class _TaskPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MyDataModel context = new MyDataModel();
            int tasknameMaxLength = 50;
            int descriptionMaxLength = 50;
            Label1.Text = tbxDescription.Text.Length.ToString();
            tbxDescription.Attributes.Add("MaxLength", tbxDescription.MaxLength.ToString());
            string actualUserName = Page.User.Identity.Name;

            int goalID;
            if (!int.TryParse(Request.QueryString["GoalID"], out goalID))
            {
                tbxTaskName.MaxLength = tasknameMaxLength;
                tbxTaskName.ToolTip = string.Format("Max {0} characters", tasknameMaxLength);
                tbxDescription.ToolTip = string.Format("Max {0} characters", descriptionMaxLength);
                lblGoalName.Text = Messages.goalName + Messages.errorMissingGoalID;
                lblGoalName.ForeColor = Color.Red;
                return;
            }
            else
            {
                T_GOAL goalAbsentOrDeleted = T_GOALservice.GetGoalByID(context, goalID);
                if (goalAbsentOrDeleted == null)
                {
                    BadGoalID(Messages.goalName + " is absent!");
                    return;
                }
                else if (goalAbsentOrDeleted.T_ACCOUNT.Email != actualUserName)
                {
                    BadGoalID(Messages.goalName + " is incorrect!");
                    return;
                }
                else
                {
                    lblGoalName.ForeColor = Color.Empty;
                    btnSave.Enabled = true;
                    lblGoalName.Text = Messages.goalName + goalAbsentOrDeleted.GoalName;
                }
            }

            if (!IsPostBack)
            {
                int taskID;

                if (int.TryParse(Request.QueryString["TaskID"], out taskID))
                {
                    if (!T_TASKservice.IsTaskExists(context, taskID))
                    {
                        return;
                    }
                    T_TASK task = T_TASKservice.GetTaskByID(context, taskID);
                    tbxTaskName.Text = task.TaskName;
                    tbxDescription.Text = task.Description;
                    Label1.Text = task.Description.Count().ToString();
                    dlstDone.SelectedValue = task.Done.ToString().ToLower();
                }
                else
                {
                    Label1.Text = tbxDescription.Text.Length.ToString();
                }
            }
        }

        private void BadGoalID(string errorMessage)
        {
            lblGoalName.Text = errorMessage;
            lblGoalName.ForeColor = Color.Red;
            btnSave.Enabled = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            MyDataModel context = new MyDataModel();
            int goalID;

            if (!int.TryParse(Request.QueryString["GoalID"], out goalID))
            {
                lblGoalName.Text = Messages.goalName + Messages.errorMissingGoalID;
                lblGoalName.ForeColor = Color.Red;
                return;
            }

            string taskName = tbxTaskName.Text;
            if (string.IsNullOrEmpty(taskName))
            {
                taskName = "noname";
            }
            string description = tbxDescription.Text;
            bool isDone = bool.Parse(dlstDone.SelectedItem.Value);

            T_GOAL goal = T_GOALservice.GetGoalByID(context, goalID);
            if (goal == null)
            {
                lblGoalName.Text = Messages.goalName + Messages.errorIncorrectGoalID;
                lblGoalName.ForeColor = Color.Red;
                return;
            }

            int taskID;
            //create(if TaskID absent) or update task
            if (int.TryParse(Request.QueryString["TaskID"], out taskID))
            {
                if (!T_TASKservice.IsTaskExists(context, taskID))
                {
                    return;
                }
                T_TASK task = T_TASKservice.GetTaskByID(context, taskID);
                task.GoalID = goalID;
                task.TaskName = taskName;
                task.Description = description;
                task.Done = isDone;
                task.UpdateDate = DateTime.Now;
            }
            else
            {
                T_TASK newTask = new T_TASK { GoalID = goalID, TaskName = taskName, Description = description, Done = isDone, UpdateDate = DateTime.Now, T_GOAL = goal };
                context.T_TASK.Add(newTask);
            }
            context.SaveChanges();
            Response.Redirect("~/_GoalPage.aspx?GoalID=" + goalID);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string goalsPage = "Goals Page";
            int goalID;
            if (btnCancel.Text == goalsPage)
            {
                Response.Redirect("~/_GoalsPage.aspx");
            }
            else if (!int.TryParse(Request.QueryString["GoalID"], out goalID))
            {
                lblGoalName.Text = Messages.goalName + Messages.errorMissingGoalID;
                lblGoalName.ForeColor = Color.Red;
                btnCancel.Text = goalsPage;
                btnSave.Enabled = false;
            }
            else
            {
                Response.Redirect("~/_GoalPage.aspx?GoalID=" + goalID);
            }
        }
    }
}