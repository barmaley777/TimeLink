using System.Linq;
using TimeLink.Models;

namespace TimeLink.Services
{
    public static class T_ACCOUNTservice
    {
        public static T_ACCOUNT GetAccountByEmail(MyDataModel db, string email) {
            return db.T_ACCOUNT.FirstOrDefault(n => n.Email == email);
        }
    }
}