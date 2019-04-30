using System;
using System.Web.Security;
using System.Web.UI;
using TimeLink.Models;
using TimeLink.Services;

namespace TimeLink
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetDBData.GenerateDBData();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            MyDataModel context = new MyDataModel();

            string email = tbxEmail.Text;
            string password = tbxPassword.Text;

            T_ACCOUNT existingAccount = T_ACCOUNTservice.GetAccountByEmail(context,email);
            if (existingAccount == null)
            {
                lblError.Text = string.Format("Email '{0}' isn't registered.", email);
            }
            else if (existingAccount.Active == false)
            {
                lblError.Text = string.Format("Email '{0}' is blocked.", email);
            }
            else if (existingAccount.Password != password)
            {
                lblError.Text = "Invalid password.";
            }
            else
            {
                FormsAuthentication.SetAuthCookie(email, false);
                FormsAuthentication.RedirectFromLoginPage(email, false);
            }
        }
    }
}
