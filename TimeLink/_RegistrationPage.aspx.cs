using System;
using System.Drawing;
using TimeLink.Constants;
using TimeLink.Models;
using TimeLink.Services;

namespace TimeLink
{
    public partial class _RegistrationPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            MyDataModel context = new MyDataModel();
            tbxEmail.BorderColor = Color.Empty;

            string email = tbxEmail.Text.Trim();
            string password = tbxPassword.Text.Trim();

            if (T_ACCOUNTservice.GetAccountByEmail(context, email) != null)
            {
                lblConfirmation.Text = Messages.errorMailExists;
                lblConfirmation.Visible = true;
                tbxEmail.BorderColor = Color.Red;
            }
            else
            {
                string name = tbxFirstName.Text;
                string surname = tbxSecondName.Text;
                bool isMale = dlstGender.SelectedItem.Text == "Male" ? true : false;
                T_ACCOUNT newAccount = new T_ACCOUNT() { Email = email, Password = password, Name = name, Surname = surname, IsMale = isMale, Active = true };
                context.T_ACCOUNT.Add(newAccount);
                context.SaveChanges();
                lblConfirmation.Text = string.Format("email '{0}' registered successfully", email);
                lblConfirmation.Visible = true;
                btnConfirm.Enabled = false;
            }
        }

        protected void btnButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
    }
}